/* SyncTwinRcEngine */
/* Compile only with IDE version '1.0.6' or '1.5.7 beta' or '1.5.8 beta' or '1.6.0' to '1.8.1' */

#include <EEPROM.h>
#include <Streaming.h>          /* Librairie remplacant Serial.print() */
#include <SoftRcPulseIn.h>      /* Librairie utilisee pour lire un signal pwm en entree */
#include <SoftRcPulseOut.h>     /* Librairie utilisee pour creer un signal pwm en sortie */
//#include <TinyPinChange.h>      /* Permet detecter en interruption les changements d'etat */
#include "Macros.h"
#include <util/atomic.h>

/* MAIN SETTINGS */
//#define CHECK_I2C_LCDAdress     /* Uncomment for check I2C address */
//#define READ_Button_AnalogPin   /* Uncomment for check analog buttons (!!!!!! comment ARDUINO2PC if you want read values on serial window !!!!!!) */
/* MAIN SETTINGS */

//#define ISI2CSLAVE              /* PLEASE, COMMENT LCDON, DEBUG, ARDUINO2PC, EXTERNALVBATT, RECORDER and RETURNTOMENU IF MODULE IS SLAVE */
//#define LCDON                   /* LCD Menu */
//#define DEBUG
//#define SERIALPLOTTER           /* Multi plot in IDE (don't use this option with ARDUINO2PC) */
#define ARDUINO2PC              /* PC interface (!!!!!! don't use this option with SERIALPLOTTER or READ_Button_AnalogPin !!!!!!) */
#define EXTERNALVBATT           /* Read external battery voltage */
#define RECORDER                /* Recorder/Player */
#define GLOWMANAGER             /* Glow driver */
#define SECURITYENGINE          /* Engines security On/off */
#define RETURNTOMENU            /* Uncommented if you need access to the menu */

//#define INT_REF                 /* internal 1.1v reference */

#ifdef LCDON
#include <LCDMenuLib.h>         /* https://github.com/Jomelo/LCDMenuLib */
//#include <Wire.h>               /* Interface LCD I2C, SDA = A4, SCL = A5) */
//#include <LiquidCrystal_I2C.h>  /* https://bitbucket.org/fmalpartida/new-liquidcrystal/wiki/Home */
#include "LCDML_Setup.h"        /* change the value of _LCDMenuLib_cfg_lcd_type ---- you have to edit the LCDMenuLib___config.h in the lib dir (here 115) */
#else
#include <Wire.h>               /* Interface LCD I2C, SDA = A4, SCL = A5) */
#endif

uint8_t lcdI2Caddress;          /* 39 in decimal or 0x27 in hexa, by default */
uint8_t newlcdI2Caddress;
uint16_t newValue;
uint8_t newMinValue, newMaxValue;
//LiquidCrystal_I2C lcd(lcdI2Caddress, 2, 1, 0, 4, 5, 6, 7, 3, POSITIVE); // Addr, En, Rw, Rs, d4, d5, d6, d7, backlighpin, polarity

//affectation des pins des entrees RX et sorties servos
#define BROCHE_SENSOR1          10   /* Hall or IR motor 1 */
#define BROCHE_SENSOR2          11   /* Hall or IR motor 2 */
#define BROCHE_VOIE_AUX         A0   /* Multiplex RX pinout 5 */
#define BROCHE_VOIE_THROTTLE    A1   /* Multiplex RX pinout 2 */
#define BROCHE_SERVO1           4   /* Servo motor 1 */
#define BROCHE_SERVO2           5   /* Servo motor 2 */

#ifdef EXTERNALVBATT
#define BROCHE_BATTEXT          A7  /* External battery voltage (V+) */
#include <VoltageReference.h>   /* A TESTER */
VoltageReference vRef;
#endif

#ifdef GLOWMANAGER
#define BROCHE_GLOW1            6  /* Glow driver motor 1 */
#define BROCHE_GLOW2            7  /* Glow driver motor 2 */
#endif


// affectation des pins du contacteur rotatif
#define switchPin1  A2
#define switchPin2  A3
int switchState = 0;/* affectation du statut de depart du contacteur rotatif */

boolean checkActualI2CAdress = false;
boolean readAnalogButtonpin = false;
boolean synchroIsActive = false;
boolean glowControlIsActive = false;
boolean SecurityIsON = false;
boolean saveSettings = false;
boolean RunSetup = false;
boolean simulateSpeed = false;

