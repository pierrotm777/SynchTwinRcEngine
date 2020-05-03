/* SyncTwinRcEngine */
/* Compile only with IDE version '1.0.6' or '1.5.7 beta' or '1.5.8 beta' or '1.6.0' to '1.8.2' */

#include <EEPROM.h>
#include <Streaming.h>          /* Librairie remplacant Serial.print() */
#include <SoftRcPulseOut.h>     /* Librairie utilisee pour creer un signal pwm en sortie */
#include "Macros.h"
#include <TinyPpmReader.h>      /* Librairie utilisee pour lire un signal ppm en entree */
#include <Rcul.h>
#include <SoftSerial.h>

// software Serial #1: TX = digital pin 2, RX = digital pin 3
//SoftSerial Serial(10,11);

#include <FlySkyIBus.h>

// A TESTER I2C EEPROM pour recorder https://www.hobbytronics.co.uk/arduino-external-eeprom

#define FirmwareVersion 0.5


//#define DEBUG
//#define SECURITYENGINE          /* Engines security On/off */
#define ARDUINO2PC                /* PC interface (!!!!!! don't use this option with SerialPLOTTER or READ_Button_AnalogPin !!!!!!) */
//#define EXTERNALVBATT             /* Read external battery voltage */
//#define GLOWMANAGER             /* Glow driver */
//#define PIDCONTROL              /* Use PID control for define the variable stepMotor in SynchroMotors */
//#define I2CSLAVEFOUND           /* for command a second module by the I2C port */
//#define INT_REF                 /* internal 1.1v reference */
//#define SerialPLOTTER           /* Multi plot in IDE (don't use this option with ARDUINO2PC) */
//#define RECORDER                /* L'enregistreur est déplacé dans VB */
//#define TELEMETRY_FRSKY           /* Frsky S-PORT Telemetry for VOLTAGE,RPM and TEMP */

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
9     Telemetry Frsky S-Port

10    Led Red/Setting's Port RX
11    Led Red/Setting's Port TX
12    Led Yellow
13    Led Yellow

A0    Led Green
A1    Led Green
A2    Led Yellow
A3    Led Yellow//External power V+
A4    SDA // Connexion SD I2C
A5    SCL // Connexion SD I2C
A6    
A7    

*/

#ifdef TELEMETRY_FRSKY
#include "FrSkySportSensor.h"
#include "FrSkySportSensorAss.h"
#include "FrSkySportSensorFcs.h"
//#include "FrSkySportSensorFlvss.h"
//#include "FrSkySportSensorGps.h"
#include "FrSkySportSensorRpm.h"
//#include "FrSkySportSensorSp2uart.h"
//#include "FrSkySportSensorVario.h"
#include "FrSkySportSingleWireSerial.h"
#include "FrSkySportDecoder.h"
#ifdef POLLING_ENABLED
FrSkySportDecoder decodFrsky(true);                     // Create decoder object with polling
#else
FrSkySportDecoder decodFrsky;                           // Create decoder object without polling
#endif
FrSkySportSensorAss ass;                               // Create ASS sensor with default ID
FrSkySportSensorFcs fcs;                               // Create FCS-40A sensor with default ID (use ID8 for FCS-150A)
//FrSkySportSensorFlvss flvss1;                          // Create FLVSS sensor with default ID
//FrSkySportSensorFlvss flvss2(FrSkySportSensor::ID15);  // Create FLVSS sensor with given ID
//FrSkySportSensorGps gps;                               // Create GPS sensor with default ID
FrSkySportSensorRpm rpm;                               // Create RPM sensor with default ID
//FrSkySportSensorSp2uart sp2uart;                       // Create SP2UART Type B sensor with default ID
//FrSkySportSensorVario vario;                           // Create Variometer sensor with default ID
#endif

/* Select your radio's channels order */
#define AETR

//Channel order
#ifdef AETR
  #define AILERON  0
  #define ELEVATOR 1
  #define THROTTLE 2
  #define RUDDER   3
#endif
#ifdef AERT
  #define AILERON  0
  #define ELEVATOR 1
  #define THROTTLE 3
  #define RUDDER   2
#endif
#ifdef ARET
  #define AILERON  0
  #define ELEVATOR 2
  #define THROTTLE 3
  #define RUDDER   1
#endif
#ifdef ARTE
  #define AILERON  0
  #define ELEVATOR 3
  #define THROTTLE 2
  #define RUDDER   1
