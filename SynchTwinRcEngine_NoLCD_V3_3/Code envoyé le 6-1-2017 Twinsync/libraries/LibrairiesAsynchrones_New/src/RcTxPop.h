#ifndef RcTxPop_h
#define RcTxPop_h

class RcTxPop
{
  public:
    virtual uint8_t  RcTxPopIsSynchro() = 0;
    virtual void     RcTxPopSetWidth_us(uint16_t Width_us, uint8_t Ch = 255);
};

#endif