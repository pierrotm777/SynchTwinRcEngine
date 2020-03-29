#ifdef RECORDER
/* 
  Recorder save throttle movements into the eeprom and play the sequence saved
*/
typedef enum {PT, REC, PLAY} MODE; // PT = PassThrough, ie follow POT or Joystick
MODE mode = PT;
uint8_t stateChange;// = 0; // Play/Rec every other sample, interpolate between
uint8_t ind, ang;

#define MAX_LEN 200
#define EEpromAdd 80
typedef struct conf
{
  short len;
  byte minServo;
  byte maxServo;
  byte d[MAX_LEN];
} Conf;

Conf conf;

void setupRecorder()
{
  byte *p = (byte*)&conf;
  for (uint8_t i = EEpromAdd; i < sizeof(Conf); i++) // reading is fast so do the lot, could just read the data
  {
    byte b = EEPROM.read(i);
    *(p++) = b;
//    Serial << i << " val " << b << endl;
  }

  conf.len = constrain(conf.len, EEpromAdd, MAX_LEN -1);
  conf.minServo = constrain(conf.minServo, 10, 170);
  conf.maxServo = constrain(conf.maxServo, 10, 170);

  ledFlashSaveInEEProm(5);
}

uint8_t m = 1;
void loopRecorder()
{
  ang = map(Width_us, ms.minimumPulse_US, ms.maximumPulse_US, 1, 179);

  switch(m)
  {
    case 0:
      if(mode == PT) mode = PLAY;
      else if(mode == PLAY) mode = PT;
      stateChange = 0;
      ind = 0;
      //Serial.println(F("Play..."));
      break;          
    case 1:
      if(mode == PT) {mode = REC;}
      else if(mode == REC)
      {
        mode = PT;
        conf.len--;
      }
      stateChange = 0;
      ind = 0;
      //Serial.println(F("Record..."));
      break;
  }     
   
//  if (mode == PT)
//  {
//    Serial.print("Pot ");
//    Serial.print(pot);
//    Serial.print(" Servo ");
//    Serial.println(ang);
//  } 
//  if (mode != PT)
//  {
//    Serial.print(ind);
//    Serial.print(" ");
//    Serial.print(stateChange);
//    Serial.print(" ");
//    Serial.println(ang);
//  } 


  if (mode == PLAY)
  {
    if (stateChange)
    {
      ang = conf.d[ind] + conf.d[ind++ +1]; // WOW C is not Java ?
      ang /= 2;
    }
    else
    {
      ang = conf.d[ind];
    }
  }

  if (mode == REC)
  {
    ServoMotor1.write(ang); // Lets the Servo class do any uS to Angle conversion :-)
//    SoftRcPulseOut::refresh(1); 
    ang = ServoMotor1.read(); // Get it back into Degrees
    if (!stateChange) {conf.d[ind++] = ang;}
    conf.len = ind;
  }
 
  if (ind >= conf.len && mode == PLAY || ind >= MAX_LEN)
  {

    PIN_LOW(B,0);//(LED3, LOW); // Flash on reset to show loop during playback
    PIN_HIGH(B,1);//(LED4, HIGH);

    ind = 0; // go back to bagining
    stateChange = 1; // will get reset below
    if (mode == REC)
    {
      mode = PT; // stop recording
      conf.len--; // dont want n+ stateChange
    }
  }

  ServoMotor1.write(ang);
  SoftRcPulseOut::refresh(1);

  stateChange ^= 1; // classic XOR stateChange
//  delay(40); // give the LEDs some time to flash

  if(mode == REC){PIN_HIGH(B,0);}else{PIN_LOW(B,0);}
  if(mode == PLAY){PIN_HIGH(B,1);}else{PIN_LOW(B,1);}

}

void saveConfig()
{
  uint8_t *p = (uint8_t*)&conf;
  uint16_t sz = sizeof(Conf) + conf.len - MAX_LEN + 1; // Only write recorded data
  for (uint8_t i = EEpromAdd; i < sz; i++)
  {
    EEPROM.update(i, *p++);
  }

  ledFlashSaveInEEProm(5);
}

#endif//end RECORDER
