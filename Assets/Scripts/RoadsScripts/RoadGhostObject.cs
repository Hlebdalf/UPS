using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RoadGhostObject : MonoBehaviour {
    private GameObject MainCamera;
    private GameObject CameraCollider;
    private Roads RoadsClass;
    private Builds BuildsClass;
    private Field FieldClass;
    private GameObject correctObject;
    private GameObject incorrectObject;
    private float eps = 1e-5f;
    public bool isBusy = false, isFirst = false, isCollision = false;

    public GameObject InterfaceObject;
    public float x1, y1, x2, y2, len, angle;
    public int idx, idxPreFub, connectedRoad, connectedRoad2 = -1;
    public long prevCost = 0;
    public bool isFollow = true;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        CameraCollider = MainCamera.transform.Find("CameraCollider").gameObject;
        RoadsClass = MainCamera.GetComponent <Roads> ();
        BuildsClass = MainCamera.GetComponent <Builds> ();
        FieldClass = MainCamera.GetComponent <Field> ();
        correctObject = gameObject.transform.Find("Correct").gameObject;
        incorrectObject = gameObject.transform.Find("Incorrect").gameObject;
    }

    private void Update() {
        if (isFollow && !isBusy) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (isFirst) {
                    x1 = hit.point.x;
                    y1 = hit.point.z;
                    Vector2 point1 = RoundMovingCoordinateOnTheRoad(idx, connectedRoad);
                    Vector2 point2 = RoundCoodinate(point1);
                    x1 = point2.x;
                    y1 = point2.y;
                }
                else {
                    if (connectedRoad2 == -1) {
                        Vector2 point = RoundCoodinate(new Vector2(hit.point.x, hit.point.z));
                        x2 = point.x;
                        y2 = point.y;
                    }
                    else {
                        Vector3 point = RoadsClass.RoundCoordinateOnTheRoad(hit.point, connectedRoad2);
                        x2 = point.x;
                        y2 = point.z;
                    }
                }

                len = (float)Math.Sqrt(Math.Pow(x2 - x1, 2) + Math.Pow(y2 - y1, 2));
                RoadsClass.cost -= prevCost;
                RoadsClass.cost += (long)(len * RoadsClass.costForMetr);
                prevCost = (long)(len * RoadsClass.costForMetr);
                angle = (float)Math.Acos((x2 - x1) / len);
                transform.rotation = Quaternion.Euler(0, RoadsClass.funcAngle(len, x2 - x1, x1, y1, x2, y2), 0);
                transform.position = new Vector3((x1 + x2) / 2, 0.2f, (y1 + y2) / 2);
                transform.localScale = new Vector3(1, 1, len / 2);

                Renderer rend = gameObject.GetComponent <Renderer> ();
                rend.material.mainTextureScale = new Vector2(1, len / 2);

                if (isCollision || !CheckAngle()) {
                    correctObject.SetActive(false);
                    incorrectObject.SetActive(true);
                }
                else {
                    correctObject.SetActive(true);
                    incorrectObject.SetActive(false);
                }

                if (Input.GetMouseButtonDown(0) && !isCollision && CheckAngle()) {
                    RoadsClass.InterfaceClass.ActivateMenu();
                    gameObject.layer = 0;
                    RoadsClass.isFollowGhost = isFollow = false;
                    RoadsClass.RoadType = "";
                }

                if (Input.GetMouseButtonDown(1)) {
                    RoadsClass.InterfaceClass.ActivateMenu();
                    RoadsClass.isFollowGhost = isFollow = false;
                    RoadsClass.RoadType = "";
                    RoadsClass.cost -= prevCost;
                    RoadsClass.DeleteGhost(gameObject);
                }
            }
        }
        isBusy = false;
    }

    private void OnTriggerStay(Collider other) {
        if (other.gameObject == CameraCollider) return;
        if (other.gameObject.GetComponent <RoadObject> ()) {
            int idxCollision = RoadsClass.objects.IndexOf(other.gameObject);
            Vector2 point = RoundCoodinate(new Vector2(x1, y1));
            if (FieldClass.objects[(int)point.x + FieldClass.fieldSizeHalf, (int)point.y + FieldClass.fieldSizeHalf] == null) {
                if (idxCollision != connectedRoad) {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit)) {
                        RoadObject dataRoad = RoadsClass.objects[idxCollision].GetComponent <RoadObject> ();
                        float mainRoadA = dataRoad.y1 - dataRoad.y2, mainRoadB = dataRoad.x2 - dataRoad.x1, mainRoadC = dataRoad.x1 * dataRoad.y2 - dataRoad.x2 * dataRoad.y1; // main road line
                        float normA = -mainRoadB, normB = mainRoadA, normC = -(normA * hit.point.x + normB * hit.point.z); // norm
                        float normCrossMainRoadX = -(mainRoadC * normB - normC * mainRoadB) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate
                        float normCrossMainRoadY = -(mainRoadA * normC - normA * mainRoadC) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate
                        float dist = (float)Math.Sqrt(Math.Pow(normCrossMainRoadX - hit.point.x, 2) + Math.Pow(normCrossMainRoadY - hit.point.z, 2));
                        if (isFollow) {
                            if (dist < 2) {
                                isCollision = false;
                                connectedRoad2 = idxCollision;
                            }
                            else {
                                isCollision = true;
                                connectedRoad2 = -1;
                            }
                        }
                    }
                }
            }
            else {
                CrossroadObject crossroad = FieldClass.objects[(int)point.x + FieldClass.fieldSizeHalf, (int)point.y + FieldClass.fieldSizeHalf].GetComponent <CrossroadObject> ();
                bool p = true;
                for (int i = 0; i < crossroad.connectedRoads.Count; ++i) {
                    if (crossroad.connectedRoads[i] == idxCollision) p = false;
                }
                if (p) {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit)) {
                        RoadObject dataRoad = RoadsClass.objects[idxCollision].GetComponent <RoadObject> ();
                        float mainRoadA = dataRoad.y1 - dataRoad.y2, mainRoadB = dataRoad.x2 - dataRoad.x1, mainRoadC = dataRoad.x1 * dataRoad.y2 - dataRoad.x2 * dataRoad.y1; // main road line
                        float normA = -mainRoadB, normB = mainRoadA, normC = -(normA * hit.point.x + normB * hit.point.z); // norm
                        float normCrossMainRoadX = -(mainRoadC * normB - normC * mainRoadB) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate
                        float normCrossMainRoadY = -(mainRoadA * normC - normA * mainRoadC) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate
                        float dist = (float)Math.Sqrt(Math.Pow(normCrossMainRoadX - hit.point.x, 2) + Math.Pow(normCrossMainRoadY - hit.point.z, 2));
                        if (isFollow) {
                            if (dist < 2) {
                                isCollision = false;
                                connectedRoad2 = idxCollision;
                            }
                            else {
                                isCollision = true;
                                connectedRoad2 = -1;
                            }
                        }
                    }
                }
            }
        }
        else if (other.gameObject.GetComponent <RoadGhostObject> () || other.gameObject.GetComponent <BuildObject> () || other.gameObject.GetComponent <BuildGhostObject> ()) {
            isCollision = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject != CameraCollider) {
            isCollision = false;
            connectedRoad2 = -1;
        }
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(1) && !BuildsClass.isFollowGhost && !RoadsClass.isFollowGhost && BuildsClass.ghostObjects.Count == 0) {
            RoadsClass.isFollowGhost = isFollow = false;
            RoadsClass.RoadType = "";
            RoadsClass.cost -= prevCost;
            RoadsClass.DeleteGhost(gameObject);
        }
    }

    private void OnMouseDown() {
        if (!RoadsClass.isFollowGhost && RoadsClass.RoadType == "" && BuildsClass.ghostObjects.Count == 0) {
            RoadsClass.InterfaceClass.DeactivateAllMenu();
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
        // float x = point.x, y = point.y, low, high;

        // low = (int)(x / FieldClass.gridSize) * FieldClass.gridSize;
        // high = low + FieldClass.gridSize;
        // if (Math.Abs(x - low) < Math.Abs(x - high)) point.x = low;
        // else point.x = high;
        
        // low = (int)(y / FieldClass.gridSize) * FieldClass.gridSize;
        // high = low + FieldClass.gridSize;
        // if (Math.Abs(y - low) < Math.Abs(y - high)) point.y = low;
        // else point.y = high;

        point.x = Mathf.Round(point.x);
        point.y = Mathf.Round(point.y);

        return point;
    }

    private Vector2 RoundMovingCoordinateOnTheRoad(int idxGhost, int idxRoad) {
        RoadObject data = RoadsClass.objects[idxRoad].GetComponent <RoadObject> ();
        float cursorX = x1;
        float cursorY = y1;

        float mainRoadA = data.y1 - data.y2, mainRoadB = data.x2 - data.x1, mainRoadC = data.x1 * data.y2 - data.x2 * data.y1; // main road line
        float ghostRoadA = y1 - y2, ghostRoadB = x2 - x1, ghostRoadC = x1 * y2 - x2 * y1; // ghost road line
        float mainRoadCrossGhostRoadX = -(mainRoadC * ghostRoadB - ghostRoadC * mainRoadB) / (mainRoadA * ghostRoadB - ghostRoadA * mainRoadB); // rounded coordinate
        float mainRoadCrossGhostRoadY = -(mainRoadA * ghostRoadC - ghostRoadA * mainRoadC) / (mainRoadA * ghostRoadB - ghostRoadA * mainRoadB); // rounded coordinate

        float normA = -mainRoadB, normB = mainRoadA, normC = -(normA * cursorX + normB * cursorY); // norm
        float normCrossMainRoadX = -(mainRoadC * normB - normC * mainRoadB) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate
        float normCrossMainRoadY = -(mainRoadA * normC - normA * mainRoadC) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate

        float minDist = (float)Math.Sqrt(Math.Pow(normCrossMainRoadX - cursorX, 2) + Math.Pow(normCrossMainRoadY - cursorY, 2));
        int minDistIdx = idxRoad;
        float bestLineA = mainRoadA, bestLineB = mainRoadB, bestLineC = mainRoadC;
        for (int i = 0; i < data.connectedCrossroads.Count; ++i) {
            for (int j = 0; j < RoadsClass.crossroads[data.connectedCrossroads[i]].GetComponent <CrossroadObject> ().connectedRoads.Count; ++j) {
                if (RoadsClass.crossroads[data.connectedCrossroads[i]].GetComponent <CrossroadObject> ().connectedRoads[j] == connectedRoad) continue;
                RoadObject tmpData = RoadsClass.objects[RoadsClass.crossroads[data.connectedCrossroads[i]].GetComponent <CrossroadObject> ().connectedRoads[j]].GetComponent <RoadObject> ();

                float tmpA = tmpData.y1 - tmpData.y2, tmpB = tmpData.x2 - tmpData.x1, tmpC = tmpData.x1 * tmpData.y2 - tmpData.x2 * tmpData.y1; // tmp road line
                float tmpNormA = -tmpB, tmpNormB = tmpA, tmpNormC = -(tmpNormA * cursorX + tmpNormB * cursorY); // norm
                float tmpNormCrossTmpX = -(tmpC * tmpNormB - tmpNormC * tmpB) / (tmpA * tmpNormB - tmpNormA * tmpB); // rounded coordinate
                float tmpNormCrossTmpY = -(tmpA * tmpNormC - tmpNormA * tmpC) / (tmpA * tmpNormB - tmpNormA * tmpB); // rounded coordinate

                float tmpDist1 = (float)Math.Sqrt(Math.Pow(tmpNormCrossTmpX - tmpData.x1, 2) + Math.Pow(tmpNormCrossTmpY - tmpData.y1, 2));
                float tmpDist2 = (float)Math.Sqrt(Math.Pow(tmpNormCrossTmpX - tmpData.x2, 2) + Math.Pow(tmpNormCrossTmpY - tmpData.y2, 2));
                float tmpLen = (float)Math.Sqrt(Math.Pow(tmpData.x2 - tmpData.x1, 2) + Math.Pow(tmpData.y2 - tmpData.y1, 2));
                if (tmpDist1 + tmpDist2 - tmpLen > eps) {
                    if (tmpDist1 < tmpDist2) {
                        tmpNormCrossTmpX = tmpData.x1;
                        tmpNormCrossTmpY = tmpData.y1;
                    }
                    else {
                        tmpNormCrossTmpX = tmpData.x2;
                        tmpNormCrossTmpY = tmpData.y2;
                    }
                }

                float tmpDist = (float)Math.Sqrt(Math.Pow(tmpNormCrossTmpX - cursorX, 2) + Math.Pow(tmpNormCrossTmpY - cursorY, 2));
                if (tmpDist < minDist) {
                    minDist = tmpDist;
                    minDistIdx = RoadsClass.crossroads[data.connectedCrossroads[i]].GetComponent <CrossroadObject> ().connectedRoads[j];
                    bestLineA = tmpNormA;
                    bestLineB = tmpNormB;
                    bestLineC = tmpNormC;
                }
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

    private bool CheckAngle() {
        if (connectedRoad == -1) return false;
        float angle = (float)Math.Atan2(y2 - y1, x2 - x1) * 57.3f;
        if (FieldClass.objects[(int)Mathf.Round(x1) + FieldClass.fieldSizeHalf, (int)Mathf.Round(y1) + FieldClass.fieldSizeHalf] != null &&
            FieldClass.objects[(int)Mathf.Round(x1) + FieldClass.fieldSizeHalf, (int)Mathf.Round(y1) + FieldClass.fieldSizeHalf].GetComponent <CrossroadObject> ()) {
            CrossroadObject crossroadClass = FieldClass.objects[(int)Mathf.Round(x1) + FieldClass.fieldSizeHalf, (int)Mathf.Round(y1) + FieldClass.fieldSizeHalf].GetComponent <CrossroadObject> ();
            for (int i = 0; i < crossroadClass.connectedRoads.Count; ++i) {
                RoadObject roadClass = RoadsClass.objects[crossroadClass.connectedRoads[i]].GetComponent <RoadObject> ();
                if ((Math.Abs(crossroadClass.x - roadClass.x1) > 1 || Math.Abs(crossroadClass.y - roadClass.y1) > 1) &&
                    (Math.Abs(crossroadClass.x - roadClass.x2) > 1 || Math.Abs(crossroadClass.y - roadClass.y2) > 1)) {
                    float angleRoad1 = Mathf.Atan2(roadClass.y2 - roadClass.y1, roadClass.x2 - roadClass.x1) * 57.3f;
                    float angleRoad2 = Mathf.Atan2(roadClass.y1 - roadClass.y2, roadClass.x1 - roadClass.x2) * 57.3f;
                    if (Math.Abs(angle - angleRoad1) < 40 || Math.Abs(angle - angleRoad2) < 40) return false;
                }
                else {
                    float roadX1 = roadClass.x1;
                    float roadY1 = roadClass.y1;
                    float roadX2 = roadClass.x2;
                    float roadY2 = roadClass.y2;
                    if (Math.Sqrt(Math.Pow(x1 - roadX1, 2) + Math.Pow(y1 - roadY1, 2)) > Math.Sqrt(Math.Pow(x1 - roadX2, 2) + Math.Pow(y1 - roadY2, 2))) {
                        float tmpX = roadX1, tmpY = roadY1;
                        roadX1 = roadX2;
                        roadY1 = roadY2;
                        roadX2 = tmpX;
                        roadY2 = tmpY;
                    }
                    float angleRoad = Mathf.Atan2(roadY2 - roadY1, roadX2 - roadX1) * 57.3f;
                    if (Math.Abs(angle - angleRoad) < 40) return false;
                }
            }
        }
        else {
            RoadObject roadClass = RoadsClass.objects[connectedRoad].GetComponent <RoadObject> ();
            float roadX1 = roadClass.x1;
            float roadY1 = roadClass.y1;
            float roadX2 = roadClass.x2;
            float roadY2 = roadClass.y2;
            if (Math.Sqrt(Math.Pow(x1 - roadX1, 2) + Math.Pow(y1 - roadY1, 2)) > Math.Sqrt(Math.Pow(x1 - roadX2, 2) + Math.Pow(y1 - roadY2, 2))) {
                float tmpX = roadX1, tmpY = roadY1;
                roadX1 = roadX2;
                roadY1 = roadY2;
                roadX2 = tmpX;
                roadY2 = tmpY;
            }
            float angleRoad = Mathf.Atan2(roadY2 - roadY1, roadX2 - roadX1) * 57.3f;
            if (Math.Abs(angle - angleRoad) < 40) return false;
        }
        return true;
    }
}