#endif
#ifdef ATRE
  #define AILERON  0
  #define ELEVATOR 3
  #define THROTTLE 1
  #define RUDDER   2
#endif
#ifdef ATER
  #define AILERON  0
  #define ELEVATOR 2
  #define THROTTLE 1
  #define RUDDER   3
#endif

#ifdef EATR
  #define AILERON  1
  #define ELEVATOR 0
  #define THROTTLE 2
  #define RUDDER   3
#endif
#ifdef EART
  #define AILERON  1
  #define ELEVATOR 0
  #define THROTTLE 3
  #define RUDDER   2
#endif
#ifdef ERAT
  #define AILERON  2
  #define ELEVATOR 0
  #define THROTTLE 3
  #define RUDDER   1
#endif
#ifdef ERTA
  #define AILERON  3
  #define ELEVATOR 0
  #define THROTTLE 2
  #define RUDDER   1
#endif
#ifdef ETRA
  #define AILERON  3
  #define ELEVATOR 0
  #define THROTTLE 1
  #define RUDDER   2
#endif
#ifdef ETAR
  #define AILERON  2
  #define ELEVATOR 0
  #define THROTTLE 1
  #define RUDDER   3
#endif

#ifdef TEAR
  #define AILERON  2
  #define ELEVATOR 1
  #define THROTTLE 0
  #define RUDDER   3
#endif
#ifdef TERA
  #define AILERON  3
  #define ELEVATOR 1
  #define THROTTLE 0
  #define RUDDER   2
#endif
#ifdef TREA
  #define AILERON  3
  #define ELEVATOR 2
  #define THROTTLE 0
  #define RUDDER   1
#endif
#ifdef TRAE
  #define AILERON  2
  #define ELEVATOR 3
  #define THROTTLE 0
  #define RUDDER   1
#endif
#ifdef TARE
  #define AILERON  1
  #define ELEVATOR 3
  #define THROTTLE 0
  #define RUDDER   2
#endif
#ifdef TAER
  #define AILERON  1
  #define ELEVATOR 2
  #define THROTTLE 0
  #define RUDDER   3
#endif

#ifdef RETA
  #define AILERON  3
  #define ELEVATOR 1
  #define THROTTLE 2
  #define RUDDER   0
#endif
#ifdef REAT
  #define AILERON  2
  #define ELEVATOR 1
  #define THROTTLE 3
  #define RUDDER   0
#endif
#ifdef RAET
  #define AILERON  1
  #define ELEVATOR 2
  #define THROTTLE 3
  #define RUDDER   0
#endif
#ifdef RATE
  #define AILERON  1
  #define ELEVATOR 3
  #define THROTTLE 2
  #define RUDDER   0
#endif
#ifdef RTAE
  #define AILERON  2
  #define ELEVATOR 3
  #define THROTTLE 1
  #define RUDDER   0
#endif
#ifdef RTEA
  #define AILERON  3
  #define ELEVATOR 2
  #define THROTTLE 1
  #define RUDDER   0
#endif

//affectation des pins des entrees RX et sorties servos
#define BROCHE_PPMINPUT         0    /* Multiplex RX pinout 2 */
#define BROCHE_SENSOR1          2    /* Hall or IR motor 1 */
#define BROCHE_SENSOR2          3    /* Hall or IR motor 2 */
#define BROCHE_MOTOR1           4    /* Servo motor 1 */
#define BROCHE_MOTOR2           5    /* Servo motor 2 */
#define BROCHE_RUDDER           6    /* Servo rudder */

#ifdef GLOWMANAGER
#define BROCHE_GLOW1            7  /* Glow driver motor 1 (PD7)*/ 
#define BROCHE_GLOW2            8  /* Glow driver motor 2 (PB0)*/
#endif

#ifdef EXTERNALVBATT
#define BROCHE_BATTEXT          A3  /* External battery voltage (V+) */
#endif

#define LED_SIGNAL_FOUND      250
#define LED_SIGNAL_NOTFOUND   1000
#define LED                   5,B // declare LED in PCB5 (D13)
//#define LED1GREEN             2,B //D10
//#define LED2GREEN             3,B //D11
#define LED1RED               4,B //D12
#define LED2RED               0,C //A0
#define LED1YELLOW            1,C //A1
#define LED2YELLOW            2,C //A2


