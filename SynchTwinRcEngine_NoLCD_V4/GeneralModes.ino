// Definition des modes:
void mode0()//run mode
{   
  readCaptorTransitions();/* read sensors */

  static uint32_t BeginChronoServoMs = millis();

  /* Receiver pulse acquisition and command of 2 servos */
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
  else if (ms.InputMode == 1/*SBUS*/ || ms.InputMode == 2/*SRXL*/ || ms.InputMode == 3/*SUMD*/ || ms.InputMode == 4/*IBUS*/ || ms.InputMode == 5/*JETI*/)
  {
    RcBusRx.process(); /* Don't forget to call the SBusRx.process()! */
    if(RcBusRx.isSynchro()) /* One SBUS frame just arrived */
    {
#ifdef DEBUG
//      if(millis() - checkSBUSValues >= 1000)
//      {
//        SettingsPort.print(F("Ch17="));    SettingsPort.println(SBusRx.flags(SBUS_RX_CH17)); /* Digital Channel#17 */
//        SettingsPort.print(F("Ch18="));    SettingsPort.println(SBusRx.flags(SBUS_RX_CH18)); /* Digital Channel#18 */
//        SettingsPort.print(F("FrmLost=")); SettingsPort.println(SBusRx.flags(SBUS_RX_FRAME_LOST)); /* Switch off the Transmitter to check this */
//        SettingsPort.print(F("FailSafe="));SettingsPort.println(SBusRx.flags(SBUS_RX_FAILSAFE));   /* Switch off the Transmitter to check this */
//        checkSBUSValues=millis(); 
//      }  
#endif
      if(ms.InputMode == 1/*SBUS*/)
      {
        InputSignalExist = !RcBusRx.flags(SBUS_RX_FRAME_LOST);        
      }
      else
      {
        InputSignalExist = true;
      }
      Width_us    = RcBusRx.width_us(MotorNbChannel);
      WidthAux_us = RcBusRx.width_us(ms.AuxiliaryNbChannel);
      WidthRud_us = RcBusRx.width_us(RudderNbChannel);
      WidthAil_us = RcBusRx.width_us(AileronNbChannel);
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
    // Blink each 250ms if PPM found on pin Rx
    if(millis()-LedStartMs>=LED_SIGNAL_FOUND)
    {
      flip(LED);
      LedStartMs=millis(); // Restart the Chrono for the LED 
    }  
    BeginChronoServoMs=millis();  /* Restart the Chrono for Pulse */
  
  }
  else//Si absence du canal RX
  {
    // Blink each 1s if PPM not found on pin Rx
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

#ifdef PLOTTER /* You can read the battery voltage, speeds and throttle and auxiliary pulses with the SettingsPort Plotter (IDE >= 1.6.6) */
   SettingsPort << ((GetExternalVoltage()>1)?GetExternalVoltage()*1000:0) << "," << Width_us << "," << WidthAux_us << "," << WidthRud_us << "," << WidthAil_us << "," << vitesse1 << "," << vitesse2 << endl;
#endif  
 
}//end mode0()

#ifdef ARDUINO2PC//interface between arduino and PC
void SerialFromToVB()/* thanks to LOUSSOUARN Philippe for this code */
{
  
  if( MsgDisponible() >= 0) //MsgDisponible() retourne la longueur du message recu; le message est disponible dans Message
  {
            
      SecurityIsON = false;
      static uint16_t posInUs;
  
      pos = atoi(Message); //conversion Message ASCII vers Entier (ATOI= Ascii TO Integer)
      if( (pos >= 0) && (pos <= 180))//reception curseur VB 'servos'
      { 
        //if(pos<=1) {pos=2;} //Servo making noise at 0 and 1. Need to be at least 2.
        if (InputSignalExist == true && simulateSpeed == true)//verifie si Ch Moteur est inactif !!!
        {
          posInUs = map(pos, 0, 180, ms.minimumPulse_US, ms.maximumPulse_US);
          if (ms.reverseServo1 == 0) {ServoMotor1.write_us(posInUs);}else{ServoMotor1.write_us((ms.centerposServo1*2)-posInUs);}               
          if (ms.reverseServo2 == 0) {ServoMotor2.write_us(posInUs);}else{ServoMotor2.write_us((ms.centerposServo2*2)-posInUs);}
          waitMs(5);
          SoftRcPulseOut::refresh(1);      // generates the servo pulse
        } 
      }
      else if( (pos >= ms.minimumPulse_US) && (pos <=ms.maximumPulse_US))//reception VB en uS 'servos'
      {        
        if (InputSignalExist == true && simulateSpeed == true)//verifie si Ch Moteur est inactif !!!
        {
          //posInUs = map(pos,0,180,minimumPulse_US,maximumPulse_US);
          if (ms.reverseServo1 == 0) {ServoMotor1.write_us(pos);}else{ServoMotor1.write_us((ms.centerposServo1*2)-pos);}               
          if (ms.reverseServo2 == 0) {ServoMotor2.write_us(pos);}else{ServoMotor2.write_us((ms.centerposServo2*2)-pos);}
          waitMs(5);
          SoftRcPulseOut::refresh(1);      // generates the servo pulse
        } 
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
      else if(pos == 362)//SRXL
      {             
        ms.InputMode = 2;
        EEPROM.put(0, ms);
        blinkNTime(5,LED_SIGNAL_FOUND,LED_SIGNAL_FOUND);
      }
      else if(pos == 363)//SUMD
      {             
        ms.InputMode = 3;
        EEPROM.put(0, ms);
        blinkNTime(5,LED_SIGNAL_FOUND,LED_SIGNAL_FOUND);
      }
      else if(pos == 364)//IBUS
      {             
        ms.InputMode = 4;
        EEPROM.put(0, ms);
        blinkNTime(5,LED_SIGNAL_FOUND,LED_SIGNAL_FOUND);
      }
      else if(pos == 365)//JETI
      {             
        ms.InputMode = 5;
        EEPROM.put(0, ms);
        blinkNTime(5,LED_SIGNAL_FOUND,LED_SIGNAL_FOUND);
      }
      
      else if(pos == 368)//envoie settings au port serie
      {//format send: 1657|1657|1225|1225|2|2075|1393|1070|2205|1|0|0|100|39|2|0|5.00|27.61|1.000|0|1000|20000|0|4.0
        sendConfigToSettingsPort();
      } 
      else if(pos == 369)//demande configuration par defaut par VB
      {       
        SettingsWriteDefault();     
      }
      else if(pos == 370)//erase EEprom
      { 
        clearEEprom();
      }  

        
      else if(pos == 400)//lecture de la position du canal moteur:
      {             
        SettingsPort << F("M") << Width_us << endl;       
      }
      else if(pos == 401)//run or stop speed read
      {  
        SettingsPort << F("V") << vitesse1 << F("|") << vitesse2 << endl;/* Send speed to VB interface */
        //SettingsPort << F("V") << 8500 << F("|") << 8400 << endl;/* Send speed to VB interface */
      } 
      else if(pos == 402)//lecture de la position du canal auxiliaire:
      {        
        SettingsPort << F("A") << WidthAux_us << endl;
      }

      else if(pos == 403)//lecture voltage arduino, temp arduino et voltage batterie RX:
      {        
        SettingsPort << F("STS") << _FLOAT(readVcc()/1000,3) << F("|");
        SettingsPort << GetTemp() << F("|");
#ifdef EXTERNALVBATT
        SettingsPort <<  _FLOAT(GetExternalVoltage(),3) << endl;
#else
        SettingsPort <<  F("NOTUSED|") << endl;
#endif
      }
      else if(pos == 404)//On Glow Plug command
      {
        glowControlIsActive=true;
      }
      else if(pos == 405)//Off Glow Plug command
      { 
        glowControlIsActive=false;
      } 

#ifdef FRAM_USED           
      else if(pos == 600)//read FRAM Infos
      {
        //readFramInfos(); //à tester
      }
      else if(pos == 601)//read FRAM
      {
      }
      else if(pos == 602)//write to FRAM
      {
        //readFramInfos(); //à tester
      }
      else if(pos == 603)//erase FRAM
      {
      }
      else if(pos == 604)//dump all FRAM
      {
      }
#else
      else if(pos == 600)//read FRAM Infos
      {
        SettingsPort << F("NOFRAM") <<endl;
      } 
#endif


#ifdef RECORDER
      else if(pos == 700)//RECORDER ON
      {
        ServoRecorderIsON = true;
#ifdef DEBUG
        SettingsPort << F("RECORDER ON") <<endl;
#endif
      } 
      else if(pos == 701)//RECORDER OFF
      {
        ServoRecorderIsON = false;
#ifdef DEBUG
        SettingsPort << F("RECORDER OFF") <<endl;
#endif
      }
      else if(pos == 702)// Start Play
      {
        recoderMode = 0;releaseButtonMode = true;
#ifdef DEBUG
        SettingsPort << F("PLAY ON") <<endl;
#endif
      }
      else if(pos == 703)//Stop Play
      {
        recoderMode = 1;releaseButtonMode = true;
#ifdef DEBUG
        SettingsPort << F("PLAY OFF") <<endl;
#endif
      }
      else if(pos == 704)//Read VB / Throttle stick
      {
        recoderMode = 1;releaseButtonMode = true;
#ifdef DEBUG
        SettingsPort << F("Read Throttle") <<endl;
        SettingsPort << F("Read VB Slider") <<endl;
#endif
      }        
#endif

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
        CheckIfVBUsed = true;
        SettingsPort << F("FIRM|") << FirmwareVersion << endl;
      }

      if(CountChar(Message,',')>0)
      {/*formats received: S1,1500,1500,1000,1000,2,2000,1250,1200,1900,1,0
                           S2,0,99,2,0,0,0,1000,20000,0,0,4.0
                           S3,1000,1000,100
                           S4,0,0,100*/
        static String checkMess;       
        StrSplit(Message, ",",  StrTbl, SUB_STRING_NB_MAX, &SeparFound);
        checkMess = StrTbl[0];
        if (checkMess == "S1")
        {
            ms.centerposServo1 = atoi(StrTbl[1]);//centerposServo1
            ms.centerposServo2 = atoi(StrTbl[2]);//centerposServo2
            ms.idelposServos1  = atoi(StrTbl[3]);//idelposServos1
            ms.idelposServos2  = atoi(StrTbl[4]);//idelposServos2
            ms.responseTime    = atoi(StrTbl[5]);//responseTime
            ms.fullThrottle    = atoi(StrTbl[6]);//fullThrottle
            ms.beginSynchro    = atoi(StrTbl[7]);//beginSynchro
            ms.minimumPulse_US = atoi(StrTbl[8]);//minimumPulse_US
            ms.maximumPulse_US = atoi(StrTbl[9]);//maximumPulse_US
            ms.auxChannel      = atoi(StrTbl[10]);//auxChannel
            ms.reverseServo1   = atoi(StrTbl[11]);//reverseServo1
            StrSplitRestore(",", StrTbl, SeparFound);//Imperatif SeparFound <= SUB_STRING_NB_MAX
            EEPROM.put(0,ms);        
            blinkNTime(5,100,100);              
#ifdef DEBUG
            SettingsPort << F("New settings S1: ") << endl;
#endif
        }
        else if (checkMess == "S2")
        {
            ms.reverseServo2   = atoi(StrTbl[1]);//reverseServo2
            ms.diffVitesseErr  = atoi(StrTbl[2]);//diffVitesseErr
            ms.nbPales         = atoi(StrTbl[3]);//nbPales
            ms.channelsOrder   = atoi(StrTbl[4]);//AETR,AERT,ARET ...
            ms.moduleMasterOrSlave = atoi(StrTbl[5]);//moduleMasterOrSlave
            ms.fahrenheitDegrees = atoi(StrTbl[6]);//fahrenheitDegrees
            ms.minimumSpeed    = atoi(StrTbl[7]);//minimum motor rpm
            ms.maximumSpeed    = atoi(StrTbl[8]);//maximum motor rpm
            ms.InputMode       = atoi(StrTbl[9]);//CPPM,SBUS or IBUS
            ms.coeff_division  = atof(StrTbl[10]);//coeff_division external battery
            ms.telemetryInUse   = atoi(StrTbl[11]);//0=no , 1=yes
            StrSplitRestore(",", StrTbl, SeparFound);//Imperatif SeparFound <= SUB_STRING_NB_MAX
            EEPROM.put(0,ms);        
            blinkNTime(5,100,100);            
#ifdef DEBUG
            SettingsPort << F("New settings S2: ") << endl;           
#endif
        }
        else if (checkMess == "S3")// simulation on
        {
          simulateSpeed = true;
          vitesse1    = atoi(StrTbl[1]);
          vitesse2    = atoi(StrTbl[2]);
          diffVitesse = atoi(StrTbl[3]);
        }
        else if (checkMess == "S4")// simulation off
        {
          simulateSpeed = false;
          vitesse1    = 0;
          vitesse2    = 0;
          diffVitesse = 100;         
        }
        
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
