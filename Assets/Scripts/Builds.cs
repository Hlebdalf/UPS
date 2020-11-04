using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Builds : MonoBehaviour {
    public GameObject[] preFubs;
    public GameObject[] preFubsGhost;
    public float roundCoordinatesConst;
    private List <GameObject> objects;
    private List <GameObject> ghostObjects;

    void Start() {
        objects = new List <GameObject> ();
        ghostObjects = new List <GameObject> ();
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

    private int ToIndex(string type) {
        int choose = -1;
        if (type == "House1" || type == "House1Ghost") choose = 0;
        if (type == "House2" || type == "House2Ghost") choose = 1;
        if (type == "Road" || type == "RoadGhost") choose = 2;
        return choose;
    }

    public void BuildObject(string type, Vector3 point) {
        point = RoundCoodinates(point);
        objects.Add(Instantiate(preFubs[ToIndex(type)], point, preFubs[ToIndex(type)].transform.rotation));
    }

    public void CreateGhost(string type, Vector3 point) {
        ghostObjects.Add(Instantiate(preFubsGhost[ToIndex(type)], point, preFubsGhost[ToIndex(type)].transform.rotation));
        ghostObjects[ghostObjects.Count - 1].AddComponent <MoveBuild> ();
    }
}
