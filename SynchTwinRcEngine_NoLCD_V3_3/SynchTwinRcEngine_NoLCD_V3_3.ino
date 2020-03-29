/* SyncTwinRcEngine */
/* Compile only with IDE version '1.0.6' or '1.5.7 beta' or '1.5.8 beta' or '1.6.0' to '1.8.2' */

#include <EEPROM.h>
#include <Streaming.h>          /* Librairie remplacant Serial.print() */
//#include <SoftRcPulseIn.h>      /* Librairie utilisee pour lire un signal pwm en entree */
#include <SoftRcPulseOut.h>     /* Librairie utilisee pour creer un signal pwm en sortie */
//#include <TinyPinChange.h>      /* Permet detecter en interruption les changements d'etat */
#include "Macros.h"
#include <TinyPpmReader.h>      /* Librairie utilisee pour lire un signal ppm en entree */
#include <Rcul.h>

#define FirmwareVersion 0.5

//#define DEBUG
//#define SERIALPLOTTER           /* Multi plot in IDE (don't use this option with ARDUINO2PC) */
#define ARDUINO2PC              /* PC interface (!!!!!! don't use this option with SERIALPLOTTER or READ_Button_AnalogPin !!!!!!) */
#define EXTERNALVBATT           /* Read external battery voltage */
#define GLOWMANAGER             /* Glow driver */
#define SECURITYENGINE          /* Engines security On/off */
#define PIDCONTROL              /* Use PID control for define the variable stepMotor in SynchroMotors */
//#define SDDATALOGGER            /* Use SD card for save speeds */
//#define I2CSLAVEFOUND           /* for command a second module by the I2C port */
#define INT_REF                 /* internal 1.1v reference */

/*
0     INPUT PPM
1     Tx (Not used)
2     Hall or IR motor 1 
3     Hall or IR motor 2 
4     Servo motor 1 
5     Servo motor 2 
6     Glow driver motor 1 
7     Glow driver motor 2
8     Led Green
9     Led Green
SD card attached to SPI bus as follows:
10    CS
11    MOSI
12    MISO
13    CLK

A0    Led Red
A1    Led Red
A2    Led Yellow
A3    Led Yellow
A4    SDA
A5    SCL 
A6    
A7    External power V+

*/

#ifdef SDDATALOGGER
#include <SD.h>
Sd2Card card;
SdVolume volume;
SdFile root;
const int chipSelect = 10;
#endif

//affectation des pins des entrees RX et sorties servos
#define BROCHE_VOIE_THROTTLE    0   /* Multiplex RX pinout 2 */
#define BROCHE_SENSOR1          2    /* Hall or IR motor 1 */
#define BROCHE_SENSOR2          3    /* Hall or IR motor 2 */
#define BROCHE_SERVO1           4    /* Servo motor 1 */
#define BROCHE_SERVO2           5    /* Servo motor 2 */
//#define BROCHE_VOIE_AUX         A0   /* Multiplex RX pinout 5 */

#ifdef GLOWMANAGER
#define BROCHE_GLOW1            6  /* Glow driver motor 1 */
#define BROCHE_GLOW2            7  /* Glow driver motor 2 */
#endif


#ifdef EXTERNALVBATT
#define BROCHE_BATTEXT          A7  /* External battery voltage (V+) */
#include <VoltageReference.h>   /* A TESTER */
VoltageReference vRef;
#else
//pin A7 usable for another thing
#endif

//boolean checkActualI2CAdress = false;
boolean synchroIsActive = false;
boolean glowControlIsActive = false;
boolean SecurityIsON = false;
//boolean saveSettings = false;
//boolean RunSetup = false;
boolean simulateSpeed = false;
boolean SDCardUsable = false;
boolean RunLogInSDCard = false;
//boolean RunEngineRecoder = false;
//boolean RunEnginePlayer = false;

/* Variables Chronos*/
uint32_t BeginChronoLcdMs;//=millis();
uint32_t BeginChronoServoMs;//=millis();
uint32_t BeginIdleMode;//=0;
uint32_t ReadCaptorsMs;//=millis();
uint32_t BeginSecurityMs;//=millis();
uint32_t LedStartMs;//=millis();
uint32_t SendMotorsToVBMs;//=millis();

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
static uint16_t minimumSpeed;
static uint16_t maximumSpeed;

