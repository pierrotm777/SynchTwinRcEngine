void UseSynchroMotors()
{

  //comparaison des differences de vitesses et compensations moteurs (plage d'erreur definit ici a 100tr/mn)
  diffVitesse = vitesse1 - vitesse2;

#ifdef PIDCONTROL
  double gap = diffVitesseErr-diffVitesse; //distance away from diffVitesseErr
  gap = abs(gap);
  if (gap < 10)
  {  //we're close to setpoint, use conservative tuning parameters
    myPID.SetTunings(consKp, consKi, consKd);
  }
  else
  {
     //we're far from setpoint, use aggressive tuning parameters
     myPID.SetTunings(aggKp, aggKi, aggKd);
  }
  
  myPID.Compute();
 
#else
  stepMotor = 10;
#endif

/*    
         
       V2            V1   
      --+--         --+--
 _______|_____/ \_____|_______
|_______|_____| |_____________|
              | |
              | |
              | |
            __| |__
            |_____|

*/

  
  if ((vitesse1 > minimumSpeed) && (vitesse2 > minimumSpeed))//if diff exist synchro is active
  {
    if(abs(diffVitesse) >=  diffVitesseErr)//here, diffVitesseErr = 100;
    {
      if (diffVitesse < 0)//V1 < V2 
      {
        if (reverseServo1 == 0) {ServoMotor1.write_us(Width_us + (int16_t)(stepMotor));}else{ServoMotor1.write_us((centerposServo1*2)-Width_us + (int16_t)(stepMotor));}               
        if (reverseServo2 == 0) {ServoMotor2.write_us(Width_us - (int16_t)(stepMotor));}else{ServoMotor2.write_us((centerposServo2*2)-Width_us - (int16_t)(stepMotor));}        
      }
      else if (diffVitesse > 0)//V1 > V2
      {         
        if (reverseServo1 == 0) {ServoMotor1.write_us(Width_us - (int16_t)(stepMotor));}else{ServoMotor1.write_us((centerposServo1*2)-Width_us - (int16_t)(stepMotor));}               
        if (reverseServo2 == 0) {ServoMotor2.write_us(Width_us + (int16_t)(stepMotor));}else{ServoMotor2.write_us((centerposServo2*2)-Width_us + (int16_t)(stepMotor));}        
      }
    }
    else
    {//si les capteurs fonctionnent et V1 = V2 à +- diffVitesseErr près           
      if (reverseServo1 == 0) {ServoMotor1.write_us(Width_us);}else{ServoMotor1.write_us((centerposServo1*2)-Width_us);}               
      if (reverseServo2 == 0) {ServoMotor2.write_us(Width_us);}else{ServoMotor2.write_us((centerposServo2*2)-Width_us);}        
    }
  }

#ifdef SECURITYENGINE
/* security moteurs */
  else if(vitesse1 < minimumSpeed && vitesse2 > minimumSpeed )//arret des 2 moteurs pendant 1s puis mise en securite de 1 et reprise de 2
  {
    //ajouter mixage Aux avec gouvernail
    if(millis() - BeginIdleMode < 1000)/* Compteur mise en securite pendant 1s */
    {
      if (reverseServo1 == 0) {ServoMotor1.write_us(idelposServos1);}else{ServoMotor1.write_us((centerposServo1*2)-idelposServos1);}               
      if (reverseServo2 == 0) {ServoMotor2.write_us(idelposServos2);}else{ServoMotor2.write_us((centerposServo2*2)-idelposServos2);}
    }
    else
    {
      if (reverseServo1 == 0) {ServoMotor1.write_us(idelposServos1);}else{ServoMotor1.write_us((centerposServo1*2)-idelposServos1);}               
      if (reverseServo2 == 0) {ServoMotor2.write_us(Width_us);}else{ServoMotor2.write_us((centerposServo2*2)-Width_us);}
      BeginIdleMode=millis(); /* Restart the Chrono */
    }
  }
  
  else if(vitesse2 < minimumSpeed && vitesse1 > minimumSpeed)//arret des 2 moteurs pendant 1s puis mise en securite de 2 et reprise de 1
  {
    //ajouter mixage Aux avec gouvernail
    if(millis() - BeginIdleMode < 1000)/* Compteur mise en securite pendant 1s */
    {
      if (reverseServo1 == 0) {ServoMotor1.write_us(idelposServos1);}else{ServoMotor1.write_us((centerposServo1*2)-idelposServos1);}               
      if (reverseServo2 == 0) {ServoMotor2.write_us(idelposServos2);}else{ServoMotor2.write_us((centerposServo2*2)-idelposServos2);}
    }
    else
    {
      if (reverseServo1 == 0) {ServoMotor1.write_us(Width_us);}else{ServoMotor1.write_us((centerposServo1*2)-Width_us);}               
      if (reverseServo2 == 0) {ServoMotor2.write_us(idelposServos2);}else{ServoMotor2.write_us((centerposServo2*2)-idelposServos2);}
      BeginIdleMode=millis(); /* Restart the Chrono */
    }
    
  }
/* fin securite moteurs */
#endif

  else if(vitesse1 == 0 || vitesse2 == 0)//vitesse1=0 et vitesse2=0
  {// Si les capteurs ne marchent pas ou moteurs eteints
    ServoMoteursWrite_Us(); 
  }
}

