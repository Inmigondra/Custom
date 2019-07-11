// This simple code allow you to send data from Arduino to Unity3D.

// uncomment "NATIVE_USB" if you're using ARM CPU (Arduino DUE, Arduino M0, ..)
#define SerialPort NATIVE_USB

// uncomment "SERIAL_USB" if you're using non ARM CPU (Arduino Uno, Arduino Mega, ..)
#define SERIAL_USB


const int CombatPin =2;
const int UtilitaryPin =3;
const int MotorPin =4;
const int InfosPin =5;

const int CombatLEDPin = 6;
const int UtilitaryLEDPin = 7;
const int MotorLEDPin = 8;
const int InfosLEDPin =9;

//Analogue Potar
const int CoordonneePotLeftPin = 0;
const int CoordonneePotMidPin = 1;
const int CoordonneePotRightPin = 2;

const int TypeLedLeftPin = 10;
const int TypeLedMidPin = 11;
const int TypeLedRightPin = 12;

//Repair
const int CoordonneeFixElecPin = 13; 
const int CoordonneeFixElecLedPin = 22;

const int CoordonneeFixMecaPin = 23;
const int CoordonneeFixMecaLedPin = 24;

const int CoordonneeElecOnLedPin = 25;
const int CoordonneeHazardLedPin = 26;

//shield
const int PotarShieldPin = 3; //analog
const int LevierFirePin = 27;
const int LevierFireLedPin = 28;
const int JackShieldPin = 29;
const int JackWeaponLedPin = 30;
const int JackShieldLedPin = 31;
const int ButtonTypeAWeaponPin = 32;
const int ButtonTypeBWeaponPin = 33;
const int WeaponLedPin = 34;
const int RepairBattlePin = 35;
const int RepairBattleLedPin = 36;
const int ChargeWeaponDTPin = 37;
const int ChargeWeaponClockPin = 38;
int ChargeWeaponClockLast;  
int ChargeWeaponClockValue;
boolean bCW;

const int ChargeWeaponLedPin = 39;
const int WeaponEnergyLedPin = 40;
const int WeaponHazardLedPin = 41;



int CombatPinButtonState = 0;
int UtilitaryPinButtonState = 0;
int MotorPinButtonState = 0;
int InfosPinButtonState = 0;

int CoordonneePotLeftValue =0;
int CoordonneePotMidValue =0;
int CoordonneePotRightValue =0;

int CoordonneeFixElecButtonState =0;

int CoordonneeFixMecaButtonState =0;


int PotarShieldValue =0;
int LevierFireButtonState =0;
int JackShieldButtonState =0;
int ButtonTypeAWeaponButtonState =0;
int ButtonTypeBWeaponButtonState =0;
int RepairBattleButtonState =0;
int ChargeWeaponDTButtonState =0;
int ChargeWeaponClockButtonState =0;


int VoiceSensorButtonState =0;
//int LanguetteButtonState =0;



volatile unsigned int CRNValue = 0;


//String TextToSend = String( );
void setup() {
  #ifdef NATIVE_USB
    SerialUSB.begin(1); //Baudrate is irevelant for Native USB
  #endif

  #ifdef SERIAL_USB
    Serial.begin(250000); // You can choose any baudrate, just need to also change it in Unity.
    while (!Serial); // wait for Leonardo enumeration, others continue immediately
  #endif

  // Module Energy
  pinMode(CombatPin, INPUT_PULLUP);
  pinMode(UtilitaryPin, INPUT_PULLUP);
  pinMode(MotorPin, INPUT_PULLUP);
  pinMode(InfosPin, INPUT_PULLUP);

  
  pinMode (CombatLEDPin, OUTPUT);
  pinMode (UtilitaryLEDPin, OUTPUT);
  pinMode (MotorLEDPin, OUTPUT);
  pinMode (InfosLEDPin, OUTPUT);




  // Module Coordonnée
  pinMode (CoordonneeFixElecPin, INPUT_PULLUP);
  pinMode (CoordonneeFixMecaPin, INPUT_PULLUP);

  pinMode (TypeLedLeftPin, OUTPUT);
  pinMode (TypeLedMidPin, OUTPUT);
  pinMode (TypeLedRightPin, OUTPUT);
  pinMode (CoordonneeFixElecLedPin, OUTPUT);
  pinMode (CoordonneeFixMecaLedPin,OUTPUT);
  pinMode (CoordonneeElecOnLedPin, OUTPUT);
  pinMode (CoordonneeHazardLedPin, OUTPUT);

  //Module Armement
  pinMode (LevierFirePin, INPUT_PULLUP);
  pinMode (LevierFireLedPin, OUTPUT);
  pinMode (JackShieldPin, INPUT_PULLUP);
  pinMode (JackWeaponLedPin, OUTPUT);
  pinMode (JackShieldLedPin,OUTPUT);
  pinMode (ButtonTypeAWeaponPin, INPUT_PULLUP);
  pinMode (ButtonTypeBWeaponPin, INPUT_PULLUP);
  pinMode (WeaponLedPin, OUTPUT);
  pinMode (RepairBattlePin, INPUT_PULLUP);
  pinMode (RepairBattleLedPin, OUTPUT);
  pinMode (ChargeWeaponDTPin, INPUT);
  pinMode (ChargeWeaponClockPin,INPUT);
  ChargeWeaponClockLast = digitalRead(ChargeWeaponClockPin);
  
  pinMode (ChargeWeaponLedPin, OUTPUT);
  pinMode (WeaponHazardLedPin, OUTPUT);
  




  // Test de Travail
  //pinMode (Languette, INPUT_PULLUP);

  //inMode(VoiceSensor, INPUT);


  //pinMode (ClockPin, INPUT);
  //digitalWrite (ClockPin, HIGH);
  //pinMode (DtPin, HIGH);
  
}

