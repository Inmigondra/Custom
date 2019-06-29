// This simple code allow you to send data from Arduino to Unity3D.

// uncomment "NATIVE_USB" if you're using ARM CPU (Arduino DUE, Arduino M0, ..)
#define SerialPort NATIVE_USB

// uncomment "SERIAL_USB" if you're using non ARM CPU (Arduino Uno, Arduino Mega, ..)
#define SERIAL_USB


const int MotorEnergyPin =8;
const int ShieldEnergyPin =9;
const int MissileEnergyPin =10;
const int OtherEnergyPin =11;
const int VoiceSensor = 12;
const int Languette = 4;

const int ClockPin = 2;
const int DtPin = 13;


int MotorEnergyButtonState = 0;
int ShieldEnergyButtonState = 0;
int MissileEnergyButtonState = 0;
int OtherEnergyButtonState = 0;
int VoiceSensorButtonState =0;
int LanguetteButtonState =0;

int valPota;

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

  pinMode(MotorEnergyPin, INPUT_PULLUP);
  pinMode(ShieldEnergyPin, INPUT_PULLUP);
  pinMode(MissileEnergyPin, INPUT_PULLUP);
  pinMode(OtherEnergyPin, INPUT_PULLUP);

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
  MotorEnergyButtonState = digitalRead(MotorEnergyPin);
  ShieldEnergyButtonState = digitalRead(ShieldEnergyPin);
  MissileEnergyButtonState = digitalRead(MissileEnergyPin);
  OtherEnergyButtonState = digitalRead(OtherEnergyPin);
  VoiceSensorButtonState = digitalRead (VoiceSensor);
  LanguetteButtonState = digitalRead (Languette);

  if (LanguetteButtonState == HIGH) {
    TextToSend = TextToSend + " LanguetteOff";
  }else{
    TextToSend = TextToSend + " LanguetteOn";
  }
  if (MotorEnergyButtonState == HIGH) {
    //sendData ("Motor On");
    TextToSend = TextToSend + " MotorOff";
  } else {
    //sendData ("Motor Off");
    TextToSend = TextToSend + " MotorOn";
  }

  if (ShieldEnergyButtonState == HIGH) {
    //sendData ("Motor On");
    TextToSend = TextToSend + " ShieldOff";
  } else {
    //sendData ("Motor Off");
    TextToSend = TextToSend + " ShieldOn";
  }

  if (MissileEnergyButtonState == HIGH) {
    //sendData ("Motor On");
    TextToSend = TextToSend + " MissileOff";
  } else {
    //sendData ("Motor Off");
    TextToSend = TextToSend + " MissileOn";
  }

  if (OtherEnergyButtonState == HIGH) {
    //sendData ("Motor On");
    TextToSend = TextToSend + " OtherOff";
  } else {
    //sendData ("Motor Off");
    TextToSend = TextToSend + " OtherOn";
  }

  valPota = analogRead(A0);
  TextToSend = TextToSend + " " + valPota;

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
