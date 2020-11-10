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
    public int idx;
}

public class Roads : MonoBehaviour {
    private List <GameObject> objects;
    private List <GameObject> ghostObjects;
    private List <RoadObject> ghostObjectsData;
    private bool isFollowGhost = false;
    private string RoadType = "";
    
    public GameObject[] preFubs;
    public GameObject[] preFubsGhost;

    private void Start() {
        objects = new List <GameObject> ();
        ghostObjects = new List <GameObject> ();
        ghostObjectsData = new List <RoadObject> ();
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
        newGhostObjectData.idx = ToIndex(type);
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
            objects.Add(Instantiate(preFubs[ghostObjectsData[i].idx], ghostObject.transform.position, ghostObject.transform.rotation));
            objects[objects.Count - 1].transform.localScale = ghostObject.transform.localScale;
            MeshRenderer MeshRendererClass = ghostObject.GetComponent <MeshRenderer> ();
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
