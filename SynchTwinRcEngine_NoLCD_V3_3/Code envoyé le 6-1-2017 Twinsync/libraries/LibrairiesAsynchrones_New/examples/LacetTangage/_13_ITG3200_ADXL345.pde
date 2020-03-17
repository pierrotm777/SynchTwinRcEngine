_13_ITG3200_ADXL345/                                                                                000755  001751  000144  00000000000 11472520505 011712  5                                                                                                    ustar 00                                                                000000  000000                                                                                                                                                                         _13_ITG3200_ADXL345/_13_ITG3200_ADXL345.pde                                                         000644  001751  000144  00000013521 11471512711 015043  0                                                                                                    ustar 00                                                                000000  000000                                                                                                                                                                         /**
 * 
*/

import processing.serial.*;

Serial myPort;  // Create object from Serial class

boolean firstSample = true;

float [] RwAcc = new float[3];         //projection of normalized gravitation force vector on x/y/z axis, as measured by accelerometer
float [] Gyro = new float[3];          //Gyro readings
float [] RwGyro = new float[3];        //Rw obtained from last estimated value and gyro movement
float [] Awz = new float[2];           //angles between projection of R on XZ/YZ plane and Z axis (deg)
float [] RwEst = new float[3];


int lastTime = 0;
int interval = 0;
float wGyro = 10.0;

int lf = 10; // 10 is '\n' in ASCII
byte[] inBuffer = new byte[100];

PFont font;
final int VIEW_SIZE_X = 600, VIEW_SIZE_Y = 600;


void setup() 
{
  size(VIEW_SIZE_X, VIEW_SIZE_Y, P3D);
  myPort = new Serial(this, "/dev/ttyUSB0", 9600);  
  
  // The font must be located in the sketch's "data" directory to load successfully
  font = loadFont("CourierNew36.vlw"); 

}


void readSensors() {
  if(myPort.available() > 0) {
    if (myPort.readBytesUntil(lf, inBuffer) > 0) {
      String inputString = new String(inBuffer);
      String [] inputStringArr = split(inputString, ',');
      
      // convert raw readings to G
      RwAcc[0] = float(inputStringArr[0]) / 256.0;
      RwAcc[1] = float(inputStringArr[1])/ 256.0;
      RwAcc[2] = float(inputStringArr[2])/ 256.0;
      
      // convert raw readings to degrees/sec
      Gyro[0] = float(inputStringArr[3]) / 14.375;
      Gyro[1] = float(inputStringArr[4]) / 14.375;
      Gyro[2] = float(inputStringArr[5]) / 14.375;
    }
  }
}


void normalize3DVec(float [] vector) {
  float R;
  R = sqrt(vector[0]*vector[0] + vector[1]*vector[1] + vector[2]*vector[2]);
  vector[0] /= R;
  vector[1] /= R;  
  vector[2] /= R;  
}


float squared(float x){
  return x*x;
}


void buildBoxShape() {
  //box(60, 10, 40);
  noStroke();
  beginShape(QUADS);
  
  //Z+ (to the drawing area)
  fill(#00ff00);
  vertex(-30, -5, 20);
  vertex(30, -5, 20);
  vertex(30, 5, 20);
  vertex(-30, 5, 20);
  
  //Z-
  fill(#0000ff);
  vertex(-30, -5, -20);
  vertex(30, -5, -20);
  vertex(30, 5, -20);
  vertex(-30, 5, -20);
  
  //X-
  fill(#ff0000);
  vertex(-30, -5, -20);
  vertex(-30, -5, 20);
  vertex(-30, 5, 20);
  vertex(-30, 5, -20);
  
  //X+
  fill(#ffff00);
  vertex(30, -5, -20);
  vertex(30, -5, 20);
  vertex(30, 5, 20);
  vertex(30, 5, -20);
  
  //Y-
  fill(#ff00ff);
  vertex(-30, -5, -20);
  vertex(30, -5, -20);
  vertex(30, -5, 20);
  vertex(-30, -5, 20);
  
  //Y+
  fill(#00ffff);
  vertex(-30, 5, -20);
  vertex(30, 5, -20);
  vertex(30, 5, 20);
  vertex(-30, 5, 20);
  
  endShape();
}


void drawCube() {  
  pushMatrix();
    translate(300, 450, 0);
    scale(4,4,4);
    
    rotateX(HALF_PI * -RwEst[0]);
    rotateZ(HALF_PI * RwEst[1]);
    
    buildBoxShape();
    
  popMatrix();
}


void getInclination() {
  int w = 0;
  float tmpf = 0.0;
  int currentTime, signRzGyro;
  
  
  readSensors();
  normalize3DVec(RwAcc);
  
  currentTime = millis();
  interval = currentTime - lastTime;
  lastTime = currentTime;
  
  if (firstSample || Float.isNaN(RwEst[0])) { // the NaN check is used to wait for good data from the Arduino
    for(w=0;w<=2;w++) {
      RwEst[w] = RwAcc[w];    //initialize with accelerometer readings
    }
  }
  else{
    //evaluate RwGyro vector
    if(abs(RwEst[2]) < 0.1) {
      //Rz is too small and because it is used as reference for computing Axz, Ayz it's error fluctuations will amplify leading to bad results
      //in this case skip the gyro data and just use previous estimate
      for(w=0;w<=2;w++) {
        RwGyro[w] = RwEst[w];
      }
    }
    else {
      //get angles between projection of R on ZX/ZY plane and Z axis, based on last RwEst
      for(w=0;w<=1;w++){
        tmpf = Gyro[w];                        //get current gyro rate in deg/s
        tmpf *= interval / 1000.0f;                     //get angle change in deg
        Awz[w] = atan2(RwEst[w],RwEst[2]) * 180 / PI;   //get angle and convert to degrees 
        Awz[w] += tmpf;             //get updated angle according to gyro movement
      }
      
      //estimate sign of RzGyro by looking in what qudrant the angle Axz is, 
      //RzGyro is pozitive if  Axz in range -90 ..90 => cos(Awz) >= 0
      signRzGyro = ( cos(Awz[0] * PI / 180) >=0 ) ? 1 : -1;
      
      //reverse calculation of RwGyro from Awz angles, for formulas deductions see  http://starlino.com/imu_guide.html
      for(w=0;w<=1;w++){
        RwGyro[0] = sin(Awz[0] * PI / 180);
        RwGyro[0] /= sqrt( 1 + squared(cos(Awz[0] * PI / 180)) * squared(tan(Awz[1] * PI / 180)) );
        RwGyro[1] = sin(Awz[1] * PI / 180);
        RwGyro[1] /= sqrt( 1 + squared(cos(Awz[1] * PI / 180)) * squared(tan(Awz[0] * PI / 180)) );        
      }
      RwGyro[2] = signRzGyro * sqrt(1 - squared(RwGyro[0]) - squared(RwGyro[1]));
    }
    
    //combine Accelerometer and gyro readings
    for(w=0;w<=2;w++) RwEst[w] = (RwAcc[w] + wGyro * RwGyro[w]) / (1 + wGyro);

    normalize3DVec(RwEst);
  }
  
  firstSample = false;
}


void draw() {  
  getInclination();
  
  background(#000000);
  fill(#ffffff);
  
  textFont(font, 20);
  //float temp_decoded = 35.0 + ((float) (temp + 13200)) / 280;
  //text("temp:\n" + temp_decoded + " C", 350, 250);
  text("RwAcc (G):\n" + RwAcc[0] + "\n" + RwAcc[1] + "\n" + RwAcc[2] + "\ninterval: " + interval, 20, 50);
  text("Gyro (¬∞/s):\n" + Gyro[0] + "\n" + Gyro[1] + "\n" + Gyro[2], 220, 50);
  text("Awz (¬∞):\n" + Awz[0] + "\n" + Awz[1], 420, 50);
  text("RwGyro (¬∞/s):\n" + RwGyro[0] + "\n" + RwGyro[1] + "\n" + RwGyro[2], 20, 180);
  text("RwEst :\n" + RwEst[0] + "\n" + RwEst[1] + "\n" + RwEst[2], 220, 180);
  
  // display axes
  pushMatrix();
    translate(450, 250, 0);
    stroke(#ffffff);
    scale(100, 100, 100);
    line(0,0,0,1,0,0);
    line(0,0,0,0,-1,0);
    line(0,0,0,0,0,1);
    line(0,0,0, -RwEst[0], RwEst[1], RwEst[2]);
  popMatrix();
  
  drawCube();
}


                                                                                                                                                                               _13_ITG3200_ADXL345/LICENSE.txt                                                                     000644  001751  000144  00000104513 11472511745 013547  0                                                                                                    ustar 00                                                                000000  000000                                                                                                                                                                                             GNU GENERAL PUBLIC LICENSE
                       Version 3, 29 June 2007

 Copyright (C) 2007 Free Software Foundation, Inc. <http://fsf.org/>
 Everyone is permitted to copy and distribute verbatim copies
 of this license document, but changing it is not allowed.

                            Preamble

  The GNU General Public License is a free, copyleft license for
software and other kinds of works.

  The licenses for most software and other practical works are designed
to take away your freedom to share and change the works.  By contrast,
the GNU General Public License is intended to guarantee your freedom to
share and change all versions of a program--to make sure it remains free
software for all its users.  We, the Free Software Foundation, use the
GNU General Public License for most of our software; it applies also to
any other work released this way by its authors.  You can apply it to
your programs, too.

  When we speak of free software, we are referring to freedom, not
price.  Our General Public Licenses are designed to make sure that you
have the freedom to distribute copies of free software (and charge for
them if you wish), that you receive source code or can get it if you
want it, that you can change the software or use pieces of it in new
free programs, and that you know you can do these things.

  To protect your rights, we need to prevent others from denying you
these rights or asking you to surrender the rights.  Therefore, you have
certain responsibilities if you distribute copies of the software, or if
you modify it: responsibilities to respect the freedom of others.

  For example, if you distribute copies of such a program, whether
gratis or for a fee, you must pass on to the recipients the same
freedoms that you received.  You must make sure that they, too, receive
or can get the source code.  And you must show them these terms so they
know their rights.

  Developers that use the GNU GPL protect your rights with two steps:
(1) assert copyright on the software, and (2) offer you this License
giving you legal permission to copy, distribute and/or modify it.

  For the developers' and authors' protection, the GPL clearly explains
that there is no warranty for this free software.  For both users' and
authors' sake, the GPL requires that modified versions be marked as
changed, so that their problems will not be attributed erroneously to
authors of previous versions.

  Some devices are designed to deny users access to install or run
modified versions of the software inside them, although the manufacturer
can do so.  This is fundamentally incompatible with the aim of
protecting users' freedom to change the software.  The systematic
pattern of such abuse occurs in the area of products for individuals to
use, which is precisely where it is most unacceptable.  Therefore, we
have designed this version of the GPL to prohibit the practice for those
products.  If such problems arise substantially in other domains, we
stand ready to extend this provision to those domains in future versions
of the GPL, as needed to protect the freedom of users.

  Finally, every program is threatened constantly by software patents.
States should not allow patents to restrict development and use of
software on general-purpose computers, but in those that do, we wish to
avoid the special danger that patents applied to a free program could
make it effectively proprietary.  To prevent this, the GPL assures that
patents cannot be used to render the program non-free.

  The precise terms and conditions for copying, distribution and
modification follow.

                       TERMS AND CONDITIONS

  0. Definitions.

  "This License" refers to version 3 of the GNU General Public License.

  "Copyright" also means copyright-like laws that apply to other kinds of
works, such as semiconductor masks.

  "The Program" refers to any copyrightable work licensed under this
License.  Each licensee is addressed as "you".  "Licensees" and
"recipients" may be individuals or organizations.

  To "modify" a work means to copy from or adapt all or part of the work
in a fashion requiring copyright permission, other than the making of an
exact copy.  The resulting work is called a "modified version" of the
earlier work or a work "based on" the earlier work.

  A "covered work" means either the unmodified Program or a work based
on the Program.

  To "propagate" a work means to do anything with it that, without
permission, would make you directly or secondarily liable for
infringement under applicable copyright law, except executing it on a
computer or modifying a private copy.  Propagation includes copying,
distribution (with or without modification), making available to the
public, and in some countries other activities as well.

  To "convey" a work means any kind of propagation that enables other
parties to make or receive copies.  Mere interaction with a user through
a computer network, with no transfer of a copy, is not conveying.

  An interactive user interface displays "Appropriate Legal Notices"
to the extent that it includes a convenient and prominently visible
feature that (1) displays an appropriate copyright notice, and (2)
tells the user that there is no warranty for the work (except to the
extent that warranties are provided), that licensees may convey the
work under this License, and how to view a copy of this License.  If
the interface presents a list of user commands or options, such as a
menu, a prominent item in the list meets this criterion.

  1. Source Code.

  The "source code" for a work means the preferred form of the work
for making modifications to it.  "Object code" means any non-source
form of a work.

  A "Standard Interface" means an interface that either is an official
standard defined by a recognized standards body, or, in the case of
interfaces specified for a particular programming language, one that
is widely used among developers working in that language.

  The "System Libraries" of an executable work include anything, other
than the work as a whole, that (a) is included in the normal form of
packaging a Major Component, but which is not part of that Major
Component, and (b) serves only to enable use of the work with that
Major Component, or to implement a Standard Interface for which an
implementation is available to the public in source code form.  A
"Major Component", in this context, means a major essential component
(kernel, window system, and so on) of the specific operating system
(if any) on which the executable work runs, or a compiler used to
produce the work, or an object code interpreter used to run it.

  The "Corresponding Source" for a work in object code form means all
the source code needed to generate, install, and (for an executable
work) run the object code and to modify the work, including scripts to
control those activities.  However, it does not include the work's
System Libraries, or general-purpose tools or generally available free
programs which are used unmodified in performing those activities but
which are not part of the work.  For example, Corresponding Source
includes interface definition files associated with source files for
the work, and the source code for shared libraries and dynamically
linked subprograms that the work is specifically designed to require,
such as by intimate data communication or control flow between those
subprograms and other parts of the work.

  The Corresponding Source need not include anything that users
can regenerate automatically from other parts of the Corresponding
Source.

  The Corresponding Source for a work in source code form is that
same work.

  2. Basic Permissions.

  All rights granted under this License are granted for the term of
copyright on the Program, and are irrevocable provided the stated
conditions are met.  This License explicitly affirms your unlimited
permission to run the unmodified Program.  The output from running a
covered work is covered by this License only if the output, given its
content, constitutes a covered work.  This License acknowledges your
rights of fair use or other equivalent, as provided by copyright law.

  You may make, run and propagate covered works that you do not
convey, without conditions so long as your license otherwise remains
in force.  You may convey covered works to others for the sole purpose
of having them make modifications exclusively for you, or provide you
with facilities for running those works, provided that you comply with
the terms of this License in conveying all material for which you do
not control copyright.  Those thus making or running the covered works
for you must do so exclusively on your behalf, under your direction
and control, on terms that prohibit them from making any copies of
your copyrighted material outside their relationship with you.

  Conveying under any other circumstances is permitted solely under
the conditions stated below.  Sublicensing is not allowed; section 10
makes it unnecessary.

  3. Protecting Users' Legal Rights From Anti-Circumvention Law.

  No covered work shall be deemed part of an effective technological
measure under any applicable law fulfilling obligations under article
11 of the WIPO copyright treaty adopted on 20 December 1996, or
similar laws prohibiting or restricting circumvention of such
measures.

  When you convey a covered work, you waive any legal power to forbid
circumvention of technological measures to the extent such circumvention
is effected by exercising rights under this License with respect to
the covered work, and you disclaim any intention to limit operation or
modification of the work as a means of enforcing, against the work's
users, your or third parties' legal rights to forbid circumvention of
technological measures.

  4. Conveying Verbatim Copies.

  You may convey verbatim copies of the Program's source code as you
receive it, in any medium, provided that you conspicuously and
appropriately publish on each copy an appropriate copyright notice;
keep intact all notices stating that this License and any
non-permissive terms added in accord with section 7 apply to the code;
keep intact all notices of the absence of any warranty; and give all
recipients a copy of this License along with the Program.

  You may charge any price or no price for each copy that you convey,
and you may offer support or warranty protection for a fee.

  5. Conveying Modified Source Versions.

  You may convey a work based on the Program, or the modifications to
produce it from the Program, in the form of source code under the
terms of section 4, provided that you also meet all of these conditions:

    a) The work must carry prominent notices stating that you modified
    it, and giving a relevant date.

    b) The work must carry prominent notices stating that it is
    released under this License and any conditions added under section
    7.  This requirement modifies the requirement in section 4 to
    "keep intact all notices".

    c) You must license the entire work, as a whole, under this
    License to anyone who comes into possession of a copy.  This
    License will therefore apply, along with any applicable section 7
    additional terms, to the whole of the work, and all its parts,
    regardless of how they are packaged.  This License gives no
    permission to license the work in any other way, but it does not
    invalidate such permission if you have separately received it.

    d) If the work has interactive user interfaces, each must display
    Appropriate Legal Notices; however, if the Program has interactive
    interfaces that do not display Appropriate Legal Notices, your
    work need not make them do so.

  A compilation of a covered work with other separate and independent
works, which are not by their nature extensions of the covered work,
and which are not combined with it such as to form a larger program,
in or on a volume of a storage or distribution medium, is called an
"aggregate" if the compilation and its resulting copyright are not
used to limit the access or legal rights of the compilation's users
beyond what the individual works permit.  Inclusion of a covered work
in an aggregate does not cause this License to apply to the other
parts of the aggregate.

  6. Conveying Non-Source Forms.

  You may convey a covered work in object code form under the terms
of sections 4 and 5, provided that you also convey the
machine-readable Corresponding Source under the terms of this License,
in one of these ways:

    a) Convey the object code in, or embodied in, a physical product
    (including a physical distribution medium), accompanied by the
    Corresponding Source fixed on a durable physical medium
    customarily used for software interchange.

    b) Convey the object code in, or embodied in, a physical product
    (including a physical distribution medium), accompanied by a
    written offer, valid for at least three years and valid for as
    long as you offer spare parts or customer support for that product
    model, to give anyone who possesses the object code either (1) a
    copy of the Corresponding Source for all the software in the
    product that is covered by this License, on a durable physical
    medium customarily used for software interchange, for a price no
    more than your reasonable cost of physically performing this
    conveying of source, or (2) access to copy the
    Corresponding Source from a network server at no charge.

    c) Convey individual copies of the object code with a copy of the
    written offer to provide the Corresponding Source.  This
    alternative is allowed only occasionally and noncommercially, and
    only if you received the object code with such an offer, in accord
    with subsection 6b.

    d) Convey the object code by offering access from a designated
    place (gratis or for a charge), and offer equivalent access to the
    Corresponding Source in the same way through the same place at no
    further charge.  You need not require recipients to copy the
    Corresponding Source along with the object code.  If the place to
    copy the object code is a network server, the Corresponding Source
    may be on a different server (operated by you or a third party)
    that supports equivalent copying facilities, provided you maintain
    clear directions next to the object code saying where to find the
    Corresponding Source.  Regardless of what server hosts the
    Corresponding Source, you remain obligated to ensure that it is
    available for as long as needed to satisfy these requirements.

    e) Convey the object code using peer-to-peer transmission, provided
    you inform other peers where the object code and Corresponding
    Source of the work are being offered to the general public at no
    charge under subsection 6d.

  A separable portion of the object code, whose source code is excluded
from the Corresponding Source as a System Library, need not be
included in conveying the object code work.

  A "User Product" is either (1) a "consumer product", which means any
tangible personal property which is normally used for personal, family,
or household purposes, or (2) anything designed or sold for incorporation
into a dwelling.  In determining whether a product is a consumer product,
doubtful cases shall be resolved in favor of coverage.  For a particular
product received by a particular user, "normally used" refers to a
typical or common use of that class of product, regardless of the status
of the particular user or of the way in which the particular user
actually uses, or expects or is expected to use, the product.  A product
is a consumer product regardless of whether the product has substantial
commercial, industrial or non-consumer uses, unless such uses represent
the only significant mode of use of the product.

  "Installation Information" for a User Product means any methods,
procedures, authorization keys, or other information required to install
and execute modified versions of a covered work in that User Product from
a modified version of its Corresponding Source.  The information must
suffice to ensure that the continued functioning of the modified object
code is in no case prevented or interfered with solely because
modification has been made.

  If you convey an object code work under this section in, or with, or
specifically for use in, a User Product, and the conveying occurs as
part of a transaction in which the right of possession and use of the
User Product is transferred to the recipient in perpetuity or for a
fixed term (regardless of how the transaction is characterized), the
Corresponding Source conveyed under this section must be accompanied
by the Installation Information.  But this requirement does not apply
if neither you nor any third party retains the ability to install
modified object code on the User Product (for example, the work has
been installed in ROM).

  The requirement to provide Installation Information does not include a
requirement to continue to provide support service, warranty, or updates
for a work that has been modified or installed by the recipient, or for
the User Product in which it has been modified or installed.  Access to a
network may be denied when the modification itself materially and
adversely affects the operation of the network or violates the rules and
protocols for communication across the network.

  Corresponding Source conveyed, and Installation Information provided,
in accord with this section must be in a format that is publicly
documented (and with an implementation available to the public in
source code form), and must require no special password or key for
unpacking, reading or copying.

  7. Additional Terms.

  "Additional permissions" are terms that supplement the terms of this
License by making exceptions from one or more of its conditions.
Additional permissions that are applicable to the entire Program shall
be treated as though they were included in this License, to the extent
that they are valid under applicable law.  If additional permissions
apply only to part of the Program, that part may be used separately
under those permissions, but the entire Program remains governed by
this License without regard to the additional permissions.

  When you convey a copy of a covered work, you may at your option
remove any additional permissions from that copy, or from any part of
it.  (Additional permissions may be written to require their own
removal in certain cases when you modify the work.)  You may place
additional permissions on material, added by you to a covered work,
for which you have or can give appropriate copyright permission.

  Notwithstanding any other provision of this License, for material you
add to a covered work, you may (if authorized by the copyright holders of
that material) supplement the terms of this License with terms:

    a) Disclaiming warranty or limiting liability differently from the
    terms of sections 15 and 16 of this License; or

    b) Requiring preservation of specified reasonable legal notices or
    author attributions in that material or in the Appropriate Legal
    Notices displayed by works containing it; or

    c) Prohibiting misrepresentation of the origin of that material, or
    requiring that modified versions of such material be marked in
    reasonable ways as different from the original version; or

    d) Limiting the use for publicity purposes of names of licensors or
    authors of the material; or

    e) Declining to grant rights under trademark law for use of some
    trade names, trademarks, or service marks; or

    f) Requiring indemnification of licensors and authors of that
    material by anyone who conveys the material (or modified versions of
    it) with contractual assumptions of liability to the recipient, for
    any liability that these contractual assumptions directly impose on
    those licensors and authors.

  All other non-permissive additional terms are considered "further
