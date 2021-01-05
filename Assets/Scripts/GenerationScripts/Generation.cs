using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Generation : MonoBehaviour {
    private GameObject MainCamera;
    private Builds BuildsClass;
    private Roads RoadsClass;
    private Cars CarsClass;
    private Field FieldClass;
    private GenerationRoads GenerationRoadsClass;
    private GenerationDistricts GenerationDistrictsClass;
    private GenerationHouses GenerationHousesClass;
    private GenerationCommerces GenerationCommercesClass;
    private GenerationParkings GenerationParkingsClass;
    private GenerationGraph GenerationGraphClass;

    public ulong seed;
    public int timeGeneration, maxCntRoads;
    public int minLenRoads, deltaLenRoads;
    public int averageCntCommercesInDistrict, averageCntParkingInDistrict;
    public float timeRoadsBuildGeneration, timeHousesBuildGeneration, timeCommerceBuildGeneration;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        BuildsClass = MainCamera.GetComponent <Builds> ();
        RoadsClass = MainCamera.GetComponent <Roads> ();
        FieldClass = MainCamera.GetComponent <Field> ();
        GenerationRoadsClass = MainCamera.GetComponent <GenerationRoads> ();
        GenerationDistrictsClass = MainCamera.GetComponent <GenerationDistricts> ();
        GenerationHousesClass = MainCamera.GetComponent <GenerationHouses> ();
        GenerationCommercesClass = MainCamera.GetComponent <GenerationCommerces> ();
        GenerationParkingsClass = MainCamera.GetComponent <GenerationParkings> ();
        GenerationGraphClass = MainCamera.GetComponent <GenerationGraph> ();
    }

    public ulong funcSeed(ulong _seed) {
        return (ulong)(Math.PI * Math.Sqrt(_seed) * (_seed % 1e9 + 1));
    }

    public bool CheckTime(DateTimeOffset endTime) {
        if (DateTimeOffset.Compare(DateTimeOffset.Now, endTime) >= 0) return false;
        else return true;
    }

    public void StartGeneration() {
        Debug.Log("Start Generation: " + DateTimeOffset.Now);
        timeRoadsBuildGeneration = timeGeneration * 0.01f;
        timeCommerceBuildGeneration = timeGeneration * 0.14f;
        timeHousesBuildGeneration = timeGeneration * 0.85f;
        seed = GenerationRoadsClass.StartGeneration(seed);
        seed = GenerationDistrictsClass.StartGeneration(seed);
        seed = GenerationParkingsClass.StartGeneration(seed);
        seed = GenerationCommercesClass.StartGeneration(seed);
        seed = GenerationHousesClass.StartGeneration(seed);
        GenerationGraphClass.StartGeneration();
        Debug.Log("End Generation: " + DateTimeOffset.Now);
    }
}
