/*
 ON-BOARD GLOW DRIVER - for nine cylinder radial
  Phil Wilson 2013
 ----------------------------------------------------------------------------------------
 Run on an Arduino pro-mini, Uno or similar
 PWM is set-up on pin 3 using timer2
 Each glowplug is assigned it's own output pin which switches it's glow plug via a MOSFET
 Each time there is a pulse, one of the glow plug output pins is turned on for the duration of the pulse
 The pulse length is varied to determine the brightness of the glow
 The glow plugs are switched in sequence so only one glow is on at a time
 Hence the glow plugs are switched at 1/9th of the PWM frequency
 Using a 3 cell Lipo battery (11.1v) to power the glowplugs you only need about 4-5% duty cycle per 
   plug for full glowing - ie. approx 40-50% of the PWM divided by 9
 Pin 8 senses the receiver input to control the glow brightness
 So the transmitter can set full brightness for low throttle settings/start-up, and less at high throttle
 If there is no receiver input, the glowplug brightness is determined by a trim pot
 */

 /*
 ToDO:
 watchdog timer
 Lipo voltage sensor
 */


 //declare constants and variables
 //--------------------------------------------------------------------------------------

 //arduino pins for glowplug outputs
 const int PLUG_PINS[9] = {2,4,5,6,7,9,10,11,12};  

 const int THROTTLE_PIN = 8;   //connect reciever throttle channel (well, actually the channel mixed from throttle) to arduino pin 8
          //standard receiver pulses are approx 1-2msec long
 const int MIN_PULSE_LENGTH = 800; //shortest length of throttle pulse in usec
 const int MAX_PULSE_LENGTH = 2100; //longest value for throttle pulse in usec

 const int LED_PIN = 13;    //the arduino board has an LED on pin 13. I'm using it to glow in proportion the glow plug brightness to 
           // give  a visual indication of the glow power
 const int PWM_PIN = 3;    //I'm going to set up PWM on pin 3 with timer2

 //trim pot is used to set glow brightness when no receiver connected
 const int TRIMPOT_PIN = 3;   //analog pin 3

 volatile int plugNumber = 0;  //counter to increment the GlowPlugs in the interrupt routine


 //pointers to the AVR ports for glowplug output pins
 //this is so the ports can be accessed in an array - interesting to have done but didn't gain much optimization
 volatile uint8_t* plugPort[9] = {&PORTB, &PORTB, &PORTB, &PORTB, &PORTD, &PORTD, &PORTD, &PORTD, &PORTD}; 

 //AVR port bit numbers for the arduino GlowPlug pins
 const int plugPortBit[9] = {1,2,3,4,2,4,5,6,7};  

void setup()
{
 //Serial.begin(9600);    //just for testing

    pinMode(THROTTLE_PIN, INPUT);
 pinMode(LED_PIN, OUTPUT);
 pinMode(PWM_PIN, OUTPUT);   //doesn't need to be set as output to work the PMW unless you want to actually use the output
           //but it is quite useful for testing
  
 // Configure pins for glow plug outputs
 // need pin 8 for input capture - in case I need it if pulseIn is too slow
 // pin 3 is the PWM pin so can't be used as a glowplug output
 for (int i = 0; i < 9; i++)  
 {
  pinMode(PLUG_PINS[i], OUTPUT); 
 }

 /* this just saves about 50bytes of code space
 //AVR PORTB 
 bitSet(DDRB, 1);//arduino pin 9
 bitSet(DDRB, 2);//arduino pin 10
 bitSet(DDRB, 3);//arduino pin 11
 bitSet(DDRB, 4);//arduino pin 12
 //AVR PORTD
 bitSet(DDRD, 2);//arduino pin 2
 bitSet(DDRD, 4);//arduino pin 4
 bitSet(DDRD, 5);//arduino pin 5
 bitSet(DDRD, 6);//arduino pin 6
 bitSet(DDRD, 7);//arduino pin 7
 */ 
  

 // Set up Timer 2 to do pulse width modulation on pin3
 //------------------------------------------------------------------------------------------------------------

 // Set fast PWM mode with TOP = 0xff: WGM22:0 = 3  (p.150)
 TCCR2A |= _BV(WGM21) | _BV(WGM20);
 TCCR2B &= ~_BV(WGM22);

 // Do non-inverting PWM on pin OC2B (arduino pin 3) (p.159).
 // OC2A (arduino pin 11) stays in normal port operation:
 // COM2B1=1, COM2B0=0, COM2A1=0, COM2A0=0
 TCCR2A = (TCCR2A | _BV(COM2B1)) & ~(_BV(COM2B0) | _BV(COM2A1) | _BV(COM2A0));


 //prescalar sets the frequency of the counter (p.162 AVR data sheet)
 //------------------------------------------------------------
 // No prescaler (p.162) - clear the last 3 bits (CS22:0) in TCCR2B:
 //TCCR2B = (TCCR2B & ~(_BV(CS22) | _BV(CS21))) | _BV(CS20);

 //another way, this time set all CS bits, for lowest prescalar speed:
 //TCCR2B = (TCCR2B & B11111000) | B00000111;
  
 TCCR2B = (TCCR2B & B11111000) | B00000011; //divisor 32. so approx 2kHz for pin3 PWM. gives 216Hz on each plug 

    /*
 - i think these frequencies are a factor of 2 out, so divisor 1 is 62.5kHz = 16000000/256 (8bit counter). checked on 'scope:

 Setting  Divisor  Frequency
 0x01   1    31250
 0x02   8   3906.25
 0x03    32    976.5625
 0x04   64   488.28125
 0x05   128    244.140625
 0x06    256    122.0703125
 0x07   1024    30.517578125

 TCCR2B = TCCR2B & 0b11111000 | <setting>;

 All frequencies are in Hz and assume a 16000000 Hz system clock. 
    */

 //---------------------------------------------------------prescalar set

 // Enable interrupt when TCNT2 reaches TOP (0xFF) (p.151, 163)
 TIMSK2 |= _BV(TOIE2);

 // Enable interrupt on OCR2B compare match.
 TIMSK2 |= _BV(OCIE2B);

 //set PWM duty cycle 
 OCR2B = 0; //0 to 255 for PWM duty cycle

 //--------------------------------------------------------------------timer2 all set up

}



