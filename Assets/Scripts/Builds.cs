using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Builds : MonoBehaviour {
    private List <GameObject> objects;
    private List <GameObject> ghostObjects;
    private List <int> ghostObjectsIdx;
    private bool isFollowGhost = false;
    
    public GameObject[] preFubs;
    public GameObject[] preFubsGhost;
    public float roundCoordinatesConst;

    private void Start() {
        objects = new List <GameObject> ();
        ghostObjects = new List <GameObject> ();
        ghostObjectsIdx = new List <int> ();
    }

    private int ToIndex(string type) {
        int choose = -1;
        if (type == "House1" || type == "House1Ghost") choose = 0;
        if (type == "House2" || type == "House2Ghost") choose = 1;
        if (type == "Road" || type == "RoadGhost") choose = 2;
        return choose;
    }

    public Vector3 RoundCoodinates(Vector3 point) {
        float x = point.x, z = point.z, low, high;

        low = (int)(x / roundCoordinatesConst) * roundCoordinatesConst;
        high = low + roundCoordinatesConst;
        if (Math.Abs(x - low) < Math.Abs(x - high)) point.x = low;
        else point.x = high;
        point.x -= 0.5f;

        point.y = 0;

        low = (int)(z / roundCoordinatesConst) * roundCoordinatesConst;
        high = low + roundCoordinatesConst;
        if (Math.Abs(z - low) < Math.Abs(z - high)) point.z = low;
        else point.z = high;
        point.z -= 0.5f;

        return point;
    }

    public bool GetIsFollowGhost() {
        return isFollowGhost;
    }

    public void SetIsFollowGhost(bool p) {
        isFollowGhost = p;
    }

    public int GetCountGhostObjects() {
        return ghostObjects.Count;
    }

    public int GetCountObjects() {
        return objects.Count;
    }

    public void CreateGhost(string type, Vector3 point) {
        ghostObjects.Add(Instantiate(preFubsGhost[ToIndex(type)], point, preFubsGhost[ToIndex(type)].transform.rotation));
        ghostObjectsIdx.Add(ToIndex(type));
        ghostObjects[ghostObjects.Count - 1].AddComponent <MoveGhostBuild> ();
        isFollowGhost = true;
    }

    public void DeleteGhost(GameObject ghostObject) {
        Destroy(ghostObject);
        ghostObjectsIdx.RemoveAt(ghostObjects.IndexOf(ghostObject));
        ghostObjects.Remove(ghostObject);
    }

    public void CreateObjects() {
        for (int i = 0; i < ghostObjects.Count; ++i) {
            GameObject ghostObject = ghostObjects[i];
            objects.Add(Instantiate(preFubs[ghostObjectsIdx[i]], ghostObject.transform.position, ghostObject.transform.rotation));
            objects[objects.Count - 1].AddComponent <MoveBuild> ();
            DeleteGhost(ghostObject);
        }
    }

    public void DeleteObject(GameObject obj) {
        Destroy(obj);
        objects.Remove(obj);
    }
}