/* Variables Chronos*/
uint32_t BeginChronoLcdMs;//=millis();
uint32_t BeginChronoServoMs;//=millis();
uint32_t BeginIdleMode;//=0;
uint32_t ReadCaptorsMs;//=millis();
uint32_t BeginSecurityMs;
uint32_t LedStartMs;

/* Valeurs par defaut dans EEprom */
uint16_t centerposServo1;                        //mode1 (in address 1)
uint16_t centerposServo2;                        //mode1 (in address 2)
uint16_t idelposServos1;//600                    //mode1 (in address 3)
uint16_t idelposServos2;//600                    //mode1 (in address 3)
uint8_t responseTime;                            //mode2 (= a TAUX_DE_MOYENNAGE) (in address 4)
uint16_t fullThrottle;//2400                     //mode1 (in address 5)
uint16_t beginSynchro;//                         //mode1 (in address 6)
uint8_t auxChannel;                              //mode2 (in address 7)
uint8_t reverseServo1;                           //mode2 (in address 8)
uint8_t reverseServo2;                           //mode2 (in address 9)
uint16_t minimumPulse_US;                        //mode1 (in address)
uint16_t maximumPulse_US;                        //mode1 (in address)
//uint8_t telemetryType;                           //mode2 0- Rien, 1- FrSky (S-Port), 2- Futaba Sbus, 3- Hitec, 4- Hott, 5- Jeti 6- Spektrum
uint8_t nbPales;                                 //mode2 the number of blades of the propeller or nb of magnets
uint8_t moduleMasterOrSlave;                     //0 = module maitre , 1= module esclave
uint8_t fahrenheitDegrees;                       //0 = C degrees , 1= Fahrenheit degrees

/* comptage tour/mn */
volatile unsigned char FirstInputChangeCount, SecondInputChangeCount;
uint8_t VirtualPortNb, VirtualPortNb_;
uint16_t vitesse1, vitesse2;
uint16_t diffVitesseErr = 100;  //difference de vitesse entre les 2 moteurs en tr/mn toleree

/* ******************************************************************************
 * !!!!!! DOIT ETRE AU MINIMUM POUR BLOQUER LES HELICES SI PAS DE SIGNAL !!!!!! *
 ********************************************************************************  */
uint16_t Width_us = 600;
uint16_t WidthAux_us = 600;
/* Gestion entrée RX */
volatile uint16_t tpulse;     // Durée impulsion RX 
uint16_t tpulse_0;            // Durée impulsion RX precedente
uint16_t tpulse_1;            // Durée impulsion RX courante

/* Creation des objets entree RX et sorties servos */
SoftRcPulseIn  RxChannelPulseMotor;       /* Rx Pulse Engine channel */
SoftRcPulseIn  RxChannelPulseAux;         /* Rx Pulse AUX channel (Use or not use an Switch 2 or positions on your RC Transmitter) */
SoftRcPulseOut ServoMotor1;               /* Servo Engine 1 */
SoftRcPulseOut ServoMotor2;               /* Servo Engine 2 */

#ifdef ARDUINO2PC
#define LONGUEUR_MSG_MAX   70             /* ex: 1656,1653,1385,1385,2,2073,1389,1225,2073,1,0,0,1,3,39,0,0 */
#define RETOUR_CHARRIOT    0x0D           /* CR (code ASCII) */
#define PASSAGE_LIGNE      0x0A           /* LF (code ASCII) */
#define BACK_SPACE         0x08
char Message[LONGUEUR_MSG_MAX + 1];
uint8_t SubStrNb, SeparFound;
#define SUB_STRING_NB_MAX  17//16
char *StrTbl[SUB_STRING_NB_MAX];          /* declaration de pointeurs sur chaine, 1 pointeur = 2 octets seulement */
#endif
int pos = 0;


#ifdef ISI2CSLAVE
/* I2C Slave Init */
#define SLAVE_ADDRESS           0x29  //slave address,any number from 0x01 to 0x7F
/* I2C Slave Init */
#endif

/* Variables placées en SRAM */
char buff[20];//equal to width LCD
#ifdef DEBUG
const char SensType0[] PROGMEM = "normal";
const char SensType1[] PROGMEM = "inverse";
const char * const readServoSens[2] PROGMEM = {SensType0, SensType1};
/* Variables placées en SRAM */
#endif

