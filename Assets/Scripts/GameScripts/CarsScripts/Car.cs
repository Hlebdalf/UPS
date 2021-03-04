using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Car : MonoBehaviour {
    private GameObject MainCamera;
    private GameObject CameraCollider;
    private Cars CarsClass;
    private const float Distance = 1.5f;

    public float speed, mainSpeed = 10f, speedTheFront = 0f;
    public float acceleration = 0.5f, braking = 0.5f;
    public int numOfLane = 0, idxRoad = -1;
    public bool onVisibleInCamera = false, isFollowTheFront = false, isStop = false, inCrossroad = false;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraCollider = MainCamera.transform.Find("CameraCollider").gameObject;
        CarsClass = MainCamera.GetComponent <Cars> ();
        mainSpeed = speed = UnityEngine.Random.Range(5f, 10f);
        numOfLane = (int)UnityEngine.Random.Range(1f, 2.99f);
    }

    private void Update() {
        if (onVisibleInCamera) {
            isFollowTheFront = isStop = false;
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, Distance);
            for (int i = 0; i < hits.Length; ++i) {
                RaycastHit hit = hits[i];
                if (hit.collider.gameObject.tag == "Car") {
                    isFollowTheFront = true;
                    speedTheFront = hit.collider.gameObject.GetComponent <Car> ().speed;
                }
                else if (hit.collider.gameObject.tag == "TrafficLight" && !inCrossroad) {
                    CrossroadObject crossroadObjectClass = hit.collider.gameObject.GetComponent <CrossroadObject> ();
                    if (crossroadObjectClass.idxRoadGO == idxRoad || idxRoad < 0 || crossroadObjectClass.idxRoadGO < 0) isStop = false;
                    else isStop = true;
                }
            }
            if (isStop) speed = 0f;
            else if (isFollowTheFront) speed = Math.Max(Math.Min(speedTheFront - braking, speed), 0f);
            else speed = Math.Min(mainSpeed, speed + acceleration);
        }
        else speed = mainSpeed;
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject == CameraCollider) {
            onVisibleInCamera = true;
        }
        if (collider.gameObject.tag == "TrafficLight") {
            inCrossroad = true;
        }
    }

    private void OnTriggerExit(Collider collider) {
        if (collider.gameObject == CameraCollider) {
            onVisibleInCamera = false;
        }
        if (collider.gameObject.tag == "TrafficLight") {
            inCrossroad = false;
        }
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) CarsClass.SetLinePath(gameObject);
    }
}
