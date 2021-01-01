using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Builds : MonoBehaviour {
    public GameObject[] preFubs;
    public GameObject[] preFubsGhost;
    public int[] idxsDistrict1;
    public int[] idxsDistrict2;
    public int[] idxsDistrict3;
    public int[] idxsDistrict4;
    public List <GameObject> objects;
    public List <GameObject> ghostObjects;
    public bool isFollowGhost = false;

    private void Start() {
        objects = new List <GameObject> ();
        ghostObjects = new List <GameObject> ();
    }

    private void Update() {
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter)) {
            CreateObjects();
        }
    }

    private int ToIndex(string name) {
        for (int i = 0; i < preFubs.Length; ++i) {
            if (preFubs[i].name == name || preFubsGhost[i].name == name) {
                return i;
            }
        }
        return -1;
    }

    public void CreateGhost(string name, Vector3 point) {
        ghostObjects.Add(Instantiate(preFubsGhost[ToIndex(name)], point, preFubsGhost[ToIndex(name)].transform.rotation));
        ghostObjects[ghostObjects.Count - 1].AddComponent <BuildGhostObject> ();
        isFollowGhost = true;

        BuildGhostObject data = ghostObjects[ghostObjects.Count - 1].GetComponent <BuildGhostObject> ();
        data.x = point.x;
        data.y = point.z;
        data.idx = ghostObjects.Count - 1;
        data.idxPreFub = ToIndex(name);
    }

    public void DeleteGhost(GameObject ghostObject) {
        ghostObjects.Remove(ghostObject);
        Destroy(ghostObject);
    }

    public void CreateObjects() {
        for (int i = 0; i < ghostObjects.Count; ++i) {
            GameObject ghostObject = ghostObjects[i];
            BuildGhostObject ghostObjectClass = ghostObject.GetComponent <BuildGhostObject> ();
            objects.Add(Instantiate(preFubs[ghostObjectClass.idxPreFub], ghostObject.transform.position, ghostObject.transform.rotation));
            objects[objects.Count - 1].AddComponent <BuildObject> ();

            BuildObject data = objects[objects.Count - 1].GetComponent <BuildObject> ();
            data.x = ghostObjectClass.x;
            data.y = ghostObjectClass.y;
            data.idx = objects.Count - 1;
            data.connectedRoad = ghostObjectClass.connectedRoad;

            objects[objects.Count - 1].AddComponent <Rigidbody> ();
            objects[objects.Count - 1].GetComponent <Rigidbody> ().useGravity = false;

            DeleteGhost(ghostObject);
        }
    }

    public void CreateObject(Vector3 point, float rotate, int idxPreFub, int connectedRoad) {
        objects.Add(Instantiate(preFubs[idxPreFub], point, Quaternion.Euler(0, rotate, 0)));
        objects[objects.Count - 1].AddComponent <BuildObject> ();
        BuildObject data = objects[objects.Count - 1].GetComponent <BuildObject> ();

        data.x = point.x;
        data.y = point.z;
        data.idx = objects.Count - 1;
        data.connectedRoad = connectedRoad;

        objects[objects.Count - 1].AddComponent <Rigidbody> ();
        objects[objects.Count - 1].GetComponent <Rigidbody> ().useGravity = false;
    }
}