void setup()
{

  /* Init analog serial */
  ADC_setup();
  
  /* init pins leds in output */
  PIN_OUTPUT(B,0);//D8
  PIN_OUTPUT(B,1);//D9

  Serial.begin(9600);//bloque la lecture des pins 0 et 1
  while (!Serial);// wait for serial port to connect.

  //initialise les capteurs effet hall ou IR avec une interruption associee
  TinyPinChange_Init();
  VirtualPortNb=TinyPinChange_RegisterIsr(BROCHE_SENSOR1, InterruptFunctionToCall1);
  VirtualPortNb_=TinyPinChange_RegisterIsr(BROCHE_SENSOR2, InterruptFunctionToCall2);
  /* Enable Pin Change for each pin */
  TinyPinChange_EnablePin(BROCHE_SENSOR1);
  TinyPinChange_EnablePin(BROCHE_SENSOR2);
  
  readAllEEprom();//read all settings from EEprom (sauvegarde valeurs defaut au 1er demarrage)
  switchState = readModeInStart();//lecture du codeur qui designe le mode a lancer

#ifdef CHECK_I2C_LCDAdress
  checkActualI2CAdress = true;
  Wire.begin();lcd.begin(20, 4);
  while (!checkI2CAddress()); //wait as long as checkI2CAddress() return false
#endif//end check I2C adress

#ifdef EXTERNALVBATT
  vRef.begin();
#endif

  Wire.begin();
#ifdef LCDON
  if (checkActualI2CAdress == false && readAnalogButtonpin == false)
  { 
//    TWBR = 12;// 400 kHz (maximum) (see http://www.gammon.com.au/forum/?id=10896)
    lcd.begin(20, 4); lcd.clear();

    checkRunSetup();/* check if we enter in setup during start (check buttons) */

    /* Affiche le menu principal */
    if (RunSetup == true)
    {
      LCDML_DISP_groupEnable(_LCDML_G1); // enable group 1
      LCDML_setup(_LCDML_BACK_cnt);   /* Setup for LcdMenuLib */
    }

  }
#else
  switch (switchState)
  {
    case 0://main mode (VB.NET interface use this mode)
      break;
    case 1://recorder mode
#ifdef RECORDER
      setupRecorder();
#endif 
      break;
    case 2://servo test mode
      break;
    case 3://reset settings in eeprom
      saveSettings = false;
      clearEEprom();SettingsWriteDefault();
      saveSettings = false;
      break;
  }
#endif//endif LCDON

#ifdef ISI2CSLAVE
    // Start I²C bus as a slave
    Wire.begin(SLAVE_ADDRESS);
    // Set the callback to call when data is requested.
    Wire.onRequest(requestCallback);
    Wire.onReceive(receiveCallback);
#endif

#ifdef DEBUG
  Serial << F("SynchTwinRcEngine est demarre") << endl;
  //Serial << F("Librairies Asynchrones: V") << SoftRcPulseIn::LibTextVersionRevision() << endl << endl;
  Serial << F("Librairies Asynchrones: V") << SoftRcPulseIn::LibVersion() << F(".") << SoftRcPulseIn::LibRevision() << endl << endl;
  readAllEEpromOnSerial();//lecture configuration du module dans le terminal serie

#endif//endif DEBUG


  //initialisation des voies moteur et auxiliaire: (minimumPulse_US = 600 et maximumPulse_US = 2400 (depend de la marque RX/TX))
  RxChannelPulseMotor.attach(BROCHE_VOIE_THROTTLE);//, minimumPulse_US, maximumPulse_US);//initialisation de la voie entree moteur:
  RxChannelPulseAux.attach(BROCHE_VOIE_AUX);//, minimumPulse_US, maximumPulse_US);//initialisation de la voie entree auxiliaire:
  ServoMotor1.attach(BROCHE_SERVO1);
  ServoMotor2.attach(BROCHE_SERVO2);

#ifdef GLOWMANAGER
  //initialisation du chauffage des bougies
  //glowSetup();
  PIN_OUTPUT(D,BROCHE_GLOW1);PIN_OUTPUT(D,BROCHE_GLOW2);
#endif

#ifdef SECURITYENGINE
  SecurityIsON = true;//security is set always ON on start
  /* two servos always on idel positions on start */
  if (reverseServo1 == 0) {
    ServoMotor1.write_us(idelposServos1);
  } else {
    ServoMotor1.write_us((centerposServo1 * 2) - idelposServos1);
  }
  if (reverseServo2 == 0) {
    ServoMotor2.write_us(idelposServos2);
  } else {
    ServoMotor2.write_us((centerposServo2 * 2) - idelposServos2);
  }
#endif

  if (RunSetup == false) //launch mode0 automitically()
  {
#ifdef LCDON
    //initiate bargraph and degre characters for mode0()
    lcd.createChar(7, charbackslash); intBarGraph_Characters();
#endif

  }


}//fin setup

