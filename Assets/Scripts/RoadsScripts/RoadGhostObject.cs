﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RoadGhostObject : MonoBehaviour {
    private GameObject MainCamera;
    private Roads RoadsClass;
    private float eps = 1e-5f;
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
                    data.x1 = hit.point.x;
                    data.y1 = hit.point.z;
                    Vector2 point1 = RoundMovingCoordinateOnTheRoad(data, idx, connectedRoad);
                    Vector2 point2 = RoundCoodinate(point1);
                    data.x1 = point2.x;
                    data.y1 = point2.y;
                }
                else {
                    Vector2 point = RoundCoodinate(new Vector2(hit.point.x, hit.point.z));
                    data.x2 = point.x;
                    data.y2 = point.y;
                }

                data.len = (float)Math.Sqrt(Math.Pow(data.x2 - data.x1, 2) + Math.Pow(data.y2 - data.y1, 2));
                transform.rotation = Quaternion.Euler(0, RoadsClass.funcAngle(len, data.x2 - data.x1, data.x1, data.y1, data.x2, data.y2), 0);
                transform.position = new Vector3((data.x1 + data.x2) / 2, 0.2f, (data.y1 + data.y2) / 2);
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
                RoadGhostObject data = RoadsClass.ghostObjects[idx].GetComponent <RoadGhostObject> ();
                float dist1 = (float)Math.Sqrt(Math.Pow(hit.point.x - data.x1, 2) + Math.Pow(hit.point.z - data.y1, 2));
                float dist2 = (float)Math.Sqrt(Math.Pow(hit.point.x - data.x2, 2) + Math.Pow(hit.point.z - data.y2, 2));
                if (dist1 < dist2) isFirst = true;
                else isFirst = false;
            }
            gameObject.layer = 2;
            RoadsClass.isFollowGhost = isFollow = isBusy = true;
        }
    }

    public Vector2 RoundCoodinate(Vector2 point) {
        int gridSize = 1;
        float x = point.x, y = point.y, low, high;

        low = (int)(x / gridSize) * gridSize;
        high = low + gridSize;
        if (Math.Abs(x - low) < Math.Abs(x - high)) point.x = low;
        else point.x = high;
        
        low = (int)(y / gridSize) * gridSize;
        high = low + gridSize;
        if (Math.Abs(y - low) < Math.Abs(y - high)) point.y = low;
        else point.y = high;

        return point;
    }

    private Vector2 RoundMovingCoordinateOnTheRoad(RoadGhostObject dataGhost, int idxGhost, int idxRoad) {
        RoadObject data = RoadsClass.objects[idxRoad].GetComponent <RoadObject> ();
        float cursorX = dataGhost.x1;
        float cursorY = dataGhost.y1;

        float mainRoadA = data.y1 - data.y2, mainRoadB = data.x2 - data.x1, mainRoadC = data.x1 * data.y2 - data.x2 * data.y1; // main road line
        float ghostRoadA = dataGhost.y1 - dataGhost.y2, ghostRoadB = dataGhost.x2 - dataGhost.x1, ghostRoadC = dataGhost.x1 * dataGhost.y2 - dataGhost.x2 * dataGhost.y1; // ghost road line
        float mainRoadCrossGhostRoadX = -(mainRoadC * ghostRoadB - ghostRoadC * mainRoadB) / (mainRoadA * ghostRoadB - ghostRoadA * mainRoadB); // rounded coordinate
        float mainRoadCrossGhostRoadY = -(mainRoadA * ghostRoadC - ghostRoadA * mainRoadC) / (mainRoadA * ghostRoadB - ghostRoadA * mainRoadB); // rounded coordinate

        float normA = -mainRoadB, normB = mainRoadA, normC = -(normA * cursorX + normB * cursorY); // norm
        float normCrossMainRoadX = -(mainRoadC * normB - normC * mainRoadB) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate
        float normCrossMainRoadY = -(mainRoadA * normC - normA * mainRoadC) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate

        float minDist = (float)Math.Sqrt(Math.Pow(normCrossMainRoadX - cursorX, 2) + Math.Pow(normCrossMainRoadY - cursorY, 2));
        int minDistIdx = idxRoad;
        float bestLineA = mainRoadA, bestLineB = mainRoadB, bestLineC = mainRoadC;
        for (int i = 0; i < data.connectedRoads.Count; ++i) {
            RoadObject tmpData = RoadsClass.objects[data.connectedRoads[i]].GetComponent <RoadObject> ();

            float tmpA = tmpData.y1 - tmpData.y2, tmpB = tmpData.x2 - tmpData.x1, tmpC = tmpData.x1 * tmpData.y2 - tmpData.x2 * tmpData.y1; // tmp road line
            float tmpNormA = -tmpB, tmpNormB = tmpA, tmpNormC = -(tmpNormA * cursorX + tmpNormB * cursorY); // norm
            float tmpNormCrossTmpX = -(tmpC * tmpNormB - tmpNormC * tmpB) / (tmpA * tmpNormB - tmpNormA * tmpB); // rounded coordinate
            float tmpNormCrossTmpY = -(tmpA * tmpNormC - tmpNormA * tmpC) / (tmpA * tmpNormB - tmpNormA * tmpB); // rounded coordinate

            float tmpDist = (float)Math.Sqrt(Math.Pow(tmpNormCrossTmpX - cursorX, 2) + Math.Pow(tmpNormCrossTmpY - cursorY, 2));
            if (tmpDist < minDist) {
                minDist = tmpDist;
                minDistIdx = data.connectedRoads[i];
                bestLineA = tmpNormA;
                bestLineB = tmpNormB;
                bestLineC = tmpNormC;
            }
        }

        float x = -(bestLineC * ghostRoadB - ghostRoadC * bestLineB) / (bestLineA * ghostRoadB - ghostRoadA * bestLineB); // rounded coordinate
        float y = -(bestLineA * ghostRoadC - ghostRoadA * bestLineC) / (bestLineA * ghostRoadB - ghostRoadA * bestLineB); // rounded coordinate

        Vector2 ans = new Vector2(x, y);
        RoadsClass.ghostObjects[idxGhost].GetComponent <RoadGhostObject> ().connectedRoad = minDistIdx;
        float dist1 = (float)Math.Sqrt(Math.Pow(mainRoadCrossGhostRoadX - data.x1, 2) + Math.Pow(mainRoadCrossGhostRoadY - data.y1, 2));
        float dist2 = (float)Math.Sqrt(Math.Pow(mainRoadCrossGhostRoadX - data.x2, 2) + Math.Pow(mainRoadCrossGhostRoadY - data.y2, 2));
        float dist = (float)Math.Sqrt(Math.Pow(data.x2 - data.x1, 2) + Math.Pow(data.y2 - data.y1, 2));
        if (dist1 + dist2 - dist > eps) {
            if (dist1 < dist2) ans = new Vector2(data.x1, data.y1);
            else ans = new Vector2(data.x2, data.y2);
        }
        return ans;
    }
}
