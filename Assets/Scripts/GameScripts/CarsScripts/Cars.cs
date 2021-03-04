using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Jobs;
using Unity.Jobs;
using Unity.Collections;

public class Cars : MonoBehaviour {
    private GameObject MainCamera;
    private Builds BuildsClass;
    private Roads RoadsClass;
    private Field FieldClass;
    private GenerationGraph GenerationGraphClass;
    private List <(List <Vector3> pointsPathToStart, List <Vector3> pointsPathToEnd, List <Vector3> pointsPathToParking)> paths, pathsDefault;
    private List <int> itForQueue;
    private List <int> cntFrameForDelay;
    private int cntCars = 1000;

    private JobHandle handle;
    private NativeArray <Vector3> vertexTo;
    private NativeArray <Vector3> vertexFrom;
    private NativeArray <bool> isShiftVector;
    private NativeArray <int> numOfLanes;
    private NativeArray <bool> vertexIsActive;
    private NativeArray <bool> onVisibleInCamera;
    private NativeArray <int> cntWaitingFrames;
    private NativeArray <float> speeds;
    private int cntMissedFrames = 0;

    public GameObject[] preFubs;
    public List <GameObject> objects;
    public List <Car> objectClasses;
    public bool isStarted = false, isRegeneration = false;
    public LineRenderer linePath;
    public GameObject visiblePathObj = null;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        BuildsClass = MainCamera.GetComponent <Builds> ();
        RoadsClass = MainCamera.GetComponent <Roads> ();
        FieldClass = MainCamera.GetComponent <Field> ();
        GenerationGraphClass = MainCamera.GetComponent <GenerationGraph> ();
        cntCars = MainMenuButtonManagment.cntCars;
        objects = new List <GameObject> ();
        objectClasses = new List <Car> ();
    }

    private void OnEnable() {
        paths = new List <(List <Vector3> pointsPathToStart, List <Vector3> pointsPathToEnd, List <Vector3> pointsPathToParking)> ();
        pathsDefault = new List <(List <Vector3> pointsPathToStart, List <Vector3> pointsPathToEnd, List <Vector3> pointsPathToParking)> ();
        itForQueue = new List <int> ();
        cntFrameForDelay = new List <int> ();
        vertexTo = new NativeArray <Vector3> (cntCars, Allocator.Persistent);
        vertexFrom = new NativeArray <Vector3> (cntCars, Allocator.Persistent);
        isShiftVector = new NativeArray <bool> (cntCars, Allocator.Persistent);
        numOfLanes = new NativeArray <int> (cntCars, Allocator.Persistent);
        vertexIsActive = new NativeArray <bool> (cntCars, Allocator.Persistent);
        onVisibleInCamera = new NativeArray <bool> (cntCars, Allocator.Persistent);
        cntWaitingFrames = new NativeArray <int> (cntCars, Allocator.Persistent);
        speeds = new NativeArray <float> (cntCars, Allocator.Persistent);
        for (int i = 0; i < cntCars; ++i) {
            vertexIsActive[i] = false;
            onVisibleInCamera[i] = false;
            cntWaitingFrames[i] = 0;
            speeds[i] = 10f;
            numOfLanes[i] = 0;
        }
    }

    private void Update() {
        if (isStarted && objects.Count < cntCars && !GenerationGraphClass.isRegeneration) {
            if (BuildsClass.objects.Count <= 0) {
                Debug.Log("There is not houses");
            }
            else if (BuildsClass.commerces.Count <= 0) {
                Debug.Log("There is not commerce");
            }
            else {
                (List <GameObject> objectPathToStart, List <GameObject> objectPathToEnd, List <GameObject> objectPathToParking) localPaths;
                if ((int)UnityEngine.Random.Range(0, 1.99f) == 0) localPaths = StartFromHouse();
                else localPaths = StartFromCommerce();

                if (!localPaths.objectPathToStart[0].GetComponent <Parking> ().carInParking) {
                    List <Vector3> pointsPathToStart = ObjectsToPoints(localPaths.objectPathToStart, "ToStart");
                    List <Vector3> pointsPathToEnd = ObjectsToPoints(localPaths.objectPathToEnd, "ToEnd");
                    List <Vector3> pointsPathToParking = ObjectsToPoints(localPaths.objectPathToParking, "ToParking");
                    
                    List <Vector3> pointsPathToStartDefault = new List <Vector3> ();
                    for (int i = 0; i < pointsPathToStart.Count; ++i)
                        pointsPathToStartDefault.Add(pointsPathToStart[i]);

                    List <Vector3> pointsPathToEndDefault = new List <Vector3> ();
                    for (int i = 0; i < pointsPathToEnd.Count; ++i)
                        pointsPathToEndDefault.Add(pointsPathToEnd[i]);

                    List <Vector3> pointsPathToParkingDefault = new List <Vector3> ();
                    for (int i = 0; i < pointsPathToParking.Count; ++i)
                        pointsPathToParkingDefault.Add(pointsPathToParking[i]);
                    
                    pointsPathToStart = ShiftRoadVectors(pointsPathToStart, 1, pointsPathToStart.Count - 1);
                    pointsPathToEnd = ShiftRoadVectors(pointsPathToEnd, 0, pointsPathToEnd.Count - 1);
                    pointsPathToParking = ShiftRoadVectors(pointsPathToParking, 0, pointsPathToParking.Count - 2);

                    objects.Add(Instantiate(preFubs[(int)UnityEngine.Random.Range(0, preFubs.Length - 0.01f)], pointsPathToStart[0], Quaternion.Euler(0, 0, 0)));
                    objectClasses.Add(objects[objects.Count - 1].GetComponent <Car> ());
                    paths.Add((pointsPathToStart, pointsPathToEnd, pointsPathToParking));
                    pathsDefault.Add((pointsPathToStartDefault, pointsPathToEndDefault, pointsPathToParkingDefault));
                    vertexIsActive[objects.Count - 1] = false;
                    onVisibleInCamera[objects.Count - 1] = false;
                    cntWaitingFrames[objects.Count - 1] = 0;
                    speeds[objects.Count - 1] = 10f;
                    isShiftVector[objects.Count - 1] = false;
                    numOfLanes[objects.Count - 1] = 0;
                    itForQueue.Add(1);
                    cntFrameForDelay.Add(0);
                }
            }
        }
    }

    private void FixedUpdate() {
        if (handle.IsCompleted) {
            Transform[] transformArray = new Transform[objects.Count];
            for (int i = 0; i < objects.Count; ++i) {
                transformArray[i] = objects[i].transform;
                onVisibleInCamera[i] = objectClasses[i].onVisibleInCamera;
                speeds[i] = objectClasses[i].speed;
                if (itForQueue[i] == 2 || itForQueue[i] == paths[i].pointsPathToStart.Count + paths[i].pointsPathToEnd.Count + paths[i].pointsPathToParking.Count) {
                    isShiftVector[i] = false;
                }
                else isShiftVector[i] = true;
                // numOfLanes[i] = objectClasses[i].numOfLane;
                if (!vertexIsActive[i]) {
                    numOfLanes[i] = (int)UnityEngine.Random.Range(1f, 2.99f);
                    if (itForQueue[i] < paths[i].pointsPathToStart.Count) {
                        if (objects[i] == visiblePathObj) {
                            linePath.positionCount = paths[i].pointsPathToStart.Count;
                            for (int j = 0; j < paths[i].pointsPathToStart.Count; ++j) {
                                Vector3 pos = paths[i].pointsPathToStart[j];
                                linePath.SetPosition(j, new Vector3(pos.x, 0.5f, pos.z));
                            }
                        }
                        objectClasses[i].idxRoad = GetIdxRoad(pathsDefault[i].pointsPathToStart[itForQueue[i] - 1], pathsDefault[i].pointsPathToStart[itForQueue[i]]);
                        vertexFrom[i] = paths[i].pointsPathToStart[itForQueue[i] - 1];
                        vertexTo[i] = paths[i].pointsPathToStart[itForQueue[i]++];
                        vertexIsActive[i] = true;
                    }
                    else if (cntFrameForDelay[i] < 100) {
                        ++cntFrameForDelay[i];
                    }
                    else if (itForQueue[i] == paths[i].pointsPathToStart.Count) {
                        if (objects[i] == visiblePathObj) {
                            linePath.positionCount = paths[i].pointsPathToEnd.Count;
                            for (int j = 0; j < paths[i].pointsPathToEnd.Count; ++j) {
                                Vector3 pos = paths[i].pointsPathToEnd[j];
                                linePath.SetPosition(j, new Vector3(pos.x, 0.5f, pos.z));
                            }
                        }
                        objectClasses[i].idxRoad = GetIdxRoad(pathsDefault[i].pointsPathToStart[itForQueue[i] - 1], pathsDefault[i].pointsPathToEnd[itForQueue[i] - pathsDefault[i].pointsPathToStart.Count]);
                        vertexFrom[i] = paths[i].pointsPathToStart[itForQueue[i] - 1];
                        vertexTo[i] = paths[i].pointsPathToEnd[itForQueue[i]++ - paths[i].pointsPathToStart.Count];
                        vertexIsActive[i] = true;
                    }
                    else if (itForQueue[i] < paths[i].pointsPathToStart.Count + paths[i].pointsPathToEnd.Count) {
                        if (objects[i] == visiblePathObj) {
                            linePath.positionCount = paths[i].pointsPathToEnd.Count;
                            for (int j = 0; j < paths[i].pointsPathToEnd.Count; ++j) {
                                Vector3 pos = paths[i].pointsPathToEnd[j];
                                linePath.SetPosition(j, new Vector3(pos.x, 0.5f, pos.z));
                            }
                        }
                        objectClasses[i].idxRoad = GetIdxRoad(pathsDefault[i].pointsPathToEnd[itForQueue[i] - pathsDefault[i].pointsPathToStart.Count - 1], pathsDefault[i].pointsPathToEnd[itForQueue[i] - pathsDefault[i].pointsPathToStart.Count]);
                        vertexFrom[i] = paths[i].pointsPathToEnd[itForQueue[i] - paths[i].pointsPathToStart.Count - 1];
                        vertexTo[i] = paths[i].pointsPathToEnd[itForQueue[i]++ - paths[i].pointsPathToStart.Count];
                        vertexIsActive[i] = true;
                    }
                    else if (cntFrameForDelay[i] < 200) {
                        ++cntFrameForDelay[i];
                    }
                    else if (itForQueue[i] == paths[i].pointsPathToStart.Count + paths[i].pointsPathToEnd.Count) {
                        if (objects[i] == visiblePathObj) {
                            linePath.positionCount = paths[i].pointsPathToParking.Count;
                            for (int j = 0; j < paths[i].pointsPathToParking.Count; ++j) {
                                Vector3 pos = paths[i].pointsPathToParking[j];
                                linePath.SetPosition(j, new Vector3(pos.x, 0.5f, pos.z));
                            }
                        }
                        objectClasses[i].idxRoad = GetIdxRoad(pathsDefault[i].pointsPathToEnd[itForQueue[i] - pathsDefault[i].pointsPathToStart.Count - 1], pathsDefault[i].pointsPathToParking[itForQueue[i] - pathsDefault[i].pointsPathToStart.Count - pathsDefault[i].pointsPathToEnd.Count]);
                        vertexFrom[i] = paths[i].pointsPathToEnd[itForQueue[i] - paths[i].pointsPathToStart.Count - 1];
                        vertexTo[i] = paths[i].pointsPathToParking[itForQueue[i]++ - paths[i].pointsPathToStart.Count - paths[i].pointsPathToEnd.Count];
                        vertexIsActive[i] = true;
                    }
                    else if (itForQueue[i] < paths[i].pointsPathToStart.Count + paths[i].pointsPathToEnd.Count + paths[i].pointsPathToParking.Count) {
                        if (objects[i] == visiblePathObj) {
                            linePath.positionCount = paths[i].pointsPathToParking.Count;
                            for (int j = 0; j < paths[i].pointsPathToParking.Count; ++j) {
                                Vector3 pos = paths[i].pointsPathToParking[j];
                                linePath.SetPosition(j, new Vector3(pos.x, 0.5f, pos.z));
                            }
                        }
                        objectClasses[i].idxRoad = GetIdxRoad(pathsDefault[i].pointsPathToParking[itForQueue[i] - pathsDefault[i].pointsPathToStart.Count - pathsDefault[i].pointsPathToEnd.Count - 1], pathsDefault[i].pointsPathToParking[itForQueue[i] - pathsDefault[i].pointsPathToStart.Count - pathsDefault[i].pointsPathToEnd.Count]);
                        vertexFrom[i] = paths[i].pointsPathToParking[itForQueue[i] - paths[i].pointsPathToStart.Count - paths[i].pointsPathToEnd.Count - 1];
                        vertexTo[i] = paths[i].pointsPathToParking[itForQueue[i]++ - paths[i].pointsPathToStart.Count - paths[i].pointsPathToEnd.Count];
                        vertexIsActive[i] = true;
                    }
                    else {
                        if (objects[i] == visiblePathObj) linePath.positionCount = 0;
                        DeleteObject(i);
                    }
                }
            }
            TransformAccessArray transformAccessArray = new TransformAccessArray(transformArray);

            CarMoveJob job = new CarMoveJob();
            job.vertexTo = vertexTo;
            job.vertexFrom = vertexFrom;
            job.isShiftVector = isShiftVector;
            job.vertexIsActive = vertexIsActive;
            job.onVisibleInCamera = onVisibleInCamera;
            job.cntWaitingFrames = cntWaitingFrames;
            job.speeds = speeds;
            job.numOfLanes = numOfLanes;
            job.cameraPos = MainCamera.transform.position;
            job.fixedDeltaTime = Time.fixedDeltaTime;
            job.cntMissedFrames = cntMissedFrames;

            handle = job.Schedule(transformAccessArray);
            handle.Complete();

            transformAccessArray.Dispose();
            cntMissedFrames = 0;
        }
        else ++cntMissedFrames;
    }

    private void OnDisable() {
        vertexTo.Dispose();
        vertexFrom.Dispose();
        isShiftVector.Dispose();
        vertexIsActive.Dispose();
        onVisibleInCamera.Dispose();
        cntWaitingFrames.Dispose();
        speeds.Dispose();
        numOfLanes.Dispose();
    }

    private int GetIdxRoad(Vector3 From, Vector3 To) {
        GameObject fromObj = FieldClass.objects[(int)From.x + FieldClass.fieldSizeHalf, (int)From.z + FieldClass.fieldSizeHalf];
        GameObject toObj = FieldClass.objects[(int)To.x + FieldClass.fieldSizeHalf, (int)To.z + FieldClass.fieldSizeHalf];
        if (fromObj == toObj) return -1;

        if (fromObj && toObj) {
            if (fromObj.GetComponent <CrossroadObject> () && toObj.GetComponent <CrossroadObject> ()) {
                CrossroadObject crossroadObjectClassFrom = fromObj.GetComponent <CrossroadObject> ();
                CrossroadObject crossroadObjectClassTo = toObj.GetComponent <CrossroadObject> ();
                for (int i = 0; i < crossroadObjectClassFrom.connectedRoads.Count; ++i) {
                    for (int j = 0; j < crossroadObjectClassTo.connectedRoads.Count; ++j) {
                        if (crossroadObjectClassFrom.connectedRoads[i] == crossroadObjectClassTo.connectedRoads[j]) {
                            return crossroadObjectClassFrom.connectedRoads[i];
                        }
                    }
                }
                return -1;
            }
            else return -1;
        }
        else return -1;
    }

    private List <Vector3> ShiftRoadVectors(List <Vector3> pointsPath, int start, int end) {
        for (int i = start; i < end; i += 2) {
            Vector3 From = pointsPath[i], To = pointsPath[i + 1];
            float lessDX = (float)Math.Cos(Math.Atan2(To.z - From.z, To.x - From.x)) * 1f;
            float lessDZ = (float)Math.Sin(Math.Atan2(To.z - From.z, To.x - From.x)) * 1f;
            if (i > start) {
                From.x += lessDX;
                From.z += lessDZ;
            }
            if (i + 1 < end) {
                To.x -= lessDX;
                To.z -= lessDZ;
            }
            pointsPath[i] = From;
            pointsPath[i + 1] = To;
        }
        return pointsPath;
    }

    private List <Vector3> ObjectsToPoints(List <GameObject> objectPath, string type) {
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
                    if (type == "ToStart") pointsPath.Add(new Vector3(objClass.x, 0, objClass.y));
                    pointsPath.Add(new Vector3(normCrossMainRoadX, 0, normCrossMainRoadY));
                }
                else {
                    pointsPath.Add(new Vector3(normCrossMainRoadX, 0, normCrossMainRoadY));
                    if (type == "ToParking") pointsPath.Add(new Vector3(objClass.x, 0, objClass.y));
                }
            }
            if (obj.GetComponent <CrossroadObject> ()) {
                CrossroadObject objClass = obj.GetComponent <CrossroadObject> ();
                pointsPath.Add(new Vector3(objClass.x, 0, objClass.y));
                pointsPath.Add(new Vector3(objClass.x, 0, objClass.y));
            }
        }
        return pointsPath;
    }
    
    private (List <int> parent, List <float> dist) Dijkstra(int start) {
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
        return (parent, dist);
    }

    private (List <GameObject> objectPathToStart, List <GameObject> objectPathToEnd, List <GameObject> objectPathToParking) StartFromHouse() {
        GameObject BuildGameObject = BuildsClass.objects[(int)UnityEngine.Random.Range(0, BuildsClass.objects.Count - 0.01f)];
        GameObject CommerceGameObject = BuildsClass.commerces[(int)UnityEngine.Random.Range(0, BuildsClass.commerces.Count - 0.01f)];
        
        if (GenerationGraphClass.isRegeneration) {
            while (GenerationGraphClass.isRegeneration) {}
            return StartFromHouse();
        }
        
        (List <int> parent, List <float> dist) graphData = Dijkstra(FieldClass.numInGraph[BuildGameObject]);
        
        if (GenerationGraphClass.isRegeneration) {
            while (GenerationGraphClass.isRegeneration) {}
            return StartFromHouse();
        }

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
        
        if (GenerationGraphClass.isRegeneration) {
            while (GenerationGraphClass.isRegeneration) {}
            return StartFromHouse();
        }

        GameObject Parking = BuildsClass.parkings[minDistI];
        
        List <GameObject> objectPathToStart = new List <GameObject> ();
        for (int v = FieldClass.numInGraph[Parking]; v != -1; v = graphData.parent[v]) {
            objectPathToStart.Add(FieldClass.objInGraph[v]);
            if (FieldClass.objInGraph[v] == BuildGameObject) break;
        }
        
        if (GenerationGraphClass.isRegeneration) {
            while (GenerationGraphClass.isRegeneration) {}
            return StartFromHouse();
        }

        List <GameObject> objectPathToEnd = new List <GameObject> ();
        for (int v = FieldClass.numInGraph[CommerceGameObject]; v != -1; v = graphData.parent[v]) {
            objectPathToEnd.Add(FieldClass.objInGraph[v]);
            if (FieldClass.objInGraph[v] == BuildGameObject) break;
        }
        objectPathToEnd.Reverse();
        
        if (GenerationGraphClass.isRegeneration) {
            while (GenerationGraphClass.isRegeneration) {}
            return StartFromHouse();
        }

        graphData = Dijkstra(FieldClass.numInGraph[CommerceGameObject]);
        
        if (GenerationGraphClass.isRegeneration) {
            while (GenerationGraphClass.isRegeneration) {}
            return StartFromHouse();
        }

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
        
        if (GenerationGraphClass.isRegeneration) {
            while (GenerationGraphClass.isRegeneration) {}
            return StartFromHouse();
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
        
        if (GenerationGraphClass.isRegeneration) {
            while (GenerationGraphClass.isRegeneration) {}
            return StartFromCommerce();
        }

        (List <int> parent, List <float> dist) graphData = Dijkstra(FieldClass.numInGraph[CommerceGameObject]);

        if (GenerationGraphClass.isRegeneration) {
            while (GenerationGraphClass.isRegeneration) {}
            return StartFromCommerce();
        }

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

        if (GenerationGraphClass.isRegeneration) {
            while (GenerationGraphClass.isRegeneration) {}
            return StartFromCommerce();
        }

        GameObject Parking = BuildsClass.parkings[minDistI];
        
        List <GameObject> objectPathToStart = new List <GameObject> ();
        for (int v = FieldClass.numInGraph[Parking]; v != -1; v = graphData.parent[v]) {
            objectPathToStart.Add(FieldClass.objInGraph[v]);
            if (FieldClass.objInGraph[v] == CommerceGameObject) break;
        }
        
        if (GenerationGraphClass.isRegeneration) {
            while (GenerationGraphClass.isRegeneration) {}
            return StartFromCommerce();
        }

        List <GameObject> objectPathToEnd = new List <GameObject> ();
        for (int v = FieldClass.numInGraph[BuildGameObject]; v != -1; v = graphData.parent[v]) {
            objectPathToEnd.Add(FieldClass.objInGraph[v]);
            if (FieldClass.objInGraph[v] == CommerceGameObject) break;
        }
        objectPathToEnd.Reverse();
        
        if (GenerationGraphClass.isRegeneration) {
            while (GenerationGraphClass.isRegeneration) {}
            return StartFromCommerce();
        }

        graphData = Dijkstra(FieldClass.numInGraph[BuildGameObject]);
        
        if (GenerationGraphClass.isRegeneration) {
            while (GenerationGraphClass.isRegeneration) {}
            return StartFromCommerce();
        }

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
        
        if (GenerationGraphClass.isRegeneration) {
            while (GenerationGraphClass.isRegeneration) {}
            return StartFromCommerce();
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

    public void DeleteObject(int idx) {
        GameObject obj = objects[idx];
        objects.RemoveAt(idx);
        objectClasses.RemoveAt(idx);
        paths.RemoveAt(idx);
        pathsDefault.RemoveAt(idx);
        itForQueue.RemoveAt(idx);
        cntFrameForDelay.RemoveAt(idx);
        for (int i = idx; i < vertexIsActive.Length - 1; ++i) {
            vertexIsActive[i] = vertexIsActive[i + 1];
            onVisibleInCamera[i] = onVisibleInCamera[i + 1];
            vertexTo[i] = vertexTo[i + 1];
            vertexFrom[i] = vertexFrom[i + 1];
            isShiftVector[i] = isShiftVector[i + 1];
            cntWaitingFrames[i] = cntWaitingFrames[i + 1];
            speeds[i] = speeds[i + 1];
            numOfLanes[i] = numOfLanes[i + 1];
        }
        Destroy(obj);
    }

    public void SetLinePath(GameObject obj) {
        visiblePathObj = obj;
        int idx = objects.IndexOf(visiblePathObj);
        if (itForQueue[idx] < paths[idx].pointsPathToStart.Count) {
            linePath.positionCount = paths[idx].pointsPathToStart.Count;
            for (int j = 0; j < paths[idx].pointsPathToStart.Count; ++j) {
                Vector3 pos = paths[idx].pointsPathToStart[j];
                linePath.SetPosition(j, new Vector3(pos.x, 0.5f, pos.z));
            }
        }
        if (itForQueue[idx] == paths[idx].pointsPathToStart.Count && itForQueue[idx] < paths[idx].pointsPathToStart.Count + paths[idx].pointsPathToEnd.Count) {
            linePath.positionCount = paths[idx].pointsPathToEnd.Count;
            for (int j = 0; j < paths[idx].pointsPathToEnd.Count; ++j) {
                Vector3 pos = paths[idx].pointsPathToEnd[j];
                linePath.SetPosition(j, new Vector3(pos.x, 0.5f, pos.z));
            }
        }
        if (itForQueue[idx] == paths[idx].pointsPathToStart.Count + paths[idx].pointsPathToEnd.Count && itForQueue[idx] < paths[idx].pointsPathToStart.Count + paths[idx].pointsPathToEnd.Count + paths[idx].pointsPathToParking.Count) {
            linePath.positionCount = paths[idx].pointsPathToParking.Count;
            for (int j = 0; j < paths[idx].pointsPathToParking.Count; ++j) {
                Vector3 pos = paths[idx].pointsPathToParking[j];
                linePath.SetPosition(j, new Vector3(pos.x, 0.5f, pos.z));
            }
        }
    }
}
