using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transparent : MonoBehaviour
{
  public SpriteRenderer sprite;
  public Color color;
    // Start is called before the first frame update
    void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        sprite.color = new Color(0.9f, 0.8f, 0.2f, 0.4f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