void loop()
{
#ifdef ARDUINO2PC
  SerialFromToVB();
#endif

  /* edit these lines that follow into the LCDML_control.ino file
  #  define _LCDML_CONTROL_analog_enter_min        680     // Button Enter
  #  define _LCDML_CONTROL_analog_enter_max        700
  #  define _LCDML_CONTROL_analog_down_min         490     // Button Down
  #  define _LCDML_CONTROL_analog_down_max         510
  #  define _LCDML_CONTROL_analog_enter_down_min   390     // Button Enter + Down
  #  define _LCDML_CONTROL_analog_enter_down_max   420
  */
#ifdef READ_Button_AnalogPin
  readAnalogButtonpin = true;

#ifdef LCDON 
  lcd.setCursor(0, 0); lcd.print(F("[ Analog Pin Test! ]"));
  lcd.setCursor(5, 2); lcd.print(F("Min: ")); lcd.print(anaRead(_LCDML_CONTROL_analog_pin) - 10); lcd.print(F("  "));
  lcd.setCursor(5, 3); lcd.print(F("Max: ")); lcd.print(anaRead(_LCDML_CONTROL_analog_pin) + 10); lcd.print(F("  "));
#endif  
#if !defined(ARDUINO2PC) && !defined(LCDON)
  Serial << F("Min: ") << anaRead(_LCDML_CONTROL_analog_pin) - 10) << F("  ") << endl; 
  Serial << F("Max: ") << anaRead(_LCDML_CONTROL_analog_pin) + 10) << F("  ") << endl << endl; 
#endif
  waitMs(1000);
#endif
  
  /* menu is launched if two buttons pressed during start */
  if (RunSetup == true && readAnalogButtonpin == false && checkActualI2CAdress == false)
  {
#ifdef LCDON
    LCDML_run(_LCDML_priority);                /* lcd function call */
    //lecture de la position du canal moteur:
    if (RxChannelPulseMotor.available())
    {
      AVERAGE(Width_us, RxChannelPulseMotor.width_us(), responseTime);
    }
#endif
    switch (switchState)
    {
      case 0://main mode (VB.NET interface use this mode)
        break;
      case 1://recorder mode
#ifdef RECORDER  
      loopRecorder();
#endif
        break;
      case 2://servo test mode
        modeServosTest();
        break;
      case 3://reset settings in eeprom
        break;
    }
  }
  else if (RunSetup == false && readAnalogButtonpin == false && checkActualI2CAdress == false)
  {
    mode0();/* main mode launched if no buttons pressed during start */

#ifdef ISI2CSLAVE
    // Request data from slave.
    Wire.beginTransmission(SLAVE_ADDRESS);
    int avaiData = Wire.requestFrom(SLAVE_ADDRESS, (uint8_t)2);   
    if(avaiData == 2)
    {
      int receivedValue = Wire.read() << 8 | Wire.read();
      Serial << receivedValue << endl;
    }
    else
    {
      Serial << "Unexpected number of bytes received: " << endl;
      Serial << avaiData << endl;
    }   
    Wire.endTransmission();   
    waitMs(1000); 
#endif
  
  }


}//fin loop

#ifdef LCDON
void readAuxTypeLCD()
{
  switch (auxChannel)
  {
    case 1: lcd.print(F("No ")); break; //Aux non connecte
    case 2: lcd.write(byte(7)); lcd.print(F("|/")); break; //Aux connecte a inter 3 positions
    case 3:
    case 5:
    case 6: lcd.write(byte(7)); lcd.print(F("/")); break; //Aux connecte a inter 2 positions
    case 4: lcd.print(F("<>")); break; //Aux connecte a direction
  }
}
#endif

