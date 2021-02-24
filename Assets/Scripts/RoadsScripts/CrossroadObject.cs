using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossroadObject : MonoBehaviour {
    private GameObject MainCamera;
    private Roads RoadsClass;

    public float x, y;
    public int idx, xr, yr, idxRoadGO = -1;
    public List <int> connectedRoads;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        RoadsClass = MainCamera.GetComponent <Roads> ();
        connectedRoads = new List <int> ();
    }

    private void Start() {
        StartCoroutine(AsyncTrafficLightsSwitch());
    }

    private void OnMouseOver() {
        RoadsClass.idxOverCrossroad = idx;
    }

    private void OnMouseExit() {
        RoadsClass.idxOverCrossroad = -1;
    }

    IEnumerator AsyncTrafficLightsSwitch() {
        for (int i = 0; true; i = (i + 1) % connectedRoads.Count) {
            if (connectedRoads.Count > 2) idxRoadGO = connectedRoads[i];
            else idxRoadGO = -2;
            yield return new WaitForSeconds(3f);
        }
    }
}
