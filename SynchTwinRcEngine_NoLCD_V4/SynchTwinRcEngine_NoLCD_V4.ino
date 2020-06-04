/* SyncTwinRcEngine */
/* Compile only with IDE version '1.0.6' or '1.5.7 beta' or '1.5.8 beta' or '1.6.0' to '1.8.13' */

#include <EEPROM.h>
#include <Streaming.h>          /* Librairie remplacant SettingsPort.print() */
#include <TinyPinChange.h>
#include <SoftRcPulseOut.h>     /* Librairie utilisee pour creer un signal pwm en sortie */
//Input libraries
#include <TinyCppmReader.h>
#include <RcBusRx.h>
#define DISPLAY_EVERY_SERIAL_FRAME_NB   40 // To not flood the serial console!

/* Select your radio's channels order (see Marcos.h for other modes) */
typedef struct{
  uint8_t  Aileron; 
  uint8_t  Elevator; 
  uint8_t  Throttle; 
  uint8_t  Rudder; 
}ChannelOrderSt_t;
enum {AETR = 0, AERT,ARET,ARTE,ATRE,ATER,EATR,EART,ERAT,ERTA,ETRA,ETAR,TEAR,TERA,TREA,TRAE,TARE,TAER,RETA,REAT,RAET,RATE,RTAE,RTEA};
enum {AILERON = 0, ELEVATOR, THROTTLE, RUDDER};

const ChannelOrderSt_t ChannelOrder[] PROGMEM = {
                 /* AETR */   {0, 1, 2, 3},
                 /* AERT */   {0, 1, 3, 2},
                 /* ARET */   {0, 2, 3, 1},
                 /* ARTE */   {0, 3, 2, 1},
                 /* ATRE */   {0, 3, 1, 2},
                 /* ATER */   {0, 2, 1, 3},
                 /* EATR */   {1, 0, 2, 3},
                 /* EART */   {1, 0, 3, 2},
                 /* ERAT */   {2, 0, 3, 1},
                 /* ERTA */   {3, 0, 2, 1},
                 /* ETRA */   {3, 0, 1, 2},
                 /* ETAR */   {2, 0, 1, 3},
                 /* TEAR */   {2, 1, 0, 3},
                 /* TERA */   {3, 1, 0, 2},
                 /* TREA */   {3, 2, 0, 1},
                 /* TRAE */   {2, 3, 0, 1},
                 /* TARE */   {1, 3, 0, 2},
                 /* TAER */   {1, 2, 0, 3},
                 /* RETA */   {3, 1, 2, 0},
                 /* REAT */   {2, 1, 3, 0},
                 /* RAET */   {1, 2, 3, 0},
                 /* RATE */   {1, 3, 2, 0},
                 /* RTAE */   {2, 3, 1, 0},
                 /* RTEA */   {3, 2, 1, 0}
                            };

//#define AETR
#include "Macros.h"

#include <SoftSerial.h>
// software SettingsPort #1: RX = digital pin 10, TX = digital pin 11
SoftSerial SettingsPort(10,11);

// A TESTER I2C EEPROM pour recorder https://www.hobbytronics.co.uk/arduino-external-eeprom
// et ici http://heliosoph.mit-links.info/512kb-eeprom-atmega328p/

#define FirmwareVersion 0.6


//#define DEBUG
#define SECURITYENGINE          /* Engines security On/off */
#define ARDUINO2PC              /* PC interface (!!!!!! don't use this option with SettingsPortPLOTTER or READ_Button_AnalogPin !!!!!!) */
#define EXTERNALVBATT           /* Read external battery voltage */
#define GLOWMANAGER             /* Glow driver */
//#define I2CSLAVEFOUND           /* for command a second module by the I2C port */
#define INT_REF                 /* internal 1.1v reference */
//#define SerialPLOTTER           /* Multi plot in IDE (don't use this option with ARDUINO2PC) */
#define RECORDER                /* L'enregistreur est déplacé dans VB */
//#define TELEMETRY_FRSKY           /* Frsky S-PORT Telemetry for VOLTAGE,RPM and TEMP */
//#define FRAM_USED
#define EXTLED                /* bloque le port serie settings pins 10-11*/

