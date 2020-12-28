using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BuildGhostObject : MonoBehaviour {
    private GameObject MainCamera;
    private Builds BuildsClass;
    private Roads RoadsClass;
    private Field FieldClass;
    private float eps = 1e-5f;
    private bool isFollow = true, isBusy = false, isCollision = false;

    public float x, y;
    public int idx, idxPreFub, connectedRoad = -1;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        BuildsClass = MainCamera.GetComponent <Builds> ();
        RoadsClass = MainCamera.GetComponent <Roads> ();
        FieldClass = MainCamera.GetComponent <Field> ();
    }

    private void Start() {
        gameObject.layer = 2;
    }

    private void Update() {
        if (isFollow && !isBusy) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                (Vector3 pos, float rotate) rounded = RoundCoordinateOnRoad(hit.point);
                transform.position = rounded.pos;
                transform.rotation = Quaternion.Euler(0, rounded.rotate, 0);
            }
            if (Input.GetMouseButtonDown(0) && !isCollision) {
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

    private (Vector3 pos, float rotate) RoundCoordinateOnRoad(Vector3 point) {
        float width = (int)gameObject.GetComponent <BoxCollider> ().size.x + 2;
        float len = (int)gameObject.GetComponent <BoxCollider> ().size.z + 2;
        int ansId = -1, ansIdGhost = -1;
        float ans = (int)1e9, ansGhost = (int)1e9;
        float rotate = 0;

        List <float> min = new List <float> ();
        List <int> minId = new List <int> ();
        for (int i = 0; i < RoadsClass.objects.Count; ++i) {
            RoadObject dataRoad = RoadsClass.objects[i].GetComponent <RoadObject> ();
            float mainRoadA = dataRoad.y1 - dataRoad.y2, mainRoadB = dataRoad.x2 - dataRoad.x1, mainRoadC = dataRoad.x1 * dataRoad.y2 - dataRoad.x2 * dataRoad.y1; // main road line
            float normA = -mainRoadB, normB = mainRoadA, normC = -(normA * point.x + normB * point.z); // norm
            float normCrossMainRoadX = -(mainRoadC * normB - normC * mainRoadB) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate
            float normCrossMainRoadY = -(mainRoadA * normC - normA * mainRoadC) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate
            float dist1 = (float)Math.Sqrt(Math.Pow(dataRoad.x1 - normCrossMainRoadX, 2) + Math.Pow(dataRoad.y1 - normCrossMainRoadY, 2));
            float dist2 = (float)Math.Sqrt(Math.Pow(dataRoad.x2 - normCrossMainRoadX, 2) + Math.Pow(dataRoad.y2 - normCrossMainRoadY, 2));
            float dist3 = (float)Math.Sqrt(Math.Pow(dataRoad.x2 - dataRoad.x1, 2) + Math.Pow(dataRoad.y2 - dataRoad.y1, 2));
            float dist = (float)Math.Sqrt(Math.Pow(normCrossMainRoadX - point.x, 2) + Math.Pow(normCrossMainRoadY - point.z, 2));
            if (dist < len && dist1 + dist2 - dist3 <= eps) {
                min.Add(dist);
                minId.Add(i);
            }
        }

        List <float> minGhost = new List <float> ();
        List <int> minIdGhost = new List <int> ();
        for (int i = 0; i < RoadsClass.ghostObjects.Count; ++i) {
            RoadGhostObject dataRoad = RoadsClass.ghostObjects[i].GetComponent <RoadGhostObject> ();
            float mainRoadA = dataRoad.y1 - dataRoad.y2, mainRoadB = dataRoad.x2 - dataRoad.x1, mainRoadC = dataRoad.x1 * dataRoad.y2 - dataRoad.x2 * dataRoad.y1; // main road line
            float normA = -mainRoadB, normB = mainRoadA, normC = -(normA * point.x + normB * point.z); // norm
            float normCrossMainRoadX = -(mainRoadC * normB - normC * mainRoadB) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate
            float normCrossMainRoadY = -(mainRoadA * normC - normA * mainRoadC) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate
            float dist1 = (float)Math.Sqrt(Math.Pow(dataRoad.x1 - normCrossMainRoadX, 2) + Math.Pow(dataRoad.y1 - normCrossMainRoadY, 2));
            float dist2 = (float)Math.Sqrt(Math.Pow(dataRoad.x2 - normCrossMainRoadX, 2) + Math.Pow(dataRoad.y2 - normCrossMainRoadY, 2));
            float dist3 = (float)Math.Sqrt(Math.Pow(dataRoad.x2 - dataRoad.x1, 2) + Math.Pow(dataRoad.y2 - dataRoad.y1, 2));
            float dist = (float)Math.Sqrt(Math.Pow(normCrossMainRoadX - point.x, 2) + Math.Pow(normCrossMainRoadY - point.z, 2));
            if (dist < len && dist1 + dist2 - dist3 <= eps) {
                minGhost.Add(dist);
                minIdGhost.Add(i);
            }
        }

        for (int i = 0; i < min.Count; ++i) {
            if (min[i] < ans) {
                ans = min[i];
                ansId = minId[i];
            }
        }

        for (int i = 0; i < minGhost.Count; ++i) {
            if (minGhost[i] < ans) {
                ansGhost = minGhost[i];
                ansIdGhost = minIdGhost[i];
            }
        }

        if (ansId == -1 && ansIdGhost == -1) return (point, 0);
        else if (ans < ansGhost) {
            RoadObject dataRoad = RoadsClass.objects[ansId].GetComponent <RoadObject> ();
            float mainRoadA = dataRoad.y1 - dataRoad.y2, mainRoadB = dataRoad.x2 - dataRoad.x1, mainRoadC = dataRoad.x1 * dataRoad.y2 - dataRoad.x2 * dataRoad.y1; // main road line
            float normA = -mainRoadB, normB = mainRoadA, normC = -(normA * point.x + normB * point.z); // norm
            float normCrossMainRoadX = -(mainRoadC * normB - normC * mainRoadB) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate
            float normCrossMainRoadY = -(mainRoadA * normC - normA * mainRoadC) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate
            float x = (float)Math.Cos(Math.Atan(normA / -normB)) * (len / 2 + 1);
            float y = (float)Math.Sin(Math.Atan(normA / -normB)) * (len / 2 + 1);
            float dist = (mainRoadA * point.x + mainRoadB * point.z + mainRoadC) / (float)Math.Sqrt(Math.Pow(mainRoadA, 2) + Math.Pow(mainRoadB, 2));
            if (dataRoad.y1 >= dataRoad.y2) dist *= -1;
            if (dist < 0) {
                point.x = normCrossMainRoadX + x;
                point.z = normCrossMainRoadY + y;
                rotate = RoundRotateOnRoad(dataRoad.x1, dataRoad.y1, dataRoad.x2, dataRoad.y2);
            }
            else {
                point.x = normCrossMainRoadX - x;
                point.z = normCrossMainRoadY - y;
                rotate = RoundRotateOnRoad(dataRoad.x1, dataRoad.y1, dataRoad.x2, dataRoad.y2) * -1;
            }
        }
        else {
            RoadGhostObject dataRoad = RoadsClass.ghostObjects[ansIdGhost].GetComponent <RoadGhostObject> ();
            float mainRoadA = dataRoad.y1 - dataRoad.y2, mainRoadB = dataRoad.x2 - dataRoad.x1, mainRoadC = dataRoad.x1 * dataRoad.y2 - dataRoad.x2 * dataRoad.y1; // main road line
            float normA = -mainRoadB, normB = mainRoadA, normC = -(normA * point.x + normB * point.z); // norm
            float normCrossMainRoadX = -(mainRoadC * normB - normC * mainRoadB) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate
            float normCrossMainRoadY = -(mainRoadA * normC - normA * mainRoadC) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate
            float x = (float)Math.Cos(Math.Atan(normA / -normB)) * (len / 2 + 1);
            float y = (float)Math.Sin(Math.Atan(normA / -normB)) * (len / 2 + 1);
            float dist = (mainRoadA * point.x + mainRoadB * point.z + mainRoadC) / (float)Math.Sqrt(Math.Pow(mainRoadA, 2) + Math.Pow(mainRoadB, 2));
            if (dataRoad.y1 >= dataRoad.y2) dist *= -1;
            if (dist < 0) {
                point.x = normCrossMainRoadX + x;
                point.z = normCrossMainRoadY + y;
                rotate = RoundRotateOnRoad(dataRoad.x1, dataRoad.y1, dataRoad.x2, dataRoad.y2);
            }
            else {
                point.x = normCrossMainRoadX - x;
                point.z = normCrossMainRoadY - y;
                rotate = RoundRotateOnRoad(dataRoad.x1, dataRoad.y1, dataRoad.x2, dataRoad.y2) * -1;
            }
        }

        point = RoundCoordinate(point);

        return (point, rotate);
    }

    private Vector3 RoundCoordinate(Vector3 point) {
        float x = point.x, z = point.z, low, high;

        low = (int)(x / FieldClass.gridSize) * FieldClass.gridSize;
        high = low + FieldClass.gridSize;
        if (Math.Abs(x - low) < Math.Abs(x - high)) point.x = low;
        else point.x = high;

        point.y = 0;
        
        low = (int)(z / FieldClass.gridSize) * FieldClass.gridSize;
        high = low + FieldClass.gridSize;
        if (Math.Abs(z - low) < Math.Abs(z - high)) point.z = low;
        else point.z = high;

        return point;
    }

    private float RoundRotateOnRoad(float x1, float y1, float x2, float y2) {
        float dist = (float)Math.Sqrt(Math.Pow(x1 - x2, 2) + Math.Pow(y1 - y2, 2));
        float angle = (float)Math.Acos((float)Math.Abs(x1 - x2) / dist) * (float)57.3;
        if ((x1 <= x2 && y1 <= y2) || (x1 >= x2 && y1 >= y2)) angle *= -1;
        return angle;
    }
}
