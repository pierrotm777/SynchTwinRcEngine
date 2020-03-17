#ifdef RECORDER
/* 
  Recorder save throttle movements into the eeprom and play the sequence saved
*/
typedef enum {PT, REC, PLAY} MODE; // PT = PassThrough, ie follow POT or Joystick
MODE mode = PT;
uint8_t stateChange;// = 0; // Play/Rec every other sample, interpolate between
uint8_t ind, ang;//, pot;

#define MAX_LEN 100
typedef struct conf
{
  short len;
//  uint16_t minPot;
//  uint16_t maxPot;
  byte minServo;
  byte maxServo;
  byte d[MAX_LEN];
} Conf;

Conf conf;

void setupRecorder()
{
  byte *p = (byte*)&conf;
  for (uint8_t i = 40; i < sizeof(Conf); i++) // reading is fast so do the lot, could just read the data
  {
    byte b = EEPROM.read(i);
    *(p++) = b;
//    Serial << i << " val " << b << endl;
  }

  conf.len = constrain(conf.len, 40, MAX_LEN -1);
  conf.minServo = constrain(conf.minServo, 10, 170);
  conf.maxServo = constrain(conf.maxServo, 10, 170);

  flash3();
}

void loopRecorder()
{
//  if (RxChannelPulseMotor.available())//PLACé DANS LOOP PRINCIPAL
//  {
//    AVERAGE(Width_us,RxChannelPulseMotor.width_us(), responseTime);
    ang = map(Width_us,minimumPulse_US, maximumPulse_US,1,179);
//  }

#ifdef LCDON 
  if(LCDML_BUTTON_checkAny()) { // check if any button is presed (enter, up, down, left, right)
    if (LCDML_BUTTON_checkEnter()) // check if button 'enter' is pressed one time
    {
      LCDML_BUTTON_resetEnter();
      lcd.setCursor(0,0);
      switch(LCDML.getCursorPos())
      {
        case 0:
          if(mode == PT) mode = PLAY;
          else if(mode == PLAY) mode = PT;
          stateChange = 0;
          ind = 0;
          lcd.print(F("Play..."));
          break;          
        case 1:
          if(mode == PT) {mode = REC;}
          else if(mode == REC)
          {
            mode = PT;
            conf.len--;
          }
          stateChange = 0;
          ind = 0;
          lcd.print(F("Record..."));
          break;
      }     
    }
    if (LCDML_BUTTON_checkDown())// check if button 'down' is pressed one time
    {
      LCDML_BUTTON_resetDown();
      lcd.clear();
//      lcd.setCursor(2,0);lcdPrint("!!  Recorder  !!");
      lcd.setCursor(6,2);lcd.print(F(" Abort !!!"));
      PIN_LOW(B,0);PIN_LOW(B,1);
      mode = PT;
      waitMs(2000UL);
      LCDML.jumpToElement(16);/* return to Play/Record menu */
      
    }
  }

  if (mode == PT)
  {
    lcd.setCursor(0,1);lcd.print(F("Servo "));lcd.print(ang);lcd.print(F(" "));
  } 
  if (mode != PT)
  {
    lcd.setCursor(0,1);lcd.print(ind);lcd.print(stateChange);lcd.print(F(" "));lcd.print(ang);lcd.print(F(" "));
  } 
#endif

  if (mode == PLAY)
  {
    if (stateChange)
    {
      ang = conf.d[ind] + conf.d[ind++ +1]; // WOW C is not Java ?
      ang /= 2;
    }
    else
    {
      ang = conf.d[ind];
    }
#ifdef LCDON
    lcd.setCursor(0,0);swapTwoText("Playing...","            ",500);
#endif
  }

  if (mode == REC)
  {
    ServoMotor1.write(ang); // Lets the Servo class do any uS to Angle conversion :-)
//    SoftRcPulseOut::refresh(1); 
    ang = ServoMotor1.read(); // Get it back into Degrees
    if (!stateChange) {conf.d[ind++] = ang;}
    conf.len = ind;
#ifdef LCDON
    lcd.setCursor(0,0);swapTwoText("Recording...","            ",500);
#endif
  }
#ifdef LCDON
  switch(mode)
  {
    case 0:lcd.setCursor(16,0);lcd.print(F("PT  "));break;          
    case 1:lcd.setCursor(16,0);lcd.print(F("REC "));break; 
    case 2:lcd.setCursor(16,0);lcd.print(F("PLAY"));break; 
  } 
#endif  
  if (ind >= conf.len && mode == PLAY || ind >= MAX_LEN)
  {

    PIN_LOW(B,0);//(LED3, LOW); // Flash on reset to show loop during playback
    PIN_HIGH(B,1);//(LED4, HIGH);

    ind = 0; // go back to bagining
    stateChange = 1; // will get reset below
    if (mode == REC)
    {
      mode = PT; // stop recording
      conf.len--; // dont want n+ stateChange
    }
#ifdef LCDON
    lcd.setCursor(0,0);lcd.print(F("Ready...   "));
#endif
  }

  ServoMotor1.write(ang);
  SoftRcPulseOut::refresh(1);

  stateChange ^= 1; // classic XOR stateChange
//  delay(40); // give the LEDs some time to flash

  if(mode == REC){PIN_HIGH(B,0);}else{PIN_LOW(B,0);}
  if(mode == PLAY){PIN_HIGH(B,1);}else{PIN_LOW(B,1);}

}

