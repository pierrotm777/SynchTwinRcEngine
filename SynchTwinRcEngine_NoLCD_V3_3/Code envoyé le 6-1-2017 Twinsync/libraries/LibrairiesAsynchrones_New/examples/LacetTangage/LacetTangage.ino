#include <SoftRcPulseIn.h>
#include <TinyPinChange.h>
#include <SoftRcPulseOut.h> 
SoftRcPulseOut myservo;
SoftRcPulseOut myservo1;
SoftRcPulseOut myservo2;
SoftRcPulseOut myservo3;



#define BROCHE_VOIE1  13
#define BROCHE_VOIE2  12
#define BROCHE_VOIE3  11
#define BROCHE_VOIE4  10

#define SERVO_PIN0     2
#define SERVO_PIN1     3
#define SERVO_PIN2     4
#define SERVO_PIN3     5
#define REFRESH_PERIOD_MS 20

SoftRcPulseIn ImpulsionVoie1; /* Objet de type Impulsion entrante */
SoftRcPulseIn ImpulsionVoie2;
SoftRcPulseIn ImpulsionVoie3;
SoftRcPulseIn ImpulsionVoie4;

int gaz;
double lacet;
double roulis;
double tangage;
int var=0;

void setup()
{
   Serial.begin(9600);
   myservo.attach(SERVO_PIN0);
   myservo1.attach(SERVO_PIN1);
   myservo2.attach(SERVO_PIN2);
   myservo3.attach(SERVO_PIN3);
   ImpulsionVoie1.attach(BROCHE_VOIE1);
   ImpulsionVoie2.attach(BROCHE_VOIE2);
   ImpulsionVoie3.attach(BROCHE_VOIE3);
   ImpulsionVoie4.attach(BROCHE_VOIE4);
}


void loop()    {

  gaz=ImpulsionVoie1.width_us();
  lacet=ImpulsionVoie2.width_us();
  roulis=ImpulsionVoie3.width_us();
  tangage=ImpulsionVoie4.width_us();
  //Serial.println(gaz);
  //Serial.println(lacet);
  //Serial.println(roulis);
  //Serial.println(tangage);
  /////////////////////////////////////////////////////////////////////
  if (lacet < 1450){ 
    myservo.write_us(gaz);
    myservo3.write_us(gaz);  
    lacet =1498-lacet;
    //Serial.println(gaz-lacet);
    myservo1.write_us(gaz-lacet);
    myservo2.write_us(gaz-lacet);
    var = var+1;
  }
  /////////////////////////////////////////////////////////////////////
  if (lacet > 1550){ 
    myservo.write_us(gaz);
    myservo2.write_us(gaz);  
    lacet =1498-lacet;
    //Serial.println(gaz-lacet);
    myservo3.write_us(gaz-lacet);
    myservo1.write_us(gaz-lacet);
    var = var+1; 
  }
  /////////////////////////////////////////////////////////////////////
  if (tangage < 1450){
    myservo.write_us(gaz);
    myservo2.write_us(gaz);  
    tangage = 1498-tangage
    //Serial.println(gaz-tangage);
    myservo3.write_us(gaz-tangage);
    myservo1.write_us(gaz-tangage);
    var = var+1; 
  }
  /////////////////////////////////////////////////////////////////////
  if (tangage > 1550){
    myservo.write_us(gaz);
    myservo2.write_us(gaz);  
    tangage = 1498-tangage
    //Serial.println(gaz-tangage);
    myservo3.write_us(gaz-tangage);
    myservo1.write_us(gaz-tangage);
    var = var+1; 
  } 
  /////////////////////////////////////////////////////////////////////
  if (var==0){
    myservo.write_us(ImpulsionVoie1.width_us());
    myservo1.write_us(ImpulsionVoie1.width_us());
    myservo2.write_us(ImpulsionVoie1.width_us());
    myservo3.write_us(ImpulsionVoie1.width_us());
    delay(REFRESH_PERIOD_MS);
    SoftRcPulseOut::refresh(1);
  }
  else{
    var=0;
  }
 
}
  

