#ifdef LCDON

/* ********************************************************************* */
void LCDML_DISP_setup(LCDML_FUNC_back)
/* ********************************************************************* */
{ 
  // setup function 
  LCDML_DISP_funcend();  
}
void LCDML_DISP_loop(LCDML_FUNC_back) 
{
}
void LCDML_DISP_loop_end(LCDML_FUNC_back) 
{
  LCDML.goBack(); // go one layer back
}

/* ********************************************************************* */
void LCDML_DISP_setup(LCDML_FUNC_root)
/* ********************************************************************* */
{ 
  // setup function 
  LCDML_DISP_funcend();  
}
void LCDML_DISP_loop(LCDML_FUNC_root) 
{
}
void LCDML_DISP_loop_end(LCDML_FUNC_root) 
{
  LCDML.goRoot(); // go one layer back
}

///* ********************************************************************* */
//void LCDML_DISP_setup(LCDML_FUNC_initscreen)
///* ********************************************************************* */
//{
//  // setup function
//  lcd.clear();
//}
//void LCDML_DISP_loop(LCDML_FUNC_initscreen) 
//{ 
//  if(LCDML_BUTTON_checkAny()) { // check if any button is pressed to left this function
//    LCDML_DISP_funcend(); // function end    
//  }   
//}
//void LCDML_DISP_loop_end(LCDML_FUNC_initscreen) 
//{  
//  LCDML.goRoot(); // go to root element (first element of this menu with id=0)
//}

///* ********************************************************************* */
//void LCDML_DISP_setup(LCDML_Greetings)
///* ********************************************************************* */
//{
//  // setup function 
//  lcd.clear();
//  lcd.setCursor(5,0);lcd.print(F("Powered by"));
//  lcd.setCursor(0,1);lcd.print(F("********************"));
//  lcd.setCursor(0,2);lcd.print(F("* Async. Lib. V1.0 *"));//lcd.print(SoftRcPulseIn::LibVersion());
////  lcd.print(".");lcd.print(SoftRcPulseIn::LibRevision());lcd.print(" *");
//  lcd.setCursor(0,3);lcd.print(F("* by P. LOUSSOUARN *")); 
//}
//void LCDML_DISP_loop(LCDML_Greetings) 
//{
//  if(LCDML_BUTTON_checkAny()) 
//  { 
//    LCDML_DISP_funcend();
//  } 
//}
//void LCDML_DISP_loop_end(LCDML_Greetings)
//{
//} 

/* ********************************************************************* */
void LCDML_DISP_setup(LCDML_ServosTest)
/* ********************************************************************* */
{
  // setup function 
  lcd.clear();
  lcd.setCursor(2,0);lcd.print(F("!! Servo Test !!"));
  LCDML_DISP_triggerMenu(200); // starts a trigger event for the loop function every 200 millisecounds   
}
void LCDML_DISP_loop(LCDML_ServosTest) 
{
  modeServosTest();
  
  if(LCDML_BUTTON_checkAny()) // check if any button is presed (enter, up, down, left, right)
  { 
    // LCDML_DISP_funcend calls the loop_end function
    LCDML_DISP_funcend();
  } 
}
void LCDML_DISP_loop_end(LCDML_ServosTest)
{
}

/* ********************************************************************* */
void LCDML_DISP_setup(LCDML_intTempExtVoltage)
/* ********************************************************************* */
{
  // setup function 
  lcd.clear();
  intBarGraph_Characters();
  LCDML_DISP_triggerMenu(200); // starts a trigger event for the loop function every 100 millisecounds  
}
void LCDML_DISP_loop(LCDML_intTempExtVoltage) 
{
  lcd.clear();
  lcd.setCursor(0, 0);lcd.print(F("Int. Temp:"));lcd.setCursor(11, 0);lcd.print(GetTemp(), 1);lcd.print(F("\x06"));lcd.print((EEPROM.read(33)==0)?"C":"F"); 
  drawBars(GetTemp(), 1, 18, '-', '+', -50, 50);//size of the bar from -50°C to +50°C  
#ifdef EXTERNALVBATT
  lcd.setCursor(0, 2);lcd.print(F("Ext. VBat:"));lcd.setCursor(11, 2);lcd.print(GetExternalVoltage());lcd.print(F("V"));
  drawBars(GetExternalVoltage(), 3, 18, '-', '+', 0, 20);//size of the bar from 0v to +20v
#endif    
  if(LCDML_BUTTON_checkAny()) // check if any button is presed (enter, up, down, left, right)
  { 
    // LCDML_DISP_funcend calls the loop_end function
    LCDML_DISP_funcend();
  } 
}
void LCDML_DISP_loop_end(LCDML_intTempExtVoltage)
{
  LCDML.scrollbarInit();//reset vertical scrollbar 
}


