using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GenerationGraph : MonoBehaviour {
    private GameObject MainCamera;
    private Builds BuildsClass;
    private Roads RoadsClass;
    private Cars CarsClass;
    private Field FieldClass;

    public bool isRegeneration = false;
    public bool isOver = false;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        BuildsClass = MainCamera.GetComponent <Builds> ();
        RoadsClass = MainCamera.GetComponent <Roads> ();
        FieldClass = MainCamera.GetComponent <Field> ();
    }

    IEnumerator AsyncGen() {
        isRegeneration = true;
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
        yield return null;
        
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
        yield return null;
        isOver = true;
        isRegeneration = false;
    }

    public void StartGeneration() {
        StartCoroutine(AsyncGen());
    }
}
