// Definition des modes:
void mode0()//run mode
{   
  readCaptorTransitions();/* read sensors */
  
#ifdef SERIALPLOTTER /* You can read the battery voltage, speeds and throttle and auxiliary pulses with the Serial Plotter (IDE >= 1.6.6) */
    Serial << ((GetExternalVoltage()>1)?GetExternalVoltage()*1000:0) << "," << Width_us << "," << WidthAux_us << "," << vitesse1 << "," << vitesse2 << endl;
#endif
  
  /* Receiver pulse acquisition and command of 2 servos */
  if(RxChannelPulseMotor.available())
  {
    AVERAGE(Width_us,RxChannelPulseMotor.width_us(), responseTime);/* average */

#ifdef SECURITYENGINE/* Security motors */    
    if (SecurityIsON == true && Width_us >= (fullThrottle - 100))
    {
      SecurityIsON = false;/*( Security is OFF if joystick pass on top position one time) */ 
      BeginSecurityMs=millis(); /* Start the Security Chrono */     
    }
    if (SecurityIsON == false && Width_us <= (idelposServos1 + 100))
    { 
      if(millis() - BeginSecurityMs >= 10000)
      {
        SecurityIsON = true;/*( Security is ON if joystick pass on minimum position afert 10s) */ 
        BeginSecurityMs=millis(); /* Restart the Security Chrono */ 
      }    
    }

    if(SecurityIsON == true) 
    {
      /* Blink LED Management */
      if(millis()-LedStartMs>=1000)
      {
        PIN_HIGH(B,0);PIN_HIGH(B,1);waitMs(20);PIN_LOW(B,0);PIN_LOW(B,1);
        LedStartMs=millis(); /* Restart the Chrono for the LED */
      }
    }
    
#endif/* End Security motors */

    readAuxiliaryChannel();//read Auxiliary channel (1 to 6)
    
    SoftRcPulseOut::refresh(1); /* (=1) allows to synchronize outgoing pulses with incoming pulses */
    BeginChronoServoMs=millis();  /* Restart the Chrono for Pulse */
  
  }
  else//Si absence du canal RX
  {
    /*Si aucun signal au demarrage, les helices sont bloquees avec "Width_us=600")*/
    /* Check for pulse motor extinction */
    if(millis() - BeginChronoServoMs >= 30)//30ms
    {
      Width_us=idelposServos1;
      WidthAux_us=idelposServos1;
      (reverseServo1 == 0)?ServoMotor1.write_us(idelposServos1):ServoMotor1.write_us((centerposServo1*2)-idelposServos1);               
      (reverseServo2 == 0)?ServoMotor2.write_us(idelposServos2):ServoMotor2.write_us((centerposServo2*2)-idelposServos2); 
      /* Refresh the servos with the last known position in order to avoid "flabby" servos */
      SoftRcPulseOut::refresh(1); /* Immediate refresh of outgoing pulses */
      BeginChronoServoMs=millis(); /* Restart the Chrono for Pulse */
    }  
  }
 
// return to menu if two buttons held
#ifdef RETURNTOMENU
#ifdef LCDON
  // check button held time 
  if((millis() - g_LCDML_DISP_press_time) >= 2000) 
  {
    g_LCDML_DISP_press_time = millis(); // reset press time
    static uint16_t value;  // analogpin for keypad
    value = anaRead(_LCDML_CONTROL_analog_pin);
    if(value >= _LCDML_CONTROL_analog_enter_down_min && value <= _LCDML_CONTROL_analog_enter_down_max)//0 = value min with button down and enter help, 400 = value max
    {
      RunSetup = true;
      //LCDML_DISP_groupEnable(_LCDML_G1); /* display all buttons in group 1 */
      //LCDML_setup(_LCDML_BACK_cnt);   /* Setup for LcdMenuLib */
      
      // this function must called here, do not delete it
      LCDML_run(_LCDML_priority); 
    
      lcd.clear();
      lcd.setCursor(0,1);
      lcd.print(F("[ Welcome on Menu! ]"));
      waitMs(2000);
      LCDML.goRoot();
    }
  }
#endif // end LCDON
#endif // end RETURNTOMENU 

}//end mode0()

