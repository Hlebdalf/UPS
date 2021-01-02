using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildProcessObject : MonoBehaviour {
    private GameObject MainCamera;
    private Builds BuildsClass;
    private Field FieldClass;

    public float x, y, rotate;
    public int idxPreFub, connectedRoad;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        BuildsClass = MainCamera.GetComponent <Builds> ();
        FieldClass = MainCamera.GetComponent <Field> ();
    }

    private void Start() {
        StartCoroutine(Delay_COR());
    }

    IEnumerator Delay_COR() {
        yield return new WaitForSeconds(FieldClass.timeBuildProcess);
        BuildsClass.CreateObject(new Vector3(x, 0, y), rotate, idxPreFub, connectedRoad);
        Destroy(gameObject);
    }
}
