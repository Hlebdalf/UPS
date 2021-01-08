using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interface : MonoBehaviour {
    private GameObject MainCamera;
    private Builds BuildsClass;
    private Roads RoadsClass;

    public GameObject PreFubButton;
    public GameObject InterfaceBuilds;
    public GameObject InterfaceCommerces;
    public GameObject InterfaceRoads;
    public GameObject InterfaceOther;
    public GameObject InterfaceBuildsContent;
    public GameObject InterfaceCommercesContent;
    public GameObject InterfaceRoadsContent;
    public string[] buildNames;
    public string[] commerceNames;
    public string[] roadNames;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        BuildsClass = MainCamera.GetComponent <Builds> ();
        RoadsClass = MainCamera.GetComponent <Roads> ();
    }

    private void CreateInterfaceBuilds() {
        int posX = 100;
        int cntButton = 0;
        for (int i = 0; i < BuildsClass.idxsDistrict1.Length; ++i) {
            GameObject newButton = Instantiate(PreFubButton, new Vector3(posX, 0, 0), Quaternion.Euler(0, 0, 0));
            newButton.transform.SetParent(InterfaceBuildsContent.transform);
            newButton.GetComponent <InterfaceButton> ().Init(buildNames[cntButton], BuildsClass.preFubs[BuildsClass.idxsDistrict1[i]].name, "House");
            posX += 200;
            ++cntButton;
        }
        for (int i = 0; i < BuildsClass.idxsDistrict2.Length; ++i) {
            GameObject newButton = Instantiate(PreFubButton, new Vector3(posX, 0, 0), Quaternion.Euler(0, 0, 0));
            newButton.transform.SetParent(InterfaceBuildsContent.transform);
            newButton.GetComponent <InterfaceButton> ().Init(buildNames[cntButton], BuildsClass.preFubs[BuildsClass.idxsDistrict2[i]].name, "House");
            posX += 200;
            ++cntButton;
        }
        for (int i = 0; i < BuildsClass.idxsDistrict3.Length; ++i) {
            GameObject newButton = Instantiate(PreFubButton, new Vector3(posX, 0, 0), Quaternion.Euler(0, 0, 0));
            newButton.transform.SetParent(InterfaceBuildsContent.transform);
            newButton.GetComponent <InterfaceButton> ().Init(buildNames[cntButton], BuildsClass.preFubs[BuildsClass.idxsDistrict3[i]].name, "House");
            posX += 200;
            ++cntButton;
        }
        for (int i = 0; i < BuildsClass.idxsDistrict4.Length; ++i) {
            GameObject newButton = Instantiate(PreFubButton, new Vector3(posX, 0, 0), Quaternion.Euler(0, 0, 0));
            newButton.transform.SetParent(InterfaceBuildsContent.transform);
            newButton.GetComponent <InterfaceButton> ().Init(buildNames[cntButton], BuildsClass.preFubs[BuildsClass.idxsDistrict4[i]].name, "House");
            posX += 200;
            ++cntButton;
        }
    }

    private void CreateInterfaceCommerces() {
        int posX = 100;
        for (int i = 0; i < BuildsClass.idxsCommerces.Length; ++i) {
            GameObject newButton = Instantiate(PreFubButton, new Vector3(posX, 0, 0), Quaternion.Euler(0, 0, 0));
            newButton.transform.SetParent(InterfaceCommercesContent.transform);
            newButton.GetComponent <InterfaceButton> ().Init(commerceNames[i], BuildsClass.preFubs[BuildsClass.idxsCommerces[i]].name, "House");
            posX += 200;
        }
    }

    private void CreateInterfaceRoads() {
        int posX = 100;
        for (int i = 0; i < RoadsClass.preFubs.Length; ++i) {
            GameObject newButton = Instantiate(PreFubButton, new Vector3(posX, 0, 0), Quaternion.Euler(0, 0, 0));
            newButton.transform.SetParent(InterfaceRoadsContent.transform);
            newButton.GetComponent <InterfaceButton> ().Init(roadNames[i], RoadsClass.preFubs[i].name, "Road");
            posX += 200;
        }
    }

    public void CreateInterface() {
        CreateInterfaceBuilds();
        CreateInterfaceCommerces();
        CreateInterfaceRoads();
    }

    public void DeactivateAllMenu() {
        InterfaceBuilds.SetActive(false);
        InterfaceCommerces.SetActive(false);
        InterfaceRoads.SetActive(false);
        InterfaceOther.SetActive(false);
    }

    public void ActivateMenu(string type) {
        DeactivateAllMenu();
        switch (type) {
            case "Builds":
                InterfaceBuilds.SetActive(true);
                break;
            case "Commerces":
                InterfaceCommerces.SetActive(true);
                break;
            case "Roads":
                InterfaceRoads.SetActive(true);
                break;
            case "Other":
                InterfaceOther.SetActive(true);
                break;
        }
    }
}
