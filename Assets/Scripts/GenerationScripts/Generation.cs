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

    public ulong seed;
    public int timeGeneration, maxCntRoads;
    public int minLenRoads, deltaLenRoads;
    public int averageCntCommercesInDistrict, averageCntParkingInDistrict;
    public float timeRoadsBuildGeneration, timeHousesBuildGeneration, timeCommerceBuildGeneration;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        BuildsClass = MainCamera.GetComponent <Builds> ();
        RoadsClass = MainCamera.GetComponent <Roads> ();
        CarsClass = MainCamera.GetComponent <Cars> ();
        FieldClass = MainCamera.GetComponent <Field> ();
        GenerationRoadsClass = MainCamera.GetComponent <GenerationRoads> ();
        GenerationDistrictsClass = MainCamera.GetComponent <GenerationDistricts> ();
        GenerationHousesClass = MainCamera.GetComponent <GenerationHouses> ();
        GenerationCommercesClass = MainCamera.GetComponent <GenerationCommerces> ();
        GenerationParkingsClass = MainCamera.GetComponent <GenerationParkings> ();
    }

    public void GenerationGraph() {
        CarsClass.isRegeneration = true;
        FieldClass.numInGraph = new Dictionary <GameObject, int> ();
        FieldClass.objInGraph = new Dictionary <int, GameObject> ();

        for (int i = 0; i < BuildsClass.objects.Count; ++i) {
            FieldClass.numInGraph.Add(BuildsClass.objects[i], FieldClass.numInGraph.Count);
            FieldClass.objInGraph.Add(FieldClass.objInGraph.Count, BuildsClass.objects[i]);
        }
        for (int i = 0; i < BuildsClass.parkings.Count; ++i) {
            FieldClass.numInGraph.Add(BuildsClass.parkings[i], FieldClass.numInGraph.Count);
            FieldClass.objInGraph.Add(FieldClass.objInGraph.Count, BuildsClass.parkings[i]);
        }
        for (int i = 0; i < BuildsClass.commerces.Count; ++i) {
            FieldClass.numInGraph.Add(BuildsClass.commerces[i], FieldClass.numInGraph.Count);
            FieldClass.objInGraph.Add(FieldClass.objInGraph.Count, BuildsClass.commerces[i]);
        }
        for (int i = 0; i < RoadsClass.crossroads.Count; ++i) {
            FieldClass.numInGraph.Add(RoadsClass.crossroads[i], FieldClass.numInGraph.Count);
            FieldClass.objInGraph.Add(FieldClass.objInGraph.Count, RoadsClass.crossroads[i]);
        }

        FieldClass.graph = new List <(int v, float w)> [FieldClass.numInGraph.Count];
        for (int i = 0; i < FieldClass.numInGraph.Count; ++i) {
            FieldClass.graph[i] = new List <(int v, float w)> ();
        }
        
        foreach (KeyValuePair <GameObject, int> keyValue in FieldClass.numInGraph) {
            if (keyValue.Key.GetComponent <BuildObject> ()) {
                BuildObject BuildObjectClass = keyValue.Key.GetComponent <BuildObject> ();
                RoadObject RoadObjectClass = RoadsClass.objects[BuildObjectClass.connectedRoad].GetComponent <RoadObject> ();
                for (int i = 0; i < RoadObjectClass.connectedCrossroads.Count; ++i) {
                    GameObject crossroadObject = RoadsClass.crossroads[RoadObjectClass.connectedCrossroads[i]];
                    CrossroadObject crossroadObjectClass = crossroadObject.GetComponent <CrossroadObject> ();
                    float dist = (float)Math.Sqrt(Math.Pow(BuildObjectClass.x - crossroadObjectClass.x, 2) + Math.Pow(BuildObjectClass.y - crossroadObjectClass.y, 2));

                    FieldClass.graph[keyValue.Value].Add((FieldClass.numInGraph[crossroadObject], dist));
                    FieldClass.graph[FieldClass.numInGraph[crossroadObject]].Add((keyValue.Value, dist));
                }
            }
            if (keyValue.Key.GetComponent <CrossroadObject> ()) {
                CrossroadObject CrossroadObjectClass = keyValue.Key.GetComponent <CrossroadObject> ();
                for (int i = 0; i < CrossroadObjectClass.connectedRoads.Count; ++i) {
                    RoadObject RoadObjectClass = RoadsClass.objects[CrossroadObjectClass.connectedRoads[i]].GetComponent <RoadObject> ();
                    for (int j = 0; j < RoadObjectClass.connectedCrossroads.Count; ++j) {
                        GameObject crossroadObject = RoadsClass.crossroads[RoadObjectClass.connectedCrossroads[j]];
                        CrossroadObject CrossroadObjectClass2 = crossroadObject.GetComponent <CrossroadObject> ();
                        if (keyValue.Value != FieldClass.numInGraph[crossroadObject]) {
                            float dist = (float)Math.Sqrt(Math.Pow(CrossroadObjectClass.x - CrossroadObjectClass2.x, 2) + Math.Pow(CrossroadObjectClass.y - CrossroadObjectClass2.y, 2));
                            FieldClass.graph[keyValue.Value].Add((FieldClass.numInGraph[crossroadObject], dist));
                            FieldClass.graph[FieldClass.numInGraph[crossroadObject]].Add((keyValue.Value, dist));
                        }
                    }
                }
            }
        }
        CarsClass.isRegeneration = false;
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
        GenerationGraph();
        Debug.Log("End Generation: " + DateTimeOffset.Now);
    }
}