// Run forever
void loop() {

  ProcessEncoder();

  
  String TextToSend = "";

  CoordonneePotLeftValue = analogRead (CoordonneePotLeftPin);
  TextToSend = TextToSend + " ValPotState: " + CoordonneePotLeftValue;


  ButtonTypeAWeaponButtonState = digitalRead (ButtonTypeAWeaponPin);
  if (ButtonTypeAWeaponButtonState == LOW){
    TextToSend = TextToSend + " ChooseWeaponOne";
  }else{
    TextToSend = TextToSend + " ChooseWeaponTwo";
  }

  RepairBattleButtonState = digitalRead (RepairBattlePin);
  if (RepairBattleButtonState == LOW){
    
    TextToSend = TextToSend + " RepairBattleOn";
  }else{
    
    TextToSend = TextToSend + " RepairBattleOff";
  }
  
  JackShieldButtonState = digitalRead (JackShieldPin);
  if (JackShieldButtonState == LOW){
    TextToSend = TextToSend + " ShieldConnected";
  }else{
    TextToSend = TextToSend + " ShieldDisconnected";
  }

  LevierFireButtonState = digitalRead (LevierFirePin);
  if (LevierFireButtonState == LOW){
    TextToSend = TextToSend + " FireWeaponOn";
  }else{
    TextToSend = TextToSend + " FireWeaponOff";
  }

  if (CoordonneePotLeftValue < 341){
    digitalWrite (TypeLedLeftPin, LOW);
    digitalWrite (TypeLedMidPin, LOW);
    digitalWrite (TypeLedRightPin, HIGH);
  }else if (CoordonneePotLeftValue > 341 && CoordonneePotLeftValue < 682){
    digitalWrite (TypeLedLeftPin, LOW);
    digitalWrite (TypeLedMidPin, HIGH);
    digitalWrite (TypeLedRightPin, LOW);
  }else{
    digitalWrite (TypeLedLeftPin, HIGH);
    digitalWrite (TypeLedMidPin, LOW);
    digitalWrite (TypeLedRightPin, LOW);
  }

  PotarShieldValue = analogRead (PotarShieldPin);
  TextToSend = TextToSend + " ShieldOrientation: " + PotarShieldValue;

  CoordonneePotMidValue = analogRead (CoordonneePotMidPin);
  TextToSend = TextToSend + " ValPotX: " + CoordonneePotMidValue;

  CoordonneePotRightValue = analogRead (CoordonneePotRightPin);
  TextToSend = TextToSend + " ValPotY: " + CoordonneePotRightValue;


  CoordonneeFixElecButtonState = digitalRead(CoordonneeFixElecPin);

  if (CoordonneeFixElecButtonState == LOW){
      digitalWrite (CoordonneeFixElecLedPin, HIGH);
      TextToSend = TextToSend + " CoordonneeRepairElecOn";
    
  }else{
    digitalWrite (CoordonneeFixElecLedPin, LOW);
    TextToSend = TextToSend + " CoordonneeRepairElecOn";
  }

  CoordonneeFixMecaButtonState = digitalRead (CoordonneeFixMecaPin);
  
  if (CoordonneeFixMecaButtonState == LOW){
      digitalWrite (CoordonneeFixMecaLedPin, HIGH);
      TextToSend = TextToSend + " CoordonneeRepairMecaOn";
    
  }else{
    digitalWrite (CoordonneeFixMecaLedPin, LOW);
    TextToSend = TextToSend + " CoordonneeRepairMecaOn";
  }
  



  


  
  //sendData("Hello World!");
  CombatPinButtonState = digitalRead(CombatPin);
  UtilitaryPinButtonState = digitalRead(UtilitaryPin);
  MotorPinButtonState = digitalRead(MotorPin);
  InfosPinButtonState = digitalRead(InfosPin);

  
  //VoiceSensorButtonState = digitalRead (VoiceSensor);
  //LanguetteButtonState = digitalRead (Languette);

  /*if (LanguetteButtonState == HIGH) {
    TextToSend = TextToSend + " LanguetteOff";
  }else{
    TextToSend = TextToSend + " LanguetteOn";
  }*/
  if (CombatPinButtonState == HIGH) {
    digitalWrite (CombatLEDPin, LOW);
    //sendData ("Motor On");
    TextToSend = TextToSend + " MotorOff";
  } else {
    digitalWrite (CombatLEDPin, HIGH);
    //sendData ("Motor Off");
    TextToSend = TextToSend + " MotorOn";
  }

  if (UtilitaryPinButtonState == HIGH) {
    digitalWrite (CoordonneeElecOnLedPin, LOW);
    digitalWrite (UtilitaryLEDPin, LOW);
    //sendData ("Motor On");
    TextToSend = TextToSend + " ShieldOff";
  } else {
    digitalWrite (UtilitaryLEDPin, HIGH);
    digitalWrite (CoordonneeElecOnLedPin, HIGH);
    //sendData ("Motor Off");
    TextToSend = TextToSend + " ShieldOn";
  }

  if (MotorPinButtonState == HIGH) {
    digitalWrite (MotorLEDPin, LOW);
    //sendData ("Motor On");
    TextToSend = TextToSend + " MissileOff";
  } else {
    digitalWrite (MotorLEDPin, HIGH);
    //sendData ("Motor Off");
    TextToSend = TextToSend + " MissileOn";
  }

  if (InfosPinButtonState == HIGH) {
    digitalWrite (InfosLEDPin, LOW);
    //sendData ("Motor On");
    TextToSend = TextToSend + " OtherOff";
  } else {
    digitalWrite (InfosLEDPin, HIGH);
    //sendData ("Motor Off");
    TextToSend = TextToSend + " OtherOn";
  }












/*valPota = analogRead(A0);
  TextToSend = TextToSend + " ValPot: " + valPota;
*/
  if (VoiceSensorButtonState == HIGH) {
    TextToSend = TextToSend + " VoiceButtonHigh"; 
  }
  else {
    TextToSend = TextToSend + " VoiceButtonLow";
  }

  
  TextToSend = TextToSend + " CRNValue:" + CRNValue;

  
  sendData (TextToSend);
  //Serial.println (TextToSend);
  
  delay(5); // Choose your delay having in mind your ReadTimeout in Unity3D
}




void ProcessEncoder(){
  
  ChargeWeaponClockValue = digitalRead(ChargeWeaponClockPin);
  if (ChargeWeaponClockValue != ChargeWeaponClockLast){ // Means the knob is rotating
    // if the knob is rotating, we need to determine direction
    // We do that by reading pin B.
    if (digitalRead(ChargeWeaponDTPin) != ChargeWeaponClockValue) {  // Means pin A Changed first - We're Rotating Clockwise
      CRNValue ++;
      digitalWrite (ChargeWeaponLedPin, HIGH);
      bCW = true;
    } else {// Otherwise B changed first and we're moving CCW
      bCW = false;
      digitalWrite (ChargeWeaponLedPin, HIGH);
      CRNValue++;
    }
    
    
  } 
  ChargeWeaponClockLast = ChargeWeaponClockValue;
}

void sendData(String data){
   #ifdef NATIVE_USB
    SerialUSB.println(data); // need a end-line because wrmlh.csharp use readLine method to receive data 
  #endif

  #ifdef SERIAL_USB
   Serial.println(data); // need a end-line because wrmlh.csharp use readLine method to receive data
  #endif
}
