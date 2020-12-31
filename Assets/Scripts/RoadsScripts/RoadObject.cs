using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadObject : MonoBehaviour {
    private GameObject MainCamera;
    private Roads RoadsClass;

    public float x1, y1, x2, y2, len, angle;
    public int idx;
    public bool inProgress = true;
    public List <int> connectedCrossroads;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        RoadsClass = MainCamera.GetComponent <Roads> ();
        connectedCrossroads = new List <int> ();
    }

    // private void OnTriggerStay(Collider other) {
    //     if (inProgress) {
    //         RoadsClass.objects.Remove(gameObject);
    //         Destroy(gameObject);
    //     }
    // }

    private void OnMouseOver() {
        RoadsClass.idxOverRoad = idx;
    }

    private void OnMouseExit() {
        RoadsClass.idxOverRoad = -1;
    }
}
