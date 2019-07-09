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


const int VoiceSensor = 12;
const int Languette = 4;

int valPota;

const int ClockPin = 26;
const int DtPin = 13;


int CombatPinButtonState = 0;
int UtilitaryPinButtonState = 0;
int MotorPinButtonState = 0;
int InfosPinButtonState = 0;



int VoiceSensorButtonState =0;
int LanguetteButtonState =0;



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

  pinMode(CombatPin, INPUT_PULLUP);
  pinMode(UtilitaryPin, INPUT_PULLUP);
  pinMode(MotorPin, INPUT_PULLUP);
  pinMode(InfosPin, INPUT_PULLUP);

  
  pinMode (CombatLEDPin, OUTPUT);
  pinMode (UtilitaryLEDPin, OUTPUT);
  pinMode (MotorLEDPin, OUTPUT);
  pinMode (InfosLEDPin, OUTPUT);

  pinMode (Languette, INPUT_PULLUP);

  pinMode(VoiceSensor, INPUT);


  pinMode (ClockPin, INPUT);
  digitalWrite (ClockPin, HIGH);
  pinMode (DtPin, HIGH);
  
  attachInterrupt (0,ProcessEncoder, CHANGE);
}

// Run forever
void loop() {
  String TextToSend = "";
  //sendData("Hello World!");
  CombatPinButtonState = digitalRead(CombatPin);
  UtilitaryPinButtonState = digitalRead(UtilitaryPin);
  MotorPinButtonState = digitalRead(MotorPin);
  InfosPinButtonState = digitalRead(InfosPin);

  
  VoiceSensorButtonState = digitalRead (VoiceSensor);
  LanguetteButtonState = digitalRead (Languette);

  if (LanguetteButtonState == HIGH) {
    TextToSend = TextToSend + " LanguetteOff";
  }else{
    TextToSend = TextToSend + " LanguetteOn";
  }
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
    digitalWrite (UtilitaryLEDPin, LOW);
    //sendData ("Motor On");
    TextToSend = TextToSend + " ShieldOff";
  } else {
    digitalWrite (UtilitaryLEDPin, HIGH);
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

  valPota = analogRead(A0);
  TextToSend = TextToSend + " ValPot: " + valPota;

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
  if (digitalRead (ClockPin) == digitalRead (DtPin)){
    CRNValue ++;
  }else{
    CRNValue ++;
  }
}

void sendData(String data){
   #ifdef NATIVE_USB
    SerialUSB.println(data); // need a end-line because wrmlh.csharp use readLine method to receive data 
  #endif

  #ifdef SERIAL_USB
    Serial.println(data); // need a end-line because wrmlh.csharp use readLine method to receive data
  #endif
}
