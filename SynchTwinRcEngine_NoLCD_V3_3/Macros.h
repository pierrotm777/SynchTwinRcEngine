#ifndef _MACROS_H_
#define _MACROS_H_


///* use soft_restart() for have a reset (see https://github.com/connornishijima/SoftReset/blob/master/SoftReset.h) 
//15mS    WDTO_15MS
//30mS    WDTO_30MS
//60mS    WDTO_60MS
//120mS   WDTO_120MS
//250mS   WDTO_250MS
//500mS   WDTO_500MS
//1S      WDTO_1S
//2S      WDTO_2S
//4S      WDTO_4S
//8S      WDTO_8S
//*/
//#include <avr/wdt.h>
//#define soft_restart()      \
//do                          \
//{                           \
//    wdt_enable(WDTO_15MS);  \
//    for(;;)                 \
//    {                       \
//    }                       \
//} while(0)
//	
///* Function Pototype (For newer AVRs (such as the ATmega1281) 
//also add this function to your code to then disable the 
//watchdog after a reset (e.g., after a soft reset):) */
//void wdt_init(void) __attribute__((naked)) __attribute__((section(".init3")));
//
//// Function Implementation
//void wdt_init(void)
//{
//   MCUSR = 0;
//   wdt_disable();
//   return;
//}
	
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
       Serial.print(#Tbl);Serial.print(F("["));Serial.print((int)Idx);Serial.print(F("] = '"));    \
       Serial.print(Tbl[Idx]);Serial.println(F("'"));                                              \
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
#define PIN_INPUT(port,pin) DDR ## port &= ~(1<<pin)
#define PIN_OUTPUT(port,pin) DDR ## port |= (1<<pin)
#define PIN_LOW(port,pin) PORT ## port &= ~(1<<pin)
#define PIN_HIGH(port,pin) PORT ## port |= (1<<pin)
#define PIN_TOGGLE(port,pin) PORT ## port ^= (1<<pin)
//#define PIN_READ(port,pin) (PIN ## port & (1<<pin))
//#define  PIN_INPUT_WITH_PULLUP(port,pin) PIN_INPUT(port,pin);PIN_HIGH(port,pin)

/* Ci-dessous, choisir EDGE_TYPE entre rien (#define EDGE_TYPE), Falling (#define EDGE_TYPE Falling) ou Rising (#define EDGE_TYPE Rising) */
#define EDGE_TYPE           Rising

/*  vvv  Ne rien modifier aux 3 macros ci-dessous vvv */
#define CONCAT3_(a, b, c)   a##b##c
#define CONCAT3(a, b, c)    CONCAT3_(a, b, c)
#define TINY_PIN_CHANGE     CONCAT3(TinyPinChange_, EDGE_TYPE,  Edge)
/*  ^^^  Ne rien modifier aux 3 macros ci-dessus  ^^^ */

#endif /* _MACROS_H_ */
