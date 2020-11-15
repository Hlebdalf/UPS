using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Pair<T, U> {
    public Pair() {
    }

    public Pair(T first, U second) {
        this.First = first;
        this.Second = second;
    }

    public T First { get; set; }
    public U Second { get; set; }
};

public struct RoadObject {
    public float x1, y1, x2, y2, len;
    public int idxPreFab;
}

public class Roads : MonoBehaviour {
    private List <GameObject> objects;
    private List <RoadObject> objectsData;
    private List <GameObject> ghostObjects;
    private List <RoadObject> ghostObjectsData;
    private bool isFollowGhost = false;
    private string RoadType = "";
    
    public GameObject[] preFubs;
    public GameObject[] preFubsGhost;

    private void Start() {
        objects = new List <GameObject> ();
        objectsData = new List <RoadObject> ();
        ghostObjects = new List <GameObject> ();
        ghostObjectsData = new List <RoadObject> ();
        CreateDefaultRoad("Road");
    }

    private void Update() {
        if (!isFollowGhost && RoadType != "") {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (Input.GetMouseButtonDown(0))
                    CreateGhost(RoadType, hit.point);
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
                    Quaternion.Euler(0, funcK(objectData.len, objectData.x2 - objectData.x1, objectData.x1, objectData.y1, objectData.x2, objectData.y2), 0)));
        objects[objects.Count - 1].transform.localScale = new Vector3(1, 1, objectData.len / 2);
        MeshRenderer MeshRendererClass = objects[objects.Count - 1].GetComponent <MeshRenderer> ();
        MeshRendererClass.materials[0].SetTextureScale("_MainTex", new Vector2(objectData.len / 2, 1));
    }

    private float funcK(float dist, float leg, float x1, float y1, float x2, float y2) {
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

    public void GetRoadType(string type) {
        RoadType = type;
    }

    public void SetRoadType(string type) {
        RoadType = type;
    }

    public bool GetIsFollowGhost() {
        return isFollowGhost;
    }

    public void SetIsFollowGhost(bool p) {
        isFollowGhost = p;
    }

    public Pair <float, float> GetFirstCoordinatesGhostObject(GameObject ghostObject) {
        RoadObject data = ghostObjectsData[ghostObjects.IndexOf(ghostObject)];
        return new Pair <float, float> (data.x1, data.y1);
    }

    public void SetFirstCoordinatesGhostObject(GameObject ghostObject, Vector3 point) {
        int idx = ghostObjects.IndexOf(ghostObject);
        float dist1 = (float)Math.Sqrt(Math.Pow(point.x - ghostObjectsData[idx].x1, 2) + Math.Pow(point.z - ghostObjectsData[idx].y1, 2));
        float dist2 = (float)Math.Sqrt(Math.Pow(point.x - ghostObjectsData[idx].x2, 2) + Math.Pow(point.z - ghostObjectsData[idx].y2, 2));
        if (dist1 < dist2) {
            RoadObject data = ghostObjectsData[idx];
            float tmp;
            tmp = data.x1;
            data.x1 = data.x2;
            data.x2 = tmp;
            tmp = data.y1;
            data.y1 = data.y2;
            data.y2 = tmp;
            ghostObjectsData[idx] = data;
        }
    }

    public void SetSecondCoordinatesGhostObject(GameObject ghostObject, Vector3 point) {
        int idx = ghostObjects.IndexOf(ghostObject);
        RoadObject data = ghostObjectsData[idx];
        data.x2 = point.x;
        data.y2 = point.z;
        ghostObjectsData[idx] = data;
    }

    public void SetLenGhostObject(GameObject ghostObject, float len) {
        int idx = ghostObjects.IndexOf(ghostObject);
        RoadObject data = ghostObjectsData[idx];
        data.len = len;
        ghostObjectsData[idx] = data;
    }

    public void CreateGhost(string type, Vector3 point) {
        ghostObjects.Add(Instantiate(preFubsGhost[ToIndex(type)], point, preFubsGhost[ToIndex(type)].transform.rotation));
        ghostObjects[ghostObjects.Count - 1].AddComponent <MoveGhostRoad> ();
        RoadObject newGhostObjectData = new RoadObject();
        newGhostObjectData.idxPreFab = ToIndex(type);
        newGhostObjectData.x1 = point.x;
        newGhostObjectData.y1 = point.z;
        ghostObjectsData.Add(newGhostObjectData);
        isFollowGhost = true;
    }

    public void DeleteGhost(GameObject ghostObject) {
        ghostObjectsData.RemoveAt(ghostObjects.IndexOf(ghostObject));
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
            //objects[objects.Count - 1].AddComponent <MoveRoad> ();
            DeleteGhost(ghostObject);
        }
    }

    public void DeleteObject(GameObject obj) {
        objects.Remove(obj);
        Destroy(obj);
    }
}
