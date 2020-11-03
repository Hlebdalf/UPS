using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Builds : MonoBehaviour {
    public GameObject[] preFubs;
    public int RoundCoordinatesConst;
    private List <GameObject> objects;

    void Start() {
        objects = new List <GameObject> ();
    }

    private Vector3 RoundCoodinates(Vector3 point) {
        float x = point.x, y = point.y, z = point.z;
        int low, high;

        low = (int)(x / RoundCoordinatesConst) * RoundCoordinatesConst;
        high = low + RoundCoordinatesConst;
        if (Math.Abs(x - low) < Math.Abs(x - high)) point.x = low;
        else point.x = high;

        low = (int)(y / RoundCoordinatesConst) * RoundCoordinatesConst;
        high = low + RoundCoordinatesConst;
        if (Math.Abs(y - low) < Math.Abs(y - high)) point.y = low;
        else point.y = high;

        low = (int)(z / RoundCoordinatesConst) * RoundCoordinatesConst;
        high = low + RoundCoordinatesConst;
        if (Math.Abs(z - low) < Math.Abs(z - high)) point.z = low;
        else point.z = high;

        return point;
    }

    public void BuildObject(string type, Vector3 point) {
        point = RoundCoodinates(point);
        switch (type) {
            case "House1":
                objects.Add(Instantiate(preFubs[0], point, preFubs[0].transform.rotation));
                break;
            case "House2":
                objects.Add(Instantiate(preFubs[1], point, preFubs[1].transform.rotation));
                break;
            case "Road":
                //objects.Add(Instantiate());
                break;
        }
    }
}
