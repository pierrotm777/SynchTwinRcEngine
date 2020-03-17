/* 
  Le capteur IR GP2Y0D810Z0F est connecté comme suit:
    * BOARD -> ARDUINO
    * Vcc   -> 5V
    * GND   -> GND
    * OUT   -> PIN 6 and 7
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
void InterruptFunctionToCall1(void)
{
  uint8_t PortChange;
    
  PortChange = TinyPinChange_GetPortEvent(VirtualPortNb);
  if(PortChange & TinyPinChange_PinToMsk(BROCHE_SENSOR1)) /* Check BROCHE_SENSOR1 has changed */
  {
      FirstInputChangeCount++; /* Rising AND Falling edges are counted */
  }
}
void InterruptFunctionToCall2(void)
{
  uint8_t PortChange;
    
  PortChange = TinyPinChange_GetPortEvent(VirtualPortNb_);  
  if(PortChange & TinyPinChange_PinToMsk(BROCHE_SENSOR2)) /* Check BROCHE_SENSOR2 has changed */
  {
      SecondInputChangeCount++; /* Rising AND Falling edges are counted */
  }
}

void readCaptorTransitions()
{
  /* Display Transition numbers every second */
  if((millis()-ReadCaptorsMs >= 500))
  {
    //noInterrupts(); /* Mandatory since counters are 16 bits */
    ATOMIC_BLOCK(ATOMIC_FORCEON){//ATOMIC_RESTORESTATE or ATOMIC_FORCEON
      if (simulateSpeed == false)
      {
        vitesse1 = (FirstInputChangeCount > 0)?FirstInputChangeCount*60*2/nbPales:0;// preventing 0/nbPales
        vitesse2 = (SecondInputChangeCount > 0)?SecondInputChangeCount*60*2/nbPales:0;
      }
      FirstInputChangeCount = 0;SecondInputChangeCount = 0; //Set NbTops to 0 ready for calculations 
    }
    //interrupts();
    ReadCaptorsMs=millis();
  }

  if (vitesse1 !=0) {PIN_HIGH(B,0);PIN_LOW(B,0);}// D8(GREEN)
  if (vitesse2 !=0) {PIN_HIGH(B,1);PIN_LOW(B,1);}// D9(GREEN)
  
#ifdef LCDON 
    /* Info change on LCD each 1 seconde */
    if(millis() - BeginChronoLcdMs >= 1000)//lets not saturate the loop() and makes the movement of servos more fluid
    {
      lcd.setCursor(0,0);lcd.print(F("[Speed 1&2]"));
      lcd.setCursor(0,1);lcd.print(F("S1["));lcd.print(vitesse1);lcd.print(F("]      "));
      lcd.setCursor(16-(String(vitesse2).length()),1);lcd.print(F("S2["));lcd.print(vitesse2);lcd.print(F("]"));
      //use 2000 bytes memory
      static uint8_t StringToPrint; 
      lcd.setCursor(0,3);
      switch(StringToPrint)
      {
//        case 0:lcd.print(F("Mem.:"));lcd.print(freeRam());lcd.print(F("  "));break;
#ifdef EXTERNALVBATT
        case 1:lcd.print(F("V:"));lcd.print(GetExternalVoltage());lcd.print(F("V   "));break;
        case 2:battery_Informations();break;
#endif
        case 3:lcd.print(F("Aux.:"));readAuxTypeLCD();break;
        case 4:lcd.print(F("Sync.:"));lcd.print((synchroIsActive == true)?"On ":"Off");break;
        case 5:lcd.print(F("T:"));lcd.print(GetTemp());lcd.print(F("\x06"));lcd.print((EEPROM.read(33)==0)?"C":"F");break;
//        case 6:lcd.print(F("Time:"));lcd.print(ElapsedTime());break;
      }      
      lcd.setCursor(11,3);lcd.print(F("Secu.:"));lcd.print((SecurityIsON == true)?"On ":"Off");  
      (StringToPrint < 5)?StringToPrint++:StringToPrint=1;
      //use 2000 bytes memory 
      BeginChronoLcdMs=millis(); /* Restart the Chrono for the LCD */
    }
    
#endif
 
}