/*
0     INPUT PPM
1     Led Red
2     Hall or IR motor 1 
3     Hall or IR motor 2 
4     Servo motor 1 
5     Servo motor 2 
6     Servo rudder 
7     Glow driver motor 1
8     Glow driver motor 2
9     Telemetry Frsky S-Port

10    Setting's Port RX
11    Setting's Port TX
12    Led Yellow
13    Led Yellow (Pro Mini LED)

A0    Led Green
A1    Led Green
A2    Led Yellow
A3    Led Yellow
A4    SDA // Connexion SD I2C
A5    SCL // Connexion SD I2C
A6    Led Red
A7    External power V+

*/

//affectation des pins des entrees RX et sorties servos
#define BROCHE_PPMINPUT         0    /* PPM,SBUS,SRXL,SUMD,IBUS or JETI Input */
#define BROCHE_SENSOR1          2    /* Hall or IR motor 1 */
#define BROCHE_SENSOR2          3    /* Hall or IR motor 2 */
#define BROCHE_MOTOR1           4    /* Servo motor 1 */
#define BROCHE_MOTOR2           5    /* Servo motor 2 */
#define BROCHE_RUDDER           6    /* Servo rudder */

// http://philsradial.blogspot.com/2013/02/glowplug-driver.html
#ifdef GLOWMANAGER
//Declare the Pin(s) used in "TinySoftPwm.h"
//In this sketch, #define TINY_SOFT_PWM_USES_P7 and #define TINY_SOFT_PWM_USES_P7 must be enabled (not commented).
#include <TinySoftPwm.h>
#define BROCHE_GLOW1            7  /* Glow driver motor 1 (PD7)*/ 
#define BROCHE_GLOW2            8  /* Glow driver motor 2 (PB0)*/
bool GlowDriverInUse = false;
#endif

// Frsky Telemetry input pin    9 /* see decodFrsky.begin(FrSkySportSingleWireSerial::SOFT_SERIAL_PIN_9, &ass, &fcs, &rpm ); */


#ifdef EXTERNALVBATT
#define BROCHE_BATTEXT          A7  /* External battery voltage (V+) */
#endif

#define LED_SIGNAL_FOUND      250
#define LED_SIGNAL_NOTFOUND   1000
#define LED                   5,B // declare LED in PCB5 (D13)

#ifdef EXTLED
#define LED1RED     /*LED1*/  2,B //D10 /*Flashes Status of Glowplug driver 1*/
#define LED2RED     /*LED2*/  3,B //D11 /*Flashes Status of Glowplug driver 2*/ 
#define LED1GREEN   /*LED5*/  0,C //A0 /*RPM Sensor for Engine1*/
#define LED2GREEN   /*LED6*/  1,C //A1 /*RPM Sensor for Engine2*/
#define LED1YELLOW  /*LED3*/  2,C //A2 /*On when device is managing sync and both engines running*/
#define LED2YELLOW  /*LED4*/  3,C //A3 /*On when Transmitter stick above 1/5 throttle. Off when transmitter stick below 1/5th throttle.*/
//Atmega328PB (A6(PE2) et A7(PE3) sont aussi dispo)
#endif

boolean RunConfig = true;
boolean CheckIfVBUsed = false;
boolean ServoRecorderIsON = false;
boolean synchroIsActive = false;
boolean glowControlIsActive = false;
boolean SecurityIsON = false;
boolean simulateSpeed = false;

unsigned long startedWaiting = millis();
unsigned long started1s = millis();
unsigned long ShutDownSerialSetting = millis();
unsigned long LedRedStartMs = millis();
static uint32_t StartPwmUs=micros();