void readAllEEprom()
{
  byte id = EEPROMReadInt(0); // read the first byte from the EEPROM
  if ( id == 0x99)
  {
    centerposServo1 = EEPROMReadInt(1);       //mode1 (in address 1&2)
    centerposServo2 = EEPROMReadInt(3);       //mode1 (in address 3&4)
    idelposServos1 = EEPROMReadInt(5);        //mode1 (in address 5&6)
    idelposServos2 = EEPROMReadInt(7);        //mode1 (in address 7&8)
    responseTime = EEPROM.read(9);          //mode2 (= a TAUX_DE_MOYENNAGE) (in address 9&10)
    fullThrottle = EEPROMReadInt(11);         //mode1 (in address 11&12)
    beginSynchro = EEPROMReadInt(13);         //mode1 (in address 13&14)
    auxChannel = EEPROM.read(15);           //mode2 (in address 15&16)
    reverseServo1 = EEPROM.read(17);        //mode2 (in address 17&18)
    reverseServo2 = EEPROM.read(19);        //mode2 (in address 19&20)
//    telemetryType = 0;//EEPROM.read(21);        //mode2 (in address 21&22)
    minimumPulse_US = EEPROMReadInt(23);      //mode1 (in address 23&24)
    maximumPulse_US = EEPROMReadInt(25);      //mode1 (in address 25&26)
    nbPales = EEPROM.read(27);              //mode2 (in address 27)
    lcdI2Caddress = EEPROM.read(29);
    moduleMasterOrSlave = EEPROM.read(31);
    fahrenheitDegrees = EEPROM.read(33);
  }
  else
  {
    SettingsWriteDefault();
  }

}


#ifdef DEBUG
void readAllEEpromOnSerial()
{
  Serial << F("Chargement valeurs EEPROM ...") << endl;
  Serial << F("EEprom ID: 0x") << _HEX(EEPROM.read(0)) << endl;
  Serial << F("Centre servo1: ") << centerposServo1 << endl;
  Serial << F("Centre servo2: ") << centerposServo2 << endl;
  Serial << F("Position Repos servos 1: ") << idelposServos1 << "us" << endl;
  Serial << F("Position Repos servos 2: ") << idelposServos2 << "us" << endl;
  Serial << F("Vitesse de reponse: ") << responseTime << endl;
  Serial << F("Position maxi servos: ") << fullThrottle << "us" << endl;
  Serial << F("Debut synchro servos: ") << beginSynchro << "us" << endl;
  Serial << F("Position mini general: ") << minimumPulse_US << "us" << endl;
  Serial << F("Position maxi general: ") << maximumPulse_US << "us" << endl;
  if (auxChannel == 1) Serial << F("Connexion Ch AUX: mode ") << auxChannel << F(" (Ch Aux non connecte)") <<  endl;
  if (auxChannel == 2) Serial << F("Connexion Ch AUX: mode ") << auxChannel << F(" (inter 3 positions) ") <<  endl;
  if (auxChannel == 3 || auxChannel == 5 || auxChannel == 6) Serial << F("Connexion Ch AUX: mode ") << auxChannel << F(" (inter 2 positions)") <<  endl;
  if (auxChannel == 4) Serial << F("Connexion Ch AUX: mode ") << auxChannel << F(" (Ch Aux connecte a direction)") << endl;
  //  strcpy_P(buff, (char*)pgm_read_word(&(readServoSens[reverseServo1])));
  //  Serial << F("Inversion servo1: ") << buff << endl;
  //  strcpy_P(buff, (char*)pgm_read_word(&(readServoSens[reverseServo2])));
  //  Serial << F("Inversion servo2: ") << buff << endl;
  Serial << F("Adresse I2C LCD: ") << lcdI2Caddress << F(" (0x") << _HEX(lcdI2Caddress) << F(")") << endl;
  Serial << F("Nombre de pales ou d'aimants: ") << _DEC(nbPales) << endl;
  Serial << F("Voltage interne : ") <<  _FLOAT(readVcc() / 1000, 3) << F("v") << endl;
  Serial << F("Temperature interne : ") <<  GetTemp() << _FILL('°', 1) << F("C") << endl;
#ifdef EXTERNALVBATT
  Serial << F("Voltage externe : ") <<  _FLOAT(GetExternalVoltage(), 3) << F("v") << endl << endl;
  battery_Informations();
#endif
}
#endif