/* ********************************************************************* */
void LCDML_DISP_setup(LCDML_ResetSettings)
/* ********************************************************************* */
{
  // setup function 
  lcd.clear();
  lcd.setCursor(2,0);lcd.print(F("!! Rst EEprom !!"));
  lcd.setCursor(2,2);lcd.print(F("Default Settings"));
  lcd.setCursor(2,3);lcd.print(F("are loaded !!!"));
  saveSettings=false;  
}
void LCDML_DISP_loop(LCDML_ResetSettings) 
{
  if (LCDML_BUTTON_checkAny()) // check if any button is pressed (enter, up, down, left, right)
  {
    if (LCDML_BUTTON_checkEnter()) // check if button 'enter' is pressed one time
    {
      LCDML_BUTTON_resetEnter();
      if(saveSettings==false)
      { 
        lcd.clear();
        lcd.setCursor(2,0);lcd.print(F("!! Rst EEprom !!"));
        lcd.setCursor(2,1);lcd.print(F("Are you certain ?"));
        lcd.setCursor(0,3);lcd.print(F("Down[No]  Enter[Yes]")); 
        saveSettings = true;
      }
      else
      {
        lcd.clear();
        lcd.setCursor(2,0);lcd.print(F("!! Rst EEprom !!"));
        lcd.setCursor(7,2);lcd.print(F(" Done !!!"));
        saveSettings = false;
//        clearEEprom();SettingsWriteDefault();
        waitMs(2000UL);//delay(2000); 
        LCDML_DISP_funcend();       
      }      
    }
    if (LCDML_BUTTON_checkDown())
    {
      LCDML_BUTTON_resetDown();
      lcd.clear();
//      lcd.setCursor(2,0);lcd.print(F("!! Rst EEprom !!"));
      lcd.setCursor(6,2);lcd.print(F(" Abort !!!"));
      saveSettings = false;
      waitMs(2000UL);//delay(2000); 
      LCDML_DISP_funcend();       
    }    
  }
}
void LCDML_DISP_loop_end(LCDML_ResetSettings)
{
  saveSettings = false; 
}

/* ********************************************************************* */
void LCDML_DISP_setup(LCDML_Recorder)
/* ********************************************************************* */
{
  // setup function 
  lcd.clear();
  LCDML_DISP_triggerMenu(100); // starts a trigger event for the loop function every 100 millisecounds
  lcd.setCursor(0,0);lcd.print(F("Ready..."));  
  switch(LCDML.getCursorPos())
  {
    case 0:      
      lcd.setCursor(0,2);lcd.print(F("Enter[Play/Stop]"));
      break;
    case 1:
      lcd.setCursor(0,2);lcd.print(F("Enter[Rec/Stop]"));
      break;
  } 
  intBarGraph_Characters();
#ifdef RECORDER
  setupRecorder();
#endif  
}
void LCDML_DISP_loop(LCDML_Recorder) 
{ 

#ifdef RECORDER  
  loopRecorder();//  Serial << mode << endl;
#endif

}
void LCDML_DISP_loop_end(LCDML_Recorder)
{
  LCDML.scrollbarInit();//reset vertical scrollbar  
}

/* ********************************************************************* */
void LCDML_DISP_setup(LCDML_FUNC_goRoot)
/* ********************************************************************* */
{
  // setup function 
  LCDML_DISP_groupEnable(_LCDML_G1);  // enable all menu items in G1
  LCDML_DISP_funcend();  
}
void LCDML_DISP_loop(LCDML_FUNC_goRoot) 
{
}
void LCDML_DISP_loop_end(LCDML_FUNC_goRoot) 
{
  LCDML.goRoot(); // go to root element (first element of this menu with id=0)
}