void loop()
{
 
 //read throttle pulse from receiver
 //pulseIn is a bit crappy really, but it works ok here
 //if it offends you then set-up input capture (throttle is on pin 8 for that) or a pin change interrupt
 unsigned long throttlePulseLength = pulseIn(THROTTLE_PIN, HIGH);  //returns zero if no receiver input, which leads to no glowplus on

 if (throttlePulseLength == 0)  //no receiver input - jumpered to run off the glow battery for running engine on test bed
  throttlePulseLength = readTrimPot();

 //check it is not outside range because 'map' does not check it
 throttlePulseLength = constrain(throttlePulseLength, MIN_PULSE_LENGTH, MAX_PULSE_LENGTH);

 //for testing....
 ////////////////////////////////////////////////////////
 //throttlePulseLength = 1000;
 /////////////////////////////////////////////////////////


 // map the throttle pulse length to 8bits for the timer compare register
 OCR2B = map(throttlePulseLength, MIN_PULSE_LENGTH, MAX_PULSE_LENGTH, 0, 255); 

 /* for testing
 Serial.print(throttlePulseLength);
 Serial.print("    ");
 Serial.println(OCR2B);
 */
}

unsigned long readTrimPot()
{
 unsigned long trimPotValue = analogRead(TRIMPOT_PIN);
 trimPotValue = map(trimPotValue, 0, 1023, MIN_PULSE_LENGTH, MAX_PULSE_LENGTH);
 return trimPotValue;
 
}

// Service routine for TIMER2's overflow interrupt - this turns GLOWPLUGS ON.
ISR(TIMER2_OVF_vect) {
 
 if (OCR2B > 0)   //no need to turn on glowplugs if OCR2B is zero
 {

  bitSet(PORTB, 5); //13 - LED pin to show amount of glow. Much quicker than digitalWrite here
 
  //set glow plug pins high in sequence - each one will be on every 9th tick, so will have a 9th of the PMW duty %
  bitSet(*plugPort[plugNumber], plugPortBit[plugNumber]); //this fancy way didn't save much time 0.5usec
  //digitalWrite(PLUG_PINS[plugNumber],HIGH);   // 7usec
  
  //next time around do the next plug in seqeunce
  plugNumber++;
  if (plugNumber > 8) plugNumber = 0;

 }

}

// interrupt service routine for timer 2 compare match on OCR2B - this turns GLOWPLUGS OFF.
ISR(TIMER2_COMPB_vect) {
 
 //clear all glow plug outputs to turn them all off, as well as LED pin
 //direct port access is much quicker here rather than 10 digitalWrite statements
 PORTB &= ~((1<<1)|(1<<2)|(1<<3)|(1<<4)|(1<<5));
 PORTD &= ~((1<<2)|(1<<4)|(1<<5)|(1<<6)|(1<<7));


}
