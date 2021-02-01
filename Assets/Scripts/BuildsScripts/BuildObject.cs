using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObject : MonoBehaviour {
    private GameObject MainCamera;
    private Interface InterfaceClass;
    private Builds BuildsClass;
    private List <GameObject> posterObjects;
    private List <int> idxNotActive;

    public float x, y;
    public int idx, connectedRoad, idxCommerceType = -1, idxPreFub = -1;
    public int maxCntPeople = 0, cntPosters = 0, maxCntPosters = 0;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        InterfaceClass = MainCamera.GetComponent <Interface> ();
        BuildsClass = MainCamera.GetComponent <Builds> ();
        posterObjects = new List <GameObject> ();
        idxNotActive = new List <int> ();
    }

    private void Start() {
        foreach (Transform child in gameObject.transform) {
            print(child.gameObject.name);
            if (child.gameObject.name == "PosterPlane") {
                posterObjects.Add(child.gameObject);
                idxNotActive.Add(idxNotActive.Count);
            }
        }
    }

    private void OnMouseDown() {
        if (Input.GetMouseButtonDown(1)) {
            BuildsClass.HouseMenuClass.DeactivateMenu();
            BuildsClass.HouseMenuClass.ActivateMenu(gameObject.GetComponent <BuildObject> ());
        }
        if (Input.GetMouseButtonDown(0)) AddPoster();
    }

    public void AddPoster() {
        if (idxNotActive.Count <= 0) return;
        int idx = (int)UnityEngine.Random.Range(0f, idxNotActive.Count - 0.01f);
        posterObjects[idxNotActive[idx]].SetActive(true);
        idxNotActive.RemoveAt(idx);
    }
}
