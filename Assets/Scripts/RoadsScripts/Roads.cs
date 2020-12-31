using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Roads : MonoBehaviour {
    private GameObject MainCamera;
    private Field FieldClass;
    private float eps = 1e-5f;
    
    public GameObject[] preFubs;
    public GameObject[] preFubsGhost;
    public List <GameObject> objects;
    public List <GameObject> crossroads;
    public List <GameObject> ghostObjects;
    public bool isFollowGhost = false;
    public int idxOverRoad = -1;
    public string RoadType = "";

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        FieldClass = MainCamera.GetComponent <Field> ();
    }

    private void Start() {
        objects = new List <GameObject> ();
        ghostObjects = new List <GameObject> ();
        crossroads = new List <GameObject> ();
    }

    private void Update() {
        if (!isFollowGhost && RoadType != "" && idxOverRoad != -1) {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (Input.GetMouseButtonDown(0)) {
                    CreateGhost(RoadType, RoundCoordinateOnTheRoad(hit.point, idxOverRoad), idxOverRoad);
                }
            }
        }
        if (Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.KeypadEnter)) {
            CreateObjects();
        }
    }

    private int ToIndex(string name) {
        for (int i = 0; i < preFubs.Length; ++i) {
            if (preFubs[i].name == name || preFubsGhost[i].name == name) {
                return i;
            }
        }
        return -1;
    }

    public float funcAngle(float dist, float leg, float x1, float y1, float x2, float y2) {
        if (dist == 0) return 0;
        else {
            if (y1 <= y2) return (float)(90 - Math.Acos(leg / dist) * 57.3);
            else return (float)(-270 + Math.Acos(leg / dist) * 57.3);
        }
    }

    public Vector3 RoundCoodinate(Vector3 point) {
        int gridSize = 1;
        float x = point.x, z = point.z, low, high;

        low = (int)(x / gridSize) * gridSize;
        high = low + gridSize;
        if (Math.Abs(x - low) < Math.Abs(x - high)) point.x = low;
        else point.x = high;
        
        low = (int)(z / gridSize) * gridSize;
        high = low + gridSize;
        if (Math.Abs(z - low) < Math.Abs(z - high)) point.z = low;
        else point.z = high;

        return point;
    }

    public Vector3 RoundCoordinateOnTheRoad(Vector3 point, int idxRoad) {
        RoadObject data = objects[idxRoad].GetComponent <RoadObject> ();
        float a1 = data.y1 - data.y2, b1 = data.x2 - data.x1, c1 = data.x1 * data.y2 - data.x2 * data.y1; // line
        float a2 = -b1, b2 = a1, c2 = -(a2 * point.x + b2 * point.z); // norm
        if (a1 * b2 - a2 * b1 == 0) return point; // parallel
        float x = -(c1 * b2 - c2 * b1) / (a1 * b2 - a2 * b1);
        float y = -(a1 * c2 - a2 * c1) / (a1 * b2 - a2 * b1);
        Vector3 ans = new Vector3(x, 0, y);
        float dist1 = (float)Math.Sqrt(Math.Pow(x - data.x1, 2) + Math.Pow(y - data.y1, 2));
        float dist2 = (float)Math.Sqrt(Math.Pow(x - data.x2, 2) + Math.Pow(y - data.y2, 2));
        float dist = (float)Math.Sqrt(Math.Pow(data.x2 - data.x1, 2) + Math.Pow(data.y2 - data.y1, 2));
        if (dist1 + dist2 - dist > eps) {
            if (dist1 < dist2) ans = new Vector3(data.x1, 0, data.y1);
            else ans = new Vector3(data.x2, 0, data.y2);
        }
        ans = RoundCoodinate(ans);
        return ans;
    }

    public void CreateGhost(string name, Vector3 point, int idxRoad) {
        ghostObjects.Add(Instantiate(preFubsGhost[ToIndex(name)], point, preFubsGhost[ToIndex(name)].transform.rotation));
        ghostObjects[ghostObjects.Count - 1].AddComponent <RoadGhostObject> ();
        RoadGhostObject data = ghostObjects[ghostObjects.Count - 1].GetComponent <RoadGhostObject> ();
        data.idx = ghostObjects.Count - 1;
        data.idxPreFub = ToIndex(name);
        data.x1 = point.x;
        data.y1 = point.z;
        data.connectedRoad = idxRoad;
        isFollowGhost = true;

        ghostObjects[ghostObjects.Count - 1].AddComponent <Rigidbody> ();
        ghostObjects[ghostObjects.Count - 1].GetComponent <Rigidbody> ().useGravity = false;
    }

    public void DeleteGhost(GameObject ghostObject) {
        ghostObjects.Remove(ghostObject);
        Destroy(ghostObject);
    }

    public void CreateObjects() {
        for (int i = 0; i < ghostObjects.Count; ++i) {
            GameObject ghostObject = ghostObjects[i];
            RoadGhostObject ghostObjectClass = ghostObject.GetComponent <RoadGhostObject> ();
            objects.Add(Instantiate(preFubs[ghostObjectClass.idxPreFub], new Vector3(ghostObject.transform.position.x, 0, ghostObject.transform.position.z),
                        ghostObject.transform.rotation));
            objects[objects.Count - 1].AddComponent <RoadObject> ();
            RoadObject objectClass = objects[objects.Count - 1].GetComponent <RoadObject> ();

            if (FieldClass.objects[(int)ghostObjectClass.x1 + FieldClass.fieldSizeHalf, (int)ghostObjectClass.y1 + FieldClass.fieldSizeHalf] == null) {
                RoadObject objectClassConnectWithGhost = objects[ghostObjectClass.connectedRoad].GetComponent <RoadObject> ();
                float mainRoadA = objectClassConnectWithGhost.y1 - objectClassConnectWithGhost.y2, mainRoadB = objectClassConnectWithGhost.x2 - objectClassConnectWithGhost.x1,
                      mainRoadC = objectClassConnectWithGhost.x1 * objectClassConnectWithGhost.y2 - objectClassConnectWithGhost.x2 * objectClassConnectWithGhost.y1; // main road line
                float ghostRoadA = ghostObjectClass.y1 - ghostObjectClass.y2, ghostRoadB = ghostObjectClass.x2 - ghostObjectClass.x1, ghostRoadC = ghostObjectClass.x1 * ghostObjectClass.y2 - ghostObjectClass.x2 * ghostObjectClass.y1; // ghost road line
                float mainRoadCrossGhostRoadX = -(mainRoadC * ghostRoadB - ghostRoadC * mainRoadB) / (mainRoadA * ghostRoadB - ghostRoadA * mainRoadB); // rounded coordinate
                float mainRoadCrossGhostRoadY = -(mainRoadA * ghostRoadC - ghostRoadA * mainRoadC) / (mainRoadA * ghostRoadB - ghostRoadA * mainRoadB); // rounded coordinate

                FieldClass.objects[(int)ghostObjectClass.x1 + FieldClass.fieldSizeHalf, (int)ghostObjectClass.y1 + FieldClass.fieldSizeHalf] = Instantiate(preFubs[ToIndex("Crossroad1")],
                    new Vector3(mainRoadCrossGhostRoadX, 0.1f, mainRoadCrossGhostRoadY), Quaternion.Euler(0, 0, 0));
                crossroads.Add(FieldClass.objects[(int)ghostObjectClass.x1 + FieldClass.fieldSizeHalf, (int)ghostObjectClass.y1 + FieldClass.fieldSizeHalf]);
                FieldClass.objects[(int)ghostObjectClass.x1 + FieldClass.fieldSizeHalf, (int)ghostObjectClass.y1 + FieldClass.fieldSizeHalf].AddComponent <CrossroadObject> ();
                CrossroadObject crossroadClass = FieldClass.objects[(int)ghostObjectClass.x1 + FieldClass.fieldSizeHalf, (int)ghostObjectClass.y1 + FieldClass.fieldSizeHalf].GetComponent <CrossroadObject> ();

                crossroadClass.x = mainRoadCrossGhostRoadX;
                crossroadClass.y = mainRoadCrossGhostRoadY;
                crossroadClass.xr = (int)ghostObjectClass.x1;
                crossroadClass.yr = (int)ghostObjectClass.y1;
                crossroadClass.idx = crossroads.Count - 1;

                crossroadClass.connectedRoads.Add(objects.Count - 1);
                crossroadClass.connectedRoads.Add(ghostObjectClass.connectedRoad);
                if (objectClass.connectedCrossroads.Find(x => x == crossroadClass.idx) == default(int))
                    objectClass.connectedCrossroads.Add(crossroadClass.idx);
                if (objects[ghostObjectClass.connectedRoad].GetComponent <RoadObject> ().connectedCrossroads.Find(x => x == crossroadClass.idx) == default(int))
                    objects[ghostObjectClass.connectedRoad].GetComponent <RoadObject> ().connectedCrossroads.Add(crossroadClass.idx);
            }
            else {
                CrossroadObject crossroadClass = FieldClass.objects[(int)ghostObjectClass.x1 + FieldClass.fieldSizeHalf, (int)ghostObjectClass.y1 + FieldClass.fieldSizeHalf].GetComponent <CrossroadObject> ();
                crossroadClass.connectedRoads.Add(objects.Count - 1);
                if (objectClass.connectedCrossroads.Find(x => x == crossroadClass.idx) == default(int))
                    objectClass.connectedCrossroads.Add(crossroadClass.idx);
                if (objects[ghostObjectClass.connectedRoad].GetComponent <RoadObject> ().connectedCrossroads.Find(x => x == crossroadClass.idx) == default(int))
                    objects[ghostObjectClass.connectedRoad].GetComponent <RoadObject> ().connectedCrossroads.Add(crossroadClass.idx);
            }

            if (ghostObjectClass.connectedRoad2 != -1) {
                if (FieldClass.objects[(int)ghostObjectClass.x2 + FieldClass.fieldSizeHalf, (int)ghostObjectClass.y2 + FieldClass.fieldSizeHalf] == null) {
                    RoadObject objectClassConnectWithGhost = objects[ghostObjectClass.connectedRoad2].GetComponent <RoadObject> ();
                    float mainRoadA = objectClassConnectWithGhost.y1 - objectClassConnectWithGhost.y2, mainRoadB = objectClassConnectWithGhost.x2 - objectClassConnectWithGhost.x1,
                        mainRoadC = objectClassConnectWithGhost.x1 * objectClassConnectWithGhost.y2 - objectClassConnectWithGhost.x2 * objectClassConnectWithGhost.y1; // main road line
                    float ghostRoadA = ghostObjectClass.y1 - ghostObjectClass.y2, ghostRoadB = ghostObjectClass.x2 - ghostObjectClass.x1, ghostRoadC = ghostObjectClass.x1 * ghostObjectClass.y2 - ghostObjectClass.x2 * ghostObjectClass.y1; // ghost road line
                    float mainRoadCrossGhostRoadX = -(mainRoadC * ghostRoadB - ghostRoadC * mainRoadB) / (mainRoadA * ghostRoadB - ghostRoadA * mainRoadB); // rounded coordinate
                    float mainRoadCrossGhostRoadY = -(mainRoadA * ghostRoadC - ghostRoadA * mainRoadC) / (mainRoadA * ghostRoadB - ghostRoadA * mainRoadB); // rounded coordinate
                
                    FieldClass.objects[(int)ghostObjectClass.x2 + FieldClass.fieldSizeHalf, (int)ghostObjectClass.y2 + FieldClass.fieldSizeHalf] = Instantiate(preFubs[ToIndex("Crossroad1")],
                        new Vector3(mainRoadCrossGhostRoadX, 0.1f, mainRoadCrossGhostRoadY), Quaternion.Euler(0, 0, 0));
                    crossroads.Add(FieldClass.objects[(int)ghostObjectClass.x2 + FieldClass.fieldSizeHalf, (int)ghostObjectClass.y2 + FieldClass.fieldSizeHalf]);
                    FieldClass.objects[(int)ghostObjectClass.x2 + FieldClass.fieldSizeHalf, (int)ghostObjectClass.y2 + FieldClass.fieldSizeHalf].AddComponent <CrossroadObject> ();
                    CrossroadObject crossroadClass = FieldClass.objects[(int)ghostObjectClass.x2 + FieldClass.fieldSizeHalf, (int)ghostObjectClass.y2 + FieldClass.fieldSizeHalf].GetComponent <CrossroadObject> ();

                    crossroadClass.x = mainRoadCrossGhostRoadX;
                    crossroadClass.y = mainRoadCrossGhostRoadY;
                    crossroadClass.xr = (int)ghostObjectClass.x2;
                    crossroadClass.yr = (int)ghostObjectClass.y2;
                    crossroadClass.idx = crossroads.Count - 1;

                    crossroadClass.connectedRoads.Add(objects.Count - 1);
                    crossroadClass.connectedRoads.Add(ghostObjectClass.connectedRoad2);
                    if (objectClass.connectedCrossroads.Find(x => x == crossroadClass.idx) == default(int))
                        objectClass.connectedCrossroads.Add(crossroadClass.idx);
                    if (objects[ghostObjectClass.connectedRoad2].GetComponent <RoadObject> ().connectedCrossroads.Find(x => x == crossroadClass.idx) == default(int))
                        objects[ghostObjectClass.connectedRoad2].GetComponent <RoadObject> ().connectedCrossroads.Add(crossroadClass.idx);
                }
                else {
                    CrossroadObject crossroadClass = FieldClass.objects[(int)ghostObjectClass.x2 + FieldClass.fieldSizeHalf, (int)ghostObjectClass.y2 + FieldClass.fieldSizeHalf].GetComponent <CrossroadObject> ();
                    crossroadClass.connectedRoads.Add(objects.Count - 1);
                    if (objectClass.connectedCrossroads.Find(x => x == crossroadClass.idx) == default(int))
                        objectClass.connectedCrossroads.Add(crossroadClass.idx);
                    if (objects[ghostObjectClass.connectedRoad2].GetComponent <RoadObject> ().connectedCrossroads.Find(x => x == crossroadClass.idx) == default(int))
                        objects[ghostObjectClass.connectedRoad2].GetComponent <RoadObject> ().connectedCrossroads.Add(crossroadClass.idx);
                }
            }

            float len = (float)Math.Sqrt(Math.Pow(ghostObjectClass.x2 - ghostObjectClass.x1, 2) + Math.Pow(ghostObjectClass.y2 - ghostObjectClass.y1, 2));
            objectClass.x1 = ghostObjectClass.x1;
            objectClass.y1 = ghostObjectClass.y1;
            objectClass.x2 = ghostObjectClass.x2;
            objectClass.y2 = ghostObjectClass.y2;
            objectClass.len = ghostObjectClass.len;
            objectClass.angle = ghostObjectClass.angle;
            objectClass.idx = objects.Count - 1;

            objects[objects.Count - 1].transform.localScale = ghostObject.transform.localScale;
            MeshRenderer MeshRendererClass = objects[objects.Count - 1].GetComponent <MeshRenderer> ();
            MeshRendererClass.materials[0].SetTextureScale("_MainTex", new Vector2(ghostObjectClass.len / 2, 1));

            objects[objects.Count - 1].AddComponent <Rigidbody> ();
            objects[objects.Count - 1].GetComponent <Rigidbody> ().useGravity = false;

            DeleteGhost(ghostObject);
        }
    }

    public void CreateObject(string name, Vector3 point1, Vector3 point2, float angle, int connectedRoad = -1) {
        float len = (float)Math.Sqrt(Math.Pow(point2.x - point1.x, 2) + Math.Pow(point2.z - point1.z, 2));

        objects.Add(Instantiate(preFubs[ToIndex(name)], new Vector3((point1.x + point2.x) / 2, 0, (point1.z + point2.z) / 2), Quaternion.Euler(0, funcAngle(len, point2.x - point1.x, point1.x, point1.z, point2.x, point2.z), 0)));
        objects[objects.Count - 1].transform.localScale = new Vector3(1, 1, len / 2);
        MeshRenderer MeshRendererClass = objects[objects.Count - 1].GetComponent <MeshRenderer> ();
        MeshRendererClass.materials[0].SetTextureScale("_MainTex", new Vector2(len / 2, 1));

        objects[objects.Count - 1].AddComponent <RoadObject> ();
        RoadObject objectClass = objects[objects.Count - 1].GetComponent <RoadObject> ();

        if (connectedRoad != -1) {
            if (FieldClass.objects[(int)point1.x + FieldClass.fieldSizeHalf, (int)point1.z + FieldClass.fieldSizeHalf] == null) {
                FieldClass.objects[(int)point1.x + FieldClass.fieldSizeHalf, (int)point1.z + FieldClass.fieldSizeHalf] = Instantiate(preFubs[ToIndex("Crossroad1")],
                    new Vector3(point1.x, 0.1f, point1.z), Quaternion.Euler(0, 0, 0));
                crossroads.Add(FieldClass.objects[(int)point1.x + FieldClass.fieldSizeHalf, (int)point1.z + FieldClass.fieldSizeHalf]);
                FieldClass.objects[(int)point1.x + FieldClass.fieldSizeHalf, (int)point1.z + FieldClass.fieldSizeHalf].AddComponent <CrossroadObject> ();
                CrossroadObject crossroadClass = FieldClass.objects[(int)point1.x + FieldClass.fieldSizeHalf, (int)point1.z + FieldClass.fieldSizeHalf].GetComponent <CrossroadObject> ();

                crossroadClass.x = point1.x;
                crossroadClass.y = point1.z;
                crossroadClass.xr = (int)point1.x;
                crossroadClass.yr = (int)point1.y;
                crossroadClass.idx = crossroads.Count - 1;

                crossroadClass.connectedRoads.Add(objects.Count - 1);
                crossroadClass.connectedRoads.Add(connectedRoad);
                if (objectClass.connectedCrossroads.Find(x => x == crossroadClass.idx) == default(int))
                    objectClass.connectedCrossroads.Add(crossroadClass.idx);
                if (objects[connectedRoad].GetComponent <RoadObject> ().connectedCrossroads.Find(x => x == crossroadClass.idx) == default(int))
                    objects[connectedRoad].GetComponent <RoadObject> ().connectedCrossroads.Add(crossroadClass.idx);
            }
            else {
                CrossroadObject crossroadClass = FieldClass.objects[(int)point1.x + FieldClass.fieldSizeHalf, (int)point1.z + FieldClass.fieldSizeHalf].GetComponent <CrossroadObject> ();
                crossroadClass.connectedRoads.Add(objects.Count - 1);
                if (objectClass.connectedCrossroads.Find(x => x == crossroadClass.idx) == default(int))
                    objectClass.connectedCrossroads.Add(crossroadClass.idx);
                if (objects[connectedRoad].GetComponent <RoadObject> ().connectedCrossroads.Find(x => x == crossroadClass.idx) == default(int))
                    objects[connectedRoad].GetComponent <RoadObject> ().connectedCrossroads.Add(crossroadClass.idx);
            }
        }

        objectClass.x1 = point1.x;
        objectClass.y1 = point1.z;
        objectClass.x2 = point2.x;
        objectClass.y2 = point2.z;
        objectClass.len = len;
        objectClass.angle = angle;
        objectClass.idx = objects.Count - 1;

        objects[objects.Count - 1].AddComponent <Rigidbody> ();
        objects[objects.Count - 1].GetComponent <Rigidbody> ().useGravity = false;

        objectClass.inProgress = false;
    }
}
