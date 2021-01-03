using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GenerationParkings : MonoBehaviour {
    private struct Block {
        public int minI;
        public List <long> priority;
        public List <Vector3> point;
        public List <float> rotate;
        public List <int> roadIdx;

        public Block(int minI) {
            this.minI = minI;
            this.priority = new List <long> ();
            this.point = new List <Vector3> ();
            this.rotate = new List <float> ();
            this.roadIdx = new List <int> ();
        }
    }

    private GameObject MainCamera;
    private Generation GenerationClass;
    private Roads RoadsClass;
    private Builds BuildsClass;
    private Field FieldClass;
    private List <Block> SqrtDecomp;
    private int[] cntInDistrict = {0, 0, 0, 0};
    private int sqrtSize = 20;
    private ulong seed;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        RoadsClass = MainCamera.GetComponent <Roads> ();
        BuildsClass = MainCamera.GetComponent <Builds> ();
        FieldClass = MainCamera.GetComponent <Field> ();
        GenerationClass = MainCamera.GetComponent <Generation> ();
        SqrtDecomp = new List <Block> ();
    }

    private void BuildBlock(long priority, Vector3 point, float rotate, int roadIdx) {
        Block newBlock = new Block(0);
        newBlock.minI = 0;
        newBlock.priority.Add(priority);
        newBlock.point.Add(point);
        newBlock.rotate.Add(rotate);
        newBlock.roadIdx.Add(roadIdx);
        SqrtDecomp.Add(newBlock);
    }

    private void AddBlock(long priority, Vector3 point, float rotate, int roadIdx) {
        if (SqrtDecomp.Count == 0) BuildBlock(priority, point, rotate, roadIdx);
        else {
            Block tmpB = SqrtDecomp[SqrtDecomp.Count - 1];
            if (tmpB.priority.Count >= sqrtSize) BuildBlock(priority, point, rotate, roadIdx);
            else {
                if (priority < tmpB.priority[tmpB.minI])
                    tmpB.minI = tmpB.priority.Count;
                tmpB.priority.Add(priority);
                tmpB.point.Add(point);
                tmpB.rotate.Add(rotate);
                tmpB.roadIdx.Add(roadIdx);
                SqrtDecomp[SqrtDecomp.Count - 1] = tmpB;
            }
        }
    }

    private (Vector3 point, float rotate, int roadIdx) GetMinBlock() {
        int SqrtDecompMinI = (int)1e9;
        for (int it = 0; it < SqrtDecomp.Count; ++it) {
            if (SqrtDecompMinI >= SqrtDecomp.Count || SqrtDecomp[it].priority[SqrtDecomp[it].minI] < SqrtDecomp[SqrtDecompMinI].priority[SqrtDecomp[SqrtDecompMinI].minI])
                SqrtDecompMinI = it;
        }
        Block ansBlock = SqrtDecomp[SqrtDecompMinI];
        (Vector3, float, int) ans = (ansBlock.point[ansBlock.minI], ansBlock.rotate[ansBlock.minI], ansBlock.roadIdx[ansBlock.minI]);
        ansBlock.priority.RemoveAt(ansBlock.minI);
        ansBlock.point.RemoveAt(ansBlock.minI);
        ansBlock.rotate.RemoveAt(ansBlock.minI);
        ansBlock.roadIdx.RemoveAt(ansBlock.minI);
        ansBlock.minI = (int)1e9;
        for (int i = 0; i < ansBlock.priority.Count; ++i) {
            if (ansBlock.minI >= ansBlock.priority.Count || ansBlock.priority[i] < ansBlock.priority[ansBlock.minI])
                ansBlock.minI = i;
        }
        if (ansBlock.priority.Count == 0) SqrtDecomp.RemoveAt(SqrtDecompMinI);
        else SqrtDecomp[SqrtDecompMinI] = ansBlock;
        return ans;
    }

    private bool CheckCross(Vector3 Segment1Point1, Vector3 Segment1Point2, Vector3 Segment2Point1, Vector3 Segment2Point2, float eps = 0) {
        float mainRoadA = Segment1Point1.z - Segment1Point2.z, mainRoadB = Segment1Point2.x - Segment1Point1.x,
              mainRoadC = Segment1Point1.x * Segment1Point2.z - Segment1Point2.x * Segment1Point1.z; // main road line
        float ghostRoadA = Segment2Point1.z - Segment2Point2.z, ghostRoadB = Segment2Point2.x - Segment2Point1.x, ghostRoadC = Segment2Point1.x * Segment2Point2.z - Segment2Point2.x * Segment2Point1.z; // ghost road line
        if (mainRoadA * ghostRoadB - ghostRoadA * mainRoadB == 0) return false;
        float mainRoadCrossGhostRoadX = -(mainRoadC * ghostRoadB - ghostRoadC * mainRoadB) / (mainRoadA * ghostRoadB - ghostRoadA * mainRoadB); // rounded coordinate
        float mainRoadCrossGhostRoadY = -(mainRoadA * ghostRoadC - ghostRoadA * mainRoadC) / (mainRoadA * ghostRoadB - ghostRoadA * mainRoadB); // rounded coordinate
        if (mainRoadCrossGhostRoadX <= Math.Max(Segment1Point1.x, Segment1Point2.x) + eps && mainRoadCrossGhostRoadX >= Math.Min(Segment1Point1.x, Segment1Point2.x) - eps &&
            mainRoadCrossGhostRoadX <= Math.Max(Segment2Point1.x, Segment2Point2.x) + eps && mainRoadCrossGhostRoadX >= Math.Min(Segment2Point1.x, Segment2Point2.x) - eps &&
            mainRoadCrossGhostRoadY <= Math.Max(Segment1Point1.z, Segment1Point2.z) + eps && mainRoadCrossGhostRoadY >= Math.Min(Segment1Point1.z, Segment1Point2.z) - eps &&
            mainRoadCrossGhostRoadY <= Math.Max(Segment2Point1.z, Segment2Point2.z) + eps && mainRoadCrossGhostRoadY >= Math.Min(Segment2Point1.z, Segment2Point2.z) - eps)
            return true;
        return false;
    }

    private bool CheckCollisionBuilds(Vector3 tmpVertex1, Vector3 tmpVertex2, Vector3 tmpVertex3, Vector3 tmpVertex4, Vector3 vertex1, Vector3 vertex2, Vector3 vertex3, Vector3 vertex4, float dist) {
        return (CheckCross(tmpVertex1, tmpVertex2, vertex1, vertex2, dist) ||
        CheckCross(tmpVertex1, tmpVertex2, vertex1, vertex3, dist) ||
        CheckCross(tmpVertex1, tmpVertex2, vertex1, vertex4, dist) ||
        CheckCross(tmpVertex1, tmpVertex2, vertex2, vertex3, dist) ||
        CheckCross(tmpVertex1, tmpVertex2, vertex2, vertex4, dist) ||
        CheckCross(tmpVertex1, tmpVertex2, vertex3, vertex4, dist) ||
        
        CheckCross(tmpVertex1, tmpVertex3, vertex1, vertex2, dist) ||
        CheckCross(tmpVertex1, tmpVertex3, vertex1, vertex3, dist) ||
        CheckCross(tmpVertex1, tmpVertex3, vertex1, vertex4, dist) ||
        CheckCross(tmpVertex1, tmpVertex3, vertex2, vertex3, dist) ||
        CheckCross(tmpVertex1, tmpVertex3, vertex2, vertex4, dist) ||
        CheckCross(tmpVertex1, tmpVertex3, vertex3, vertex4, dist) ||
        
        CheckCross(tmpVertex1, tmpVertex4, vertex1, vertex2, dist) ||
        CheckCross(tmpVertex1, tmpVertex4, vertex1, vertex3, dist) ||
        CheckCross(tmpVertex1, tmpVertex4, vertex1, vertex4, dist) ||
        CheckCross(tmpVertex1, tmpVertex4, vertex2, vertex3, dist) ||
        CheckCross(tmpVertex1, tmpVertex4, vertex2, vertex4, dist) ||
        CheckCross(tmpVertex1, tmpVertex4, vertex3, vertex4, dist) ||
        
        CheckCross(tmpVertex2, tmpVertex3, vertex1, vertex2, dist) ||
        CheckCross(tmpVertex2, tmpVertex3, vertex1, vertex3, dist) ||
        CheckCross(tmpVertex2, tmpVertex3, vertex1, vertex4, dist) ||
        CheckCross(tmpVertex2, tmpVertex3, vertex2, vertex3, dist) ||
        CheckCross(tmpVertex2, tmpVertex3, vertex2, vertex4, dist) ||
        CheckCross(tmpVertex2, tmpVertex3, vertex3, vertex4, dist) ||
        
        CheckCross(tmpVertex2, tmpVertex4, vertex1, vertex2, dist) ||
        CheckCross(tmpVertex2, tmpVertex4, vertex1, vertex3, dist) ||
        CheckCross(tmpVertex2, tmpVertex4, vertex1, vertex4, dist) ||
        CheckCross(tmpVertex2, tmpVertex4, vertex2, vertex3, dist) ||
        CheckCross(tmpVertex2, tmpVertex4, vertex2, vertex4, dist) ||
        CheckCross(tmpVertex2, tmpVertex4, vertex3, vertex4, dist) ||
        
        CheckCross(tmpVertex3, tmpVertex4, vertex1, vertex2, dist) ||
        CheckCross(tmpVertex3, tmpVertex4, vertex1, vertex3, dist) ||
        CheckCross(tmpVertex3, tmpVertex4, vertex1, vertex4, dist) ||
        CheckCross(tmpVertex3, tmpVertex4, vertex2, vertex3, dist) ||
        CheckCross(tmpVertex3, tmpVertex4, vertex2, vertex4, dist) ||
        CheckCross(tmpVertex3, tmpVertex4, vertex3, vertex4, dist));
    }

    public ulong StartGeneration(ulong newSeed) {
        seed = newSeed;
        for (int i = 0; i < RoadsClass.objects.Count; ++i) {
            RoadObject roadObjectClass = RoadsClass.objects[i].GetComponent <RoadObject> ();
            float mainRoadA = roadObjectClass.y1 - roadObjectClass.y2, mainRoadB = roadObjectClass.x2 - roadObjectClass.x1,
                  mainRoadC = roadObjectClass.x1 * roadObjectClass.y2 - roadObjectClass.x2 * roadObjectClass.y1; // main road line
            float len = (float)Math.Sqrt(Math.Pow(roadObjectClass.x1 - roadObjectClass.x2, 2) + Math.Pow(roadObjectClass.y1 - roadObjectClass.y2, 2));
            float angleHouse = roadObjectClass.angle;
            for (int lenIt = 1; lenIt < len; ++lenIt) {
                float posOnRoadX, posOnRoadY;
                float posOnRoadXv1 = roadObjectClass.x1 + (float)Math.Cos(Math.Atan(mainRoadA / -mainRoadB)) * lenIt;
                float posOnRoadYv1 = roadObjectClass.y1 + (float)Math.Sin(Math.Atan(mainRoadA / -mainRoadB)) * lenIt;
                float posOnRoadXv2 = roadObjectClass.x1 - (float)Math.Cos(Math.Atan(mainRoadA / -mainRoadB)) * lenIt;
                float posOnRoadYv2 = roadObjectClass.y1 - (float)Math.Sin(Math.Atan(mainRoadA / -mainRoadB)) * lenIt;
                float distV1ToPoint2 = (float)Math.Sqrt(Math.Pow(posOnRoadXv1 - roadObjectClass.x2, 2) + Math.Pow(posOnRoadYv1 - roadObjectClass.y2, 2));
                float distV2ToPoint2 = (float)Math.Sqrt(Math.Pow(posOnRoadXv2 - roadObjectClass.x2, 2) + Math.Pow(posOnRoadYv2 - roadObjectClass.y2, 2));
                if (distV1ToPoint2 < distV2ToPoint2) {
                    posOnRoadX = posOnRoadXv1;
                    posOnRoadY = posOnRoadYv1;
                }
                else {
                    posOnRoadX = posOnRoadXv2;
                    posOnRoadY = posOnRoadYv2;
                }

                float normA = -mainRoadB, normB = mainRoadA, normC = -(normA * posOnRoadX + normB * posOnRoadY); // norm

                float widthHouse1 = (int)BuildsClass.preFubs[BuildsClass.idxParking].GetComponent <BoxCollider> ().size.x * 0.1f;
                float lenHouse1 = (int)BuildsClass.preFubs[BuildsClass.idxParking].GetComponent <BoxCollider> ().size.z * 0.1f;
                float dx1 = (float)Math.Cos(Math.Atan(normA / -normB)) * (lenHouse1 / 2 + 2);
                float dy1 = (float)Math.Sin(Math.Atan(normA / -normB)) * (lenHouse1 / 2 + 2);
                Vector3 point1 = RoadsClass.RoundCoodinate(new Vector3(posOnRoadX + dx1, 0, posOnRoadY + dy1));

                AddBlock((int)(seed % 1e9), point1, angleHouse, i);
                seed = GenerationClass.funcSeed(seed);

                float widthHouse2 = (int)BuildsClass.preFubs[BuildsClass.idxParking].GetComponent <BoxCollider> ().size.x * 0.1f;
                float lenHouse2 = (int)BuildsClass.preFubs[BuildsClass.idxParking].GetComponent <BoxCollider> ().size.z * 0.1f;
                float dx2 = (float)Math.Cos(Math.Atan(normA / -normB)) * (lenHouse2 / 2 + 2);
                float dy2 = (float)Math.Sin(Math.Atan(normA / -normB)) * (lenHouse2 / 2 + 2);
                Vector3 point2 = RoadsClass.RoundCoodinate(new Vector3(posOnRoadX - dx2, 0, posOnRoadY - dy2));

                AddBlock((int)(seed % 1e9), point2, angleHouse, i);
                seed = GenerationClass.funcSeed(seed);
            }
        }

        while (BuildsClass.parkings.Count < GenerationClass.averageCntParkingInDistrict * 4) {
            if (SqrtDecomp.Count == 0) return seed;
            (Vector3 point, float rotate, int roadIdx) minBlock = GetMinBlock();

            int sum = cntInDistrict[0] + cntInDistrict[1] + cntInDistrict[2] + cntInDistrict[3];
            if (!(SqrtDecomp.Count == 1 && SqrtDecomp[0].priority.Count <= 4 * GenerationClass.averageCntParkingInDistrict - sum) &&
                cntInDistrict[FieldClass.districts[(int)minBlock.point.x + FieldClass.fieldSizeHalf, (int)minBlock.point.z + FieldClass.fieldSizeHalf]] >=
                GenerationClass.averageCntParkingInDistrict) continue;
            
            if (FieldClass.objects[(int)minBlock.point.x + FieldClass.fieldSizeHalf, (int)minBlock.point.z + FieldClass.fieldSizeHalf] == null) {
                float angle = (float)(minBlock.rotate / 57.3);
                float widthHouse = (int)BuildsClass.preFubs[BuildsClass.idxParking].GetComponent <BoxCollider> ().size.x * 0.1f;
                float lenHouse = (int)BuildsClass.preFubs[BuildsClass.idxParking].GetComponent <BoxCollider> ().size.z * 0.1f;
                Vector3 side1 = new Vector3(minBlock.point.x + (float)Math.Cos(angle) * (widthHouse / 2), 0, minBlock.point.z + (float)Math.Sin(angle) * (widthHouse / 2));
                Vector3 side2 = new Vector3(minBlock.point.x - (float)Math.Cos(angle) * (widthHouse / 2), 0, minBlock.point.z - (float)Math.Sin(angle) * (widthHouse / 2));

                Vector3 vertex1 = new Vector3(side1.x + (float)Math.Cos(angle + Math.PI / 2) * (lenHouse / 2), 0, side1.z + (float)Math.Sin(angle + Math.PI / 2) * (lenHouse / 2));
                Vector3 vertex2 = new Vector3(side1.x - (float)Math.Cos(angle + Math.PI / 2) * (lenHouse / 2), 0, side1.z - (float)Math.Sin(angle + Math.PI / 2) * (lenHouse / 2));
                Vector3 vertex3 = new Vector3(side2.x + (float)Math.Cos(angle + Math.PI / 2) * (lenHouse / 2), 0, side2.z + (float)Math.Sin(angle + Math.PI / 2) * (lenHouse / 2));
                Vector3 vertex4 = new Vector3(side2.x - (float)Math.Cos(angle + Math.PI / 2) * (lenHouse / 2), 0, side2.z - (float)Math.Sin(angle + Math.PI / 2) * (lenHouse / 2));

                bool collision = false;
                for (int j = 0; j < RoadsClass.objects.Count; ++j) {
                    RoadObject tmpRoadObject = RoadsClass.objects[j].GetComponent <RoadObject> ();
                    if (CheckCross(new Vector3(tmpRoadObject.x1, 0, tmpRoadObject.y1), new Vector3(tmpRoadObject.x2, 0, tmpRoadObject.y2), vertex1, vertex2, 1) ||
                        CheckCross(new Vector3(tmpRoadObject.x1, 0, tmpRoadObject.y1), new Vector3(tmpRoadObject.x2, 0, tmpRoadObject.y2), vertex1, vertex3, 1) ||
                        CheckCross(new Vector3(tmpRoadObject.x1, 0, tmpRoadObject.y1), new Vector3(tmpRoadObject.x2, 0, tmpRoadObject.y2), vertex1, vertex4, 1) ||
                        CheckCross(new Vector3(tmpRoadObject.x1, 0, tmpRoadObject.y1), new Vector3(tmpRoadObject.x2, 0, tmpRoadObject.y2), vertex2, vertex3, 1) ||
                        CheckCross(new Vector3(tmpRoadObject.x1, 0, tmpRoadObject.y1), new Vector3(tmpRoadObject.x2, 0, tmpRoadObject.y2), vertex2, vertex4, 1) ||
                        CheckCross(new Vector3(tmpRoadObject.x1, 0, tmpRoadObject.y1), new Vector3(tmpRoadObject.x2, 0, tmpRoadObject.y2), vertex3, vertex4, 1)) {
                        collision = true;
                        break;
                    }
                }

                for (int itX = (int)(minBlock.point.x - Math.Max(lenHouse, widthHouse)); itX <= minBlock.point.x + Math.Max(lenHouse, widthHouse); ++itX) {
                    for (int itY = (int)(minBlock.point.z - Math.Max(lenHouse, widthHouse)); itY <= minBlock.point.z + Math.Max(lenHouse, widthHouse); ++itY) {
                        if (itX >= FieldClass.fieldSizeHalf || itY >= FieldClass.fieldSizeHalf || itX <= -FieldClass.fieldSizeHalf || itY <= -FieldClass.fieldSizeHalf ||
                            FieldClass.objects[itX + FieldClass.fieldSizeHalf, itY + FieldClass.fieldSizeHalf] == null ||
                            !(FieldClass.objects[itX + FieldClass.fieldSizeHalf, itY + FieldClass.fieldSizeHalf].GetComponent <BuildObject> ())) continue;
                        BuildObject tmpBuildObject = FieldClass.objects[itX + FieldClass.fieldSizeHalf, itY + FieldClass.fieldSizeHalf].GetComponent <BuildObject> ();

                        float tmpAngle = (float)(RoadsClass.objects[tmpBuildObject.connectedRoad].GetComponent <RoadObject> ().angle / 57.3);
                        float tmpWidthHouse = (int)tmpBuildObject.GetComponent <BoxCollider> ().size.x * 0.1f;
                        float tmpLenHouse = (int)tmpBuildObject.GetComponent <BoxCollider> ().size.z * 0.1f;

                        Vector3 tmpSide1 = new Vector3(tmpBuildObject.x + (float)Math.Cos(tmpAngle) * (tmpWidthHouse / 2), 0, tmpBuildObject.y + (float)Math.Sin(tmpAngle) * (tmpWidthHouse / 2));
                        Vector3 tmpSide2 = new Vector3(tmpBuildObject.x - (float)Math.Cos(tmpAngle) * (tmpWidthHouse / 2), 0, tmpBuildObject.y - (float)Math.Sin(tmpAngle) * (tmpWidthHouse / 2));

                        Vector3 tmpVertex1 = new Vector3(tmpSide1.x + (float)Math.Cos(tmpAngle + Math.PI / 2) * (tmpLenHouse / 2), 0, tmpSide1.z + (float)Math.Sin(tmpAngle + Math.PI / 2) * (tmpLenHouse / 2));
                        Vector3 tmpVertex2 = new Vector3(tmpSide1.x - (float)Math.Cos(tmpAngle + Math.PI / 2) * (tmpLenHouse / 2), 0, tmpSide1.z - (float)Math.Sin(tmpAngle + Math.PI / 2) * (tmpLenHouse / 2));
                        Vector3 tmpVertex3 = new Vector3(tmpSide2.x + (float)Math.Cos(tmpAngle + Math.PI / 2) * (tmpLenHouse / 2), 0, tmpSide2.z + (float)Math.Sin(tmpAngle + Math.PI / 2) * (tmpLenHouse / 2));
                        Vector3 tmpVertex4 = new Vector3(tmpSide2.x - (float)Math.Cos(tmpAngle + Math.PI / 2) * (tmpLenHouse / 2), 0, tmpSide2.z - (float)Math.Sin(tmpAngle + Math.PI / 2) * (tmpLenHouse / 2));

                        if (CheckCollisionBuilds(tmpVertex1, tmpVertex2, tmpVertex3, tmpVertex4, vertex1, vertex2, vertex3, vertex4, 1)) {
                            collision = true;
                            break;
                        }
                    }
                }

                if (!collision) {
                    BuildsClass.CreateObject(minBlock.point, minBlock.rotate * -1, BuildsClass.idxParking, minBlock.roadIdx);
                    ++cntInDistrict[FieldClass.districts[(int)minBlock.point.x + FieldClass.fieldSizeHalf, (int)minBlock.point.z + FieldClass.fieldSizeHalf]];
                    FieldClass.objects[(int)minBlock.point.x + FieldClass.fieldSizeHalf, (int)minBlock.point.z + FieldClass.fieldSizeHalf] = BuildsClass.parkings[BuildsClass.parkings.Count - 1];
                }
            }
        }
        return seed;
    }
}
