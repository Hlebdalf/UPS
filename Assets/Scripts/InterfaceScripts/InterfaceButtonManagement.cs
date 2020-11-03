using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceButtonManagement : MonoBehaviour {
    public Interface InterfaceClass;

    void Start() {
        InterfaceClass = InterfaceClass.GetComponent <Interface> ();
    }

    public void House1() {
        InterfaceClass.SetTypeObject("House1");
    }

    public void House2() {
        InterfaceClass.SetTypeObject("House2");
    }

    public void Road() {
        InterfaceClass.SetTypeObject("Road");
    }
}
