using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GenerationRoads : MonoBehaviour {
    private struct Block {
        public int minI;
        public List <long> priority;
        public List <Vector3> point;
        public List <int> roadIdx;

        public Block(int minI) {
            this.minI = minI;
            this.priority = new List <long> ();
            this.point = new List <Vector3> ();
            this.roadIdx = new List <int> ();
        }
    }

    private GameObject MainCamera;
    private Generation GenerationClass;
    private Roads RoadsClass;
    private Field FieldClass;
    private List <Block> SqrtDecomp;
    private int sqrtSize = 20;
    private ulong seed;
    private float maxX = 0, minX = 0;
    private float maxY = 0, minY = 0;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        RoadsClass = MainCamera.GetComponent <Roads> ();
        FieldClass = MainCamera.GetComponent <Field> ();
        GenerationClass = MainCamera.GetComponent <Generation> ();
        SqrtDecomp = new List <Block> ();
    }

    private void BuildBlock(long priority, Vector3 point, int roadIdx) {
        Block newBlock = new Block(0);
        newBlock.minI = 0;
        newBlock.priority.Add(priority);
        newBlock.point.Add(point);
        newBlock.roadIdx.Add(roadIdx);
        SqrtDecomp.Add(newBlock);
    }

    private void AddBlock(long priority, Vector3 point, int roadIdx) {
        if (SqrtDecomp.Count == 0) BuildBlock(priority, point, roadIdx);
        else {
            Block tmpB = SqrtDecomp[SqrtDecomp.Count - 1];
            if (tmpB.priority.Count >= sqrtSize) BuildBlock(priority, point, roadIdx);
            else {
                if (priority < tmpB.priority[tmpB.minI])
                    tmpB.minI = tmpB.priority.Count;
                tmpB.priority.Add(priority);
                tmpB.point.Add(point);
                tmpB.roadIdx.Add(roadIdx);
                SqrtDecomp[SqrtDecomp.Count - 1] = tmpB;
            }
        }
    }

    private (Vector3 point, int roadIdx) GetMinBlock() {
        int SqrtDecompMinI = (int)1e9;
        for (int it = 0; it < SqrtDecomp.Count; ++it) {
            if (SqrtDecompMinI >= SqrtDecomp.Count || SqrtDecomp[it].priority[SqrtDecomp[it].minI] < SqrtDecomp[SqrtDecompMinI].priority[SqrtDecomp[SqrtDecompMinI].minI])
                SqrtDecompMinI = it;
        }
        Block ansBlock = SqrtDecomp[SqrtDecompMinI];
        (Vector3, int) ans = (ansBlock.point[ansBlock.minI], ansBlock.roadIdx[ansBlock.minI]);
        if (ansBlock.priority[ansBlock.minI] > (long)5e8) ansBlock.priority[ansBlock.minI] = Math.Min((int)1e9, (int)(ansBlock.priority[ansBlock.minI] + 1e8));
        else ansBlock.priority[ansBlock.minI] = Math.Min((int)1e9, (int)(ansBlock.priority[ansBlock.minI] + 5e8));
        ansBlock.minI = (int)1e9;
        for (int i = 0; i < ansBlock.priority.Count; ++i) {
            if (ansBlock.minI >= ansBlock.priority.Count || ansBlock.priority[i] < ansBlock.priority[ansBlock.minI])
                ansBlock.minI = i;
        }
        SqrtDecomp[SqrtDecompMinI] = ansBlock;
        return ans;
    }

    private bool CheckCollision(Vector3 point1, Vector3 point2, float angle, int connectedRoad) {
        if (point1.x <= -FieldClass.fieldSizeHalf || point1.x >= FieldClass.fieldSizeHalf ||
            point1.z <= -FieldClass.fieldSizeHalf || point1.z >= FieldClass.fieldSizeHalf ||
            point2.x <= -FieldClass.fieldSizeHalf || point2.x >= FieldClass.fieldSizeHalf ||
            point2.z <= -FieldClass.fieldSizeHalf || point2.z >= FieldClass.fieldSizeHalf) return false;
        if (FieldClass.objects[(int)point1.x + FieldClass.fieldSizeHalf, (int)point1.z + FieldClass.fieldSizeHalf] == null) {
            for (int i = 0; i < RoadsClass.objects.Count; ++i) {
                if (connectedRoad != i) {
                    RoadObject data = RoadsClass.objects[i].GetComponent <RoadObject> ();
                    float mainRoadA = data.y1 - data.y2, mainRoadB = data.x2 - data.x1, mainRoadC = data.x1 * data.y2 - data.x2 * data.y1; // main road line
                    float ghostRoadA = point1.z - point2.z, ghostRoadB = point2.x - point1.x, ghostRoadC = point1.x * point2.z - point2.x * point1.z; // ghost road line
                    if (mainRoadA * ghostRoadB - ghostRoadA * mainRoadB == 0) continue;
                    float mainRoadCrossGhostRoadX = -(mainRoadC * ghostRoadB - ghostRoadC * mainRoadB) / (mainRoadA * ghostRoadB - ghostRoadA * mainRoadB); // rounded coordinate
                    float mainRoadCrossGhostRoadY = -(mainRoadA * ghostRoadC - ghostRoadA * mainRoadC) / (mainRoadA * ghostRoadB - ghostRoadA * mainRoadB); // rounded coordinate
                    if (mainRoadCrossGhostRoadX <= Math.Max(data.x1, data.x2) + 5 && mainRoadCrossGhostRoadX >= Math.Min(data.x1, data.x2) - 5 &&
                        mainRoadCrossGhostRoadX <= Math.Max(point1.x, point2.x) + 5 && mainRoadCrossGhostRoadX >= Math.Min(point1.x, point2.x) - 5 &&
                        mainRoadCrossGhostRoadY <= Math.Max(data.y1, data.y2) + 5 && mainRoadCrossGhostRoadY >= Math.Min(data.y1, data.y2) - 5 &&
                        mainRoadCrossGhostRoadY <= Math.Max(point1.z, point2.z) + 5 && mainRoadCrossGhostRoadY >= Math.Min(point1.z, point2.z) - 5)
                        return false;
                }
            }
        }
        else {
            CrossroadObject crossroad = FieldClass.objects[(int)point1.x + FieldClass.fieldSizeHalf, (int)point1.z + FieldClass.fieldSizeHalf].GetComponent <CrossroadObject> ();
            for (int i = 0; i < crossroad.connectedRoads.Count; ++i) {
                RoadObject tmpRoad = RoadsClass.objects[crossroad.connectedRoads[i]].GetComponent <RoadObject> ();
                if (Math.Abs(angle - tmpRoad.angle) < 30) return false;
            }
            for (int i = 0; i < RoadsClass.objects.Count; ++i) {
                bool p = true;
                for (int j = 0; j < crossroad.connectedRoads.Count; ++j) {
                    if (crossroad.connectedRoads[j] == i) {
                        p = false;
                        break;
                    }
                }
                if (p) {
                    RoadObject data = RoadsClass.objects[i].GetComponent <RoadObject> ();
                    float mainRoadA = data.y1 - data.y2, mainRoadB = data.x2 - data.x1, mainRoadC = data.x1 * data.y2 - data.x2 * data.y1; // main road line
                    float ghostRoadA = point1.z - point2.z, ghostRoadB = point2.x - point1.x, ghostRoadC = point1.x * point2.z - point2.x * point1.z; // ghost road line
                    if (mainRoadA * ghostRoadB - ghostRoadA * mainRoadB == 0) continue;
                    float mainRoadCrossGhostRoadX = -(mainRoadC * ghostRoadB - ghostRoadC * mainRoadB) / (mainRoadA * ghostRoadB - ghostRoadA * mainRoadB); // rounded coordinate
                    float mainRoadCrossGhostRoadY = -(mainRoadA * ghostRoadC - ghostRoadA * mainRoadC) / (mainRoadA * ghostRoadB - ghostRoadA * mainRoadB); // rounded coordinate
                    if (mainRoadCrossGhostRoadX <= Math.Max(data.x1, data.x2) + 5 && mainRoadCrossGhostRoadX >= Math.Min(data.x1, data.x2) - 5 &&
                        mainRoadCrossGhostRoadX <= Math.Max(point1.x, point2.x) + 5 && mainRoadCrossGhostRoadX >= Math.Min(point1.x, point2.x) - 5 &&
                        mainRoadCrossGhostRoadY <= Math.Max(data.y1, data.y2) + 5 && mainRoadCrossGhostRoadY >= Math.Min(data.y1, data.y2) - 5 &&
                        mainRoadCrossGhostRoadY <= Math.Max(point1.z, point2.z) + 5 && mainRoadCrossGhostRoadY >= Math.Min(point1.z, point2.z) - 5)
                        return false;
                }
            }
        }
        return true;
    }

    public ulong StartGeneration(ulong newSeed) {
        seed = newSeed;
        RoadsClass.CreateObject("Road1", new Vector3(0, 0, 100), new Vector3 (0, 0, 110), 90);
        AddBlock((int)(seed % 1e9), new Vector3 (0, 0, 110), RoadsClass.objects.Count - 1);
        seed = GenerationClass.funcSeed(seed);
        DateTimeOffset startDate = DateTimeOffset.Now;
        DateTimeOffset endDate = startDate.AddSeconds(GenerationClass.timeRoadsBuildGeneration);
        while (GenerationClass.CheckTime(endDate) && RoadsClass.objects.Count < GenerationClass.maxCntRoads) {
            (Vector3 point, int roadIdx) startPoint = GetMinBlock();

            RoadObject RoadObjectClass = RoadsClass.objects[startPoint.roadIdx].GetComponent <RoadObject> ();
            float startAngle = RoadObjectClass.angle;
            float angle = startAngle + (-135 + (int)(seed % 270));
            float len = GenerationClass.minLenRoads + (float)(seed % (ulong)GenerationClass.deltaLenRoads);
            float x = (float)Math.Cos(angle / 57.3) * len;
            float y = (float)Math.Sin(angle / 57.3) * len;

            Vector3 endPoint = RoadsClass.RoundCoodinate(new Vector3(startPoint.point.x + x, 0, startPoint.point.z + y));

            if (CheckCollision(startPoint.point, endPoint, angle, startPoint.roadIdx)) {
                maxX = (float)Math.Max(maxX, Math.Max(startPoint.point.x, endPoint.x));
                maxY = (float)Math.Max(maxY, Math.Max(startPoint.point.z, endPoint.z));
                minX = (float)Math.Min(minX, Math.Min(startPoint.point.x, endPoint.x));
                minY = (float)Math.Min(minY, Math.Min(startPoint.point.z, endPoint.z));
                FieldClass.centerX = (int)((maxX + minX) / 2);
                FieldClass.centerY = (int)((maxY + minY) / 2);
                RoadsClass.CreateObject("Road1", startPoint.point, endPoint, angle, startPoint.roadIdx);
                AddBlock((int)(seed % 1e9), endPoint, RoadsClass.objects.Count - 1);
                seed = GenerationClass.funcSeed(seed);
            }
        }
        return seed;
    }
}
