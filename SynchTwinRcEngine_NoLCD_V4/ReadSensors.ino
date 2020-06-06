/* 
  Le capteur IR GP2Y0D810Z0F est connecté comme suit:
    * BOARD -> ARDUINO
    * Vcc   -> 5V
    * GND   -> GND
    * OUT   -> PIN 2 and 3
  Le capteur Effet Hall US5881LUA est connecté comme suit:
     Pin 1: 5v
     Pin 2: GND
     Pin 3: +5V via une résistance pull-up de 10 KOhms MAIS AUSSI sur la PIN 6 et 7 d'Arduino 
    (les TLE 4905 et UGN 3140 ( faciles à trouver et équivalents entr'eux ) sont 3 à 4 fois plus sensibles que le 5881)
    un bon aimant NdFeBo (N42,N52)...
	
  Pour un moteur brushless on pourra utiliser ce type de montage:
  http://openrcforums.com/forum/posting.php?mode=quote&f=95&p=61185&sid=95230e0106f4ab92d7a5d9ea5117509b
  oue celui-ci:
  http://www.bhabbott.net.nz/wattmeter.html
*/

/* Function called in interruption in case of change on pins */
void InterruptFunctionToCall(void)
{
  if(TINY_PIN_CHANGE(VirtualPortNb, BROCHE_SENSOR1)) /* Check BROCHE_SENSOR1 has changed (rising edge) See Macros.h */
  {
      FirstInputChangeCount++; /* Rising edges are counted */
#ifdef RMPOUTPUT      
      flip(RPMOUT1);
#endif
  }

  if(TINY_PIN_CHANGE(VirtualPortNb_, BROCHE_SENSOR2)) /* Check BROCHE_SENSOR2 has changed (rising edge) See Macros.h */
  {
      SecondInputChangeCount++; /* Rising edges are counted */
#ifdef RMPOUTPUT      
      flip(RPMOUT2);
#endif      
  }
}

void readCaptorTransitions()
{  
  /* Display Transition numbers every second */
  if((millis()-ReadCaptorsMs >= 1000))
  {
    noInterrupts(); /* Mandatory since counters are 16 bits */
    ReadCaptorsMs=millis();
    if (simulateSpeed == false)
    {
      vitesse1=0;vitesse2=0;
      readings_V1[index] = FirstInputChangeCount * 60;  /* Convert frecuency to RPM, note: this works for one interruption per full rotation. For two interrups per full rotation use rpmcount * 30.*/
      readings_V2[index] = SecondInputChangeCount * 60;  /* Convert frecuency to RPM, note: this works for one interruption per full rotation. For two interrups per full rotation use rpmcount * 30.*/
      while (index <= 4)
      {
        vitesse1 = vitesse1 + readings_V1[index];
        vitesse2 = vitesse2 + readings_V2[index];
        FirstInputChangeCount = 0;SecondInputChangeCount = 0; //Set NbTops to 0 ready for calculations 
        index++;          
      }
        vitesse1 = vitesse1 / 5;
        vitesse2 = vitesse2 / 5;
        vitesse1 = (vitesse1 > 0)?vitesse1/ms.nbPales:0;// preventing 0/nbPales
        vitesse2 = (vitesse2 > 0)?vitesse2/ms.nbPales:0;
     }
    interrupts();
  }

#ifdef EXTLED
  if (vitesse1 !=0) {on(LED1GREEN);off(LED1GREEN);}
  if (vitesse2 !=0) {on(LED2GREEN);off(LED2GREEN);}
#endif

 
}
