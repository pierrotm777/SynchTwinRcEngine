******************************************************************************************************************
* Augmentation ou diminution des regimes moteurs en fonction de la lecture des capteurs Effet Hall ou capteur IR.*
******************************************************************************************************************
 
 A-Le capteur Effet Hall US5881LUA est disponible chez MC Hobby
     http://shop.mchobby.be/product.php?id_product=86 
 Les aimants surpuissant de Rare Earth sont aussi disponibles chez MC Hobby
     http://shop.mchobby.be/product.php?id_product=87 

 * Le capteur Effet Hall US5881LUA est connecte comme suit:
     Pin 1: +5v
     Pin 2: Masse/GND
     Pin 3: +5V via une resistance pull-up de 10 KOhms
            MAIS AUSSI
            sur les PIN A0 et A1 d'Arduino (pour lecture des senseurs)
 * Exemple de circuit, plan sur :
     http://mchobby.be/wiki/index.php?title=Senseur_%C3%A0_Effet_Hall#Code_Arduino
	     
 -Autre capteur IR possible:
     http://www.miniinthebox.com/fr/lm393-capteur-de-reflexion-infrarouge-module-k1208042-bricolage-pour-arduino-bleu_p1392686.html
 
 -Contruire son capteur IR:
     http://maxembedded.com/2013/08/04/how-to-build-an-ir-sensor/
     http://www.instructables.com/id/DIY-Infrared-Sensor-Module/
	 
 -Autre capteur IR GP2Y0D810Z0F 10cm:
     http://boutique.semageek.com/fr/139-capteur-de-d%C3%A9tection-d-obtacle-infrarouge-gp2y0d810z0f.html


 */
 
/* biblioththeque pour accelerer le PWM
https://github.com/projectgus/digitalIOPerformance
*/
//#define DIGITALIO_NO_MIX_ANALOGWRITE
//#include "digitalIOPerformance.h"

/*Librairies Asynchrones
http://p.loussouarn.free.fr/arduino/asynchrone/asynchrone.html

Utilisations des 6 modes du canal auxiliaire:
MODE1: No AUX CH
  C'est le mode dans lequel le module devra etre is l'entree AUX n'est pas conectee.
  Le module laisse l'entree gaz controler les servos jusqu'a 1/5eme de sa position.
  Au dela des 1/5eme des gaz le module controle la position des servos et synchronise 
  les moteurs.Si le manche des gaz bouge, le processus est repete.

  
MODE2: Independent Run Up Mode
  Dans ce mode, l'entree AUX est connectee a un switch 3 positions de l'emetteur.
  Si la sortie de ce canal est en dessous de 1/3 de sa course le moteur 1 est controlle par
  le manche des gaz pendant que le moteur 2 est maintenu dans la position 'idle' programmee.
  Si la sortie du canal AUX est entre 1/3 et 2/3 de sa course, le manche des gaz controle les
  deux moteurs et le module comme si etait dans le mode 1.Si la sortie AUX est au dela des 2/3
  de sa course, le manche des gaz controle le moteur 2 et le moteur 1 est maintenu dans la 
  position 'idle' programmee.Ce mode est ideal pour l'ajustement du melange de carburant.
  
MODE3: AUX CH Sync Defeat
  Dans ce mode, le canal AUX est ossocie a un switch 2 positions de l'émetteur.
  Dans une position,la synchronisation est active.Dans l'autre position, le module
  ne fait rien et se comporte comme un cable 'Y' (bien que les directions et centres
  servos soient toujours controles par le module). Il s'agit d'un mode utile pour 
  comprendre comment votre avion va réagir alors que les moteurs sont contrôlés par le module.
  
MODE4: "MODE4: "MODE4: AUX CH is for Rudder Steering(!!non utilisable pour l'instant!!)
  Dans ce mode, le module se comporte comme dans le mode1 (canal aux. non connecté) avant les 1/3 moteur.
  Au delà des 1/3 moteur, le canal auxiliaire est supposé être relié à la sortie du récepteur de gouvernail"
  Il y a une zone morte autour du centre (ainsi, le trim du gouvernail est sans effet sur les moteurs).
  Quand le manche du gouvernail dépasse cette zone morte, il augmente la vitesse d'un moteur.
  Bouger le manche du gouvernail dans l'autre sens augmente la vitesse de l'autre moteur.
  Le gouvernail en position maxi d'un côté, se traduira par environ la moitié des gaz sur le moteur pour ce même côté
  Cela permet, lors du roulage des avions, le contrôle aux moteurs plutôt que d'une roue orientable .
  Ce mode authorize aussi des manoeuvres accrobatiques impossibles avec un seul moteur.
  Ce mode est déactivé si un des moteurs est à l'arrêt.
  Il est actif si les deux moteurs ou aucun des moteurs fonctionnent.
  Cela permet les essais au banc ainsi que l'exploitation si les deux moteurs sont en marche"
  
MODE5: Canal auxilaire contrôle les bougies (FACTORY PRESET MODE)
  Dans ce mode, la canal auxilaire active ou pas le contrôle des bougies.
  Un inter 2 positions sera utilisé. Dans une position les bougies seront
  allimentées et pas dans l'autre.

MODE6: NO DEADSTICK DETECTION with AUX CH Controls Glow plugs
  This mode is the same as MODE5 except that the engines are not idled in the event of a
  deadstick. The engines are synchronized when the stick is above idle and both engines
  are running. If both engines are not running then the throttle servos are just moved to the
  transmitter stick position. This allows full throttle control of one running engine at all times.
  The reason for this mode is if you have a very stable twin engine plane that flies well on
  one engine you may not want to idle the running engine in the event of a dead stick on one engine.
  Plugs are controlled by the AUX Channel just like in Mode 5.
  In this mode the AUX CH turns the glow plug drivers on and off. A two position switch
  should be used and in one position glow plugs will be off and in the other position the
  glow plugs will be on.



