using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RoadGhostObject : MonoBehaviour {
    private GameObject MainCamera;
    private Roads RoadsClass;
    private bool isFollow = true;
    private bool isBusy = false;
    private bool isFirst = false;

    public float x1, y1, x2, y2, len;
    public int idx;
    public int connectedRoad;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        RoadsClass = MainCamera.GetComponent <Roads> ();
    }

    private void Update() {
        if (isFollow && !isBusy) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                RoadGhostObject data = gameObject.GetComponent <RoadGhostObject> ();
                if (isFirst) {
                    data.x1 = (int)hit.point.x;
                    data.y1 = hit.point.z;
                    Vector2 point = RoadsClass.RoundMovingCoordinateOnTheRoad(data, idx, connectedRoad);
                    data.x1 = point.x;
                    data.y1 = point.y;
                }
                else {
                    data.x2 = hit.point.x;
                    data.y2 = hit.point.z;
                }

                data.len = (float)Math.Sqrt(Math.Pow(data.x2 - data.x1, 2) + Math.Pow(data.y2 - data.y1, 2));
                transform.rotation = Quaternion.Euler(0, RoadsClass.funcAngle(len, data.x2 - data.x1, data.x1, data.y1, data.x2, data.y2), 0);
                transform.position = new Vector3((data.x1 + data.x2) / 2, 0, (data.y1 + data.y2) / 2);
                transform.localScale = new Vector3(1, 1, data.len / 2);

                MeshRenderer MeshRendererClass = gameObject.GetComponent <MeshRenderer> ();
                MeshRendererClass.materials[0].SetTextureScale("_MainTex", new Vector2(data.len / 2, 1));

                if (Input.GetMouseButtonDown(0)) {
                    gameObject.layer = 0;
                    RoadsClass.isFollowGhost = isFollow = false;
                    RoadsClass.RoadType = "";
                }

                if (Input.GetMouseButtonDown(1)) {
                    gameObject.layer = 0;
                    RoadsClass.isFollowGhost = isFollow = false;
                    RoadsClass.RoadType = "";
                    RoadsClass.DeleteGhost(gameObject);
                }
            }
        }
        isBusy = false;
    }

    private void OnMouseDown() {
        if (!RoadsClass.isFollowGhost && RoadsClass.RoadType == "") {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                RoadObject data = RoadsClass.ghostObjects[idx].GetComponent <RoadObject> ();
                float dist1 = (float)Math.Sqrt(Math.Pow(hit.point.x - data.x1, 2) + Math.Pow(hit.point.z - data.y1, 2));
                float dist2 = (float)Math.Sqrt(Math.Pow(hit.point.x - data.x2, 2) + Math.Pow(hit.point.z - data.y2, 2));
                if (dist1 < dist2) isFirst = true;
                else isFirst = false;
            }
            gameObject.layer = 2;
            RoadsClass.isFollowGhost = isFollow = isBusy = true;
        }
    }
}
