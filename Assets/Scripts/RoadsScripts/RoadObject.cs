using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadObject : MonoBehaviour {
    private GameObject MainCamera;
    private Roads RoadsClass;

    public float x1, y1, x2, y2, len;
    public int idx;
    public List <int> connectedRoads;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        RoadsClass = MainCamera.GetComponent <Roads> ();
        connectedRoads = new List <int> ();
    }

    private void OnMouseOver() {
        RoadsClass.idxOverRoad = idx;
    }

    private void OnMouseExit() {
        RoadsClass.idxOverRoad = -1;
    }
}
