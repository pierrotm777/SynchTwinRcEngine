
#define HalfDeadZone 100
void readAuxiliaryChannel()
{

  /* remplacer mini et maxi par idelposServos et fullThrottle ? */
//  static uint16_t OneQuarterThrottle = THROTTLE_FRACT(minimumPulse_US, maximumPulse_US, 1, 4); /* 1/4 */
  static uint16_t OneFifthThrottle = THROTTLE_FRACT(minimumPulse_US, maximumPulse_US, 1, 5); /* 1/5 */
  static uint16_t OneThirdThrottle = THROTTLE_FRACT(minimumPulse_US, maximumPulse_US, 1, 3); /* 1/3 */
  static uint16_t TwoThirdThrottle = THROTTLE_FRACT(minimumPulse_US, maximumPulse_US, 2, 3); /* 2/3 */
  /* remplacer mini et maxi par idelposServos et fullThrottle ? */
     
  /* a remplacer par les center servo 1&2 ? */
  static uint16_t MiddleThrottle = THROTTLE_FRACT(minimumPulse_US, maximumPulse_US, 1, 2); /* 1/2 */
  /* a remplacer par les center servo 1&2 ? */
  
//  Serial <<  "1/5 " << OneFifthThrottle << "| 1/3 " << OneThirdThrottle << "| 2/3 " << TwoThirdThrottle << "| Middle " << MiddleThrottle << endl;
  
  if(RxChannelPulseAux.available())//verifie si Ch Aux est actif !!!
  {
    AVERAGE(WidthAux_us,RxChannelPulseAux.width_us(), responseTime);
  }

  if(Width_us >= OneFifthThrottle){PIN_HIGH(B,3);}else{PIN_LOW(B,3);}

  //ajouter gestion de l'entree AUX avec inters 2 ou 3 positions:
  switch (auxChannel)
  {
      //AUX n'est pas connecte (moteurs synchronises au-dela de 1/5 de RX)
    case 1:
       if (Width_us >= OneFifthThrottle)
       {
         synchroIsActive=true;//synchro active
         if (SecurityIsON == false) UseSynchroMotors();
       }
       else
       {
         synchroIsActive=false;//synchro inactive
         ServoMoteursWrite_Us();
       }
       break;
       
      //mode ajustement du melange de carburant(inter 3 positions):
      //sous 1/3 moteur1 controle par RX et moteur2 en idle
      //entre 1/3 et 2/3 les moteurs sont controle par RX
      //au-dessus 2/3 le moteur1 est en idle et le moteur2 controle par RX
    case 2:
       synchroIsActive=false;//synchro inactive
       //Serial << "Idle 1: " << idelposServos1 << " Idle 2: " << idelposServos2 << " Aux: " << WidthAux_us << " Motors: " << Width_us << endl;
       if(WidthAux_us < OneThirdThrottle)//(OK)
       {
         //sous 1/3 ==> moteur1 controle par RX et moteur2 en idle 
         if (SecurityIsON == false)
         {  
           if (reverseServo1 == 0) {ServoMotor1.write_us(Width_us);}else{ServoMotor1.write_us((centerposServo1*2)-Width_us);}               
           if (reverseServo2 == 0) {ServoMotor2.write_us(idelposServos2);}else{ServoMotor2.write_us((centerposServo2*2)-idelposServos2);}
         }
         else
         {//mise en securite
           (reverseServo1 == 0)?ServoMotor1.write_us(minimumPulse_US):ServoMotor1.write_us(maximumPulse_US);               
           (reverseServo2 == 0)?ServoMotor2.write_us(minimumPulse_US):ServoMotor2.write_us(maximumPulse_US);                       
         }
       }
       if(WidthAux_us > TwoThirdThrottle)//(OK)
       {
         //au-dessus 2/3 ==> le moteur1 est en idle et le moteur2 controle par RX 
         if (SecurityIsON == false)
         {       
           if (reverseServo1 == 0) {ServoMotor1.write_us(idelposServos1);}else{ServoMotor1.write_us((centerposServo1*2)-idelposServos1);}               
           if (reverseServo2 == 0) {ServoMotor2.write_us(Width_us);}else{ServoMotor2.write_us((centerposServo2*2)-Width_us);}
         }
         else
         {//mise en securite
           (reverseServo1 == 0)?ServoMotor1.write_us(minimumPulse_US):ServoMotor1.write_us(maximumPulse_US);               
           (reverseServo2 == 0)?ServoMotor2.write_us(minimumPulse_US):ServoMotor2.write_us(maximumPulse_US);                       
         }
       }
       if((WidthAux_us >= OneThirdThrottle) && (WidthAux_us <= TwoThirdThrottle))//(OK)
       {
         //entre 1/3 et 2/3 ==> les moteurs sont controle par RX 
         ServoMoteursWrite_Us();
       }
       break;
       
      //activation synchro on/off (inter 2 positions)
      //position 1, synchro est active
      //position 2, pas de synchro (idem cable Y sur les moteurs) 
    case 3:
       if (WidthAux_us > minimumPulse_US)
       {
         synchroIsActive=true;//synchro active
         UseSynchroMotors();
       }
       else if (WidthAux_us < maximumPulse_US)
       {
         synchroIsActive=false;//synchro inactive
         ServoMoteursWrite_Us();
       }
       break;
       
      //mixage gouvernail/moteurs (auxiliaire connectee a direction du RX)
    case 4:
       //code mixant le gouvernail avec les moteurs au dela de 1/3 moteur
       static uint16_t LargeurMixed;
       if (Width_us < OneThirdThrottle)
       {
         synchroIsActive=true;//synchro active
         UseSynchroMotors();
       }
       else
       {
         //http://arduino.cc/en/reference/constrain
        //Val=un calcul quelconque
        //Val=constrain(Val, PulseMin_US, PulseMax_US); //Val final sera forcement entre PulseMin_US et PulseMax_US
         
         
         synchroIsActive=false;//synchro non active
         /*Min Dir        Middle Dir            Max Dir
         +---------XXX--------+--------------------+         
         */
         //HalfDeadZone definie une zone morte autour du point milieu
         if (WidthAux_us >= minimumPulse_US && WidthAux_us <= (HalfDeadZone - MiddleThrottle))
         { //   1000 > Aux < 1500
           LargeurMixed = map(WidthAux_us, minimumPulse_US, centerposServo1, centerposServo1/3, 0);
           if (SecurityIsON == false)
           {
             if (reverseServo1 == 0) {ServoMotor1.write_us(Width_us-LargeurMixed);}else{ServoMotor1.write_us((centerposServo1*2)-Width_us+LargeurMixed);}        
             if (reverseServo2 == 0) {ServoMotor2.write_us(Width_us);}else{ServoMotor2.write_us((centerposServo2*2)-Width_us);}
           }
           else
           {//mise en securite
             (reverseServo1 == 0)?ServoMotor1.write_us(minimumPulse_US):ServoMotor1.write_us(maximumPulse_US);               
             (reverseServo2 == 0)?ServoMotor2.write_us(minimumPulse_US):ServoMotor2.write_us(maximumPulse_US);                         
           }
         }
         /*Min Dir        Middle Dir            Max Dir
         +--------------------+---------XXX--------+         
         */
         if (WidthAux_us >= (MiddleThrottle + HalfDeadZone) && WidthAux_us <= maximumPulse_US)
         {
           LargeurMixed = map(WidthAux_us, centerposServo2, maximumPulse_US, 0, centerposServo2/3);
           if (SecurityIsON == false)
           {
             if (reverseServo1 == 0) {ServoMotor1.write_us(Width_us);}else{ServoMotor1.write_us((centerposServo1*2)-Width_us);}
             if (reverseServo2 == 0) {ServoMotor2.write_us(Width_us-LargeurMixed);}else{ServoMotor2.write_us((centerposServo2*2)-Width_us+LargeurMixed);}
           }
           else
           {//mise en securite
             (reverseServo1 == 0)?ServoMotor1.write_us(minimumPulse_US):ServoMotor1.write_us(maximumPulse_US);               
             (reverseServo2 == 0)?ServoMotor2.write_us(minimumPulse_US):ServoMotor2.write_us(maximumPulse_US);              
           }
         }
         /*Min Dir        Middle Dir            Max Dir
         +-------------------XXX-------------------+         
         */
         if (WidthAux_us >= (MiddleThrottle - HalfDeadZone) && WidthAux_us <= (MiddleThrottle + HalfDeadZone))
         {
           ServoMoteursWrite_Us();
         }
       }
       break;
       
      //controle bougies on/off (inter 2 positions)
    case 5:
#ifdef GLOWMANAGER
       if (WidthAux_us > MiddleThrottle)
       {
         glowControlIsActive=true;//glow active
         PIN_HIGH(D,6);PIN_HIGH(D,7);//command MOSFET gate
         PIN_HIGH(C,6);//Red Led is ON
       }
       else if (WidthAux_us < MiddleThrottle)
       {
         glowControlIsActive=false;//glow inactive
         PIN_LOW(D,6);PIN_LOW(D,7);//command MOSFET gate
         PIN_LOW(C,6);//Red Led is OFF
       }
#endif       
       break;
       
      //controle bougies on/off (inter 2 positions)
      //synchronisation permanente moteurs au dessus du niveau idle)
      //si moteurs off, les servos continuent a bouger      
    case 6:
#ifdef GLOWMANAGER    
       if (WidthAux_us > MiddleThrottle)
       {
         glowControlIsActive=true;//glow active
         PIN_HIGH(D,6);PIN_HIGH(D,7);//command MOSFET gate
         PIN_HIGH(C,6);//Red Led is ON
       }
       else if (WidthAux_us < MiddleThrottle)
       {
         glowControlIsActive=false;//glow inactive
         PIN_LOW(D,6);PIN_LOW(D,7);//command MOSFET gate
         PIN_LOW(C,6);//Red Led is OFF
       }
#endif       
       if((WidthAux_us > idelposServos1) && (WidthAux_us > idelposServos2) && (vitesse1 > 0) && (vitesse2 > 0))
       {
         synchroIsActive=true;//synchro active
         UseSynchroMotors();
       }
       else
       {
         synchroIsActive = false;//synchro moteur non active
         ServoMoteursWrite_Us();
       }        
       break;
           
  }
}

void ServoMoteursWrite_Us()
{
  if (SecurityIsON == false)
  {
    (reverseServo1 == 0)?ServoMotor1.write_us(Width_us):ServoMotor1.write_us((centerposServo1*2)-Width_us);
    (reverseServo2 == 0)?ServoMotor2.write_us(Width_us):ServoMotor2.write_us((centerposServo2*2)-Width_us);
  }
  else
  {//mise en securite
    (reverseServo1 == 0)?ServoMotor1.write_us(idelposServos1):ServoMotor1.write_us((centerposServo1*2)-idelposServos1);               
    (reverseServo2 == 0)?ServoMotor2.write_us(idelposServos2):ServoMotor2.write_us((centerposServo2*2)-idelposServos2);    
  }
}