restrictions" within the meaning of section 10.  If the Program as you
received it, or any part of it, contains a notice stating that it is
governed by this License along with a term that is a further
restriction, you may remove that term.  If a license document contains
a further restriction but permits relicensing or conveying under this
License, you may add to a covered work material governed by the terms
of that license document, provided that the further restriction does
not survive such relicensing or conveying.

  If you add terms to a covered work in accord with this section, you
must place, in the relevant source files, a statement of the
additional terms that apply to those files, or a notice indicating
where to find the applicable terms.

  Additional terms, permissive or non-permissive, may be stated in the
form of a separately written license, or stated as exceptions;
the above requirements apply either way.

  8. Termination.

  You may not propagate or modify a covered work except as expressly
provided under this License.  Any attempt otherwise to propagate or
modify it is void, and will automatically terminate your rights under
this License (including any patent licenses granted under the third
paragraph of section 11).

  However, if you cease all violation of this License, then your
license from a particular copyright holder is reinstated (a)
provisionally, unless and until the copyright holder explicitly and
finally terminates your license, and (b) permanently, if the copyright
holder fails to notify you of the violation by some reasonable means
prior to 60 days after the cessation.

  Moreover, your license from a particular copyright holder is
reinstated permanently if the copyright holder notifies you of the
violation by some reasonable means, this is the first time you have
received notice of violation of this License (for any work) from that
copyright holder, and you cure the violation prior to 30 days after
your receipt of the notice.

  Termination of your rights under this section does not terminate the
licenses of parties who have received copies or rights from you under
this License.  If your rights have been terminated and not permanently
reinstated, you do not qualify to receive new licenses for the same
material under section 10.

  9. Acceptance Not Required for Having Copies.

  You are not required to accept this License in order to receive or
run a copy of the Program.  Ancillary propagation of a covered work
occurring solely as a consequence of using peer-to-peer transmission
to receive a copy likewise does not require acceptance.  However,
nothing other than this License grants you permission to propagate or
modify any covered work.  These actions infringe copyright if you do
not accept this License.  Therefore, by modifying or propagating a
covered work, you indicate your acceptance of this License to do so.

  10. Automatic Licensing of Downstream Recipients.

  Each time you convey a covered work, the recipient automatically
receives a license from the original licensors, to run, modify and
propagate that work, subject to this License.  You are not responsible
for enforcing compliance by third parties with this License.

  An "entity transaction" is a transaction transferring control of an
organization, or substantially all assets of one, or subdividing an
organization, or merging organizations.  If propagation of a covered
work results from an entity transaction, each party to that
transaction who receives a copy of the work also receives whatever
licenses to the work the party's predecessor in interest had or could
give under the previous paragraph, plus a right to possession of the
Corresponding Source of the work from the predecessor in interest, if
the predecessor has it or can get it with reasonable efforts.

  You may not impose any further restrictions on the exercise of the
rights granted or affirmed under this License.  For example, you may
not impose a license fee, royalty, or other charge for exercise of
rights granted under this License, and you may not initiate litigation
(including a cross-claim or counterclaim in a lawsuit) alleging that
any patent claim is infringed by making, using, selling, offering for
sale, or importing the Program or any portion of it.

  11. Patents.

  A "contributor" is a copyright holder who authorizes use under this
License of the Program or a work on which the Program is based.  The
work thus licensed is called the contributor's "contributor version".

  A contributor's "essential patent claims" are all patent claims
owned or controlled by the contributor, whether already acquired or
hereafter acquired, that would be infringed by some manner, permitted
by this License, of making, using, or selling its contributor version,
but do not include claims that would be infringed only as a
consequence of further modification of the contributor version.  For
purposes of this definition, "control" includes the right to grant
patent sublicenses in a manner consistent with the requirements of
this License.

  Each contributor grants you a non-exclusive, worldwide, royalty-free
patent license under the contributor's essential patent claims, to
make, use, sell, offer for sale, import and otherwise run, modify and
propagate the contents of its contributor version.

  In the following three paragraphs, a "patent license" is any express
agreement or commitment, however denominated, not to enforce a patent
(such as an express permission to practice a patent or covenant not to
sue for patent infringement).  To "grant" such a patent license to a
party means to make such an agreement or commitment not to enforce a
patent against the party.

  If you convey a covered work, knowingly relying on a patent license,
and the Corresponding Source of the work is not available for anyone
to copy, free of charge and under the terms of this License, through a
publicly available network server or other readily accessible means,
then you must either (1) cause the Corresponding Source to be so
available, or (2) arrange to deprive yourself of the benefit of the
patent license for this particular work, or (3) arrange, in a manner
consistent with the requirements of this License, to extend the patent
license to downstream recipients.  "Knowingly relying" means you have
actual knowledge that, but for the patent license, your conveying the
covered work in a country, or your recipient's use of the covered work
in a country, would infringe one or more identifiable patents in that
country that you have reason to believe are valid.

  If, pursuant to or in connection with a single transaction or
arrangement, you convey, or propagate by procuring conveyance of, a
covered work, and grant a patent license to some of the parties
receiving the covered work authorizing them to use, propagate, modify
or convey a specific copy of the covered work, then the patent license
you grant is automatically extended to all recipients of the covered
work and works based on it.

  A patent license is "discriminatory" if it does not include within
the scope of its coverage, prohibits the exercise of, or is
conditioned on the non-exercise of one or more of the rights that are
specifically granted under this License.  You may not convey a covered
work if you are a party to an arrangement with a third party that is
in the business of distributing software, under which you make payment
to the third party based on the extent of your activity of conveying
the work, and under which the third party grants, to any of the
parties who would receive the covered work from you, a discriminatory
patent license (a) in connection with copies of the covered work
conveyed by you (or copies made from those copies), or (b) primarily
for and in connection with specific products or compilations that
contain the covered work, unless you entered into that arrangement,
or that patent license was granted, prior to 28 March 2007.

  Nothing in this License shall be construed as excluding or limiting
any implied license or other defenses to infringement that may
otherwise be available to you under applicable patent law.

  12. No Surrender of Others' Freedom.

  If conditions are imposed on you (whether by court order, agreement or
otherwise) that contradict the conditions of this License, they do not
excuse you from the conditions of this License.  If you cannot convey a
covered work so as to satisfy simultaneously your obligations under this
License and any other pertinent obligations, then as a consequence you may
not convey it at all.  For example, if you agree to terms that obligate you
to collect a royalty for further conveying from those to whom you convey
the Program, the only way you could satisfy both those terms and this
License would be to refrain entirely from conveying the Program.

  13. Use with the GNU Affero General Public License.

  Notwithstanding any other provision of this License, you have
permission to link or combine any covered work with a work licensed
under version 3 of the GNU Affero General Public License into a single
combined work, and to convey the resulting work.  The terms of this
License will continue to apply to the part which is the covered work,
but the special requirements of the GNU Affero General Public License,
section 13, concerning interaction through a network will apply to the
combination as such.

  14. Revised Versions of this License.

  The Free Software Foundation may publish revised and/or new versions of
the GNU General Public License from time to time.  Such new versions will
be similar in spirit to the present version, but may differ in detail to
address new problems or concerns.

  Each version is given a distinguishing version number.  If the
Program specifies that a certain numbered version of the GNU General
Public License "or any later version" applies to it, you have the
option of following the terms and conditions either of that numbered
version or of any later version published by the Free Software
Foundation.  If the Program does not specify a version number of the
GNU General Public License, you may choose any version ever published
by the Free Software Foundation.

  If the Program specifies that a proxy can decide which future
versions of the GNU General Public License can be used, that proxy's
public statement of acceptance of a version permanently authorizes you
to choose that version for the Program.

  Later license versions may give you additional or different
permissions.  However, no additional obligations are imposed on any
author or copyright holder as a result of your choosing to follow a
later version.

  15. Disclaimer of Warranty.

  THERE IS NO WARRANTY FOR THE PROGRAM, TO THE EXTENT PERMITTED BY
APPLICABLE LAW.  EXCEPT WHEN OTHERWISE STATED IN WRITING THE COPYRIGHT
HOLDERS AND/OR OTHER PARTIES PROVIDE THE PROGRAM "AS IS" WITHOUT WARRANTY
OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING, BUT NOT LIMITED TO,
THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR
PURPOSE.  THE ENTIRE RISK AS TO THE QUALITY AND PERFORMANCE OF THE PROGRAM
IS WITH YOU.  SHOULD THE PROGRAM PROVE DEFECTIVE, YOU ASSUME THE COST OF
ALL NECESSARY SERVICING, REPAIR OR CORRECTION.

  16. Limitation of Liability.

  IN NO EVENT UNLESS REQUIRED BY APPLICABLE LAW OR AGREED TO IN WRITING
WILL ANY COPYRIGHT HOLDER, OR ANY OTHER PARTY WHO MODIFIES AND/OR CONVEYS
THE PROGRAM AS PERMITTED ABOVE, BE LIABLE TO YOU FOR DAMAGES, INCLUDING ANY
GENERAL, SPECIAL, INCIDENTAL OR CONSEQUENTIAL DAMAGES ARISING OUT OF THE
USE OR INABILITY TO USE THE PROGRAM (INCLUDING BUT NOT LIMITED TO LOSS OF
DATA OR DATA BEING RENDERED INACCURATE OR LOSSES SUSTAINED BY YOU OR THIRD
PARTIES OR A FAILURE OF THE PROGRAM TO OPERATE WITH ANY OTHER PROGRAMS),
EVEN IF SUCH HOLDER OR OTHER PARTY HAS BEEN ADVISED OF THE POSSIBILITY OF
SUCH DAMAGES.

  17. Interpretation of Sections 15 and 16.

  If the disclaimer of warranty and limitation of liability provided
above cannot be given local legal effect according to their terms,
reviewing courts shall apply local law that most closely approximates
an absolute waiver of all civil liability in connection with the
Program, unless a warranty or assumption of liability accompanies a
copy of the Program in return for a fee.

                     END OF TERMS AND CONDITIONS

            How to Apply These Terms to Your New Programs

  If you develop a new program, and you want it to be of the greatest
possible use to the public, the best way to achieve this is to make it
free software which everyone can redistribute and change under these terms.

  To do so, attach the following notices to the program.  It is safest
to attach them to the start of each source file to most effectively
state the exclusion of warranty; and each file should have at least
the "copyright" line and a pointer to where the full notice is found.

    <one line to give the program's name and a brief idea of what it does.>
    Copyright (C) <year>  <name of author>

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

Also add information on how to contact you by electronic and paper mail.

  If the program does terminal interaction, make it output a short
notice like this when it starts in an interactive mode:

    <program>  Copyright (C) <year>  <name of author>
    This program comes with ABSOLUTELY NO WARRANTY; for details type `show w'.
    This is free software, and you are welcome to redistribute it
    under certain conditions; type `show c' for details.

The hypothetical commands `show w' and `show c' should show the appropriate
parts of the General Public License.  Of course, your program's commands
might be different; for a GUI interface, you would use an "about box".

  You should also get your employer (if you work as a programmer) or school,
if any, to sign a "copyright disclaimer" for the program, if necessary.
For more information on this, and how to apply and follow the GNU GPL, see
<http://www.gnu.org/licenses/>.

  The GNU General Public License does not permit incorporating your program