boolean synchroIsActive = false;
boolean glowControlIsActive = false;
boolean SecurityIsON = false;
boolean simulateSpeed = false;
boolean RunLogInSDCard = false;

/* Variables Chronos*/
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
         
//#define Serial_SUM_PPM         PITCH,YAW,THROTTLE,ROLL,AUX1,AUX2,AUX3,AUX4,8,9,10,11 //For Graupner/Spektrum
#define Serial_SUM_PPM         ROLL,PITCH,THROTTLE,YAW,AUX1,AUX2,AUX3,AUX4,8,9,10,11 //For Robe/Hitec/Futaba
//#define Serial_SUM_PPM         ROLL,PITCH,YAW,THROTTLE,AUX1,AUX2,AUX3,AUX4,8,9,10,11 //For Multiplex
//#define Serial_SUM_PPM         PITCH,ROLL,THROTTLE,YAW,AUX1,AUX2,AUX3,AUX4,8,9,10,11 //For some Hitec/Sanwa/Others

#if defined(Serial_SUM_PPM) //Channel order for PPM SUM RX Configs
  static uint8_t rcChannel[RC_CHANS] = {Serial_SUM_PPM};
#endif
*/

/** La structure permettant de stocker les données */
//https://www.carnetdumaker.net/articles/stocker-des-donnees-en-memoire-eeprom-avec-une-carte-arduino-genuino/
struct MaStructure {
  /* Valeurs par defaut dans EEprom */
  byte ID;
  uint8_t InputMode;                               //0-PPM, 1-SBUS, 2,IBUS
  uint8_t radioRcMode;                             //Rc mode 1 to 4
  uint8_t AuxiliaryNbChannel;                      //default is channel 5
  uint16_t centerposServo1;                        //value in uS
  uint16_t centerposServo2;                        //value in uS
  uint16_t idelposServos1;//600                    //value in uS
  uint16_t idelposServos2;//600                    //value in uS
  uint8_t responseTime;                            //mode2 (= a TAUX_DE_MOYENNAGE) (in address 4)
  uint16_t fullThrottle;//2400                     //value in uS
  uint16_t beginSynchro;//                         //value in uS
  uint8_t auxChannel;                              //1 to 4
  uint8_t reverseServo1;                           //0(normal), 1(reverse)
  uint8_t reverseServo2;                           //0(normal), 1(reverse)
  double diffVitesseErr;                           //difference de vitesse entre les 2 moteurs en tr/mn toleree
  uint16_t minimumPulse_US;                        //value in uS
  uint16_t maximumPulse_US;                        //value in uS
  uint8_t telemetryType;                           //mode2 0- Rien, 1- FrSky (S-Port), 2- Futaba Sbus, 3- Hitec, 4- Hott, 5- Jeti 6- Spektrum
  uint8_t nbPales;                                 //number of blades or nb of magnets
  uint8_t moduleMasterOrSlave;                     //0 = module maitre , 1= module esclave
  uint8_t fahrenheitDegrees;                       //0 = C degrees , 1= Fahrenheit degrees
  uint16_t minimumSpeed;                           //value in uS
  uint16_t maximumSpeed;                           //value in uS
}; // Ne pas oublier le point virgule !

MaStructure ms;

uint8_t MotorNbChannel;
uint8_t RudderNbChannel;
uint8_t AileronNbChannel;

/* comptage tr/mn */
volatile uint16_t FirstInputChangeCount=0, SecondInputChangeCount=0;
uint8_t VirtualPortNb, VirtualPortNb_;
uint16_t vitesse1, vitesse2;                    //blades speeds in rpm
//double diffVitesseErr;                          //difference de vitesse entre les 2 moteurs en tr/mn toleree
double diffVitesse;                             //difference de vitesse entre les 2 moteurs en tr/mn (peut etre negatif !!!)
double stepMotor;                               //nb de micro secondes ajoutes ou enleves
int readings_V1[5];                             //averaging last 5 readings_V1
int readings_V2[5];                             //averaging last 5 readings_V2

int index = 0;

//#ifdef PIDCONTROL
////http://www.ferdinandpiette.com/blog/2012/04/asservissement-en-vitesse-dun-moteur-avec-arduino/
//#include <SimpleTimer.h>
//
//#endif

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

