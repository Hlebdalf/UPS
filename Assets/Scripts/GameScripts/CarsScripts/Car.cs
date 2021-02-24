using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Car : MonoBehaviour {
    private GameObject MainCamera;
    private GameObject CameraCollider;
    private const float Distance = 4f;

    public float speed, mainSpeed = 10f;
    public int numOfLane = 0;
    public bool onVisibleInCamera = false, isFollowTheFront = false, isStop = false;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraCollider = MainCamera.transform.Find("CameraCollider").gameObject;
        mainSpeed = speed = UnityEngine.Random.Range(5f, 10f);
        numOfLane = (int)UnityEngine.Random.Range(1f, 2.99f);
    }

    private void Update() {
        if (onVisibleInCamera) {
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, Distance)) {
                if (hit.collider.gameObject.tag == "TrafficLight") {
                    // isStop = true;
                    isFollowTheFront = false;
                }
                else if (hit.collider.gameObject.tag == "Car" && hit.distance <= 2f) isFollowTheFront = true;
                else {
                    isFollowTheFront = false;
                    isStop = false;
                }
            }
            else {
                isFollowTheFront = false;
                isStop = false;
            }
            if (isStop) speed = 0f;
            else if (isFollowTheFront) speed = Math.Max(Math.Min(hit.collider.gameObject.GetComponent <Car> ().speed - 1f, mainSpeed), 0f);
            else speed = mainSpeed;
            Debug.DrawRay(ray.origin, ray.direction * Distance, Color.red);
        }
        else speed = mainSpeed;
    }

    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject == CameraCollider) {
            onVisibleInCamera = true;
        }
    }

    private void OnTriggerExit(Collider collider) {
        if (collider.gameObject == CameraCollider) {
            onVisibleInCamera = false;
        }
    }
}
