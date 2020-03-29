// Definition des modes:
void mode0()//run mode
{   
  readCaptorTransitions();/* read sensors */

//#ifdef SERIALPLOTTER /* You can read the battery voltage, speeds and throttle and auxiliary pulses with the Serial Plotter (IDE >= 1.6.6) */
//   Serial << ((GetExternalVoltage()>1)?GetExternalVoltage()*1000:0) << "," << Width_us << "," << WidthAux_us << "," << vitesse1 << "," << vitesse2 << endl;
//#endif

  /* Receiver pulse acquisition and command of 2 servos */
  //1.Direction, 2.Profondeur, 3.Moteur, 4.Ailerons
  if (ms.InputMode == 0)//PPM
  {
    if (TinyPpmReader.isSynchro())
    {
      InputSignalExist = true;
      Width_us    = TinyPpmReader.width_us(ms.MotorNbChannel);//AVERAGE(Width_us,RxChannelPulseMotor.width_us(), responseTime);/* average */
      WidthAux_us = TinyPpmReader.width_us(ms.AuxiliaryNbChannel);
      WidthRud_us = TinyPpmReader.width_us(ms.RudderNbChannel);
      WidthAil_us = TinyPpmReader.width_us(ms.AileronNbChannel);      
    }
  }
  else if (ms.InputMode == 1)//SBUS
  {
    recupSbusdata();// conversion des données SBUS pour les 16 Voies
    while ( Serial.available() ) { // tant qu'un octet arrive sur le SBUS
      int val = Serial.read();  // lecture de l'octet
      if  (( memread == 0 ) and ( val == 15)) { // detection de la fin et debut de la trame SBUS (une trame fini par 0 et commence par 15)
        cpt = 0; // remise a zero du compteur dans la trame
      }
      memread = val; // memorisation de la dernière valeur reçu
      buf[cpt] = val; // stock la valeur reçu dans le buffer
      cpt +=1;        // incrémente le compteur
      if (cpt == 26) {cpt=0;} // au cas ou on aurait pas reçu les caractères de synchro on reset le compteur
    } // fin du while

    Width_us    = map(voie[ms.MotorNbChannel], -100, +100, 900, 2100);
    WidthAux_us = map(voie[ms.AuxiliaryNbChannel], -100, +100, 900, 2100);
    WidthRud_us = map(voie[ms.RudderNbChannel], -100, +100, 900, 2100);
    WidthAil_us = map(voie[ms.AileronNbChannel], -100, +100, 900, 2100);
  }
  else if (ms.InputMode == 2)//IBUS
  {
    IBus.loop();
    if (IBus.readChannel(0) > 0)
    {
      InputSignalExist = true;
      Width_us    = IBus.readChannel(ms.MotorNbChannel-1);
      WidthAux_us = IBus.readChannel(ms.AuxiliaryNbChannel-1);
      WidthRud_us = IBus.readChannel(ms.RudderNbChannel-1);
      WidthAil_us = IBus.readChannel(ms.AileronNbChannel-1);
    }
  }
  else
  {
    InputSignalExist = false;
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
        PIN_HIGH(B,0);PIN_HIGH(B,1);waitMs(20);PIN_LOW(B,0);PIN_LOW(B,1); //led green flash one time each second
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
    /*Si aucun signal au demarrage, les helices sont bloquees avec "Width_us=1000")*/
    /* Check for pulse motor extinction */
    if(millis() - BeginChronoServoMs >= 30 && simulateSpeed == false)//30ms
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
  if( MsgDisponible() >= 0) //MsgDisponible() retourne la longueur du message recu; le message est disponible dans Message
  {
      //Serial << millis() << endl;  
      SecurityIsON = false;
      static uint16_t posInUs;
     
      pos = atoi(Message); //conversion Message ASCII vers Entier (ATOI= Ascii TO Integer)
      if( (pos >= 0) && (pos <= 180))//reception curseur VB 'servos'
      { 
        //if(pos<=1) {pos=2;} //Servo making noise at 0 and 1. Need to be at least 2.
        //if (!RxChannelPulseMotor.available() && simulateSpeed == true)//verifie si Ch Moteur est inactif !!!
        if (!TinyPpmReader.isSynchro() && simulateSpeed == true)//verifie si Ch Moteur est inactif !!!
        {
          PIN_TOGGLE(B,0);
          posInUs = map(pos, 0, 180, ms.minimumPulse_US, ms.maximumPulse_US);
          if (ms.reverseServo1 == 0) {ServoMotor1.write_us(posInUs);}else{ServoMotor1.write_us((ms.centerposServo1*2)-posInUs);}               
          if (ms.reverseServo2 == 0) {ServoMotor2.write_us(posInUs);}else{ServoMotor2.write_us((ms.centerposServo2*2)-posInUs);}
          waitMs(5);
          SoftRcPulseOut::refresh(1);      // generates the servo pulse
        } 
      }
      else if( (pos >= ms.minimumPulse_US) && (pos <=ms.maximumPulse_US))//reception VB en uS 'servos'
      {        
        //if (!RxChannelPulseMotor.available() && simulateSpeed == true)//verifie si Ch Moteur est inactif !!!
        if (!TinyPpmReader.isSynchro() && simulateSpeed == true)//verifie si Ch Moteur est inactif !!!
        {
          PIN_TOGGLE(B,0);
          //posInUs = map(pos,0,180,minimumPulse_US,maximumPulse_US);
          if (ms.reverseServo1 == 0) {ServoMotor1.write_us(pos);}else{ServoMotor1.write_us((ms.centerposServo1*2)-pos);}               
          if (ms.reverseServo2 == 0) {ServoMotor2.write_us(pos);}else{ServoMotor2.write_us((ms.centerposServo2*2)-pos);}
          waitMs(5);
          SoftRcPulseOut::refresh(1);      // generates the servo pulse
        } 
      }      
//      else if( (pos >= 181) && (pos <= 360))//reception curseur VB 'Auxiliaire' ou 'Direction'
//      { 
//        if(!RxChannelPulseAux.available() && simulateSpeed == true)//verifie si Ch Aux est inactif !!!
//        {
//          posInUs = map(pos,181,360,minimumPulse_US,maximumPulse_US);
////          if (reverseServo1 == 0) {ServoMotor1.write_us(posInUs);}else{ServoMotor1.write_us((centerposServo1*2)-posInUs);}               
////          if (reverseServo2 == 0) {ServoMotor2.write_us(posInUs);}else{ServoMotor2.write_us((centerposServo2*2)-posInUs);}  
////          waitMs(5);                       // waits 5ms for for refresh period 
////          SoftRcPulseOut::refresh(1);      // generates the servo pulse
//        }          
//      }        
      else if(pos == 363)//envoie settings au port serie
      {//format send: 1657|1657|1225|1225|2|2075|1393|1070|2205|1|0|0|100|39|2|0|5.00|27.61|1.000|0|1000|20000|ki|kp|kd 
        sendConfigToSerial();      
      } 
      
      //else if(countInMessage==18)
      else if(CountChar(Message,',')==18)//reception settings from VB
      {//format received: 1500,1500,1000,1000,2,2000,1250,1200,1900,1,0,0,99.00,2,0,0,0,1000,20000
        StrSplit(Message, ",",  StrTbl, SUB_STRING_NB_MAX, &SeparFound);     
        ms.centerposServo1 = atoi(StrTbl[0]);//EEPROMWrite(1, atoi(StrTbl[0]));//centerposServo1
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
        ms.diffVitesseErr  = atoi(StrTbl[12]);//diffVitesseErr (42 used because 21 return error !)
        ms.nbPales         = atoi(StrTbl[13]);//nbPales
        //EEPROM.update(29, atoi(StrTbl[14]));//LCD adresse
        ms.moduleMasterOrSlave = atoi(StrTbl[15]);//moduleMasterOrSlave
        ms.fahrenheitDegrees = atoi(StrTbl[16]);//fahrenheitDegrees
        ms.minimumSpeed    = atoi(StrTbl[17]);//minimum motor rpm
        ms.maximumSpeed    = atoi(StrTbl[18]);//maximum motor rpm (need 37 to 40)
        StrSplitRestore(",", StrTbl, SeparFound);//Imperatif SeparFound <= SUB_STRING_NB_MAX
        EEPROM.put(0,ms);
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
      else if(pos == 366)//run or stop speed read
      {  
        Serial << F("V") << vitesse1 << F("|") << vitesse2 << endl;/* Send speed to VB interface */
      } 
      else if(pos == 367)//lecture de la position du canal auxiliaire:
      {        
        Serial << F("A") << WidthAux_us << endl;
      }
      else if(pos == 368)//lecture voltage arduino, temp arduino et voltage batterie RX:
      {        
        Serial << F("STS") << _FLOAT(5/1000,3) << F("|");//Serial << F("STS") << _FLOAT(readVcc()/1000,3) << F("|");
        Serial << 0 << F("|");//Serial << GetTemp() << F("|");
#ifdef EXTERNALVBATT
        Serial <<  _FLOAT(GetExternalVoltage(),3) << endl;
#else
        Serial <<  F("NOTUSED|") << endl;
#endif
      }
      else if(pos == 369)//On/Off Glow Plug command
      { 
        if (glowControlIsActive==false)
        {
          PIN_HIGH(C,6);
          glowControlIsActive=true;
        }
        else
        { 
          PIN_LOW(C,6);
          glowControlIsActive=false;        
        }
      }
      else if(pos == 370)//erase EEprom
      { 
        clearEEprom();
      }

      else if(CountChar(Message,',') == 3)//check XXX,A,B,C 
      {
        StrSplit(Message, ",",  StrTbl, SUB_STRING_NB_MAX, &SeparFound);
        switch (atoi(StrTbl[0]))
        {
#ifdef SDDATALOGGER
          case 886://886,x,y1,y2         
            writeSDdatalog(atoi(StrTbl[2]),atoi(StrTbl[3])); // OK avec VB
            break;
          case 890://890,fileName,0,0         
            //dumpSDdatalog(StrTbl[1]); // Pas OK avec VB
            break;
          case 891://891,fileName,0,0         
            deleteSDdatalog(StrTbl[1]); // OK avec VB
            break;            
#endif 
#ifdef PIDCONTROL           
          case 887://887,Kp,Ki,Kd
            ms.consKp = strtod(StrTbl[1],NULL);
            ms.consKi = strtod(StrTbl[2],NULL);
            ms.consKd = strtod(StrTbl[3],NULL);
            //Serial << StrTbl[1] << F("|") << StrTbl[2] << F("|") << StrTbl[3] << endl;
            //EEPROMWriteDouble(50, diffVitesseErr);
//            EEPROMWrite(58, consKp);
//            EEPROMWrite(66, consKi);
//            EEPROMWrite(74, consKd);
            EEPROM.put(0,ms);
            ledFlashSaveInEEProm(5);
            break;
#endif
          case 888://888,v1,v2,consigne
            simulateSpeed = true;
            vitesse1 = atoi(StrTbl[1]);
            vitesse2 = atoi(StrTbl[2]);
            ms.diffVitesseErr = atoi(StrTbl[3]);
            ledFlashSaveInEEProm(5);
            break;                                 
        }
        StrSplitRestore(",", StrTbl, SeparFound);//Imperatif SeparFound <= SUB_STRING_NB_MAX
        Serial.flush(); // clear serial port
      } 
#ifdef SDDATALOGGER           
      else if(pos == 371)//read SD Card Infos
      {
        readSDCardInfos(); //OK avec VB
      }
      else if(pos == 372)//start/stop log file on SD Card
      {
        RunLogInSDCard = !RunLogInSDCard;
        //(RunLogInSDCard)?Serial << F("LogOn") <<endl:Serial << F("LogOf") <<endl;
      }
      else if(pos == 373)//read log file from SD Card
      { 
        listSDdatalog(); //OK avec VB
      }
#else
      else if(pos == 371)//read SD Card Infos
      {
        Serial << F("SDNO") <<endl;
      } 
#endif
      else if (pos == 889)
      {
        simulateSpeed = false;                   
      }                    
      else if(pos == 998)//send if V+ external is used
      {
#ifdef EXTERNALVBATT         
        Serial << F("POWER|1") << endl;
#else
        Serial << F("POWER|0") << endl;
#endif
      }  
      else if(pos == 999)//send Module Version
      { 
        Serial << F("FIRM|") << FirmwareVersion << endl;
      }            
      readAuxiliaryChannel();//read Auxiliary channel and his modes (1 to 6)
    }  

}//end SerialFromToVB()

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

void recupSbusdata(void){
// récuperation des données SBUS et conversion dans le tableau des voies
// les données SBUS sont sur 8 bits et les données des voies sont sur 11 bits
// il faut donc jouer a cheval sur les octets pour calculer les voies.

  voie[1]  = ((buf[1]|buf[2]<< 8) & 0x07FF);
  voie[2]  = ((buf[2]>>3|buf[3]<<5) & 0x07FF);
  voie[3]  = ((buf[3]>>6|buf[4]<<2|buf[5]<<10) & 0x07FF);
  voie[4]  = ((buf[5]>>1|buf[6]<<7) & 0x07FF);
  voie[5]  = ((buf[6]>>4|buf[7]<<4) & 0x07FF);
  voie[6]  = ((buf[7]>>7|buf[8]<<1|buf[9]<<9) & 0x07FF);
  voie[7]  = ((buf[9]>>2|buf[10]<<6) & 0x07FF);
  voie[8]  = ((buf[10]>>5|buf[11]<<3) & 0x07FF);
 
  voie[9]  = ((buf[12]|buf[13]<< 8) & 0x07FF);
  voie[10]  = ((buf[13]>>3|buf[14]<<5) & 0x07FF);
  voie[11] = ((buf[14]>>6|buf[15]<<2|buf[16]<<10) & 0x07FF);
  voie[12] = ((buf[16]>>1|buf[17]<<7) & 0x07FF);
  voie[13] = ((buf[17]>>4|buf[18]<<4) & 0x07FF);
  voie[14] = ((buf[18]>>7|buf[19]<<1|buf[20]<<9) & 0x07FF);
  voie[15] = ((buf[20]>>2|buf[21]<<6) & 0x07FF);
  voie[16] = ((buf[21]>>5|buf[22]<<3) & 0x07FF);
 
  ((buf[23]) & 1 )      ? voie[17] = 2047 : voie[17] = 0 ;
  ((buf[23] >> 1) & 1 ) ? voie[18] = 2047 : voie[17] = 0 ;
 
  // detection du failsafe
  if ((buf[23] >> 3) & 1) {
    voie[0] = 0; // Failsafe
  }
  else
  {
    voie[0] = 1; // Normal
    InputSignalExist = true;
  }
  for(int x = 1; x<19 ; x++)
  {
    voie[x]= (lround(voie[x]/9.92) - 100);
  }
}
