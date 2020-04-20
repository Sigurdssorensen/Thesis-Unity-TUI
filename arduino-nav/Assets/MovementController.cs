using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
  protected GameObject imageTarget;
  public GameObject home;
  public GameObject camera;
  public CameraController cameraController;

  public float speed = 0.25f;
  public float speedIncrease = 0.5f;

  protected float imageTargetSquareSize = 0.03f;
  protected float imageTargetCentreX = 149.5779f;
  protected float imageTargetCentreZ = 37.7945491f + 10f;
  protected float imageTargetRealtimeX;
  protected float imageTargetRealtimeZ;
  public float imageTargetSquaresX;
  public float imageTargetSquaresZ;

  public float vectorX = 0f;
  public float vectorZ = 0f;
  public float rotationY = 0f;

  // Start is called before the first frame update
  void Start()
  {
    imageTarget = GameObject.Find("ImageTarget");
    home = GameObject.Find("Home");
    cameraController = camera.GetComponent<CameraController>();
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

    home.transform.rotation = Quaternion.Euler(
      0f,
      rotationY,
      0f
    );

    rotationY = cameraController.digitalY;

    home.transform.Translate(new Vector3(vectorX, 0f, 0f));
    home.transform.Translate(new Vector3(0f, 0f, vectorZ));

    imageTargetCentreX += vectorX;
    imageTargetCentreZ += vectorZ;

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
    if(imageTargetSquaresX >= 2 && imageTargetSquaresX <= 10) {
      vectorX = speed + speedIncrease*(imageTargetSquaresX-2);
    } else if (imageTargetSquaresX <= -2 && imageTargetSquaresX >= -10) {
      vectorX = -(speed + speedIncrease*((imageTargetSquaresX*-1)-2));
    } else {
      vectorX = 0f;
    }
  }

  void UpdateZPosition() 
  {
    if(imageTargetSquaresZ >= 2 && imageTargetSquaresZ <= 10) {
      vectorZ = speed + speedIncrease*(imageTargetSquaresZ-2);
    } else if (imageTargetSquaresZ <= -2 && imageTargetSquaresZ >= -10) {
      vectorZ = -(speed + speedIncrease*((imageTargetSquaresZ*-1)-2));
    } else {
      vectorZ = 0f;
    }
  }
}