#ifdef LCDON
/* read the Min & Max pulse from throttle stick */
void radioSetupMinMaxPulse()
{
  static unsigned long beginTime = 0;
  static unsigned long lastTime = 0; 
  beginTime = millis();  // Noter l'heure de début
  lastTime = beginTime + 10000;  // 10s after !
  minimumPulse_US = EEPROMReadInt(23);      //mode1 (in address 23&24)
  maximumPulse_US = EEPROMReadInt(25);      //mode1 (in address 25&26)
  lcd.setCursor(0,1);lcd.print(F("[  Move Throttle   ]"));
  do
  {

    /* Blink LED Management */
    if(millis()-LedStartMs>=100)
    {
      PIN_TOGGLE(B,1);  /* At the next loop, if the half period is elapsed, the LED state will be inverted */
      LedStartMs=millis(); /* Restart the Chrono for the LED */
    }

    lcd.setCursor(16,3);lcd.print(10-((millis() - beginTime)/1000));lcd.print(F("s "));
    lcd.setCursor(0,2);lcd.print(F("Pulse: "));lcd.print(Width_us);
    // Each time through the loop, only save the extreme values if they are greater than the last time through the loop. 
    // At the end we should have the true min and max for Throttle channel.
    if (Width_us < minimumPulse_US)  { minimumPulse_US = Width_us; }
    if (Width_us > maximumPulse_US)  { maximumPulse_US = Width_us; }
    if(LCDML_BUTTON_checkDown())//button down pushed 
    {
      break;
    }
  }
  while (lastTime > millis());    // Keep looping until time's up
  PIN_LOW(B,1);
}
/* read the Min & Max pulse from throttle stick */
#endif

void modeServosTest()//test le fonctionnement des 4 voies en sortie (2servos et 2 glow)
{
  for(pos = 0; pos < 180; pos += 1)  // goes from 0 degrees to 180 degrees 
  {                                  // in steps of 1 degree 
    if (reverseServo1 == 0) {ServoMotor1.write(pos);}else{ServoMotor1.write((90*2)-pos);};               
    if (reverseServo2 == 0) {ServoMotor2.write(pos);}else{ServoMotor2.write((90*2)-pos);};
    delay(5);                       // waits 20ms for refresh period 
    SoftRcPulseOut::refresh();      // generates the servo pulse
  } 
  for(pos = 180; pos>=1; pos-=1)     // goes from 180 degrees to 0 degrees 
  {  
    if (reverseServo1 == 0) {ServoMotor1.write(pos);}else{ServoMotor1.write((90*2)-pos);};               
    if (reverseServo2 == 0) {ServoMotor2.write(pos);}else{ServoMotor2.write((90*2)-pos);};    
    delay(5);                       // waits 20ms for for refresh period 
    SoftRcPulseOut::refresh();      // generates the servo pulse
  }   
}

