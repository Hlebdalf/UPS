using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossroadObject : MonoBehaviour {
    private GameObject MainCamera;
    private Roads RoadsClass;

    public float x, y;
    public int idx, xr, yr;
    public List <int> connectedRoads;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        RoadsClass = MainCamera.GetComponent <Roads> ();
        connectedRoads = new List <int> ();
    }

    private void OnMouseOver() {
        RoadsClass.idxOverCrossroad = idx;
    }

    private void OnMouseExit() {
        RoadsClass.idxOverCrossroad = -1;
    }
}
