using System.Collections;
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
            List <GameObject> objectPath = new List <GameObject> ();
            if ((int)UnityEngine.Random.Range(0, 1.99f) == 0) objectPath = StartFromHouse();
            else objectPath = StartFromCommerce();

            List <Vector3> pointsPath = new List <Vector3> ();
            for (int j = 0; j < objectPath.Count; ++j) {
                GameObject obj = objectPath[j];
                if (obj.GetComponent <BuildObject> ()) {
                    BuildObject objClass = obj.GetComponent <BuildObject> ();
                    pointsPath.Add(new Vector3(objClass.x, 0, objClass.y));
                }
                if (obj.GetComponent <CrossroadObject> ()) {
                    CrossroadObject objClass = obj.GetComponent <CrossroadObject> ();
                    pointsPath.Add(new Vector3(objClass.x, 0, objClass.y));
                }
            }

            objects.Add(Instantiate(preFubs[0], pointsPath[0], Quaternion.Euler(0, 0, 0)));
            objects[objects.Count - 1].AddComponent <CarObject> ();
            CarObject carClass = objects[objects.Count - 1].GetComponent <CarObject> ();
            for (int j = 0; j < pointsPath.Count; ++j) {
                carClass.queuePoints.Enqueue(pointsPath[j]);
            }
        }
    }
    
    private List <int> Dijkstra(int start) {
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
        return parent;
    }

    private List <GameObject> StartFromHouse() {
        GameObject BuildGameObject = BuildsClass.objects[(int)UnityEngine.Random.Range(0, BuildsClass.objects.Count - 1)];
        GameObject CommerceGameObject = BuildsClass.commerces[(int)UnityEngine.Random.Range(0, BuildsClass.commerces.Count - 1)];
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
        GameObject CommerceGameObject = BuildsClass.commerces[(int)UnityEngine.Random.Range(0, BuildsClass.commerces.Count - 1)];
        GameObject BuildGameObject = BuildsClass.objects[(int)UnityEngine.Random.Range(0, BuildsClass.objects.Count - 1)];
        List <int> parent = Dijkstra(FieldClass.numInGraph[CommerceGameObject]);
        List <GameObject> objectPath = new List <GameObject> ();
        for (int v = FieldClass.numInGraph[BuildGameObject]; v != -1; v = parent[v]) {
            objectPath.Add(FieldClass.objInGraph[v]);
            if (FieldClass.objInGraph[v] == CommerceGameObject) break;
        }
        objectPath.Reverse();
        return objectPath;
    }

    public void StartCars() {
        isStarted = true;
    }

    public void DeleteObject(GameObject obj) {
        objects.Remove(obj);
        Destroy(obj);
    }
}
