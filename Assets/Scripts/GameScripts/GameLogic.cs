using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameLogic : MonoBehaviour {
    private GameObject MainCamera;
    private Generation GenerationClass;
    private Interface InterfaceClass;
    private Cars CarsClass;
    private People PeopleClass;
    private GameObject LoadCircle;
    private bool enableLoadPlane = true;

    public GameObject InterfaceObject;
    public GameObject LoadPanel;

    IEnumerator LoadPanelDelay() {
        while (enableLoadPlane) {
            LoadCircle.transform.Rotate(0f, 0f, 10f);
            yield return new WaitForSeconds(0.1f);
        }
    }

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        GenerationClass = MainCamera.GetComponent <Generation> ();
        InterfaceClass = InterfaceObject.GetComponent <Interface> ();
        CarsClass = MainCamera.GetComponent <Cars> ();
        PeopleClass = MainCamera.GetComponent <People> ();
        LoadCircle = LoadPanel.transform.Find("LoadCircle").transform.gameObject;
    }

    private void Start() {
        StartCoroutine(LoadPanelDelay());
        GenerationClass.StartGeneration();
        InterfaceClass.CreateInterface();
        CarsClass.StartCars();
        PeopleClass.StartPeople();
        //enableLoadPlane = false;
        //LoadPanel.SetActive(false);
    }
}
