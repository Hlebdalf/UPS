using System.Collections;
using System.Collections.Generic;
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
    public float x1, y1, x2, y2, k, dist, leg;
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
}
