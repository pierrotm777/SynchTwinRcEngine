//https://github.com/adafruit/Adafruit_FRAM_I2C/blob/master/examples/MB85RC256V/MB85RC256V.ino
//https://learn.adafruit.com/adafruit-i2c-fram-breakout/wiring-and-test
#ifdef FRAM_USED
/***********************
kbellco.com 4/30/16

https://www.instructables.com/id/Working-With-Adafruit-FRAM-Memory/

This sketch builds on the example Adafruit sketch for the 32k FRAM 
memory chip with I2C.  Each memory address stores a value from 0-255. 
The Adafruit example uses both hex (0xff) and decimal connotations 
which I found confusing, and not way to store larger values.

Below I added a couple of simple functions that allow numbers 
as high as 65536 to be stored and retrieved by breaking them into 2 bytes.
 
If your sketch has a need to store values between resets, this could 
be a simple answer. Values must be below 65536. Note 2 addresses are 
used to store values, so make sure you don't overwrite half of your 
stored value.


************************/

#include <Wire.h>
#include "Adafruit_FRAM_I2C.h"


/*****Uncomment below to dump all of the memory to screen  SLOW!!!!  ***/
//#define dumpMemory

/* Example code for the Adafruit I2C FRAM breakout */

/* Connect SCL    to analog 5
   Connect SDA    to analog 4
   Connect VDD    to 5.0V DC
   Connect GROUND to common ground */

Adafruit_FRAM_I2C fram     = Adafruit_FRAM_I2C();
uint16_t          framAddr = 0;

void setupFRAM(void) {
  Serial.begin(9600);

  if (fram.begin()) {  // you can stick the new i2c addr in here, e.g. begin(0x51);
    SettingsPort << F("Found I2C FRAM") << endl;
  } else {
    SettingsPort << F("NOFRAM") << endl;
    //Serial.println("Will continue in case this processor doesn't support repeated start\r\n");
  }

  // Read the first byte
//  uint8_t test = fram.read8(0x0);
//  Serial.print("Restarted "); Serial.print(test); Serial.println(" times");
  // Test write ++
//  fram.write8(0x0, test + 1);

#  ifdef dumpMemory
  dumpMem();
#  endif
  
}

void loopFRAM() 
{

  delay(3000);   //Slow down so you can see what is written above

  for (unsigned int writeValue = 32700; writeValue < 68000; writeValue++) {
    unsigned int writeAddress = 10;
    writeMem(writeAddress, writeValue);
    unsigned int readValue = readMem(writeAddress);

    if(writeValue % 1000 == 0) {       Serial.print("writeAddress: "); Serial.print(writeAddress);
      Serial.print(" writeValue: "); Serial.print(writeValue);
      Serial.print(" read Value: "); Serial.println(readValue);
    }

  }

}

void writeMem(int address, long value) 
{
  unsigned int MSB = value / 256L;      // Break the value into 2 Byte-parts for storing
  unsigned int LSB = value % 256L;      // Above is MSB, remainder (Modulo) is LSB
  fram.write8(address, MSB);              // Store the value MSB at address add1
  fram.write8(address + 1, LSB);          // Store the value LSB at address add1 + 1
}

unsigned int readMem(unsigned int address) 
{
  unsigned int MSB = fram.read8(address);           //Read the 2 bytes from memory
  unsigned int LSB = fram.read8(address + 1);
  unsigned int value = (256 * MSB) + LSB;
  return value;
}

void dumpMem()
{
  // dump the entire 32K of memory!
  uint8_t value;
  for (uint16_t a = 0; a < 32768; a++) 
  {
    value = fram.read8(a);
    if ((a % 32) == 0) {
    Serial.print("\n 0x"); Serial.print(a, HEX); Serial.print(": ");
  }
  Serial.print("0x");
  if (value < 0x1)
    Serial.print('0');
  Serial.print(value, HEX); Serial.print(" ");
  }

}


#endif
