using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScrolling : MonoBehaviour {
    public float roof;
    public float floor;
    public float speed;
    private float LeftRight(float angle, float vector)
    {
        return Mathf.Cos(angle / 57.2958f) * vector;
    }
    
    private float ForwardBack(float angle, float vector)
    {
        return Mathf.Sin(angle / 57.2958f) * vector;
    }
    private void FixedUpdate() {

        if ((Input.mouseScrollDelta.y > 0 && transform.position.y > floor) ||
            (Input.mouseScrollDelta.y < 0 && transform.position.y < roof)) {
            transform.Translate(0, 0, Input.mouseScrollDelta.y % speed);
        }
       
    }
}
