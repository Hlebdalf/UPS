using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScrolling : MonoBehaviour
{
    public float roof;
    public float floor;
    public float speed;
    void Start()
    {
        
    }

    void FixedUpdate()
    {
        if (Input.mouseScrollDelta.y > 0 && transform.position.y > floor)
        {
            transform.position += new Vector3(0, -Input.mouseScrollDelta.y % speed, Input.mouseScrollDelta.y % speed);
        }
        if (Input.mouseScrollDelta.y < 0 && transform.position.y < roof)
        {
            transform.position += new Vector3(0, -Input.mouseScrollDelta.y % speed, Input.mouseScrollDelta.y % speed);
        }
    }
}
