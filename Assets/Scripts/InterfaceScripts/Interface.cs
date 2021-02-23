using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Interface : MonoBehaviour {
    private GameObject MainCamera;
    private Economy EconomyClass;
    private Builds BuildsClass;
    private Roads RoadsClass;
    private bool interfaceBuildsIsOpened = false;
    private bool InterfaceCommercesIsOpened = false;
    private bool InterfaceRoadsIsOpened = false;
    private bool InterfaceOtherIsOpened = false;
    public bool EconomyPanelActivity = false;
    private bool UpgradesPanelActivity = false;
    private bool isActivityInfo = true;
    private bool isBusy = false;
    private Animator InterfaceBuildsAnimator;
    private Animator InterfaceCommercesAnimator;
    private Animator InterfaceRoadsAnimator;
    private Animator InterfaceOtherAnimator;
    private Animator BuildPanelAnimator;
    private string lastMenu = "";

    public GameObject PreFubButton;
    public GameObject FastStats;
    public GameObject InterfaceBuilds;
    public GameObject InterfaceCommerces;
    public GameObject InterfaceRoads;
    public GameObject InterfaceOther;
    public GameObject InterfaceBuildsContent;
    public GameObject InterfaceCommercesContent;
    public GameObject InterfaceRoadsContent;
    public GameObject PanelClass;
    public GameObject EconomyPanelObject;
    public GameObject UpgradesPanelObject;
    public GameObject BuildPanelObject;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        EconomyClass = MainCamera.GetComponent <Economy> ();
        BuildsClass = MainCamera.GetComponent <Builds> ();
        RoadsClass = MainCamera.GetComponent <Roads> ();
        InterfaceBuildsAnimator = InterfaceBuilds.GetComponent <Animator> ();
        InterfaceCommercesAnimator = InterfaceCommerces.GetComponent <Animator> ();
        InterfaceRoadsAnimator = InterfaceRoads.GetComponent <Animator> ();
        InterfaceOtherAnimator = InterfaceOther.GetComponent <Animator> ();
        BuildPanelAnimator = BuildPanelObject.GetComponent<Animator>();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.F1) && !isBusy) {
            if (isActivityInfo) SetDisabilityInfo();
            else SetActivityInfo();
            isBusy = true;
        }
        else if (Input.GetKeyDown(KeyCode.Tab) && !isBusy) {
            FastStats.SetActive(!FastStats.activeSelf);
        }
        else isBusy = false;
    }

    public void DeactivateAllMenu() {
        if (interfaceBuildsIsOpened) {
            BuildPanelAnimator.Play("Back");
            InterfaceBuildsAnimator.Play("Back");
            interfaceBuildsIsOpened = false;
            lastMenu = "Builds";
        }
        if (InterfaceCommercesIsOpened) {
            BuildPanelAnimator.Play("Back");
            InterfaceCommercesAnimator.Play("Back");
            InterfaceCommercesIsOpened = false;
            lastMenu = "Commerces";
        }
        if (InterfaceRoadsIsOpened) {
            BuildPanelAnimator.Play("Back");
            InterfaceRoadsAnimator.Play("Back");
            InterfaceRoadsIsOpened = false;
            lastMenu = "Roads";
        }
        if (InterfaceOtherIsOpened) {
            BuildPanelAnimator.Play("Back");
            InterfaceOtherAnimator.Play("Back");
            InterfaceOtherIsOpened = false;
            lastMenu = "Other";
        }
        EconomyPanelActivity = false;
        UpgradesPanelActivity = false;
        UpgradesPanelObject.SetActive(UpgradesPanelActivity); 
        EconomyPanelObject.SetActive(EconomyPanelActivity);   
    }

    public void ActivateMenu(string type = "Last") {
        DeactivateAllMenu();
        switch (type) {
            case "Builds":
                if (!interfaceBuildsIsOpened) {
                    BuildPanelAnimator.Play("Forward");
                    InterfaceBuildsAnimator.Play("Forward");
                    interfaceBuildsIsOpened = true;
                }
                break;
            case "Commerces":
                if (!InterfaceCommercesIsOpened) {
                    BuildPanelAnimator.Play("Forward");
                    InterfaceCommercesAnimator.Play("Forward");
                    InterfaceCommercesIsOpened = true;
                }
                break;
            case "Roads":
                if (!InterfaceRoadsIsOpened) {
                    BuildPanelAnimator.Play("Forward");
                    InterfaceRoadsAnimator.Play("Forward");
                    InterfaceRoadsIsOpened = true;
                }
                break;
            case "Other":
                if (!InterfaceOtherIsOpened) {
                    BuildPanelAnimator.Play("Forward");
                    InterfaceOtherAnimator.Play("Forward");
                    InterfaceOtherIsOpened = true;
                }
                break;
            case "Last":
                ActivateMenu(lastMenu);
                break;
        }
    }

    public void SetActivityInfo() {
        PanelClass.SetActive(true);
        isActivityInfo = true;
    }

    public void SetDisabilityInfo() {
        PanelClass.SetActive(false);
        isActivityInfo = false;
    }
    public void SetEconomyPanelActivity()
    {
        EconomyPanelActivity = !EconomyPanelActivity;
        EconomyPanelObject.SetActive(EconomyPanelActivity);
        EconomyClass.FillInTheMenuWithStatistics();
    }
    public void SetUpgradesPanelActivity()
    {
        UpgradesPanelActivity = !UpgradesPanelActivity;
        UpgradesPanelObject.SetActive(UpgradesPanelActivity);
    }
    public void ExitToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}
