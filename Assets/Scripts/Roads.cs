using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public struct RoadObject {
    public float x1, y1, x2, y2, len;
    public int idxPreFab;
}

public class Roads : MonoBehaviour {
    private float eps = 1e-5f;
    private List <GameObject> objects;
    private List <RoadObject> objectsData;
    private List <GameObject> ghostObjects;
    private List <RoadObject> ghostObjectsData;
    private List <int> ghostObjectsConnect;
    private bool isFollowGhost = false;
    private int idxOverRoad = -1;
    private string RoadType = "";
    
    public GameObject[] preFubs;
    public GameObject[] preFubsGhost;

    private void Start() {
        objects = new List <GameObject> ();
        objectsData = new List <RoadObject> ();
        ghostObjects = new List <GameObject> ();
        ghostObjectsData = new List <RoadObject> ();
        ghostObjectsConnect = new List <int> ();
        CreateDefaultRoad("Road");
    }

    private void Update() {
        if (!isFollowGhost && RoadType != "" && idxOverRoad != -1) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (Input.GetMouseButtonDown(0)) {
                    Vector2 point = RoundCoordinateOnTheRoad(new Vector2(hit.point.x, hit.point.z), idxOverRoad);
                    CreateGhost(RoadType, new Vector3(point.x, 0, point.y), idxOverRoad);
                }
            }
        }
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter)) {
            CreateObjects();
        }
    }

    private void CreateDefaultRoad(string type) {
        RoadObject objectData;
        objectData.x1 = 0;
        objectData.y1 = 100;
        objectData.x2 = 0;
        objectData.y2 = 110;
        objectData.len = (float)Math.Sqrt(Math.Pow(objectData.x2 - objectData.x1, 2) + Math.Pow(objectData.y2 - objectData.y1, 2));
        objectData.idxPreFab = ToIndex(type);
        objectsData.Add(objectData);
        objects.Add(Instantiate(preFubs[objectData.idxPreFab], new Vector3((objectData.x1 + objectData.x2) / 2, 0, (objectData.y1 + objectData.y2) / 2),
                    Quaternion.Euler(0, funcAngle(objectData.len, objectData.x2 - objectData.x1, objectData.x1, objectData.y1, objectData.x2, objectData.y2), 0)));
        objects[objects.Count - 1].transform.localScale = new Vector3(1, 1, objectData.len / 2);
        MeshRenderer MeshRendererClass = objects[objects.Count - 1].GetComponent <MeshRenderer> ();
        MeshRendererClass.materials[0].SetTextureScale("_MainTex", new Vector2(objectData.len / 2, 1));
        objects[objects.Count - 1].AddComponent <MoveRoad> ();
    }

    private float funcAngle(float dist, float leg, float x1, float y1, float x2, float y2) {
        if (dist == 0) return 0;
        else {
            if (y1 <= y2) return (float)(90 - Math.Acos(leg / dist) * 57.3);
            else return (float)(-270 + Math.Acos(leg / dist) * 57.3);
        }
    }

    private int ToIndex(string type) {
        int choose = -1;
        if (type == "Road" || type == "RoadGhost") choose = 0;
        return choose;
    }

    public int GetIndex(GameObject RoadObject) {
        return objects.IndexOf(RoadObject);
    }

    public int GetGhostIndex(GameObject GhostRoadObject) {
        return ghostObjects.IndexOf(GhostRoadObject);
    }

    public string GetRoadType() {
        return RoadType;
    }

    public void SetRoadType(string type) {
        RoadType = type;
    }

    public int GetIsOverRoad() {
        return idxOverRoad;
    }

    public void SetIsOverRoad(int idx) {
        idxOverRoad = idx;
    }

    public bool GetIsFollowGhost() {
        return isFollowGhost;
    }

    public void SetIsFollowGhost(bool p) {
        isFollowGhost = p;
    }

    public RoadObject GetGhostRoadObject(int idx) {
        return ghostObjectsData[idx];
    }

    public void SetGhostRoadObject(RoadObject data, int idx) {
        ghostObjectsData[idx] = data;
    }

    public int GetIdxGhostObjectConnect(int idx) {
        return ghostObjectsConnect[idx];
    }

    public Vector2 RoundCoordinateOnTheRoad(Vector2 point, int idxRoad) {
        RoadObject data = objectsData[idxRoad];
        float a1 = data.y1 - data.y2, b1 = data.x2 - data.x1, c1 = data.x1 * data.y2 - data.x2 * data.y1; // line
        float a2 = -b1, b2 = a1, c2 = -(a2 * point.x + b2 * point.y); // norm
        if (a1 * b2 - a2 * b1 == 0) return point; // parallel
        float x = -(c1 * b2 - c2 * b1) / (a1 * b2 - a2 * b1);
        float y = -(a1 * c2 - a2 * c1) / (a1 * b2 - a2 * b1);
        Vector2 ans = new Vector2(x, y);
        float dist1 = (float)Math.Sqrt(Math.Pow(x - data.x1, 2) + Math.Pow(y - data.y1, 2));
        float dist2 = (float)Math.Sqrt(Math.Pow(x - data.x2, 2) + Math.Pow(y - data.y2, 2));
        float dist = (float)Math.Sqrt(Math.Pow(data.x2 - data.x1, 2) + Math.Pow(data.y2 - data.y1, 2));
        if (dist1 + dist2 - dist > eps) {
            if (dist1 < dist2) ans = new Vector2(data.x1, data.y1);
            else ans = new Vector2(data.x2, data.y2);
        }
        return ans;
    }

    public Vector2 RoundMovingCoordinateOnTheRoad(RoadObject dataGhost, int idxRoad) {
        RoadObject data = objectsData[idxRoad];
        float a1 = data.y1 - data.y2, b1 = data.x2 - data.x1, c1 = data.x1 * data.y2 - data.x2 * data.y1;
        float a2 = dataGhost.y1 - dataGhost.y2, b2 = dataGhost.x2 - dataGhost.x1, c2 = dataGhost.x1 * dataGhost.y2 - dataGhost.x2 * dataGhost.y1;
        float x = -(c1 * b2 - c2 * b1) / (a1 * b2 - a2 * b1);
        float y = -(a1 * c2 - a2 * c1) / (a1 * b2 - a2 * b1);
        Vector2 ans = new Vector2(x, y);
        float dist1 = (float)Math.Sqrt(Math.Pow(x - data.x1, 2) + Math.Pow(y - data.y1, 2));
        float dist2 = (float)Math.Sqrt(Math.Pow(x - data.x2, 2) + Math.Pow(y - data.y2, 2));
        float dist = (float)Math.Sqrt(Math.Pow(data.x2 - data.x1, 2) + Math.Pow(data.y2 - data.y1, 2));
        if (dist1 + dist2 - dist > eps) {
            if (dist1 < dist2) ans = new Vector2(data.x1, data.y1);
            else ans = new Vector2(data.x2, data.y2);
        }
        return ans;
    }

    public void CreateGhost(string type, Vector3 point, int idxRoad) {
        ghostObjects.Add(Instantiate(preFubsGhost[ToIndex(type)], point, preFubsGhost[ToIndex(type)].transform.rotation));
        ghostObjects[ghostObjects.Count - 1].AddComponent <MoveGhostRoad> ();
        RoadObject data = new RoadObject();
        data.idxPreFab = ToIndex(type);
        data.x1 = point.x;
        data.y1 = point.z;
        ghostObjectsData.Add(data);
        ghostObjectsConnect.Add(idxRoad);
        isFollowGhost = true;
    }

    public void DeleteGhost(GameObject ghostObject) {
        ghostObjectsData.RemoveAt(ghostObjects.IndexOf(ghostObject));
        ghostObjectsConnect.RemoveAt(ghostObjects.IndexOf(ghostObject));
        ghostObjects.Remove(ghostObject);
        Destroy(ghostObject);
    }

    public void CreateObjects() {
        for (int i = 0; i < ghostObjects.Count; ++i) {
            GameObject ghostObject = ghostObjects[i];
            objects.Add(Instantiate(preFubs[ghostObjectsData[i].idxPreFab], ghostObject.transform.position, ghostObject.transform.rotation));
            objectsData.Add(ghostObjectsData[i]);
            objects[objects.Count - 1].transform.localScale = ghostObject.transform.localScale;
            MeshRenderer MeshRendererClass = objects[objects.Count - 1].GetComponent <MeshRenderer> ();
            MeshRendererClass.materials[0].SetTextureScale("_MainTex", new Vector2(ghostObjectsData[i].len / 2, 1));
            objects[objects.Count - 1].AddComponent <MoveRoad> ();
            DeleteGhost(ghostObject);
        }
    }

    public void DeleteObject(GameObject obj) {
        objectsData.RemoveAt(objects.IndexOf(obj));
        objects.Remove(obj);
        Destroy(obj);
    }
}
