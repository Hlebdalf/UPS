﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class People : MonoBehaviour {
    private GameObject MainCamera;
    private Builds BuildsClass;
    private Roads RoadsClass;
    private Field FieldClass;

    public GameObject[] preFubs;
    public List <GameObject> objects;
    public GameObject Checkobject;
    public bool isStarted = false, isRegeneration = false;
    public float eps = 0.01f;
    public float speed = 1;
    public int cntPeople = 10000;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        BuildsClass = MainCamera.GetComponent <Builds> ();
        RoadsClass = MainCamera.GetComponent <Roads> ();
        FieldClass = MainCamera.GetComponent <Field> ();
        objects = new List <GameObject> ();
    }

    private void Update() {
        if (isStarted && objects.Count < cntPeople && !isRegeneration) {
            List <GameObject> objectPath = new List <GameObject> ();

            if ((int)UnityEngine.Random.Range(0, 1.99f) == 0) objectPath = StartFromHouse();
            else objectPath = StartFromCommerce();

            List <Vector3> pointsPath = new List <Vector3> ();
            for (int i = 0; i < objectPath.Count; ++i) {
                GameObject obj = objectPath[i];
                if (obj.GetComponent <BuildObject> ()) {
                    BuildObject objClass = obj.GetComponent <BuildObject> ();
                    RoadObject roadObjClass = RoadsClass.objects[objClass.connectedRoad].GetComponent <RoadObject> ();

                    float mainRoadA = roadObjClass.y1 - roadObjClass.y2, mainRoadB = roadObjClass.x2 - roadObjClass.x1,
                        mainRoadC = roadObjClass.x1 * roadObjClass.y2 - roadObjClass.x2 * roadObjClass.y1; // main road line
                    float normA = -mainRoadB, normB = mainRoadA, normC = -(normA * objClass.x + normB * objClass.y); // norm
                    float normCrossMainRoadX = -(mainRoadC * normB - normC * mainRoadB) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate
                    float normCrossMainRoadY = -(mainRoadA * normC - normA * mainRoadC) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate

                    if (i == 0) {
                        pointsPath.Add(new Vector3(objClass.x, 0, objClass.y));
                        pointsPath.Add(new Vector3(normCrossMainRoadX, 0, normCrossMainRoadY));
                    }
                    else {
                        pointsPath.Add(new Vector3(normCrossMainRoadX, 0, normCrossMainRoadY));
                        pointsPath.Add(new Vector3(objClass.x, 0, objClass.y));
                    }
                }
                if (obj.GetComponent <CrossroadObject> ()) {
                    CrossroadObject objClass = obj.GetComponent <CrossroadObject> ();
                    pointsPath.Add(new Vector3(objClass.x, 0, objClass.y));
                }
            }

            Vector3 crossSave = pointsPath[0], toSave = pointsPath[1];
            for (int i = 0; i < pointsPath.Count - 2; ++i) {
                Vector3 from = crossSave, cross1 = toSave, cross2 = toSave, to = pointsPath[i + 2];
                crossSave = toSave;
                toSave = to;

                float mainRoadA1 = from.z - cross1.z, mainRoadB1 = cross1.x - from.x, mainRoadC1 = from.x * cross1.z - cross1.x * from.z; // line1
                float mainRoadA2 = cross2.z - to.z, mainRoadB2 = to.x - cross2.x, mainRoadC2 = cross2.x * to.z - to.x * cross2.z; // line2

                float x1, y1;
                if (mainRoadA1 == 0) {
                    x1 = 0f;
                    y1 = 0.45f;
                }
                else {
                    x1 = (float)Math.Abs(Math.Cos(Math.Atan(mainRoadB1 / mainRoadA1)) * (0.9f));
                    y1 = (float)Math.Abs(Math.Sin(Math.Atan(mainRoadB1 / mainRoadA1)) * (0.9f));
                }
                float cs1 = cross1.x - from.x;
                float sn1 = cross1.z - from.z;
                if (cs1 >= 0 && sn1 >= 0) {
                    from.x += x1;
                    from.z -= y1;
                    cross1.x += x1;
                    cross1.z -= y1;
                }
                if (cs1 <= 0 && sn1 >= 0) {
                    from.x += x1;
                    from.z += y1;
                    cross1.x += x1;
                    cross1.z += y1;
                }
                if (cs1 <= 0 && sn1 <= 0) {
                    from.x -= x1;
                    from.z += y1;
                    cross1.x -= x1;
                    cross1.z += y1;
                }
                if (cs1 >= 0 && sn1 <= 0) {
                    from.x -= x1;
                    from.z -= y1;
                    cross1.x -= x1;
                    cross1.z -= y1;
                }

                float x2, y2;
                if (mainRoadA2 == 0) {
                    x2 = 0f;
                    y2 = 0.45f;
                }
                else {
                    x2 = (float)Math.Abs(Math.Cos(Math.Atan(mainRoadB2 / mainRoadA2)) * (0.9f));
                    y2 = (float)Math.Abs(Math.Sin(Math.Atan(mainRoadB2 / mainRoadA2)) * (0.9f));
                }
                float cs2 = to.x - cross2.x;
                float sn2 = to.z - cross2.z;
                if (cs2 >= 0 && sn2 >= 0) {
                    cross2.x += x2;
                    cross2.z -= y2;
                    to.x += x2;
                    to.z -= y2;
                }
                if (cs2 <= 0 && sn2 >= 0) {
                    cross2.x += x2;
                    cross2.z += y2;
                    to.x += x2;
                    to.z += y2;
                }
                if (cs2 <= 0 && sn2 <= 0) {
                    cross2.x -= x2;
                    cross2.z += y2;
                    to.x -= x2;
                    to.z += y2;
                }
                if (cs2 >= 0 && sn2 <= 0) {
                    cross2.x -= x2;
                    cross2.z -= y2;
                    to.x -= x2;
                    to.z -= y2;
                }

                // line1
                mainRoadA1 = from.z - cross1.z;
                mainRoadB1 = cross1.x - from.x;
                mainRoadC1 = from.x * cross1.z - cross1.x * from.z;
                // line2
                mainRoadA2 = cross2.z - to.z;
                mainRoadB2 = to.x - cross2.x;
                mainRoadC2 = cross2.x * to.z - to.x * cross2.z;

                if (mainRoadA1 * mainRoadB2 - mainRoadA2 * mainRoadB1 != 0) {
                    float mainRoad1CrossmainRoad2X = -(mainRoadC1 * mainRoadB2 - mainRoadC2 * mainRoadB1) / (mainRoadA1 * mainRoadB2 - mainRoadA2 * mainRoadB1); // cross coordinate
                    float mainRoad1CrossmainRoad2Y = -(mainRoadA1 * mainRoadC2 - mainRoadA2 * mainRoadC1) / (mainRoadA1 * mainRoadB2 - mainRoadA2 * mainRoadB1); // cross coordinate

                    pointsPath[i + 1] = new Vector3(mainRoad1CrossmainRoad2X, 0, mainRoad1CrossmainRoad2Y);
                }
            }

            objects.Add(Instantiate(preFubs[(int)UnityEngine.Random.Range(0, preFubs.Length - 0.01f)], pointsPath[0], Quaternion.Euler(0, 0, 0)));
            objects[objects.Count - 1].AddComponent <HumanObject> ();
            HumanObject humanClass = objects[objects.Count - 1].GetComponent <HumanObject> ();

            for (int i = 0; i < pointsPath.Count; ++i) {
                humanClass.queuePoints.Enqueue(pointsPath[i]);
            }
        }
    }
    
    private List <int> Dijkstra(int start) {
        List <(int v, float w)>[] graph = FieldClass.graph;
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
            for (int i = 0; i < graph[p.v].Count; ++i) {
                int u = graph[p.v][i].v;
                float w = graph[p.v][i].w;
                if (!used[u] && dist[u] > p.w + w) {
                    queueEdge.Remove((dist[u], u));
                    dist[u] = p.w + w;
                    parent[u] = p.v;
                    queueEdge.Add((dist[u], u));
                }
            }
        }
        return parent;
    }

    private List <GameObject> StartFromHouse() {
        GameObject BuildGameObject = BuildsClass.objects[(int)UnityEngine.Random.Range(0, BuildsClass.objects.Count - 0.01f)];
        GameObject CommerceGameObject = BuildsClass.commerces[(int)UnityEngine.Random.Range(0, BuildsClass.commerces.Count - 0.01f)];
        
        List <int> parent = Dijkstra(FieldClass.numInGraph[BuildGameObject]);

        List <GameObject> objectPath = new List <GameObject> ();
        for (int v = FieldClass.numInGraph[CommerceGameObject]; v != -1; v = parent[v]) {
            objectPath.Add(FieldClass.objInGraph[v]);
            if (FieldClass.objInGraph[v] == BuildGameObject) break;
        }
        objectPath.Reverse();

        return objectPath;
    }

    private List <GameObject> StartFromCommerce() {
        GameObject CommerceGameObject = BuildsClass.commerces[(int)UnityEngine.Random.Range(0, BuildsClass.commerces.Count - 0.01f)];
        GameObject BuildGameObject = BuildsClass.objects[(int)UnityEngine.Random.Range(0, BuildsClass.objects.Count - 0.01f)];
        
        List <int> parent = Dijkstra(FieldClass.numInGraph[CommerceGameObject]);

        List <GameObject> objectPath = new List <GameObject> ();
        for (int v = FieldClass.numInGraph[BuildGameObject]; v != -1; v = parent[v]) {
            objectPath.Add(FieldClass.objInGraph[v]);
            if (FieldClass.objInGraph[v] == CommerceGameObject) break;
        }
        objectPath.Reverse();

        return objectPath;
    }

    public void StartPeople() {
        isStarted = true;
    }

    public void DeleteObject(GameObject obj) {
        objects.Remove(obj);
        Destroy(obj);
    }
}
