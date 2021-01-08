﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GameLogic : MonoBehaviour {
    private GameObject MainCamera;
    private Generation GenerationClass;
    private Interface InterfaceClass;
    private Cars CarsClass;
    private People PeopleClass;

    public GameObject InterfaceObject;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        GenerationClass = MainCamera.GetComponent <Generation> ();
        InterfaceClass = InterfaceObject.GetComponent <Interface> ();
        CarsClass = MainCamera.GetComponent <Cars> ();
        PeopleClass = MainCamera.GetComponent <People> ();
    }

    private void Start() {
        GenerationClass.StartGeneration();
        InterfaceClass.CreateInterface();
        CarsClass.StartCars();
        PeopleClass.StartPeople();
    }
}