using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceButtonManagement : MonoBehaviour {
    public Interface InterfaceClass;
    public Builds BuildsClass;
    public Roads RoadsClass;

    void Start() {
        InterfaceClass = InterfaceClass.GetComponent <Interface> ();
        BuildsClass = BuildsClass.GetComponent <Builds> ();
        RoadsClass = RoadsClass.GetComponent <Roads> ();
    }

    private Vector3 GetMousePosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            return hit.point;
        }
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
