using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;

public class CameraController : MonoBehaviour
{
  public SerialPort sp = new SerialPort("/dev/cu.SLAB_USBtoUART", 9600);
  public float spInput;
  public GameObject camera;
  float speed = 5.0f;
  float x = 0.0f;
  float y = 0.0f;
  float z = 0.0f;
  int axisHalfRotation = 180;

  // Start is called before the first frame update
  void Start()
  {
    try {
      sp.Open();
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
      x,
      y,
      z
    );

    try {
      if(sp.IsOpen) {
        spInput = float.Parse(sp.ReadLine());
        if (spInput < 1000 - axisHalfRotation) 
        {
          // if (spInput < 0) 
          // {
          //   spInput += 180;
          // }
          z = spInput; // arduino x is unity y roll
        } 
        else if (spInput < 2000 - axisHalfRotation) 
        {
          spInput -= 1000;
          // if (spInput < 0) 
          // {
          //   spInput += 180;
          // }
          x = spInput*-1; // arduino y is unity x yaw
        } 
        else
        {
          spInput -= 2000;
          spInput *= 2;
          // if (spInput < 0)
          // {
          //   spInput += 180;
          // }
          y = spInput*-1; // arduino z is unity z pitch
        }
      }
    } catch (Exception e) {
      print("mistakes were made!");
    }

    if (Input.GetKey("left"))
    {
      x -= 1.0f;
    } else if (Input.GetKey("right")) 
    {
      x += 1.0f;
    } else if (Input.GetKey("w")) 
    {
      camera.transform.Translate(new Vector3(0, speed, 0));
    } else if (Input.GetKey("s")) 
    {
      camera.transform.Translate(new Vector3(0, -speed, 0));
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
}