#ifdef LCDON
void swapTwoText(String text1, String text2 , uint16_t ms)
{
  static uint8_t blinkPos;
  if(millis() - BeginChronoLcdMs >= ms)//lets not saturate the loop() and makes the movement of servos more fluid
  {
    switch(blinkPos)
    {
      case 0:lcd.print(text1);break;
      case 1:lcd.print(text2);break;
    }  
    (blinkPos <= 1)?blinkPos++:blinkPos=0;       
    BeginChronoLcdMs=millis(); /* Restart the Chrono for the LCD */
  }
}
#endif//end LCDON

//boolean pressed(byte but)
//{
//  if (digitalRead(but))
//  {
//    while (digitalRead(but)) delay(20);
//    return true;
//  }
//  return false;
//}

//boolean pressedOnly(byte but)
//{
//  if (digitalRead(but)) // Button Pressed ? (_LCDML_CONTROL_digital_down/Play)
//  {
//    while (digitalRead(but)) // Wait for it to be released
//    {
//      delay(20);
//      if (pressed(_LCDML_CONTROL_digital_enter)) // Are we Pressing this one too ?
//      {
//        while (digitalRead(but)) delay(20); // Wait for release, Actually we don't need this test its done in pressed()
//        saveConfig();
//        return false; // Don't do the Set/Play we did a save instead
//      }
//    }
//    return true; // Pressed DO it
//  }
//  return false; // Never Pressed
//}

//void waitBut(byte led, byte flashSpeed, boolean driveServo)
//{
//  byte pressed = 0;
//  byte ledState = 0;
//  long ledScheduled = millis();
//  long servoScheduled = ledScheduled;
//
//  while(true)
//  {
//    if (millis() >= servoScheduled && driveServo)
//    {
//      pot = constrain(analogRead(POTPIN), conf.minPot, conf.maxPot);
//      ServoMotor1.write(map(pot, conf.minPot, conf.maxPot, 10, 170));
//      servoScheduled += 40;
//    }
//
//    if (millis() >= ledScheduled)
//    {
//      ledScheduled += 100 * flashSpeed;
////      digitalWrite(led, ledState ^= 1);
//      if (led==5)LED5.stateChange();
//      if (led==6)LED6.stateChange();
//    }
//    pressed |= !digitalRead(_LCDML_CONTROL_digital_down);
//
//    if (pressed && digitalRead(_LCDML_CONTROL_digital_down)) break;
//  }
//  if (led==5)LED5.turnOff();//digitalWrite(led, LOW);
//  if (led==6)LED6.turnOff();
//  delay(50); // Debounce
//}

//void calibratePot() // and Servo
//{
//  LED5.turnOn();//digitalWrite(RLED, HIGH);
//  conf.len = 0; // Nothing Recorded
//  waitBut(5, 1, false);
//  conf.minPot = analogRead(POTPIN);
//  waitBut(6, 1, false);
//  conf.maxPot = analogRead(POTPIN);
////  Serial <<"calibratePot OK" << endl;
//  calibrateServo();
//}
//
//void calibrateServo() // Only
//{
//  conf.minServo = 10;
//  conf.maxServo = 170;
//  LED5.turnOn();//digitalWrite(RLED, HIGH);
//  while (!digitalRead(_LCDML_CONTROL_digital_down)) delay(40); // Ensure Set released
//  waitBut(5, 2, true);
//  pot = constrain(analogRead(POTPIN), conf.minPot, conf.maxPot);
//  conf.minServo = map(pot, conf.minPot, conf.maxPot, 10, 170);
////  Serial << F("Pot v ") << pot << F(" servo v ") << conf.minServo << endl;
//  waitBut(6, 2, true);
//  pot = constrain(analogRead(POTPIN), conf.minPot, conf.maxPot);
//  conf.maxServo = map(pot, conf.minPot, conf.maxPot, 10, 170);
////  Serial << F("Pot ^ ") << pot << F(" servo ^ ") << conf.maxServo << endl;
//
//  saveConfig();
//}

void flash3()
{
  for (uint8_t b = 0; b < 3; b++)
  {
    PIN_HIGH(B,0);//(LED3, HIGH);
    PIN_HIGH(B,1);//(LED4, HIGH);
    waitMs(100);
    PIN_LOW(B,0);//(LED3, LOW);
    PIN_LOW(B,1);//(LED4, LOW);
    waitMs(100);
  }

}

void saveConfig()
{
  uint8_t *p = (uint8_t*)&conf;
  uint16_t sz = sizeof(Conf) + conf.len - MAX_LEN + 1; // Only write recorded data
  for (uint8_t i = 40; i < sz; i++)
  {
    EEPROM.write(i, *p++);
  }

  flash3();
}

#endif//end RECORDER
