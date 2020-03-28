#include <MPU6050_tockn.h>
#include <Wire.h>

MPU6050 mpu6050(Wire);

long timer = 0;

void setup() {
  Serial.begin(9600);
  Wire.begin();
  mpu6050.begin();
  mpu6050.calcGyroOffsets(true);
}

void loop() {
  mpu6050.update();

  if(millis() - timer > 1){

    Serial.flush();
    
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
    Serial.println(String(mpu6050.getAngleX())); // roll
    Serial.flush();

    Serial.println(String(mpu6050.getAngleY()+1000)); // pitch
    Serial.flush();
    Serial.println(String(mpu6050.getAngleZ()+2000)); // yaw
    Serial.flush();

//    Serial.println("=======================================================\n");
    timer = millis();
    
  }
}
