#ifndef TinySoftPwm_h
#define TinySoftPwm_h

// a Tiny optimized Software PWM Manager (pins shall not be part of the same port anymore: now, they can be scattered)
// Only resources RAM/Program Memory of used pins are declared in the code at compilation time.
// based largely on Atmel's AVR136: Low-Jitter Multi-Channel Software PWM Application Note:
// http://www.atmel.com/dyn/resources/prod_documents/doc8020.pdf
// RC Navy 2013-2015
// http://p.loussouarn.free.fr
// 11/01/2015: Automated multi port support (at compilation time) added for ATtiny167
// 01/02/2015: Automated Multi port support added for ATmega328p (UNO)

#if defined(ARDUINO) && ARDUINO >= 100
#include "Arduino.h"
#else
#include "WProgram.h"
#endif

#include <inttypes.h>

/*************************************************/
/* Define here the PIN to use with Tiny Soft PWM */
/*       Unused Pin(s) SHALL be commented        */
/*************************************************/
// #define TINY_SOFT_PWM_USES_PIN0
// #define TINY_SOFT_PWM_USES_PIN1
// #define TINY_SOFT_PWM_USES_PIN2
// #define TINY_SOFT_PWM_USES_PIN3 /* /!\ used for USB on DigiSpark (pro): do not use it for PWM if DigiUSB or SerialCDC are also used /!\ */
// #define TINY_SOFT_PWM_USES_PIN4 /* /!\ used for USB on DigiSpark (pro): do not use it for PWM if DigiUSB or SerialCDC are also used /!\ */
// #define TINY_SOFT_PWM_USES_PIN5
// #define TINY_SOFT_PWM_USES_PIN6
#define TINY_SOFT_PWM_USES_PIN7
#define TINY_SOFT_PWM_USES_PIN8
// #define TINY_SOFT_PWM_USES_PIN9
// #define TINY_SOFT_PWM_USES_PIN10
// #define TINY_SOFT_PWM_USES_PIN11
// #define TINY_SOFT_PWM_USES_PIN12
// #define TINY_SOFT_PWM_USES_PIN13


/*******************************************************************/
/* Do NOT modify below: it's used to optimize RAM and Program size */
/*******************************************************************/
#if defined (__AVR_ATtiny85__)
#undef TINY_SOFT_PWM_USES_PIN6
#undef TINY_SOFT_PWM_USES_PIN7
#undef TINY_SOFT_PWM_USES_PIN8
#undef TINY_SOFT_PWM_USES_PIN9
#undef TINY_SOFT_PWM_USES_PIN10
#undef TINY_SOFT_PWM_USES_PIN11
#undef TINY_SOFT_PWM_USES_PIN12
#undef TINY_SOFT_PWM_USES_PIN13
#endif

#ifdef TINY_SOFT_PWM_USES_PIN0
#undef TINY_SOFT_PWM_USES_PIN0
#define TINY_SOFT_PWM_USES_PIN0  1
#else
#define TINY_SOFT_PWM_USES_PIN0  0
#endif

#ifdef TINY_SOFT_PWM_USES_PIN1
#undef TINY_SOFT_PWM_USES_PIN1
#define TINY_SOFT_PWM_USES_PIN1  1
#else
#define TINY_SOFT_PWM_USES_PIN1  0
#endif

#ifdef TINY_SOFT_PWM_USES_PIN2
#undef TINY_SOFT_PWM_USES_PIN2
#define TINY_SOFT_PWM_USES_PIN2  1
#else
#define TINY_SOFT_PWM_USES_PIN2  0
#endif

#ifdef TINY_SOFT_PWM_USES_PIN3
#undef TINY_SOFT_PWM_USES_PIN3
#define TINY_SOFT_PWM_USES_PIN3  1
#else
#define TINY_SOFT_PWM_USES_PIN3  0
#endif

#ifdef TINY_SOFT_PWM_USES_PIN4
#undef TINY_SOFT_PWM_USES_PIN4
#define TINY_SOFT_PWM_USES_PIN4  1
#else
#define TINY_SOFT_PWM_USES_PIN4  0
#endif

#ifdef TINY_SOFT_PWM_USES_PIN5
#undef TINY_SOFT_PWM_USES_PIN5
#define TINY_SOFT_PWM_USES_PIN5  1
#else
#define TINY_SOFT_PWM_USES_PIN5  0
#endif

#ifdef TINY_SOFT_PWM_USES_PIN6
#undef TINY_SOFT_PWM_USES_PIN6
#define TINY_SOFT_PWM_USES_PIN6  1
#else
#define TINY_SOFT_PWM_USES_PIN6  0
#endif

#ifdef TINY_SOFT_PWM_USES_PIN7
#undef TINY_SOFT_PWM_USES_PIN7
#define TINY_SOFT_PWM_USES_PIN7  1
#else
#define TINY_SOFT_PWM_USES_PIN7  0
#endif

#ifdef TINY_SOFT_PWM_USES_PIN8
#undef TINY_SOFT_PWM_USES_PIN8
#define TINY_SOFT_PWM_USES_PIN8  1
#else
#define TINY_SOFT_PWM_USES_PIN8  0
#endif

#ifdef TINY_SOFT_PWM_USES_PIN9
#undef TINY_SOFT_PWM_USES_PIN9
#define TINY_SOFT_PWM_USES_PIN9  1
#else
#define TINY_SOFT_PWM_USES_PIN9  0
#endif

