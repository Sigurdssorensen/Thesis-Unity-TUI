using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowAlong : MonoBehaviour
{
  public GameObject self;
  public GameObject home;
  public float offsetZ;
  public float offsetY;
    // Start is called before the first frame update
    void Start()
    {
      self = GameObject.Find("ARCamera");
      offsetZ = 9.55f;
      offsetY = 70f;
    }

    // Update is called once per frame
    void Update()
    {
        self.transform.position = new Vector3(home.transform.position.x, offsetY, home.transform.position.y);
    }
}