#define SERIAL_BAUD         115200         /* 115200 is need for use BlueSmirF BT module */
#ifdef ARDUINO2PC
#define LONGUEUR_MSG_MAX   75             /* ex: 1500,1500,1000,1000,2,2000,1250,1000,2000,1,1,0,99,2,0,0,0,1000,20000,0,0 */
#define RETOUR_CHARRIOT    0x0D           /* CR (code ASCII) */
#define PASSAGE_LIGNE      0x0A           /* LF (code ASCII) */
#define BACK_SPACE         0x08
char Message[LONGUEUR_MSG_MAX + 1];
uint8_t SubStrNb, SeparFound;
#define SUB_STRING_NB_MAX  20
char *StrTbl[SUB_STRING_NB_MAX];          /* declaration de pointeurs sur chaine, 1 pointeur = 2 octets seulement */
#endif
int pos = 0;

//#ifdef I2CSLAVEFOUND
//#include <Wire.h>               /* Interface LCD I2C, SDA = A4, SCL = A5) */
//#define SLAVE_ADRESS      20
//#endif

boolean RunConfig = false;
unsigned long startedWaiting = millis();
unsigned long started1s = millis();


void setup()
{
  Serial.begin(SERIAL_BAUD);//bloque la lecture des pins 0 et 1
  while (!Serial);// wait for Serial port to connect.
  
  if (RunConfig == false)
  {
    String sdata="";
    Serial << F("Wait Return");
    byte ch;
    while(millis() - startedWaiting <= 5000) //waiting 5s return key
    { 
      /* Check 1s */
      if(millis()-started1s>=1000)
      {
        Serial << F(".");started1s=millis();
      }
      if(Serial.available() > 0)
      {
        ch = Serial.read();
        sdata += (char)ch;
        if (ch=='\r')
        {
          sdata.trim(); // Process command in sdata.
          sdata = ""; // Clear the string ready for the next command.
          RunConfig = true;
          break;
        }        
      }   
    }
  }

//*********************
//  RunConfig = true;
//*********************

  (RunConfig == true?Serial << endl << endl << F("Configuration mode is actived") << endl:Serial << endl << endl << F("Starting without configuration") << endl);
  
  

#ifdef TELEMETRY_FRSKY// telemetrie sur ici pin 12 (pin 2 à 12 possibles)
  //decodFrsky.begin(FrSkySportSingleWireSerial::SOFT_Serial_PIN_12, &ass, &fcs, &flvss1, &flvss2, &gps, &rpm, &sp2uart, &vario);
  decodFrsky.begin(FrSkySportSingleWireSerial::SOFT_Serial_PIN_9, &ass, &fcs, &rpm );
#endif
  
//#ifdef I2CSLAVEFOUND
//  Wire.begin(SLAVE_ADRESS,2);
//#endif

//#ifdef PIDCONTROL
//#endif
  
  /* init pins leds in output */
  out(LED);
//  out(LED1GREEN);out(LED2GREEN);
  out(LED1RED);out(LED2RED);
  out(LED1YELLOW);out(LED2YELLOW);
  
#ifdef GLOWMANAGER
  //initialisation du chauffage des bougies
  //PIN_OUTPUT(D,BROCHE_GLOW1);PIN_OUTPUT(D,BROCHE_GLOW2);
#endif  


  readAllEEprom();//read all settings from EEprom (save default's values in first start)

  AileronNbChannel = AILERON + 1;
  MotorNbChannel   = THROTTLE + 1;
  RudderNbChannel  = RUDDER + 1;
   
  //initialise les capteurs effet hall ou IR avec une interruption associee
  TinyPinChange_Init();
  VirtualPortNb=TinyPinChange_RegisterIsr(BROCHE_SENSOR1, InterruptFunctionToCall);
  VirtualPortNb_=TinyPinChange_RegisterIsr(BROCHE_SENSOR2, InterruptFunctionToCall);
  /* Enable Pin Change for each pin */
  TinyPinChange_EnablePin(BROCHE_SENSOR1);
  TinyPinChange_EnablePin(BROCHE_SENSOR2);
  

#ifdef DEBUG
  Serial << F("SynchTwinRcEngine est demarre") << endl << endl;
#endif//endif DEBUG

  switch (ms.InputMode)//CPPM,SBUS or IBUS
  {
    case CPPM:
#ifdef DEBUG
        Serial << F("CPPM selected") << endl;
#endif       
        blinkNTime(1,125,250);
        if (RunConfig == true)
        {
          TinyPpmReader.attach(9); // Attach TinyPpmReader to pin 9 (Telemetry for settings mode with VB only)
        }
        else
        {
          Serial.flush();delay(500); // wait for last transmitted data to be sent
          TinyPpmReader.attach(BROCHE_PPMINPUT); // Attach TinyPpmReader to SIGNAL_INPUT_PIN pin 
        }          
      break;
    case SBUS:
#ifdef DEBUG
        Serial << F("SBUS selected") << endl;
#endif
        blinkNTime(2,125,250);
        if (RunConfig == false)
        {
          Serial.flush();delay(500); // wait for last transmitted data to be sent
          Serial.begin(100000, SERIAL_8E2);// Attention ! Bloque la communication avec VB.
        }
      break;
    case IBUS:
#ifdef DEBUG
        Serial << F("IBUS selected") << endl;
#endif       
        blinkNTime(3,125,250);
        if (RunConfig == false)
        {             
          Serial.flush();delay(500); // wait for last transmitted data to be sent
          IBus.begin(Serial);// Attention ! Bloque la communication avec VB.
        }
      break;
  }

#ifdef DEBUG
  readAllEEpromOnSerial();//lecture configuration du module dans le terminal serie
#endif//endif DEBUG

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
}//fin setup


