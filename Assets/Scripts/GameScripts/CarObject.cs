using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class CarObject : MonoBehaviour {
    private GameObject MainCamera;
    private Cars CarsClass;
    private float rotate;
    private Vector3 vertexTo;
    private bool stage1 = true, stage2 = false, stage3 = false;
    private bool vertexIsActive = false;

    public Queue <Vector3> queuePointsToStart;
    public Queue <Vector3> queuePointsToEnd;
    public Queue <Vector3> queuePointsToParking;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        CarsClass = MainCamera.GetComponent <Cars> ();
        queuePointsToStart = new Queue <Vector3> ();
        queuePointsToEnd = new Queue <Vector3> ();
        queuePointsToParking = new Queue <Vector3> ();
    }

    private void Start() {
        transform.position = queuePointsToStart.Dequeue();
    }

    IEnumerator DelayAfterStage1() {
        yield return new WaitForSeconds(1);
        stage2 = true;
    }

    IEnumerator DelayAfterStage2() {
        yield return new WaitForSeconds(1);
        stage3 = true;
    }

    private void FixedUpdate() {
        if (vertexIsActive) {
            Vector3 vertexFrom = transform.position;
            float dist = (float)Math.Sqrt(Math.Pow(vertexTo.x - vertexFrom.x, 2) + Math.Pow(vertexTo.z - vertexFrom.z, 2));
            if (dist > CarsClass.eps) {
                float angle = (float)Math.Atan2(vertexTo.z - vertexFrom.z, vertexTo.x - vertexFrom.x);
                Vector3 move = new Vector3((float)Math.Cos(angle), 0, (float)Math.Sin(angle));
                transform.Find("Car").gameObject.transform.rotation = Quaternion.Euler(0, angle * -57.3f + 90f, 0);
                transform.Translate(move * CarsClass.speed * Time.fixedDeltaTime);
            }
            else vertexIsActive = false;
        }
        if (stage1) {
            if (!vertexIsActive) {
                if (queuePointsToStart.Count > 0) {
                    vertexTo = queuePointsToStart.Dequeue();
                    vertexIsActive = true;
                }
                else {
                    stage1 = false;
                    StartCoroutine(DelayAfterStage1());
                }
            }
        }
        else if (stage2) {
            if (!vertexIsActive) {
                if (queuePointsToEnd.Count > 0) {
                    vertexTo = queuePointsToEnd.Dequeue();
                    vertexIsActive = true;
                }
                else {
                    stage2 = false;
                    StartCoroutine(DelayAfterStage2());
                }
            }
        }
        else if (stage3) {
            if (!vertexIsActive) {
                if (queuePointsToParking.Count > 0) {
                    vertexTo = queuePointsToParking.Dequeue();
                    vertexIsActive = true;
                }
                else {
                    stage3 = false;
                    CarsClass.DeleteObject(gameObject);
                }
            }
        }
    }
}
