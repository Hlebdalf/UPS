using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Builds : MonoBehaviour {
    private GameObject MainCamera;
    private Economy EconomyClass;
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
    public Texture[] posterTextures;
    public List <GameObject> objects;
    public List <GameObject> objectsWithAvailableSeats;
    public List <GameObject> objectsWithoutAvailableSeats;
    public List <GameObject> commerces;
    public List <GameObject> commercesWithAvailableSeats;
    public List <GameObject> commercesWithoutAvailableSeats;
    public List <GameObject> parkings;
    public List <GameObject> ghostObjects;
    public List <GameObject> deleteObjects;
    public GameObject InterfaceObject;
    public Interface InterfaceClass;
    public GameObject PreFubHouseMenu;
    public bool isPressEnter = false;
    public bool isFollowGhost = false;
    public bool isDeleting = false;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        EconomyClass = MainCamera.GetComponent <Economy> ();
        RoadsClass = MainCamera.GetComponent <Roads> ();
        FieldClass = MainCamera.GetComponent <Field> ();
        InterfaceClass = InterfaceObject.GetComponent <Interface> ();
        GenerationGraphClass = MainCamera.GetComponent <GenerationGraph> ();
        objects = new List <GameObject> ();
        objectsWithAvailableSeats = new List <GameObject> ();
        objectsWithoutAvailableSeats = new List <GameObject> ();
        commerces = new List <GameObject> ();
        commercesWithAvailableSeats = new List <GameObject> ();
        commercesWithoutAvailableSeats = new List <GameObject> ();
        parkings = new List <GameObject> ();
        ghostObjects = new List <GameObject> ();
        deleteObjects = new List <GameObject> ();
    }

    private void Update() {
        if ((Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter)) && !isPressEnter && !isFollowGhost) {
            isPressEnter = true;
        }
        if (isPressEnter && !RoadsClass.isPressEnter) {
            if (isDeleting) {
                if (deleteObjects.Count > 0) DeleteObjects();
                else {
                    StartCoroutine(DelayReGen(false));
                    isPressEnter = false;
                }
            }
            else {
                if (ghostObjects.Count > 0) CreateObjects();
                else {
                    StartCoroutine(DelayReGen());
                    isPressEnter = false;
                }
            }
        }
    }

    IEnumerator DelayReGen(bool building = true) {
        if (building) yield return new WaitForSeconds(FieldClass.timeBuildProcess + 1);
        else yield return new WaitForSeconds(1);
        GenerationGraphClass.StartGeneration();
        yield return null;
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
        data.cost = preFubs[ToIndex(name)].GetComponent <BuildObject> ().cost;

        EconomyClass.AddOptMoney(-data.cost);
    }

    public void DeleteGhost(GameObject ghostObject, bool addMoney = true) {
        if (addMoney) EconomyClass.AddOptMoney(ghostObject.GetComponent <BuildGhostObject> ().cost);
        ghostObjects.Remove(ghostObject);
        Destroy(ghostObject);
    }

    public void CreateObjects() {
        EconomyClass.AddMoney();
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

            DeleteGhost(ghostObject, false);
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
            commercesWithAvailableSeats.Add(commerces[commerces.Count - 1]);
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
            if (isGeneration) data.isGenBuild = true;
            EconomyClass.AddBuild(FieldClass.districts[(int)data.x + FieldClass.fieldSizeHalf, (int)data.y + FieldClass.fieldSizeHalf], idxPreFub);

            commerces[commerces.Count - 1].AddComponent <Rigidbody> ();
            commerces[commerces.Count - 1].GetComponent <Rigidbody> ().useGravity = false;
        }
        else if (idxPreFub == idxParking) {
            parkings.Add(Instantiate(preFubs[idxPreFub], point, Quaternion.Euler(0, rotate, 0)));
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
            if (isGeneration) data.isGenBuild = true;

            List <GameObject> carObjects = new List <GameObject> ();
            Transform parent = parkings[parkings.Count - 1].transform.Find("Build");
            foreach (Transform child in parent) {
                if (child.gameObject.name == "Car") {
                    carObjects.Add(child.gameObject);
                }
            }
            int cntCars = (int)UnityEngine.Random.Range(25f, carObjects.Count - 25f);
            for (int i = 0; i < cntCars; ++i) {
                carObjects[(int)UnityEngine.Random.Range(0f, carObjects.Count - 0.01f)].SetActive(true);
            }

            parkings[parkings.Count - 1].AddComponent <Rigidbody> ();
            parkings[parkings.Count - 1].GetComponent <Rigidbody> ().useGravity = false;
        }
        else {
            objects.Add(Instantiate(preFubs[idxPreFub], point, Quaternion.Euler(0, rotate, 0)));
            objectsWithAvailableSeats.Add(objects[objects.Count - 1]);
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
            if (isGeneration) data.isGenBuild = true;
            EconomyClass.AddBuild(FieldClass.districts[(int)data.x + FieldClass.fieldSizeHalf, (int)data.y + FieldClass.fieldSizeHalf], idxPreFub);

            objects[objects.Count - 1].AddComponent <Rigidbody> ();
            objects[objects.Count - 1].GetComponent <Rigidbody> ().useGravity = false;
        }
    }

    public void DeleteObjects() {
        for (int i = 0; i < deleteObjects.Count; ++i) {
            GameObject obj = deleteObjects[i];
            BuildObject objClass = obj.GetComponent <BuildObject> ();
            if (objClass.idxPreFub == -1 && objClass.idxCommerceType == -1) parkings.Remove(obj);
            else if (objClass.idxPreFub == -1) {
                commerces.Remove(obj);
                commercesWithAvailableSeats.Remove(obj);
                commercesWithoutAvailableSeats.Remove(obj);
            }
            else {
                objects.Remove(obj);
                objectsWithAvailableSeats.Remove(obj);
                objectsWithoutAvailableSeats.Remove(obj);
            }
            deleteObjects.RemoveAt(i);
            Destroy(obj);
        }
    }
}