void loop()
{

  if (RunConfig == true)
  {
#ifdef ARDUINO2PC
    SerialFromToVB();
#endif
  }
  
    mode0();/* main mode launched if no buttons pressed during start */    

}//fin loop

void readAllEEprom()
{

/*
  ms.ID                  = 0x99;//write the ID to indicate valid data
  ms.InputMode           = 1;   //0-PPM, 1-SBUS, 2,IBUS
  ms.radioRcMode         = 1;   // mode is 1 to 4
  ms.AuxiliaryNbChannel  = 5;
  ms.centerposServo1     = 1500;
  ms.centerposServo2     = 1500;
  ms.idelposServos1      = 1000;
  ms.idelposServos2      = 1000;
  ms.responseTime        = 2   ;
  ms.fullThrottle        = 2000;
  ms.beginSynchro        = 1250;
  ms.auxChannel          = 1;   
  ms.reverseServo1       = 1;
  ms.reverseServo2       = 0;
  ms.diffVitesseErr      = 99;//difference de vitesse entre les 2 moteurs en tr/mn toleree
  ms.minimumPulse_US     = 1000;
  ms.maximumPulse_US     = 2000;
  ms.telemetryType       = 0; //mode2 0- Rien, 1- FrSky (S-Port), 2- Futaba Sbus, 3- Hitec, 4- Hott, 5- Jeti 6- Spektrum
  ms.nbPales             = 2;   
  ms.moduleMasterOrSlave = 0;   
  ms.fahrenheitDegrees   = 0;
  ms.minimumSpeed        = 1000;//minimum motor rpm
  ms.maximumSpeed        = 20000;//maximum motor rpm
  ms.InputMode           = 0;//CPPM defaut
 */
 
  EEPROM.get(0,ms);// Read all EEPROM settings in one time
  if ( ms.ID != 0x99)
  {
    blinkNTime(5,100,100);
    SettingsWriteDefault();
    waitMs(500);
    readAllEEprom();
  }
//  else
//  {
//    //EEPROM Ok!
//    waitMs(2000);
//    blinkNTime(5,LED_SIGNAL_FOUND,LED_SIGNAL_FOUND);
//  }
}


