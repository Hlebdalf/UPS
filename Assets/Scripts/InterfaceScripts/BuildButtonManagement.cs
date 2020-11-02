using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildButtonManagement : MonoBehaviour {

    public Builds BuildsClass;

    void Start() {
        BuildsClass = BuildsClass.GetComponent <Builds> ();
    }

    public void House1() {
        BuildsClass.BuildObject("House1");
    }

    public void House2() {
        BuildsClass.BuildObject("House2");
    }

    public void Road() {
        BuildsClass.BuildObject("Road");
    }
}
