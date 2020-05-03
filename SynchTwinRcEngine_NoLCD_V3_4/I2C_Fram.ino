//https://github.com/adafruit/Adafruit_FRAM_I2C/blob/master/examples/MB85RC256V/MB85RC256V.ino
//https://learn.adafruit.com/adafruit-i2c-fram-breakout/wiring-and-test
#ifdef FRAM_USED
#include <Wire.h>
#include "Adafruit_FRAM_I2C.h"

/* Example code for the Adafruit I2C FRAM breakout */

/* Connect SCL    to analog 5
   Connect SDA    to analog 4
   Connect VDD    to 5.0V DC
   Connect GROUND to common ground */
   
Adafruit_FRAM_I2C fram     = Adafruit_FRAM_I2C();

void setupFRAM(void) {

#ifdef DEBUG  
  if (fram.begin()) {  // you can stick the new i2c addr in here, e.g. begin(0x51); default is 0x50;
    SettingsPort << F("Found I2C FRAM") << endl;
  } else {
    SettingsPort << F("I2C FRAM not identified ... check your connections?\r\n") << endl;
    SettingsPort << F("Will continue in case this processor doesn't support repeated start\r\n") << endl;
  }
#endif
  
  // Read the first byte
  uint8_t test = fram.read8(0x0);
  SettingsPort << F("Restarted ")<< test << F(" times") << endl;
  // Test write ++
  fram.write8(0x0, test+1);
  
  // dump the entire 32K of memory!
  uint8_t value;
  for (uint16_t a = 0; a < 32768; a++) {
    value = fram.read8(a);
    if ((a % 32) == 0) {
      SettingsPort << F("\n 0x") << _HEX(a) << F(": ");
    }
    SettingsPort << F("0x"); 
    if (value  < 0x1) 
      SettingsPort.print('0');
    SettingsPort << _HEX(value) << F(" ");
  }
}

#endif
