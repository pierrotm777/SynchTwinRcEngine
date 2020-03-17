#ifndef _LCDML_SETUP_H_
#define _LCDML_SETUP_H_

#ifdef LCDON

//#define _LCDML_DISP_cfg_control                  1    /* 0=seriell, 1=analog, 2=digital, 3=encoder */
/*
       +5V
       -+-
        |
----.   # R=4.7K
    |   |
  A6|---+--------.
    |   |        |
UNO |   # R=10K  # R=4.7K
    |   |        |
    |    |> BP1   |> PB2
    |   |        |
    |  -+-      -+-
----'  GND      GND
*/

//#if(_LCDML_DISP_cfg_control == 1)//pin A0 = 400 if enter & down
#  define _LCDML_CONTROL_analog_pin              A6
#  define _LCDML_CONTROL_analog_enter_min        680     // Button Enter
#  define _LCDML_CONTROL_analog_enter_max        700  
#  define _LCDML_CONTROL_analog_down_min         490     // Button Down
#  define _LCDML_CONTROL_analog_down_max         510
//#  define _LCDML_CONTROL_analog_up_min           520     // Button Up
//#  define _LCDML_CONTROL_analog_up_max           590
//#  define _LCDML_CONTROL_analog_back_min         950     // Button Back
//#  define _LCDML_CONTROL_analog_back_max         1020   
//#  define _LCDML_CONTROL_analog_left_min         430     // Button Left
//#  define _LCDML_CONTROL_analog_left_max         500   
//#  define _LCDML_CONTROL_analog_right_min        610     // Button Right
//#  define _LCDML_CONTROL_analog_right_max        680  
#  define _LCDML_CONTROL_analog_enter_down_min   390
#  define _LCDML_CONTROL_analog_enter_down_max   420 
//#endif

/* settings for lcd */
#define _LCDML_DISP_cols             20
#define _LCDML_DISP_rows             4
/* i2c address */
#define _LCDML_DISP_addr             39//EEPROM.read(29);/* 39 in decimal or 0x27 in hexa, by default */

#define _LCDML_DISP_e                               2
#define _LCDML_DISP_rs                              0
#define _LCDML_DISP_rw	                            1
#define _LCDML_DISP_backlight                       3
#define _LCDML_DISP_backlight_pol                   POSITIVE // NEGATIVE
#define _LCDML_DISP_dat4                            4
#define _LCDML_DISP_dat5                            5
#define _LCDML_DISP_dat6                            6
#define _LCDML_DISP_dat7                            7

/* lib config */
#define _LCDML_DISP_cfg_button_press_time           150    /* button press time in ms */
#define _LCDML_DISP_cfg_scrollbar	                  1      /* 0 = no scrollbar, 1 = scrollbar with custom chars */
#define _LCDML_DISP_cfg_scrollbar_custom_redefine   0      /* 0 = nothing, 1 = redefine custom chars on scrolling */
#define _LCDML_DISP_cfg_lcd_standard                0      /* 0 = HD44780 standard / 1 = HD44780U standard */
//#define _LCDML_DISP_cfg_initscreen                  0
//#define _LCDML_DISP_cfg_initscreen_time             5000

/* menu element count - last element id */ 
#define _LCDML_DISP_cnt                            13//24 

// LCDML_root        => layer 0 
// LCDML_root_X      => layer 1 
// LCDML_root_X_X    => layer 2 
// LCDML_root_X_X_X  => layer 3 
// LCDML_root_...    => layer ... 

/* init lcdmenulib */
LCDML_DISP_init(_LCDML_DISP_cnt);

/* LCDMenuLib_element(id, group, prev_layer_element, new_element_num, lang_char_array, callback_function) */
LCDML_DISP_add(0  ,  _LCDML_G1  , LCDML_root         , 1  , ".Settings"          , LCDML_FUNC);
  LCDML_DISP_add(1  ,  _LCDML_G1  , LCDML_root_1       , 1  , "General Calibrate"  , LCDML_mode1);/* Min Gene, Max Gene, Center 1&2, Idle 1&2, Full Throttle, Begin Synchro */
  LCDML_DISP_add(2  ,  _LCDML_G1  , LCDML_root_1       , 2  , "Response time"      , LCDML_mode2);
  LCDML_DISP_add(3  ,  _LCDML_G1  , LCDML_root_1       , 3  , "Auxiliary chanel"   , LCDML_mode2);
  LCDML_DISP_add(4 ,  _LCDML_G1   , LCDML_root_1       , 4  , "Nb of blades"       , LCDML_mode2);
  LCDML_DISP_add(5 ,  _LCDML_G1   , LCDML_root_1       , 5  , "Reverse servo 1&2"  , LCDML_mode2);
  LCDML_DISP_add(6 ,  _LCDML_G1   , LCDML_root_1       , 6 , "Back"               , LCDML_FUNC_back);
