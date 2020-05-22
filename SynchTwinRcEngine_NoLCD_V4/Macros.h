#ifndef _MACROS_H_
#define _MACROS_H_
	
/* Possible values to compute a shifting average fin order to smooth the recieved pulse witdh */
#define AVG_WITH_1_VALUE        0
#define AVG_WITH_2_VALUES       1
#define AVG_WITH_4_VALUES       2
#define AVG_WITH_8_VALUES       3
#define AVG_WITH_16_VALUES      4
//#define AVERAGE_LEVEL          AVG_WITH_4_VALUES  /* Choose here the average level among the above listed values */
/* Higher is the average level, more the system is stable (jitter suppression), but lesser is the reaction */
/* Macro for average */
#define AVERAGE(ValueToAverage,LastReceivedValue,AverageLevelInPowerOf2)  ValueToAverage=(((ValueToAverage)*((1<<(AverageLevelInPowerOf2))-1)+(LastReceivedValue))/(1<<(AverageLevelInPowerOf2)))

/* Macro calcul fraction */
#define THROTTLE_FRACT(Min_US, Max_US, Numerator, Denominator)     ( (Min_US) + (((Max_US) - (Min_US)) * (Numerator) / (Denominator)) ) 

//DISPLAY_STR_TBL(StrTbl); // <-- Mettre le nom de la table des pointeurs en argument de la macro
#if DEBUG
#define DISPLAY_STR_TBL(Tbl) do{                                                                \
   for(uint8_t Idx = 0; Idx < TBL_ITEM_NB(Tbl); Idx ++)                                        \
   {                                                                                           \
     if(Tbl[Idx])                                                                              \
     {                                                                                         \
       SettingsPort.print(#Tbl);SettingsPort.print(F("["));SettingsPort.print((int)Idx);SettingsPort.print(F("] = '"));    \
       SettingsPort.print(Tbl[Idx]);SettingsPort.println(F("'"));                                              \
     }                                                                                         \
   }                                                                                           \
 }while(0)
#else
#define DISPLAY_STR_TBL(Tbl)
#endif

/*
LED1 B,0
LED2 B,1
LED3 B,2
LED4 B,3
LED5 B,4
LED6 B,5
 */
/* Macro function to declare an output pin */
#define out(x)      _out(x)
#define _out(bit,port)  DDR##port |= (1 << bit)
/* Macro function to declare an input pin */
#define in(x)     _in(x)
#define _in(bit,port) DDR##port &= ~(1 << bit)
/* Macro function to set an output pin high */
#define on(x)     _on(x)
#define _on(bit,port) PORT##port |= (1 << bit)
/* Macro function to set an output pin low */
#define off(x)      _off(x)
#define _off(bit,port)  PORT##port &= ~(1 << bit)
/* Macro function to set internal pullup resistor of input pin (same as "on" macro)*/
#define pullup(x)   _on(x)
/* Macro function to get state of input pin */
#define getin(x)      _getin(x)
#define _getin(bit,port)  (PIN##port & (1 << bit))
/* Macro function to toggle an output pin */
#define flip(x)     _flip(x)
#define _flip(bit,port) PORT##port ^= (1 << bit)

/* Ci-dessous, choisir EDGE_TYPE entre rien (#define EDGE_TYPE), Falling (#define EDGE_TYPE Falling) ou Rising (#define EDGE_TYPE Rising) */
#define EDGE_TYPE           Rising

/*  vvv  Ne rien modifier aux 3 macros ci-dessous vvv */
#define CONCAT3_(a, b, c)   a##b##c
#define CONCAT3(a, b, c)    CONCAT3_(a, b, c)
#define TINY_PIN_CHANGE     CONCAT3(TinyPinChange_, EDGE_TYPE,  Edge)
/*  ^^^  Ne rien modifier aux 3 macros ci-dessus  ^^^ */

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
#endif /* _MACROS_H_ */
