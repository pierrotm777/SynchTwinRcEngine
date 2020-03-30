/* SyncTwinRcEngine */
/* Compile only with IDE version '1.0.6' or '1.5.7 beta' or '1.5.8 beta' or '1.6.0' to '1.8.2' */

#include <EEPROM.h>
#include <Streaming.h>          /* Librairie remplacant Serial.print() */
#include <SoftRcPulseOut.h>     /* Librairie utilisee pour creer un signal pwm en sortie */
#include "Macros.h"
#include <TinyPpmReader.h>      /* Librairie utilisee pour lire un signal ppm en entree */
#include <Rcul.h>

#include <FlySkyIBus.h>

#define FirmwareVersion 0.5

//#define DEBUG
//#define SERIALPLOTTER           /* Multi plot in IDE (don't use this option with ARDUINO2PC) */
#define ARDUINO2PC              /* PC interface (!!!!!! don't use this option with SERIALPLOTTER or READ_Button_AnalogPin !!!!!!) */
//#define EXTERNALVBATT           /* Read external battery voltage */
//#define GLOWMANAGER             /* Glow driver */
//#define SECURITYENGINE          /* Engines security On/off */
//#define PIDCONTROL              /* Use PID control for define the variable stepMotor in SynchroMotors */
//#define SDDATALOGGER            /* Use SD card for save speeds */
//#define I2CSLAVEFOUND           /* for command a second module by the I2C port */
//#define INT_REF                 /* internal 1.1v reference */
//#define RECORDER

/*
0     INPUT PPM
1     Tx (Not used)
2     Hall or IR motor 1 
3     Hall or IR motor 2 
4     Servo motor 1 
5     Servo motor 2 
6     Servo rudder 
7     Glow driver motor 1
8     Glow driver motor 2
9     
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
#define BROCHE_PPMINPUT         0    /* Multiplex RX pinout 2 */
#define BROCHE_SENSOR1          2    /* Hall or IR motor 1 */
#define BROCHE_SENSOR2          3    /* Hall or IR motor 2 */
#define BROCHE_MOTOR1           4    /* Servo motor 1 */
#define BROCHE_MOTOR2           5    /* Servo motor 2 */
#define BROCHE_RUDDER           6    /* Servo rudder */

#ifdef GLOWMANAGER
#define BROCHE_GLOW1            7  /* Glow driver motor 1 */
#define BROCHE_GLOW2            8  /* Glow driver motor 2 */
#endif


#ifdef EXTERNALVBATT
#define BROCHE_BATTEXT          A7  /* External battery voltage (V+) */
#include <VoltageReference.h>   /* A TESTER */
VoltageReference vRef;
#else
//pin A7 usable for another thing
#endif

boolean synchroIsActive = false;
boolean glowControlIsActive = false;
boolean SecurityIsON = false;
boolean simulateSpeed = false;
boolean SDCardUsable = false;
boolean RunLogInSDCard = false;

/* Variables Chronos*/
uint32_t BeginChronoLcdMs;//=millis();
uint32_t BeginChronoServoMs;//=millis();
uint32_t BeginIdleMode;//=0;
uint32_t ReadCaptorsMs;//=millis();
uint32_t BeginSecurityMs;//=millis();
uint32_t LedStartMs;//=millis();
uint32_t SendMotorsToVBMs;//=millis();

enum { CPPM=0, SBUS, IBUS };
bool InputSignalExist = false;

//SBUS variables
int buf[25];
int voie[18];
int memread;
int cpt;
int s1,s2,s3,s4;

/*
#define RC_CHANS  12
enum rc {ROLL,PITCH,YAW ,THROTTLE,AUX1,AUX2,
         AUX3,AUX4 ,AUX5,AUX6    ,AUX7,AUX8};
         
//#define SERIAL_SUM_PPM         PITCH,YAW,THROTTLE,ROLL,AUX1,AUX2,AUX3,AUX4,8,9,10,11 //For Graupner/Spektrum
#define SERIAL_SUM_PPM         ROLL,PITCH,THROTTLE,YAW,AUX1,AUX2,AUX3,AUX4,8,9,10,11 //For Robe/Hitec/Futaba
//#define SERIAL_SUM_PPM         ROLL,PITCH,YAW,THROTTLE,AUX1,AUX2,AUX3,AUX4,8,9,10,11 //For Multiplex
//#define SERIAL_SUM_PPM         PITCH,ROLL,THROTTLE,YAW,AUX1,AUX2,AUX3,AUX4,8,9,10,11 //For some Hitec/Sanwa/Others

#if defined(SERIAL_SUM_PPM) //Channel order for PPM SUM RX Configs
  static uint8_t rcChannel[RC_CHANS] = {SERIAL_SUM_PPM};
#endif
*/

