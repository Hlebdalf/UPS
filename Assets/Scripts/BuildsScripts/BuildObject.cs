using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObject : MonoBehaviour {
    private GameObject MainCamera;
    private Interface InterfaceClass;
    private Builds BuildsClass;
    private List <GameObject> posterObjects;
    private List <int> idxNotActive;
    private GameObject HouseMenu = null;

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
            if (child.gameObject.name == "PosterPlane") {
                posterObjects.Add(child.gameObject);
                idxNotActive.Add(idxNotActive.Count);
            }
        }
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(1)) CreateMenu();
    }

    private void CreateMenu() {
        if (HouseMenu == null) {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(x, gameObject.GetComponent <BoxCollider> ().size.y * 0.1f, y));
            HouseMenu = Instantiate(BuildsClass.PreFubHouseMenu, screenPos, Quaternion.Euler(0, 0, 0));
            HouseMenu.transform.SetParent(BuildsClass.InterfaceObject.transform);
            HouseMenu.GetComponent <HouseMenu> ().ActivateMenu(gameObject);
        }
    }

    public void DestroyMenu() {
        Destroy(HouseMenu);
        HouseMenu = null;
    }

    public void AddPoster() {
        if (idxNotActive.Count <= 0) return;
        int idx = (int)UnityEngine.Random.Range(0f, idxNotActive.Count - 0.01f);
        posterObjects[idxNotActive[idx]].SetActive(true);
        idxNotActive.RemoveAt(idx);
    }
}