/* comptage tr/mn */
volatile uint16_t FirstInputChangeCount=0, SecondInputChangeCount=0;
uint8_t VirtualPortNb, VirtualPortNb_;
uint16_t vitesse1, vitesse2;                    //blades speeds in rpm
double diffVitesseErr;                          //difference de vitesse entre les 2 moteurs en tr/mn toleree
double diffVitesse;                             //difference de vitesse entre les 2 moteurs en tr/mn (peut etre negatif !!!)
double stepMotor;                               //nb de micro secondes ajoutes ou enleves
int readings_V1[10];                            //averaging last 10 readings_V1
int readings_V2[10];                            //averaging last 10 readings_V2

int index = 0;

#ifdef PIDCONTROL
#include <PID_v1.h>
//Define PID Variables
//Define the aggressive and conservative Tuning Parameters
double aggKp=100, aggKi=0.2, aggKd=1;//aggressive
double consKp=1, consKi=0.05, consKd=0.25;//conservative
PID myPID(&diffVitesse, &stepMotor, &diffVitesseErr, consKp, consKi, consKd, DIRECT);
// EEPROM addresses for persisted data
const int SpAddress = 50;//0
const int KpAddress = 58;//8
const int KiAddress = 66;//16
const int KdAddress = 74;//24
#endif


/* ******************************************************************************
 * !!!!!! DOIT ETRE AU MINIMUM POUR BLOQUER LES HELICES SI PAS DE SIGNAL !!!!!! *
 ********************************************************************************  */
uint16_t Width_us = 1000;
uint16_t WidthAux_us = 1000;

/* Creation des objets entree RX et sorties servos */
//SoftRcPulseIn  RxChannelPulseMotor;       /* Rx Pulse Engine channel */
//SoftRcPulseIn  RxChannelPulseAux;         /* Rx Pulse AUX channel (Use or not use an Switch 2 or positions on your RC Transmitter) */
SoftRcPulseOut ServoMotor1;               /* Servo Engine 1 */
SoftRcPulseOut ServoMotor2;               /* Servo Engine 2 */

#define SERIALBAUD         115200         /* 115200 is need for use BlueSmirF BT module */
#ifdef ARDUINO2PC
#define LONGUEUR_MSG_MAX   70             /* ex: 1656,1653,1385,1385,2,2073,1389,1225,2073,1,0,0,1,3,39,0,0,1000,20000 */
#define RETOUR_CHARRIOT    0x0D           /* CR (code ASCII) */
#define PASSAGE_LIGNE      0x0A           /* LF (code ASCII) */
#define BACK_SPACE         0x08
char Message[LONGUEUR_MSG_MAX + 1];
uint8_t SubStrNb, SeparFound;
#define SUB_STRING_NB_MAX  19
char *StrTbl[SUB_STRING_NB_MAX];          /* declaration de pointeurs sur chaine, 1 pointeur = 2 octets seulement */
#endif
int pos = 0;

#ifdef I2CSLAVEFOUND
#include <Wire.h>               /* Interface LCD I2C, SDA = A4, SCL = A5) */
#endif

