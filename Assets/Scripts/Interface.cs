using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interface : MonoBehaviour {
    public Builds BuildsClass;
    private string typeObject;
    private Vector3 pointObject;
    
    void Start() {
        BuildsClass = BuildsClass.GetComponent <Builds> ();
        typeObject = "";
    }

    private void BuildObject(string type, Vector3 point) {
        BuildsClass.BuildObject(type, point);
    }

    public void SetTypeObject(string type) {
        typeObject = type;
    }

    public void SetPosObject(Vector3 point) {
        pointObject = point;
    }

    public void SetPosAndBuildObject(Vector3 point) {
        if (typeObject != "") {
            BuildObject(typeObject, point);
            typeObject = "";
        }
    }
}
