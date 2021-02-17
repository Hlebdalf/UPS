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

    public GameObject InterfaceObject;
    public GameObject LightObject;
    public float gameSpeed = 1f;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        GenerationClass = MainCamera.GetComponent <Generation> ();
        InterfaceClass = InterfaceObject.GetComponent <Interface> ();
        EconomyClass = MainCamera.GetComponent <Economy> ();
        CarsClass = MainCamera.GetComponent <Cars> ();
        PeopleClass = MainCamera.GetComponent <People> ();
    }

    private void FixedUpdate() {
        LightObject.transform.Rotate(gameSpeed * Time.deltaTime, 0, 0);
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
