using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManagement : MonoBehaviour {
    public float rayDistance;
    public Interface InterfaceClass;

    void Start() {
        InterfaceClass = InterfaceClass.GetComponent <Interface> ();
    }

    void Update() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // Debug.DrawRay(transform.position, ray.direction * rayDistance);
        if (Input.GetMouseButtonDown(0)) {
            if (Physics.Raycast(ray, out hit)) {
                // Debug.Log(hit.point);
                InterfaceClass.SetPosAndBuildObject(hit.point);
            }
        }
    }
}
