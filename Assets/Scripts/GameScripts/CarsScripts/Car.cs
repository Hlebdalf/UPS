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
    public int mainNumOfLane = 2, numOfLane = 2, idxRoad = -1;
    public bool onVisibleInCamera = false, isFollowTheFront = false, isStop = false, inCrossroad = false, canChangeLane = false;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraCollider = MainCamera.transform.Find("CameraCollider").gameObject;
        CarsClass = MainCamera.GetComponent <Cars> ();
        mainSpeed = speed = UnityEngine.Random.Range(3f, 6f);
    }

    private void Update() {
        if (onVisibleInCamera) {
            isFollowTheFront = isStop = false;
            RaycastHit[] hits = Physics.RaycastAll(transform.position, transform.forward, Distance);
            for (int i = 0; i < hits.Length; ++i) {
                RaycastHit hit = hits[i];
                if (hit.collider.gameObject.tag == "Car") {
                    Car carClass = hit.collider.gameObject.GetComponent <Car> ();
                    if (carClass.idxRoad == idxRoad || (carClass.inCrossroad && inCrossroad)) {
                        isFollowTheFront = true;
                        speedTheFront = carClass.speed;
                    }
                }
                else if (hit.collider.gameObject.tag == "TrafficLight" && !inCrossroad) {
                    CrossroadObject crossroadObjectClass = hit.collider.gameObject.GetComponent <CrossroadObject> ();
                    if (crossroadObjectClass.idxRoadGO == idxRoad || idxRoad < 0 || crossroadObjectClass.idxRoadGO < 0) isStop = false;
                    else isStop = true;
                }
            }
            if (isStop) speed = Math.Max(speed - braking, 0f);
            else if (isFollowTheFront) speed = Math.Max(Math.Max(speedTheFront - 1f, speed - braking), 0f);
            else speed = Math.Min(mainSpeed, speed + acceleration);

            if (mainNumOfLane != numOfLane) {
                bool p = true;
                Vector3 dir = transform.forward;
                if (numOfLane > mainNumOfLane)  dir = transform.right;
                else dir = transform.right;
                Ray ray = new Ray(transform.position, dir);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 0.5f)) {
                    if (hit.collider.gameObject.name == "SideCar") p = false;
                }
                canChangeLane = p;
                if (!p) speed = Math.Max(speed - 0.5f, 0f);
            }
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
}
