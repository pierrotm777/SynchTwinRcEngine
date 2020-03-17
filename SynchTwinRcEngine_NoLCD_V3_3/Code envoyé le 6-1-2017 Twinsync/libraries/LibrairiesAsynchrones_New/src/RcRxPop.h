#ifndef RcRxPop_h
#define RcRxPop_h

class RcRxPop
{
  public:
    virtual uint8_t  RcRxPopIsSynchro() = 0;
    virtual uint16_t RcRxPopGetWidth_us(uint8_t Ch) = 0;
};

#endif