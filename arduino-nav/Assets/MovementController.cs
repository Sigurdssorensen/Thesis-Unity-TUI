using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
  protected GameObject imageTarget;
  public GameObject home;

  public float speed = 0.25f;
  public float speedIncrease = 0.5f;

  protected float imageTargetSquareSize = 0.03f;
  protected float imageTargetCentreX = 149.5779f;
  protected float imageTargetCentreZ = 37.7945491f + 10f;
  protected float imageTargetRealtimeX;
  protected float imageTargetRealtimeZ;
  public float imageTargetSquaresX;
  public float imageTargetSquaresZ;
  // Start is called before the first frame update
  void Start()
  {
    imageTarget = GameObject.Find("ImageTarget");
    home = GameObject.Find("Home");
  }

  // Update is called once per frame
  void Update()
  {
    imageTargetRealtimeX = imageTarget.transform.position.x - imageTargetCentreX;
    imageTargetRealtimeZ = imageTarget.transform.position.z - imageTargetCentreZ;

    imageTargetSquaresX = imageTargetRealtimeX / imageTargetSquareSize;
    imageTargetSquaresZ = imageTargetRealtimeZ / imageTargetSquareSize;

    UpdateXPosition();
    UpdateZPosition();

    // if (Input.GetKey("w")) 
    // {
    //   home.transform.Translate(new Vector3(0, 0, speed));
    // } else if (Input.GetKey("s")) 
    // {
    //   home.transform.Translate(new Vector3(0, 0, -speed));
    // } else if (Input.GetKey("a")) 
    // {
    //   home.transform.Translate(new Vector3(-speed, 0, 0));
    // } else if (Input.GetKey("d")) 
    // {
    //   home.transform.Translate(new Vector3(speed, 0, 0));
    // } else if (Input.GetKey(KeyCode.Space)) 
    // {
    //   home.transform.Translate(new Vector3(0, 0, speed));
    // } else if (Input.GetKey(KeyCode.LeftControl)) 
    // {
    //   home.transform.Translate(new Vector3(0, 0, -speed));
    // }
    
  }
  
  void UpdateXPosition() 
  {
    // Super dry if check - refactor when possible
    if(imageTargetSquaresX >= 10) {
      home.transform.Translate(new Vector3(speed + speedIncrease*8, 0, 0));
      imageTargetCentreX += speed + speedIncrease*8;
    } else if(imageTargetSquaresX >= 9) {
      home.transform.Translate(new Vector3(speed + speedIncrease*7, 0, 0));
      imageTargetCentreX += speed + speedIncrease*7;
    } else if(imageTargetSquaresX >= 8) {
      home.transform.Translate(new Vector3(speed + speedIncrease*6, 0, 0));
      imageTargetCentreX += speed + speedIncrease*6;
    } else if(imageTargetSquaresX >= 7) {
      home.transform.Translate(new Vector3(speed + speedIncrease*5, 0, 0));
      imageTargetCentreX += speed + speedIncrease*5;
    } else if(imageTargetSquaresX >= 6) {
      home.transform.Translate(new Vector3(speed + speedIncrease*4, 0, 0));
      imageTargetCentreX += speed + speedIncrease*4;
    } else if(imageTargetSquaresX >= 5) {
      home.transform.Translate(new Vector3(speed + speedIncrease*3, 0, 0));
      imageTargetCentreX += speed + speedIncrease*3;
    } else if(imageTargetSquaresX >= 4) {
      home.transform.Translate(new Vector3(speed + speedIncrease*2, 0, 0));
      imageTargetCentreX += speed + speedIncrease*2;
    } else if(imageTargetSquaresX >= 3) {
      home.transform.Translate(new Vector3(speed + speedIncrease*1, 0, 0));
      imageTargetCentreX += speed + speedIncrease*1;
    } else if(imageTargetSquaresX >= 2) {
      home.transform.Translate(new Vector3(speed, 0, 0));
      imageTargetCentreX += speed;
    } else if(imageTargetSquaresX <= -10) {
      home.transform.Translate(new Vector3(-(speed + speedIncrease*8), 0, 0));
      imageTargetCentreX -= (speed + speedIncrease*8);
    } else if(imageTargetSquaresX <= -9) {
      home.transform.Translate(new Vector3(-(speed + speedIncrease*7), 0, 0));
      imageTargetCentreX -= (speed + speedIncrease*7);
    } else if(imageTargetSquaresX <= -8) {
      home.transform.Translate(new Vector3(-(speed + speedIncrease*6), 0, 0));
      imageTargetCentreX -= (speed + speedIncrease*6);
    } else if(imageTargetSquaresX <= -7) {
      home.transform.Translate(new Vector3(-(speed + speedIncrease*5), 0, 0));
      imageTargetCentreX -= (speed + speedIncrease*5);
    } else if(imageTargetSquaresX <= -6) {
      home.transform.Translate(new Vector3(-(speed + speedIncrease*4), 0, 0));
      imageTargetCentreX -= (speed + speedIncrease*4);
    } else if(imageTargetSquaresX <= -5) {
      home.transform.Translate(new Vector3(-(speed + speedIncrease*3), 0, 0));
      imageTargetCentreX -= (speed + speedIncrease*3);
    } else if(imageTargetSquaresX <= -4) {
      home.transform.Translate(new Vector3(-(speed + speedIncrease*2), 0, 0));
      imageTargetCentreX -= (speed + speedIncrease*2);
    } else if(imageTargetSquaresX <= -3) {
      home.transform.Translate(new Vector3(-(speed + speedIncrease*1), 0, 0));
      imageTargetCentreX -= (speed + speedIncrease*1);
    } else if(imageTargetSquaresX <= -2) {
      home.transform.Translate(new Vector3(-speed, 0, 0));
      imageTargetCentreX -= speed;
    }
  }

  void UpdateZPosition() 
  {
    if(imageTargetSquaresZ >= 10){
      home.transform.Translate(new Vector3(0, 0, (speed + speedIncrease*8)));
      imageTargetCentreZ += (speed + speedIncrease*8);
    } else if(imageTargetSquaresZ >= 9){
      home.transform.Translate(new Vector3(0, 0, (speed + speedIncrease*7)));
      imageTargetCentreZ += (speed + speedIncrease*7);
    } else if(imageTargetSquaresZ >= 8){
      home.transform.Translate(new Vector3(0, 0, (speed + speedIncrease*6)));
      imageTargetCentreZ += (speed + speedIncrease*6);
    } else if(imageTargetSquaresZ >= 7){
      home.transform.Translate(new Vector3(0, 0, (speed + speedIncrease*5)));
      imageTargetCentreZ += (speed + speedIncrease*5);
    } else if(imageTargetSquaresZ >= 6){
      home.transform.Translate(new Vector3(0, 0, (speed + speedIncrease*4)));
      imageTargetCentreZ += (speed + speedIncrease*4);
    } else if(imageTargetSquaresZ >= 5){
      home.transform.Translate(new Vector3(0, 0, (speed + speedIncrease*3)));
      imageTargetCentreZ += (speed + speedIncrease*3);
    } else if(imageTargetSquaresZ >= 4){
      home.transform.Translate(new Vector3(0, 0, (speed + speedIncrease*2)));
      imageTargetCentreZ += (speed + speedIncrease*2);
    } else if(imageTargetSquaresZ >= 3){
      home.transform.Translate(new Vector3(0, 0, (speed + speedIncrease*1)));
      imageTargetCentreZ += (speed + speedIncrease*1);
    } else if(imageTargetSquaresZ >= 2){
      home.transform.Translate(new Vector3(0, 0, speed));
      imageTargetCentreZ += speed;
    } else if(imageTargetSquaresZ <= -3) {
      home.transform.Translate(new Vector3(0, 0, -(speed + speedIncrease*3)));
      imageTargetCentreZ -= (speed + speedIncrease*3);
    } else if(imageTargetSquaresZ <= -2){
      home.transform.Translate(new Vector3(0, 0, -(speed + speedIncrease)));
      imageTargetCentreZ -= (speed + speedIncrease);
    } else if(imageTargetSquaresZ <= -1){
      home.transform.Translate(new Vector3(0, 0, -speed));
      imageTargetCentreZ -= speed;
    }
  }
}
