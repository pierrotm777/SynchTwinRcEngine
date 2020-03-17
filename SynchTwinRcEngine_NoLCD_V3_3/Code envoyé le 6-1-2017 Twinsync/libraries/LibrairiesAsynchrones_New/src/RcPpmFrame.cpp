#include "RcPpmFrame.h"
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

/*
Signal PPM transportant 4 voies: (Polarisation positive)
===============================
    300 us
 -->----<--
    .--.        .--.        .--.        .--.        .--.                               .--.        .--.        .--.        .--.        .--.
    |  |        |  |        |  |        |  |        |  |                               |  |        |  |        |  |        |  |        |  |
    |  |        |  |        |  |        |  |        |  |                               |  |        |  |        |  |        |  |        |  |
   -'  '--------'  '--------'  '--------'  '--------'  '-------------------------------'  '--------'  '--------'  '--------'  '--------'  '-
    X-----------X-----------X-----------X-----------X----------------------------------X-----------X-----------X-----------X-----------X
       VOIE 1      VOIE 2      VOIE 3      VOIE 4                 SYNCHRO                 VOIE 1      VOIE 2      VOIE 3      VOIE 4
   <----------->
  1000 -> 2000 us
   <----------------------------------------------------------------------------------->
                                  Periode # 20 ms
*/

/*************************************************************************
								MACROS
*************************************************************************/
#define CHANNEL_MAX_NB		8
#define PPM_PERIOD_US		20000
#define CH_START_PULSE_US	300
#define CH_NEUTRAL_US		1500

enum {PPM_START_PULSE_US=0, WAIT_FOR_END_OF_START_PULSE_US, WAIT_FOR_END_OF_PULSE_US};

/*************************************************************************
							GLOBAL VARIABLES
*************************************************************************/
static uint8_t ChannelMaxNb=CHANNEL_MAX_NB;
static uint8_t ChIdx=0;
static uint8_t PpmPin=255;
static boolean PpmPolar=RC_PPM_FRAME_POL_POS;

/*************************************************************************
					PRIVATE FUNCTION PROTOTYPES
*************************************************************************/
uint16_t PpmTbl[1+CHANNEL_MAX_NB]; /* +1 for Synchro */

//========================================================================================================================
void RcPpmFrame_Init(uint8_t PpmOutPin, uint8_t ChannelNb, boolean Polar/*=RC_PPM_FRAME_POL_POS*/)
{
uint8_t Idx;

  if(ChannelNb<=CHANNEL_MAX_NB) ChannelMaxNb=ChannelNb;
  for(Idx=1; Idx <= ChannelMaxNb; Idx++)
  {
    RcPpmFrame_SetChannelPulseWidth(Idx, CH_NEUTRAL_US); /* Set all Channel to Neutral */
  }
  PpmPin=PpmOutPin;
  pinMode(PpmPin, OUTPUT);
  ChIdx=ChannelMaxNb;
  RcPpmFrame_SetPolar(Polar);
}
//========================================================================================================================
void RcPpmFrame_SetPolar(boolean Polar)
{
  PpmPolar=Polar;
}
//========================================================================================================================
void RcPpmFrame_SetChannelPulseWidth(uint8_t ChannelIdx, uint16_t PulseWidth_us)
{
uint8_t Idx;
uint16_t Sum=0;

  if(ChannelIdx<=ChannelMaxNb)
  {
    PpmTbl[ChannelIdx]=PulseWidth_us-CH_START_PULSE_US;
    for(Idx=1; Idx <= ChannelMaxNb; Idx++)
    {
      Sum+=(PpmTbl[Idx]+CH_START_PULSE_US);
    }
    PpmTbl[0]=PPM_PERIOD_US-Sum-CH_START_PULSE_US;
  }
}
//========================================================================================================================
void RcPpmFrame_Generate()
{
static uint8_t  PpmState=PPM_START_PULSE_US;
static uint32_t StartUs;

  switch(PpmState)
  {
    case PPM_START_PULSE_US:
    StartUs=micros();
    digitalWrite(PpmPin,HIGH ^ !PpmPolar);
    ChIdx++;
    if(ChIdx>(ChannelMaxNb+0)) ChIdx=0;
    PpmState=WAIT_FOR_END_OF_START_PULSE_US;
    break;
    
    case WAIT_FOR_END_OF_START_PULSE_US:
    if((micros()-StartUs)>=CH_START_PULSE_US)
    {
      StartUs=micros();
      digitalWrite(PpmPin,LOW ^ !PpmPolar);
      PpmState=WAIT_FOR_END_OF_PULSE_US;
    }
    break;
    
    case WAIT_FOR_END_OF_PULSE_US:
    if((micros()-StartUs)>=PpmTbl[ChIdx])
    {
      PpmState=PPM_START_PULSE_US;
    }
    break;
  }
}
//========================================================================================================================
