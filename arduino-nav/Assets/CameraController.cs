using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class CameraController : MonoBehaviour
{
  // private SerialPort serialPort = new SerialPort("/dev/cu.SLAB_USBtoUART", 9600);
  private SerialPort serialPort = new SerialPort("/dev/cu.ESP32_Nav-ESP32SPP", 9600); //115200
  private string serialPortLine;

  private int resetCounter = 0;

  private string[] serialPortInput = new string[4];
  protected float physicalX = 0.0f;
  protected float physicalY = 0.0f;
  protected float physicalZ = 0.0f;
  protected float physicalPanSideways = 0.0f;

  public GameObject camera;
  protected float speed = 0.25f;
  protected float speedIncrease = 0.5f;
  protected float digitalX = 0.0f;
  protected float digitalY = 0.0f;
  protected float digitalZ = 0.0f;

  protected float buffer = -0.09f;

  protected GameObject imageTarget;

  protected float imageTargetSquareSize = 0.03f;
  protected float imageTargetCentreX = 149.5779f;
  protected float imageTargetCentreZ = 37.7945491f; //77.7943f
  protected float imageTargetRealtimeX;
  protected float imageTargetRealtimeZ;
  protected float imageTargetSquaresX;
  protected float imageTargetSquaresZ;

  // Start is called before the first frame update
  void Start()
  {
    try 
    {
      // serialPort.ReadTimeout = 18;
      serialPort.Open();
    } catch (Exception e)
    { 
      print("Nothing really happens");
      print(e);
    }
    imageTarget = GameObject.Find("ImageTarget");
  }

  // Update is called once per frame
  void Update()
  {
    camera.transform.rotation = Quaternion.Euler(
      digitalX,
      digitalY,
      digitalZ
    );
    
    imageTargetRealtimeX = imageTarget.transform.position.x - imageTargetCentreX;
    imageTargetRealtimeZ = imageTarget.transform.position.z - imageTargetCentreZ;
    print("realtimeZ: " + imageTargetRealtimeZ);

    imageTargetSquaresX = imageTargetRealtimeX / imageTargetSquareSize;
    imageTargetSquaresZ = imageTargetRealtimeZ / imageTargetSquareSize;

    print("squaresZ: " + imageTargetSquaresZ);

    UpdateXPosition();
    UpdateZPosition();

    try 
    {
      if(serialPort.IsOpen)
      {

        serialPortLine = serialPort.ReadLine();
        
        if(serialPortLine.Contains(".")) {
          serialPortInput = serialPortLine.Split(' ');
          physicalX = float.Parse(serialPortInput[0]);
          physicalY = float.Parse(serialPortInput[1]);
          physicalZ = float.Parse(serialPortInput[2]);
          physicalPanSideways = float.Parse(serialPortInput[3]);

          resetCounter = 0;

        } else {
          if(resetCounter >= 10) {
            print("Restart");
            serialPort.Close();
            serialPort = new SerialPort("/dev/cu.ESP32_Nav-ESP32SPP", 9600);
            serialPort.Open();
          }
          resetCounter += 1;
        }
        
        if(!IsInBufferRange(physicalY, 'y'))
        {
          digitalX = physicalY; // pitch
        }
        if(!IsInBufferRange(physicalZ, 'z'))
        {
          digitalY = physicalZ; // yaw
        }
        if(!IsInBufferRange(physicalX, 'x'))
        {
          digitalZ = physicalX; // roll
        }

        if (physicalPanSideways < -10) {
          camera.transform.Translate(new Vector3((physicalPanSideways/10)*-1, 0, 0));
        } else if (physicalPanSideways > 10) {
          camera.transform.Translate(new Vector3((physicalPanSideways/10)*-1, 0, 0));
        }

        Array.Clear(serialPortInput, 0, serialPortInput.Length);
        serialPort.DiscardOutBuffer();
        serialPort.DiscardInBuffer();
      } else {
      }
    }
    catch (Exception e)
    {
      print("something wrong " + e);
    }

    if (Input.GetKey("w")) 
    {
      camera.transform.Translate(new Vector3(0, 0, speed));
    } else if (Input.GetKey("s")) 
    {
      camera.transform.Translate(new Vector3(0, 0, -speed));
    } else if (Input.GetKey("a")) 
    {
      camera.transform.Translate(new Vector3(-speed, 0, 0));
    } else if (Input.GetKey("d")) 
    {
      camera.transform.Translate(new Vector3(speed, 0, 0));
    } else if (Input.GetKey(KeyCode.Space)) 
    {
      camera.transform.Translate(new Vector3(0, 0, speed));
    } else if (Input.GetKey(KeyCode.LeftControl)) 
    {
      camera.transform.Translate(new Vector3(0, 0, -speed));
    }
  }

  Boolean IsInBufferRange(float physicalDegrees, char physicalAxis)
  {
    switch (physicalAxis)
    {
      case 'x':
        return IsInRange(digitalZ, physicalDegrees);
      case 'y':
        return IsInRange(digitalX, physicalDegrees);
      case 'z':
        return IsInRange(digitalY, physicalDegrees);
      default:
        return false;
    }
  }
  Boolean IsInRange(float digitalAxis, float physicalDegrees)
  {
    if(digitalAxis - physicalDegrees > buffer && digitalAxis - physicalDegrees < buffer*-1 ) {
      return true; // 90 - 90.1 = -0.1 if(-0.1 > -0.2) true if(-0.1 < 0.2) true
    }              // 90 - 89.9 = 0.1 if(0.1 > -0.2) true if(0.1 < 0.2) true
    else {         // 90 - 90.3 = -0.3 if(-0.3 > -0.2) false if(-0.3 < 0.2) true
      return false; // 90 - 89.7 = 0.3 if(0.3 > -0.2) true if(0.3 < 0.2) false
    }
  }
  void UpdateXPosition() 
  {
    // Super dry if check - refactor when possible
    if(imageTargetSquaresX >= 10) {
      camera.transform.Translate(new Vector3(speed + speedIncrease*8, 0, 0));
      imageTargetCentreX += speed + speedIncrease*8;
    } else if(imageTargetSquaresX >= 9) {
      camera.transform.Translate(new Vector3(speed + speedIncrease*7, 0, 0));
      imageTargetCentreX += speed + speedIncrease*7;
    } else if(imageTargetSquaresX >= 8) {
      camera.transform.Translate(new Vector3(speed + speedIncrease*6, 0, 0));
      imageTargetCentreX += speed + speedIncrease*6;
    } else if(imageTargetSquaresX >= 7) {
      camera.transform.Translate(new Vector3(speed + speedIncrease*5, 0, 0));
      imageTargetCentreX += speed + speedIncrease*5;
    } else if(imageTargetSquaresX >= 6) {
      camera.transform.Translate(new Vector3(speed + speedIncrease*4, 0, 0));
      imageTargetCentreX += speed + speedIncrease*4;
    } else if(imageTargetSquaresX >= 5) {
      camera.transform.Translate(new Vector3(speed + speedIncrease*3, 0, 0));
      imageTargetCentreX += speed + speedIncrease*3;
    } else if(imageTargetSquaresX >= 4) {
      camera.transform.Translate(new Vector3(speed + speedIncrease*2, 0, 0));
      imageTargetCentreX += speed + speedIncrease*2;
    } else if(imageTargetSquaresX >= 3) {
      camera.transform.Translate(new Vector3(speed + speedIncrease*1, 0, 0));
      imageTargetCentreX += speed + speedIncrease*1;
    } else if(imageTargetSquaresX >= 2) {
      camera.transform.Translate(new Vector3(speed, 0, 0));
      imageTargetCentreX += speed;
    } else if(imageTargetSquaresX <= -10) {
      camera.transform.Translate(new Vector3(-(speed + speedIncrease*8), 0, 0));
      imageTargetCentreX -= (speed + speedIncrease*8);
    } else if(imageTargetSquaresX <= -9) {
      camera.transform.Translate(new Vector3(-(speed + speedIncrease*7), 0, 0));
      imageTargetCentreX -= (speed + speedIncrease*7);
    } else if(imageTargetSquaresX <= -8) {
      camera.transform.Translate(new Vector3(-(speed + speedIncrease*6), 0, 0));
      imageTargetCentreX -= (speed + speedIncrease*6);
    } else if(imageTargetSquaresX <= -7) {
      camera.transform.Translate(new Vector3(-(speed + speedIncrease*5), 0, 0));
      imageTargetCentreX -= (speed + speedIncrease*5);
    } else if(imageTargetSquaresX <= -6) {
      camera.transform.Translate(new Vector3(-(speed + speedIncrease*4), 0, 0));
      imageTargetCentreX -= (speed + speedIncrease*4);
    } else if(imageTargetSquaresX <= -5) {
      camera.transform.Translate(new Vector3(-(speed + speedIncrease*3), 0, 0));
      imageTargetCentreX -= (speed + speedIncrease*3);
    } else if(imageTargetSquaresX <= -4) {
      camera.transform.Translate(new Vector3(-(speed + speedIncrease*2), 0, 0));
      imageTargetCentreX -= (speed + speedIncrease*2);
    } else if(imageTargetSquaresX <= -3) {
      camera.transform.Translate(new Vector3(-(speed + speedIncrease*1), 0, 0));
      imageTargetCentreX -= (speed + speedIncrease*1);
    } else if(imageTargetSquaresX <= -2) {
      camera.transform.Translate(new Vector3(-speed, 0, 0));
      imageTargetCentreX -= speed;
    }
  }

  void UpdateZPosition() 
  {
    if(imageTargetSquaresZ >= 10){
      camera.transform.Translate(new Vector3(0, 0, (speed + speedIncrease*8)));
      imageTargetCentreZ += (speed + speedIncrease*8);
    } else if(imageTargetSquaresZ >= 9){
      camera.transform.Translate(new Vector3(0, 0, (speed + speedIncrease*7)));
      imageTargetCentreZ += (speed + speedIncrease*7);
    } else if(imageTargetSquaresZ >= 8){
      camera.transform.Translate(new Vector3(0, 0, (speed + speedIncrease*6)));
      imageTargetCentreZ += (speed + speedIncrease*6);
    } else if(imageTargetSquaresZ >= 7){
      camera.transform.Translate(new Vector3(0, 0, (speed + speedIncrease*5)));
      imageTargetCentreZ += (speed + speedIncrease*5);
    } else if(imageTargetSquaresZ >= 6){
      camera.transform.Translate(new Vector3(0, 0, (speed + speedIncrease*4)));
      imageTargetCentreZ += (speed + speedIncrease*4);
    } else if(imageTargetSquaresZ >= 5){
      camera.transform.Translate(new Vector3(0, 0, (speed + speedIncrease*3)));
      imageTargetCentreZ += (speed + speedIncrease*3);
    } else if(imageTargetSquaresZ >= 4){
      camera.transform.Translate(new Vector3(0, 0, (speed + speedIncrease*2)));
      imageTargetCentreZ += (speed + speedIncrease*2);
    } else if(imageTargetSquaresZ >= 3){
      camera.transform.Translate(new Vector3(0, 0, (speed + speedIncrease*1)));
      imageTargetCentreZ += (speed + speedIncrease*1);
    } else if(imageTargetSquaresZ >= 2){
      camera.transform.Translate(new Vector3(0, 0, speed));
      imageTargetCentreZ += speed;
    } else if(imageTargetSquaresZ <= -3) {
      camera.transform.Translate(new Vector3(0, 0, -(speed + speedIncrease*3)));
      imageTargetCentreZ -= (speed + speedIncrease*3);
    } else if(imageTargetSquaresZ <= -2){
      camera.transform.Translate(new Vector3(0, 0, -(speed + speedIncrease)));
      imageTargetCentreZ -= (speed + speedIncrease);
    } else if(imageTargetSquaresZ <= -1){
      camera.transform.Translate(new Vector3(0, 0, -speed));
      imageTargetCentreZ -= speed;
    }
  }
}

