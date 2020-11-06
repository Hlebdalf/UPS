using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour {
    private Vector3 MovingVector = new Vector3(0, 0, 0);
    private bool IsMoving = false;

    public float XMovingMP;
    public float ZMovingMP;
    public float Sensivity1;
    public float Sensivity2;

    private void FixedUpdate() {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) {
            if (Input.GetKey(KeyCode.W))
                transform.position += new Vector3(0, 0, ZMovingMP);
            if (Input.GetKey(KeyCode.S))
                transform.position += new Vector3(0, 0, -ZMovingMP);
            if (Input.GetKey(KeyCode.A))
                transform.position += new Vector3(-XMovingMP, 0, 0);
            if (Input.GetKey(KeyCode.D))
                transform.position += new Vector3(XMovingMP, 0, 0);
        }
        else if (IsMoving) {
            float x = Input.mousePosition.x;
            float y = Input.mousePosition.y;
            float MovingDirectionX = MovingVector.x * (x - 980) / 16f * 9f / 1500;
            float MovingDirectionZ = MovingVector.z * (y - 540) / 1500;
            transform.position += new Vector3(MovingDirectionX / Sensivity1, 0, MovingDirectionZ / Sensivity2);
        }
    }

    public void SwitchDirection(string direction) {
        switch (direction) {
            case "Begin":
                MovingVector = new Vector3(1, 0, 1);
                IsMoving = true;
                break;
            case "Finish":
                IsMoving = false;
                MovingVector = new Vector3(0, 0, 0);
                break;
        }
    }
}
