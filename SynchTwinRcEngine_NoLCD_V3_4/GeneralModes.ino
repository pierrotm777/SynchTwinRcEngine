// Definition des modes:
void mode0()//run mode
{   
  readCaptorTransitions();/* read sensors */

//#ifdef SettingsPortPLOTTER /* You can read the battery voltage, speeds and throttle and auxiliary pulses with the SettingsPort Plotter (IDE >= 1.6.6) */
//   SettingsPort << ((GetExternalVoltage()>1)?GetExternalVoltage()*1000:0) << "," << Width_us << "," << WidthAux_us << "," << vitesse1 << "," << vitesse2 << endl;
//#endif
  static uint32_t BeginChronoServoMs = millis();

  /* Receiver pulsse acquisition and command of 2 servos */
  if (ms.InputMode == 0)//PPM
  {
    if (TinyCppmReader.isSynchro())
    {
      InputSignalExist = true;
      Width_us    = TinyCppmReader.width_us(MotorNbChannel);
      WidthAux_us = TinyCppmReader.width_us(ms.AuxiliaryNbChannel);
      WidthRud_us = TinyCppmReader.width_us(RudderNbChannel);
      WidthAil_us = TinyCppmReader.width_us(AileronNbChannel);      
    }
    else
    {
      InputSignalExist = false;
    }
  }
  else if (ms.InputMode == 1)//SBUS
  {
    SBusRx.process(); /* Don't forget to call the SBusRx.process()! */
    if(SBusRx.isSynchro()) /* One SBUS frame just arrived */
    {
      if (!SBusRx.flags(SBUS_RX_FRAME_LOST))
      {
        InputSignalExist = true;
      }
      else
      {
        InputSignalExist = false;
      }
      
      /* Display SBUS channels and flags in the serial console */
//      for(uint8_t Ch = 1; Ch <= SBUS_RX_CH_NB; Ch++)
//      {
//        Serial.print(F("Ch["));Serial.print(Ch);Serial.print(F("]="));Serial.print(SBusRx.width_us(Ch));Serial.print(F(" Raw="));Serial.println(SBusRx.rawData(Ch));
//      }
      Width_us    = SBusRx.width_us(MotorNbChannel);
      WidthAux_us = SBusRx.width_us(ms.AuxiliaryNbChannel);
      WidthRud_us = SBusRx.width_us(RudderNbChannel);
      WidthAil_us = SBusRx.width_us(AileronNbChannel);
//      Serial.print(F("Ch17="));    Serial.println(SBusRx.flags(SBUS_RX_CH17)); /* Digital Channel#17 */
//      Serial.print(F("Ch18="));    Serial.println(SBusRx.flags(SBUS_RX_CH18)); /* Digital Channel#18 */
//      Serial.print(F("FrmLost=")); Serial.println(SBusRx.flags(SBUS_RX_FRAME_LOST)); /* Switch off the Transmitter to check this */
//      Serial.print(F("FailSafe="));Serial.println(SBusRx.flags(SBUS_RX_FAILSAFE));   /* Switch off the Transmitter to check this */
    }
  }
  else if (ms.InputMode == 2)//IBUS
  {
    IBus.loop();
    if (IBus.readChannel(0) > 0)
    {
      InputSignalExist = true;
      Width_us    = IBus.readChannel(MotorNbChannel-1);
      WidthAux_us = IBus.readChannel(ms.AuxiliaryNbChannel-1);
      WidthRud_us = IBus.readChannel(RudderNbChannel-1);
      WidthAil_us = IBus.readChannel(AileronNbChannel-1);
    }
    else
    {
      InputSignalExist = false;
    }    
  }
  
  if (InputSignalExist == true)
  {    
#ifdef SECURITYENGINE/* Security motors */    
    if (SecurityIsON == true && Width_us >= (ms.fullThrottle - 100))
    {
      SecurityIsON = false;/*( Security is set OFF if joystick pass on top position one time) */ 
      BeginSecurityMs=millis(); /* Start the Security Chrono */     
    }
    if (SecurityIsON == false && Width_us <= (ms.idelposServos1 + 100))
    { 
      if(millis() - BeginSecurityMs >= 10000)
      {
        SecurityIsON = true;/*( Security is set ON if joystick pass on minimum position after 10s) */ 
        BeginSecurityMs=millis(); /* Restart the Security Chrono */ 
      }    
    }

    if(SecurityIsON == true) 
    {
      /* Blink LED Management */
      if(millis()-LedStartMs>=1000)
      {
        flip(LED);//blinkNTime(1,125,1000);
        LedStartMs=millis(); /* Restart the Chrono for the LED */
      }
    } 
#endif/* End Security motors */

    readAuxiliaryChannel();//read Auxiliary channel (1 to 6)
    
    SoftRcPulseOut::refresh(1); /* (=1) allows to synchronize outgoing pulses with incoming pulses */
    // Blink each 250ms if PPM found on pin 2
    if(millis()-LedStartMs>=LED_SIGNAL_FOUND)
    {
      flip(LED);
      LedStartMs=millis(); // Restart the Chrono for the LED 
    }  
    BeginChronoServoMs=millis();  /* Restart the Chrono for Pulse */
  
  }
  else//Si absence du canal RX
  {
    // Blink each 1s if PPM not found on pin 2
    if(millis()-LedStartMs>=LED_SIGNAL_NOTFOUND)
    {
      flip(LED);
      LedStartMs=millis(); // Restart the Chrono for the LED 
    }
    /*Si aucun signal au demarrage, les helices sont bloquees avec "Width_us=1000")*/
    /* Check for pulse motor extinction */
    //if(millis() - BeginChronoServoMs >= 30 && simulateSpeed == false)//30ms
    if(millis() - BeginChronoServoMs >= 30)//30ms
    {
      (ms.reverseServo1 == 0)?ServoMotor1.write_us(ms.idelposServos1):ServoMotor1.write_us((ms.centerposServo1*2)-ms.idelposServos1);               
      (ms.reverseServo2 == 0)?ServoMotor2.write_us(ms.idelposServos2):ServoMotor2.write_us((ms.centerposServo2*2)-ms.idelposServos2); 
      /* Refresh the servos with the last known position in order to avoid "flabby" servos */
      SoftRcPulseOut::refresh(1); /* Immediate refresh of outgoing pulses */
      BeginChronoServoMs=millis(); /* Restart the Chrono for Pulse */
    }  
  }
 
}//end mode0()

