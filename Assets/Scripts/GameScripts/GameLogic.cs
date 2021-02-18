using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameLogic : MonoBehaviour {
    private GameObject MainCamera;
    private Generation GenerationClass;
    private Interface InterfaceClass;
    private Economy EconomyClass;
    private Cars CarsClass;
    private People PeopleClass;
    private bool isChangeOfDay = false;

    public GameObject InterfaceObject;
    public GameObject LightObject;
    public float gameSpeed = 1f, preAngleLight = 45;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        GenerationClass = MainCamera.GetComponent <Generation> ();
        InterfaceClass = InterfaceObject.GetComponent <Interface> ();
        EconomyClass = MainCamera.GetComponent <Economy> ();
        CarsClass = MainCamera.GetComponent <Cars> ();
        PeopleClass = MainCamera.GetComponent <People> ();
    }

    private void FixedUpdate() {
        if ((int)(LightObject.transform.eulerAngles.x) == 45 && preAngleLight < LightObject.transform.eulerAngles.x && !isChangeOfDay) {
            StartCoroutine(ChangeOfDay());
            EconomyClass.NewDay();
        }
        preAngleLight = LightObject.transform.eulerAngles.x;
        LightObject.transform.Rotate(gameSpeed * Time.deltaTime, 0, 0);
    }

    IEnumerator ChangeOfDay() {
        isChangeOfDay = true;
        yield return new WaitForSeconds(5f);
        isChangeOfDay = false;
    }

    IEnumerator AsyncStartLogic() {
        GenerationClass.StartGeneration();
        while (!GenerationClass.isOver) {
            yield return null;
        }
        CarsClass.StartCars();
        PeopleClass.StartPeople();
        EconomyClass.StartEconomy();
    }

    private void Start() {
        StartCoroutine(AsyncStartLogic());
    }
}
