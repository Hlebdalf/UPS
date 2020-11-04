using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManagement : MonoBehaviour {
    public Interface InterfaceClass;

    void Start() {
        InterfaceClass = InterfaceClass.GetComponent <Interface> ();
    }

    void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Input.GetMouseButtonDown(0)) {
            if (Physics.Raycast(ray, out hit)) {
                //InterfaceClass.SetPosAndBuildObject(hit.point);
            }
        }
    }
}
