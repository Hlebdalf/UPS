using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuButtonManagment : MonoBehaviour {
    public static long seed = -1;
    public static int cntRoad = 200;
    public static int cntCommerce = 15;
    public static int cntParking = 2;
    public static int cntCars = 2000;
    public static int cntPeople = 500;

    private Animator GodAnimator;
    private InputField seedText;
    private InputField cntRoadText;
    private InputField cntCommerceText;
    private InputField cntParkingText;
    private InputField cntCarsText;
    private InputField cntPeopleText;
    private GameObject loadCircleObj;

    public GameObject God;
    public GameObject seedObj;
    public GameObject cntRoadObj;
    public GameObject cntCommerceObj;
    public GameObject cntParkingObj;
    public GameObject cntCarsObj;
    public GameObject cntPeopleObj;
    public GameObject loadPanelObj;

    private void Start() {
        GodAnimator = God.GetComponent <Animator> ();
        seedText = seedObj.GetComponent <InputField> ();
        cntRoadText = cntRoadObj.GetComponent <InputField> ();
        cntCommerceText = cntCommerceObj.GetComponent <InputField> ();
        cntParkingText = cntParkingObj.GetComponent <InputField> ();
        cntCarsText = cntCarsObj.GetComponent <InputField> ();
        cntPeopleText = cntPeopleObj.GetComponent <InputField> ();
        loadCircleObj = loadPanelObj.transform.Find("LoadCircle").gameObject;
    }

    public void StartSettingsMenu() {
        GodAnimator.Play("LoadMenuAnimationForward");
        if (seed != -1) seedText.text = seed.ToString();
        cntRoadText.text = cntRoad.ToString();
        cntCommerceText.text = cntCommerce.ToString();
        cntParkingText.text = cntParking.ToString();
        cntCarsText.text = cntCars.ToString();
        cntPeopleText.text = cntPeople.ToString();
    }

    public void BackFromSettings() {
        GodAnimator.Play("LoadMenuAnimationBack");
    }

    public void StartExitMenu() {
        GodAnimator.SetFloat("Reverse", 1);
        GodAnimator.Play("ConfirmAnimationForward");
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void CancelExit() {
        GodAnimator.SetFloat("Reverse", -1);
        GodAnimator.Play("ConfirmAnimationBack");
    }

    IEnumerator LoadingLevel() {
        loadPanelObj.SetActive(true);
        yield return null;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("GameField");
        while (!asyncLoad.isDone) {
            loadCircleObj.transform.Rotate(0, 0, 2);
            yield return null;
        }
        Debug.Log("Loading complete");
    }

    public void StartGame() {
        StartCoroutine(LoadingLevel());
    }

    public void SeedInput() {
        if (seedText.text == "") return;
        MainMenuButtonManagment.seed = Math.Abs(Convert.ToInt64(seedText.text));
    }

    public void CntRoadInput() {
        if (cntRoadText.text == "") return;
        MainMenuButtonManagment.cntRoad = Convert.ToInt32(cntRoadText.text);
    }
    
    public void CntCommerceInput() {
        if (cntCommerceText.text == "") return;
        MainMenuButtonManagment.cntCommerce = Convert.ToInt32(cntCommerceText.text);
    }

    public void CntParkingInput() {
        if (cntParkingText.text == "") return;
        MainMenuButtonManagment.cntParking = Convert.ToInt32(cntParkingText.text);
    }

    public void CntCarsInput() {
        if (cntCarsText.text == "") return;
        MainMenuButtonManagment.cntCars = Convert.ToInt32(cntCarsText.text);
    }

    public void CntPeopleInput() {
        if (cntPeopleText.text == "") return;
        MainMenuButtonManagment.cntPeople = Convert.ToInt32(cntPeopleText.text);
    }
}
