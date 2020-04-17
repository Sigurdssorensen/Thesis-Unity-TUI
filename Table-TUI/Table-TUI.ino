// https://forum.arduino.cc/index.php?topic=587830.0
// https://github.com/tockn/MPU6050_tockn

#include <MPU6050_tockn.h>
#include <Wire.h>
#include <BluetoothSerial.h>

BluetoothSerial ESP_BT;

MPU6050 mpu6050(Wire);

long timer = 0;

const int resetCameraButtonPin = 14;
int resetCameraButtonState = 0;
bool resetButtonIsHeld = false;

const int panCameraSidewaysPin = 15;
int panCameraSidewaysState = 0;
bool panCameraSidewaysButtonIsHeld = false;

float fullRotation = 360.0;

float currentCameraX = 0.0;
float currentCameraY = 0.0;
float currentCameraZ = 0.0;

float physicalCameraX;
float physicalCameraY;
float physicalCameraZ;

float continuousYawCameraZ;
bool continuousYawRunning = false;
const float yawRightMin = 20;
const float yawRightMax = 100;
const float yawLeftMin = 320;
const float yawLeftMax = 240;

String payloadCameraX;
String payloadCameraY;
String payloadCameraZ;
float payloadPanSideways = 0.0;

String payload;

void setup() {
  pinMode(resetCameraButtonPin, INPUT);
  pinMode(panCameraSidewaysPin, INPUT);
  
//  Serial.begin(9600);
//  Serial.begin(460800);
  ESP_BT.begin("ESP32_Nav"); //Name of your Bluetooth Signal
  ESP_BT.println("test");
  
  Wire.begin();
  mpu6050.begin();
  mpu6050.calcGyroOffsets(true);
}

void loop() {
  mpu6050.update();

  if(millis() - timer > 16) {
//    Serial.flush();
    ESP_BT.flush();
    
    resetCameraButtonState = digitalRead(resetCameraButtonPin);
    panCameraSidewaysState = digitalRead(panCameraSidewaysPin);

    resetIfPushed();
    
    physicalCameraX = mpu6050.getAngleX();
    physicalCameraY = mpu6050.getAngleY()*-1;
    physicalCameraZ = mpu6050.getAngleZ()*-1;

    panSidewaysIfPushed();

    adjustRotation();

    payload = createPayload();

//    Serial.println(payload);
    ESP_BT.println(payload);
   
    timer = millis();
  }
}

void panSidewaysIfPushed() {
  if(panCameraSidewaysState == 1 && !panCameraSidewaysButtonIsHeld) {
    panCameraSidewaysButtonIsHeld = true;
    panSideways();
        
  } else if (panCameraSidewaysState == 0 && panCameraSidewaysButtonIsHeld) {
    panCameraSidewaysButtonIsHeld = false;
    payloadPanSideways = 0.0;
    
  } else if (panCameraSidewaysButtonIsHeld) {
    panSideways();
  }
}

void panSideways() {
  if(physicalCameraX > 10 || physicalCameraX < -10) {
    payloadPanSideways = physicalCameraX;
    
  } else {
    payloadPanSideways = 0.0;
  }
  physicalCameraX = 0.0;
}

void resetIfPushed() {
  if(resetCameraButtonState == 1 && !resetButtonIsHeld) {
    resetButtonIsHeld = true;
    resetCamera();
  } else if (resetCameraButtonState == 0 && resetButtonIsHeld) {
    resetButtonIsHeld = false;
  }
}

void resetCamera() {
  currentCameraX = 0.0;
  currentCameraY = 0.0;
  currentCameraZ = 0.0;
}

void adjustRotation() {
  if(physicalCameraX < 0) {
    physicalCameraX += fullRotation;
  }
  
  if(physicalCameraY < 0) {
    physicalCameraY += fullRotation;
  }
  
  if(physicalCameraZ < 0) {
    physicalCameraZ += fullRotation;
  }
}

String createPayload() {
  if(isRight(physicalCameraZ, yawRightMin, physicalCameraZ, yawRightMax)) {
    continuousYawRight();
    return assemblePayload(continuousYawCameraZ);
     
  } else if (isLeft(physicalCameraZ, yawLeftMin, physicalCameraZ, yawLeftMax)) {
    continuousYawLeft();
    return assemblePayload(continuousYawCameraZ);
    
  } else {
    if(continuousYawRunning) {
      setCurrentCamera();
    }
    return assemblePayload(physicalCameraZ);
  }
}

void continuousYawRight() {
  if(!continuousYawRunning) {
    continuousYawRunning = true;
    continuousYawCameraZ = continuousYawCameraZ + physicalCameraZ;
    continuousYawCameraZ += 2;
    
  } else {
    continuousYawCameraZ += 2;
  }
}

void continuousYawLeft()
{
  if(!continuousYawRunning) {
    continuousYawRunning = true;
    continuousYawCameraZ = continuousYawCameraZ + physicalCameraZ;
    continuousYawCameraZ -= 2;
    
  } else {
    continuousYawCameraZ -= 2;
  }
}

void setCurrentCamera() {
  continuousYawRunning = false;
  if(isRight(physicalCameraZ, 0, physicalCameraZ, 100)) {
    currentCameraZ = continuousYawCameraZ - physicalCameraZ;
    
  } else if (isLeft(physicalCameraZ, 360, physicalCameraZ, 200)) {
    currentCameraZ = continuousYawCameraZ + (360 - physicalCameraZ);
  }
}

String assemblePayload(float zAxis) {
  payloadCameraX = String(physicalCameraX - currentCameraX);
  payloadCameraY = String(physicalCameraY + currentCameraY);
  
  if(continuousYawRunning) {
    payloadCameraZ = String(zAxis);
    
  } else if (isRight(yawRightMin, zAxis, 0, zAxis)) {
    // right
    payloadCameraZ = String(currentCameraZ + zAxis);
    
  } else if (isLeft(yawLeftMin, zAxis, 360, zAxis)) {
    // left
    payloadCameraZ = String(currentCameraZ - (360 - zAxis));
  }
  return payloadCameraX + " " + payloadCameraY + " " + payloadCameraZ + " " + payloadPanSideways;
}

bool isLeft(float param1, float param2, float param3, float param4) {
  if (param1 < param2 && param3 > param4) {
    return true;
  }
  return false;
}

bool isRight(float param1, float param2, float param3, float param4) {
  if(param1 > param2 && param3 < param4) {
    return true;
  }
  return false;
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
