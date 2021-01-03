using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CarObject : MonoBehaviour {
    private GameObject MainCamera;
    private Cars CarsClass;
    private float rotate;
    private Vector3 vertexTo;
    private bool vertexIsActive = false;

    public Queue <Vector3> queuePoints;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        CarsClass = MainCamera.GetComponent <Cars> ();
        queuePoints = new Queue <Vector3> ();
    }

    private void Start() {
        // Debug.Log("Start: (" + queuePoints.Peek().x + " : " + queuePoints.Peek().z + ")");
        transform.position = queuePoints.Dequeue();
    }

    private void FixedUpdate() {
        if (vertexIsActive) {
            Vector3 vertexFrom = transform.position;
            float dist = (float)Math.Sqrt(Math.Pow(vertexTo.x - vertexFrom.x, 2) + Math.Pow(vertexTo.z - vertexFrom.z, 2));
            if (dist > CarsClass.eps) {
                float angle = (float)Math.Atan2(vertexTo.z - vertexFrom.z, vertexTo.x - vertexFrom.x);
                Vector3 move = new Vector3((float)Math.Cos(angle), 0, (float)Math.Sin(angle));
                transform.Translate(move * CarsClass.speed * Time.fixedDeltaTime);
            }
            else vertexIsActive = false;
        }
        if (!vertexIsActive) {
            if (queuePoints.Count > 0) {
                // Debug.Log("Next: (" + queuePoints.Peek().x + " : " + queuePoints.Peek().z + ")");
                vertexTo = queuePoints.Dequeue();
                vertexIsActive = true;
            }
            else CarsClass.DeleteObject(gameObject);
        }
    }
}
