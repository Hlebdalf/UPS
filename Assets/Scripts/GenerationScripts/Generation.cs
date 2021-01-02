using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Generation : MonoBehaviour {
    private GameObject MainCamera;
    private GenerationRoads GenerationRoadsClass;
    private GenerationDistricts GenerationDistrictsClass;
    private GenerationHouses GenerationHousesClass;
    private GenerationCommerces GenerationCommercesClass;

    public ulong seed;
    public int timeBuild, maxCntRoads;
    public int minLenRoads, deltaLenRoads;
    public int averageCntCommercesInDistrict;
    public float timeRoadsBuildGeneration, timeHousesBuildGeneration, timeCommerceBuildGeneration;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        GenerationRoadsClass = MainCamera.GetComponent <GenerationRoads> ();
        GenerationDistrictsClass = MainCamera.GetComponent <GenerationDistricts> ();
        GenerationHousesClass = MainCamera.GetComponent <GenerationHouses> ();
        GenerationCommercesClass = MainCamera.GetComponent <GenerationCommerces> ();
    }

    private void Start() {
        timeRoadsBuildGeneration = timeBuild * 0.01f;
        timeCommerceBuildGeneration = timeBuild * 0.14f;
        timeHousesBuildGeneration = timeBuild * 0.85f;
        seed = GenerationRoadsClass.StartGeneration(seed);
        seed = GenerationDistrictsClass.StartGeneration(seed);
        seed = GenerationCommercesClass.StartGeneration(seed);
        seed = GenerationHousesClass.StartGeneration(seed);
    }

    public ulong funcSeed(ulong _seed) {
        return (ulong)(Math.PI * Math.Sqrt(_seed) * (_seed % 1e9 + 1));
    }

    public bool CheckTime(DateTimeOffset endTime) {
        if (DateTimeOffset.Compare(DateTimeOffset.Now, endTime) >= 0) return false;
        else return true;
    }
}
