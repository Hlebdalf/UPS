using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildObject : MonoBehaviour {
    private GameObject MainCamera;
    private Field FieldClass;
    private Interface InterfaceClass;
    private Economy EconomyClass;
    private Builds BuildsClass;
    private List <GameObject> posterObjects;
    private List <int> idxNotActive;
    private GameObject HouseMenu = null;
    private GameObject normObject;
    private GameObject deleteObject;

    public float x, y;
    public int idx, connectedRoad, idxCommerceType = -1, idxPreFub = -1;
    public int cntPeople = 0, maxCntPeople = 100, cntPosters = 0, maxCntPosters = 0;
    public long cost = 1000000, posterCost = 10000;
    public int startMenuSizeW = 200, startMenuSizeH = 100;
    public bool isGenBuild = false, isDeleting = false;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        FieldClass = MainCamera.GetComponent <Field> ();
        InterfaceClass = MainCamera.GetComponent <Interface> ();
        EconomyClass = MainCamera.GetComponent <Economy> ();
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
        if (Input.GetMouseButtonDown(1)) { CreateMenu(); print(3); }
    }

    public void CreateMenu() {
        if (!BuildsClass.InterfaceObject.GetComponent<Interface>().PosterPanelActivity)
        {
            BuildsClass.InterfaceObject.GetComponent<Interface>().SetPosterPanelActivity();
            print(1);
        }
        else
        {
            BuildsClass.InterfaceObject.GetComponent<Interface>().SetPosterPanelProperties();
        }
    }

    public void AddPoster() {
        if (idxNotActive.Count <= 0) return;
        EconomyClass.AddPoster(FieldClass.districts[(int)x + FieldClass.fieldSizeHalf, (int)y + FieldClass.fieldSizeHalf]);
        int idx = (int)UnityEngine.Random.Range(0f, idxNotActive.Count - 0.01f);
        posterObjects[idxNotActive[idx]].SetActive(true);
        Material posterMatHD = posterObjects[idxNotActive[idx]].GetComponent<Transform>().transform.Find("PosterPlaneHD").GetComponent<MeshRenderer>().material;
        Texture HologramBasemap = BuildsClass.posterTextures[(int)UnityEngine.Random.Range(0f, BuildsClass.posterTextures.Length - 0.01f)];
        posterMatHD.SetTexture("HologramAlbedo", HologramBasemap);
        posterMatHD.SetFloat("RandomFlash", UnityEngine.Random.Range(0.5f, 2f));
        idxNotActive.RemoveAt(idx);
        ++cntPosters;
    }
}