/* ********************************************************************* */
uint8_t g_func_timer_info = 0;  // time counter (global variable)
unsigned long g_timer_1 = 0;    // timer variable (globale variable)
void LCDML_DISP_setup(LCDML_mode1)
/* ********************************************************************* */
{
  // setup function 
  lcd.clear();
  lcd.print(F("Min/Max general"));
  g_func_timer_info = 10; // reset and set timer  
  lcd.setCursor(0,1);lcd.print(F("[  Move Throttle   ]"));  
  intBarGraph_Characters();
  LCDML_DISP_triggerMenu(100);
  saveSettings = false;
}
void LCDML_DISP_loop(LCDML_mode1) 
{
  if(saveSettings==false) buildBarGraphMotorThrottlePosition(Width_us);  
  newValue = Width_us;
  if (LCDML_BUTTON_checkAny()) // check if any button is pressed (enter, up, down, left, right)
  {
    if (LCDML_BUTTON_checkEnter()) // check if button 'enter' is pressed one time
    {
      lcd.clear();
      LCDML_BUTTON_resetEnter();
      if(saveSettings==false)
      {      
        lcd.setCursor(2,1);lcd.print(F("Are you certain ?"));
        lcd.setCursor(0,3);lcd.print(F("Down[No]  Enter[Yes]")); 
        saveSettings = true;
      }
      else
      {
        lcd.clear();
        lcd.print(F("Min/Max general"));radioSetupMinMaxPulse();
        EEPROMWriteInt(23, minimumPulse_US);EEPROMWriteInt(25, maximumPulse_US);
        lcd.setCursor(0,1);lcd.print(F("   !!! Saved !!!    ")); 
        lcd.setCursor(0,2);lcd.print(F("New Min is: "));lcd.print(EEPROMReadInt(23));
        lcd.setCursor(16,3);lcd.print(F("   "));
        lcd.setCursor(0,3);lcd.print(F("New Max is: "));lcd.print(EEPROMReadInt(25));       
        waitMs(2000);
        lcd.clear();
        
        /* Center 1&2, Idle 1&2, Full Throttle, Begin Synchro are deduceted from Min and Max */
        centerposServo1 = THROTTLE_FRACT(minimumPulse_US, maximumPulse_US, 1, 2);EEPROMWriteInt(1, centerposServo1); /* 1/2 */
        centerposServo2 = THROTTLE_FRACT(minimumPulse_US, maximumPulse_US, 1, 2);EEPROMWriteInt(3, centerposServo2); /* 1/2 */
        fullThrottle = maximumPulse_US - 140;EEPROMWriteInt(11, fullThrottle);        //mode1 (in address 11&12)
        beginSynchro = maximumPulse_US - 800;EEPROMWriteInt(13, beginSynchro);        //mode1 (in address 13&14)
        idelposServos1 = maximumPulse_US - 990;EEPROMWriteInt(5, idelposServos1);     //mode1 (in address 5&6)
        idelposServos2 = maximumPulse_US - 990;EEPROMWriteInt(7, idelposServos2);     //mode1 (in address 7&8)
        lcd.setCursor(0,0);lcd.print(F("Center Servo 1&2"));lcd.setCursor(0,1);lcd.print(centerposServo1);waitMs(2000);
        lcd.setCursor(0,0);lcd.print(F("Idle Position 1&2"));lcd.setCursor(0,1);lcd.print(idelposServos1);waitMs(2000);
        lcd.setCursor(0,0);lcd.print(F("Full throttle"));lcd.setCursor(0,1);lcd.print(fullThrottle);waitMs(2000);
        lcd.setCursor(0,0);lcd.print(F("Begin synchro"));lcd.setCursor(0,1);lcd.print(beginSynchro);waitMs(2000);
        
        ledFlashSaveInEEProm(20);

        waitMs(2000);
        LCDML.scrollbarInit();//reset vertical scrollbar
        LCDML_DISP_funcend();       
      }      
    }
    if (LCDML_BUTTON_checkDown())
    {
      LCDML_BUTTON_resetDown();
      lcd.clear();
      lcd.setCursor(6,2);lcd.print(F(" Abort !!!"));
      saveSettings = false;
      waitMs(2000UL);//delay(2000); 
      LCDML_DISP_funcend();       
    }    
  }
}
void LCDML_DISP_loop_end(LCDML_mode1)
{
  LCDML.scrollbarInit();//reset vertical scrollbar
}


