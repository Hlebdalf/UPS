﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Cars : MonoBehaviour {
    private GameObject MainCamera;
    private Builds BuildsClass;
    private Roads RoadsClass;
    private Field FieldClass;

    public GameObject[] preFubs;
    public List <GameObject> objects;
    public bool isStarted = false;
    public float eps = 0.5f;
    public float speed = 10;
    public int cntCars = 100;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        BuildsClass = MainCamera.GetComponent <Builds> ();
        RoadsClass = MainCamera.GetComponent <Roads> ();
        FieldClass = MainCamera.GetComponent <Field> ();
        objects = new List <GameObject> ();
    }

    private void Update() {
        if (isStarted && objects.Count < cntCars) {
            (List <GameObject> objectPathToStart, List <GameObject> objectPathToEnd, List <GameObject> objectPathToParking) paths;
            paths.objectPathToStart = new List <GameObject> ();
            paths.objectPathToEnd = new List <GameObject> ();
            paths.objectPathToParking = new List <GameObject> ();

            if ((int)UnityEngine.Random.Range(0, 1.99f) == 0) paths = StartFromHouse();
            else paths = StartFromCommerce();

            List <Vector3> pointsPathToStart = new List <Vector3> ();
            for (int i = 0; i < paths.objectPathToStart.Count; ++i) {
                GameObject obj = paths.objectPathToStart[i];
                if (obj.GetComponent <BuildObject> ()) {
                    BuildObject objClass = obj.GetComponent <BuildObject> ();
                    RoadObject roadObjClass = RoadsClass.objects[objClass.connectedRoad].GetComponent <RoadObject> ();

                    float mainRoadA = roadObjClass.y1 - roadObjClass.y2, mainRoadB = roadObjClass.x2 - roadObjClass.x1,
                        mainRoadC = roadObjClass.x1 * roadObjClass.y2 - roadObjClass.x2 * roadObjClass.y1; // main road line
                    float normA = -mainRoadB, normB = mainRoadA, normC = -(normA * objClass.x + normB * objClass.y); // norm
                    float normCrossMainRoadX = -(mainRoadC * normB - normC * mainRoadB) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate
                    float normCrossMainRoadY = -(mainRoadA * normC - normA * mainRoadC) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate

                    if (i == 0) {
                        pointsPathToStart.Add(new Vector3(objClass.x, 0, objClass.y));
                        pointsPathToStart.Add(new Vector3(normCrossMainRoadX, 0, normCrossMainRoadY));
                    }
                    else {
                        pointsPathToStart.Add(new Vector3(normCrossMainRoadX, 0, normCrossMainRoadY));
                    }
                }
                if (obj.GetComponent <CrossroadObject> ()) {
                    CrossroadObject objClass = obj.GetComponent <CrossroadObject> ();
                    pointsPathToStart.Add(new Vector3(objClass.x, 0, objClass.y));
                }
            }

            
            List <Vector3> pointsPathToEnd = new List <Vector3> ();
            for (int i = 0; i < paths.objectPathToEnd.Count; ++i) {
                GameObject obj = paths.objectPathToEnd[i];
                if (obj.GetComponent <BuildObject> ()) {
                    BuildObject objClass = obj.GetComponent <BuildObject> ();
                    RoadObject roadObjClass = RoadsClass.objects[objClass.connectedRoad].GetComponent <RoadObject> ();

                    float mainRoadA = roadObjClass.y1 - roadObjClass.y2, mainRoadB = roadObjClass.x2 - roadObjClass.x1,
                        mainRoadC = roadObjClass.x1 * roadObjClass.y2 - roadObjClass.x2 * roadObjClass.y1; // main road line
                    float normA = -mainRoadB, normB = mainRoadA, normC = -(normA * objClass.x + normB * objClass.y); // norm
                    float normCrossMainRoadX = -(mainRoadC * normB - normC * mainRoadB) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate
                    float normCrossMainRoadY = -(mainRoadA * normC - normA * mainRoadC) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate

                    if (i == 0) {
                        pointsPathToEnd.Add(new Vector3(normCrossMainRoadX, 0, normCrossMainRoadY));
                    }
                    else {
                        pointsPathToEnd.Add(new Vector3(normCrossMainRoadX, 0, normCrossMainRoadY));
                    }
                }
                if (obj.GetComponent <CrossroadObject> ()) {
                    CrossroadObject objClass = obj.GetComponent <CrossroadObject> ();
                    pointsPathToEnd.Add(new Vector3(objClass.x, 0, objClass.y));
                }
            }

            List <Vector3> pointsPathToParking = new List <Vector3> ();
            for (int i = 0; i < paths.objectPathToParking.Count; ++i) {
                GameObject obj = paths.objectPathToParking[i];
                if (obj.GetComponent <BuildObject> ()) {
                    BuildObject objClass = obj.GetComponent <BuildObject> ();
                    RoadObject roadObjClass = RoadsClass.objects[objClass.connectedRoad].GetComponent <RoadObject> ();

                    float mainRoadA = roadObjClass.y1 - roadObjClass.y2, mainRoadB = roadObjClass.x2 - roadObjClass.x1,
                        mainRoadC = roadObjClass.x1 * roadObjClass.y2 - roadObjClass.x2 * roadObjClass.y1; // main road line
                    float normA = -mainRoadB, normB = mainRoadA, normC = -(normA * objClass.x + normB * objClass.y); // norm
                    float normCrossMainRoadX = -(mainRoadC * normB - normC * mainRoadB) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate
                    float normCrossMainRoadY = -(mainRoadA * normC - normA * mainRoadC) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate

                    if (i == 0) {
                        pointsPathToParking.Add(new Vector3(normCrossMainRoadX, 0, normCrossMainRoadY));
                    }
                    else {
                        pointsPathToParking.Add(new Vector3(normCrossMainRoadX, 0, normCrossMainRoadY));
                        pointsPathToParking.Add(new Vector3(objClass.x, 0, objClass.y));
                    }
                }
                if (obj.GetComponent <CrossroadObject> ()) {
                    CrossroadObject objClass = obj.GetComponent <CrossroadObject> ();
                    pointsPathToParking.Add(new Vector3(objClass.x, 0, objClass.y));
                }
            }

            objects.Add(Instantiate(preFubs[(int)UnityEngine.Random.Range(0, preFubs.Length - 0.01f)], pointsPathToStart[0], Quaternion.Euler(0, 0, 0)));
            objects[objects.Count - 1].AddComponent <CarObject> ();
            CarObject carClass = objects[objects.Count - 1].GetComponent <CarObject> ();

            for (int i = 0; i < pointsPathToStart.Count; ++i) {
                carClass.queuePointsToStart.Enqueue(pointsPathToStart[i]);
            }
            for (int i = 0; i < pointsPathToEnd.Count; ++i) {
                carClass.queuePointsToEnd.Enqueue(pointsPathToEnd[i]);
            }
            for (int i = 0; i < pointsPathToParking.Count; ++i) {
                carClass.queuePointsToParking.Enqueue(pointsPathToParking[i]);
            }
        }
    }
    
    private (List <int> parent, List <float> dist) Dijkstra(int start) {
        List <float> dist = new List <float> ();
        List <int> parent = new List <int> ();
        List <bool> used = new List <bool> ();
        for (int i = 0; i < FieldClass.numInGraph.Count; ++i) {
            dist.Add((float)(1e9 + 7));
            parent.Add(-1);
            used.Add(false);
        }
        SortedSet <(float w, int v)> queueEdge = new SortedSet <(float w, int v)> ();
        dist[start] = 0;
        queueEdge.Add((0, start));
        while (queueEdge.Count > 0) {
            (float w, int v) p = queueEdge.Min;
            queueEdge.Remove(p);
            used[p.v] = true;
            for (int i = 0; i < FieldClass.graph[p.v].Count; ++i) {
                int u = FieldClass.graph[p.v][i].v;
                float w = FieldClass.graph[p.v][i].w;
                if (!used[u] && dist[u] > p.w + w) {
                    queueEdge.Remove((dist[u], u));
                    dist[u] = p.w + w;
                    parent[u] = p.v;
                    queueEdge.Add((dist[u], u));
                }
            }
        }
        return (parent, dist);
    }

    private (List <GameObject> objectPathToStart, List <GameObject> objectPathToEnd, List <GameObject> objectPathToParking) StartFromHouse() {
        GameObject BuildGameObject = BuildsClass.objects[(int)UnityEngine.Random.Range(0, BuildsClass.objects.Count - 0.01f)];
        GameObject CommerceGameObject = BuildsClass.commerces[(int)UnityEngine.Random.Range(0, BuildsClass.commerces.Count - 0.01f)];
        
        (List <int> parent, List <float> dist) graphData = Dijkstra(FieldClass.numInGraph[BuildGameObject]);

        int minDistI = 0;
        for (int i = 0; i < BuildsClass.parkings.Count; ++i) {
            float dist = 0f;
            for (int p = FieldClass.numInGraph[BuildsClass.parkings[i]], v = graphData.parent[p]; v != -1; p = v, v = graphData.parent[v]) {
                for (int j = 0; j < FieldClass.graph[v].Count; ++j) {
                    if (FieldClass.graph[v][j].v == p) {
                        dist += FieldClass.graph[v][j].w;
                        break;
                    }
                }
                if (FieldClass.objInGraph[v] == BuildGameObject) break;
            }
            if (graphData.dist[FieldClass.numInGraph[BuildsClass.parkings[i]]] < dist)
                minDistI = i;
        }

        GameObject Parking = BuildsClass.parkings[minDistI];
        
        List <GameObject> objectPathToStart = new List <GameObject> ();
        for (int v = FieldClass.numInGraph[Parking]; v != -1; v = graphData.parent[v]) {
            objectPathToStart.Add(FieldClass.objInGraph[v]);
            if (FieldClass.objInGraph[v] == BuildGameObject) break;
        }

        List <GameObject> objectPathToEnd = new List <GameObject> ();
        for (int v = FieldClass.numInGraph[CommerceGameObject]; v != -1; v = graphData.parent[v]) {
            objectPathToEnd.Add(FieldClass.objInGraph[v]);
            if (FieldClass.objInGraph[v] == BuildGameObject) break;
        }
        objectPathToEnd.Reverse();

        graphData = Dijkstra(FieldClass.numInGraph[CommerceGameObject]);

        minDistI = 0;
        for (int i = 0; i < BuildsClass.parkings.Count; ++i) {
            float dist = 0f;
            for (int p = FieldClass.numInGraph[BuildsClass.parkings[i]], v = graphData.parent[p]; v != -1; p = v, v = graphData.parent[v]) {
                for (int j = 0; j < FieldClass.graph[v].Count; ++j) {
                    if (FieldClass.graph[v][j].v == p) {
                        dist += FieldClass.graph[v][j].w;
                        break;
                    }
                }
                if (FieldClass.objInGraph[v] == CommerceGameObject) break;
            }
            if (graphData.dist[FieldClass.numInGraph[BuildsClass.parkings[i]]] < dist)
                minDistI = i;
        }

        Parking = BuildsClass.parkings[minDistI];

        List <GameObject> objectPathToParking = new List <GameObject> ();
        for (int v = FieldClass.numInGraph[Parking]; v != -1; v = graphData.parent[v]) {
            objectPathToParking.Add(FieldClass.objInGraph[v]);
            if (FieldClass.objInGraph[v] == CommerceGameObject) break;
        }
        objectPathToParking.Reverse();

        return (objectPathToStart, objectPathToEnd, objectPathToParking);
    }

    private (List <GameObject> objectPathToStart, List <GameObject> objectPathToEnd, List <GameObject> objectPathToParking) StartFromCommerce() {
        GameObject CommerceGameObject = BuildsClass.commerces[(int)UnityEngine.Random.Range(0, BuildsClass.commerces.Count - 0.01f)];
        GameObject BuildGameObject = BuildsClass.objects[(int)UnityEngine.Random.Range(0, BuildsClass.objects.Count - 0.01f)];
        
        (List <int> parent, List <float> dist) graphData = Dijkstra(FieldClass.numInGraph[CommerceGameObject]);

        int minDistI = 0;
        for (int i = 0; i < BuildsClass.parkings.Count; ++i) {
            float dist = 0f;
            for (int p = FieldClass.numInGraph[BuildsClass.parkings[i]], v = graphData.parent[p]; v != -1; p = v, v = graphData.parent[v]) {
                for (int j = 0; j < FieldClass.graph[v].Count; ++j) {
                    if (FieldClass.graph[v][j].v == p) {
                        dist += FieldClass.graph[v][j].w;
                        break;
                    }
                }
                if (FieldClass.objInGraph[v] == CommerceGameObject) break;
            }
            if (graphData.dist[FieldClass.numInGraph[BuildsClass.parkings[i]]] < dist)
                minDistI = i;
        }

        GameObject Parking = BuildsClass.parkings[minDistI];
        
        List <GameObject> objectPathToStart = new List <GameObject> ();
        for (int v = FieldClass.numInGraph[Parking]; v != -1; v = graphData.parent[v]) {
            objectPathToStart.Add(FieldClass.objInGraph[v]);
            if (FieldClass.objInGraph[v] == CommerceGameObject) break;
        }

        List <GameObject> objectPathToEnd = new List <GameObject> ();
        for (int v = FieldClass.numInGraph[BuildGameObject]; v != -1; v = graphData.parent[v]) {
            objectPathToEnd.Add(FieldClass.objInGraph[v]);
            if (FieldClass.objInGraph[v] == CommerceGameObject) break;
        }
        objectPathToEnd.Reverse();

        graphData = Dijkstra(FieldClass.numInGraph[BuildGameObject]);

        minDistI = 0;
        for (int i = 0; i < BuildsClass.parkings.Count; ++i) {
            float dist = 0f;
            for (int p = FieldClass.numInGraph[BuildsClass.parkings[i]], v = graphData.parent[p]; v != -1; p = v, v = graphData.parent[v]) {
                for (int j = 0; j < FieldClass.graph[v].Count; ++j) {
                    if (FieldClass.graph[v][j].v == p) {
                        dist += FieldClass.graph[v][j].w;
                        break;
                    }
                }
                if (FieldClass.objInGraph[v] == BuildGameObject) break;
            }
            if (graphData.dist[FieldClass.numInGraph[BuildsClass.parkings[i]]] < dist)
                minDistI = i;
        }

        Parking = BuildsClass.parkings[minDistI];

        List <GameObject> objectPathToParking = new List <GameObject> ();
        for (int v = FieldClass.numInGraph[Parking]; v != -1; v = graphData.parent[v]) {
            objectPathToParking.Add(FieldClass.objInGraph[v]);
            if (FieldClass.objInGraph[v] == BuildGameObject) break;
        }
        objectPathToParking.Reverse();

        return (objectPathToStart, objectPathToEnd, objectPathToParking);
    }

    public void StartCars() {
        isStarted = true;
    }

    public void DeleteObject(GameObject obj) {
        objects.Remove(obj);
        Destroy(obj);
    }
}