void SettingsWriteDefault()
{
  EEPROMWriteInt(0, 0x99);               //write the ID to indicate valid data
  EEPROMWriteInt(1, 1500);                //mode1 (in address 1&2)
  EEPROMWriteInt(3, 1500);                //mode1 (in address 3&4)
  EEPROMWriteInt(5, 1000);                //mode1 (in address 5&6)
  EEPROMWriteInt(7, 1000);                //mode1 (in address 7&8)
  EEPROM.write(9, AVERAGE_LEVEL);   //mode2 (= a TAUX_DE_MOYENNAGE) (in address 9&10)
  EEPROMWriteInt(11, 2000);               //mode1 (in address 11&12)
  EEPROMWriteInt(13, 1250);               //mode1 (in address 13&14)
  EEPROM.write(15, 1);                  //mode2 (in address 15&16)
  EEPROM.write(17, 0);                  //mode2 (in address 17&18)
  EEPROM.write(19, 0);                  //mode2 (in address 19&20)
//  EEPROM.write(21, 0);                  //mode2 telemetry (in address 21&22)
  EEPROMWriteInt(23, 1000);               //mode1 (in address 23&24)
  EEPROMWriteInt(25, 2000);               //mode1 (in address 25&26)
  EEPROM.write(27, 2);                  //mode2 (in address 27)
  EEPROM.write(29, 39);                 // (in address 29)
  EEPROM.write(31, 0);                  //
  EEPROM.write(33, 0);

}

//This function will write a 2 byte integer to the eeprom at the specified address and address + 1
void EEPROMWriteInt(uint16_t p_address, uint16_t p_value)
{
  byte lowByte = ((p_value >> 0) & 0xFF);
  byte highByte = ((p_value >> 8) & 0xFF);
  //#if (ARDUINO <  10602)
//  EEPROM.write(p_address, lowByte);
//  EEPROM.write(p_address + 1, highByte);
  //#else  /* EEPROM.update ==> 1.6.2+ uniquement */
    EEPROM.update(p_address, lowByte);
    EEPROM.update(p_address + 1, highByte);
  //#endif
}

//This function will read a 2 byte integer from the eeprom at the specified address and address + 1
//Voir http://forum.arduino.cc/index.php/topic,37470.0.html
uint16_t EEPROMReadInt(uint16_t p_address)
{
  byte lowByte = EEPROM.read(p_address);
  byte highByte = EEPROM.read(p_address + 1);
  return ((lowByte << 0) & 0xFF) + ((highByte << 8) & 0xFF00);
}

void clearEEprom()// write a 0 to all 512 bytes of the EEPROM
{
  for (int i = 0; i < 512; i++)
  {
    EEPROM.write(i, 0);
  }
  ledFlashSaveInEEProm(20);
}

void sendConfigToSerial()
{
  Serial << F("LLA");
  Serial << centerposServo1 << F("|");//array(0)
  Serial << centerposServo2 << F("|");//array(1)
  Serial << idelposServos1 << F("|");//array(2)
  Serial << idelposServos2 << F("|");//array(3)
  Serial << responseTime << F("|");//array(4)
  Serial << fullThrottle << F("|");//array(5)
  Serial << beginSynchro << F("|");//array(6)
  Serial << minimumPulse_US << F("|");//array(7)
  Serial << maximumPulse_US << F("|");//array(8)
  Serial << auxChannel << F("|");//array(9)
  Serial << reverseServo1 << F("|");//array(10)
  Serial << reverseServo2 << F("|");//array(11)
  Serial << 0 << F("|");//array(12)//  Serial << telemetryType << F("|");//array(12)
  Serial << lcdI2Caddress << F("|");//array(13)
  Serial << _DEC(nbPales) << F("|"); //array(14)
  Serial << 0 << F("|");//switchState (don't used in LCD version -- array(15))
  Serial << _FLOAT(readVcc() / 1000, 3) << F("|"); //array(16)
  Serial <<  GetTemp() << F("|");//array(17)
#ifdef EXTERNALVBATT
  Serial <<  _FLOAT(GetExternalVoltage(), 3) << F("|"); //attention a I2C qui utilise les pins A4 et A5 //array(18)
#else
  Serial <<  _FLOAT(1.000, 3) << F("|"); //attention a I2C qui utilise les pins A4 et A5 //array(18)
#endif
  Serial << moduleMasterOrSlave << F("|");//array(19)
  Serial << fahrenheitDegrees << endl;//array(20)
  Serial.flush(); // clear serial port
}


