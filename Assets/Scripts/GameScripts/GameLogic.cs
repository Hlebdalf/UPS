using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameLogic : MonoBehaviour {
    private GameObject MainCamera;
    private Generation GenerationClass;
    private Cars CarsClass;
    private People PeopleClass;
    private Field FieldClass;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        GenerationClass = MainCamera.GetComponent <Generation> ();
        CarsClass = MainCamera.GetComponent <Cars> ();
        PeopleClass = MainCamera.GetComponent <People> ();
        FieldClass = MainCamera.GetComponent <Field> ();
    }

    private void Start() {
        GenerationClass.StartGeneration();
        CarsClass.StartCars();
        PeopleClass.StartPeople();
    }
}
