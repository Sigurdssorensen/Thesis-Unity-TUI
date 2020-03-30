// https://forum.arduino.cc/index.php?topic=587830.0
// https://github.com/tockn/MPU6050_tockn

#include <MPU6050_tockn.h>
#include <Wire.h>

MPU6050 mpu6050(Wire);

const int updateCameraButtonPin = 15;
const int resetCameraButtonPin = 14;

long timer = 0;

int newUpdateCameraButtonState = 0;
int newResetCameraButtonState = 0;

int currentUpdateCameraButtonState = 0;
int currentResetCameraButtonState = 0;

float currentCameraX = 0.0;
float currentCameraY = 0.0;
float currentCameraZ = 0.0;

float physicalCameraX;
float physicalCameraY;
float physicalCameraZ;

String payloadCameraX;
String payloadCameraY;
String payloadCameraZ;

String payload;

void setup() {
  pinMode(updateCameraButtonPin, INPUT);
  pinMode(resetCameraButtonPin, INPUT);
  
  Serial.begin(9600);
  Wire.begin();
  mpu6050.begin();
  mpu6050.calcGyroOffsets(true);
}

void loop() {
  mpu6050.update();

  if(millis() - timer > 1000){
    Serial.flush();
    
    newUpdateCameraButtonState = digitalRead(updateCameraButtonPin);
    newResetCameraButtonState = digitalRead(resetCameraButtonPin);

//    resetIfPushed();
    Serial.println(newUpdateCameraButtonState);
    
    physicalCameraX = mpu6050.getAngleX();
    physicalCameraY = mpu6050.getAngleY()*-1;
    physicalCameraZ = mpu6050.getAngleZ()*-1;

//    updateIfPushed();
    
    payloadCameraX = String(currentCameraX + (physicalCameraX - currentCameraX));
    payloadCameraY = String(currentCameraY + (physicalCameraY - currentCameraY));
    payloadCameraZ = String(currentCameraZ + (physicalCameraZ - currentCameraZ));
    
    payload = payloadCameraX + " " + payloadCameraY + " " + payloadCameraZ;
//    Serial.println(payload);
   
    timer = millis();
  }
}

void resetIfPushed() {
  if(newResetCameraButtonState != currentResetCameraButtonState) {
    resetCamera();
    currentResetCameraButtonState = newResetCameraButtonState;
  }
}

void resetCamera() {
  currentCameraX = 0.0;
  currentCameraY = 0.0;
  currentCameraZ = 0.0;
}

void updateIfPushed() {
  if(newUpdateCameraButtonState != currentUpdateCameraButtonState) {
    updateCamera();
    currentUpdateCameraButtonState = newUpdateCameraButtonState;
  }
}

void updateCamera() {
  currentCameraX = physicalCameraX;
  currentCameraY = physicalCameraY;
  currentCameraZ = physicalCameraZ;
}


//    Serial.println("=======================================================");
//    Serial.print("temp : ");Serial.println(mpu6050.getTemp());
//    Serial.print(String(mpu6050.getAccX()));
//    Serial.print("\n");
//    Serial.print("\taccY : ");Serial.print(mpu6050.getAccY());
//    Serial.print("\taccZ : ");Serial.println(mpu6050.getAccZ());
//  
//    Serial.print("gyroX : ");Serial.print(mpu6050.getGyroX());
//    Serial.print("\tgyroY : ");Serial.print(mpu6050.getGyroY());
//    Serial.print("\tgyroZ : ");Serial.println(mpu6050.getGyroZ());
//  
//    Serial.print("accAngleX : ");Serial.print(mpu6050.getAccAngleX());
//    Serial.print("\taccAngleY : ");Serial.println(mpu6050.getAccAngleY());
//  
//    Serial.print("gyroAngleX : ");Serial.print(mpu6050.getGyroAngleX());
//    Serial.print("\tgyroAngleY : ");Serial.print(mpu6050.getGyroAngleY());
//    Serial.print("\tgyroAngleZ : ");Serial.println(mpu6050.getGyroAngleZ());
//    
//    Serial.println(String(mpu6050.getAngleX())); // roll
//    Serial.flush();
//
//    Serial.println(String(mpu6050.getAngleY()+1000)); // pitch
//    Serial.flush();
//    Serial.println(String(mpu6050.getAngleZ()+2000)); // yaw
//    Serial.flush();

//    Serial.println("=======================================================\n");