/* Variables Chronos*/
uint32_t BeginIdleMode;//=0;
uint32_t ReadCaptorsMs;//=millis();
uint32_t BeginSecurityMs;//=millis();
uint32_t LedStartMs;//=millis();
uint32_t SendMotorsToVBMs;//=millis();

enum { CPPM=0, SBUS, SRXL, SUMD, IBUS, JETIEX };
bool InputSignalExist = false;

#ifdef RECORDER
uint8_t recoderMode /*= 1*/;
bool releaseButtonMode = false;
#endif

/** La structure permettant de stocker les données */
//https://www.carnetdumaker.net/articles/stocker-des-donnees-en-memoire-eeprom-avec-une-carte-arduino-genuino/
//struct __attribute__ ((packed)) MaStructure {
struct MaStructure {
  /* Valeurs par defaut dans EEprom */
  byte ID;
  uint8_t InputMode;                               //0-PPM, 1-SBUS, 2-SRXL, 3-SUMD, 4-IBUS, 5-JETI
//  uint8_t radioRcMode;                             //Rc mode 1 to 4
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
  uint8_t channelsOrder;                           //AETR(AILERON,ELEVATOR,THROTTLE,RUDDER),AERT(AILERON,ELEVATOR,RUDDER,THROTTLE),ARET(AILERON,RUDDER,ELEVATOR,THROTTLE) etc...
  float coeff_division;                            //4.0
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


/* ******************************************************************************
 * !!!!!! DOIT ETRE AU MINIMUM POUR BLOQUER LES HELICES SI PAS DE SIGNAL !!!!!! *
 ********************************************************************************  */
uint16_t Width_us = 1000;
uint16_t WidthAux_us = 1000;
uint16_t WidthRud_us = 1000;
uint16_t WidthAil_us = 1000;

/* Object TinyCppmReader creation */
TinyCppmReader TinyCppmReader; 
/* Creation des objets Sorties servos */
SoftRcPulseOut ServoMotor1;               /* Servo Engine 1 */
SoftRcPulseOut ServoMotor2;               /* Servo Engine 2 */
SoftRcPulseOut ServoRudder;

#define SERIAL_BAUD         115200         /* 115200 is need for use BlueSmirF BT module */
#ifdef ARDUINO2PC
#define LONGUEUR_MSG_MAX   48             /* ex: S1,1500,1500,1000,1000,2,2000,1250,1000,2000,1 ou S2,1,0,99,2,0,0,0,1000,20000,0,0 */
#define RETOUR_CHARRIOT    0x0D           /* CR (code ASCII) */
#define PASSAGE_LIGNE      0x0A           /* LF (code ASCII) */
#define BACK_SPACE         0x08
char Message[LONGUEUR_MSG_MAX + 1];
uint8_t SubStrNb, SeparFound;
#define SUB_STRING_NB_MAX  13//23             /* nombre de valeurs splitées */
char *StrTbl[SUB_STRING_NB_MAX];          /* declaration de pointeurs sur chaine, 1 pointeur = 2 octets seulement */
#endif
uint16_t pos = 0;

#ifdef I2CSLAVEFOUND
#include <Wire.h>               /* Interface LCD I2C, SDA = A4, SCL = A5) */
#define SLAVE_ADRESS      20
#endif


//https://www.rcgroups.com/forums/showthread.php?2245978-FrSky-S-Port-telemetry-library-easy-to-use-and-configurable
#ifdef TELEMETRY_FRSKY
//#define FASTSERIAL
//#include <FastSerial.h>
//FastSerialPort0(Serial);
#include "FrSkySportSensorRpm.h"
#include "FrSkySportSingleWireSerial.h"
#include "FrSkySportTelemetry.h"
FrSkySportSensorRpm rpm;             // Create RPM sensor with default ID
FrSkySportTelemetry decodFrsky;      // Create telemetry object without polling
#endif

void setup()
{

//  clearEEprom();

#ifdef FRAM_USED
  setupFRAM();
#endif

  Serial.begin(115200);

  SettingsPort.begin(57600);
  delay(500);//while (SettingsPort.available() > 0)

#ifdef DEBUG
  SettingsPort << F("SynchTwinRcEngine est demarre") << endl << endl;
#endif//endif DEBUG

#ifdef TELEMETRY_FRSKY// telemetrie sur ici pin 12 (pin 2 à 12 possibles)
  //decodFrsky.begin(FrSkySportSingleWireSerial::SOFT_SettingsPort_PIN_12, &ass, &fcs, &flvss1, &flvss2, &gps, &rpm, &sp2uart, &vario);
  decodFrsky.begin(FrSkySportSingleWireSerial::SOFT_SERIAL_PIN_9,&rpm );
#endif
  
//#ifdef I2CSLAVEFOUND
//  Wire.begin(SLAVE_ADRESS,2);
//#endif
 
  readAllEEprom();//read all settings from EEprom (save default's values in first start)

  AileronNbChannel = (uint8_t)pgm_read_byte(&ChannelOrder[ms.channelsOrder].Aileron) + 1;//AILERON + 1;
  MotorNbChannel   = (uint8_t)pgm_read_byte(&ChannelOrder[ms.channelsOrder].Throttle) + 1;//THROTTLE + 1;
  RudderNbChannel  = (uint8_t)pgm_read_byte(&ChannelOrder[ms.channelsOrder].Rudder) + 1;//RUDDER + 1;
#ifdef DEBUG
    SettingsPort << F("Aileron")<< F("=") << AileronNbChannel << endl;
//    SettingsPort << F("Elevator")<< F("=")<< (uint8_t)pgm_read_byte(&ChannelOrder[Mode].Elevator) << endl;
    SettingsPort << F("Throttle")<< F("=")<< MotorNbChannel << endl;
    SettingsPort << F("Rudder")<< F("=") << RudderNbChannel << endl << endl;

#endif
   
  //initialise les capteurs effet hall ou IR avec une interruption associee
  TinyPinChange_Init();
  VirtualPortNb=TinyPinChange_RegisterIsr(BROCHE_SENSOR1, InterruptFunctionToCall);
  VirtualPortNb_=TinyPinChange_RegisterIsr(BROCHE_SENSOR2, InterruptFunctionToCall);
  /* Enable Pin Change for each pin */
  TinyPinChange_EnablePin(BROCHE_SENSOR1);
  TinyPinChange_EnablePin(BROCHE_SENSOR2);
  
  switch (ms.InputMode)//CPPM,SBUS,SRXL,SUMD,IBUS or JETI
  {
    case CPPM:
        blinkNTime(1,125,250);
        if (RunConfig == true)
        {
          SettingsPort << F("CPPM selected") << endl << endl;
        }     
        TinyCppmReader.attach(BROCHE_PPMINPUT); // Attach TinyPpmReader to SIGNAL_INPUT_PIN pin 
      break;
    case SBUS:
        blinkNTime(2,125,250);
        if (RunConfig == true)
        {
          SettingsPort << F("SBUS selected") << endl << endl;
        }
          Serial.begin(SBUS_RX_SERIAL_CFG);
          RcBusRx.serialAttach(&Serial);
          RcBusRx.setProto(RC_BUS_RX_SBUS);
      break;
    case SRXL:
        blinkNTime(3,125,250);
        if (RunConfig == true)
        {
          SettingsPort << F("SRXL selected") << endl << endl;
        }
        Serial.begin(SRXL_RX_SERIAL_CFG);        
        RcBusRx.serialAttach(&Serial);
        RcBusRx.setProto(RC_BUS_RX_SRXL);
      break;
    case SUMD:
        blinkNTime(4,125,250);
        if (RunConfig == true)
        {
          SettingsPort << F("SUMD selected") << endl << endl;
        }
        Serial.begin(SUMD_RX_SERIAL_CFG);       
        RcBusRx.serialAttach(&Serial);
        RcBusRx.setProto(RC_BUS_RX_SUMD);
      break;
    case IBUS:
        blinkNTime(5,125,250);
        if (RunConfig == true)
        {
          SettingsPort << F("IBUS selected") << endl << endl;
        }
        Serial.begin(IBUS_RX_SERIAL_CFG);
        RcBusRx.serialAttach(&Serial);
        RcBusRx.setProto(RC_BUS_RX_IBUS);
      break;
    case JETIEX:
        blinkNTime(6,125,250);
        if (RunConfig == true)
        {
          SettingsPort << F("IBUS selected") << endl << endl;
        }
        Serial.begin(JETI_RX_SERIAL_CFG);
        RcBusRx.serialAttach(&Serial);
        RcBusRx.setProto(RC_BUS_RX_JETI);
      break;
  }

#ifdef DEBUG
  readAllEEpromOnSettingsPort();//lecture configuration du module dans le terminal serie
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

  /* init pins leds in output (RED leds are defined here if SettingsPort not used for settings*/
#ifdef EXTLED
  out(LED);
  out(LED1GREEN);on(LED1GREEN);
  out(LED2GREEN);on(LED2GREEN);
  out(LED1YELLOW);on(LED1YELLOW);
  out(LED2YELLOW);on(LED2YELLOW);
  delay(500);
  off(LED1GREEN);off(LED2GREEN);
  off(LED1YELLOW);off(LED2YELLOW);
#endif

#ifdef RECORDER
  setupRecorder();
#endif

#ifdef GLOWMANAGER
/*
The two RED leds on the SyncTwinRcEngine will turn on continuous when the glow drivers are on. If
the glow plug is not connected or burned out the RED led for that glow driver will flash
slowly. If a glow driver battery is getting low (below 1.15 volts under load) the RED led
for that glow driver on the TwinSync will flash rapidly.
*/
  pinMode(BROCHE_GLOW1,INPUT_PULLUP);//check if glow module is connected for motor 1
  pinMode(BROCHE_GLOW2,INPUT_PULLUP);//check if glow module is connected for motor 2
  if (digitalRead(BROCHE_GLOW1) == LOW && digitalRead(BROCHE_GLOW2) == LOW)// if Glow driver's resistor pull down found
  {
    GlowDriverInUse = true;
    pinMode(BROCHE_GLOW1,OUTPUT);
    pinMode(BROCHE_GLOW2,OUTPUT);
    on(LED1RED);on(LED2RED);   
    //initialisation du chauffage des bougies
    glowSetup();    
  }
  else
  {
    GlowDriverInUse = false;
  }

#endif  

  SettingsPort << F("Setup is DONE ...") << endl << endl;

}//fin setup


void loop()
{

#ifdef ARDUINO2PC
  if (CheckIfVBUsed == false)
  {
    if (RunConfig == true)
    {
      /* Check 1s */
      if(millis()-started1s>=1000)
      {
        SettingsPort << F(".");started1s=millis();
      }      
      /* Check 10s */
      if(millis()-ShutDownSerialSetting >= 10000)//Wait VB program 10s.
      {
        SettingsPort << endl << F("Shutdown Serial port ...") << endl << endl;
        SettingsPort.flush();
        SettingsPort.end();//SettingsPort is shut down and release pins 10/11 for Red leds.

        
#ifdef EXTLED        
        out(LED1RED);out(LED2RED);
#endif

#ifdef GLOWMANAGER
        if (GlowDriverInUse == true)
        {
          pinMode(BROCHE_GLOW1,OUTPUT);
          pinMode(BROCHE_GLOW2,OUTPUT);
#ifdef EXTLED          
          on(LED1RED);on(LED2RED);delay(500);off(LED1RED);off(LED2RED);
#endif          
        }
#endif
        RunConfig = false;
      }
    }
  }

  if (RunConfig == true)
  {
    SerialFromToVB();    
  }

#endif

if (ServoRecorderIsON == true)
{
#ifdef RECORDER
  loopRecorder();
#endif
}
else
{
  mode0();/* main mode launched if no buttons pressed during start */ 
}

#ifdef GLOWMANAGER
  if (GlowDriverInUse == true)
  {
    glowUpdate();
  }
  else
  {
    // Blink each 1s if GLOWDRIVER not found
    if(millis()-LedRedStartMs>=LED_SIGNAL_NOTFOUND)
    {
      flip(LED1RED);flip(LED2RED); 
      LedRedStartMs=millis(); // Restart the Chrono for the LED 
    }          
  }
#endif  


}//fin loop

void readAllEEprom()
{

/*
  ms.ID                  = 0x99;//write the ID to indicate valid data
  ms.InputMode           = 1;   //0-PPM, 1-SBUS, 2,IBUS
//  ms.radioRcMode         = 1;   // mode is 1 to 4
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
  ms.channelsOrder       = 0;
  ms.coeff_division      = 4.0;
 */
 
  EEPROM.get(0,ms);// Read all EEPROM settings in one time
  if ( ms.ID != 0x99)
  {
    blinkNTime(5,100,100);
    SettingsWriteDefault();
    waitMs(500);
    readAllEEprom();
  }
  else
  {
    //EEPROM Ok!
    SettingsPort <<  endl << F("EEPROM OK :-)") << endl << endl;
//    Serial << F("STRUCTURE SIZE IN EEPROM: ") << sizeof(MaStructure) << endl;
    //blinkNTime(5,LED_SIGNAL_FOUND,LED_SIGNAL_FOUND);
  }
}


#ifdef DEBUG
void readAllEEpromOnSettingsPort()
{
  SettingsPort << endl;
  SettingsPort << F("Chargement valeurs EEPROM ...") << endl << endl;
  SettingsPort << F("EEprom ID: 0x") << _HEX(ms.ID) << endl;
  if (ms.InputMode == 0) SettingsPort << F("PPM mode") << endl;
  if (ms.InputMode == 1) SettingsPort << F("SBUS mode") << endl;
  if (ms.InputMode == 2) SettingsPort << F("SRXL mode") << endl;
  if (ms.InputMode == 3) SettingsPort << F("SUMD mode") << endl;
  if (ms.InputMode == 4) SettingsPort << F("IBUS mode") << endl;
  if (ms.InputMode == 5) SettingsPort << F("JETI mode") << endl;
  SettingsPort << F("Channels Order: ") << ms.channelsOrder << endl;
  SettingsPort << F("Auxiliary Nb Channel: ") << ms.AuxiliaryNbChannel << endl;
  SettingsPort << F("Centre servo1: ") << ms.centerposServo1 << endl;
  SettingsPort << F("Centre servo2: ") << ms.centerposServo2 << endl;
  SettingsPort << F("Position Repos servos 1: ") << ms.idelposServos1 << "us" << endl;
  SettingsPort << F("Position Repos servos 2: ") << ms.idelposServos2 << "us" << endl;
  SettingsPort << F("Vitesse de reponse: ") << ms.responseTime << endl;
  SettingsPort << F("Position maxi servos: ") << ms.fullThrottle << "us" << endl;
  SettingsPort << F("Debut synchro servos: ") << ms.beginSynchro << "us" << endl;
  SettingsPort << F("Position mini general: ") << ms.minimumPulse_US << "us" << endl;
  SettingsPort << F("Position maxi general: ") << ms.maximumPulse_US << "us" << endl;
  if (ms.auxChannel == 1) SettingsPort << F("Connexion Ch AUX: mode ") << ms.auxChannel << F(" (Ch Aux non connecte)") <<  endl;
  if (ms.auxChannel == 2) SettingsPort << F("Connexion Ch AUX: mode ") << ms.auxChannel << F(" (inter 3 positions) ") <<  endl;
  if (ms.auxChannel == 3 || ms.auxChannel == 5 || ms.auxChannel == 6) SettingsPort << F("Connexion Ch AUX: mode ") << ms.auxChannel << F(" (inter 2 positions)") <<  endl;
  if (ms.auxChannel == 4) SettingsPort << F("Connexion Ch AUX: mode ") << ms.auxChannel << F(" (Ch Aux connecte a direction)") << endl;
  SettingsPort << F("Difference vitess Err: ") << ms.diffVitesseErr << endl;
  SettingsPort << F("Nombre de pales ou d'aimants: ") << _DEC(ms.nbPales) << endl;
  SettingsPort << F("Voltage interne : ") <<  _FLOAT(readVcc() / 1000, 3) << F("v") << endl;
  if (ms.fahrenheitDegrees == 0)
  {
    SettingsPort << F("Temperature interne : ") <<  GetTemp() << "\xC2\xB0" << F("C") << endl;
  }
  else
  { 
    SettingsPort << F("Temperature interne : ") <<  GetTemp() << "\xC2\xB0" << F("F") << endl;
  }

  SettingsPort << F("Mini Motor : ") <<  ms.minimumSpeed << F(" rpm") << endl;
  SettingsPort << F("Maxi Motor : ") <<  ms.maximumSpeed << F(" rpm") << endl;
  
#ifdef EXTERNALVBATT
  SettingsPort << F("Voltage externe : ") <<  _FLOAT(GetExternalVoltage(), 3) << F("v") << endl;
  battery_Informations();
#endif
}
#endif

void SettingsWriteDefault()
{
//static uint8_t AddressMax = 0;

  ms.ID                  = 0x99;//AddressMax += sizeof(ms.ID); //write the ID to indicate valid data
  ms.InputMode           = 1;//AddressMax += sizeof(ms.InputMode);   //0-PPM, 1-SBUS, 2-SRXL, 3-SUMD, 4-IBUS, 5-JETI
//  ms.radioRcMode         = 1;//AddressMax += sizeof(ms.radioRcMode);   // mode is 0 to 3 (mode 1 à 4)
  ms.AuxiliaryNbChannel  = 5;//AddressMax += sizeof(ms.AuxiliaryNbChannel);
  ms.centerposServo1     = 1500;//AddressMax += sizeof(ms.centerposServo1);
  ms.centerposServo2     = 1500;//AddressMax += sizeof(ms.centerposServo2);
  ms.idelposServos1      = 1000;//AddressMax += sizeof(ms.idelposServos1);
  ms.idelposServos2      = 1000;//AddressMax += sizeof(ms.idelposServos2);
  ms.responseTime        = 2   ;//AddressMax += sizeof(ms.responseTime);
  ms.fullThrottle        = 2000;//AddressMax += sizeof(ms.fullThrottle);
  ms.beginSynchro        = 1250;//AddressMax += sizeof(ms.beginSynchro);
  ms.auxChannel          = 1;//AddressMax += sizeof(ms.auxChannel);   
  ms.reverseServo1       = 1;//AddressMax += sizeof(ms.reverseServo1);
  ms.reverseServo2       = 0;//AddressMax += sizeof(ms.reverseServo2);
  ms.diffVitesseErr      = 99;//AddressMax += sizeof(ms.diffVitesseErr);//difference de vitesse entre les 2 moteurs en tr/mn toleree
  ms.minimumPulse_US     = 1000;//AddressMax += sizeof(ms.minimumPulse_US);
  ms.maximumPulse_US     = 2000;//AddressMax += sizeof(ms.maximumPulse_US);
  ms.telemetryType       = 0;//AddressMax += sizeof(ms.telemetryType); //mode2 0- Rien, 1- FrSky (S-Port), 2- Futaba Sbus, 3- Hitec, 4- Hott, 5- Jeti 6- Spektrum
  ms.nbPales             = 2;//AddressMax += sizeof(ms.nbPales);   
  ms.moduleMasterOrSlave = 0;//AddressMax += sizeof(ms.moduleMasterOrSlave);   
  ms.fahrenheitDegrees   = 0;//AddressMax += sizeof(ms.fahrenheitDegrees);
  ms.minimumSpeed        = 1000;//AddressMax += sizeof(ms.minimumSpeed);//minimum motor rpm
  ms.maximumSpeed        = 20000;//AddressMax += sizeof(ms.maximumSpeed);//maximum motor rpm
  ms.channelsOrder       = 0;
  ms.coeff_division      = 4.0;

  //SettingsPort << F("Address maxi: ") << AddressMax << endl;
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

void sendConfigToSettingsPort()
{ //format send: 1657|1657|1225|1225|2|2075|1393|1070|2205|1|0|0|100|39|2|0|5.00|27.61|1.000|0|1000|20000|0
  //EEPROM.get(0,ms);
  SettingsPort << F("LLA");
  SettingsPort << ms.centerposServo1 << F("|");//array(0)
  SettingsPort << ms.centerposServo2 << F("|");//array(1)
  SettingsPort << ms.idelposServos1 << F("|");//array(2)
  SettingsPort << ms.idelposServos2 << F("|");//array(3)
  SettingsPort << ms.responseTime << F("|");//array(4)
  SettingsPort << ms.fullThrottle << F("|");//array(5)
  SettingsPort << ms.beginSynchro << F("|");//array(6)
  SettingsPort << ms.minimumPulse_US << F("|");//array(7)
  SettingsPort << ms.maximumPulse_US << F("|");//array(8)
  SettingsPort << ms.auxChannel << F("|");//array(9)
  SettingsPort << ms.reverseServo1 << F("|");//array(10)
  SettingsPort << ms.reverseServo2 << F("|");//array(11)
  SettingsPort << ms.diffVitesseErr << F("|");//array(12)
  SettingsPort << F("0|");//array(13) NON UTILISé
  SettingsPort << _DEC(ms.nbPales) << F("|"); //array(14)
  SettingsPort << ms.channelsOrder << F("|");//array(15))
  SettingsPort << _FLOAT(readVcc() / 1000, 3) << F("|"); //array(16)
  SettingsPort <<  GetTemp() << F("|");//array(17)
#ifdef EXTERNALVBATT
  SettingsPort <<  _FLOAT(GetExternalVoltage(), 3) << F("|"); //attention a I2C qui utilise les pins A4 et A5 //array(18)
#else
  SettingsPort << F("NOTUSED|");//array(18)
#endif
  SettingsPort << ms.moduleMasterOrSlave << F("|");//array(19)
  SettingsPort << ms.fahrenheitDegrees << F("|");//array(20)
  SettingsPort << ms.minimumSpeed << F("|");//array(21)
  SettingsPort << ms.maximumSpeed << F("|");//array(22)
  SettingsPort << ms.InputMode << F("|");//array(23)
  SettingsPort << ms.coeff_division << endl;//array(24)
  //SettingsPort.flush(); // clear SettingsPort port
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

//#ifdef EXTERNALVBATT
////Start of ADC managements functions
//void ADC_setup(){
//  ADCSRA =  bit (ADEN);                      // turn ADC on
//  ADCSRA |= bit (ADPS0) |  bit (ADPS1) | bit (ADPS2);  // Prescaler of 128
//#ifdef INT_REF
//  ADMUX  =  bit (REFS0) | bit (REFS1);    // internal 1.1v reference
//#else
//  ADMUX  =  bit (REFS0) ;   // external 5v reference
//#endif
//}
//
//int anaRead(int adc_pin){
//  ADC_read_conversion();// read result of previously triggered conversion
//  ADC_start_conversion(adc_pin);// start a conversion for next loop 
//}
//
//void ADC_start_conversion(int adc_pin){
//  ADMUX &= ~(0x07) ; //clearing enabled channels
//  ADMUX  |= (adc_pin & 0x07) ;    // AVcc and select input port
//  bitSet (ADCSRA, ADSC) ;
//}
//
//int ADC_read_conversion(){
// while(bit_is_set(ADCSRA, ADSC));
// return ADC ;
//}
////End of ADC management functions
//#endif
