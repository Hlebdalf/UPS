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
    private ulong seed;

    public GameObject loadPanelObj;
    public GameObject loadCircleObj;
    public int timeGeneration, maxCntRoads;
    public int minLenRoads, deltaLenRoads;
    public int averageCntCommercesInDistrict, averageCntParkingInDistrict;
    public float timeRoadsBuildGeneration, timeHousesBuildGeneration, timeCommerceBuildGeneration;
    public bool isOver = false;

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

    private void Start() {
        loadCircleObj = loadPanelObj.transform.Find("LoadCircle").gameObject;
    }

    public void FuncSeed() {
        seed = (ulong)(Math.PI * Math.Sqrt(seed) * (seed % 1e9 + 1));
    }

    public ulong FuncSeed(ulong _seed) {
        return (ulong)(Math.PI * Math.Sqrt(_seed) * (_seed % 1e9 + 1));
    }

    public ulong GetSeed() {
        return seed;
    }

    public bool CheckTime(DateTimeOffset endTime) {
        if (DateTimeOffset.Compare(DateTimeOffset.Now, endTime) >= 0) return false;
        else return true;
    }

    IEnumerator StartGen() {
        Debug.Log("Start Generation: " + DateTimeOffset.Now);
        if (MainMenuButtonManagment.seed == -1 || MainMenuButtonManagment.seed == 0)
            seed = (ulong)UnityEngine.Random.Range(0f, 1e9f);
        else seed = (ulong)MainMenuButtonManagment.seed;
        maxCntRoads = MainMenuButtonManagment.cntRoad;
        averageCntCommercesInDistrict = MainMenuButtonManagment.cntCommerce;
        averageCntParkingInDistrict = MainMenuButtonManagment.cntParking;
        
        timeRoadsBuildGeneration = timeGeneration * 0.01f;
        timeCommerceBuildGeneration = timeGeneration * 0.14f;
        timeHousesBuildGeneration = timeGeneration * 0.85f;

        yield return null;
        StartCoroutine(StartRoadsGen());
    }

    IEnumerator StartRoadsGen() {
        GenerationRoadsClass.StartGeneration();
        print("Прокладываем дороги...");
        while (!GenerationRoadsClass.isOver) {
            loadCircleObj.transform.Rotate(0, 0, 2);
            yield return null;
        }
        StartCoroutine(StartDistrictsGen());
    }

    IEnumerator StartDistrictsGen() {
        GenerationDistrictsClass.StartGeneration();
        print("Делим город на кварталы...");
        while (!GenerationDistrictsClass.isOver) {
            loadCircleObj.transform.Rotate(0, 0, 2);
            yield return null;
        }
        StartCoroutine(StartParkingsGen());
    }

    IEnumerator StartParkingsGen() {
        GenerationParkingsClass.StartGeneration();
        print("Строим парковки...");
        while (!GenerationParkingsClass.isOver) {
            loadCircleObj.transform.Rotate(0, 0, 2);
            yield return null;
        }
        StartCoroutine(StartCommercesGen());
    }

    IEnumerator StartCommercesGen() {
        GenerationCommercesClass.StartGeneration();
        print("Строим коммерцию...");
        while (!GenerationCommercesClass.isOver) {
            loadCircleObj.transform.Rotate(0, 0, 2);
            yield return null;
        }
        StartCoroutine(StartHousesGen());
    }

    IEnumerator StartHousesGen() {
        GenerationHousesClass.StartGeneration();
        print("Строим дома...");
        while (!GenerationHousesClass.isOver) {
            loadCircleObj.transform.Rotate(0, 0, 2);
            yield return null;
        }
        StartCoroutine(StartGraphGen());
    }

    IEnumerator StartGraphGen() {
        GenerationGraphClass.StartGeneration();
        print("Чертим план города...");
        while (!GenerationGraphClass.isOver) {
            loadCircleObj.transform.Rotate(0, 0, 2);
            yield return null;
        }
        StartCoroutine(EndGen());
    }

    IEnumerator EndGen() {
        yield return null;
        loadPanelObj.SetActive(false);
        Debug.Log("End Generation: " + DateTimeOffset.Now);
        isOver = true;
    }

    public void StartGeneration() {
        StartCoroutine(StartGen());
    }
}
