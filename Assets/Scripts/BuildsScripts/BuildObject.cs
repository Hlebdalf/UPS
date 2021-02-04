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
    private GameObject normObject;
    private GameObject deleteObject;

    public float x, y;
    public int idx, connectedRoad, idxCommerceType = -1, idxPreFub = -1;
    public int maxCntPeople = 0, cntPosters = 0, maxCntPosters = 0;
    public int startMenuSizeW = 200, startMenuSizeH = 100;
    public bool isGenBuild = false, isDeleting = false;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        InterfaceClass = MainCamera.GetComponent <Interface> ();
        BuildsClass = MainCamera.GetComponent <Builds> ();
        posterObjects = new List <GameObject> ();
        idxNotActive = new List <int> ();
        normObject = gameObject.transform.Find("Build").gameObject;
        deleteObject = gameObject.transform.Find("DeletingBuild").gameObject;
    }

    private void Start() {
        Transform parent = gameObject.transform.Find("Build");
        foreach (Transform child in parent) {
            if (child.gameObject.name == "PosterPlane") {
                posterObjects.Add(child.gameObject);
                idxNotActive.Add(idxNotActive.Count);
            }
        }
        maxCntPosters = posterObjects.Count;
        if (isGenBuild) {
            int defaultCntPosters = (int)UnityEngine.Random.Range(0f, maxCntPosters + 0.99f);
            for (int i = 0; i < defaultCntPosters; ++i) AddPoster();
        }
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0) && BuildsClass.isDeleting) {
            if (isDeleting) {
                BuildsClass.deleteObjects.Remove(gameObject);
                normObject.SetActive(true);
                deleteObject.SetActive(false);
            }
            else {
                BuildsClass.deleteObjects.Add(gameObject);
                normObject.SetActive(false);
                deleteObject.SetActive(true);
            }
            isDeleting = !isDeleting;
        }
        if (Input.GetMouseButtonDown(1)) CreateMenu();
    }

    public void CreateMenu() {
        if (HouseMenu == null) {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(new Vector3(x, gameObject.GetComponent <BoxCollider> ().size.y * gameObject.transform.localScale.x, y));
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
        Material posterMat = posterObjects[idxNotActive[idx]].GetComponent <MeshRenderer> ().material;
        posterMat.SetTexture("HologramAlbedo", BuildsClass.posterTextures[(int)UnityEngine.Random.Range(0f, BuildsClass.posterTextures.Length - 0.01f)]);
        posterMat.SetFloat("RandomFlash", UnityEngine.Random.Range(0.5f, 2f));
        posterObjects[idxNotActive[idx]].GetComponent <MeshRenderer> ().material = posterMat;
        idxNotActive.RemoveAt(idx);
        ++cntPosters;
    }
}