/* ********************************************************************* */
void LCDML_DISP_setup(LCDML_mode2)
/* ********************************************************************* */
{
  // setup function 
  lcd.clear();
  switch (LCDML.getCursorPos())
  {
    case 1:lcd.print(F("Response time"));newMinValue=0; newMaxValue=5;break;
    case 2:lcd.print(F("Aux. chanel"));newMinValue=1; newMaxValue=6;break;
    case 3:lcd.print(F("Nb of blades"));newMinValue=1; newMaxValue=5;break;
    case 4:lcd.print(F("Reverse servo 1&2"));newMinValue=0; newMaxValue=1;break;
  }
  if(LCDML.getCursorPos()==8)
  {
    lcd.createChar(7, charbackslash);lcd.setCursor(15,0);readAuxTypeLCD();
  }  
  lcd.setCursor(0,1);lcd.print(F("[  Move Throttle   ]"));   
  intBarGraph_Characters();
  LCDML_DISP_triggerMenu(200);
  saveSettings = false;
}
void LCDML_DISP_loop(LCDML_mode2) 
{

  static uint16_t motorPositionValue;
  if(LCDML.getCursorPos()==8)
  {
    lcd.createChar(7, charbackslash);lcd.setCursor(15,0);readAuxTypeLCD();
  }
  motorPositionValue  = (Width_us<minimumPulse_US)?minimumPulse_US:Width_us;//empeche 'Width_us<minimumPulse_US'
  if(saveSettings==false) buildBarGraphMotorThrottlePosition(motorPositionValue);
  newValue = map( motorPositionValue, minimumPulse_US, maximumPulse_US, newMinValue, newMaxValue );
//  if(millis() - BeginChronoLcdMs >= 1000)//lets not saturate the loop() and makes the movement of servos more fluid
//  { 
//    Serial << "NewValue:" << newValue << "| Motorpos: " << motorPositionValue<< " | Max: " << minimumPulse_US << " | Min: " << maximumPulse_US << " | AuxMin: " <<newMinValue << " | AuxMax: " << newMaxValue <<endl;     
//    BeginChronoLcdMs=millis(); /* Restart the Chrono for the LCD */
//  }
  lcd.setCursor(13,2);lcd.print(F("-->"));lcd.setCursor(17,2);lcd.print(F("["));lcd.print(newValue);lcd.print(F("]")); 
  if (LCDML_BUTTON_checkAny()) // check if any button is pressed (enter, up, down, left, right)
  {
    if (LCDML_BUTTON_checkEnter()) // check if button 'enter' is pressed one time
    {
      lcd.clear();
      LCDML_BUTTON_resetEnter();
      if(saveSettings==false)
      {         
        lcd.setCursor(2,1);lcd.print(F("Are you certain ?"));
        lcd.setCursor(0,3);lcd.print(F("Down[No]  Enter[Yes]")); 
        saveSettings = true;
      }
      else
      {
        lcd.clear();
        switch (LCDML.getCursorPos())
        {
          case 1:lcd.print(F("Response time"));EEPROM.write(9, newValue);break;
          case 2:lcd.print(F("Aux. chanel"));EEPROM.write(15, newValue);break;
          case 3:lcd.print(F("Nb of blades"));EEPROM.write(27, newValue);break;
          case 4:lcd.print(F("Reverse servo 1&2"));EEPROM.write(17, newValue);break;
        }
        lcd.setCursor(0,1);lcd.print(F("   !!! Saved !!!    "));  
        lcd.setCursor(0,2);lcd.print(F("New value is: "));lcd.print(newValue);

        ledFlashSaveInEEProm(20);

        waitMs(2000);
        LCDML.scrollbarInit();//reset vertical scrollbar
        LCDML_DISP_funcend();       
      }      
    }
    if (LCDML_BUTTON_checkDown())
    {
      LCDML_BUTTON_resetDown();
      lcd.clear();
      lcd.setCursor(6,2);lcd.print(F(" Abort !!!"));
      saveSettings = false;
      waitMs(2000UL);//delay(2000); 
      LCDML_DISP_funcend();       
    }    
  }

}
void LCDML_DISP_loop_end(LCDML_mode2)
{
  LCDML.scrollbarInit();//reset vertical scrollbar
}


#define CHAR_WIDTH 5//number of pixel in x for one character
void drawBars ( int value, uint8_t row, uint8_t barLength, char charStart, char charEnd , int minvalue, int maxvalue)
{
   int numBars;
   lcd.setCursor (0, row);
   lcd.print (charStart);   
   value = map ( value, minvalue, maxvalue, 0, ( barLength ) * CHAR_WIDTH );
   numBars = value / CHAR_WIDTH;   
   // Limit the size of the bargraph to barLength
   if ( numBars > barLength )
   {
     numBars = barLength;
   }
   lcd.setCursor ( 1, row );   
   // Draw the bars
   while ( numBars-- )
   {
      lcd.print ( char( 5 ) );
   }   
   // Draw the fractions
   numBars = value % CHAR_WIDTH;
   lcd.print ( char(numBars) );
   lcd.setCursor (barLength + 1, row);
   lcd.print (charEnd);  
}