#ifdef TINY_SOFT_PWM_USES_PIN10
#undef TINY_SOFT_PWM_USES_PIN10
#define TINY_SOFT_PWM_USES_PIN10 1
#else
#define TINY_SOFT_PWM_USES_PIN10 0
#endif

#ifdef TINY_SOFT_PWM_USES_PIN11
#undef TINY_SOFT_PWM_USES_PIN11
#define TINY_SOFT_PWM_USES_PIN11 1
#else
#define TINY_SOFT_PWM_USES_PIN11 0
#endif

#ifdef TINY_SOFT_PWM_USES_PIN12
#undef TINY_SOFT_PWM_USES_PIN12
#define TINY_SOFT_PWM_USES_PIN12 1
#else
#define TINY_SOFT_PWM_USES_PIN12 0
#endif

#ifdef TINY_SOFT_PWM_USES_PIN13
#undef TINY_SOFT_PWM_USES_PIN13
#define TINY_SOFT_PWM_USES_PIN13 1
#else
#define TINY_SOFT_PWM_USES_PIN13 0
#endif

#define TINY_SOFT_PWM_CH_MAX     (TINY_SOFT_PWM_USES_PIN0  + TINY_SOFT_PWM_USES_PIN1 + TINY_SOFT_PWM_USES_PIN2 + \
                                  TINY_SOFT_PWM_USES_PIN3  + TINY_SOFT_PWM_USES_PIN4 + TINY_SOFT_PWM_USES_PIN5 + \
                                  TINY_SOFT_PWM_USES_PIN6  + TINY_SOFT_PWM_USES_PIN7 + TINY_SOFT_PWM_USES_PIN8 + \
                                  TINY_SOFT_PWM_USES_PIN9  + TINY_SOFT_PWM_USES_PIN10+ TINY_SOFT_PWM_USES_PIN11+ \
                                  TINY_SOFT_PWM_USES_PIN12 + TINY_SOFT_PWM_USES_PIN13)

#if defined (__AVR_ATtiny85__)
#define TINY_SOFT_PWM_USES_PORT0  0
#define TINY_SOFT_PWM_USES_PORT1  1
#ifndef digitalPinToPortIdx
#define digitalPinToPortIdx(p)    1
#endif
#else
#if defined (__AVR_ATtiny167__)
#define TINY_SOFT_PWM_USES_PORT0  (TINY_SOFT_PWM_USES_PIN5  || TINY_SOFT_PWM_USES_PIN6 || TINY_SOFT_PWM_USES_PIN7  || \
                                   TINY_SOFT_PWM_USES_PIN8  || TINY_SOFT_PWM_USES_PIN9 || TINY_SOFT_PWM_USES_PIN10 || \
                                   TINY_SOFT_PWM_USES_PIN11 || TINY_SOFT_PWM_USES_PIN12)
#define TINY_SOFT_PWM_USES_PORT1  (TINY_SOFT_PWM_USES_PIN0  || TINY_SOFT_PWM_USES_PIN1 || TINY_SOFT_PWM_USES_PIN2  || \
                                   TINY_SOFT_PWM_USES_PIN3  || TINY_SOFT_PWM_USES_PIN4)

#ifndef digitalPinToPortIdx
#define digitalPinToPortIdx(p)    (((p) >= 5 && (p) <= 12) ? (0) : (1))
#endif
#else
/* Last supported target is assumed to be ATmega328p (UNO) */
#define TINY_SOFT_PWM_USES_PORT0  (TINY_SOFT_PWM_USES_PIN0  || TINY_SOFT_PWM_USES_PIN1  || TINY_SOFT_PWM_USES_PIN2  || \
                                   TINY_SOFT_PWM_USES_PIN3  || TINY_SOFT_PWM_USES_PIN4  || TINY_SOFT_PWM_USES_PIN5  || \
                                   TINY_SOFT_PWM_USES_PIN6  || TINY_SOFT_PWM_USES_PIN7)
#define TINY_SOFT_PWM_USES_PORT1  (TINY_SOFT_PWM_USES_PIN8  || TINY_SOFT_PWM_USES_PIN9  || TINY_SOFT_PWM_USES_PIN10 || \
                                   TINY_SOFT_PWM_USES_PIN11 || TINY_SOFT_PWM_USES_PIN12 || TINY_SOFT_PWM_USES_PIN13)
#ifndef digitalPinToPortIdx
#define digitalPinToPortIdx(p)    (((p) <= 7 ) ? (0) : (1))
#endif
#endif
#endif

#if (TINY_SOFT_PWM_USES_PORT0 == 1)
#ifdef PORTA /* Does not exist on UNO */
#undef  TINY_SOFT_PWM_USES_PORT0
#define TINY_SOFT_PWM_PORT0       PORTA
#define TINY_SOFT_PWM_DDR0        DDRA
#else
/* ATmega328p (UNO) */
#warning PORTD
#define TINY_SOFT_PWM_PORT0       PORTD
#define TINY_SOFT_PWM_DDR0        DDRD
#endif
#endif

#if (TINY_SOFT_PWM_USES_PORT1 == 1)
#undef  TINY_SOFT_PWM_USES_PORT1
#define TINY_SOFT_PWM_PORT1       PORTB
#define TINY_SOFT_PWM_DDR1        DDRB
#endif

#ifndef digitalPinToPortBit
#define digitalPinToPortBit(p)    digitalPinToPCMSKbit(p)
#endif

/* Public Function Prototypes */
void TinySoftPwm_begin(uint8_t TickMax, uint8_t PwmInit);
void TinySoftPwm_analogWrite(uint8_t Pin, uint8_t Pwm);
void TinySoftPwm_process(void);

#endif