void setup()
{

#ifdef I2CSLAVEFOUND
  Wire.begin();
#endif

#ifdef PIDCONTROL
  //initialize the variables we're linked to
  /* diffVitesse = V1-V2 */
  diffVitesseErr = EEPROMRead(42);
  
  //turn the PID on
  myPID.SetMode(AUTOMATIC);
  myPID.SetOutputLimits(0,100);// default is 0-255
#endif

#ifdef EXTERNALVBATT
  /* Init analog serial */
  //ADC_setup();
#endif
  
  /* init pins leds in output */
  PIN_OUTPUT(B,0);//D8
  PIN_OUTPUT(B,1);//D9
  PIN_OUTPUT(C,2);//D16 (A2)
  PIN_OUTPUT(C,3);//D17 (A3)
  PIN_OUTPUT(C,6);//A6 (Glow is On or Off)
  
#ifdef GLOWMANAGER
  //initialisation du chauffage des bougies
  PIN_OUTPUT(D,BROCHE_GLOW1);PIN_OUTPUT(D,BROCHE_GLOW2);
#endif  
  
  Serial.begin(SERIALBAUD);//bloque la lecture des pins 0 et 1
  while (!Serial);// wait for serial port to connect.

#ifdef SDDATALOGGER
  pinMode(chipSelect, OUTPUT);
  // see if the card is present and can be initialized:
  if (!SD.begin(chipSelect)) 
  {
    SDCardUsable = false;
  }
  else
  {
    SDCardUsable = true;
  }
#endif

  //initialise les capteurs effet hall ou IR avec une interruption associee
  TinyPinChange_Init();
  VirtualPortNb=TinyPinChange_RegisterIsr(BROCHE_SENSOR1, InterruptFunctionToCall);
  VirtualPortNb_=TinyPinChange_RegisterIsr(BROCHE_SENSOR2, InterruptFunctionToCall);
  /* Enable Pin Change for each pin */
  TinyPinChange_EnablePin(BROCHE_SENSOR1);
  TinyPinChange_EnablePin(BROCHE_SENSOR2);
  
  readAllEEprom();//read all settings from EEprom (save default's values in first start)
  //switchState = readModeInStart();//lread switch position

#ifdef EXTERNALVBATT
  vRef.begin();
#endif

//  switch (switchState)
//  {
//    case 0://main mode (VB.NET interface use this mode)
//      break;
//    case 1://recorder mode
//      break;
//    case 2://servo test mode
//      break;
//    case 3://reset settings in eeprom
//      saveSettings = false;
//      clearEEprom();SettingsWriteDefault();
//      saveSettings = false;
//      break;
//  }


#ifdef DEBUG
  Serial << F("SynchTwinRcEngine est demarre") << endl;
  Serial << F("Librairies Asynchrones: V") << SOFT_RC_PULSE_IN_VERSION << F(".") << SOFT_RC_PULSE_IN_REVISION << endl << endl;
  readAllEEpromOnSerial();//lecture configuration du module dans le terminal serie
#endif//endif DEBUG
  //Serial << F("Voies dans PPM = ") << (int)TinyPpmReader.detectedChannelNb() << endl;
  //waitMs(250);
  Serial.end();
  //initialisation des voies moteur et auxiliaire: (minimumPulse_US = 600 et maximumPulse_US = 2400 (depend de la marque RX/TX))
  TinyPpmReader.attach(BROCHE_VOIE_THROTTLE); /* Attach TinyPpmReader to PPM_INPUT_PIN pin */
  //RxChannelPulseMotor.attach(BROCHE_VOIE_THROTTLE);//, minimumPulse_US, maximumPulse_US);//initialisation de la voie entree moteur:
  //RxChannelPulseAux.attach(BROCHE_VOIE_AUX);//, minimumPulse_US, maximumPulse_US);//initialisation de la voie entree auxiliaire:
  ServoMotor1.attach(BROCHE_SERVO1);
  ServoMotor2.attach(BROCHE_SERVO2);
  

#ifdef SECURITYENGINE
  /* two servos always on idle positions on start */
  SecurityIsON = true;//security is set always ON on start
//  if (reverseServo1 == 0) {
//    ServoMotor1.write_us(idelposServos1);
//  } else {
//    ServoMotor1.write_us((centerposServo1 * 2) - idelposServos1);
//  }
//  if (reverseServo2 == 0) {
//    ServoMotor2.write_us(idelposServos2);
//  } else {
//    ServoMotor2.write_us((centerposServo2 * 2) - idelposServos2);
//  }
#endif

  ledFlashInRunMode();
}//fin setup


void loop()
{
#ifdef ARDUINO2PC
  SerialFromToVB();
#endif

  mode0();/* main mode launched if no buttons pressed during start */

}//fin loop