LCDML_DISP_add(7 ,  _LCDML_G1   , LCDML_root         , 2  , ".Tools"             , LCDML_FUNC);
  LCDML_DISP_add(8 ,  _LCDML_G1   , LCDML_root_2       , 1  , "Engine Recorder"    , LCDML_FUNC);
  LCDML_DISP_add(9 ,  _LCDML_G1   , LCDML_root_2_1     , 1  , "Play"           , LCDML_Recorder);
  LCDML_DISP_add(10 ,  _LCDML_G1  , LCDML_root_2_1     , 2  , "Record"         , LCDML_Recorder);
  LCDML_DISP_add(11 ,  _LCDML_G1  , LCDML_root_2_1     , 3  , "Back"           , LCDML_FUNC_back);
  LCDML_DISP_add(12 ,  _LCDML_G1  , LCDML_root_2       , 2  , "Temp / VBat"        , LCDML_intTempExtVoltage);
  LCDML_DISP_add(13 ,  _LCDML_G1  , LCDML_root_2       , 3  , "Back"               , LCDML_FUNC_back);
//  LCDML_DISP_add(14 ,  _LCDML_G1  , LCDML_root_2       , 3  , "Reset Settings"     , LCDML_ResetSettings);
//  LCDML_DISP_add(15 ,  _LCDML_G1  , LCDML_root_2       , 4  , "Servo test"         , LCDML_ServosTest);
//  LCDML_DISP_add(16 ,  _LCDML_G1  , LCDML_root_2       , 5  , "Back"               , LCDML_FUNC_back);
//LCDML_DISP_add(17 ,  _LCDML_G1  , LCDML_root         , 3  , ".Greetings"         , LCDML_Greetings);
//LCDML_DISP_add(18 ,  _LCDML_G1  , LCDML_root         , 4  , ".Back"              , LCDML_FUNC_root);
//LCDML_DISP_add(19 ,  _LCDML_G1  , LCDML_root         , 5  , "InitScreen"         , LCDML_FUNC_initscreen);

LCDML_DISP_createMenu(_LCDML_DISP_cnt);

/* define backend function */
#define _LCDML_BACK_cnt    1  // last backend function id
LCDML_BACK_init(_LCDML_BACK_cnt);
LCDML_BACK_new_timebased_static  (0  , ( 20UL )         , _LCDML_start  , LCDML_BACKEND_control);
LCDML_BACK_new_timebased_dynamic (1  , ( 1000UL )       , _LCDML_stop   , LCDML_BACKEND_menu);
/* own backend function */

LCDML_BACK_create();

/* Custom LCD character char array addresses. */
/* character generator: http://www.quinapalus.com/hd44780udg.html */
/* bargraph */
//#define BARGRAPHINPROGMEM
#ifdef BARGRAPHINPROGMEM
const char charBarGraph0[] PROGMEM ={B10000,B10000,B10000,B10000,B10000,B10000,B10000,B00000}; 
const char charBarGraph1[] PROGMEM ={B10000,B10000,B10000,B10000,B10000,B10000,B10000,B00000}; 
const char charBarGraph2[] PROGMEM ={B11000,B11000,B11000,B11000,B11000,B11000,B11000,B00000};
const char charBarGraph3[] PROGMEM ={B11100,B11100,B11100,B11100,B11100,B11100,B11100,B00000}; 
const char charBarGraph4[] PROGMEM ={B11110,B11110,B11110,B11110,B11110,B11110,B11110,B00000}; 
const char charBarGraph5[] PROGMEM ={B11111,B11111,B11111,B11111,B11111,B11111,B11111,B00000};
const char charBarGraph6[] PROGMEM ={B1110,B10001,B10001,B10001,B1110,0,0,0}; // degre
const char * const charBarGraph[] PROGMEM = {charBarGraph0, charBarGraph1, charBarGraph2, charBarGraph3, charBarGraph4, charBarGraph5, charBarGraph6};
#else
const char charBarGraph0[] ={B10000,B10000,B10000,B10000,B10000,B10000,B10000,B00000}; 
const char charBarGraph1[] ={B10000,B10000,B10000,B10000,B10000,B10000,B10000,B00000}; 
const char charBarGraph2[] ={B11000,B11000,B11000,B11000,B11000,B11000,B11000,B00000};
const char charBarGraph3[] ={B11100,B11100,B11100,B11100,B11100,B11100,B11100,B00000}; 
const char charBarGraph4[] ={B11110,B11110,B11110,B11110,B11110,B11110,B11110,B00000}; 
const char charBarGraph5[] ={B11111,B11111,B11111,B11111,B11111,B11111,B11111,B00000};
const char charBarGraph6[] ={B1110,B10001,B10001,B10001,B1110,0,0,0}; // degre
const char *  charBarGraph[] = {charBarGraph0, charBarGraph1, charBarGraph2, charBarGraph3, charBarGraph4, charBarGraph5, charBarGraph6};
#endif
byte charbackslash[8] ={B0,B10000,B1000,B100,B10,B1,B0,B0};

#endif /* end LCDON */
#endif /* _LCDML_SETUP_H_ */