into proprietary programs.  If your program is a subroutine library, you
may consider it more useful to permit linking proprietary applications with
the library.  If this is what you want to do, use the GNU Lesser General
Public License instead of this License.  But first, please read
<http://www.gnu.org/philosophy/why-not-lgpl.html>.
                                                                                                                                                                                     _13_ITG3200_ADXL345/data/                                                                           000755  001751  000144  00000000000 11466737620 012636  5                                                                                                    ustar 00                                                                000000  000000                                                                                                                                                                         _13_ITG3200_ADXL345/data/CourierNew36.vlw                                                           000644  001751  000144  00000340350 11406765374 015631  0                                                                                                    ustar 00                                                                000000  000000                                                                                                                                                                                 $   @         !                      "      	                #                      $                      %                      &                      '                      (               
       )                      *                      +                      ,                      -                      .                      /                      0                      1                      2                      3                      4                      5                      6                      7                      8                      9                      :                      ;      	                <                       =   	                    >                       ?                      @                      A            €€€€       B                       C                      D                       E                      F                      G                      H                      I                      J                      K                       L                      M            €€€€       N            €€€€       O                      P                      Q                      R                       S                      T                      U                      V            €€€ю       W            €€€э       X                      Y                       Z                      [               	       \                      ]                      ^                      _         €€€ш€€€ю       `                      a                      b                       c                      d                      e                      f                      g                      h                      i                      j                      k                      l                      m                       n                      o                      p                       q                      r                      s                      t                      u                      v            €€€€       w                       x                      y            €€€€       z                      {      
                |               	       }      
                ~                      †            H€€€№       °                      Ґ                      £                      §                      •                       ¶               	       І                      ®      	                ©                       ™                      Ђ                      ђ            €€€€       ≠                      Ѓ                       ѓ            €€€€       ∞   
   
                ±                      і                      µ                      ґ                      Ј                      Є                      Ї                      ї                      њ                      ј            €€€€       Ѕ            €€€€       ¬            €€€€       √            €€€€       ƒ            €€€€       ≈            €€€€       ∆            €€€€       «                      »                      …                                             Ћ                      ћ                      Ќ                      ќ                      ѕ                      —            €€€€       “                      ”                      ‘                      ’                      ÷                      „                      Ў                      ў                      Џ                      џ                      №                      Ё                       я                      а                      б                      в                      г                      д                      е                      ж            €€€€       з                      и                      й                      к                      л                      м                      н                      о                      п                      с                      т                      у                      ф                      х                      ц                      ч                      ш                      щ                      ъ                      ы                      ь                      э             €€€€       €            €€€€                  €€€€                                       €€€€                                                                                                                                                                      €€€€                                                                                                               1                     9                     :                     =                     >                     A                     B                     C            €€€€      D                     G            €€€€      H                     P                     Q                     R            €€€€      S            €€€€      T                      U                     X                      Y                     Z                     [                     ^                     _                     `                     a                     b                     c                     d                     e                     n                     o                     p                     q                     x                      y                     z                     {                     |                     }                     ~                     Т                     ∆                     «                     Ў                     ў                     Џ                     џ                     №                     Ё                     ©                     ј                                                        €€€€                      	                                                                                                                                            !                      "                      &                      0                       9      
                :      
                D                      ђ                      !"            €€€ю      "                     "                     "                     "                     "   "         !         "   	                  "+   %                   "H                     "`                     "d            €€€€      "e                     %                      ы                      ы                    ЭшфЦЯ€€€€°ш€€€€шъ€€€€цз€€€€Џґ€€€€≤™€€€€£Ы€€€€Цp€€€€_W€€€€RL€€€€F*€€€€€€€€ ъ€€ф  Ў€€ћ  LллJ                    rффw  ъ€€ъ  pшцn ц€ф   ц€ф‘€Ў   ‘€Ў≤€∞   ≤€∞£€°   £€°О€Р   О€Р_€]   _€]P€P   P€PD€D   D€D€   €ь    ь  љ     њ      nш{   lшГ        р€ъ   з€ь       €€ъ  €€ъ       €€о  €€о       ;€€√  ;€€√       L€€Ѓ  L€€Ѓ       Y€€°  Y€€°       h€€Р  h€€Р    nйъь€€цттъ€€шъзh ъ€€€€€€€€€€€€€€ъ lз€€€€€€€€€€€€зl    ≈€€;  ≈€€7       р€€  р€€       ъ€ь   ъ€ь    lзьш€€€фшш€€€ърr ъ€€€€€€€€€€€€€€ъ hзь€€€€€€€€€€€тr    P€€™ P€€І       f€€Р  c€€О       Ы€€]  Ы€€]       ™€€L  ™€€L       √€€*  љ€€(       т€€  т€€       ш€ц   ш€е        yъp   pъj            wъw              ц€ф              €€€           0Эз€€€йЫ7      Э€€€€€€€€€€рY   ґ€€€€€€€€€€€€з  c€€ъБ* hе€€€ь  ћ€€R      ъ€€ь  ъ€€        ≈€€е  ц€€W       7глR  ≤€€€і[$          7€€€€€€фЃБF      f€€€€€€€€€тО     9≈€€€€€€€€€о?      (p™я€€€€€€т"         H{ћ€€€Ы             Г€€йaцћ         €€ъз€€r         €€й€€€Ў       І€€Щ€€€€лЕF &lЎ€€т€€€€€€€€€€€€€€цH л€€€€€€€€€€€€“3  hшК&rђо€€€е£F           €€€              €€€              €€€              ц€ф              wшw          NЅццЅN          Г€€€€€€И        L€цhfц€N       њ€j    f€Ѕ       ш€    €ц       ш€    €ц       њ€j    h€Ѕ       N€ъjjш€N        И€€€€€€И  [Ілњ   NњшшњPLТ‘€€€€њ     7wљъ€€€фђf$  aІр€€€ьЅ=    ї€€€€ЏЦPYЅццЅN   њтІc  Г€€€€€€И        L€цhfц€N       њ€j    f€њ       ш€    €ц       ш€    €ц       њ€h    h€Ѕ       N€цjhц€P        И€€€€€€И          LњшшњP         YљцшЅL         ґ€€€€€€ьЃ       Є€€€€€€€€«      a€€…" ;“зl      њ€€*   
        ш€€              ъ€€             “€€             М€€{             (€€€{           9“€€€€w         [ь€€€€€€p €€ьйh0ь€€…WО€€€nA€€€€ъ∞€€Ф   •€€€њ€€€оrш€€   Є€€€€€Ф  ш€€    ≈€€€€=  ≤€€‘U  h€€€≈   ,т€€€€€€€€€€€€ълl 5Ё€€€€€€€€€о€€€ъ  [ІЁц€шЁЭ.(й€шЕц€ф‘€Ў≤€∞£€°М€Р_€]P€PD€D€ь  Ѕ     "‘цf   “€€ц   {€€€њ  ф€€ь*  Ф€€€О  ь€€ц  И€€€О   з€€€&  =€€€…   }€€€Б   Ѓ€€€L   б€€€   ъ€€€    €€€€    ъ€€€    б€€€   Ѓ€€€L   Г€€€}   A€€€«   л€€€&   К€€€М   ъ€€ц   О€€€Р   т€€ь*   y€€€њ   “€€ц    ‘тccф‘    ц€€“   љ€€€   (ь€€ш   К€€€Ф   ф€€ь   К€€€К   €€€й    ≈€€€?   }€€€   J€€€Ѓ   €€€г   €€€ц    €€€ъ   €€€ц   €€€е   J€€€Ѓ   {€€€Г   √€€€A  €€€л  К€€€М  т€€ь  К€€€Ц  (ь€€ц  љ€€€}   ц€€‘   cт–"          jшn            е€з            €€ь            €€€            €€€      ]ц“Б( €€€ (“т]т€€€€“€€€–€€€€тЎ€€€€€€€€€€€€€Џ&Ѓъ€€€€€€€€€ьЃ$  _њ€€€€€Ѕc      •€€€€€ђ       n€€€€€€€n     3ъ€€шUш€€ь0    ќ€€€p n€€€–    р€€љ   љ€€т    Uтќ   ќф]         БъГ              ш€ц              €€€              €€€              €€€              €€€              €€€       {ф€€€€€€€€€€€€€фwъ€€€€€€€€€€€€€€€ъwц€€€€€€€€€€€€€цw       €€€              €€€              €€€              €€€              €€€              ш€ц              ъГ         $€€€€∞  r€€€ш  њ€€€  ь€€я  ]€€€N   ∞€€і   ъ€ъ$   P€€Г    Э€е
    з€Y     Ѕґ      €€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€*ќъќ(…€€€ћъ€€€ъћ€€€ќ,ќъќ(             ,й}             ї€ц            .€€«            ґ€€J           9€€«            «€€;           J€€Є            “€€.           Y€€™           Ё€ь$           j€€Щ           е€ъ           y€€И           о€ф           Б€€}           ф€л           О€€r           ъ€г           Э€€f           &€€Џ            Ѓ€€U           0€€–            љ€€F           A€€Ѕ            …€€7           P€€≤            …€€*            ц€і             }й*                 0ЫбъъбЫ.       Е€€€€€€€€{     И€€€€€€€€€€t   ?€€€і73™€€€0  ‘€€Я      Я€€ћ A€€я      е€€?Э€€n        w€€Э÷€€&        *€€÷ш€€        €€ц€€€          €€€€€€          €€€€€€          €€€€€€          €€€ш€€        €€цЎ€€(        (€€‘Э€€r        n€€Я?€€г
      Ё€€F ќ€€Я      Э€€“  7€€€ђ57і€€€A   }€€€€€€€€€€М     И€€€€€€€€И       0°яьъЁЫ.         $[°я       *fІй€€€€      ™€€€€€€€€      ц€€€€€€€€      nшЁ•a0€€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€      Ы€€€€€€€€€€€€€Ы€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€Ц     ?Ябшъзђ]      &«€€€€€€€€Ў3    3о€€€€€€€€€€т;  г€€цБ* aЁ€€й €€о0      Ѕ€€} г€€c        &€€– ш€ф        €€ц nъr         P€€ш            з€€Ѕ           *е€€€N          Aо€€€К          cь€€€w         Ф€€€з?         Ѕ€€€™         9з€€еL          hь€€Т         І€€‘0          &‘€тr          Yф€∞         £зH€€€€€€€€€€€€€€€€“€€€€€€€€€€€€€€€€ъ€€€€€€€€€€€€€€€€€   NЭ«тшътґw      jе€€€€€€€€€шp    Ф€€€€€€€€€€€€€М   ш€€€–a" A•€€€L  fлт{        Е€€љ              €€ц              €€ш              Ы€€њ            7∞€€€F       cз€€€€€€€y        ш€€€€€€€y         hз€€€€€€€М          ;rЎ€€€≤              {€€€f              Е€€ќ              €€ъ              €€т              Щ€€∞jорЦJ    
Dp“€€€3ъ€€€€€€€€€€€€€€€l fф€€€€€€€€€€€€ЏJ   [°љрш€€шйіРD            {€€€€         ц€€€€         ∞€€€€€        J€€Ы€€€       Ё€з€€€       €€U €€€      "ш€і  €€€      Є€ф  €€€     W€€p   €€€    е€–    €€€    К€ь*    €€€   *ь€И     €€€   ≈€я     €€€  _€€F      €€€  й€€€€€€€€€€€€€Ы€€€€€€€€€€€€€€€€€€€€€€€€€€€€€Ц          €€€            €€€         Ы€€€€€€Ы       €€€€€€€€       Щ€€€€€€Ц  €€€€€€€€€€€ьоn   €€€€€€€€€€€€€ъ   €€€€€€€€€€€ьзj   €€€              €€€              €€€              €€€              €€€0Р÷ъшЁ°D      €€€€€€€€€€€Ѕ    €€€€€€€€€€€€е"   р€€€√_ _г€€“  ]р“?      з€€]             h€€≤             €€т              €€ш             €€ц             F€€≈aрЎ3        “€€}ц€€ъК=  .jЁ€€оЄ€€€€€€€€€€€€€ъF Яь€€€€€€€€€€÷7    rђяцьът≈ЫH           *Гњтъц–Г     *≤€€€€€€€€…    nъ€€€€€€€€€ф   Ф€€€ь•R 7–р[  Г€€€і         L€€€К          б€€Э           U€€г           І€€rlљфштђR    Џ€€pз€€€€€€€…  ц€€€€€€€€€€€€й" €€€€€шГ$ wф€€÷ъ€€€е.     0т€€hг€€р"       n€€їЃ€€_        €€тЕ€€         €€ъ9€€Г        $€€т Ў€ш3       І€€™ W€€тy, Nњ€€€=  £€€€€€€€€€€€Щ    Т€€€€€€€€€Г      .Рќцьц…М&   €€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€         .€€«Щ€Ы         Ц€€c           ш€р           p€€Р            Ў€€(           ?€€√            Ѓ€€_           ь€л           €€К           г€€&           W€€њ            Ѕ€€U           (€€з           Р€€Е           
ф€€           h€€Є            ќ€€N            ц€т            pъn          nђршшрђp     aр€€€€€€€€рc   Г€€€€€€€€€€€€Е J€€€«W  U«€€€Lњ€€Щ        Щ€€њш€€        €€цш€€        €€цњ€€Э        Я€€ЅU€€€ћY  Wћ€€€Y •€€€€€€€€€€€€•  о€€€€€€€€€€л
  І€€€€€€€€€€€€• A€€€шЕ**Гш€€€?™€€р3      3о€€Іо€€f        c€€оь€€        €€шф€€        €€тЃ€€Ф        Ф€€∞=€€€їP  
NЄ€€€A О€€€€€€€€€€€€Р   rф€€€€€€€€тn     p∞тььлђn      }«цьт∞f       Aй€€€€€€€Џ3     5ш€€€€€€€€€ц?   Ё€€лr Wќ€€о  h€€й     І€€∞  Є€€a       Ё€€3 ц€€        h€€Щ ь€€         €€з т€€        }€€€Ѓ€€t       ;ь€€€U€€т3     5л€€€€
 √€€фw Бш€€€€ь  Ё€€€€€€€€€€€€ф   ґ€€€€€€€зt€€≈     ?ІйъцЅt
М€€И            5ь€€(           3й€€°          wш€€я  [р–; 7}б€€€б   ц€€€€€€€€€€і    љ€€€€€€€€ЎU      }ќшьф≈ТA        (ќъќ*ћ€€€ќъ€€€ъћ€€€ќ&ќъќ(                              ,ќъќ(ћ€€€ћъ€€€ъћ€€€ќ,ќъќ(    $ќъќ(    …€€€ћ    ъ€€€ъ    ћ€€€ќ    &ќъќ(                                                        €€€€™   ]€€€т   І€€€l    й€€–    *€€ь0    n€€М     Ѓ€е    ц€N     ЏЯ                       Y“шr               3…€€€р             І€€€цЭ,           Гъ€€€°            jз€€€Ѕ0            A“€€€ЁU            (ґ€€€тw           Рь€€ьТ           wр€€€ї*            RЁ€€€‘H             Р€€€€Ѕ               RЁ€€€÷L               tо€€€ї*               Рь€€€Щ               &≤€€€тw               A“€€€яY                fз€€€…3               Гш€€€І               •€€€ъ•5               3«€€€р                 U“шrnоь€€€€€€€€€€€€€€€ьоnъ€€€€€€€€€€€€€€€€€€€ъnо€€€€€€€€€€€€€€€€€лp                                                               nоь€€€€€€€€€€€€€€€ьоnъ€€€€€€€€€€€€€€€€€€€ъnо€€€€€€€€€€€€€€€€€лptц“Y                 т€€€«3               .Эш€€€І               °€€€ъГ               0≈€€€зj                RЁ€€€‘D               wт€€€ґ(               Ть€€ьР               *ґ€€€рw               D‘€€€ЁR                љ€€€€М             H÷€€€ЁP            *ґ€€€оt           Ць€€ьТ           wт€€€≤(            Yя€€€‘D            3…€€€еf            І€€€шБ
           3Іъ€€€£             ф€€€«0               rш“U                   [£“цъф√Г   0Щш€€€€€€€€шp  €€€€€€€€€€€€€{ €€€њc* FЄ€€€.€€ь        Я€€°ц€о        €€йwъn        €€ш           Б€€т         Ц€€€Э       A£ъ€€€о     Ое€€€€€б3      €€€€€фГ
       €€€Џt         р€й            rъn                                                        nоъоn          ъ€€€ъ          nо€оp         ?љццљD     l€€€€€€c   0€€Иl€ш  ≈€И    Е€К 0€л    &€‘ Е€Щ      €ъ і€R      €€ р€   3™т€€ ъ€   f€€€€€ €€  0€€Г€€ €€  ђ€Е  €€ €€  ф€  €€ €€  ь€   €€ €€  ÷€5  €€ €€  y€÷& €€ ш€ ≈€€€€€њб€  Ттшшш≤ђ€L         t€О         *€г         ≈€y     Єі  9€€y 5ґ€т   l€€€€€€€c    DЄцше•3        Ы€€€€€€“                €€€€€€€€?               Щ€€€€€€€ђ                  т€€€ь                 Г€€ґ€€Е                т€Ўй€л               Г€€] €€c              ф€Џ  ь€ќ              Г€€]   £€€5            ф€Џ    3€€°            Г€€_     «€ъ          ф€Џ      Y€€}          Е€€€€€€€€€€€€г        ф€€€€€€€€€€€€€[        Е€€€€€€€€€€€€€€√       ф€Џ          Э€€.      И€€a          0€€Щ     ц€Ё           √€ъ  Ц€€€€€€€Ы      Ы€€€€€€€Ыш€€€€€€€€      €€€€€€€€€Ф€€€€€€€Ц      Щ€€€€€€€ЦЫ€€€€€€€€€€€ъріt    €€€€€€€€€€€€€€€€фh   Щ€€€€€€€€€€€€€€€€€Г     €€€       AЭ€€€A    €€€          p€€љ    €€€          €€ц    €€€          €€ц    €€€         «€€≤    €€€      DМф€€ц*    €€€€€€€€€€€€€€тH     €€€€€€€€€€€€€€Ё7     €€€€€€€€€€€€€€€ьP    €€€       R∞€€€ш(   €€€          Lц€€Я   €€€           _€€л   €€€           €€ъ   €€€           ,€€г   €€€        cе€€МЫ€€€€€€€€€€€€€€€€€€г€€€€€€€€€€€€€€€€€€њ Щ€€€€€€€€€€€€ьц–Э=        P°÷цътіp Jфl    ]г€€€€€€€€оY–€о  Ѓ€€€€€€€€€€€€€€ь  ≤€€€Ўn* NЫъ€€€€ p€€€О       ћ€€€т€€Е          €€ьp€€ќ            е€рі€€[            nъpт€€               ь€€                €€€                €€€                ъ€€               ÷€€,               Ф€€К               .€€ь3          “т] Я€€оF        Ў€€ф ‘€€€ЄW  7о€€€«  ћ€€€€€€€€€€€€€б   Бф€€€€€€€€€ьЭ      [•–цьъгђr   Ы€€€шшшшшшцбІ[     €€€€€€€€€€€€€€лp    Щ€€€€€€€€€€€€€€€∞    €€€     &f–€€€Ц    €€€         К€€€P   €€€          Ы€€б  €€€          е€€a  €€€           l€€ђ  €€€           €€о  €€€            €€ш  €€€            €€€  €€€            €€€  €€€           €€ц  €€€           ,€€Џ  €€€           €€Я  €€€          &т€€?  €€€         7е€€ї   €€€      LЭ€€€й Ы€€€€€€€€€€€€€€€г,  €€€€€€€€€€€€€€€•   Щ€€€€€€€€€ърґЕ,     Ы€€€€ьшшшшшшшшшшш €€€€€€€€€€€€€€€€€ Щ€€€€€€€€€€€€€€€€    €€€        €€€    €€€        €€€    €€€        €€€    €€€        Щ€Ы    €€€   Ц€Ы         €€€   €€€         €€€€€€€€€         €€€€€€€€€         €€€€€€€€€         €€€   €€€         €€€   Щ€Ы         €€€         Ц€Ы   €€€         €€€   €€€         €€€   €€€         €€€Ы€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€€€€€Ы€€€€€ьшшшшшшшшшшшшш€€€€€€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€€€€€€€   €€€           €€€   €€€           €€€   €€€           €€€   €€€           Щ€Ы   €€€     Ц€Ы         €€€     €€€         €€€€€€€€€€€         €€€€€€€€€€€         €€€€€€€€€€€         €€€     €€€         €€€     Щ€Ы         €€€                 €€€                 €€€                 €€€              Ы€€€€€€€€€€Ы        €€€€€€€€€€€€        Щ€€€€€€€€€€Ц             U°‘цътїl pцh      Wг€€€€€€€€зUе€р     Ы€€€€€€€€€€€€€€ь    Ы€€€ЄW  JОт€€€€   _€€ьU        ф€€ь  о€€h          Я€€з  n€€«           &Ёт[  і€€U                 т€€
                 ь€€                  €€€                  €€€       Ы€€€€€€€€€Ыь€€       €€€€€€€€€€€з€€      Щ€€€€€€€€€ЦІ€€f            €€€  L€€Ё           €€€   –€€√         *€€€   3ъ€€шЭR&  U∞€€€€    Pш€€€€€€€€€€€€€€о     (∞€€€€€€€€€€€г{       f•ќфьъф≈ЩL    Ы€€€€€€Ы    Ы€€€€€€Ы€€€€€€€€    €€€€€€€€Щ€€€€€€Ц    Щ€€€€€€Ц  €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€€€€€€€€€€€€€€    €€€€€€€€€€€€€€€€    €€€€€€€€€€€€€€€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€  Ы€€€€€€Ы    Ы€€€€€€Ы€€€€€€€€    €€€€€€€€Щ€€€€€€Ц    Щ€€€€€€ЦЫ€€€€€€€€€€€€€Ы€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€Ц      €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€      Ы€€€€€€€€€€€€€Ы€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€Ц       Ы€€€€€€€€€€€Ы       €€€€€€€€€€€€€       Щ€€€€€€€€€€€Ц             €€€                 €€€                 €€€                 €€€                 €€€                 €€€                 €€€                 €€€    tъn          €€€    т€о          €€€    €€ь          €€€    €€€          €€ц    €€€         (€€“    €€€        ћ€€Г    €€€ћГL  .}л€€г    €€€€€€€€€€€€€й.     .Рз€€€€€€€€€ґ         7}ЃеъъЎЩ7        Ы€€€€€€€Ы     Ы€€€€€€Ы €€€€€€€€€     €€€€€€€€ Щ€€€€€€€Ц     Щ€€€€€€Ц    €€€        Nг€€Ы      €€€      ђ€€гF        €€€     nт€€Я         €€€   *ќ€€еJ           €€€ 
Мь€€°            €€€Lя€€€ьr             €€€€€€€€€€І           €€€€йp«€€€€І           €€€™  cш€€€}          €€€     Dш€€€9         €€€      U€€€÷        €€€       Б€€€w        €€€        –€€т       €€€        N€€€Г       €€€         я€€ф   Ы€€€€€€€Ы      {€€€€€€Ы€€€€€€€€€      €€€€€€€Щ€€€€€€€Ц       ≈€€€€€ЦЫ€€€€€€€€€Ы        €€€€€€€€€€€        Щ€€€€€€€€€Ц            €€€                €€€                €€€                €€€                €€€                €€€                €€€                €€€                €€€                €€€                €€€         Ц€Ы    €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€Ы€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€€€€€€Ц€€€              Г€€€Ыш€€€ь7            =€€€€€Ф€€€€я          б€€€€Ц  €€€€£          •€€€€    €€€€€Y        [€€€€€    €€€€€о      т€€€€€    €€€≤€€√      «€€і€€€    €€€т€€}    }€€т€€€    €€€ j€€ь7  7€€€j €€€    €€€  ≈€€ЏЁ€€«  €€€    €€€  (ш€€ЯЯ€€ъ*  €€€    €€€   }€€€€€€}   €€€    €€€   ‘€€€€÷   €€€    €€€    3€€€€5    €€€    €€€     О€€Р     €€€    €€€     
яг     €€€    €€€            €€€    €€€              €€€  Ц€€€€€€Ы        Ы€€€€€€Ыш€€€€€€€        €€€€€€€€Ф€€€€€€Ц        Щ€€€€€€ЦЦ€€€њ        Ы€€€€€€Ыш€€€€І        €€€€€€€€Ф€€€€€О       Щ€€€€€€Ц   €€€€p         €€€     €€€€€P        €€€     €€€€€ш=       €€€     €€€ф€€о*      €€€     €€€H€€€е     €€€     €€€ c€€€‘    €€€     €€€  Б€€€њ   €€€     €€€   Э€€€І   €€€     €€€    Є€€€О  €€€     €€€    ћ€€€p €€€     €€€     я€€€P€€€     €€€      "й€€ш€€€     €€€       9ц€€€€€     €€€        L€€€€€     €€€         c€€€€   Ы€€€€€€Ы       Б€€€   €€€€€€€€        Э€€   Щ€€€€€€Ц         Є€        NІгшъгІP          ;÷€€€€€€€€÷;       n€€€€€€€€€€€€h     p€€€о**о€€€h   9€€€√      њ€€€7  ‘€€√        √€€“ D€€ф          т€€AЭ€€М            К€€Э–€€?            ?€€“ц€€            €€ц€€€              €€ъц€€            €€ц–€€=            ;€€“Э€€И            Г€€ЭD€€т          о€€A ‘€€√        љ€€“  ;€€€Ѕ      ї€€€9   p€€€о,  (yо€€€j     p€€€€€€€€€€€€h       ;‘€€€€€€€€÷9          JІбъъбІN      Ы€€€€€€€€€€цбІW   €€€€€€€€€€€€€€€Ў9  Щ€€€€€€€€€€€€€€€цF    €€€      lз€€о   €€€        Џ€€К   €€€         =€€Ў   €€€         €€ъ   €€€         €€ц   €€€         {€€љ   €€€        Y€€€c   €€€     PЄ€€€√    €€€€€€€€€€€€€√    €€€€€€€€€€€шБ     €€€€€€€ътЄ}       €€€                €€€                €€€                €€€             Ы€€€€€€€€€€Ы       €€€€€€€€€€€€       Щ€€€€€€€€€€Ц             D°бшшбІJ          ,ќ€€€€€€€€‘7       Uш€€€€€€€€€€ьc     P€€€о**о€€€h   $ц€€√      њ€€€7  ї€€√        √€€“ .€€ф          т€€DИ€€М            К€€Щї€€?            ?€€–т€€            €€ць€€              €€ъъ€€            €€цл€€=            ;€€“Ѓ€€И            Г€€Яt€€т          о€€Fь€€√        љ€€÷  Я€€€√      ї€€€?  й€€€о, (yо€€€y    7о€€€€€€€€€€€€t      $ќ€€€€€€€€€–;         Y≈€€€фЅО;           9Є€€гA*=0≤ъГ   5√€€€€€€€€€€€€€€ъ   р€€€€€€ь€€€€€€€ьl   цг£fA P•ршЏР$ Ы€€€€€€€€€ьцеђj      €€€€€€€€€€€€€€€фr     Щ€€€€€€€€€€€€€€€€Ц       €€€      PЅ€€€R      €€€         О€€Ѕ      €€€         €€ц      €€€         *€€ф      €€€        3я€€Э      €€€    .hњ€€€я      €€€€€€€€€€€€€ї       €€€€€€€€€€€…N         €€€€€€€€€€€c          €€€    М€€€P         €€€      t€€ц0        €€€       О€€б       €€€        љ€€°       €€€        й€€F      €€€         F€€Ё  Ы€€€€€€€Ы       Ц€€€€Ы€€€€€€€€€       б€€€€Щ€€€€€€€Ц        L€€€Ц    (Мћцъо°. tшМ    Мь€€€€€€€pе€т   •€€€€€€€€€€€€ь  ]€€€њP ;Ц€€€€  «€€Т       Dь€€  ъ€€        њ€ь  ш€€(        Т€о  ∞€€÷*       ,оw  5€€€€“Э[9        l€€€€€€€ь…К0      Fћ€€€€€€€€€≤      $_£«ш€€€€€≈          ,f“€€€r             Ы€€ћБт;          €€цф€І          €€ц€€ф9        ґ€€√€€€€њc( ;з€€€a€€€€€€€€€€€€€€€• о€фRе€€€€€€€€тn  jц fђтьшбђh   €€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€     €€€     €€€€€€     €€€     €€€€€€     €€€     €€€€€€     €€€     €€€Щ€Ы     €€€     Щ€Ы        €€€                €€€                €€€                €€€                €€€                €€€                €€€                €€€                €€€                €€€            Ы€€€€€€€€€Ы        €€€€€€€€€€€        Щ€€€€€€€€€Ц    Ы€€€€€€Ы    Ы€€€€€€Ы€€€€€€€€    €€€€€€€€Щ€€€€€€Ц    Щ€€€€€€Ц  €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    ь€€          €€ъ    р€€        €€б    ™€€r        l€€Э    ?€€ц=      =т€€7     Ы€€ьР, 0Рь€€°      ї€€€€€€€€€€√
       Ц€€€€€€€€Э          .ЩЎъъЁЩ0      Ц€€€€€€€€Ы     Ы€€€€€€€€Ыш€€€€€€€€€     €€€€€€€€€€Ф€€€€€€€€Ц     Щ€€€€€€€€Ц  Ё€€n           l€€Џ      h€€г         б€€c      б€€l         j€€я       j€€б       я€€j        г€€j       c€€г         n€€Ё     Џ€€l          е€€_     ]€€г           r€€Џ     Ў€€r            й€€]   Y€€з             w€€Ў   ÷€€t              
л€€W U€€л               {€€“ “€€w                о€€Э€€л                 }€€€€€}                  т€€€р                   €€€}                    ф€ф                     Г€Г           Ц€€€€€€€€€Ы     Ы€€€€€€€€€Ыш€€€€€€€€€€     €€€€€€€€€€€Ф€€€€€€€€€Ц     Щ€€€€€€€€€Ц   ш€€Е             Б€€ц      Є€€ђ             ђ€€Є      Т€€е     L€[     б€€Ф      W€€€    «€ќ    €€€U      (€€€H   =€€€D   D€€€&       ш€€f   љ€€€Ѕ   a€€ц        ї€€Я  0€€р€€3  Я€€љ        Ф€€√  І€€N€€ђ  Ѕ€€Ф        Y€€ш &€€ђ ∞€€& ш€€U        ,€€€$Ф€€3 3€€Щ"€€€&         ш€€lъ€њ   Ѕ€ъl€€ш          њ€€й€€?   D€€з€€љ          Щ€€€€ќ     –€€€€Ц          Y€€€€R     [€€€€W          .€€€Ў      я€€€*          ъ€€h       l€€ш            Ѕ€г       й€њ            Ы€r         }€Ц      Ы€€€€€Ы      Ы€€€€€Ы€€€€€€€      €€€€€€€Щ€€€€€Ц      Щ€€€€€Ц  М€ш;        0р€К     ≤€о"      з€І      ќ€я    ÷€њ       з€≈  √€÷         3ц€≤  ∞€з           R€€ОЩ€т0             t€€€ьF               ї€€Е               Dъ€€т5             .т€ќ÷€й"           з€яг€я         ‘€з  "й€ћ       њ€о,    0р€Є      •€ц=      =ц€•     К€ьH        L€€И  Ы€€€€€€Ы     Ы€€€€€Ы€€€€€€€€     €€€€€€€Щ€€€€€€Ц     Щ€€€€€ЦЫ€€€€шбy     Ы€€€€€€Ы€€€€€€€ъ     €€€€€€€€Щ€€€€ьеn     Щ€€€€€€Ц  “€й       й€“     ,ь€…     ќ€ь,       €€£     •€€}        –€€p   r€€–         *ш€€A A€€ъ*           w€€о=л€€w             …€€ь€€…              &ш€€€ш&               n€€€r                 €€€                  €€€                  €€€                  €€€                  €€€                  €€€              Ы€€€€€€€€€Ы          €€€€€€€€€€€          Щ€€€€€€€€€Ц      €€€€€€€€€€€€€€€  €€€€€€€€€€€€€€€  €€€€€€€€€€€€€€€  €€€        p€€М  €€€       L€€∞   €€€      5ц€«   €€€     й€я    Щ€Ы    ‘€о&           љ€ъ=            £€€[            }€€}            Y€€Я            ?ъ€љ           &о€Ў      Ц€Ы  я€й       €€€ «€ф5        €€€ Ѓ€€P         €€€М€€r          €€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€Ы€€€€€€€€€€€€€Ц€€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€€€€Ы€€€€€€€шшшшшшЦrцF             ц€Ѕ             «€€.            L€€ї             «€€9            9€€«             ї€€J            .€€“             ™€€Y            &ь€Џ            Ы€€l            ъ€е            М€€w            ф€о            €€Г            л€ф            r€€О            б€ъ            f€€Э            Ў€€(            W€€∞             –€€3            F€€љ             √€€?            7€€…             ∞€€R            *€€√             ї€ц             ;тИЫ€€€€€€€€€€€€€Щ€€€€€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€    €€€Ы€€€€€€€€€€€€€Щшшшшшш      
ђ            І€І           p€€€p         ?€€ш€ь?       й€г*е€й     «€ш7 7ъ€«    Щ€€c   f€€Ц   _€€Э     Я€€] 0ц€…     ќ€ц.ќ€й       о€“ш€D         L€шИђ           ЃМrр€€€€€€€€€€€€€€€€€€€€€рrш€€€€€€€€€€€€€€€€€€€€€€€ъnлшшшшшшшшшшшшшшшшшшшшшлn∞еJ    я€€{   "÷€€Ѓ  Ѓ€€÷"   {€€г    Fе∞ RГІЅршъцбІW      з€€€€€€€€€€€÷*     Мш€€€€€€€€€€€й             H‘€€Щ               €€й                €€ь       JЩ√тшъцй≤€€€     DЏ€€€€€€€€€€€€    n€€€€€€€€€€€€€€   J€€€яw. 
=W€€€   ќ€€Я        €€€   ъ€€       *Ѓ€€€   –€€•5 H{«€€€€€   A€€€€€€€€€€€€€€€€€Ы Hй€€€€€€€€ЎL€€€€€€  pґтьцћЩD  €€€€€ЦЫ€€€€€              €€€€€€              Щ€€€€€                 €€€                 €€€                 €€€                 €€€ AЭ“цът≤h       €€€“€€€€€€€€йW      €€€€€€€€€€€€€€{     €€€€€‘] W–€€€_    €€€€ђ     ™€€т   €€€–       –€€}   €€€W         W€€≈   €€€         €€ц   €€€           €€ш   €€€         €€о   €€€         }€€І   €€€шF       Fъ€€H   €€€€ьФ= =Ць€€і Ы€€€€€€€€€€€€€€€€… €€€€€€Ё€€€€€€€€€Ц
  Щ€€€€€P•ЁшьтњБ$        R•Џцъця™h&«рY    Hя€€€€€€€€€€€€з   t€€€€€€€€€€€€€€ь  _€€€Ўh  PЄ€€€€€ т€€І       P€€€ь }€€ќ         Ў€€з √€€U          Rзр[ ц€€               ь€€                р€€               ™€€Б           “зLF€€ьR         Џ€€з ∞€€€ЄY(  FЕр€€€б њ€€€€€€€€€€€€€€ъJ  ц€€€€€€€€€€€….     fІ–цььцяЃ}3              Ы€€€€€              €€€€€€              Щ€€€€€                 €€€                 €€€                 €€€       hітъц“ЭD €€€      Wз€€€€€€€€“€€€     {€€€€€€€€€€€€€€    _€€€‘_ 
W–€€€€€   т€€ђ     •€€€€   {€€–       –€€€   ≈€€W         W€€€   ц€€         €€€   ь€€           €€€   р€€         €€€   ™€€         }€€€   H€€шF       Fъ€€€    ≤€€ьЦ= =Фь€€€€    …€€€€€€€€€€€€€€€€Ы  
Ц€€€€€€€€о]€€€€€€    $Бљтьц√{ €€€€€Ц     ?Ц–цъцќТ;        0ћ€€€€€€€€€ћ9     hь€€€€€€€€€€€€[   a€€€фК; ;Кф€€ь7 ц€€–       ћ€€÷М€€б         я€€a‘€€A           ?€€≤ъ€€€€€€€€€€€€€€€€€тъ€€€€€€€€€€€€€€€€€шЏ€€€€€€€€€€€€€€€€€ъТ€€F               &ь€оF               y€€€ЄY&  ?WМітцy  М€€€€€€€€€€€€€€€ш   Uб€€€€€€€€€€€€йc    P°–цьшфЏ≤Эa7       tіршьшр–ЃР,     Pр€€€€€€€€€€о    =€€€€€€€€€€€ш}    Ѕ€€≤0            ъ€€              €€€           Ы€€€€€€€€€€€€€Ы   €€€€€€€€€€€€€€€   Щ€€€€€€€€€€€€€Ц       €€€               €€€               €€€               €€€               €€€               €€€               €€€               €€€               €€€               €€€           Ы€€€€€€€€€€€€Ы    €€€€€€€€€€€€€€    Щ€€€€€€€€€€€€Ц        nЃръшрїГ0€€€€€Ы   fр€€€€€€€€€€€€€€€  Ц€€€€€€€€€€€€€€€€Ц €€€ќU 
Wќ€€€€€   &€€€Щ       Э€€€€   Р€€љ         њ€€€   Ў€€A         ?€€€   ъ€€         €€€   ъ€€         €€€   Џ€€?         ?€€€   Т€€љ         њ€€€   (€€€Ф       Ц€€€€    Б€€€ќU 
Wќ€€€€€     Щ€€€€€€€€€€€€€€      hо€€€€€€€€й€€€       lЃръъбІ]€€€                 €€ъ                €€б                Т€€Щ             *Ы€€ь(      nзь€€€€€€€€€a       ъ€€€€€€€€€оN        nйшшшшшц≈}      Ы€€€€€              €€€€€€              Щ€€€€€                 €€€                 €€€                 €€€                 €€€ “ъшб£F        €€€[р€€€€€€€љ      €€€€€€€€€€€€€ћ     €€€€фЕ5 [г€€t     €€€І      9€€…     €€€        €€ц     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€   Ы€€€€€Ы     Ы€€€€€Ы €€€€€€€     €€€€€€€ Щ€€€€€Ц     Щ€€€€€Ц      €€€            €€€            €€€                                                    Ы€€€€€€€       €€€€€€€€       Щ€€€€€€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€      Ы€€€€€€€€€€€€€Ы€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€Ц       €€€          €€€          €€€                                          Ы€€€€€€€€€€€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€          €€€          €€€          €€€          €€€          €€€          €€€          €€€          €€€          €€€          €€€          €€€          €€€          €€€          €€ь         €€р         €€™       Б€€€9rр€€€€€€€€€М ъ€€€€€€€€ь}  nошшшшшбЫ*   Ы€€€€€              €€€€€€              Щ€€€€€                 €€€                 €€€                 €€€                 €€€   {ть€€€€€€Ы    €€€   ъ€€€€€€€€€    €€€   О€€€€€€€€Ц    €€€   y€€€€О       €€€ 
™€€€рN         €€€“€€€–$          €€€й€€€Ы           €€€€€ъa             €€€Џ€€≈*            €€€0Ё€€ьГ          €€€ Гь€€ЁF         €€€   *…€€€•       €€€     lш€€оf   Ы€€€€€    Фь€€€€€€€Ы€€€€€€    ъ€€€€€€€€€Щ€€€€€    Ит€€€€€€€Ц Ы€€€€€€€       €€€€€€€€       Щ€€€€€€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€      Ы€€€€€€€€€€€€€Ы€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€ЦЫ€€€€?∞тъеФRІзъцЅc    €€€€€€€€€€€€€€€€€€€£   Щ€€€€€€€€€€€€€€€€€€€f    €€€тN J€€€тn$ F€€ћ    €€€R    €€€;    €€ъ    €€€     €€€     €€€    €€€     €€€     €€€    €€€     €€€     €€€    €€€     €€€     €€€    €€€     €€€     €€€    €€€     €€€     €€€    €€€     €€€     €€€    €€€     €€€     €€€  Ы€€€€€Ы   €€€€Ы   €€€€Ы€€€€€€€   €€€€€   €€€€€Щ€€€€€Ц   €€€€Ц   €€€€ЦЫ€€€€ М‘цъеІJ     €€€€€cш€€€€€€€љ   Щ€€€€€€€€€€€€€€ћ    €€€€лy, 
]г€€p    €€€°      3€€≈    €€€         €€ц    €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€  Ы€€€€€Ы     Ы€€€€€Ы€€€€€€€     €€€€€€€Щ€€€€€Ц     Щ€€€€€Ц     NІгшшб•P        AЏ€€€€€€€€ЏH     p€€€€€€€€€€€€w   Y€€€еt$ "tй€€€] л€€Ѕ      √€€оr€€Џ        Ё€€wЅ€€_          ]€€√ц€€          €€цъ€€           €€ъя€€7          7€€яФ€€≈        √€€Ц&ь€€∞      ∞€€ь( Г€€€бp$  $nг€€€М   Я€€€€€€€€€€€€£     cй€€€€€€€€зc       _ђгььг™]    Ы€€€€€ Иќцъйђ]    €€€€€€jъ€€€€€€€Ё?   Щ€€€€€€€€€€€€€€€€]     €€€€€÷_ 
[÷€€€H    €€€€љ
     і€€й   €€€е       я€€w   €€€j         j€€√   €€€         €€ц   €€€          €€ъ   €€€9         7€€г   €€€√         њ€€Ы   €€€€°      Ы€€€.   €€€€€–W Uћ€€€Г    €€€€€€€€€€€€€€Ф     €€€lш€€€€€€€еW      €€€ Кћцълђc       €€€                 €€€                 €€€                 €€€              Ы€€€€€€€€Ы          €€€€€€€€€€          ЩшшшшшшшшЦ              f∞тъц√t €€€€€Ы   Hе€€€€€€€оY€€€€€€  j€€€€€€€€€€€€€€€€Ц N€€€‘]
 _Џ€€€€€   л€€і     ї€€€€   y€€я       е€€€   √€€c         n€€€   ц€€         €€€   ь€€          €€€   е€€7         7€€€   Э€€Ѕ         њ€€€   .€€€°       Ы€€€€    Е€€€ќW Uћ€€€€€     Т€€€€€€€€€€€€€€      Wе€€€€€€€€г€€€       cђлъъбђc€€€                 €€€                 €€€                 €€€                 €€€             Ы€€€€€€€€Ы          €€€€€€€€€€          ЩшшшшшшшшЦ Ы€€€€€   ќъцљ]   €€€€€€ Бш€€€€€€ќ Щ€€€€€3÷€€€€€€€€€ќ    €€€ь€€фЦ?Aо€€о    €€€€т    9геP    €€€К              €€€                €€€                €€€                €€€                €€€                €€€                €€€            Ы€€€€€€€€€€€€Ы     €€€€€€€€€€€€€€     Щ€€€€€€€€€€€€Ц        fђршшеђ_Pфn   Aе€€€€€€€€€€ф  ;ъ€€€€€€€€€€€€  ≈€€ЅP  DИл€ц  ь€€       [шИ  р€€лЯ]J        Ц€€€€€€€ш…ЫH    б€€€€€€€€€€–(   }ќь€€€€€€€€й     =Uj°Ѕъ€€€ЫcцЅ       И€€тз€€t        €€ш€€€€їW  ,j÷€€≤€€€€€€€€€€€€€€й"з€€€€€€€€€€€€ї aшИ7ИЄтььфЅР7       pъl                л€й                €€ь                €€€                €€€                €€€            nйь€€€€€€€€€€€ьзh  ъ€€€€€€€€€€€€€€€ъ  nй€€€€€€€€€€€€€зl      €€€                €€€                €€€                €€€                €€€                €€€                €€€                €€€                ш€€
       U‘фa    Є€€І, =w‘€€€ф    9€€€€€€€€€€€€€ґ     Wц€€€€€€€€€б{      ГЅфьшЏ™r5   Ы€€€€€      Ы€€€€€  €€€€€€      €€€€€€  Щ€€€€€      Щ€€€€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     ц€€      =љ€€€     ≤€€ђ 7t“€€€€€     .ъ€€€€€€€€€€€€€€Ы    Nф€€€€€€€–F€€€€€     Р÷ъъ÷Ы?  €€€€ЦЦ€€€€€€€Ы     Ы€€€€€€€Ыш€€€€€€€€     €€€€€€€€€Ф€€€€€€€Ц     Щ€€€€€€€Ц   ъ€л       й€ъ       Ц€€j       h€€Ц        (€€Ў       ÷€€(         Є€€J     F€€Є          ?€€њ     њ€€?           –€€*   *€€–            c€€Ы   Щ€€a            з€ъ ъ€е             €€Б €€{              ъ€л
й€ъ               Ц€€√€€Ц                &€€€€€(                 і€€€Є         Ы€€€€€Ы       Ы€€€€€Ы€€€€€€€       €€€€€€€Щ€€€€€Ц       Щ€€€€€Ц ъ€}     Цw    €ъ   і€≈    .€з   ћ€ґ    h€€   …€€h  €€h    €€[  c€€€Ў  c€€     ќ€І й€€€€L ∞€ћ      Б€цТ€€Т€€√ъ€{      5€€r€€“ “€€Г€€,       з€ь€€_ ]€€ъ€Ё        Ы€€€б б€€€Р        N€€€p   p€€€A        ъ€л   о€т         ∞€Б     €£          aц     шP      pрь€€ьрl   jрь€€ьрp  ъ€€€€€€ъ   ц€€€€€€ъ  w–ц€€€€Г   Е€€€€ц–y     б€€Y   [€€Ё         "й€€N P€€г           0о€ш{ъ€з"             =ц€€€й*               t€€€]               0о€€€й(             (й€т{€€е           з€ц= [€€б         г€ьJ   h€€Ў       Ё€€W     r€€–   r–фь€€€Ы     Э€€€ъфќtъ€€€€€€ъ     ъ€€€€€€ъwц€€€€ш}     yш€€€€фwЦ€€€€€€Ы       Ы€€€€€€Ыш€€€€€€€       €€€€€€€€Ф€€€€€€Ц       Щ€€€€€€Ц  
з€т          “€Ѕ      h€€Щ         f€€A       ÷€€*       е€«        L€€Ѕ       Г€€F         Ѕ€€N     ш€ќ          .€€Ё    •€€N           °€€w   3€€“            ш€т  ћ€€Y             Г€€Щ Y€€Ў              л€€3б€€_               r€€ц€€Џ                Џ€€€€h                 [€€€б                 P€€€l                  {€€г                  ™€€r                   Ё€й             Ы€€€€€€€€€€Ы           €€€€€€€€€€€€           ЩшшшшшшшшшшЦ        €€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€        Ѓ€тRЩ€Ы       *Џ€÷$          Yф€•
         Т€ьn          ≈€з9          Aй€љ          t€€И          Ѓ€рN          *Џ€“$       Ц€ЫYф€•
        €€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€     PЅцрf    Ц€€€€ш   h€€€€зc   ÷€€l     ь€€       €€€       €€€       €€€       €€€       €€€       €€ь     ;Г€€л    ∞€€€€Ц    ъ€€€€(    °€€€€Щ     "l€€т       €€ь       €€€       €€€       €€€       €€€       €€€       ь€€       Џ€€l     f€€€€лf    Ф€€€€ш     UњцрfГъГь€ь€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€ь€€ф€ьnцИfрцњL     ъ€€€€Ф    fй€€€€[     p€€“       €€ъ       €€€       €€€       €€€       €€€       €€€       ь€€       й€€Б;     Т€€€€Ѓ    (€€€€ъ    Щ€€€€Э    р€€n"     ь€€       €€€       €€€       €€€       €€€       €€€       €€ь     
l€€Џ   fл€€€€r   ъ€€€€Э    hтцњU       n–шЁt           …€€€€€Є        б€€€€€€€Є    ÷рcљ€€€≈Р€€€Ц  ‘€€цт€€Ў  Ы€€€Є€€€љ[р–    ≤€€€€€€€Џ        ≤€€€€€ћ           pЏьЎ}    pфтn  ъ€€ъ  wшцw                    ?гг=  …€€љ  ш€€т €€€ь$€€€€H€€€€FW€€€€Pj€€€€_Ы€€€€ФІ€€€€£ґ€€€€≤з€€€€яш€€€€ць€€€€ъ÷€€€€“f€€€€f wййt       LL            £€€•            ш€€ц            €€€€            €€€€            €€€€         wЏ€€€€яР&     3б€€€€€€€€ьцгF *р€€€€€€€€€€€€Џ÷€€€≤J Yй€€€ьc€€€_      c€€€€ґ€€О       €€€ьф€€        Ё€€Џь€€         RреFт€€            і€€{            f€€ъF       ]бл[Џ€€€Ф7 a…€€€т 0т€€€€€€€€€€€€ї  7б€€€€€€€€€ьЩ   
yЎ€€€€рЃr        €€€€            €€€€            ь€€ъ            –€€ќ            $££&           
w≈цъл™J          &я€€€€€€€љ       з€€€€€€€€€Є       Е€€я;_о€€ц       Ў€€*    .бфj       ъ€€                ш€€               і€€j               a€€њ            yф€€€€€€€ьоp       ъ€€€€€€€€€€ъ       pо€€€€€€€€тr            €€ъ                €€ъ               €€р               [€€ґ               …€€w        pцp  "•€€ь        о€цhг€€€€€€€€€€€€€€€€ръ€€€€€€€€€€€€€€€€€Иnй€€€€€€€€€€€€€ъЎp {ц{         Гъ}ъ€€lHІтътІFp€€цК€€€€€€€€€€€€€Щ О€€€€€€€€€€€К  9€€€Я" Э€€€;  ™€€Ы     Ц€€І  р€€     €€р  €€€       €€ъ  р€€"     $€€р  ™€€£     Я€€І  ?€€€£" °€€€=  €€€€€€€€€€€ Е€€€€€€€€€€€€€Иш€€r=Ір€р™?t€€ш{ъБ         ГшtЫ€€€€€€€Ы   Ы€€€€€€€Ы€€€€€€€€€   €€€€€€€€€Щ€€€€€€€Ц   Щ€€€€€€€Ц  б€€Т       Р€€б     ?€€€]     Y€€€=       К€€ц0   .ц€€К        “€€б б€€‘         (ш€€љ ї€€ш*           n€€€‘€€€p             ї€€€€€њ           ‘€€€€€€€€€€€–        “€€€€€€€€€€€ћ             €€€                  €€€             –€€€€€€€€€€€ћ        “€€€€€€€€€€€ћ             €€€                  €€€              Ы€€€€€€€€€Ы          €€€€€€€€€€€          Щ€€€€€€€€€Ц     БъГь€ь€€€€€€€€€€€€€€€€€€€€€ь€€ф€ьlъИ         ъь€ь€€€€€€€€€€€€€€€€€€€€€ь€€ф€ьnцИ      *°ть€€€€€€€       Jц€€€€€€€€€€      тоW      €€      И€L        €€      Ў€         шъ      ь€0        ™∞      з€«            "М‘ь€€Џ?          Nш€€€€€€€њ?        з€y0 Гл€€–]     г€5     jЏ€€гr   ]€тc       Rћ€€яN   lь€ќL       =≈€€Ф   3ќ€€“_      Rй€{    L√€€бr     &€й      0Ыъ€лБ (t€з        о€€€€€€ъN          Гь€€ЏФ&             N€е      ђІ        €ъ      ъц        €л      €€        ]€Я      €€      _тш*      €€€€€€€€€€€Y       шшшшшшшшцљA      БъГ   БъГъ€ъ   ъ€ъъИ   ъЕ      lђзшъйђl          Мъ€€€€€€€€ъМ       5г€ъЯR  UЃь€е7     9т€њ"        &њ€ц?   й€Э            Т€л  °€≤   $Цбът•5љї  ∞€• $€т  Wш€€€€€€€€  ф€&Б€И  9€€Щ* [з€€   Р€Гњ€=  ґ€Е     3€ь   D€Ѕц€  ш€      ‘Є   €фь€   €€             €ъц€  ъ€           €фЅ€?  …€f           D€√Б€К  L€€Г*  
]Ў«   О€Е&€т  jь€€€€€€€•  ф€& °€≤   (ЦЎъьођP   ∞€І  й€Э            Т€о   ;ц€њ"        $њ€ц?     5г€ъ°P  RЃь€з9       Оъ€€€€€€€€ъМ          lђзъълђj       9Щћцъті9    ъ€€€€€€ъ;   PfD }€√         €ш    L™зъъя€€  Ѓ€€€€€€€€  М€з_ =€€  ц€    "€€  е€[0Иф€€  [€€€€€шN€€фW ?њшш∞, €€цU       ї«     Є«      .б€«    .б€«     [ц€я   [ц€я    Р€€л$   Р€€л$   Ѕ€€ц;  Ѕ€€ц;   3г€€€N  3г€€€N   ]ш€€€h  ]ш€€€h    _ш€€€p  _ш€€€p     5е€€€P  5е€€€P     √€€ш?  √€€ш?     Ц€€о*  Ц€€о*      _ш€е   fъ€е      5з€÷    ;й€÷       ≈Ѕ     «ЅГц€€€€€€€€€€€€€€€€€€€ш€€€€€€€€€€€€€€€€€€€€Бш€€€€€€€€€€€€€€€€€€€                  €€€                  €€€                  €€€                  €€€                  €€€                  €€€                  ш€ц                  ъБ€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€      lђзшъйђl          Мъ€€€€€€€€ъМ       5г€ъЯR  UЃь€е7     9т€њ"        &њ€ц?   й€Э            Т€л  °€≤ Ѕь€€€ьцґ=    ∞€• $€т Єь€€€€€€€L   ф€&Б€И    €€  }€÷    Р€Гњ€=    €€   €ъ    D€Ѕц€    €€  (∞€ћ    €фь€     €€€€€€т7     €ъц€    €€€€€€И     €фЅ€?    €€  Т€€c    D€√Б€К    €€   Ц€€?   О€Е&€т Єъ€€ь≈  І€ь« ф€& °€≤ њ€€€€√  љ€≈ ∞€І  й€Э            Т€о   ;ц€њ"        $њ€ц?     5г€ъ°P  RЃь€з9       Оъ€€€€€€€€ъМ          lђзъълђj      nоь€€€€€€€€€€€€€€€€€€€ьоnш€€€€€€€€€€€€€€€€€€€€€€€ъrо€€€€€€€€€€€€€€€€€€€€€оr  NЅццЅL   И€€€€€€Б L€ъnjъ€Lњ€l    j€Ѕш€    €цш€    €цњ€l    j€ЅL€ъjlш€N И€€€€€€Г   NњшшњL         БъИ              ш€ц              €€€              €€€              €€€              €€€              €€€       wт€€€€€€€€€€€€€фwъ€€€€€€€€€€€€€€€ъwц€€€€€€€€€€€€€цw       €€€              €€€              €€€              €€€              €€€              ш€ц              ъГ                        tцъ€€€€€€€€€€€ъцwъ€€€€€€€€€€€€€€€ъyц€€€€€€€€€€€€€ц{    Dе∞   w€€б ђ€€Ў$"‘€€Ѓ я€€y   ≤гA    Ы€€€€€      Ы€€€€€  €€€€€€      €€€€€€  Щ€€€€€      Щ€€€€€     €€€         т€€     €€€         т€€     €€€         т€€     €€€         т€€     €€€         т€€     €€€         т€€     €€€         т€€     €€€         т€€     €€€      5≤€€€     €€€Ѓ 5rћ€€€€€     €€€€€€€€€€€€€€€€Ы   €€€€€€€€€€€€€€€€€   €€€€€€ъф“ЃЕN€€€€Ц   €€€                 €€€                 €€€                 €€€                 €€ь                 о€т                 pшp                 a™еъ€€€€€€€≈  pр€€€€€€€€€€€√ Ы€€лt€€  €€   W€€я  €€  €€   ћ€€F   €€  €€   ь€€    €€  €€   €€€    €€  €€   ъ€€    €€  €€   ќ€€F   €€  €€   U€€я  €€  €€    И€€оw€€  €€     aр€€€€€  €€      j∞т€€  €€          €€  €€          €€  €€          €€  €€          €€  €€          €€  €€          €€  €€          €€  €€          €€  €€          €€  €€          €€  €€     ≈€€€€€€√Є€€€€≈  √шшшшшші∞шшшшЅ&ќъќ*ћ€€€ћъ€€€ъћ€€€ќ&ќъќ(   ««     €€     €€ќ$   ш€€ќ}≤_(€ъй€€€€€ђТЏшрТ  К‘шц“}   Jф€€€€€€б& т€…??…€гМ€«    ≈€ЕЏ€7      5€Ёь€        €шй€      $€лЦ€і      і€Що€Ѕ9;Ѕ€о 7й€€€€€€л9   И“ъъ“И  ≈Є     «ї       «€б.    «€б.      г€цY   г€цY      (о€€О   (о€€Р      ;ц€€њ  ;ц€€њ     N€€€г3  N€€€г3     h€€€ш_  h€€€ш_    n€€€ш_  n€€€ш_   W€€€з5  W€€€з5   Aъ€€√  Aъ€€√   0т€€Щ  0т€€Ц   й€ъa   й€ъa     Џ€з9    Џ€з5      љ≈     љ≈             nоъоn          ъ€€€ъ          nо€лl                                                        fъc            я€Ё            ъ€ъ          U€€€        rг€€€€       Hя€€€€€€      r€€€€€€йФ     =€€€€€ґN      ґ€€€∞&         ц€€Ф           ъ€€           ‘€€        ЕъГИ€€°        ш€цт€€√J $UЯ€€€ U€€€€€€€€€€€€€  Yф€€€€€€€€шЩ.   √фшц–£Y          ∞еJ                     я€€{                    "÷€€Ѓ                   Ѓ€€÷"                    {€€г                     Fе∞                                                                                       Ы€€€€€€“                €€€€€€€€?               Щ€€€€€€€ђ                  т€€€ь                 Г€€ґ€€Е                т€Ўй€л               Г€€] €€c              ф€Џ  ь€ќ              Г€€]   £€€5            ф€Џ    3€€°            Г€€_     «€ъ          ф€Џ      Y€€}          Е€€€€€€€€€€€€г        ф€€€€€€€€€€€€€[        Е€€€€€€€€€€€€€€√       ф€Џ          Э€€.      И€€a          0€€Щ     ц€Ё           √€ъ  Ц€€€€€€€Ы      Ы€€€€€€€Ыш€€€€€€€€      €€€€€€€€€Ф€€€€€€€Ц      Щ€€€€€€€Ц              Dе∞                    w€€б                  ђ€€Ў$                 "‘€€Ѓ                  я€€y                    ≤гA                                                                                         Ы€€€€€€“                €€€€€€€€?               Щ€€€€€€€ђ                  т€€€ь                 Г€€ґ€€Е                т€Ўй€л               Г€€] €€c              ф€Џ  ь€ќ              Г€€]   £€€5            ф€Џ    3€€°            Г€€_     «€ъ          ф€Џ      Y€€}          Е€€€€€€€€€€€€г        ф€€€€€€€€€€€€€[        Е€€€€€€€€€€€€€€√       ф€Џ          Э€€.      И€€a          0€€Щ     ц€Ё           √€ъ  Ц€€€€€€€Ы      Ы€€€€€€€Ыш€€€€€€€€      €€€€€€€€€Ф€€€€€€€Ц      Щ€€€€€€€Ц          ••                   Hе€€еJ                Э€€РМ€€Я             =б€Ё;  ;Ё€б=            л€Т
    
Р€о            ∞И        И∞                                                                                    Ы€€€€€€“                €€€€€€€€?               Щ€€€€€€€ђ                  т€€€ь                 Г€€ґ€€Е                т€Ўй€л               Г€€] €€c              ф€Џ  ь€ќ              Г€€]   £€€5            ф€Џ    3€€°            Г€€_     «€ъ          ф€Џ      Y€€}          Е€€€€€€€€€€€€г        ф€€€€€€€€€€€€€[        Е€€€€€€€€€€€€€€√       ф€Џ          Э€€.      И€€a          0€€Щ     ц€Ё           √€ъ  Ц€€€€€€€Ы      Ы€€€€€€€Ыш€€€€€€€€      €€€€€€€€€Ф€€€€€€€Ц      Щ€€€€€€€Ц       P√шЁr  ÷Ѕ            °€ЎDЁ÷Fћ€Ы            «÷  }еьЅL                                                                                     Ы€€€€€€“                €€€€€€€€?               Щ€€€€€€€ђ                  т€€€ь                 Г€€ґ€€Е                т€Ўй€л               Г€€] €€c              ф€Џ  ь€ќ              Г€€]   £€€5            ф€Џ    3€€°            Г€€_     «€ъ          ф€Џ      Y€€}          Е€€€€€€€€€€€€г        ф€€€€€€€€€€€€€[        Е€€€€€€€€€€€€€€√       ф€Џ          Э€€.      И€€a          0€€Щ     ц€Ё           √€ъ  Ц€€€€€€€Ы      Ы€€€€€€€Ыш€€€€€€€€      €€€€€€€€€Ф€€€€€€€Ц      Щ€€€€€€€Ц     БъГ   БъГ               ъ€ъ   ъ€ъ               ъИ   ъЕ                                                                                        Ы€€€€€€“                €€€€€€€€?               Щ€€€€€€€ђ                  т€€€ь                 Г€€ґ€€Е                т€Ўй€л               Г€€] €€c              ф€Џ  ь€ќ              Г€€]   £€€5            ф€Џ    3€€°            Г€€_     «€ъ          ф€Џ      Y€€}          Е€€€€€€€€€€€€г        ф€€€€€€€€€€€€€[        Е€€€€€€€€€€€€€€√       ф€Џ          Э€€.      И€€a          0€€Щ     ц€Ё           √€ъ  Ц€€€€€€€Ы      Ы€€€€€€€Ыш€€€€€€€€      €€€€€€€€€Ф€€€€€€€Ц      Щ€€€€€€€Ц         ≈ъ…                   «c h…                   ъ  ъ                   їt wљ                   њъЅ                                                                                        Ы€€€€€€“                €€€€€€€€?               Щ€€€€€€€ђ                  т€€€ь                 Г€€ґ€€Е                т€Ўй€л               Г€€] €€c              ф€Џ  ь€ќ              Г€€]   £€€5            ф€Џ    3€€°            Г€€_     «€ъ          ф€Џ      Y€€}          Е€€€€€€€€€€€€г        ф€€€€€€€€€€€€€[        Е€€€€€€€€€€€€€€√       ф€Џ          Э€€.      И€€a          0€€Щ     ц€Ё           √€ъ  Ц€€€€€€€Ы      Ы€€€€€€€Ыш€€€€€€€€      €€€€€€€€€Ф€€€€€€€Ц      Щ€€€€€€€Ц     rть€€€ъшшшшшшшшшшш      ъ€€€€€€€€€€€€€€€€€      nй€€€€R€€€€€€€€€€€        ъ€ъ€€€     €€€        Y€€™ €€€     €€€        Ѓ€€U €€€     €€€       
ъ€ъ €€€ lъp й€т       ]€€™  €€€ о€р nъr       ≤€€Y  €€€€€€€          ь€ъ  €€€€€€€          a€€™   €€€€€€€          ї€€[   €€€€€€ь         ь€€€€€€€€€ р€о         h€€€€€€€€€€ nъn         њ€€€€€€€€€€            €€е     €€€      wъw   n€€Р     €€€      т€ф   √€€7     €€€      €€€pль€€€ьЏf≈ъ€€€€€€€€€€€€€ш€€€€€€€€€€€€€€€€€€€€€€€lрь€€€ьЎYЁь€€€€€€€€€€€€€     P°÷цътіp Jфl    ]г€€€€€€€€оY–€о  Ѓ€€€€€€€€€€€€€€ь  ≤€€€Ўn* NЫъ€€€€ p€€€О       ћ€€€т€€Е          €€ьp€€ќ            е€рі€€[            nъpт€€               ь€€                €€€                €€€                ъ€€               ÷€€,               Ф€€К               .€€ь3          “т] Я€€оF        Ў€€ф ‘€€€ЄW  7о€€€«  ћ€€€€€€€€€€€€€б   Бф€€€€€€€€€ьЭ      [•–€€ъгђr            €€                 €€ќ$               ш€€ќ            }≤_(€ъ            й€€€€€ђ            ТЏшрТ            ∞еJ               я€€{              "÷€€Ѓ             Ѓ€€÷"              {€€г               Fе∞                                                           Ы€€€€ьшшшшшшшшшшш €€€€€€€€€€€€€€€€€ Щ€€€€€€€€€€€€€€€€    €€€        €€€    €€€        €€€    €€€        €€€    €€€        Щ€Ы    €€€   Ц€Ы         €€€   €€€         €€€€€€€€€         €€€€€€€€€         €€€€€€€€€         €€€   €€€         €€€   Щ€Ы         €€€         Ц€Ы   €€€         €€€   €€€         €€€   €€€         €€€Ы€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€€€€€          Dе∞              w€€б            ђ€€Ў$           "‘€€Ѓ            я€€y              ≤гA                                                               Ы€€€€ьшшшшшшшшшшш €€€€€€€€€€€€€€€€€ Щ€€€€€€€€€€€€€€€€    €€€        €€€    €€€        €€€    €€€        €€€    €€€        Щ€Ы    €€€   Ц€Ы         €€€   €€€         €€€€€€€€€         €€€€€€€€€         €€€€€€€€€         €€€   €€€         €€€   Щ€Ы         €€€         Ц€Ы   €€€         €€€   €€€         €€€   €€€         €€€Ы€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€€€€€        ••             Hе€€еJ          Э€€РМ€€Я       =б€Ё;  ;Ё€б=      л€Т
    
Р€о      ∞И        И∞                                                        Ы€€€€ьшшшшшшшшшшш €€€€€€€€€€€€€€€€€ Щ€€€€€€€€€€€€€€€€    €€€        €€€    €€€        €€€    €€€        €€€    €€€        Щ€Ы    €€€   Ц€Ы         €€€   €€€         €€€€€€€€€         €€€€€€€€€         €€€€€€€€€         €€€   €€€         €€€   Щ€Ы         €€€         Ц€Ы   €€€         €€€   €€€         €€€   €€€         €€€Ы€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€€€€€     БъГ   БъГ         ъ€ъ   ъ€ъ         ъИ   ъЕ                                                          Ы€€€€ьшшшшшшшшшшш €€€€€€€€€€€€€€€€€ Щ€€€€€€€€€€€€€€€€    €€€        €€€    €€€        €€€    €€€        €€€    €€€        Щ€Ы    €€€   Ц€Ы         €€€   €€€         €€€€€€€€€         €€€€€€€€€         €€€€€€€€€         €€€   €€€         €€€   Щ€Ы         €€€         Ц€Ы   €€€         €€€   €€€         €€€   €€€         €€€Ы€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€€€€€    ∞еJ            я€€{           "÷€€Ѓ          Ѓ€€÷"           {€€г            Fе∞                                                 Ы€€€€€€€€€€€€€Ы€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€Ц      €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€      Ы€€€€€€€€€€€€€Ы€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€Ц         Dе∞           w€€б         ђ€€Ў$        "‘€€Ѓ         я€€y           ≤гA                                                    Ы€€€€€€€€€€€€€Ы€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€Ц      €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€      Ы€€€€€€€€€€€€€Ы€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€Ц      ••          Hе€€еJ       Э€€РМ€€Я    =б€Ё;  ;Ё€б=   л€Т
    
Р€о   ∞И        И∞                                              Ы€€€€€€€€€€€€€Ы€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€Ц      €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€      Ы€€€€€€€€€€€€€Ы€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€Ц  БъГ   БъГ      ъ€ъ   ъ€ъ      ъИ   ъЕ                                                 Ы€€€€€€€€€€€€€Ы€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€Ц      €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€      Ы€€€€€€€€€€€€€Ы€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€Ц      P√шЁr  ÷Ѕ          °€ЎDЁ÷Fћ€Ы          «÷  }еьЅL                                                                        Ц€€€њ        Ы€€€€€€Ыш€€€€І        €€€€€€€€Ф€€€€€О       Щ€€€€€€Ц   €€€€p         €€€     €€€€€P        €€€     €€€€€ш=       €€€     €€€ф€€о*      €€€     €€€H€€€е     €€€     €€€ c€€€‘    €€€     €€€  Б€€€њ   €€€     €€€   Э€€€І   €€€     €€€    Є€€€О  €€€     €€€    ћ€€€p €€€     €€€     я€€€P€€€     €€€      "й€€ш€€€     €€€       9ц€€€€€     €€€        L€€€€€     €€€         c€€€€   Ы€€€€€€Ы       Б€€€   €€€€€€€€        Э€€   Щ€€€€€€Ц         Є€        ∞еJ                 я€€{                "÷€€Ѓ               Ѓ€€÷"                {€€г                 Fе∞                                                                         NІгшъгІP          ;÷€€€€€€€€÷;       n€€€€€€€€€€€€h     p€€€о**о€€€h   9€€€√      њ€€€7  ‘€€√        √€€“ D€€ф          т€€AЭ€€М            К€€Э–€€?            ?€€“ц€€            €€ц€€€              €€ъц€€            €€ц–€€=            ;€€“Э€€И            Г€€ЭD€€т          о€€A ‘€€√        љ€€“  ;€€€Ѕ      ї€€€9   p€€€о,  (yо€€€j     p€€€€€€€€€€€€h       ;‘€€€€€€€€÷9          JІбъъбІN                 Dе∞                w€€б              ђ€€Ў$             "‘€€Ѓ              я€€y                ≤гA                                                                            NІгшъгІP          ;÷€€€€€€€€÷;       n€€€€€€€€€€€€h     p€€€о**о€€€h   9€€€√      њ€€€7  ‘€€√        √€€“ D€€ф          т€€AЭ€€М            К€€Э–€€?            ?€€“ц€€            €€ц€€€              €€ъц€€            €€ц–€€=            ;€€“Э€€И            Г€€ЭD€€т          о€€A ‘€€√        љ€€“  ;€€€Ѕ      ї€€€9   p€€€о,  (yо€€€j     p€€€€€€€€€€€€h       ;‘€€€€€€€€÷9          JІбъъбІN              ••               Hе€€еJ            Э€€РМ€€Я         =б€Ё;  ;Ё€б=        л€Т
    
Р€о        ∞И        И∞                                                                      NІгшъгІP          ;÷€€€€€€€€÷;       n€€€€€€€€€€€€h     p€€€о**о€€€h   9€€€√      њ€€€7  ‘€€√        √€€“ D€€ф          т€€AЭ€€М            К€€Э–€€?            ?€€“ц€€            €€ц€€€              €€ъц€€            €€ц–€€=            ;€€“Э€€И            Г€€ЭD€€т          о€€A ‘€€√        љ€€“  ;€€€Ѕ      ї€€€9   p€€€о,  (yо€€€j     p€€€€€€€€€€€€h       ;‘€€€€€€€€÷9          JІбъъбІN            P√шЁr  ÷Ѕ        °€ЎDЁ÷Fћ€Ы        «÷  }еьЅL                                                                      NІгшъгІP          ;÷€€€€€€€€÷;       n€€€€€€€€€€€€h     p€€€о**о€€€h   9€€€√      њ€€€7  ‘€€√        √€€“ D€€ф          т€€AЭ€€М            К€€Э–€€?            ?€€“ц€€            €€ц€€€              €€ъц€€            €€ц–€€=            ;€€“Э€€И            Г€€ЭD€€т          о€€A ‘€€√        љ€€“  ;€€€Ѕ      ї€€€9   p€€€о,  (yо€€€j     p€€€€€€€€€€€€h       ;‘€€€€€€€€÷9          JІбъъбІN           БъГ   БъГ           ъ€ъ   ъ€ъ           ъИ   ъЕ                                                                        NІгшъгІP          ;÷€€€€€€€€÷;       n€€€€€€€€€€€€h     p€€€о**о€€€h   9€€€√      њ€€€7  ‘€€√        √€€“ D€€ф          т€€AЭ€€М            К€€Э–€€?            ?€€“ц€€            €€ц€€€              €€ъц€€            €€ц–€€=            ;€€“Э€€И            Г€€ЭD€€т          о€€A ‘€€√        љ€€“  ;€€€Ѕ      ї€€€9   p€€€о,  (yо€€€j     p€€€€€€€€€€€€h       ;‘€€€€€€€€÷9          JІбъъбІN      ]фћ      √шfт€€Ё    Ў€€ц…€€€Ё  Ў€€€…Ў€€€ЁЎ€€€я ÷€€€ЁЎ€€€я   “€€€€€€я     –€€€€я      я€€€€б     я€€€€€€б   я€€€Џ–€€€б я€€€Ў–€€€б…€€€Ў  –€€€ќф€€‘    –€€фaшњ      ≈т]                  Э                  яш      5Т–цштђc  Р€р    "ґ€€€€€€€€з[p€€Т   Hр€€€€€€€€€€€€€њ  Lь€€о}*?Щь€€€€,  $т€€√      J€€€€∞  љ€€√      Ё€€€€€&3€€ф      √€ь{€€€{Р€€Р       Ѓ€€c ∞€€∞«€€D      О€€Б  P€€йц€€     p€€І   €€ш€€€     N€€љ    €€ъц€€   9ц€‘    €€тЏ€€F  "л€з     ?€€њ•€€Ц Ё€т3      О€€МP€€ъ*«€ьH      ф€€0б€€ш€€c      √€€ґ  Y€€€€Я      ≈€€т   «€€€€∞F,р€€ьH   P€€€€€€€€€€€€€рH   9ц€Ў[Џ€€€€€€€€ґ    ќ€й PІйьцќР3      ъ€3                 Я≤                        ∞еJ                 я€€{                "÷€€Ѓ               Ѓ€€÷"                {€€г                 Fе∞                                                                   Ы€€€€€€Ы    Ы€€€€€€Ы€€€€€€€€    €€€€€€€€Щ€€€€€€Ц    Щ€€€€€€Ц  €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    ь€€          €€ъ    р€€        €€б    ™€€r        l€€Э    ?€€ц=      =т€€7     Ы€€ьР, 0Рь€€°      ї€€€€€€€€€€√
       Ц€€€€€€€€Э          .ЩЎъъЁЩ0                 Dе∞                w€€б              ђ€€Ў$             "‘€€Ѓ              я€€y                ≤гA                                                                      Ы€€€€€€Ы    Ы€€€€€€Ы€€€€€€€€    €€€€€€€€Щ€€€€€€Ц    Щ€€€€€€Ц  €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    ь€€          €€ъ    р€€        €€б    ™€€r        l€€Э    ?€€ц=      =т€€7     Ы€€ьР, 0Рь€€°      ї€€€€€€€€€€√
       Ц€€€€€€€€Э          .ЩЎъъЁЩ0              ••               Hе€€еJ            Э€€РМ€€Я         =б€Ё;  ;Ё€б=        л€Т
    
Р€о        ∞И        И∞                                                                Ы€€€€€€Ы    Ы€€€€€€Ы€€€€€€€€    €€€€€€€€Щ€€€€€€Ц    Щ€€€€€€Ц  €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    ь€€          €€ъ    р€€        €€б    ™€€r        l€€Э    ?€€ц=      =т€€7     Ы€€ьР, 0Рь€€°      ї€€€€€€€€€€√
       Ц€€€€€€€€Э          .ЩЎъъЁЩ0           БъГ   БъГ           ъ€ъ   ъ€ъ           ъИ   ъЕ                                                                  Ы€€€€€€Ы    Ы€€€€€€Ы€€€€€€€€    €€€€€€€€Щ€€€€€€Ц    Щ€€€€€€Ц  €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    ь€€          €€ъ    р€€        €€б    ™€€r        l€€Э    ?€€ц=      =т€€7     Ы€€ьР, 0Рь€€°      ї€€€€€€€€€€√
       Ц€€€€€€€€Э          .ЩЎъъЁЩ0                 Dе∞                 w€€б               ђ€€Ў$              "‘€€Ѓ               я€€y                 ≤гA                                                                          Ы€€€€шбy     Ы€€€€€€Ы€€€€€€€ъ     €€€€€€€€Щ€€€€ьеn     Щ€€€€€€Ц  “€й       й€“     ,ь€…     ќ€ь,       €€£     •€€}        –€€p   r€€–         *ш€€A A€€ъ*           w€€о=л€€w             …€€ь€€…              &ш€€€ш&               n€€€r                 €€€                  €€€                  €€€                  €€€                  €€€                  €€€              Ы€€€€€€€€€Ы          €€€€€€€€€€€          Щ€€€€€€€€€Ц          }≈цшт∞Y         9й€€€€€€€Ѕ      "р€€€€€€€€€ћ     Я€€«D nт€€w     р€€     L€€“     €€€       €€ъ     €€€      7€€й     €€€   *hй€€Г     €€€  yъ€€€€°     €€€  ъ€€€€€Г     €€€  nо€€€€€ї    €€€    Y√€€€°    €€€       Б€€€3   €€€        ≤€€Ы   €€€        ?€€б   €€€        €€ъ   €€€  3Э*    €€ш   €€€  Ў€њ   3€€Ў   €€€  ъ€ь0 ,“€€РЫ€€€€€€Ы÷€€€€€€€ш$€€€€€€€€R€€€€€€ъW Щ€€€€€€Ц ?іцьлЯ*       ∞еJ                я€€{               "÷€€Ѓ              Ѓ€€÷"               {€€г                Fе∞                                                                 RГІЅршъцбІW      з€€€€€€€€€€€÷*     Мш€€€€€€€€€€€й             H‘€€Щ               €€й                €€ь       JЩ√тшъцй≤€€€     DЏ€€€€€€€€€€€€    n€€€€€€€€€€€€€€   J€€€яw. 
=W€€€   ќ€€Я        €€€   ъ€€       *Ѓ€€€   –€€•5 H{«€€€€€   A€€€€€€€€€€€€€€€€€Ы Hй€€€€€€€€ЎL€€€€€€  pґтьцћЩD  €€€€€Ц         Dе∞               w€€б             ђ€€Ў$            "‘€€Ѓ             я€€y               ≤гA                                                                     RГІЅршъцбІW      з€€€€€€€€€€€÷*     Мш€€€€€€€€€€€й             H‘€€Щ               €€й                €€ь       JЩ√тшъцй≤€€€     DЏ€€€€€€€€€€€€    n€€€€€€€€€€€€€€   J€€€яw. 
=W€€€   ќ€€Я        €€€   ъ€€       *Ѓ€€€   –€€•5 H{«€€€€€   A€€€€€€€€€€€€€€€€€Ы Hй€€€€€€€€ЎL€€€€€€  pґтьцћЩD  €€€€€Ц       ••              Hе€€еJ           Э€€РМ€€Я        =б€Ё;  ;Ё€б=       л€Т
    
Р€о       ∞И        И∞                                                              RГІЅршъцбІW      з€€€€€€€€€€€÷*     Мш€€€€€€€€€€€й             H‘€€Щ               €€й                €€ь       JЩ√тшъцй≤€€€     DЏ€€€€€€€€€€€€    n€€€€€€€€€€€€€€   J€€€яw. 
=W€€€   ќ€€Я        €€€   ъ€€       *Ѓ€€€   –€€•5 H{«€€€€€   A€€€€€€€€€€€€€€€€€Ы Hй€€€€€€€€ЎL€€€€€€  pґтьцћЩD  €€€€€Ц    P√шЁr  ÷Ѕ       °€ЎDЁ÷Fћ€Ы       «÷  }еьЅL                                                               RГІЅршъцбІW      з€€€€€€€€€€€÷*     Мш€€€€€€€€€€€й             H‘€€Щ               €€й                €€ь       JЩ√тшъцй≤€€€     DЏ€€€€€€€€€€€€    n€€€€€€€€€€€€€€   J€€€яw. 
=W€€€   ќ€€Я        €€€   ъ€€       *Ѓ€€€   –€€•5 H{«€€€€€   A€€€€€€€€€€€€€€€€€Ы Hй€€€€€€€€ЎL€€€€€€  pґтьцћЩD  €€€€€Ц    БъГ   БъГ          ъ€ъ   ъ€ъ          ъИ   ъЕ                                                                RГІЅршъцбІW      з€€€€€€€€€€€÷*     Мш€€€€€€€€€€€й             H‘€€Щ               €€й                €€ь       JЩ√тшъцй≤€€€     DЏ€€€€€€€€€€€€    n€€€€€€€€€€€€€€   J€€€яw. 
=W€€€   ќ€€Я        €€€   ъ€€       *Ѓ€€€   –€€•5 H{«€€€€€   A€€€€€€€€€€€€€€€€€Ы Hй€€€€€€€€ЎL€€€€€€  pґтьцћЩD  €€€€€Ц     ≈ъ…              «c h…              ъ  ъ              їt wљ              њъЅ                                                                   RГІЅршъцбІW      з€€€€€€€€€€€÷*     Мш€€€€€€€€€€€й             H‘€€Щ               €€й                €€ь       JЩ√тшъцй≤€€€     DЏ€€€€€€€€€€€€    n€€€€€€€€€€€€€€   J€€€яw. 
=W€€€   ќ€€Я        €€€   ъ€€       *Ѓ€€€   –€€•5 H{«€€€€€   A€€€€€€€€€€€€€€€€€Ы Hй€€€€€€€€ЎL€€€€€€  pґтьцћЩD  €€€€€Ц   ,pІ‘цъ‘Е wћццЅU      •€€€€€€€€оpг€€€€€€Є    ъ€€€€€€€€€€€€€€€€€€≈   КцЄr;Ѓ€€€€€РГ€€€Б          €€€€p    U€€ъ          €€€√      Я€€   ,}ітьшц€€€D      *€€ћ ,≈€€€€€€€€€€€€€€€€€€€€цAш€€€€€€€€€€€€€€€€€€€€€€‘€€йГ? L€€€€€€€€€€€€€€ш€€      €€€3          ‘€€=     *€€€≈          t€€оc;Эь€€€€Є, PЦ…шоnћ€€€€€€€€€€€€€€€€€€€€€ь ї€€€€€€ц∞€€ђ€€€€€€€€лp   ?ІйьйЦ"Ў™ =їшш–Іl7     R•Џцъця™h&«рY    Hя€€€€€€€€€€€€з   t€€€€€€€€€€€€€€ь  _€€€Ўh  PЄ€€€€€ т€€І       P€€€ь }€€ќ         Ў€€з √€€U          Rзр[ ц€€               ь€€                р€€               ™€€Б           “зLF€€ьR         Џ€€з ∞€€€ЄY(  FЕр€€€б њ€€€€€€€€€€€€€€ъJ  ц€€€€€€€€€€€….     fІь€ььцяЃ}3          €€                 €€ќ$               ш€€ќ            }≤_(€ъ            й€€€€€ђ            ТЏшрТ             ∞еJ                я€€{               "÷€€Ѓ              Ѓ€€÷"               {€€г                Fе∞                                                                     ?Ц–цъцќТ;        0ћ€€€€€€€€€ћ9     hь€€€€€€€€€€€€[   a€€€фК; ;Кф€€ь7 ц€€–       ћ€€÷М€€б         я€€a‘€€A           ?€€≤ъ€€€€€€€€€€€€€€€€€тъ€€€€€€€€€€€€€€€€€шЏ€€€€€€€€€€€€€€€€€ъТ€€F               &ь€оF               y€€€ЄY&  ?WМітцy  М€€€€€€€€€€€€€€€ш   Uб€€€€€€€€€€€€йc    P°–цьшфЏ≤Эa7           Dе∞               w€€б             ђ€€Ў$            "‘€€Ѓ             я€€y               ≤гA                                                                        ?Ц–цъцќТ;        0ћ€€€€€€€€€ћ9     hь€€€€€€€€€€€€[   a€€€фК; ;Кф€€ь7 ц€€–       ћ€€÷М€€б         я€€a‘€€A           ?€€≤ъ€€€€€€€€€€€€€€€€€тъ€€€€€€€€€€€€€€€€€шЏ€€€€€€€€€€€€€€€€€ъТ€€F               &ь€оF               y€€€ЄY&  ?WМітцy  М€€€€€€€€€€€€€€€ш   Uб€€€€€€€€€€€€йc    P°–цьшфЏ≤Эa7         ••              Hе€€еJ           Э€€РМ€€Я        =б€Ё;  ;Ё€б=       л€Т
    
Р€о       ∞И        И∞                                                                 ?Ц–цъцќТ;        0ћ€€€€€€€€€ћ9     hь€€€€€€€€€€€€[   a€€€фК; ;Кф€€ь7 ц€€–       ћ€€÷М€€б         я€€a‘€€A           ?€€≤ъ€€€€€€€€€€€€€€€€€тъ€€€€€€€€€€€€€€€€€шЏ€€€€€€€€€€€€€€€€€ъТ€€F               &ь€оF               y€€€ЄY&  ?WМітцy  М€€€€€€€€€€€€€€€ш   Uб€€€€€€€€€€€€йc    P°–цьшфЏ≤Эa7      БъГ   БъГ          ъ€ъ   ъ€ъ          ъИ   ъЕ                                                                   ?Ц–цъцќТ;        0ћ€€€€€€€€€ћ9     hь€€€€€€€€€€€€[   a€€€фК; ;Кф€€ь7 ц€€–       ћ€€÷М€€б         я€€a‘€€A           ?€€≤ъ€€€€€€€€€€€€€€€€€тъ€€€€€€€€€€€€€€€€€шЏ€€€€€€€€€€€€€€€€€ъТ€€F               &ь€оF               y€€€ЄY&  ?WМітцy  М€€€€€€€€€€€€€€€ш   Uб€€€€€€€€€€€€йc    P°–цьшфЏ≤Эa7    ∞еJ            я€€{           "÷€€Ѓ          Ѓ€€÷"           {€€г            Fе∞                                                   Ы€€€€€€€       €€€€€€€€       Щ€€€€€€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€      Ы€€€€€€€€€€€€€Ы€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€Ц      Dе∞           w€€б         ђ€€Ў$        "‘€€Ѓ         я€€y           ≤гA                                                        Ы€€€€€€€       €€€€€€€€       Щ€€€€€€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€      Ы€€€€€€€€€€€€€Ы€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€Ц    ••          Hе€€еJ       Э€€РМ€€Я    =б€Ё;  ;Ё€б=   л€Т
    
Р€о   ∞И        И∞                                                 Ы€€€€€€€       €€€€€€€€       Щ€€€€€€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€      Ы€€€€€€€€€€€€€Ы€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€Ц  БъГ   БъГ      ъ€ъ   ъ€ъ      ъИ   ъЕ                                                  Ы€€€€€€€       €€€€€€€€       Щ€€€€€€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€      Ы€€€€€€€€€€€€€Ы€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€Ц    P√шЁr  ÷Ѕ       °€ЎDЁ÷Fћ€Ы       «÷  }еьЅL                                                              Ы€€€€ М‘цъеІJ     €€€€€cш€€€€€€€љ   Щ€€€€€€€€€€€€€€ћ    €€€€лy, 
]г€€p    €€€°      3€€≈    €€€         €€ц    €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€  Ы€€€€€Ы     Ы€€€€€Ы€€€€€€€     €€€€€€€Щ€€€€€Ц     Щ€€€€€Ц      ∞еJ               я€€{              "÷€€Ѓ             Ѓ€€÷"              {€€г               Fе∞                                                                NІгшшб•P        AЏ€€€€€€€€ЏH     p€€€€€€€€€€€€w   Y€€€еt$ "tй€€€] л€€Ѕ      √€€оr€€Џ        Ё€€wЅ€€_          ]€€√ц€€          €€цъ€€           €€ъя€€7          7€€яФ€€≈        √€€Ц&ь€€∞      ∞€€ь( Г€€€бp$  $nг€€€М   Я€€€€€€€€€€€€£     cй€€€€€€€€зc       _ђгььг™]              Dе∞              w€€б            ђ€€Ў$           "‘€€Ѓ            я€€y              ≤гA                                                                    NІгшшб•P        AЏ€€€€€€€€ЏH     p€€€€€€€€€€€€w   Y€€€еt$ "tй€€€] л€€Ѕ      √€€оr€€Џ        Ё€€wЅ€€_          ]€€√ц€€          €€цъ€€           €€ъя€€7          7€€яФ€€≈        √€€Ц&ь€€∞      ∞€€ь( Г€€€бp$  $nг€€€М   Я€€€€€€€€€€€€£     cй€€€€€€€€зc       _ђгььг™]           ••             Hе€€еJ          Э€€РМ€€Я       =б€Ё;  ;Ё€б=      л€Т
    
Р€о      ∞И        И∞                                                              NІгшшб•P        AЏ€€€€€€€€ЏH     p€€€€€€€€€€€€w   Y€€€еt$ "tй€€€] л€€Ѕ      √€€оr€€Џ        Ё€€wЅ€€_          ]€€√ц€€          €€цъ€€           €€ъя€€7          7€€яФ€€≈        √€€Ц&ь€€∞      ∞€€ь( Г€€€бp$  $nг€€€М   Я€€€€€€€€€€€€£     cй€€€€€€€€зc       _ђгььг™]        P√шЁr  ÷Ѕ      °€ЎDЁ÷Fћ€Ы      «÷  }еьЅL                                                               NІгшшб•P        AЏ€€€€€€€€ЏH     p€€€€€€€€€€€€w   Y€€€еt$ "tй€€€] л€€Ѕ      √€€оr€€Џ        Ё€€wЅ€€_          ]€€√ц€€          €€цъ€€           €€ъя€€7          7€€яФ€€≈        √€€Ц&ь€€∞      ∞€€ь( Г€€€бp$  $nг€€€М   Я€€€€€€€€€€€€£     cй€€€€€€€€зc       _ђгььг™]        БъГ   БъГ         ъ€ъ   ъ€ъ         ъИ   ъЕ                                                                NІгшшб•P        AЏ€€€€€€€€ЏH     p€€€€€€€€€€€€w   Y€€€еt$ "tй€€€] л€€Ѕ      √€€оr€€Џ        Ё€€wЅ€€_          ]€€√ц€€          €€цъ€€           €€ъя€€7          7€€яФ€€≈        √€€Ц&ь€€∞      ∞€€ь( Г€€€бp$  $nг€€€М   Я€€€€€€€€€€€€£     cй€€€€€€€€зc       _ђгььг™]           БъГ              ъ€ъ              ГъБ                                                                                            wцъ€€€€€€€€€€€ъцyъ€€€€€€€€€€€€€€€ъwц€€€€€€€€€€€€€цw                                                                                            БъГ              ъ€ъ              ГъБ                     ЃцИ     H°яшъгІPґ€€Ё   ;÷€€€€€€€€ф€€p  c€€€€€€€€€€€€Ц   N€€€еp jъ€€€€P е€€Ѕ    €€€€€оj€€Џ    p€€Т]€€€Бљ€€Y    _€€°  y€€‘ц€€   R€€ђ   €€ъь€€  Hъ€ґ   €€цг€€f =ц€њ    f€€√Э€€фpт€…    й€€r*€€€€€÷    9е€€з }€€€€p PЃ€€€€L  ц€€€€€€€€€€€ь]  ‘€йі€€€€€€€€«.   ї€о0 =•льцћР9     ш€R               Оќ                     ∞еJ                 я€€{                "÷€€Ѓ               Ѓ€€÷"                {€€г                 Fе∞                                                                    Ы€€€€€      Ы€€€€€  €€€€€€      €€€€€€  Щ€€€€€      Щ€€€€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     ц€€      =љ€€€     ≤€€ђ 7t“€€€€€     .ъ€€€€€€€€€€€€€€Ы    Nф€€€€€€€–F€€€€€     Р÷ъъ÷Ы?  €€€€Ц           Dе∞                w€€б              ђ€€Ў$             "‘€€Ѓ              я€€y                ≤гA                                                                      Ы€€€€€      Ы€€€€€  €€€€€€      €€€€€€  Щ€€€€€      Щ€€€€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     ц€€      =љ€€€     ≤€€ђ 7t“€€€€€     .ъ€€€€€€€€€€€€€€Ы    Nф€€€€€€€–F€€€€€     Р÷ъъ÷Ы?  €€€€Ц        ••               Hе€€еJ            Э€€РМ€€Я         =б€Ё;  ;Ё€б=        л€Т
    
Р€о        ∞И        И∞                                                                Ы€€€€€      Ы€€€€€  €€€€€€      €€€€€€  Щ€€€€€      Щ€€€€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     ц€€      =љ€€€     ≤€€ђ 7t“€€€€€     .ъ€€€€€€€€€€€€€€Ы    Nф€€€€€€€–F€€€€€     Р÷ъъ÷Ы?  €€€€Ц   БъГ   БъГ           ъ€ъ   ъ€ъ           ъИ   ъЕ                                                                    Ы€€€€€      Ы€€€€€  €€€€€€      €€€€€€  Щ€€€€€      Щ€€€€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     ц€€      =љ€€€     ≤€€ђ 7t“€€€€€     .ъ€€€€€€€€€€€€€€Ы    Nф€€€€€€€–F€€€€€     Р÷ъъ÷Ы?  €€€€Ц             Dе∞                   w€€б                 ђ€€Ў$                "‘€€Ѓ                 я€€y                   ≤гA                                                                                Ц€€€€€€Ы       Ы€€€€€€Ыш€€€€€€€       €€€€€€€€Ф€€€€€€Ц       Щ€€€€€€Ц  
з€т          “€Ѕ      h€€Щ         f€€A       ÷€€*       е€«        L€€Ѕ       Г€€F         Ѕ€€N     ш€ќ          .€€Ё    •€€N           °€€w   3€€“            ш€т  ћ€€Y             Г€€Щ Y€€Ў              л€€3б€€_               r€€ц€€Џ                Џ€€€€h                 [€€€б                 P€€€l                  {€€г                  ™€€r                   Ё€й             Ы€€€€€€€€€€Ы           €€€€€€€€€€€€           ЩшшшшшшшшшшЦ             БъГ   БъГ              ъ€ъ   ъ€ъ              ъИ   ъЕ                                                                              Ц€€€€€€Ы       Ы€€€€€€Ыш€€€€€€€       €€€€€€€€Ф€€€€€€Ц       Щ€€€€€€Ц  
з€т          “€Ѕ      h€€Щ         f€€A       ÷€€*       е€«        L€€Ѕ       Г€€F         Ѕ€€N     ш€ќ          .€€Ё    •€€N           °€€w   3€€“            ш€т  ћ€€Y             Г€€Щ Y€€Ў              л€€3б€€_               r€€ц€€Џ                Џ€€€€h                 [€€€б                 P€€€l                  {€€г                  ™€€r                   Ё€й             Ы€€€€€€€€€€Ы           €€€€€€€€€€€€           ЩшшшшшшшшшшЦ              ґр.      ,рЄ            ÷€І      °€÷            F€€£5 ,£€€H             aь€€€€€€ьa               *ЫяъъяЫ*                                                                                      Ы€€€€€€“                €€€€€€€€?               Щ€€€€€€€ђ                  т€€€ь                 Г€€ґ€€Е                т€Ўй€л               Г€€] €€c              ф€Џ  ь€ќ              Г€€]   £€€5            ф€Џ    3€€°            Г€€_     «€ъ          ф€Џ      Y€€}          Е€€€€€€€€€€€€г        ф€€€€€€€€€€€€€[        Е€€€€€€€€€€€€€€√       ф€Џ          Э€€.      И€€a          0€€Щ     ц€Ё           √€ъ  Ц€€€€€€€Ы      Ы€€€€€€€Ыш€€€€€€€€      €€€€€€€€€Ф€€€€€€€Ц      Щ€€€€€€€Ц   ґр.      ,рЄ       ÷€І      °€÷       F€€£5 ,£€€H        aь€€€€€€ьa          *ЫяъъяЫ*                                                                RГІЅршъцбІW      з€€€€€€€€€€€÷*     Мш€€€€€€€€€€€й             H‘€€Щ               €€й                €€ь       JЩ√тшъцй≤€€€     DЏ€€€€€€€€€€€€    n€€€€€€€€€€€€€€   J€€€яw. 
=W€€€   ќ€€Я        €€€   ъ€€       *Ѓ€€€   –€€•5 H{«€€€€€   A€€€€€€€€€€€€€€€€€Ы Hй€€€€€€€€ЎL€€€€€€  pґтьцћЩD  €€€€€Ц      Ы€€€€€€“                  €€€€€€€€?                 Щ€€€€€€€ђ                    т€€€ь                   Г€€ґ€€Е                  т€Ўй€л                 Г€€] €€c                ф€Џ  ь€ќ                Г€€]   £€€5              ф€Џ    3€€°              Г€€_     «€ъ            ф€Џ      Y€€}            Е€€€€€€€€€€€€г          ф€€€€€€€€€€€€€[          Е€€€€€€€€€€€€€€√         ф€Џ          Э€€.        И€€a          0€€Щ       ц€Ё           √€ъ    Ц€€€€€€€Ы      Ы€€€€€€€Ы  ш€€€€€€€€      €€€€€€€€€  Ф€€€€€€€Ц      Щ€€€€€€€Џ                      ќ€Ф                      ь€                       л€5÷њ                    €€€€√                     }зцІ RГІЅршъцбІW       з€€€€€€€€€€€÷*      Мш€€€€€€€€€€€й              H‘€€Щ                €€й                 €€ь        JЩ√тшъцй≤€€€      DЏ€€€€€€€€€€€€     n€€€€€€€€€€€€€€    J€€€яw. 
=W€€€    ќ€€Я        €€€    ъ€€       *Ѓ€€€    –€€•5 H{«€€€€€    A€€€€€€€€€€€€€€€€€Ы  Hй€€€€€€€€ЎL€€€€€€   pґтьцћЩD  €€€€€Ц               ќ€Ф                ь€                 л€5÷њ              €€€€√               }зцІ            Dе∞               w€€б             ђ€€Ў$            "‘€€Ѓ             я€€y               ≤гA                                                                      P°÷цътіp Jфl    ]г€€€€€€€€оY–€о  Ѓ€€€€€€€€€€€€€€ь  ≤€€€Ўn* NЫъ€€€€ p€€€О       ћ€€€т€€Е          €€ьp€€ќ            е€рі€€[            nъpт€€               ь€€                €€€                €€€                ъ€€               ÷€€,               Ф€€К               .€€ь3          “т] Я€€оF        Ў€€ф ‘€€€ЄW  7о€€€«  ћ€€€€€€€€€€€€€б   Бф€€€€€€€€€ьЭ      [•–цьъгђr               Dе∞               w€€б             ђ€€Ў$            "‘€€Ѓ             я€€y               ≤гA                                                                     R•Џцъця™h&«рY    Hя€€€€€€€€€€€€з   t€€€€€€€€€€€€€€ь  _€€€Ўh  PЄ€€€€€ т€€І       P€€€ь }€€ќ         Ў€€з √€€U          Rзр[ ц€€               ь€€                р€€               ™€€Б           “зLF€€ьR         Џ€€з ∞€€€ЄY(  FЕр€€€б њ€€€€€€€€€€€€€€ъJ  ц€€€€€€€€€€€….     fІ–цььцяЃ}3        ∞Р        И≤       л€Ц    Р€л       =б€Ё;  9Џ€г=        Я€€МИ€€°           Jе€€зJ              °°                                                                    P°÷цътіp Jфl    ]г€€€€€€€€оY–€о  Ѓ€€€€€€€€€€€€€€ь  ≤€€€Ўn* NЫъ€€€€ p€€€О       ћ€€€т€€Е          €€ьp€€ќ            е€рі€€[            nъpт€€               ь€€                €€€                €€€                ъ€€               ÷€€,               Ф€€К               .€€ь3          “т] Я€€оF        Ў€€ф ‘€€€ЄW  7о€€€«  ћ€€€€€€€€€€€€€б   Бф€€€€€€€€€ьЭ      [•–цьъгђr       ∞Р        И≤       л€Ц    Р€л       =б€Ё;  9Џ€г=        Я€€МИ€€°           Jе€€зJ              °°                                                                    R•Џцъця™h&«рY    Hя€€€€€€€€€€€€з   t€€€€€€€€€€€€€€ь  _€€€Ўh  PЄ€€€€€ т€€І       P€€€ь }€€ќ         Ў€€з √€€U          Rзр[ ц€€               ь€€                р€€               ™€€Б           “зLF€€ьR         Џ€€з ∞€€€ЄY(  FЕр€€€б њ€€€€€€€€€€€€€€ъJ  ц€€€€€€€€€€€….     fІ–цььцяЃ}3       ∞Р        И≤        л€Ц    Р€л        =б€Ё;  9Џ€г=         Я€€МИ€€°            Jе€€зJ               °°                                                                    Ы€€€шшшшшшцбІ[     €€€€€€€€€€€€€€лp    Щ€€€€€€€€€€€€€€€∞    €€€     &f–€€€Ц    €€€         К€€€P   €€€          Ы€€б  €€€          е€€a  €€€           l€€ђ  €€€           €€о  €€€            €€ш  €€€            €€€  €€€            €€€  €€€           €€ц  €€€           ,€€Џ  €€€           €€Я  €€€          &т€€?  €€€         7е€€ї   €€€      LЭ€€€й Ы€€€€€€€€€€€€€€€г,  €€€€€€€€€€€€€€€•   Щ€€€€€€€€€ърґЕ,                Ы€€€€€  ЭъФ           €€€€€€  ъ€ц           Щ€€€€€  Тьт              €€€   0≤              €€€  ћY              €€€  Ф–     hітъц“ЭD €€€  Џ,    Wз€€€€€€€€“€€€       {€€€€€€€€€€€€€€      _€€€‘_ 
W–€€€€€     т€€ђ     •€€€€     {€€–       –€€€     ≈€€W         W€€€     ц€€         €€€     ь€€           €€€     р€€         €€€     ™€€         }€€€     H€€шF       Fъ€€€      ≤€€ьЦ= =Фь€€€€      …€€€€€€€€€€€€€€€€Ы    
Ц€€€€€€€€€€€€€€€€      $БљтьътїФL€€€€€Ц   Ы€€€шшшшшшцбІ[      €€€€€€€€€€€€€€лl     Щ€€€€€€€€€€€€€€€™      €€€     &f–€€€Ц     €€€         И€€€L    €€€          Щ€€я   €€€          я€€a   €€€           h€€ђ   €€€           €€оЦ€€€€€€€€Ы        €€шш€€€€€€€€€        €€€Ф€€€€€€€€Ц        €€€   €€€           €€ц   €€€           ,€€Џ   €€€           €€Я   €€€          $т€€?   €€€         3е€€ї    €€€      LЩь€€й  Ы€€€€€€€€€€€€€€€г,   €€€€€€€€€€€€€€€•    Щ€€€€€€€€€ърЄГ*                Ы€€€€€               €€€€€€               Щ€€€€€              (   €€€   (          €€€€€€€€€€€          (   €€€   (    hітъц“ЭD €€€       Wз€€€€€€€€“€€€      {€€€€€€€€€€€€€€     _€€€‘_ 
W–€€€€€    т€€ђ     •€€€€    {€€–       –€€€    ≈€€W         W€€€    ц€€         €€€    ь€€           €€€    р€€         €€€    ™€€         }€€€    H€€шF       Fъ€€€     ≤€€ьЦ= =Фь€€€€     …€€€€€€€€€€€€€€€€Ы   
Ц€€€€€€€€€€€€€€€€     $БљтьътїФL€€€€€Ц Ы€€€€ьшшшшшшшшшшш   €€€€€€€€€€€€€€€€€   Щ€€€€€€€€€€€€€€€€      €€€        €€€      €€€        €€€      €€€        €€€      €€€        Щ€Ы      €€€   Ц€Ы           €€€   €€€           €€€€€€€€€           €€€€€€€€€           €€€€€€€€€           €€€   €€€           €€€   Щ€Ы           €€€         Ц€Ы     €€€         €€€     €€€         €€€     €€€         €€€  Ы€€€€€€€€€€€€€€€€€  €€€€€€€€€€€€€€€€€€  Щ€€€€€€€€€€€€€€€€€                ќ€Ф                ь€                 л€5÷њ              €€€€√               }зцІ     ?Ц–цъцќТ;        0ћ€€€€€€€€€ћ9     hь€€€€€€€€€€€€[   a€€€фК; ;Кф€€ь7 ц€€–       ћ€€÷М€€б         я€€a‘€€A           ?€€≤ъ€€€€€€€€€€€€€€€€€тъ€€€€€€€€€€€€€€€€€шЏ€€€€€€€€€€€€€€€€€ъТ€€F               &ь€оF               y€€€ЄY&  ?WМітцy  М€€€€€€€€€€€€€€€ш   Uб€€€€€€€€€€€€йc    P°б€€ъфЏ≤Эa7        ќ€Ф               ь€                л€5÷њ             €€€€√              }зцІ          ∞Р        И≤      л€Ц    Р€л      =б€Ё;  9Џ€г=       Я€€МИ€€°          Jе€€зJ             °°                                                            Ы€€€€ьшшшшшшшшшшш €€€€€€€€€€€€€€€€€ Щ€€€€€€€€€€€€€€€€    €€€        €€€    €€€        €€€    €€€        €€€    €€€        Щ€Ы    €€€   Ц€Ы         €€€   €€€         €€€€€€€€€         €€€€€€€€€         €€€€€€€€€         €€€   €€€         €€€   Щ€Ы         €€€         Ц€Ы   €€€         €€€   €€€         €€€   €€€         €€€Ы€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€€€€€    ∞Р        И≤       л€Ц    Р€л       =б€Ё;  9Џ€г=        Я€€МИ€€°           Jе€€зJ              °°                                                                     ?Ц–цъцќТ;        0ћ€€€€€€€€€ћ9     hь€€€€€€€€€€€€[   a€€€фК; ;Кф€€ь7 ц€€–       ћ€€÷М€€б         я€€a‘€€A           ?€€≤ъ€€€€€€€€€€€€€€€€€тъ€€€€€€€€€€€€€€€€€шЏ€€€€€€€€€€€€€€€€€ъТ€€F               &ь€оF               y€€€ЄY&  ?WМітцy  М€€€€€€€€€€€€€€€ш   Uб€€€€€€€€€€€€йc    P°–цьшфЏ≤Эa7  Ы€€€€€€€       €€€€€€€€       Щ€€€€€€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€      Ы€€€€€€€€€€€€€Ы€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€Ц        Dе∞               w€€б             ђ€€Ў$            "‘€€Ѓ             я€€y               ≤гA                                                                     Ы€€€€€€€€€Ы        €€€€€€€€€€€        Щ€€€€€€€€€Ц            €€€                €€€                €€€                €€€                €€€                €€€                €€€                €€€                €€€                €€€                €€€         Ц€Ы    €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€Ы€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€€€€€€         Dе∞           w€€б         ђ€€Ў$        "‘€€Ѓ         я€€y           ≤гA                                      Ы€€€€€€€       €€€€€€€€       Щ€€€€€€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€      Ы€€€€€€€€€€€€€Ы€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€ЦЫ€€€€€€€€€Ы   Lе€зL€€€€€€€€€€€   е€€€гЩ€€€€€€€€€Ц   л€€€т    €€€       Lб€€Ѓ    €€€         R€A    €€€        Hц£     €€€       c€ќ     €€€       ÷њ      €€€                €€€                €€€                €€€                €€€                €€€         Ц€Ы    €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€Ы€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€€€€€€ Ы€€€€€€€  ЭъТ  €€€€€€€€  ъ€ц  Щ€€€€€€€  Тьт       €€€   0≤       €€€  ћY       €€€  Ф–        €€€  Џ,        €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€      Ы€€€€€€€€€€€€€Ы€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€Ц Ы€€€€€€€€€Ы         €€€€€€€€€€€         Щ€€€€€€€€€Ц             €€€                 €€€                 €€€                 €€€ &їц}            €€€{ъ€€ф            €€€€€€≤7            €€€€Ў9            *–€€€Г           Гь€€€€            ,÷€€ь€€€            г€€“.€€€         Ц€Ыш€Ц €€€         €€€wЎ  €€€         €€€     €€€         €€€     €€€         €€€ Ы€€€€€€€€€€€€€€€€€€ €€€€€€€€€€€€€€€€€€€ Щ€€€€€€€€€€€€€€€€€€ Ы€€€€€€€       €€€€€€€€       Щ€€€€€€€            €€€            €€€            €€€            €€€rяоp       €€€÷€€€л       €€€€€еМ&     w€€€ьЕ
      DЎ€€€€$      ≤€€€€€€       Ў€€ш€€€       т€я" €€€       aцD  €€€            €€€            €€€            €€€            €€€      Ы€€€€€€€€€€€€€Ы€€€€€€€€€€€€€€€Щ€€€€€€€€€€€€€Ц             Dе∞                  w€€б                ђ€€Ў$               "‘€€Ѓ                я€€y                  ≤гA                                                                            Ц€€€њ        Ы€€€€€€Ыш€€€€І        €€€€€€€€Ф€€€€€О       Щ€€€€€€Ц   €€€€p         €€€     €€€€€P        €€€     €€€€€ш=       €€€     €€€ф€€о*      €€€     €€€H€€€е     €€€     €€€ c€€€‘    €€€     €€€  Б€€€њ   €€€     €€€   Э€€€І   €€€     €€€    Є€€€О  €€€     €€€    ћ€€€p €€€     €€€     я€€€P€€€     €€€      "й€€ш€€€     €€€       9ц€€€€€     €€€        L€€€€€     €€€         c€€€€   Ы€€€€€€Ы       Б€€€   €€€€€€€€        Э€€   Щ€€€€€€Ц         Є€             Dе∞               w€€б             ђ€€Ў$            "‘€€Ѓ             я€€y               ≤гA                                                                  Ы€€€€ М‘цъеІJ     €€€€€cш€€€€€€€љ   Щ€€€€€€€€€€€€€€ћ    €€€€лy, 
]г€€p    €€€°      3€€≈    €€€         €€ц    €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€  Ы€€€€€Ы     Ы€€€€€Ы€€€€€€€     €€€€€€€Щ€€€€€Ц     Щ€€€€€Ц     ∞Р        И≤          л€Ц    Р€л          =б€Ё;  9Џ€г=           Я€€МИ€€°              Jе€€зJ                 °°                                                                           Ц€€€њ        Ы€€€€€€Ыш€€€€І        €€€€€€€€Ф€€€€€О       Щ€€€€€€Ц   €€€€p         €€€     €€€€€P        €€€     €€€€€ш=       €€€     €€€ф€€о*      €€€     €€€H€€€е     €€€     €€€ c€€€‘    €€€     €€€  Б€€€њ   €€€     €€€   Э€€€І   €€€     €€€    Є€€€О  €€€     €€€    ћ€€€p €€€     €€€     я€€€P€€€     €€€      "й€€ш€€€     €€€       9ц€€€€€     €€€        L€€€€€     €€€         c€€€€   Ы€€€€€€Ы       Б€€€   €€€€€€€€        Э€€   Щ€€€€€€Ц         Є€      ∞Р        И≤       л€Ц    Р€л       =б€Ё;  9Џ€г=        Я€€МИ€€°           Jе€€зJ              °°                                                                Ы€€€€ М‘цъеІJ     €€€€€cш€€€€€€€љ   Щ€€€€€€€€€€€€€€ћ    €€€€лy, 
]г€€p    €€€°      3€€≈    €€€         €€ц    €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€  Ы€€€€€Ы     Ы€€€€€Ы€€€€€€€     €€€€€€€Щ€€€€€Ц     Щ€€€€€Ц         ,я≤  .яЄ           .й€г .л€б          .л€л35о€л.         0л€й.7о€й.          е€й, е€й.           ґя(  ґя*                                                                        NІгшъгІP          ;÷€€€€€€€€÷;       n€€€€€€€€€€€€h     p€€€о**о€€€h   9€€€√      њ€€€7  ‘€€√        √€€“ D€€ф          т€€AЭ€€М            К€€Э–€€?            ?€€“ц€€            €€ц€€€              €€ъц€€            €€ц–€€=            ;€€“Э€€И            Г€€ЭD€€т          о€€A ‘€€√        љ€€“  ;€€€Ѕ      ї€€€9   p€€€о,  (yо€€€j     p€€€€€€€€€€€€h       ;‘€€€€€€€€÷9          JІбъъбІN              ,я≤  .яЄ         .й€г .л€б        .л€л35о€л.       0л€й.7о€й.        е€й, е€й.         ґя(  ґя*                                                                NІгшшб•P        AЏ€€€€€€€€ЏH     p€€€€€€€€€€€€w   Y€€€еt$ "tй€€€] л€€Ѕ      √€€оr€€Џ        Ё€€wЅ€€_          ]€€√ц€€          €€цъ€€           €€ъя€€7          7€€яФ€€≈        √€€Ц&ь€€∞      ∞€€ь( Г€€€бp$  $nг€€€М   Я€€€€€€€€€€€€£     cй€€€€€€€€зc       _ђгььг™]          AЫ–ць€€€€€€€€€€€€     5ќ€€€€€€€€€€€€€€€€€    {€€€€€€€€€€€€€€€€€€€   М€€€оКH €€€      €€€  U€€€Є    €€€      €€€ б€€«     €€€      €€€ L€€ъ$      €€€      Щ€Ы Я€€Ы       €€€   Ц€Ы    –€€H       €€€   €€€    ф€€       €€€€€€€€€    ш€€        €€€€€€€€€    ц€€       €€€€€€€€€    –€€H       €€€   €€€    Э€€Ы       €€€   Щ€Ы    F€€ь$      €€€           Ў€€…     €€€       Ц€Ы H€€€њ    €€€       €€€  И€€€фРL €€€       €€€   Е€€€€€€€€€€€€€€€€€€€€    =“€€€€€€€€€€€€€€€€€€      ;Т«ць€€€€€€€€€€€€€   {Ўштђ9   (ТЁът™;      ,б€€€€€€К {ь€€€€€€£   з€€€€€€€€ќ€€€€€€€€€љ  Ѓ€€йA Dе€€€€€€МК€€€Г *€€ь7   .ь€€€€    w€€ьО€€Ы     Ф€€€«      Ѕ€€Еќ€€7     5€€€7      3€€ќц€€     €€€€€€€€€€€€€цш€€     €€€€€€€€€€€€€€е€€7     5€€€€€€€€€€€€€€І€€Я     Ц€€€&          J€€€7   3ь€€€™           Ѕ€€йJ ;е€€€€€£(UЩ…шзc е€€€€€€€€л€€€€€€€€€€€ц  "÷€€€€€€І
°€€€€€€€€€лr   w“ъцґL   ;°бьцЎ™r9            Dе∞                  w€€б                ђ€€Ў$               "‘€€Ѓ                я€€y                  ≤гA                                                                              Ы€€€€€€€€€ьцеђj      €€€€€€€€€€€€€€€фr     Щ€€€€€€€€€€€€€€€€Ц       €€€      PЅ€€€R      €€€         О€€Ѕ      €€€         €€ц      €€€         *€€ф      €€€        3я€€Э      €€€    .hњ€€€я      €€€€€€€€€€€€€ї       €€€€€€€€€€€…N         €€€€€€€€€€€c          €€€    М€€€P         €€€      t€€ц0        €€€       О€€б       €€€        љ€€°       €€€        й€€F      €€€         F€€Ё  Ы€€€€€€€Ы       Ц€€€€Ы€€€€€€€€€       б€€€€Щ€€€€€€€Ц        L€€€Ц         Dе∞               w€€б             ђ€€Ў$            "‘€€Ѓ             я€€y               ≤гA                                                                     Ы€€€€€   ќъцљ]   €€€€€€ Бш€€€€€€ќ Щ€€€€€3÷€€€€€€€€€ќ    €€€ь€€фЦ?Aо€€о    €€€€т    9геP    €€€К              €€€                €€€                €€€                €€€                €€€                €€€                €€€            Ы€€€€€€€€€€€€Ы     €€€€€€€€€€€€€€     Щ€€€€€€€€€€€€Ц        ∞Р        И≤          л€Ц    Р€л          =б€Ё;  9Џ€г=           Я€€МИ€€°              Jе€€зJ                 °°                                                                             Ы€€€€€€€€€ьцеђj      €€€€€€€€€€€€€€€фr     Щ€€€€€€€€€€€€€€€€Ц       €€€      PЅ€€€R      €€€         О€€Ѕ      €€€         €€ц      €€€         *€€ф      €€€        3я€€Э      €€€    .hњ€€€я      €€€€€€€€€€€€€ї       €€€€€€€€€€€…N         €€€€€€€€€€€c          €€€    М€€€P         €€€      t€€ц0        €€€       О€€б       €€€        љ€€°       €€€        й€€F      €€€         F€€Ё  Ы€€€€€€€Ы       Ц€€€€Ы€€€€€€€€€       б€€€€Щ€€€€€€€Ц        L€€€Ц   ∞Р        И≤       л€Ц    Р€л       =б€Ё;  9Џ€г=        Я€€МИ€€°           Jе€€зJ              °°                                                                  Ы€€€€€   ќъцљ]   €€€€€€ Бш€€€€€€ќ Щ€€€€€3÷€€€€€€€€€ќ    €€€ь€€фЦ?Aо€€о    €€€€т    9геP    €€€К              €€€                €€€                €€€                €€€                €€€                €€€                €€€            Ы€€€€€€€€€€€€Ы     €€€€€€€€€€€€€€     Щ€€€€€€€€€€€€Ц               Dе∞             w€€б           ђ€€Ў$          "‘€€Ѓ           я€€y             ≤гA                                                               (Мћцъо°. tшМ    Мь€€€€€€€pе€т   •€€€€€€€€€€€€ь  ]€€€њP ;Ц€€€€  «€€Т       Dь€€  ъ€€        њ€ь  ш€€(        Т€о  ∞€€÷*       ,оw  5€€€€“Э[9        l€€€€€€€ь…К0      Fћ€€€€€€€€€≤      $_£«ш€€€€€≈          ,f“€€€r             Ы€€ћБт;          €€цф€І          €€ц€€ф9        ґ€€√€€€€њc( ;з€€€a€€€€€€€€€€€€€€€• о€фRе€€€€€€€€тn  jц fђтьшбђh             Dе∞            w€€б          ђ€€Ў$         "‘€€Ѓ          я€€y            ≤гA                                                          fђршшеђ_Pфn   Aе€€€€€€€€€€ф  ;ъ€€€€€€€€€€€€  ≈€€ЅP  DИл€ц  ь€€       [шИ  р€€лЯ]J        Ц€€€€€€€ш…ЫH    б€€€€€€€€€€–(   }ќь€€€€€€€€й     =Uj°Ѕъ€€€ЫcцЅ       И€€тз€€t        €€ш€€€€їW  ,j÷€€≤€€€€€€€€€€€€€€й"з€€€€€€€€€€€€ї aшИ7ИЄтььфЅР7       (Мћцъо°. tшМ    Мь€€€€€€€pе€т   •€€€€€€€€€€€€ь  ]€€€њP ;Ц€€€€  «€€Т       Dь€€  ъ€€        њ€ь  ш€€(        Т€о  ∞€€÷*       ,оw  5€€€€“Э[9        l€€€€€€€ь…К0      Fћ€€€€€€€€€≤      $_£«ш€€€€€≈          ,f“€€€r             Ы€€ћБт;          €€цф€І          €€ц€€ф9        ґ€€√€€€€њc( ;з€€€a€€€€€€€€€€€€€€€• о€фRе€€€€€€€€тn  jц fђт€€бђh           €€               €€ќ$             ш€€ќ          }≤_(€ъ          й€€€€€ђ          ТЏшрТ        fђршшеђ_Pфn   Aе€€€€€€€€€€ф  ;ъ€€€€€€€€€€€€  ≈€€ЅP  DИл€ц  ь€€       [шИ  р€€лЯ]J        Ц€€€€€€€ш…ЫH    б€€€€€€€€€€–(   }ќь€€€€€€€€й     =Uj°Ѕъ€€€ЫcцЅ       И€€тз€€t        €€ш€€€€їW  ,j÷€€≤€€€€€€€€€€€€€€й"з€€€€€€€€€€€€ї aшИ7ИЄт€€фЅР7          €€              €€ќ$            ш€€ќ         }≤_(€ъ         й€€€€€ђ         ТЏшрТ        ∞Р        И≤     л€Ц    Р€л     =б€Ё;  9Џ€г=      Я€€МИ€€°         Jе€€зJ            °°                                                             (Мћцъо°. tшМ    Мь€€€€€€€pе€т   •€€€€€€€€€€€€ь  ]€€€њP ;Ц€€€€  «€€Т       Dь€€  ъ€€        њ€ь  ш€€(        Т€о  ∞€€÷*       ,оw  5€€€€“Э[9        l€€€€€€€ь…К0      Fћ€€€€€€€€€≤      $_£«ш€€€€€≈          ,f“€€€r             Ы€€ћБт;          €€цф€І          €€ц€€ф9        ґ€€√€€€€њc( ;з€€€a€€€€€€€€€€€€€€€• о€фRе€€€€€€€€тn  jц fђтьшбђh     ∞Р        И≤    л€Ц    Р€л    =б€Ё;  9Џ€г=     Я€€МИ€€°        Jе€€зJ           °°                                                         fђршшеђ_Pфn   Aе€€€€€€€€€€ф  ;ъ€€€€€€€€€€€€  ≈€€ЅP  DИл€ц  ь€€       [шИ  р€€лЯ]J        Ц€€€€€€€ш…ЫH    б€€€€€€€€€€–(   }ќь€€€€€€€€й     =Uj°Ѕъ€€€ЫcцЅ       И€€тз€€t        €€ш€€€€їW  ,j÷€€≤€€€€€€€€€€€€€€й"з€€€€€€€€€€€€ї aшИ7ИЄтььфЅР7   €€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€     €€€     €€€€€€     €€€     €€€€€€     €€€     €€€€€€     €€€     €€€Щ€Ы     €€€     Щ€Ы        €€€                €€€                €€€                €€€                €€€                €€€                €€€                €€€                €€€                €€€            Ы€€€€€€€€€Ы        €€€€€€€€€€€        Щ€€€€€€€€€Ц                                                  Lз€йU              з€€€й              л€€€ф              FЁш€Є                N€N               ?тЃ               ]ъќ               –њ           pъl               л€й               €€ь               €€€               €€€               €€€            Ы€€€€€€€€€€€€€Ы   €€€€€€€€€€€€€€€   Щ€€€€€€€€€€€€€Ц      €€€               €€€               €€€               €€€               €€€               €€€               €€€               €€€               ш€€
       U“фa   Є€€І, =w‘€€€ф   9€€€€€€€€€€€€€ґ    Wц€€€€€€€€€б{     ГЅфьшЏ™r5                                             Э€М               ъ€ц               Мшт                0∞               –R               Ц–                Ў,              ∞Р        И≤       л€Ц    Р€л       =б€Ё;  9Џ€г=        Я€€МИ€€°           Jе€€зJ              °°                                                                €€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€     €€€     €€€€€€     €€€     €€€€€€     €€€     €€€€€€     €€€     €€€Щ€Ы     €€€     Щ€Ы        €€€                €€€                €€€                €€€                €€€                €€€                €€€                €€€                €€€                €€€            Ы€€€€€€€€€Ы        €€€€€€€€€€€        Щ€€€€€€€€€Ц                     ЭъФ   pъl           ъ€ц   л€й           Тьт   €€ь            0≤   €€€           ћU   €€€           Ф–    €€€           Џ* Ы€€€€€€€€€€€€€Ы     €€€€€€€€€€€€€€€     Щ€€€€€€€€€€€€€Ц        €€€                 €€€                 €€€                 €€€                 €€€                 €€€                 €€€                 €€€                 ш€€
       U“фa     Є€€І, =w‘€€€ф     9€€€€€€€€€€€€€ґ      Wц€€€€€€€€€б{       ГЅфьшЏ™r5            ≈ъ…               «c h…               ъ  ъ               їt wљ               њъЅ                                                                    Ы€€€€€€Ы    Ы€€€€€€Ы€€€€€€€€    €€€€€€€€Щ€€€€€€Ц    Щ€€€€€€Ц  €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    ь€€          €€ъ    р€€        €€б    ™€€r        l€€Э    ?€€ц=      =т€€7     Ы€€ьР, 0Рь€€°      ї€€€€€€€€€€√
       Ц€€€€€€€€Э          .ЩЎъъЁЩ0             ≈ъ…               «c h…               ъ  ъ               їt wљ               њъЅ                                                                    Ы€€€€€      Ы€€€€€  €€€€€€      €€€€€€  Щ€€€€€      Щ€€€€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     ц€€      =љ€€€     ≤€€ђ 7t“€€€€€     .ъ€€€€€€€€€€€€€€Ы    Nф€€€€€€€–F€€€€€     Р÷ъъ÷Ы?  €€€€Ц         ,я≤  .яЄ           .й€г .л€б          .л€л35о€л.         0л€й.7о€й.          е€й, е€й.           ґя(  ґя*                                                                  Ы€€€€€€Ы    Ы€€€€€€Ы€€€€€€€€    €€€€€€€€Щ€€€€€€Ц    Щ€€€€€€Ц  €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    ь€€          €€ъ    р€€        €€б    ™€€r        l€€Э    ?€€ц=      =т€€7     Ы€€ьР, 0Рь€€°      ї€€€€€€€€€€√
       Ц€€€€€€€€Э          .ЩЎъъЁЩ0                ,я≤  .яЄ           .й€г .л€б          .л€л35о€л.         0л€й.7о€й.          е€й, е€й.           ґя(  ґя*                                                                 Ы€€€€€      Ы€€€€€  €€€€€€      €€€€€€  Щ€€€€€      Щ€€€€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     €€€         €€€     ц€€      =љ€€€     ≤€€ђ 7t“€€€€€     .ъ€€€€€€€€€€€€€€Ы    Nф€€€€€€€–F€€€€€     Р÷ъъ÷Ы?  €€€€Ц      БъГ   БъГ            ъ€ъ   ъ€ъ            ъИ   ъЕ                                                                     Ы€€€€шбy     Ы€€€€€€Ы€€€€€€€ъ     €€€€€€€€Щ€€€€ьеn     Щ€€€€€€Ц  “€й       й€“     ,ь€…     ќ€ь,       €€£     •€€}        –€€p   r€€–         *ш€€A A€€ъ*           w€€о=л€€w             …€€ь€€…              &ш€€€ш&               n€€€r                 €€€                  €€€                  €€€                  €€€                  €€€                  €€€              Ы€€€€€€€€€Ы          €€€€€€€€€€€          Щ€€€€€€€€€Ц               Dе∞             w€€б           ђ€€Ў$          "‘€€Ѓ           я€€y             ≤гA                                                            €€€€€€€€€€€€€€€  €€€€€€€€€€€€€€€  €€€€€€€€€€€€€€€  €€€        p€€М  €€€       L€€∞   €€€      5ц€«   €€€     й€я    Щ€Ы    ‘€о&           љ€ъ=            £€€[            }€€}            Y€€Я            ?ъ€љ           &о€Ў      Ц€Ы  я€й       €€€ «€ф5        €€€ Ѓ€€P         €€€М€€r          €€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€         Dе∞            w€€б          ђ€€Ў$         "‘€€Ѓ          я€€y            ≤гA                                                        €€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€        Ѓ€тRЩ€Ы       *Џ€÷$          Yф€•
         Т€ьn          ≈€з9          Aй€љ          t€€И          Ѓ€рN          *Џ€“$       Ц€ЫYф€•
        €€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€      ≈ъ…            «€€€≈            ъ€€€ъ            «€€€≈            «ъ«                                                          €€€€€€€€€€€€€€€  €€€€€€€€€€€€€€€  €€€€€€€€€€€€€€€  €€€        p€€М  €€€       L€€∞   €€€      5ц€«   €€€     й€я    Щ€Ы    ‘€о&           љ€ъ=            £€€[            }€€}            Y€€Я            ?ъ€љ           &о€Ў      Ц€Ы  я€й       €€€ «€ф5        €€€ Ѓ€€P         €€€М€€r          €€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€      ≈ъ…           «€€€≈           ъ€€€ъ           «€€€≈           «ъ«                                                     €€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€        Ѓ€тRЩ€Ы       *Џ€÷$          Yф€•
         Т€ьn          ≈€з9          Aй€љ          t€€И          Ѓ€рN          *Џ€“$       Ц€ЫYф€•
        €€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€  ∞Р        И≤     л€Ц    Р€л     =б€Ё;  9Џ€г=      Я€€МИ€€°         Jе€€зJ            °°                                                           €€€€€€€€€€€€€€€  €€€€€€€€€€€€€€€  €€€€€€€€€€€€€€€  €€€        p€€М  €€€       L€€∞   €€€      5ц€«   €€€     й€я    Щ€Ы    ‘€о&           љ€ъ=            £€€[            }€€}            Y€€Я            ?ъ€љ           &о€Ў      Ц€Ы  я€й       €€€ «€ф5        €€€ Ѓ€€P         €€€М€€r          €€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€ ∞Р        И≤    л€Ц    Р€л    =б€Ё;  9Џ€г=     Я€€МИ€€°        Jе€€зJ           °°                                                       €€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€        Ѓ€тRЩ€Ы       *Џ€÷$          Yф€•
         Т€ьn          ≈€з9          Aй€љ          t€€И          Ѓ€рN          *Џ€“$       Ц€ЫYф€•
        €€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€         tћъшеІ9        …€€€€€€р       …€€€€€€шЕ       j€€рH            √€€f             ш€€             €€€              €€€         lйь€€€€€€€ьйl    ъ€€€€€€€€€€€ъ    lз€€€€€€€€€зj         €€€              €€€              €€€              €€€              €€€              €€€              €€€              €€€              €€€              €€ш             €€Ё             {€€Ы           Uш€€0       Еъ€€€€€€Т        о€€€€€€І         0Ігшц√Y              ••       Hе€€еJ    Э€€РМ€€Я =б€Ё;  ;Ё€б=л€Т
    
Р€о∞И        И∞∞Р        И≤л€Ц    Р€л=б€Ё;  9Џ€г= Я€€МИ€€°    Jе€€зJ       °°    ґр.      ,рЄ÷€І      °€÷F€€£5 ,£€€H aь€€€€€€ьa   *ЫяъъяЫ*  ≈ъ…«€€€≈ъ€€€ъ«€€€≈«ъ«≈ъ…«c h…ъ  ъїt wљњъЅ lл≈  U€€÷  ќ€Ф  ь€   л€5÷њ€€€€√ }зцІ P√шЁr  ÷Ѕ°€ЎDЁ÷Fћ€Ы«÷  }еьЅL    ,я≤  .яЄ  .й€г .л€б .л€л35о€л.0л€й.7о€й. е€й, е€й.  ґя(  ґя*        cђзъъгІY         cз€€€€€€€€зf      •€€€€€€€€€€€€Ѓ    ≤€€€÷h"  c“€€€ґ   y€€€Г       w€€€t ш€€y          l€€фt€€ћ            ≈€€pі€€[            Y€€іт€€            €€ть€€              €€ъъ€€            €€ц“€€0            .€€‘Р€€И            Е€€Р(€€ш3          3ш€€* О€€оL        Nо€€О   Ѓ€€€√_  _√€€€∞     {ш€€€€  €€€€ш{   љ«  Фф€€  €€фФ  «іь€€€€€€€€  €€€€€€€€ъ€€€€€€€€€  €€€€€€€€€€€€€€€€€€  €€€€€€€€€rль€€€€€€€€€€€€€ьлpъ€€€€€€€€€€€€€€€€€ъРь€€€€€€€€€€€€€€€шО  €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€    €€€         €€€  Рь€€€оw     wр€€€ъМъ€€€€€ъ     ъ€€€€€ъБш€€€шБ     Бш€€€шБnль€€€€€€€€€€€€€ьоnъ€€€€€€€€€€€€€€€€€ъnл€€€€€€€€€€€€€€€лlnоь€€€€€€€€€€€€€€€€€€ьоnц€€€€€€€€€€€€€€€€€€€€€€ъlо€€€€€€€€€€€€€€€€€€€€рr∞€€€€$  "ш€€€r   €€€њ   Ё€€ь   N€€€[    і€€І    $ь€ш    Е€€N    
е€Ы     Y€е      ґЅ  $€€€€∞  r€€€ш  њ€€€  ь€€я  Y€€€N   І€€ґ   ъ€ъ$   L€€Г    Щ€е    е€Y     √ґ        "€€€€∞  p€€€ъ"  љ€€€Б  ь€€б  W€€€U   І€€Ѕ   т€ь*   A€€О    О€й    Џ€h     …њ      ∞€€€€$  ∞€€€€$  "ш€€€t  "ш€€€t   €€€√   €€€√   я€€€  я€€€   P€€€h   P€€€h    њ€€ґ    њ€€ґ    *ь€ь   *ь€ь    М€€[    М€€[    з€І    з€І     a€ц     a€ц      Ѕ∞      њ≤  &€€€€∞  &€€€€∞  t€€€ш"  t€€€ш"  ≈€€€Б   ≈€€€Б  €€€я  €€€я  h€€€R   h€€€R   ї€€Ѕ    ї€€Ѕ   ь€ь*   ь€ь*   Y€€М    Y€€М    ђ€з    ђ€з    ш€a     ш€a     і√      іњ        &€€€€∞  &€€€€∞  t€€€ш"  t€€€ш"  ≈€€€   ≈€€€  €€€я  €€€я  h€€€R   h€€€R   љ€€ї    љ€€ї   ь€ь&   ь€ь&   ]€€К    ]€€К    ∞€е    ∞€е    ш€]     ш€]     Ѓї      ЃЄ            yъw            ц€ф            €€€            €€€            €€€            €€€      nйь€€€€€€€€€ьйlъ€€€€€€€€€€€€€ъnл€€€€€€€€€€€лp      €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€€            €€ь            о€л            rшn            lъn            л€о            €€ь            €€€            €€€            €€€            €€€      nль€€€€€€€€€ьйnъ€€€€€€€€€€€€€ъnй€€€€€€€€€€€лn      €€€            €€€            €€€            €€€            €€€      nо€€€€€€€€€€ьлnъ€€€€€€€€€€€€€ъnй€€€€€€€€€€€лl      €€€            €€€            €€€            €€€            €€ь            л€л            lшn        $ЫлъйЩ"   Fф€€€€€фH "т€€€€€€€ф$Ы€€€€€€€€€Ый€€€€€€€€€йъ€€€€€€€€€ъй€€€€€€€€€йЫ€€€€€€€€€Щ"т€€€€€€€т" Fф€€€€€тD   "ЫйъзЩ"  ЕъГ   БъГ   БъГъ€ъ   ъ€ъ   ъ€ъГъЕ   ъБ   ъБ МффО              Ѕ€€€€≈             }€е((е€             Ё€=  A€я             ъ€    €ъ             Ё€D  D€я             }€й..з€             Ѕ€€€€≈      AГ«я  МтфР
    ,p≤ш€€€й"        [Іл€€€ьЅ=     PЦЎ€€€€ЎФP        т€€€йІ[            ї∞p.                   КтфО   МтфМ     њ€€€€≈ Ѕ€€€€√    }€е*,й€ }€е((з€    Ё€D  J€я я€D  H€Ё    ъ€    €ъ ъ€    €ъ    Ё€D  ?€Ё Ё€D  ?€Ё    }€е,,г€ }€е*,е€    њ€€€€≈ Ѕ€€€€√     КтфР   МффО        ђ…      &Ў€‘     Nр€й$    Г€€т5   ї€€ьF   0б€€€W   ]ш€€€n    ]ц€€€p     3б€€€Y     љ€€€H      М€€ц=      Uт€о.      ,Џ€Ё       љЅ≈Є       «€б.      г€цY      (о€€О      ;ц€€њ     N€€€г3     h€€€ш_    n€€€ш_   W€€€з5   Aъ€€√   0т€€Ц   й€ъa     Џ€з5      љ«                    •ъИ           ш€€ш          Rя€€€т[        *Ѕ€€€€°       Оь€€€ќ3        fз€€€лf        5–€€€ьМ       °€€€€њ(        cф€€€ЁP          ъ€€ц           Иъ•                   Y•яцътіn Hфl     aз€€€€€€€€оY–€й   І€€€€€€€€€€€€€€ь   °€€€Ўl( AКт€€€€  H€€€И       …€€€ я€€Б          €€ь j€€≈            г€л Є€€U            cъr«€€€€€€€€€€€€€ь…    Ё€€€€€€€€€€€€€ъЭ     €€€                 €€€                …€€€€€€€€€€€€ьћ     ‘€€€€€€€€€€€€шЫ      ™€€w                J€€т$          ћт]  Ѕ€€е7        Ў€€ф  й€€€≤R  7о€€€«   &Ё€€€€€€€€€€€€€Ё    Оъ€€€€€€€€€ьЩ       h™Ёц€ъйЃt   ш€€€€€€€€€€€€Щ    Э€€€йш€€€€€€€€€€€€ъ  ъ€€€зш€  €€  €€ €€€}  {€€€  ш€  €€  €€ €€€гг€€€  ш€  €€  €€ €€ґ€[W€Є€€  го  €€  зй €€H€«√€L€€      €€     €€ Ё€€г €€      €€     €€ w€€} €€      €€     €€ ъь €€      €€     €€    €€   г€€€€€€йй€€€€й  й€€€€й г€€€€€€зз€€€€з  з€€€€з       rґтшц…Г         Yр€€€€€€€ф]       h€€€€€€€€€€€Y     5€€€€°33ґ€€т    ћ€€€Б      «€€r    ф€€ќ       W€€і    [рб7       €€р                €€ш        [Я≤гтшъ€€ъ     t–€€€€€€€€€€т   Ръ€€€€€€€€€€€€…  =й€€€€ЎРR*  {€€Я ;ц€€€•5       ≈€€Yе€€г7        (€€ь}€€й         °€€≤ “€€c         A€€€9 ъ€€        5о€€≤  ц€€      tц€€й  і€€«3  LРе€€€т7   9€€€€€€€€€€€€Ё3     Wш€€€€€€€€тБ
       Мћц€ц–•Y               0€€3                Ы€€Э               ъшфъ              {€ЦР€}             г€*(€г            Y€њ  ї€[            √€N  J€≈           0€я    Ё€3          Ы€w    r€Э         ъш    цъ        {€Ц      Ф€}       г€*      (€г      Y€њ        љ€[      √€N        J€≈     0€я          Ё€3    Ы€t          t€Э   ъш          цъ  {€Ц            Ц€} г€(            (€гY€€€€€€€€€€€€€€€€€€[√€€€€€€€€€€€€€€€€€€≈Еш€€€€€€€€€€€€€€€€шГъ€€€€€€€€€€€€€€€€€€ъpт€€€€€€€€€€€€€€€€тr  €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€    €€€          €€€  wш€€€ш{      wш€€€шyъ€€€€€ъ      ъ€€€€€ъГцшшшцГ      ЕцшшшцГ€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€‘€й            ь€€&т€њ            т€ц Y€€Е           nъy  Ы€€H               ‘€о               &т€…               Y€€Р                Ы€€U               “€т$               $т€‘               Y€€Э               Y€€•              $т€Џ             ‘€ш.              Ы€€c              Y€€•              (ф€÷
             
Ў€ф&              •€€[          fъp c€€Э           з€т*ц€‘           ь€ьЏ€т&            €€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€€шшшшшшшшшшшшшшшшшшш                 т                 Aґ                 ]Ы                 Фa                 ЃJ                 б                ф                 7њ                 W°                 Еp                 ђN                 ÷                ц                 (ћ     rќ?        P•  fњ€€€ќ        }w  ФЭR,ь€€[       ІP      К€€я      …(      р€€w      ц       w€€о    ‘        б€€К    PІ         ]€€ъ   nЕ          ќ€€І   °U          ?€€€3  љ3           і€€√  ц           *€€€Lб             Щ€€÷JЃ             ф€€≈Т              €€€W              
з€€?               l€€               ÷о                 LЃ                  a      ЁшоЭ(    3£тшбМ 
«€€€€€ьl  }€€€€€€ЎЕ€–0 yц€К€лp 5Ў€Рб€*    ,г€€÷    .€йъ€      3€€3      €ъй€*    (б€€е"    (€йТ€ќ* }ф€Е{€оn (ћ€ОЎ€€€€€€w  _ц€€€€€Ё Меьо°.    "ФзъзР              f…шцїL             ≤шNл€€М            Ф€t  ≤€€€_          ,€ъ  Я€€€“          Э€і   W€€€ъ         ф€Г   т€€÷         ?€€P    Lрб=         p€€D                 °€€                 ≤€€                 “€€                  р€€                  ц€€                  ш€€                  €€€                  €€€                  €€€                  €€€                  €€€                  €€€                  €€€                  €€ь                  €€ш                  €€ш                  €€т                  €€й                 €€ї                 
€€ђ                 &€€Ц         3Џл?    J€€W         ћ€€г   ]€€         ъ€€€F   Ц€“          л€€€}   ћ€j          Ы€€€І  (€Ё          "ц€€ї  љъ7            Lш€шЫцN              $ЯффЯ$                j–шЁt           ≈€€€€€Є        я€€€€€€€Є    ‘рaљ€€€«О€€€Ф  
–€€цт€€Ў  Ы€€€ИЄ€€€љ[р‘    Ѓ€€€€€€€я        ђ€€€€€–           jЏьЎ}    n–шя{          …€€€€€љ        б€€€€€€€љ   ÷рcљ€€€«Р€€€Э  ÷€€цт€€Џ  Ы€€€РЄ€€€љ[р‘    ≤€€€€€€€я        ≤€€€€€ќ           pЏьЎ{                і°                 €ъ                 К€€&               "ь€е               ™€€a               3€€“                «€€A     nоь€€€€€€€€€€€€€€ьоnъ€€€€€€€€€€€€€€€€€€ъnл€€€€€€€€€€€€€€€€лp        л€ц               И€€Б               ъ€з        nоь€€€€€€€€€€€€€€ьйnъ€€€€€€€€€€€€€€€€€€ъnо€€€€€€€€€€€€€€€€лp     N€€њ               Ў€€.               l€€£               з€ъ               &€€Е                 ь€                 Ыі                               W“шr               3≈€€€р             •€€€ш°0           Бъ€€€•            hз€€€«3            A“€€€яU            (ґ€€€т{           Рь€€€Ф           wр€€€ї*            RЁ€€€‘H             М€€€€Ѕ               RЁ€€€÷L               tо€€€ї*               Рь€€€Щ               &≤€€€ф}               A“€€€б[                cе€€€ќ7               
ш€€€™"               °€€€ъ™5               0≈€€€т                 R“цr                      nрш€€€€€€€€€€€€€€шрp ъ€€€€€€€€€€€€€€€€€€ъ pрь€€€€€€€€€€€€€€ьрntц≈=                ц€€€•              ;≤€€€фw              0…€€€÷F               ]е€€€≤"              Кь€€ш              (љ€€€яN               Nя€€€љ*              ш€€ьМ              "Ѓ€€€з]               Б€€€€Ф            ђ€€€е[           ш€€ьИ           Nя€€€љ(           $Є€€€яN           Иъ€€ш           [е€€€∞           *√€€€÷D           ;≤ь€€тt            ц€€€£              wш√7                                    jрш€€€€€€€€€€€€€€шрpъ€€€€€€€€€€€€€€€€€€ъlоь€€€€€€€€€€€€€€ьрn       c&             Ё≈             ™F™n           LІ б         
“  [ї         Мh    ≤c       .√     б      …*      f≤     rБ        їY   ќ        "я  ≤?          nІ UЭ            ≈N‘5            A“cі            їW √P          f∞  *г        я   ГМ        іa    Ё*      ]Є      Jћ     я       Ѓh    ђj        б  Rњ          r• я"           –?°r            0е«              Т&            DітьтђL  €€€        М€€€€€€€И €€€       ]€€€€€€€€ц €€€       ќ€€О;е€€Џ           ь€€  "Ўя;           €€€               Ы€€€€€€€Ы   Ы€€€€€   €€€€€€€€€   €€€€€€   Щ€€€€€€€Ц   Щ€€€€€      €€€         €€€      €€€         €€€      €€€         €€€      €€€         €€€      €€€         €€€      €€€         €€€      €€€         €€€      €€€         €€€      €€€         €€€      €€€         €€€   Ы€€€€€€€Ы   Ы€€€€€€€Ы€€€€€€€€€   €€€€€€€€€Щ€€€€€€€Ц   Щ€€€€€€€Ц    }бъцљnш€€€€      ї€€€€€€€€€€€€      y€€€€€€€–ш€€€€      ÷€€jЃ€€3  €€€      ь€€ ґ∞   €€€      €€€        €€€   Ы€€€€€€€Ы     €€€   €€€€€€€€€     €€€   Щ€€€€€€€Ц     €€€      €€€        €€€      €€€        €€€      €€€        €€€      €€€        €€€      €€€        €€€      €€€        €€€      €€€        €€€      €€€        €€€      €€€        €€€      €€€        €€€   Ы€€€€€€€Ы  Ы€€€€€€€Ы€€€€€€€€€  €€€€€€€€€Щ€€€€€€€Ц  Щ€€€€€€€Ц                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        