void readAllEEprom()
{
  byte id = EEPROMRead(0); // read the first byte from the EEPROM
  if ( id == 0x99)
  {
    centerposServo1 = EEPROMRead(1);       //mode1 (in address 1&2)
    centerposServo2 = EEPROMRead(3);       //mode1 (in address 3&4)
    idelposServos1 = EEPROMRead(5);        //mode1 (in address 5&6)
    idelposServos2 = EEPROMRead(7);        //mode1 (in address 7&8)
    responseTime = EEPROM.read(9);          //mode2 (= a TAUX_DE_MOYENNAGE) (in address 9&10)
    fullThrottle = EEPROMRead(11);         //mode1 (in address 11&12)
    beginSynchro = EEPROMRead(13);         //mode1 (in address 13&14)
    auxChannel = EEPROM.read(15);           //mode2 (in address 15&16)
    reverseServo1 = EEPROM.read(17);        //mode2 (in address 17&18)
    reverseServo2 = EEPROM.read(19);        //mode2 (in address 19&20)
    diffVitesseErr = EEPROMRead(42);       //mode2 (in address 21&22)
    minimumPulse_US = EEPROMRead(23);      //mode1 (in address 23&24)
    maximumPulse_US = EEPROMRead(25);      //mode1 (in address 25&26)
    nbPales = EEPROM.read(27);              //mode2 (in address 27)
//    lcdI2Caddress = EEPROM.read(29);
    moduleMasterOrSlave = EEPROM.read(31);
    fahrenheitDegrees = EEPROM.read(33);
    minimumSpeed = EEPROMRead(35);
    maximumSpeed = EEPROMRead(37);
    // after address 80, recorder save data and PID
  }
  else
  {
    SettingsWriteDefault();
    waitMs(500);
    readAllEEprom();
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
  Serial << F("Difference vitess Err: ") << diffVitesseErr << endl;
//  Serial << F("Adresse I2C LCD: ") << lcdI2Caddress << F(" (0x") << _HEX(lcdI2Caddress) << F(")") << endl;
  Serial << F("Nombre de pales ou d'aimants: ") << _DEC(nbPales) << endl;
  Serial << F("Voltage interne : ") <<  _FLOAT(readVcc() / 1000, 3) << F("v") << endl;
  if (fahrenheitDegrees == 0)
  {
    Serial << F("Temperature interne : ") <<  GetTemp() << _FILL('°', 1) << F("C") << endl;
  }
  else
  { 
    Serial << F("Temperature interne : ") <<  GetTemp() << _FILL('°', 1) << F("F") << endl;
  }

  Serial << F("Mini Motor : ") <<  minimumSpeed << F(" rpm") << endl;
  Serial << F("Maxi Motor : ") <<  maximumSpeed << F(" rpm") << endl;
#ifdef EXTERNALVBATT
  Serial << F("Voltage externe : ") <<  _FLOAT(GetExternalVoltage(), 3) << F("v") << endl;
  battery_Informations();
#endif
}
#endif

void SettingsWriteDefault()
{
  EEPROMWrite(0, 0x99);              //write the ID to indicate valid data
  EEPROMWrite(1, 1500);              //mode1 (in address 1&2)
  EEPROMWrite(3, 1500);              //mode1 (in address 3&4)
  EEPROMWrite(5, 1000);              //mode1 (in address 5&6)
  EEPROMWrite(7, 1000);              //mode1 (in address 7&8)
  EEPROM.update(9, 2);                  //mode2 (= a TAUX_DE_MOYENNAGE) (in address 9&10)
  EEPROMWrite(11, 2000);             //mode1 (in address 11&12)
  EEPROMWrite(13, 1250);             //mode1 (in address 13&14)
  EEPROM.update(15, 1);                  //mode2 (in address 15&16)
  EEPROM.update(17, 0);                  //mode2 (in address 17&18)
  EEPROM.update(19, 0);                  //mode2 (in address 19&20)
  EEPROMWrite(42, 99);                //mode2 diffVitesseErr (in address 21&22)
  EEPROMWrite(23, 1000);             //mode1 (in address 23&24)
  EEPROMWrite(25, 2000);             //mode1 (in address 25&26)
  EEPROM.update(27, 2);                  //mode2 (in address 27)
  EEPROM.update(29, 39);                 // (in address 29)
  EEPROM.update(31, 0);                  //master(0) or slave (1)
  EEPROM.update(33, 0);                  //fahrenheitDegrees
  EEPROMWrite(35, 1000);             //minimum motor rpm
  EEPROMWrite(37, 20000);            //maximum motor rpm

}

//This function will write a 2 byte integer to the eeprom at the specified address and address + 1
//void EEPROMWrite(uint16_t p_address, uint16_t p_value)
//{
//  byte lowByte = ((p_value >> 0) & 0xFF);
//  byte highByte = ((p_value >> 8) & 0xFF);
//  //#if (ARDUINO <  10602)
////  EEPROM.write(p_address, lowByte);
////  EEPROM.write(p_address + 1, highByte);
//  //#else  /* EEPROM.update ==> 1.6.2+ uniquement */
//    EEPROM.update(p_address, lowByte);
//    EEPROM.update(p_address + 1, highByte);
//  //#endif
//}

//This function will read a 2 byte integer from the eeprom at the specified address and address + 1
//Voir http://forum.arduino.cc/index.php/topic,37470.0.html
//uint16_t EEPROMRead(uint16_t p_address)
//{
//  byte lowByte = EEPROM.read(p_address);
//  byte highByte = EEPROM.read(p_address + 1);
//  return ((lowByte << 0) & 0xFF) + ((highByte << 8) & 0xFF00);
//}

//This function will write a 4 byte (32bit) long to the eeprom at
//the specified address to address + 3.
//void EEPROMWritelong(int address, long value)
//{
//  //Decomposition from a long to 4 bytes by using bitshift.
//  //One = Most significant -> Four = Least significant byte
//  byte four = (value & 0xFF);
//  byte three = ((value >> 8) & 0xFF);
//  byte two = ((value >> 16) & 0xFF);
//  byte one = ((value >> 24) & 0xFF);
//  
//  //Write the 4 bytes into the eeprom memory.
//  EEPROM.update(address, four);
//  EEPROM.update(address + 1, three);
//  EEPROM.update(address + 2, two);
//  EEPROM.update(address + 3, one);
//}

//This function will return a 4 byte (32bit) long from the eeprom
//at the specified address to address + 3.
//long EEPROMReadlong(long address)
//{
//  //Read the 4 bytes from the eeprom memory.
//  long four = EEPROM.read(address);
//  long three = EEPROM.read(address + 1);
//  long two = EEPROM.read(address + 2);
//  long one = EEPROM.read(address + 3);
//  
//  //Return the recomposed long by using bitshift.
//  return ((four << 0) & 0xFF) + ((three << 8) & 0xFFFF) + ((two << 16) & 0xFFFFFF) + ((one << 24) & 0xFFFFFFFF);
//}

// ************************************************
// Write floating point values to EEPROM
// ************************************************
void EEPROMWrite(int address, double value)
{
   byte* p = (byte*)(void*)&value;
   for (int i = 0; i < sizeof(value); i++)
   {
      EEPROM.update(address++, *p++);
   }
}
 
// ************************************************
// Read floating point values from EEPROM
// ************************************************
double EEPROMRead(int address)
{
   double value = 0.0;
   byte* p = (byte*)(void*)&value;
   for (int i = 0; i < sizeof(value); i++)
   {
      *p++ = EEPROM.read(address++);
   }
   return value;
}

#ifdef PIDCONTROL
// ************************************************
// Save PID parameter changes to EEPROM
// ************************************************
void SaveParameters()
{
   if (diffVitesseErr != EEPROMRead(SpAddress))
   {
      EEPROMWrite(SpAddress, diffVitesseErr);
   }
   if (consKp != EEPROMRead(KpAddress))
   {
      EEPROMWrite(KpAddress, consKp);
   }
   if (consKi != EEPROMRead(KiAddress))
   {
      EEPROMWrite(KiAddress, consKi);
   }
   if (consKd != EEPROMRead(KdAddress))
   {
      EEPROMWrite(KdAddress, consKd);
   }
}
#endif

void clearEEprom()// write a 0 to all 512 bytes of the EEPROM
{
  for (int i = 0; i < 512; i++)
  {
    EEPROM.update(i, 0);
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
  Serial << diffVitesseErr << F("|");//array(12)
#ifdef LCDON
  Serial << lcdI2Caddress << F("|");//array(13)
#else
  Serial << F("NOTUSED|");//array(13)
#endif
  Serial << _DEC(nbPales) << F("|"); //array(14)
  Serial << 0 << F("|");//switchState (don't used in LCD version -- array(15))
  Serial << _FLOAT(readVcc() / 1000, 3) << F("|"); //array(16)
  Serial <<  GetTemp() << F("|");//array(17)
#ifdef EXTERNALVBATT
  Serial <<  _FLOAT(GetExternalVoltage(), 3) << F("|"); //attention a I2C qui utilise les pins A4 et A5 //array(18)
#else
  Serial << F("NOTUSED|");//array(18)
#endif
  Serial << moduleMasterOrSlave << F("|");//array(19)
  Serial << fahrenheitDegrees << F("|");//array(20)
  Serial << minimumSpeed << F("|");//array(21)
  Serial << maximumSpeed << endl;//array(22)
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


/* LEDs fonctions */
void ledFlashInRunMode()//chenillard
{
  PIN_HIGH(B,0);waitMs(50);PIN_HIGH(B,1);waitMs(50);PIN_HIGH(C,2);waitMs(50);PIN_HIGH(C,3);waitMs(50);PIN_HIGH(C,6);
  waitMs(50);
  PIN_LOW(C,6);waitMs(50);PIN_LOW(C,3);waitMs(50);PIN_LOW(C,2);waitMs(50);PIN_LOW(B,1);waitMs(50);PIN_LOW(B,0);
  waitMs(50);
  PIN_HIGH(B,0);waitMs(50);PIN_HIGH(B,1);waitMs(50);PIN_HIGH(C,2);waitMs(50);PIN_HIGH(C,3);waitMs(50);PIN_HIGH(C,6);
  waitMs(50);
  PIN_LOW(C,6);waitMs(50);PIN_LOW(C,3);waitMs(50);PIN_LOW(C,2);waitMs(50);PIN_LOW(B,1);waitMs(50);PIN_LOW(B,0);
}

void ledFlashSaveInEEProm(uint8_t nTime)//leds flash 20 fois rapidement
{
  for (int a = 1; a < nTime; a++)
  {
    PIN_TOGGLE(B,0);
    PIN_TOGGLE(B,1);
    PIN_TOGGLE(C,2);
    PIN_TOGGLE(C,3);
    PIN_TOGGLE(C,6);
    //PIN_TOGGLE(C,5);
    waitMs(50);
  }
  PIN_LOW(B,0);
  PIN_LOW(B,1);
  PIN_LOW(C,2);
  PIN_LOW(C,3);
  PIN_LOW(C,6);
//PIN_LOW(B,5);  
}

// used for flashing a pin
//void strobeBlinkPin(int pin, int count, int onInterval, int offInterval)
//{
//  byte i;
//  for (i = 0; i < count; i++) 
//  {
//    waitMs(offInterval);
//    if(pin == 0 || pin == 1)
//    {
//      PIN_HIGH(B, pin);
//      waitMs(onInterval);
//      PIN_LOW(B, pin);    
//    }
//    else if (pin == 2 || pin == 3)
//    {
//      PIN_HIGH(C, pin);
//      waitMs(onInterval);
//      PIN_LOW(C, pin);     
//    }
//  }
//}


#ifdef I2CSLAVEFOUND
/* I2C Slave DATA */
void requestCallback()
{
//  // Contrived example - transmit a value from an analogue pin.
//  int input = anaRead(A0);
// 
//  // To send multiple bytes from the slave,
//  // you have to fill your own buffer and send it all at once.
//  uint8_t buffer[2];
//  buffer[0] = input >> 8;
//  buffer[1] = input & 0xff;
//  Wire.write(buffer, 2);
}

void receiveCallback()
{
  
}
/* I2C Slave DATA */
#endif

#ifdef EXTERNALVBATT
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
#endif

/*
 * Lecture du codeur de reglage.
 * Retourne -1 si aucun mode n'est selectionne et
 * le numero du mode de 0 a 15 si un mode est selectionne
 * Chaque pin doit avoir un pull up de definit (ici en interne)
 * Le commun est cable au GND.
 */
//int readModeInStart()
//{
//  // initialise contacteur rotatif (initialise les pull up internes sur A2 a A5)
//  pinMode(switchPin1, INPUT_PULLUP);
//  pinMode(switchPin2, INPUT_PULLUP);
//
//  static int modeSettings =
////    (! digitalRead(switchPin8)) << 3 |
////    (! digitalRead(switchPin4)) << 2 | 
//    (! digitalRead(switchPin2)) << 1 |
//    (! digitalRead(switchPin1));//conversion du code BCD Hexa en une valeur de 0 a 15
////  modeSettings = (modeSettings >= 0 && modeSettings <= 15)?modeSettings=modeSettings:modeSettings=-1; 
////  modeSettings = (modeSettings >= 0 && modeSettings <= 7)?modeSettings=modeSettings:modeSettings=-1; 
//  modeSettings = (modeSettings >= 0 && modeSettings <= 3)?modeSettings=modeSettings:modeSettings=-1; 
//  return  modeSettings;
//}
