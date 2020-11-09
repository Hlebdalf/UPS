﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGhostBuild : MonoBehaviour {
    private bool isFollow = true;
    private bool isBusy = false;
    private GameObject MainCamera;
    private Builds BuildsClass;

    void Start() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        BuildsClass = MainCamera.GetComponent <Builds> ();
    }

    void Update() {
        if (isFollow && !isBusy) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                transform.position = BuildsClass.RoundCoodinates(hit.point);
            }
            if (Input.GetMouseButtonDown(0)) {
                gameObject.layer = 0;
                isFollow = !isFollow;
                BuildsClass.SetIsFollowGhost(false);
            }
            if (Input.GetMouseButtonDown(1)) {
                gameObject.layer = 0;
                isFollow = !isFollow;
                BuildsClass.SetIsFollowGhost(false);
                BuildsClass.DeleteGhost(gameObject);
            }
        }
        isBusy = false;
    }

    void OnMouseOver() {
        if (!BuildsClass.GetIsFollowGhost() && Input.GetMouseButtonDown(1)) {
            BuildsClass.DeleteGhost(gameObject);
        }
    }

    void OnMouseDown() {
        if (!BuildsClass.GetIsFollowGhost()) {
            gameObject.layer = 2;
            isFollow = !isFollow;
            isBusy = true;
            BuildsClass.SetIsFollowGhost(true);
        }
    }
}