void intBarGraph_Characters()
{
  //setup bargraph characters
  uint8_t charBarGraphSize = (sizeof(charBarGraph ) / sizeof (charBarGraph[0]));
  for ( uint8_t i = 0; i < charBarGraphSize; i++ )
  {
#ifdef BARGRAPHINPROGMEM
    strcpy_P(buff, (char*)pgm_read_word(&(charBarGraph[i])));//read bargraph characters from PROGMEM
    lcd.createChar ( i, (uint8_t *)buff );
#else
    lcd.createChar ( i, (uint8_t *)charBarGraph[i] );//read bargraph characters from standard memory
#endif
  }

}

/*build motor throttle position and build bargraph for settings */
void buildBarGraphMotorThrottlePosition(uint16_t motorPositionValue)
{
  motorPositionValue  = (Width_us<minimumPulse_US)?minimumPulse_US:Width_us;//empeche 'Width_us<minimumPulse_US'
  lcd.setCursor(0,2);lcd.print(F("Motor: "));lcd.print(Width_us);lcd.print(F("uS"));
  //Serial.print(Width_us);Serial.print("|");Serial.print(minimumPulse_US);Serial.print("|");Serial.print(maximumPulse_US);
  lcd.setCursor(1,3);lcd.print("                  ")  ;
  drawBars(motorPositionValue, 3, 18, '-', '+', minimumPulse_US, maximumPulse_US);//size of the bar from 'minimumPulse_US' to 'maximumPulse_US'
}

#endif//end LCDON

#ifdef CHECK_I2C_LCDAdress
/*================================================================= 
                      Scanner I2C
* Pin SCL de l’Arduino (A4)
* Pin SDA de l’Arduino (A5)
================================================================ */
boolean checkI2CAddress()
{
  Wire.begin();
  uint8_t error, address;//, nDevices;
//  Serial << F("Recherche I2C en cours...") << endl;
  nDevices = 0;
  for(address = 1; address < 127; address++ ) 
  {
    Wire.beginTransmission(address);
    error = Wire.endTransmission();
    if (error == 0)
    {
//      Serial << F("Adresse I2C definie dans EEProm ") << lcdI2Caddress << F(" (0x") << decToHex(lcdI2Caddress,2) << F(")") << endl;
//      Serial << F("Equiment I2C trouve a l'addresse ");
      if (address<16)
      {
//        Serial << address << F(" (0x") << decToHex(address,2) << F(")  !") << endl;
      }
      newlcdI2Caddress = address;
//      Serial << address << F(" (0x") << decToHex(address,2) << F(")  !") << endl;
      if (lcdI2Caddress != newlcdI2Caddress)
      {        
        EEPROMWriteInt(29, newlcdI2Caddress);//sauvegarde nouvelle adresse I2C trouvee
//        Serial << F("Adresse sauvegardee dans EEprom") << endl;
        lcd.setCursor(2,1);lcd.print(F("LCD Add. is BAD! "));
        lcd.setCursor(2,2);lcd.print(F("Old.: "));lcd.print(lcdI2Caddress);lcd.print(F(" (0x"));lcd.print(decToHex(lcdI2Caddress,2));lcd.print(F(")"));
        lcd.setCursor(2,3);lcd.print(F("New.: "));lcd.print(newlcdI2Caddress);lcd.print(F(" (0x"));lcd.print(decToHex(newlcdI2Caddress,2));lcd.print(F(")"));
      }
      else
      {
//        Serial << F("Adresse dans EEprom est OK") << endl;
        lcd.setCursor(2,1);lcd.print(F("LCD Add. is OK! "));
        lcd.setCursor(2,2);lcd.print(F("Add.: "));lcd.print(lcdI2Caddress);lcd.print(F(" (0x"));lcd.print(decToHex(lcdI2Caddress,2));lcd.print(F(")"));
      }     
//      nDevices++;
    }
  }
  return true;
}


int decToHex(byte decValue, byte desiredStringLength) 
{
  String hexString = String(decValue, HEX);
//  while (hexString.length() < desiredStringLength) hexString = "0" + hexString;
  return hexString.toInt();
}
#endif//end CHECK_I2C_LCDAdress