#ifdef ARDUINO2PC//interface between arduino and PC
void SerialFromToVB()/* thanks to LOUSSOUARN Philippe for this code */
{
  if( MsgDisponible() >= 0) //MsgDisponible() retourne la longueur du message recu; le message est disponible dans Message
  {  
      SecurityIsON = false;
      //uint8_t CountM = CountChar(Message,',');
//      Serial << F("D") << Message << endl;
      pos = atoi(Message); //conversion Message ASCII vers Entier (ATOI= Ascii TO Integer)
      if( (pos >= 0) && (pos <= 180))//reception curseur VB 'servos'
      { 
        if(pos<=1) {pos=2;} //Servo making noise at 0 and 1. Need to be at least 2.
        if(RunSetup == false)
        {
          if(!RxChannelPulseMotor.available())//verifie si Ch Aux est inactif !!!
          {
//            SecurityIsON = false;
            Width_us = map(pos,0,180,minimumPulse_US,maximumPulse_US);
//            readAuxiliaryChannel();//read Auxiliary channel (1 to 6) 
          } 
        }
      }
      else if( (pos >= 181) && (pos <= 360))//reception curseur VB 'Auxiliaire' ou 'Direction'
      { 
        if(RunSetup == false)
        {
          if(!RxChannelPulseAux.available())//verifie si Ch Aux est inactif !!!
          {
//            SecurityIsON = false;
            WidthAux_us = map(pos,181,360,minimumPulse_US,maximumPulse_US);
//            readAuxiliaryChannel();//read Auxiliary channel (1 to 6) 
          }          
        }
      }        
      else if(pos == 362)//reception bouton 'auto'
      { 
        modeServosTest();     
      } 
      else if(pos == 363)//envoie settings au port serie
      {//format send: 1657|1657|1225|1225|2|2075|1393|1070|2205|1|0|0|0|39|2|0|5.00|27.61|1.000|0|0 
        sendConfigToSerial();      
      }
      else if(CountChar(Message,',')==3)//reception simulation motors from VB (forcer vitesse 1 & 2 pour simulation)
      {//format received: 99999,5000,5000,100
        simulateSpeed = true;
        StrSplit(Message, ",",  StrTbl, SUB_STRING_NB_MAX, &SeparFound); 
        vitesse1 = atoi(StrTbl[1]);
        vitesse2 = atoi(StrTbl[2]);
        diffVitesseErr = atoi(StrTbl[3]);
//        Serial << F("D") << vitesse1 << F("|") << vitesse2 << F("|") << diffVitesseErr << endl;
//        StrSplitRestore(",", StrTbl, SeparFound);//Imperatif SeparFound <= SUB_STRING_NB_MAX
        ledFlashSaveInEEProm(2);
        Serial.flush(); // clear serial port

      }       
      else if(CountChar(Message,',')==16)//reception settings from VB
      {//format received: 1656,1653,1385,1385,2,2073,1389,1225,2073,1,0,0,1,3,39,0,0
        StrSplit(Message, ",",  StrTbl, SUB_STRING_NB_MAX, &SeparFound);     
        EEPROMWriteInt(1, atoi(StrTbl[0]));//centerposServo1
        EEPROMWriteInt(3, atoi(StrTbl[1]));//centerposServo2
        EEPROMWriteInt(5, atoi(StrTbl[2]));//idelposServos1
        EEPROMWriteInt(7, atoi(StrTbl[3]));//idelposServos2
        EEPROM.write(9, atoi(StrTbl[4]));//responseTime
        EEPROMWriteInt(11, atoi(StrTbl[5]));//fullThrottle
        EEPROMWriteInt(13, atoi(StrTbl[6]));//beginSynchro
        EEPROMWriteInt(23, atoi(StrTbl[7]));//minimumPulse_US
        EEPROMWriteInt(25, atoi(StrTbl[8]));//maximumPulse_US
        EEPROM.write(15, atoi(StrTbl[9]));//auxChannel
        EEPROM.write(17, atoi(StrTbl[10]));//reverseServo1
        EEPROM.write(19, atoi(StrTbl[11]));//reverseServo2
//        EEPROM.write(21, atoi(StrTbl[12]));//telemetry
        EEPROM.write(27, atoi(StrTbl[13]));//nbPales
        EEPROM.write(29, atoi(StrTbl[14]));//LCD adresse
        EEPROM.write(31, atoi(StrTbl[15]));//moduleMasterOrSlave
        EEPROM.write(33, atoi(StrTbl[16]));//fahrenheitDegrees
//        StrSplitRestore(",", StrTbl, SeparFound);//Imperatif SeparFound <= SUB_STRING_NB_MAX
        ledFlashSaveInEEProm(20);
        Serial.flush(); // clear serial port
      }
      else if(pos == 364)//demande configuration par defaut par VB
      {       
        SettingsWriteDefault();     
      }
      else if(pos == 365)//lecture de la position du canal moteur:
      {             
        Serial << F("M") << Width_us << endl;       
      }
      else if(pos == 366)//lecture des vitesses
      {  
        Serial << F("V") << vitesse1 << F("|") << vitesse2 << endl;
      } 
      else if(pos == 367)//lecture de la position du canal auxiliaire:
      {        
        Serial << F("A") << WidthAux_us << endl;
      }
      else if(pos == 368)//lecture voltage arduino, temp arduino et voltage batterie RX:
      {        
        Serial << F("STS") << _FLOAT(readVcc()/1000,3) << F("|");
        Serial << GetTemp() << F("|");
#ifdef EXTERNALVBATT
        Serial <<  _FLOAT(GetExternalVoltage(),3) << endl;
#else
        Serial <<  _FLOAT(1.000,3) << endl;
#endif
      }
      else if(pos == 369)//On/Off Glow Plug command
      { 
        if (glowControlIsActive==false)
        {
          PIN_HIGH(B,0);PIN_HIGH(B,1);
          glowControlIsActive=true;
        }
        else
        { 
          PIN_LOW(B,0);PIN_LOW(B,1);
          glowControlIsActive=false;        
        }
        
      }
      readAuxiliaryChannel();//read Auxiliary channel and his modes (1 to 6)
    }  

}//end SerialFromToVB()

//unsigned int CountChar(String input, char c )
uint8_t CountChar(String input, char c )
{
  uint8_t retval = 0;
  for (uint8_t i = 0; i < input.length()+1; i ++)
  if (c == input [i])
  retval ++;
  return retval;
}

