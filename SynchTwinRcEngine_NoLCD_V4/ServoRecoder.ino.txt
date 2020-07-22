#ifdef RECORDER
/* 
  Recorder save throttle movements into the eeprom and play the sequence saved
*/
typedef enum {PT, REC, PLAY} MODE; // PT = PassThrough, ie follow POT or Joystick
MODE mode = PT;
uint8_t toggle;// = 0; // Play/Rec every other sample, interpolate between
uint8_t ind, ang;

#define MAX_LEN 800
#define EEpromAdd 50
typedef struct conf
{
  short len;
//  byte minServo;
//  byte maxServo;
  byte d[MAX_LEN];
} Conf;

Conf conf;

void setupRecorder()
{
//  byte *p = (byte*)&conf;
//  for (uint8_t i = EEpromAdd; i < sizeof(Conf); i++) // reading is fast so do the lot, could just read the data
//  {
//    byte b = EEPROM.read(i);
//    *(p++) = b;
//    SettingsPort << i << " val " << b << endl;
//  }

  conf.len = constrain(conf.len, EEpromAdd, MAX_LEN -1);
//  conf.minServo = constrain(conf.minServo, 10, 170);
//  conf.maxServo = constrain(conf.maxServo, 10, 170);

//  ledFlashSaveInEEProm(5);
}

void loopRecorder()
{
  ang = map(Width_us, ms.minimumPulse_US, ms.maximumPulse_US, 1, 179);

//  if (mode == PT)
//  {
//    SettingsPort.print("Pot ");
//    SettingsPort.print(pot);
//    SettingsPort.print(" Servo ");
//    SettingsPort.println(ang);
//  } 
//  

  if (releaseButtonMode == true)
  {
    switch(recoderMode)
    {
      case 0:// Start/Stop/Set Play Button
        if(mode == PT)
        { 
          mode = PLAY;SettingsPort.println(F("Play..."));
        }
        else if(mode == PLAY) 
        {
          mode = PT;SettingsPort.println(F("Stop..."));
        }
        toggle = ind = 0;
        releaseButtonMode = false;
        break;          
      case 1:// Record Button
        if(mode == PT) 
        {
          mode = REC;SettingsPort.println(F("Record..."));
        }
        else if(mode == REC)
        {
          mode = PT;
          conf.len--;
          SettingsPort.println(F("Stop..."));
        }
        toggle = ind = 0;
        releaseButtonMode = false;
        break;
    }         
  }

   
  if (mode == PLAY)
  {
    if (toggle)
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
//    ServoMotor1.write(ang); // Lets the Servo class do any uS to Angle conversion :-)
//    SoftRcPulseOut::refresh(1); 
//    ang = ServoMotor1.read(); // Get it back into Degrees
    if (!toggle) {conf.d[ind++] = ang;}
    conf.len = ind;
  }

//  if (mode != PT)
//  {
//    SettingsPort.print(ind);
//    SettingsPort.print(" ");
//    SettingsPort.print(toggle);
//    SettingsPort.print(" ");
//    SettingsPort.println(ang);
//  }   
// 
  if (ind >= conf.len && mode == PLAY || ind >= MAX_LEN)
  {

    //PIN_LOW(B,0);//(LED3, LOW); // Flash on reset to show loop during playback
    //PIN_HIGH(B,1);//(LED4, HIGH);

    ind = 0; // go back to bagining
    toggle = 1; // will get reset below
    if (mode == REC)
    {
      mode = PT; // stop recording
      conf.len--; // dont want n+ stateChange
    }
  }

  ServoMotor1.write(ang);
  ServoMotor2.write(ang);
  SoftRcPulseOut::refresh(1);

  toggle ^= 1; // classic XOR stateChange
//  delay(40); // give the LEDs some time to flash

#ifdef EXTLED
  if(mode == REC){on(LED1RED);}else{off(LED1RED);}
  if(mode == PLAY){on(LED1GREEN);}else{off(LED1GREEN);}
#endif

}

#endif//end RECORDER
