using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRoad : MonoBehaviour {
    private GameObject MainCamera;
    private Roads RoadsClass;

    void Start() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        RoadsClass = MainCamera.GetComponent <Roads> ();
    }

    void OnMouseOver() {
        RoadsClass.SetIsOverRoad(RoadsClass.GetIndex(gameObject));
    }

    void OnMouseExit() {
        RoadsClass.SetIsOverRoad(-1);
    }
}