/**********************************************************************************************************
Decompose une liste de chaines de caracteres separe par une chaine "separateur" en un tableau de chaines de
caracteres. Si la chaine initiale se termine par le separateur, le dernier element vide n'est pas compte.
Entree:
	SrcStr:		Pointeur sur la chaine a traiter
	Separ:		Pointeur sur la chaine "separateur"
	TarStrTbl:	Pointeur sur le tableau de chaines destinations
	TblLenMax:	Nombre Max de sous-chaines
Sortie:
	Le nombre de sous-chaine
	SeparFound: Le nombre de Separateur trouve ( sera utilise pour StrSplitRestore() )
Attention: cette fonction optimisee pour l'embarque remplace le 1er caractere de chaque separateur par \0
Pour restaurer la chaine d'origine, appeler StrSplitRestore(char *Separ, char **TarStrTbl, uint8_t SeparNbToRestore) apres
utilisation des sous-chaines disponible dans TarStrTbl.
**********************************************************************************************************/
int8_t StrSplit(char *SrcStr, char *Separ,  char **TarStrTbl, uint8_t TblLenMax, uint8_t *SeparFound)/* merci a LOUSSOUARN Philippe pour ce code */
{
  uint8_t SubStrFound, SeparLen;
  char *StartStr, *SeparPos;

  *SeparFound = 0;
  SubStrFound = 0;
  /* Clear Table Entries (secu) in case of empty string */
  for(uint8_t Idx = 0; Idx < TblLenMax; Idx++)
  {
    TarStrTbl[Idx] = NULL; /* End of string */
  }
  if (!strlen(SrcStr)) return(0);
  StartStr = SrcStr; /* Do NOT touch SrcStr */
  SeparLen = strlen(Separ);
  while(TblLenMax)
  {
    SeparPos = strstr(StartStr, Separ);
    if(SeparPos)
    {
      /* OK Substring found */
      *SeparPos = 0;
      TarStrTbl[(*SeparFound)++] = StartStr;
      StartStr = SeparPos + SeparLen;
      SubStrFound++; // <---- Correction d'un bug
    }
    else
    {
      SubStrFound = *SeparFound;
      if(*StartStr)
      {
        TarStrTbl[*SeparFound] = StartStr;
        SubStrFound++;
      }
      break;
    }
    TblLenMax--;
  }
  return(SubStrFound);
}
/**********************************************************************************************************
Cette fonction optimisee pour l'embarque restore la chaine SrcStr splittee a l'aide la fonction
StrSplit(char *SrcStr, char *Separ,  char **TarStrTbl, uint8_t SeparNbToRestore).
Important Note: SeparNbToRestore SHALL be <= TblLenMax passed as argument for StrSplit().
**********************************************************************************************************/
//int8_t StrSplitRestore(char *Separ,  char **TarStrTbl, uint8_t SeparNbToRestore)/* merci a LOUSSOUARN Philippe pour ce code */
//{
//  uint8_t Idx, Restored = 0;
//  
//  for(Idx = 0; Idx < SeparNbToRestore; Idx++)
//  {
//    /* Restore \0 with first char of Separator */
//    if(TarStrTbl[Idx] != NULL)
//    {
//      TarStrTbl[Idx][strlen(TarStrTbl[Idx])] = Separ[0];
//      Restored++;
//    }
//  }
//  return(Restored);
//}

int8_t MsgDisponible(void)/* merci a LOUSSOUARN Philippe pour ce code */
{
  int8_t Ret = -1;
  char CaractereRecu;
  static uint8_t Idx = 0;

  if(Serial.available() > 0)
  {
    CaractereRecu = Serial.read();
    switch(CaractereRecu)
    {
      case RETOUR_CHARRIOT:
      case PASSAGE_LIGNE:
        Message[Idx] = 0;
        Ret = Idx;
        Idx = 0;
        break; 
      case BACK_SPACE: // Gestion touche d'effacement du dernier caractere sur un terminal fonctionnant caractere par caractere (ex: HyperTerminal, TeraTerm, etc...)
        if(Idx) Idx--;
        break;
      default:
        if(Idx < LONGUEUR_MSG_MAX)
        {
            Message[Idx] = CaractereRecu;
            Idx++;
        }
        else Idx = 0; /* Reset index for the next message */
        break;
    }
  }
  return(Ret); 
}
#endif

