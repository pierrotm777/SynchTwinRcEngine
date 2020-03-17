#ifdef LCDON


/* ********************************************************************* */
/* Control: Analog with one pin                                          */
/* (different resitors values are needed for any button                  */
/* ********************************************************************* */ 
//#if (_LCDML_DISP_cfg_control == 1)
void LCDML_CONTROL_analog()
/* ********************************************************************* */
{
  if((millis() - g_LCDML_DISP_press_time) >= _LCDML_DISP_cfg_button_press_time) {
    g_LCDML_DISP_press_time = millis(); // reset press time
    
    uint16_t value = anaRead(_LCDML_CONTROL_analog_pin);  // analogpin for keypad
    
    if (value >= _LCDML_CONTROL_analog_enter_min && value <= _LCDML_CONTROL_analog_enter_max) { LCDML_BUTTON_enter(); }
//    if (value >= _LCDML_CONTROL_analog_up_min    && value <= _LCDML_CONTROL_analog_up_max)    { LCDML_BUTTON_up();    }
    if (value >= _LCDML_CONTROL_analog_down_min  && value <= _LCDML_CONTROL_analog_down_max)  { LCDML_BUTTON_down();  }
//    if (value >= _LCDML_CONTROL_analog_left_min  && value <= _LCDML_CONTROL_analog_left_max)  { LCDML_BUTTON_left();  }
//    if (value >= _LCDML_CONTROL_analog_right_min && value <= _LCDML_CONTROL_analog_right_max) { LCDML_BUTTON_right(); }
//    if (value >= _LCDML_CONTROL_analog_back_min  && value <= _LCDML_CONTROL_analog_back_max)  { LCDML_BUTTON_quit();  }
  }
}


/* ===================================================================== */
/*
/* LCDMenuLib BACKEND FUNCTION - do not change here something            */
/*
/* ===================================================================== */

/* ********************************************************************* */
void LCDML_BACK_setup(LCDML_BACKEND_control)      
/* ********************************************************************* */
{
  // setup of this backend function is called only at first start or reset   
}
boolean LCDML_BACK_loop(LCDML_BACKEND_control)
{ 
  // loop of this backend function
  if(bitRead(LCDML.control, _LCDML_control_funcend)) {    
    LCDML_BACK_reset(LCDML_BACKEND_menu); // reset setup function for DISP function 
    LCDML_BACK_dynamic_setDefaultTime(LCDML_BACKEND_menu); // reset trigger settings
    LCDML_BACK_stopStable(LCDML_BACKEND_menu); // stop an menu function stable    
  }  
   // example for init screen
//  #if (_LCDML_DISP_cfg_initscreen == 1)
//  if((millis() - g_lcdml_initscreen) >= _LCDML_DISP_cfg_initscreen_time) {
//    g_lcdml_initscreen = millis(); // reset init screen time
//    LCDML_DISP_jumpToFunc(LCDML_FUNC_initscreen); // jump to initscreen     
//  }  
//  #endif  
  
  // check control settings of lcdml         
//  #if(_LCDML_DISP_cfg_control == 1)  
  LCDML_CONTROL_analog();       
//  #endif  
 
  return true;  
}

// backend stop stable
void LCDML_BACK_stable(LCDML_BACKEND_control)
{
}
/* ===================================================================== */
/*
/* OWN BACKEND FUNCTION */
/*
/* ===================================================================== */




#endif //endif LCDON