/** La structure permettant de stocker les données */
//https://www.carnetdumaker.net/articles/stocker-des-donnees-en-memoire-eeprom-avec-une-carte-arduino-genuino/
struct MaStructure {
  /* Valeurs par defaut dans EEprom */
  byte ID;
  uint8_t InputMode;                               //0-PPM, 1-SBUS, 2,IBUS
  uint8_t MotorNbChannel;
  uint8_t AuxiliaryNbChannel;                      //channel number (default is 5)
  uint8_t RudderNbChannel;
  uint8_t AileronNbChannel;
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
  double diffVitesseErr;                          //difference de vitesse entre les 2 moteurs en tr/mn toleree
  uint16_t minimumPulse_US;                        //mode1 (in address)
  uint16_t maximumPulse_US;                        //mode1 (in address)
  //uint8_t telemetryType;                           //mode2 0- Rien, 1- FrSky (S-Port), 2- Futaba Sbus, 3- Hitec, 4- Hott, 5- Jeti 6- Spektrum
  uint8_t nbPales;                                 //mode2 the number of blades of the propeller or nb of magnets
  uint8_t moduleMasterOrSlave;                     //0 = module maitre , 1= module esclave
  uint8_t fahrenheitDegrees;                       //0 = C degrees , 1= Fahrenheit degrees
  uint16_t minimumSpeed;
  uint16_t maximumSpeed;
  //PID variables
  double aggKp;//aggressive
  double aggKi;//aggressive
  double aggKd;//aggressive
  double consKp; //conservative 
  double consKi;  //conservative
  double consKd; //conservative
  int SpAddress;//0
  int KpAddress;//8
  int KiAddress;//16
  int KdAddress;//24
}; // Ne pas oublier le point virgule !

MaStructure ms;

/* comptage tr/mn */
volatile uint16_t FirstInputChangeCount=0, SecondInputChangeCount=0;
uint8_t VirtualPortNb, VirtualPortNb_;
uint16_t vitesse1, vitesse2;                    //blades speeds in rpm
//double diffVitesseErr;                          //difference de vitesse entre les 2 moteurs en tr/mn toleree
double diffVitesse;                             //difference de vitesse entre les 2 moteurs en tr/mn (peut etre negatif !!!)
double stepMotor;                               //nb de micro secondes ajoutes ou enleves
int readings_V1[10];                            //averaging last 10 readings_V1
int readings_V2[10];                            //averaging last 10 readings_V2

int index = 0;

#ifdef PIDCONTROL
#include <PID_v1.h>
//Define PID Variables
//Define the aggressive and conservative Tuning Parameters
//double aggKp=100, aggKi=0.2, aggKd=1;//aggressive
//double consKp=1, consKi=0.05, consKd=0.25;//conservative
PID myPID(&diffVitesse, &stepMotor, &ms.diffVitesseErr, ms.consKp, ms.consKi, ms.consKd, DIRECT);
// EEPROM addresses for persisted data
//const int SpAddress = 50;//0
//const int KpAddress = 58;//8
//const int KiAddress = 66;//16
//const int KdAddress = 74;//24
#endif

/* ******************************************************************************
 * !!!!!! DOIT ETRE AU MINIMUM POUR BLOQUER LES HELICES SI PAS DE SIGNAL !!!!!! *
 ********************************************************************************  */
uint16_t Width_us = 1000;
uint16_t WidthAux_us = 1000;
uint16_t WidthRud_us = 1000;
uint16_t WidthAil_us = 1000;

/* Creation des objets Sorties servos */
SoftRcPulseOut ServoMotor1;               /* Servo Engine 1 */
SoftRcPulseOut ServoMotor2;               /* Servo Engine 2 */
SoftRcPulseOut ServoRudder;

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
#define SLAVE_ADRESS      20
#endif

