using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class MoveGhostRoad : MonoBehaviour {
    private int idx = -1;
    private bool isFollow = true;
    private bool isBusy = false;
    private bool isFirst = false;
    private GameObject MainCamera;
    private Roads RoadsClass;

    void Start() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        RoadsClass = MainCamera.GetComponent <Roads> ();
        idx = RoadsClass.GetGhostIndex(gameObject);
    }

    void Update() {
        if (isFollow && !isBusy) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                RoadObject data = RoadsClass.GetGhostRoadObject(idx);
                if (isFirst) {
                    data.x1 = hit.point.x;
                    data.y1 = hit.point.z;
                    Vector2 point = RoadsClass.RoundMovingCoordinateOnTheRoad(data, RoadsClass.GetIdxGhostObjectConnect(idx));
                    data.x1 = point.x;
                    data.y1 = point.y;
                }
                else {
                    data.x2 = hit.point.x;
                    data.y2 = hit.point.z;
                }
                float x1, y1, x2, y2, len;
                x1 = data.x1;
                y1 = data.y1;
                x2 = data.x2;
                y2 = data.y2;
                len = data.len = (float)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
                transform.rotation = Quaternion.Euler(0, funcAngle(len, x2 - x1, x1, y1, x2, y2), 0);
                transform.position = new Vector3((x1 + x2) / 2, 0, (y1 + y2) / 2);
                transform.localScale = new Vector3(1, 1, len / 2);
                MeshRenderer MeshRendererClass = gameObject.GetComponent <MeshRenderer> ();
                MeshRendererClass.materials[0].SetTextureScale("_MainTex", new Vector2(len / 2, 1));
                if (Input.GetMouseButtonDown(0)) {
                    gameObject.layer = 0;
                    isFollow = false;
                    RoadsClass.SetIsFollowGhost(false);
                    RoadsClass.SetRoadType("");
                    RoadsClass.SetGhostRoadObject(data, idx);
                }
                if (Input.GetMouseButtonDown(1)) {
                    gameObject.layer = 0;
                    isFollow = false;
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
        if (!RoadsClass.GetIsFollowGhost() && RoadsClass.GetRoadType() == "") {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                RoadObject data = RoadsClass.GetGhostRoadObject(idx);
                float x = hit.point.x;
                float y = hit.point.z;
                float x1 = data.x1;
                float y1 = data.y1;
                float x2 = data.x2;
                float y2 = data.y2;
                float dist1 = (float)Math.Sqrt(Math.Pow(x - x1, 2) + Math.Pow(y - y1, 2));
                float dist2 = (float)Math.Sqrt(Math.Pow(x - x2, 2) + Math.Pow(y - y2, 2));
                if (dist1 < dist2) isFirst = true;
                else isFirst = false;
            }
            gameObject.layer = 2;
            isFollow = true;
            isBusy = true;
            RoadsClass.SetIsFollowGhost(true);
        }
    }

    private float funcAngle(float len, float leg, float x1, float y1, float x2, float y2) {
        if (len == 0) return 0;
        else {
            if (y1 <= y2) return (float)(90 - Math.Acos(leg / len) * 57.3);
            else return (float)(-270 + Math.Acos(leg / len) * 57.3);
        }
    }
}
