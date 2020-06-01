#ifdef GLOWMANAGER

//#define COUPURE_GAZ     1080  // en µs (correspond à ms.minimumPulse_US)
#define DELAI_CHAUFFAGE 1500  // en ms
#define COURANT_DEM     100   // en % courant moteur à l'arrêt pour démarrage
#define COURANT_TRN     60    // en % courant moteur tournant
#define VITESSE_GAZ     60    // 60 µS pour 20 ms (20ms = période impulsion RX)
uint16_t variation, pwm_chauffe, pos_coupure;
unsigned long timer_chauffe, temps_ms;
bool cmd_chauffe, moteur_marche;
int tpulse_0, tpulse_1, tpulse;

void glowSetup()
{
//    position_gaz = 1100;
 variation = 0;
 
 // Init chauffage bougie 
 timer_chauffe = 0;
 SecurityIsON = true;     // en securité à la mise sous tension
//  tempo_secu = false;
 
 // Sortie PWM chauffage = arrêt
 TinySoftPwm_analogWrite(BROCHE_GLOW1, 0); // Commande PWM MOSFET , courant bougie
 TinySoftPwm_analogWrite(BROCHE_GLOW2, 0); // Commande PWM MOSFET , courant bougie
 cmd_chauffe = false;
 pwm_chauffe = 0;

 pos_coupure = ms.minimumPulse_US;//COUPURE_GAZ;
}

void glowUpdate()
{
  //--------------------------------------------
  // Gestion de la sortie PWM de commande de chauffage
  // Conditions :
  // - Ralenti : seuil coupure_moteur < posistion_gaz < idelposServos1
  // - Sortie de ralenti : temporisation de xx ms
  // - Variation de gaz rapide
  
  // idelposServos1 : seuil de chauffage au ralenti
  // coupure_gaz : seuil de coupure du moteur
  // au ralenti le chauffage est réalisé entre "coupure_gaz" et "idelposServos1"
  
  if ((Width_us >= ms.minimumPulse_US) && (Width_us <= ms.idelposServos1)) 
  {
    cmd_chauffe = true;  
    if (moteur_marche == false) 
      pwm_chauffe = map(COURANT_DEM, 0, 100, 0, 255);     // Chauffage position ralenti moteur à l'arrêt
    else  
      pwm_chauffe = map(COURANT_TRN, 0, 100, 0, 255);     // Chauffage position ralenti moteur en marche      
    timer_chauffe = millis();   // temps courant depuis alimentation en ms
  }

  //--------------------------------------------- 
  // Detection de vitesse de variation de manche de Gaz
  // en valeur en (µs)
  if ((variation > VITESSE_GAZ) && (Width_us >= ms.minimumPulse_US)) 
  {
    cmd_chauffe = true;  
    pwm_chauffe = map(COURANT_TRN, 0, 100, 0, 255);    // Chauffage en variation rapide manche gaz
    timer_chauffe = millis();   // temps courant depuis alimentation en ms    
  }  

  // -----------------------------
  // Commande chauffage bougie
  // 
  if ((cmd_chauffe == true) && (SecurityIsON == false))
  { // Gestion avec SecurityIsON
    //digitalWriteFast(LED1,HIGH);//LEDCHAU_ON
    TinySoftPwm_analogWrite(BROCHE_GLOW1, pwm_chauffe); // Commande PWM MOSFET , courant bougie
    TinySoftPwm_analogWrite(BROCHE_GLOW2, pwm_chauffe); // Commande PWM MOSFET , courant bougie
    temps_ms = millis();   // temps courant depuis alimentation bougie en ms    
    // Fin de temporisation
    if ((temps_ms > (timer_chauffe + DELAI_CHAUFFAGE)))// &&  (tempo_secu == false)) // Coupure après tempo sauf si secu en cours
      cmd_chauffe = false;  // coupure 
  }
  else 
  {
    // coupure alimentation en courant de la bougie
    pwm_chauffe = 0;
    TinySoftPwm_analogWrite(BROCHE_GLOW1, pwm_chauffe);
    TinySoftPwm_analogWrite(BROCHE_GLOW2, pwm_chauffe); 
    //digitalWriteFast(LED1,LOW);//LEDCHAU_OFF
  } 

}

#endif
