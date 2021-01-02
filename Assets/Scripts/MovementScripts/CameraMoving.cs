using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour {
    private Vector3 MovingVector = new Vector3(0, 0, 0);
    private bool IsMoving = false;

    public float XMovingMP;
    public float ZMovingMP;
    public float Sensivity1 = 300;
    public float Sensivity2 = 300;
    public float speed = 60f;

    private void Update() {
        float koeff = Time.deltaTime * speed * Mathf.Pow(XMovingMP * (transform.position.y + 15) / 10f, 2);
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A)) {
            if (Input.GetKey(KeyCode.W))
                transform.Translate(0, 0, ZMovingMP * koeff);
            if (Input.GetKey(KeyCode.S))
                transform.Translate(0, 0, -ZMovingMP * koeff);
            if (Input.GetKey(KeyCode.A))
                transform.Translate(-XMovingMP * koeff, 0, 0);
            if (Input.GetKey(KeyCode.D))
                transform.Translate(XMovingMP * koeff, 0, 0);
        }
        else if (IsMoving) {
            float x = Input.mousePosition.x;
            float y = Input.mousePosition.y;
            float MovingDirectionX = MovingVector.x * (x - Screen.width / 2) / 16f * 9f / 1500;
            float MovingDirectionZ = MovingVector.z * (y - Screen.height / 2) / 1500;
            transform.Translate(MovingDirectionX / Sensivity1 * koeff, 0, MovingDirectionZ / Sensivity2 * koeff);
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