float readVcc() {// see http://provideyourown.com/2012/secret-arduino-voltmeter-measure-battery-voltage/
  // Read 1.1V reference against AVcc
  // set the reference to Vcc and the measurement to the internal 1.1V reference
#if defined(__AVR_ATmega32U4__) || defined(__AVR_ATmega1280__) || defined(__AVR_ATmega2560__)
  ADMUX = _BV(REFS0) | _BV(MUX4) | _BV(MUX3) | _BV(MUX2) | _BV(MUX1);
#elif defined (__AVR_ATtiny24__) || defined(__AVR_ATtiny44__) || defined(__AVR_ATtiny84__)
  ADMUX = _BV(MUX5) | _BV(MUX0);
#elif defined (__AVR_ATtiny25__) || defined(__AVR_ATtiny45__) || defined(__AVR_ATtiny85__)
  ADMUX = _BV(MUX3) | _BV(MUX2);
#else
  ADMUX = _BV(REFS0) | _BV(MUX3) | _BV(MUX2) | _BV(MUX1);
#endif
  delay(2); // Wait for Vref to settle
  ADCSRA |= _BV(ADSC); // Start conversion
  while (bit_is_set(ADCSRA, ADSC)); // measuring
  uint8_t low  = ADCL; // must read ADCL first - it then locks ADCH
  uint8_t high = ADCH; // unlocks both
  float result = (high << 8) | low;
  result = 1125300L / result ; // Calculate Vcc (in mV); 1125300 = 1.1*1023*1000
  return result; // Vcc in millivolts
}

double GetTemp(void)//http://playground.arduino.cc/Main/InternalTemperatureSensor
{
  unsigned int wADC;
  double t;
  /*
    The internal temperature has to be used
    with the internal reference of 1.1V.
    Channel 8 can not be selected with
    the analogRead function yet.
    Set the internal reference and mux.
  */
  ADMUX = (_BV(REFS1) | _BV(REFS0) | _BV(MUX3));
  ADCSRA |= _BV(ADEN);  // enable the ADC
  delay(20);            // wait for voltages to become stable.
  ADCSRA |= _BV(ADSC);  // Start the ADC
  // Detect end-of-conversion
  while (bit_is_set(ADCSRA, ADSC));
  // Reading register "ADCW" takes care of how to read ADCL and ADCH.
  wADC = ADCW;
  // The offset of 324.31 could be wrong. It is just an indication.
  t = (wADC - 324.31 ) / 1.22;
  (EEPROM.read(33) == 0) ? t : t = (t * 9 / 5) + 32; //T(°F) = T(°C) × 9/5 + 32
  // The returned temperature is in degrees Celcius.
  return (t);
}

void waitMs(unsigned long timetowait)
{
  static unsigned long beginTime = 0;
  static unsigned long lastTime = 0;
  beginTime = millis();
  lastTime = beginTime + timetowait;
  do
  {
  }
  while (lastTime > millis());
}


//char * ElapsedTime()
//{
//  static int t = millis()/1000;// t is time in seconds
//  static char str[6];
//  // long h = t / 3600;
//  t = t % 3600;
//  int m = t / 60;
//  int s = t % 60;
//  // sprintf(str, "%04ld:%02d:%02d", h, m, s);
//  sprintf(str, "%02d:%02d", m, s);
//  return str;
//}

//int freeRam ()
//{
//  extern int __heap_start, *__brkval;
//  int v;
//  return (int) &v - (__brkval == 0 ? (int) &__heap_start : (int) __brkval);
//}

/* LEDs fonctions */
void ledFlashInRunMode()//chenillard
{
  for (int i = 0 ; i<=5 ; i++)
  { // boucle for pour allumer les leds une par une
    PIN_HIGH(B,i);
    waitMs(70);
  }
  for (int i = 5 ; i>=0 ; i--){
    PIN_LOW(B,i);
    waitMs(70);
  }
}

void ledFlashSaveInEEProm(uint8_t nTime)//leds flash 20 fois rapidement
{
  for (int a = 1; a < nTime; a++)
  {
    PIN_TOGGLE(B,0);
    PIN_TOGGLE(B,1);
//    PIN_TOGGLE(B,2);
//    PIN_TOGGLE(B,3);
//    PIN_TOGGLE(B,4);
//    PIN_TOGGLE(B,5);
    waitMs(50);
  }
  for (int i = 5 ; i>=0 ; i--){
    PIN_LOW(B,i);
  }
}

// used for flashing a pin
void strobeBlinkPin(int pin, int count, int onInterval, int offInterval)
{
  byte i;
  for (i = 0; i < count; i++) {
    waitMs(offInterval);
    PIN_HIGH(B, pin);
    waitMs(onInterval);
    PIN_LOW(B, pin);
  }
}