#ifdef ARDUINO2PC//interface between arduino and PC
void SerialFromToVB()/* thanks to LOUSSOUARN Philippe for this code */
{
  //SettingsPort.flush();
  
  if( MsgDisponible() >= 0) //MsgDisponible() retourne la longueur du message recu; le message est disponible dans Message
  {
            
      SecurityIsON = false;
      static uint16_t posInUs;

      //countVirgulesInMessage = CountChar(Message,',');
//#ifdef DEBUG
//      SettingsPort << F("New settings received: ") << countVirgulesInMessage + 1 << F("  ") << Message << endl;
//#endif    
      pos = atoi(Message); //conversion Message ASCII vers Entier (ATOI= Ascii TO Integer)
      if( (pos >= 0) && (pos <= 180))//reception curseur VB 'servos'
      { 
        //if(pos<=1) {pos=2;} //Servo making noise at 0 and 1. Need to be at least 2.
        if (!TinyCppmReader.isSynchro() && simulateSpeed == true)//verifie si Ch Moteur est inactif !!!
        {
          //PIN_TOGGLE(B,0);
          posInUs = map(pos, 0, 180, ms.minimumPulse_US, ms.maximumPulse_US);
          if (ms.reverseServo1 == 0) {ServoMotor1.write_us(posInUs);}else{ServoMotor1.write_us((ms.centerposServo1*2)-posInUs);}               
          if (ms.reverseServo2 == 0) {ServoMotor2.write_us(posInUs);}else{ServoMotor2.write_us((ms.centerposServo2*2)-posInUs);}
          waitMs(5);
          SoftRcPulseOut::refresh(1);      // generates the servo pulse
        } 
      }
      else if( (pos >= ms.minimumPulse_US) && (pos <=ms.maximumPulse_US))//reception VB en uS 'servos'
      {        
        if (!TinyCppmReader.isSynchro() && simulateSpeed == true)//verifie si Ch Moteur est inactif !!!
        {
          //PIN_TOGGLE(B,0);
          //posInUs = map(pos,0,180,minimumPulse_US,maximumPulse_US);
          if (ms.reverseServo1 == 0) {ServoMotor1.write_us(pos);}else{ServoMotor1.write_us((ms.centerposServo1*2)-pos);}               
          if (ms.reverseServo2 == 0) {ServoMotor2.write_us(pos);}else{ServoMotor2.write_us((ms.centerposServo2*2)-pos);}
          waitMs(5);
          SoftRcPulseOut::refresh(1);      // generates the servo pulse
        } 
      }

      else if (pos == 889)
      {
        simulateSpeed = false;                   
      }                    
      else if(pos == 998)//send if V+ external is used
      {
#ifdef EXTERNALVBATT         
        SettingsPort << F("POWER|1") << endl;
#else
        SettingsPort << F("POWER|0") << endl;
#endif
      }  
      else if(pos == 999)//send Module Version
      { 
        SettingsPort << F("FIRM|") << FirmwareVersion << endl;
      }
      
      else if(pos == 360)//CPPM
      {             
        ms.InputMode = 0;
        EEPROM.put(0, ms);
        blinkNTime(5,LED_SIGNAL_FOUND,LED_SIGNAL_FOUND);
      }
      else if(pos == 361)//SBUS
      {             
        ms.InputMode = 1;
        EEPROM.put(0, ms);
        blinkNTime(5,LED_SIGNAL_FOUND,LED_SIGNAL_FOUND);
      }
      else if(pos == 362)//IBUS
      {             
        ms.InputMode = 2;
        EEPROM.put(0, ms);
        blinkNTime(5,LED_SIGNAL_FOUND,LED_SIGNAL_FOUND);
      }
      else if(pos == 363)//envoie settings au port serie
      {//format send: 1657|1657|1225|1225|2|2075|1393|1070|2205|1|0|0|100|39|2|0|5.00|27.61|1.000|0|1000|20000|0
        sendConfigToSettingsPort();
      } 
      else if(pos == 364)//demande configuration par defaut par VB
      {       
        SettingsWriteDefault();     
      }
      
      else if(pos == 365)//lecture de la position du canal moteur:
      {             
        SettingsPort << F("M") << Width_us << endl;       
      }
      else if(pos == 366)//run or stop speed read
      {  
        SettingsPort << F("V") << vitesse1 << F("|") << vitesse2 << endl;/* Send speed to VB interface */
      } 
      else if(pos == 367)//lecture de la position du canal auxiliaire:
      {        
        SettingsPort << F("A") << WidthAux_us << endl;
      }

      else if(pos == 368)//lecture voltage arduino, temp arduino et voltage batterie RX:
      {        
        SettingsPort << F("STS") << _FLOAT(5/1000,3) << F("|");//SettingsPort << F("STS") << _FLOAT(readVcc()/1000,3) << F("|");
        SettingsPort << 0 << F("|");//SettingsPort << GetTemp() << F("|");
#ifdef EXTERNALVBATT
        SettingsPort <<  _FLOAT(GetExternalVoltage(),3) << endl;
#else
        SettingsPort <<  F("NOTUSED|") << endl;
#endif
      }
      else if(pos == 369)//On/Off Glow Plug command
      { 
        if (glowControlIsActive==false)
        {
          //PIN_HIGH(C,6);
          glowControlIsActive=true;
        }
        else
        { 
          //PIN_LOW(C,6);
          glowControlIsActive=false;        
        }
      }
      else if(pos == 370)//erase EEprom
      { 
        clearEEprom();
      }

#ifdef FRAM_USED           
      else if(pos == 371)//read FRAM Infos
      {
        //readFramInfos(); //à tester
      }
      else if(pos == 372)//start/stop FRAM
      {
      }
      else if(pos == 373)//read FRAM
      {
      }
#else
      else if(pos == 371)//read FRAM Infos
      {
        SettingsPort << F("NOFRAM") <<endl;
      } 
#endif
      
      if(CountChar(Message,',')>0)
      //if(countVirgulesInMessage == 19)//reception settings from VB (test le nombre de virgule, pas de variables)
      {//format received: 1500,1500,1000,1000,2,2000,1250,1200,1900,1,0,0,99,2,0,0,0,1000,20000,0
        //                1500,1500,1000,1000,2,2000,1250,1000,2000,1,0,0,99,2,1,0,0,1000,20000,1
#ifdef DEBUG
        SettingsPort << F("New settings received: ") << Message << endl;
        SettingsPort << sizeof(Message) << endl;
#endif               
        StrSplit(Message, ",",  StrTbl, SUB_STRING_NB_MAX, &SeparFound);     
        ms.centerposServo1 = atoi(StrTbl[0]);//centerposServo1
        ms.centerposServo2 = atoi(StrTbl[1]);//centerposServo2
        ms.idelposServos1  = atoi(StrTbl[2]);//idelposServos1
        ms.idelposServos2  = atoi(StrTbl[3]);//idelposServos2
        ms.responseTime    = atoi(StrTbl[4]);//responseTime
        ms.fullThrottle    = atoi(StrTbl[5]);//fullThrottle
        ms.beginSynchro    = atoi(StrTbl[6]);//beginSynchro
        ms.minimumPulse_US = atoi(StrTbl[7]);//minimumPulse_US
        ms.maximumPulse_US = atoi(StrTbl[8]);//maximumPulse_US
        ms.auxChannel      = atoi(StrTbl[9]);//auxChannel
        ms.reverseServo1   = atoi(StrTbl[10]);//reverseServo1
        ms.reverseServo2   = atoi(StrTbl[11]);//reverseServo2
        ms.diffVitesseErr  = atoi(StrTbl[12]);//diffVitesseErr
        ms.nbPales         = atoi(StrTbl[13]);//nbPales
        ms.radioRcMode     = atoi(StrTbl[14]);//Rc radio mode (1 to 4)
        ms.moduleMasterOrSlave = atoi(StrTbl[15]);//moduleMasterOrSlave
        ms.fahrenheitDegrees = atoi(StrTbl[16]);//fahrenheitDegrees
        ms.minimumSpeed    = atoi(StrTbl[17]);//minimum motor rpm
        ms.maximumSpeed    = atoi(StrTbl[18]);//maximum motor rpm
        ms.InputMode       = atoi(StrTbl[19]);//CPPM,SBUS or IBUS
        //ms.telemetryType   = atoi(StrTbl[20]);//0 = Frsky
        
        StrSplitRestore(",", StrTbl, SeparFound);//Imperatif SeparFound <= SUB_STRING_NB_MAX
        EEPROM.put(0,ms);
        blinkNTime(5,100,100);
        //SettingsPort.flush(); // clear SettingsPort port
      }
      else if(countVirgulesInMessage == 3)//check XXX,A,B,C 
      {
        StrSplit(Message, ",",  StrTbl, SUB_STRING_NB_MAX, &SeparFound);
        switch (atoi(StrTbl[0]))
        {
#ifdef FRAM_USED
          case 886://886,x,y1,y2         
            //writeToFRAM(atoi(StrTbl[2]),atoi(StrTbl[3])); // OK avec VB
            break;
          case 890://890,fileName,0,0         
            //readFromFRAM(StrTbl[1]); //à tester
            break;
          case 891://891,fileName,0,0         
            //eraseAllFRAM(StrTbl[1]); //à tester
            break;            
#endif 
//#ifdef PIDCONTROL           
//          case 887://887,Kp,Ki,Kd
////            ms.consKp = strtod(StrTbl[1],NULL);
////            ms.consKi = strtod(StrTbl[2],NULL);
////            ms.consKd = strtod(StrTbl[3],NULL);
//            //SettingsPort << StrTbl[1] << F("|") << StrTbl[2] << F("|") << StrTbl[3] << endl;
//
//            EEPROM.put(0,ms);
//            ledFlashSaveInEEProm(5);
//            break;
//#endif
          case 888://888,v1,v2,consigne
            simulateSpeed = true;
            vitesse1 = atoi(StrTbl[1]);
            vitesse2 = atoi(StrTbl[2]);
            ms.diffVitesseErr = atoi(StrTbl[3]);
            blinkNTime(5,LED_SIGNAL_FOUND,LED_SIGNAL_FOUND);
            break;                                 
        }
        StrSplitRestore(",", StrTbl, SeparFound);//Imperatif SeparFound <= SUB_STRING_NB_MAX
        //SettingsPort.flush(); // clear SettingsPort port
      } 
    } 

    readAuxiliaryChannel();//read Auxiliary channel and his modes (1 to 6)

}//end SettingsPortFromToVB()

