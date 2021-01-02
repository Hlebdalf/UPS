using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceButtonManagement : MonoBehaviour {
    private GameObject MainCamera;
    private Interface InterfaceClass;
    private Builds BuildsClass;
    private Roads RoadsClass;

    private void Start() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        InterfaceClass = MainCamera.GetComponent <Interface> ();
        BuildsClass = MainCamera.GetComponent <Builds> ();
        RoadsClass = MainCamera.GetComponent <Roads> ();
    }

    private Vector3 GetMousePosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) return hit.point;
        return new Vector3(0, 0, 0);
    }

    public void House1() {
        BuildsClass.CreateGhost("House1", GetMousePosition());
    }

    public void House2() {
        BuildsClass.CreateGhost("House2", GetMousePosition());
    }

    public void Road() {
        RoadsClass.RoadType = "Road1";
    }
}