#ifdef LCDON
void checkRunSetup()
{
  static unsigned long beginTime = 0;
  static unsigned long lastTime = 0;
  static uint16_t valueA6;
  /* CHECK IF USER WANTS TO RUN SETUP (Setup button depressed in first five seconds after boot) */
  beginTime = millis();  // Noter l'heure de début
  //  Serial << "Debut : millis() = " << beginTime << endl;
  lastTime = beginTime + 5000;  // 5s plus tard !
  do
  {
    valueA6 = anaRead(_LCDML_CONTROL_analog_pin);

    /* Blink LED Management */
    if (millis() - LedStartMs >= 100)
    {
      PIN_TOGGLE(B,1);
      LedStartMs = millis(); /* Restart the Chrono for the LED */
    }

    lcd.setCursor(0, 0);
    lcd.print(F("[ Enter on Menu  ? ]"));
    lcd.setCursor(0, 3);
    lcd.print(F("Click a button !"));
    lcd.setCursor(17, 3); lcd.print(5 - ((millis() - beginTime) / 1000)); lcd.print(F("s"));

    // Check the analog buttons
    if (valueA6 >= _LCDML_CONTROL_analog_enter_down_min && valueA6 <= _LCDML_CONTROL_analog_enter_down_max) //buttons enter & down held
    {
      RunSetup = true;
      lcd.clear();
      lcd.setCursor(0, 1); //ecrit a la 1ere ligne
      lcd.print(F("[ Welcome on Menu! ]"));
      delay(1000);
      lcd.clear();
      break;//end do ... while
    }
  }
  while (lastTime > millis());    // Keep looping until time's up
  ledFlashInRunMode();
  delay(1000);
  lcd.clear();
}
#endif

#ifdef ISI2CSLAVE
/* I2C Slave DATA */
void requestCallback()
{
  // Contrived example - transmit a value from an analogue pin.
    int input = anaRead(A0);
 
  // To send multiple bytes from the slave,
  // you have to fill your own buffer and send it all at once.
  uint8_t buffer[2];
  buffer[0] = input >> 8;
  buffer[1] = input & 0xff;
  Wire.write(buffer, 2);
}

void receiveCallback()
{
  
}
/* I2C Slave DATA */
#endif

//Start of ADC managements functions
void ADC_setup(){
  ADCSRA =  bit (ADEN);                      // turn ADC on
  ADCSRA |= bit (ADPS0) |  bit (ADPS1) | bit (ADPS2);  // Prescaler of 128
  #ifdef INT_REF
  ADMUX  =  bit (REFS0) | bit (REFS1);    // internal 1.1v reference
  #else
  ADMUX  =  bit (REFS0) ;   // external 5v reference
  #endif
}

int anaRead(int adc_pin){
  ADC_read_conversion();// read result of previously triggered conversion
  ADC_start_conversion(adc_pin);// start a conversion for next loop 
}

void ADC_start_conversion(int adc_pin){
  ADMUX &= ~(0x07) ; //clearing enabled channels
  ADMUX  |= (adc_pin & 0x07) ;    // AVcc and select input port
  bitSet (ADCSRA, ADSC) ;
}

int ADC_read_conversion(){
 while(bit_is_set(ADCSRA, ADSC));
 return ADC ;
}
//End of ADC management functions

/*
 * Lecture du codeur de reglage.
 * Retourne -1 si aucun mode n'est selectionne et
 * le numero du mode de 0 a 15 si un mode est selectionne
 * Chaque pin doit avoir un pull up de definit (ici en interne)
 * Le commun est cable au GND.
 */
int readModeInStart()
{
  // initialise contacteur rotatif (initialise les pull up internes sur A2 a A5)
  pinMode(switchPin1, INPUT_PULLUP);
  pinMode(switchPin2, INPUT_PULLUP);

  static int modeSettings =
//    (! digitalRead(switchPin8)) << 3 |
//    (! digitalRead(switchPin4)) << 2 | 
    (! digitalRead(switchPin2)) << 1 |
    (! digitalRead(switchPin1));//conversion du code BCD Hexa en une valeur de 0 a 15
//  modeSettings = (modeSettings >= 0 && modeSettings <= 15)?modeSettings=modeSettings:modeSettings=-1; 
//  modeSettings = (modeSettings >= 0 && modeSettings <= 7)?modeSettings=modeSettings:modeSettings=-1; 
  modeSettings = (modeSettings >= 0 && modeSettings <= 3)?modeSettings=modeSettings:modeSettings=-1; 
  return  modeSettings;
}