#ifdef DEBUG
void readAllEEpromOnSerial()
{
  Serial << endl;
  Serial << F("Chargement valeurs EEPROM ...") << endl << endl;
  Serial << F("EEprom ID: 0x") << _HEX(ms.ID) << endl;
  if (ms.InputMode == 0) Serial << F("PPM mode") << endl;
  if (ms.InputMode == 1) Serial << F("SBUS mode") << endl;
  if (ms.InputMode == 2) Serial << F("IBUS mode") << endl;
  Serial << F("Radio Rc Mode: ") << ms.radioRcMode << endl;
  Serial << F("Auxiliary Nb Channel: ") << ms.AuxiliaryNbChannel << endl;
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
  Serial << F("Difference vitess Err: ") << ms.diffVitesseErr << endl;
  Serial << F("Nombre de pales ou d'aimants: ") << _DEC(ms.nbPales) << endl;
  Serial << F("Voltage interne : ") <<  _FLOAT(readVcc() / 1000, 3) << F("v") << endl;
  if (ms.fahrenheitDegrees == 0)
  {
    Serial << F("Temperature interne : ") <<  GetTemp() << "\xC2\xB0" << F("C") << endl;
  }
  else
  { 
    Serial << F("Temperature interne : ") <<  GetTemp() << "\xC2\xB0" << F("F") << endl;
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
/*
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
 */

  ms.ID                  = 0x99;//write the ID to indicate valid data
  ms.InputMode           = 0;   //0-PPM, 1-SBUS, 2,IBUS
  ms.radioRcMode         = 1;   // mode is 0 to 3 (mode 1 à 4)
  ms.AuxiliaryNbChannel  = 5;
  ms.centerposServo1     = 1500;
  ms.centerposServo2     = 1500;
  ms.idelposServos1      = 1000;
  ms.idelposServos2      = 1000;
  ms.responseTime        = 2   ;
  ms.fullThrottle        = 2000;
  ms.beginSynchro        = 1250;
  ms.auxChannel          = 1;   
  ms.reverseServo1       = 1;
  ms.reverseServo2       = 0;
  ms.diffVitesseErr      = 99;//difference de vitesse entre les 2 moteurs en tr/mn toleree
  ms.minimumPulse_US     = 1000;
  ms.maximumPulse_US     = 2000;
  ms.telemetryType       = 0; //mode2 0- Rien, 1- FrSky (S-Port), 2- Futaba Sbus, 3- Hitec, 4- Hott, 5- Jeti 6- Spektrum
  ms.nbPales             = 2;   
  ms.moduleMasterOrSlave = 0;   
  ms.fahrenheitDegrees   = 0;
  ms.minimumSpeed        = 1000;//minimum motor rpm
  ms.maximumSpeed        = 20000;//maximum motor rpm
  //PID

                                          
  EEPROM.put(0, ms);
  blinkNTime(5,LED_SIGNAL_FOUND,LED_SIGNAL_FOUND);
}

#ifdef PIDCONTROL
// ************************************************
// Save PID parameter changes to EEPROM
// ************************************************
void SaveParameters()
{
}
#endif

void clearEEprom()// write a 0 to all 512 bytes of the EEPROM
{
  for (int i = 0; i < 512; i++)
  {
    EEPROM.update(i, 0);
  }
  blinkNTime(5,LED_SIGNAL_FOUND,LED_SIGNAL_FOUND);
}

void sendConfigToSerial()
{ //format send: 1657|1657|1225|1225|2|2075|1393|1070|2205|1|0|0|100|39|2|0|5.00|27.61|1.000|0|1000|20000|0
  //EEPROM.get(0,ms);
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
  Serial << F("0|");//array(13) NON UTILISé
  Serial << _DEC(ms.nbPales) << F("|"); //array(14)
  Serial << ms.radioRcMode << F("|");//array(15))
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
  Serial << ms.maximumSpeed << F("|");//array(22)
  Serial << ms.InputMode << endl;//array(23)
  
  Serial.flush(); // clear Serial port
}


float readVcc() {// see http://provideyourown.com/2012/secret-arduino-voltmeter-measure-battery-voltage/
  // Read 1.1V reference against AVcc
  // set the reference to Vcc and the measurement to the internal 1.1V reference
#if defined(__AVR_ATmega32U4__)
  ADMUX = _BV(REFS0) | _BV(MUX4) | _BV(MUX3) | _BV(MUX2) | _BV(MUX1);
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
  (ms.fahrenheitDegrees == 0) ? t : t = (t * 9 / 5) + 32; //T(°F) = T(°C) × 9/5 + 32
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

// used for flashing a pin
void blinkNTime(int count, int onInterval, int offInterval)
{
  byte i;
  for (i = 0; i < count; i++) 
  {
    waitMs(offInterval);
    on(LED);      //     turn on LED//digitalWrite(LED_PIN,HIGH);
    waitMs(onInterval);
    off(LED);      //     turn on LED//digitalWrite(LED_PIN,LOW);  
  }
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


//int const bchout_ar[]  = {'AETR','AERT','ARET','ARTE'};//,'ATRE','ATER','EATR','EART','ERAT','ERTA','ETRA','ETAR','TEAR','TERA','TREA','TRAE','TARE','TAER','RETA','REAT','RAET','RATE','RTAE','RTEA'};
//char channel_order(uint8_t x)
//{
//  return bchout_ar[x] ;
//}
