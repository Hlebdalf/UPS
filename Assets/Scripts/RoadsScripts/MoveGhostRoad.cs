using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MoveGhostRoad : MonoBehaviour {
    private bool isFollow = true;
    private bool isBusy = false;
    private GameObject MainCamera;
    private Roads RoadsClass;

    void Start() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        RoadsClass = MainCamera.GetComponent <Roads> ();
    }

    void Update() {
        if (isFollow && !isBusy) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                Pair <float, float> p1 = RoadsClass.GetFirstCoordinatesGhostObject(gameObject);
                float x1 = p1.First;
                float y1 = p1.Second;
                float x2 = hit.point.x;
                float y2 = hit.point.z;
                float dist = (float)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
                float leg = x2 - x1;
                transform.rotation = Quaternion.Euler(0, funcK(dist, leg, x1, y1, x2, y2), 0);
                transform.position = new Vector3((x1 + x2) / 2, 0, (y1 + y2) / 2);
                transform.localScale = new Vector3(1, 1, dist / 2);
                MeshRenderer MeshRendererClass = gameObject.GetComponent <MeshRenderer> ();
                MeshRendererClass.materials[0].SetTextureScale("_MainTex", new Vector2(dist / 2, 1));
                if (Input.GetMouseButtonDown(0)) {
                    gameObject.layer = 0;
                    isFollow = !isFollow;
                    RoadsClass.SetIsFollowGhost(false);
                    RoadsClass.SetRoadType("");
                    RoadsClass.SetSecondCoordinatesGhostObject(gameObject, hit.point);
                    RoadsClass.SetLenGhostObject(gameObject, dist);
                }
                if (Input.GetMouseButtonDown(1)) {
                    gameObject.layer = 0;
                    isFollow = !isFollow;
                    RoadsClass.SetIsFollowGhost(false);
                    RoadsClass.SetRoadType("");
                    RoadsClass.DeleteGhost(gameObject);
                }
            }
        }
        isBusy = false;
    }

    void OnMouseOver() {
        if (!RoadsClass.GetIsFollowGhost() && Input.GetMouseButtonDown(1)) {
            RoadsClass.DeleteGhost(gameObject);
        }
    }

    void OnMouseDown() {
        if (!RoadsClass.GetIsFollowGhost()) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
                RoadsClass.SetFirstCoordinatesGhostObject(gameObject, hit.point);
            gameObject.layer = 2;
            isFollow = !isFollow;
            isBusy = true;
            RoadsClass.SetIsFollowGhost(true);
        }
    }

    private float funcK(float dist, float leg, float x1, float y1, float x2, float y2) {
        if (dist == 0) return 0;
        else {
            if (y1 <= y2) return (float)(90 - Math.Acos(leg / dist) * 57.3);
            else return (float)(-270 + Math.Acos(leg / dist) * 57.3);
        }
    }
}
