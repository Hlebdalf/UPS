using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Builds : MonoBehaviour {
    private GameObject MainCamera;
    private Roads RoadsClass;
    private Field FieldClass;
    private GenerationGraph GenerationGraphClass;

    public GameObject[] preFubs;
    public GameObject[] preFubsGhost;
    public GameObject[] preFubsBuildProcess;
    public int[] idxsDistrict1;
    public int[] idxsDistrict2;
    public int[] idxsDistrict3;
    public int[] idxsDistrict4;
    public int[] idxsCommerces;
    public int idxParking;
    public List <GameObject> objects;
    public List <GameObject> commerces;
    public List <GameObject> parkings;
    public List <GameObject> ghostObjects;
    public GameObject InterfaceObject;
    public Interface InterfaceClass;
    public GameObject PreFubHouseMenu;
    public bool isPressEnter = false;
    public bool isFollowGhost = false;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        RoadsClass = MainCamera.GetComponent <Roads> ();
        FieldClass = MainCamera.GetComponent <Field> ();
        InterfaceClass = InterfaceObject.GetComponent <Interface> ();
        GenerationGraphClass = MainCamera.GetComponent <GenerationGraph> ();
        objects = new List <GameObject> ();
        commerces = new List <GameObject> ();
        parkings = new List <GameObject> ();
        ghostObjects = new List <GameObject> ();
    }

    private void Update() {
        if ((Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter)) && !isPressEnter && !isFollowGhost) {
            isPressEnter = true;
        }
        if (isPressEnter && !RoadsClass.isPressEnter) {
            if (ghostObjects.Count > 0) {
                CreateObjects();
            }
            else {
                StartCoroutine(DelayReGen());
                isPressEnter = false;
            }
        }
    }

    IEnumerator DelayReGen() {
        yield return new WaitForSeconds(FieldClass.timeBuildProcess + 1);
        GenerationGraphClass.StartGeneration();
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
        InterfaceClass.DeactivateAllMenu();
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
            if (ghostObjectClass.isFollow) continue;
            GameObject newObject = Instantiate(preFubsBuildProcess[ghostObjectClass.idxPreFub], ghostObject.transform.position, ghostObject.transform.rotation);
            newObject.AddComponent <BuildProcessObject> ();
            BuildProcessObject newObjectClass = newObject.GetComponent <BuildProcessObject> ();

            newObjectClass.x = ghostObjectClass.x;
            newObjectClass.y = ghostObjectClass.y;
            newObjectClass.rotate = ghostObjectClass.rotate;
            newObjectClass.idxPreFub = ghostObjectClass.idxPreFub;
            newObjectClass.connectedRoad = ghostObjectClass.connectedRoad;

            newObject.AddComponent <Rigidbody> ();
            newObject.GetComponent <Rigidbody> ().useGravity = false;

            DeleteGhost(ghostObject);
        }
    }

    public void CreateObject(Vector3 point, float rotate, int idxPreFub, int connectedRoad, bool isGeneration = true) {
        bool p = false;
        int itCommerce = -1;
        for (int i = 0; i < idxsCommerces.Length; ++i) {
            if (idxsCommerces[i] == idxPreFub) {
                itCommerce = i;
                p = true;
            }
        }
        if (p) {
            commerces.Add(Instantiate(preFubs[idxPreFub], point, Quaternion.Euler(0, rotate, 0)));
            commerces[commerces.Count - 1].AddComponent <BuildObject> ();
            BuildObject data = commerces[commerces.Count - 1].GetComponent <BuildObject> ();

            data.x = point.x;
            data.y = point.z;
            data.idx = commerces.Count - 1;
            data.idxCommerceType = itCommerce;
            if (connectedRoad == -1) {
                data.connectedRoad = RoadsClass.objects.Count - 1;
            }
            else {
                data.connectedRoad = connectedRoad;
            }

            commerces[commerces.Count - 1].AddComponent <Rigidbody> ();
            commerces[commerces.Count - 1].GetComponent <Rigidbody> ().useGravity = false;
        }
        else if (idxPreFub == idxParking) {
            parkings.Add(Instantiate(preFubs[idxPreFub], point, Quaternion.Euler(0, rotate, 0)));
            parkings[parkings.Count - 1].AddComponent <BuildObject> ();
            BuildObject data = parkings[parkings.Count - 1].GetComponent <BuildObject> ();

            data.x = point.x;
            data.y = point.z;
            data.idx = parkings.Count - 1;
            if (connectedRoad == -1) {
                data.connectedRoad = RoadsClass.objects.Count - 1;
            }
            else {
                data.connectedRoad = connectedRoad;
            }

            parkings[parkings.Count - 1].AddComponent <Rigidbody> ();
            parkings[parkings.Count - 1].GetComponent <Rigidbody> ().useGravity = false;
        }
        else {
            objects.Add(Instantiate(preFubs[idxPreFub], point, Quaternion.Euler(0, rotate, 0)));
            objects[objects.Count - 1].AddComponent <BuildObject> ();
            BuildObject data = objects[objects.Count - 1].GetComponent <BuildObject> ();

            data.x = point.x;
            data.y = point.z;
            data.idx = objects.Count - 1;
            data.idxPreFub = idxPreFub;
            if (connectedRoad == -1) {
                data.connectedRoad = RoadsClass.objects.Count - 1;
            }
            else {
                data.connectedRoad = connectedRoad;
            }

            objects[objects.Count - 1].AddComponent <Rigidbody> ();
            objects[objects.Count - 1].GetComponent <Rigidbody> ().useGravity = false;
        }
    }
}
