using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceButtonManagement : MonoBehaviour {
    public Interface InterfaceClass;
    public Builds BuildsClass;

    void Start() {
        InterfaceClass = InterfaceClass.GetComponent <Interface> ();
        BuildsClass = BuildsClass.GetComponent <Builds> ();
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
        if (!InterfaceClass.IsTypeSelected())
            BuildsClass.CreateGhost("House1", GetMousePosition());
        InterfaceClass.SetTypeObject("House1");
    }

    public void House2() {
        if (!InterfaceClass.IsTypeSelected())
            BuildsClass.CreateGhost("House2", GetMousePosition());
        InterfaceClass.SetTypeObject("House2");
    }

    public void Road() {
        if (!InterfaceClass.IsTypeSelected())
            BuildsClass.CreateGhost("Road", GetMousePosition());
        InterfaceClass.SetTypeObject("Road");
    }
}