uint8_t CountChar(String input, char c )
{
  uint8_t retval = 0;
  for (uint8_t i = 0; i < input.length()+1; i ++)
  if (c == input [i])
  retval ++;
  return retval;
}

//uint8_t checkVirgules(String Input , char c)
//{
//  
//  // Count number of comma
//  int count=0;
//  int count_comma;
//  int strlength = Input.length();
// 
//  do
//  {
//      if(String(Input[count]) == ",")
//      {
//        count_comma++;
//      }
//      count++;
//   
//  }while(count < strlength);
// 
//  Serial.print(F("Nombre de virgules: ")); Serial.println(count_comma);
//}


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
int8_t StrSplitRestore(char *Separ,  char **TarStrTbl, uint8_t SeparNbToRestore)/* merci a LOUSSOUARN Philippe pour ce code */
{
  uint8_t Idx, Restored = 0;
  
  for(Idx = 0; Idx < SeparNbToRestore; Idx++)
  {
    /* Restore \0 with first char of Separator */
    if(TarStrTbl[Idx] != NULL)
    {
      TarStrTbl[Idx][strlen(TarStrTbl[Idx])] = Separ[0];
      Restored++;
    }
  }
  return(Restored);
}

int8_t MsgDisponible(void)/* merci a LOUSSOUARN Philippe pour ce code */
{
  int8_t Ret = -1;
  char CaractereRecu;
  static uint8_t Idx = 0;

  
  if(SettingsPort.available() > 0)
  {
    CaractereRecu = SettingsPort.read();
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
