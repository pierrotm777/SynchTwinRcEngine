void UseSynchroMotors()
{
  static uint16_t stepMotor = 10;        //nb de micro secondes ajoutes ou enleves
  static uint16_t diffVitesse = 0;       //difference de vitesse entre les 2 moteurs en tr/mn (peut etre negatif !!!)
  static uint16_t minimumSpeed = 500;   //A AJUSTER SELON LE RALENTI DES MOTEURS vitesse mini des moteurs (500tr/mn)
  
  //comparaison des differences de vitesses et compensations moteurs (plage d'erreur definit ici a 100tr/mn)
  diffVitesse = vitesse1 - vitesse2;
  stepMotor = map(diffVitesse,0, 20000,minimumPulse_US,maximumPulse_US);//convertie des tr/m en micros secondes (20000 est la vitesse maxi possible des moteurs)
  
  if ((vitesse1 > minimumSpeed) && (vitesse2 > minimumSpeed))
  {
    if(abs(diffVitesse) >=  diffVitesseErr)//ici, diffVitesseErr = 100 (>=1);
    {
      if (diffVitesse < 0)//V1 < V2 
      {
        if (reverseServo1 == 0) {ServoMotor1.write_us(Width_us + (int16_t)(stepMotor/2));}else{ServoMotor1.write_us((centerposServo1*2)-Width_us + (int16_t)(stepMotor/2));}               
        if (reverseServo2 == 0) {ServoMotor2.write_us(Width_us - (int16_t)(stepMotor/2));}else{ServoMotor2.write_us((centerposServo2*2)-Width_us - (int16_t)(stepMotor/2));}        
      }
      else if (diffVitesse > 0)//V1 > V2
      {         
        if (reverseServo1 == 0) {ServoMotor1.write_us(Width_us - (int16_t)(stepMotor/2));}else{ServoMotor1.write_us((centerposServo1*2)-Width_us - (int16_t)(stepMotor/2));}               
        if (reverseServo2 == 0) {ServoMotor2.write_us(Width_us + (int16_t)(stepMotor/2));}else{ServoMotor2.write_us((centerposServo2*2)-Width_us + (int16_t)(stepMotor/2));}        
      }
    }
    else
    {//si les capteurs fonctionnent et V1 = V2            
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
    //Width_us = 600;//arret des deux moteurs
//    if (reverseServo1 == 0) {ServoMotor1.write_us(Width_us);}else{ServoMotor1.write_us((centerposServo1*2)-Width_us);}               
//    if (reverseServo2 == 0) {ServoMotor2.write_us(Width_us);}else{ServoMotor2.write_us((centerposServo2*2)-Width_us);}
    ServoMoteursWrite_Us(); 
 
  }
}

