using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Interface : MonoBehaviour {
    private GameObject MainCamera;
    private Builds BuildsClass;
    private Roads RoadsClass;
    private bool interfaceBuildsIsOpened = false;
    private bool InterfaceCommercesIsOpened = false;
    private bool InterfaceRoadsIsOpened = false;
    private bool InterfaceOtherIsOpened = false;
    private Animator InterfaceBuildsAnimator;
    private Animator InterfaceCommercesAnimator;
    private Animator InterfaceRoadsAnimator;
    private Animator InterfaceOtherAnimator;
    private bool isActivityInfo = true;
    private bool isBusy = false;
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

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        BuildsClass = MainCamera.GetComponent <Builds> ();
        RoadsClass = MainCamera.GetComponent <Roads> ();
        InterfaceBuildsAnimator = InterfaceBuilds.GetComponent <Animator> ();
        InterfaceCommercesAnimator = InterfaceCommerces.GetComponent <Animator> ();
        InterfaceRoadsAnimator = InterfaceRoads.GetComponent <Animator> ();
        InterfaceOtherAnimator = InterfaceOther.GetComponent <Animator> ();
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
            InterfaceBuildsAnimator.Play("Back");
            interfaceBuildsIsOpened = false;
            lastMenu = "Builds";
        }
        if (InterfaceCommercesIsOpened) {
            InterfaceCommercesAnimator.Play("Back");
            InterfaceCommercesIsOpened = false;
            lastMenu = "Commerces";
        }
        if (InterfaceRoadsIsOpened) {
            InterfaceRoadsAnimator.Play("Back");
            InterfaceRoadsIsOpened = false;
            lastMenu = "Roads";
        }
        if (InterfaceOtherIsOpened) {
            InterfaceOtherAnimator.Play("Back");
            InterfaceOtherIsOpened = false;
            lastMenu = "Other";
        }
    }

    public void ActivateMenu(string type = "Last") {
        DeactivateAllMenu();
        switch (type) {
            case "Builds":
                if (!interfaceBuildsIsOpened) {
                    InterfaceBuildsAnimator.Play("Forward");
                    interfaceBuildsIsOpened = true;
                }
                break;
            case "Commerces":
                if (!InterfaceCommercesIsOpened) {
                    InterfaceCommercesAnimator.Play("Forward");
                    InterfaceCommercesIsOpened = true;
                }
                break;
            case "Roads":
                if (!InterfaceRoadsIsOpened) {
                    InterfaceRoadsAnimator.Play("Forward");
                    InterfaceRoadsIsOpened = true;
                }
                break;
            case "Other":
                if (!InterfaceOtherIsOpened) {
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

    public void ExitToMainMenu() {
        SceneManager.LoadScene("MainMenu");
    }
}
