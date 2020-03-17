#ifndef RC_PPM_FRAME_H
#define RC_PPM_FRAME_H

/*
 English: by RC Navy (2012)
 =======
 <RcSeq> is an asynchronous library to generate a PPM pulse frame of a RC Transmitter/Receiver set.
 http://p.loussouarn.free.fr

 Francais: par RC Navy (2012)
 ========
 <RcSeq> est une librairie asynchrone pour generer le train d'impulsion PPM d'un ensemble RC.
 http://p.loussouarn.free.fr
*/

#if defined(ARDUINO) && ARDUINO >= 100
#include "Arduino.h"
#else
#include "WProgram.h"
#endif

#include <inttypes.h>
#include <stdio.h>

enum {RC_PPM_FRAME_POL_NEG=0, RC_PPM_FRAME_POL_POS};

void RcPpmFrame_Init(uint8_t PpmOutPin, uint8_t ChannelNb, boolean Polar=RC_PPM_FRAME_POL_POS);
void RcPpmFrame_SetPolar(boolean Polar);
void RcPpmFrame_SetChannelPulseWidth(uint8_t ChannelIdx, uint16_t PulseWidth_us);
void RcPpmFrame_Generate(void);

#endif
