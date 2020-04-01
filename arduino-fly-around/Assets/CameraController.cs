using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class CameraController : MonoBehaviour
{
  private SerialPort serialPort = new SerialPort("/dev/cu.SLAB_USBtoUART", 9600);
  private string[] serialPortInput = new string[4];
  protected float physicalX = 0.0f;
  protected float physicalY = 0.0f;
  protected float physicalZ = 0.0f;
  protected float physicalPanSideways = 0.0f;

  public GameObject camera;
  protected float speed = 5.0f;
  protected float digitalX = 0.0f;
  protected float digitalY = 0.0f;
  protected float digitalZ = 0.0f;
  private int axisHalfRotation = 180;

  protected float buffer = -0.09f;

  // Start is called before the first frame update
  void Start()
  {
    try 
    {
      serialPort.Open();
    } catch (Exception e)
    { 
      print("Nothing really happens");
      print(e);
    }
  }

  // Update is called once per frame
  void Update()
  {
    camera.transform.rotation = Quaternion.Euler(
      digitalX,
      digitalY,
      digitalZ
    );

    try 
    {
      if(serialPort.IsOpen)
      {
        serialPortInput = serialPort.ReadLine().Split(' ');
        physicalX = float.Parse(serialPortInput[0]);
        physicalY = float.Parse(serialPortInput[1]);
        physicalZ = float.Parse(serialPortInput[2]);
        physicalPanSideways = float.Parse(serialPortInput[3]);
        
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
      }
    }
    catch (Exception e)
    {
      print("something wrong" + e);
      print(serialPort.ReadLine());
    }

    // try {
    //   if(sp.IsOpen) {
    //     spInput = float.Parse(sp.ReadLine());
    //     if (spInput < 1000 - axisHalfRotation) 
    //     {
    //       // if (spInput < 0) 
    //       // {
    //       //   spInput += 180;
    //       // }
    //       z = spInput; // arduino x is unity y roll
    //     } 
    //     else if (spInput < 2000 - axisHalfRotation) 
    //     {
    //       spInput -= 1000;
    //       // if (spInput < 0) 
    //       // {
    //       //   spInput += 180;
    //       // }
    //       x = spInput*-1; // arduino y is unity x yaw
    //     } 
    //     else
    //     {
    //       spInput -= 2000;
    //       spInput *= 2;
    //       // if (spInput < 0)
    //       // {
    //       //   spInput += 180;
    //       // }
    //       y = spInput*-1; // arduino z is unity z pitch
    //     }
    //   }
    // } catch (Exception e) {
    //   print("mistakes were made!");
    // }

    // if (Input.GetKey("left"))
    // {
    //   x -= 1.0f;
    // } else if (Input.GetKey("right")) 
    // {
    //   x += 1.0f;
    // } else if (Input.GetKey("w")) 
    // {
    //   camera.transform.Translate(new Vector3(0, speed, 0));
    // } else if (Input.GetKey("s")) 
    // {
    //   camera.transform.Translate(new Vector3(0, -speed, 0));
    // } else if (Input.GetKey("a")) 
    // {
    //   camera.transform.Translate(new Vector3(-speed, 0, 0));
    // } else if (Input.GetKey("d")) 
    // {
    //   camera.transform.Translate(new Vector3(speed, 0, 0));
    // } else if (Input.GetKey(KeyCode.Space)) 
    // {
    //   camera.transform.Translate(new Vector3(0, 0, speed));
    // } else if (Input.GetKey(KeyCode.LeftControl)) 
    // {
    //   camera.transform.Translate(new Vector3(0, 0, -speed));
    // }
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
}