void setup()
{

#ifdef I2CSLAVEFOUND
  Wire.begin(SLAVE_ADRESS,2);
#endif

#ifdef PIDCONTROL
  //initialize the variables we're linked to
  /* diffVitesse = V1-V2 */
  //diffVitesseErr = EEPROMRead(42);
  
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

#ifdef EXTERNALVBATT
  vRef.begin();
#endif


#ifdef DEBUG
  Serial << F("SynchTwinRcEngine est demarre") << endl;
  //Serial << F("Librairies Asynchrones: V") << SOFT_RC_PULSE_IN_VERSION << F(".") << SOFT_RC_PULSE_IN_REVISION << endl << endl;
  readAllEEpromOnSerial();//lecture configuration du module dans le terminal serie
#endif//endif DEBUG

  switch (ms.InputMode)//CPPM,SBUS or IBUS
  {
    case CPPM:
      Serial.end();
      TinyPpmReader.attach(BROCHE_PPMINPUT); // Attach TinyPpmReader to SIGNAL_INPUT_PIN pin 
      break;
    case SBUS:
      Serial.flush();delay(500); // wait for last transmitted data to be sent
      Serial.begin(100000, SERIAL_8E2);// Choose your serial first: SBUS works at 100 000 bauds 
      break;
    case IBUS:
      Serial.end();
      IBus.begin(Serial);
      break;
  }

  ServoMotor1.attach(BROCHE_MOTOR1);
  ServoMotor2.attach(BROCHE_MOTOR2);
  

#ifdef SECURITYENGINE
  /* two servos always on idle positions on start */
  SecurityIsON = true;//security is set always ON on start
  if (ms.reverseServo1 == 0) {
    ServoMotor1.write_us(ms.idelposServos1);
  } else {
    ServoMotor1.write_us((ms.centerposServo1 * 2) - ms.idelposServos1);
  }
  if (ms.reverseServo2 == 0) {
    ServoMotor2.write_us(ms.idelposServos2);
  } else {
    ServoMotor2.write_us((ms.centerposServo2 * 2) - ms.idelposServos2);
  }
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

/*
  ms.ID                  = 0x99;//EEPROMWrite(0, 0x99);              //write the ID to indicate valid data
  ms.InputMode           = 0;  //0-PPM, 1-SBUS, 2,IBUS
  ms.MotorNbChannel      = 3;
  ms.AuxiliaryNbChannel  = 5;
  ms.RudderNbChannel     = 1;
  ms.AileronNbChannel    = 4;
  ms.centerposServo1     = 1500;//EEPROMWrite(1, 1500);              //mode1 (in address 1&2)
  ms.centerposServo2     = 1500;//EEPROMWrite(3, 1500);              //mode1 (in address 3&4)
  ms.idelposServos1      = 1000;//EEPROMWrite(5, 1000);              //mode1 (in address 5&6)
  ms.idelposServos2      = 1000;//EEPROMWrite(7, 1000);              //mode1 (in address 7&8)
  ms.responseTime        = 2   ;//EEPROM.update(9, 2);               //mode2 (= a TAUX_DE_MOYENNAGE) (in address 9&10)
  ms.fullThrottle        = 2000;//EEPROMWrite(11, 2000);             //mode1 (in address 11&12)
  ms.beginSynchro        = 1250;//EEPROMWrite(13, 1250);             //mode1 (in address 13&14)
  ms.auxChannel          = 1;   //EEPROM.update(15, 1);              //mode2 (in address 15&16)
  ms.reverseServo1       = 0;   //EEPROM.update(17, 0);              //mode2 (in address 17&18)
  ms.reverseServo2       = 0;   //EEPROM.update(19, 0);              //mode2 (in address 19&20)
  ms.diffVitesseErr      = 99;  //EEPROMWrite(42, 99);               //mode2 diffVitesseErr (in address 21&22)
  ms.minimumPulse_US     = 1000;//EEPROMWrite(23, 1000);             //mode1 (in address 23&24)   
  ms.maximumPulse_US     = 2000;//EEPROMWrite(25, 2000);             //mode1 (in address 25&26)
  //ms.telemetryType       = 0; //mode2 0- Rien, 1- FrSky (S-Port), 2- Futaba Sbus, 3- Hitec, 4- Hott, 5- Jeti 6- Spektrum
  ms.nbPales             = 2;   
  ms.moduleMasterOrSlave = 0;   
  ms.fahrenheitDegrees   = 0;
  ms.minimumSpeed        = 1000;//EEPROMWrite(35, 1000);             //minimum motor rpm
  ms.maximumSpeed        = 20000;//EEPROMWrite(37, 20000);            //maximum motor rpm
 */

  //byte id = EEPROMRead(0); // read the first byte from the EEPROM
  if ( ms.ID == 0x99)
  {
    EEPROM.get(0, ms);
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
  Serial << F("EEprom ID: 0x") << _HEX(ms.ID) << endl;
  if (ms.InputMode == 0) Serial << F("PPM mode") << endl;
  if (ms.InputMode == 1) Serial << F("SBUS mode") << endl;
  if (ms.InputMode == 2) Serial << F("IBUS mode") << endl;
  Serial << F("Motor Nb Channel: ") << ms.MotorNbChannel << endl;
  Serial << F("Auxiliary Nb Channel: ") << ms.AuxiliaryNbChannel << endl;
  Serial << F("Rudder Nb Channel: ") << ms.RudderNbChannel << endl;
  Serial << F("Aileron Nb Channel: ") << ms.AileronNbChannel << endl;
  Serial << F("Centre servo1: ") << ms.centerposServo1 << endl;
  Serial << F("Centre servo2: ") << ms.centerposServo2 << endl;
  Serial << F("Position Repos servos 1: ") << ms.idelposServos1 << "us" << endl;
  Serial << F("Position Repos servos 2: ") << ms.idelposServos2 << "us" << endl;
  Serial << F("Vitesse de reponse: ") << ms.responseTime << endl;
  Serial << F("Position maxi servos: ") << ms.fullThrottle << "us" << endl;
  Serial << F("Debut synchro servos: ") << ms.beginSynchro << "us" << endl;
  Serial << F("Position mini general: ") << ms.minimumPulse_US << "us" << endl;
  Serial << F("Position maxi general: ") << ms.maximumPulse_US << "us" << endl;
  if (ms.auxChannel == 1) Serial << F("Connexion Ch AUX: mode ") << ms.auxChannel << F(" (Ch Aux non connecte)") <<  endl;
  if (ms.auxChannel == 2) Serial << F("Connexion Ch AUX: mode ") << ms.auxChannel << F(" (inter 3 positions) ") <<  endl;
  if (ms.auxChannel == 3 || ms.auxChannel == 5 || ms.auxChannel == 6) Serial << F("Connexion Ch AUX: mode ") << ms.auxChannel << F(" (inter 2 positions)") <<  endl;
  if (ms.auxChannel == 4) Serial << F("Connexion Ch AUX: mode ") << ms.auxChannel << F(" (Ch Aux connecte a direction)") << endl;
//  strcpy_P(buff, (char*)pgm_read_word(&(readServoSens[reverseServo1])));
//  Serial << F("Inversion servo1: ") << buff << endl;
//  strcpy_P(buff, (char*)pgm_read_word(&(readServoSens[reverseServo2])));
//  Serial << F("Inversion servo2: ") << buff << endl;
  Serial << F("Difference vitess Err: ") << ms.diffVitesseErr << endl;
//  Serial << F("Adresse I2C LCD: ") << lcdI2Caddress << F(" (0x") << _HEX(lcdI2Caddress) << F(")") << endl;
  Serial << F("Nombre de pales ou d'aimants: ") << _DEC(ms.nbPales) << endl;
  Serial << F("Voltage interne : ") <<  _FLOAT(readVcc() / 1000, 3) << F("v") << endl;
  if (ms.fahrenheitDegrees == 0)
  {
    Serial << F("Temperature interne : ") <<  GetTemp() << _FILL('°', 1) << F("C") << endl;
  }
  else
  { 
    Serial << F("Temperature interne : ") <<  GetTemp() << _FILL('°', 1) << F("F") << endl;
  }

  Serial << F("Mini Motor : ") <<  ms.minimumSpeed << F(" rpm") << endl;
  Serial << F("Maxi Motor : ") <<  ms.maximumSpeed << F(" rpm") << endl;
#ifdef EXTERNALVBATT
  Serial << F("Voltage externe : ") <<  _FLOAT(GetExternalVoltage(), 3) << F("v") << endl;
  battery_Informations();
#endif
}
#endif

void SettingsWriteDefault()
{

  ms.ID                  = 0x99;//EEPROMWrite(0, 0x99);              //write the ID to indicate valid data
  ms.MotorNbChannel      = 3;
  ms.AuxiliaryNbChannel  = 5;
  ms.RudderNbChannel     = 1;
  ms.AileronNbChannel    = 4;
  ms.centerposServo1     = 1500;//EEPROMWrite(1, 1500);              //mode1 (in address 1&2)
  ms.centerposServo2     = 1500;//EEPROMWrite(3, 1500);              //mode1 (in address 3&4)
  ms.idelposServos1      = 1000;//EEPROMWrite(5, 1000);              //mode1 (in address 5&6)
  ms.idelposServos2      = 1000;//EEPROMWrite(7, 1000);              //mode1 (in address 7&8)
  ms.responseTime        = 2   ;//EEPROM.update(9, 2);               //mode2 (= a TAUX_DE_MOYENNAGE) (in address 9&10)
  ms.fullThrottle        = 2000;//EEPROMWrite(11, 2000);             //mode1 (in address 11&12)
  ms.beginSynchro        = 1250;//EEPROMWrite(13, 1250);             //mode1 (in address 13&14)
  ms.auxChannel          = 1;   //EEPROM.update(15, 1);              //mode2 (in address 15&16)
  ms.reverseServo1       = 0;   //EEPROM.update(17, 0);              //mode2 (in address 17&18)
  ms.reverseServo2       = 0;   //EEPROM.update(19, 0);              //mode2 (in address 19&20)
  ms.diffVitesseErr      = 99;  //EEPROMWrite(42, 99);               //mode2 diffVitesseErr (in address 21&22) //difference de vitesse entre les 2 moteurs en tr/mn toleree
  ms.minimumPulse_US     = 1000;//EEPROMWrite(23, 1000);             //mode1 (in address 23&24)   
  ms.maximumPulse_US     = 2000;//EEPROMWrite(25, 2000);             //mode1 (in address 25&26)
  //ms.telemetryType       = 0; //mode2 0- Rien, 1- FrSky (S-Port), 2- Futaba Sbus, 3- Hitec, 4- Hott, 5- Jeti 6- Spektrum
  ms.nbPales             = 2;   
  ms.moduleMasterOrSlave = 0;   
  ms.fahrenheitDegrees   = 0;
  ms.minimumSpeed        = 1000;//EEPROMWrite(35, 1000);             //minimum motor rpm
  ms.maximumSpeed        = 20000;//EEPROMWrite(37, 20000);           //maximum motor rpm
  //PID
  ms.aggKp=100;//aggressive
  ms.aggKi=0.2;//aggressive
  ms.aggKd=1;//aggressive
  ms.consKp=1;//conservative
  ms.consKi=0.05;//conservative
  ms.consKd=0.25;//conservative
                                          
  EEPROM.put(0, ms);

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
  Serial << ms.centerposServo1 << F("|");//array(0)
  Serial << ms.centerposServo2 << F("|");//array(1)
  Serial << ms.idelposServos1 << F("|");//array(2)
  Serial << ms.idelposServos2 << F("|");//array(3)
  Serial << ms.responseTime << F("|");//array(4)
  Serial << ms.fullThrottle << F("|");//array(5)
  Serial << ms.beginSynchro << F("|");//array(6)
  Serial << ms.minimumPulse_US << F("|");//array(7)
  Serial << ms.maximumPulse_US << F("|");//array(8)
  Serial << ms.auxChannel << F("|");//array(9)
  Serial << ms.reverseServo1 << F("|");//array(10)
  Serial << ms.reverseServo2 << F("|");//array(11)
  Serial << ms.diffVitesseErr << F("|");//array(12)
#ifdef LCDON
  Serial << ms.lcdI2Caddress << F("|");//array(13)
#else
  Serial << F("NOTUSED|");//array(13)
#endif
  Serial << _DEC(ms.nbPales) << F("|"); //array(14)
  Serial << 0 << F("|");//switchState (don't used in LCD version -- array(15))
  Serial << _FLOAT(readVcc() / 1000, 3) << F("|"); //array(16)
  Serial <<  GetTemp() << F("|");//array(17)
#ifdef EXTERNALVBATT
  Serial <<  _FLOAT(GetExternalVoltage(), 3) << F("|"); //attention a I2C qui utilise les pins A4 et A5 //array(18)
#else
  Serial << F("NOTUSED|");//array(18)
#endif
  Serial << ms.moduleMasterOrSlave << F("|");//array(19)
  Serial << ms.fahrenheitDegrees << F("|");//array(20)
  Serial << ms.minimumSpeed << F("|");//array(21)
  Serial << ms.maximumSpeed << endl;//array(22)
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
