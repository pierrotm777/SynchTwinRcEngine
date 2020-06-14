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
  ou celui-ci:
  http://www.bhabbott.net.nz/wattmeter.html
*/

/* Function called in interruption in case of change on pins */
void InterruptFunctionToCall1(void)
{
  FirstInputChangeCount++; /* Rising edges are counted */
}
void InterruptFunctionToCall2(void)
{
  SecondInputChangeCount++; /* Rising edges are counted */
}

void readCaptorTransitions()
{  
  /* Display Transition numbers every second */
  if((millis()-ReadCaptorsMs >= 1000))
  {
    ReadCaptorsMs=millis();     
    if (simulateSpeed == false)
    {
       vitesse1 = FirstInputChangeCount/2*60;
       vitesse2 = SecondInputChangeCount/2*60;
       FirstInputChangeCount = 0;
       SecondInputChangeCount = 0;
     }
  }

#ifdef EXTLED
  if (vitesse1 !=0) {on(LED1GREEN);off(LED1GREEN);}
  if (vitesse2 !=0) {on(LED2GREEN);off(LED2GREEN);}
#endif

#ifdef DEBUG
      SettingsPort << _DEC(getin(RPMOUT1)) << "," << _DEC(getin(RPMOUT2))<< endl;
#endif
 
}
