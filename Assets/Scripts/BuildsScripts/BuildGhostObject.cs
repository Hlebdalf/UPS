using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildGhostObject : MonoBehaviour {
    private GameObject MainCamera;
    private Builds BuildsClass;
    private bool isFollow = true, isBusy = false;

    public float x, y;
    public int idx, idxPreFub, connectedRoad = -1;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        BuildsClass = MainCamera.GetComponent <Builds> ();
    }

    private void Start() {
        gameObject.layer = 2;
    }

    private void Update() {
        if (isFollow && !isBusy) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                transform.position = hit.point;
            }
            if (Input.GetMouseButtonDown(0)) {
                gameObject.layer = 0;
                BuildsClass.isFollowGhost = isFollow = false;
            }
            if (Input.GetMouseButtonDown(1)) {
                gameObject.layer = 0;
                BuildsClass.isFollowGhost = isFollow = false;
                BuildsClass.DeleteGhost(gameObject);
            }
        }
        isBusy = false;
    }

    private void OnMouseOver() {
        if (!BuildsClass.isFollowGhost && Input.GetMouseButtonDown(1)) {
            BuildsClass.DeleteGhost(gameObject);
        }
    }

    private void OnMouseDown() {
        if (!BuildsClass.isFollowGhost) {
            gameObject.layer = 2;
            BuildsClass.isFollowGhost = isFollow = isBusy = true;
        }
    }
}
