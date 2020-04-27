#ifdef EXTERNALVBATT

// Default height units, in case no valid settings are found: 3.281 for feet, 1.0 for metres. Defaults to feet.
#define HEIGHT_UNITS_DEFAULT 3.281

// -- low voltage alarm
// default LVA threshold if no valid settings are found.
#define LOW_VOLTAGE_THRESHOLD_DEFAULT 4.7
// when the battery alarm goes off, the voltage will need to come up to the threshold plus this voltage
// before the alarm will switch off. This stops the alarm from switching on and off repeatedly when the
// voltage is very close to threshold.
#define BATTERY_MONITOR_HYSTERESIS 0.2
// --- lipo options
// this is the voltage that will be used to distinguish between 2s and 3s packs.
#define LIPO_CELL_DETECT_THRESHOLD 8.6
/*
4.20v = 100%
4.03v = 76%
3.86v = 52%
3.83v = 42%
3.79v = 30%
3.70v = 11%
3.6?v = 0%
==>2S = 8.4v
==>3S = 12.6v
*/

// this is the voltage that will be used to distinguish between 4xNIxx and 5xNIxx packs.
#define NIXX_CELL_DETECT_THRESHOLD 6
/*
1.5v = 100%
0.9v = 0%
==>4 = 7.0v
==>5 = 7.5v
*/

enum BatteryType { BATTERY_TYPE_NIMH = 0, BATTERY_TYPE_LIPO = 1, BATTERY_TYPE_NONE = 2 };

BatteryType batteryType;
  
uint8_t battery_numberOfCells()
{
  uint8_t numberOfCells=0;
  float v = GetExternalVoltage();//attention a I2C qui utilise les pins A4 et A5
  if (v != 0)
  {
    if (batteryType == BATTERY_TYPE_LIPO)
    {
      // measure the LIPO voltage and return the number of cells
      numberOfCells = ((v < LIPO_CELL_DETECT_THRESHOLD) ? 2 : 3);
    }
    else if (batteryType == BATTERY_TYPE_NIMH)
    {
      // measure the NIXX voltage and return the number of cells   
      numberOfCells = ((v < NIXX_CELL_DETECT_THRESHOLD) ? 4 : 5);
    }    
  }
  else
  {
    numberOfCells = 0;
  }

  return numberOfCells;
}

// There is a small amount of hysteresis in this function to stop the alarm from
// intermittently switching on and off near the voltage threshold.
boolean battery_isLow()
{
  if (batteryType == BATTERY_TYPE_NONE) return false;
  // TODO: would be better to abstract the hysteresis calculation here
  boolean low=false, isLow = false;
  double v = GetExternalVoltage();//attention a I2C qui utilise les pins A4 et A5 
  float threshold=0;
  
  if (batteryType == BATTERY_TYPE_LIPO)
  {
    if (isLow) low = ((v / (battery_numberOfCells())) < (threshold + BATTERY_MONITOR_HYSTERESIS));
    else low = ((v / battery_numberOfCells()) < threshold);
  }
  if (batteryType == BATTERY_TYPE_NIMH)
  {
    if (isLow) low = (v < (threshold + BATTERY_MONITOR_HYSTERESIS));
    else low = (v < threshold);
  }
  isLow = low;
  return low;
}

#ifdef DEBUG
void battery_Informations()
{
  Serial << F("Batterie type: ");
  switch (batteryType)
  {
    case BATTERY_TYPE_NONE:Serial << F("no battery : ");break;
    case BATTERY_TYPE_NIMH:Serial << F("NIMH : ");break;
    case BATTERY_TYPE_LIPO:Serial << F("LIPO : ");break;
  }
  Serial << _FLOAT(GetExternalVoltage(),3) << F("v (")<< battery_numberOfCells() << F(" elements)") << endl;
}
#endif

float GetExternalVoltage()
{
  /*
  http://skyduino.wordpress.com/2012/08/09/arduino-mesurer-la-tension-dun-batterie/
  Celui ci a ete calcule pour permettre la mesure d'une plage de tension variant entre 0v et 20v.
  La formule classique d'un pont diviseur de tension est la suivante (je passe la partie theorie, loi des mailles, etc) :
  Vs = Vin * (R2 / (R1 + R2))
  Ici R1 = 3300 ohms (3k3) et R2 = 1100 ohms (1k1), on obtient donc pour Vin = 20v (maximum admissible) :
  Vs = 20 * (1100 / (3300 + 1100)) = 20 * (1100 / 4400) = 20 * 0.25 = 5
  Vs = 5v pour Vin = 20v, soit le maximum que peut mesurer le convertisseur analogique -> numerique de l'arduino (alimentation en 5v bien sur).

        +20V maxi
       -+-
        |
----.   # R1=3k3 = Orange Orange Rouge Or
    |   |
  A3|---+
    |   |        
NaNo|   # R2=1k1 = Marron Marron Rouge Or 
    |   |        
    |   |        
    |  -+-       
    |  GND      
----'        
  */  
  static float coeff_division = 4.0;
  /* Mesure de la tension brute */
  //uint16_t raw_bat = analogRead(BROCHE_BATTEXT); //attention a I2C qui utilise les pins A4 et A5
  uint16_t raw_bat = anaRead(BROCHE_BATTEXT);
  
  /* Calcul de la tension reel -- Convert the analog reading (which goes from 0 - 1023) to a voltage (0 - 5V) */
  //float real_bat = (raw_bat != 0 )?0:((raw_bat * (readVcc() * 1.0 / 1023)) * coeff_division);

  //raw_bat = map(raw_bat, 0, 1023, 0, REFERENCE_VOLTAGE); // see https://www.youtube.com/watch?v=NPcAqJ30EwE 
  float real_bat = (raw_bat != 0 )?0:((raw_bat * (5 * 1.0 / 1023)) * coeff_division);
  
  return (real_bat);

}

#endif
