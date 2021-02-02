using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GenerationHouses : MonoBehaviour {
    private GameObject MainCamera;
    private Generation GenerationClass;
    private Roads RoadsClass;
    private Builds BuildsClass;
    private Field FieldClass;

    public bool isOver = false;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        RoadsClass = MainCamera.GetComponent<Roads>();
        BuildsClass = MainCamera.GetComponent<Builds>();
        FieldClass = MainCamera.GetComponent<Field>();
        GenerationClass = MainCamera.GetComponent<Generation>();
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

    private (int typeHouse1, int typeHouse2) GetHouseType(Vector2 posOnRoad, float normA, float normB, float normC) {
        int typeHouse1 = -1, typeHouse2 = -1;
        float dx = (float)Math.Cos(Math.Atan(normA / -normB)) * 2;
        float dy = (float)Math.Sin(Math.Atan(normA / -normB)) * 2;

        Vector2 point1 = RoadsClass.RoundCoodinate(new Vector2(posOnRoad.x + dx, posOnRoad.y + dy));
        if (FieldClass.districts[(int)point1.x + FieldClass.fieldSizeHalf, (int)point1.y + FieldClass.fieldSizeHalf] == 0) {
            if ((GenerationClass.GetSeed() % 200) < 50) typeHouse1 = BuildsClass.idxsDistrict1[0];
            if ((GenerationClass.GetSeed() % 200) >= 50) typeHouse1 = BuildsClass.idxsDistrict1[1];
        }
        if (FieldClass.districts[(int)point1.x + FieldClass.fieldSizeHalf, (int)point1.y + FieldClass.fieldSizeHalf] == 1) {
            if ((GenerationClass.GetSeed() % 200) < 50) typeHouse1 = BuildsClass.idxsDistrict2[0];
            if ((GenerationClass.GetSeed() % 200) >= 50) typeHouse1 = BuildsClass.idxsDistrict2[1];
        }
        if (FieldClass.districts[(int)point1.x + FieldClass.fieldSizeHalf, (int)point1.y + FieldClass.fieldSizeHalf] == 2) {
            if ((GenerationClass.GetSeed() % 200) < 50) typeHouse1 = BuildsClass.idxsDistrict3[0];
            if ((GenerationClass.GetSeed() % 200) >= 50) typeHouse1 = BuildsClass.idxsDistrict3[1];
        }
        if (FieldClass.districts[(int)point1.x + FieldClass.fieldSizeHalf, (int)point1.y + FieldClass.fieldSizeHalf] == 3) {
            if ((GenerationClass.GetSeed() % 200) < 50) typeHouse1 = BuildsClass.idxsDistrict4[0];
            if ((GenerationClass.GetSeed() % 200) >= 50) typeHouse1 = BuildsClass.idxsDistrict4[1];
        }

        Vector2 point2 = RoadsClass.RoundCoodinate(new Vector2(posOnRoad.x - dx, posOnRoad.y - dy));
        if (FieldClass.districts[(int)point2.x + FieldClass.fieldSizeHalf, (int)point2.y + FieldClass.fieldSizeHalf] == 0) {
            if ((GenerationClass.GetSeed() % 200) < 50) typeHouse2 = BuildsClass.idxsDistrict1[0];
            if ((GenerationClass.GetSeed() % 200) >= 50) typeHouse2 = BuildsClass.idxsDistrict1[1];
        }
        if (FieldClass.districts[(int)point2.x + FieldClass.fieldSizeHalf, (int)point2.y + FieldClass.fieldSizeHalf] == 1) {
            if ((GenerationClass.GetSeed() % 200) < 50) typeHouse2 = BuildsClass.idxsDistrict2[0];
            if ((GenerationClass.GetSeed() % 200) >= 50) typeHouse2 = BuildsClass.idxsDistrict2[1];
        }
        if (FieldClass.districts[(int)point2.x + FieldClass.fieldSizeHalf, (int)point2.y + FieldClass.fieldSizeHalf] == 2) {
            if ((GenerationClass.GetSeed() % 200) < 30) typeHouse2 = BuildsClass.idxsDistrict3[0];
            if ((GenerationClass.GetSeed() % 200) >= 30) typeHouse2 = BuildsClass.idxsDistrict3[1];
        }
        if (FieldClass.districts[(int)point2.x + FieldClass.fieldSizeHalf, (int)point2.y + FieldClass.fieldSizeHalf] == 3) {
            if ((GenerationClass.GetSeed() % 200) < 50) typeHouse2 = BuildsClass.idxsDistrict4[0];
            if ((GenerationClass.GetSeed() % 200) >= 50) typeHouse2 = BuildsClass.idxsDistrict4[1];
        }
        GenerationClass.FuncSeed();
        return (typeHouse1, typeHouse2);
    }

    IEnumerator AsyncGen() {
        DateTimeOffset startDate = DateTimeOffset.Now;
        DateTimeOffset endDate = startDate.AddSeconds(GenerationClass.timeHousesBuildGeneration);
        for (int i = 0; i < RoadsClass.objects.Count && GenerationClass.CheckTime(endDate); ++i) {
            RoadObject roadObjectClass = RoadsClass.objects[i].GetComponent<RoadObject>();
            float mainRoadA = roadObjectClass.y1 - roadObjectClass.y2, mainRoadB = roadObjectClass.x2 - roadObjectClass.x1,
                  mainRoadC = roadObjectClass.x1 * roadObjectClass.y2 - roadObjectClass.x2 * roadObjectClass.y1; // main road line
            float len = (float)Math.Sqrt(Math.Pow(roadObjectClass.x1 - roadObjectClass.x2, 2) + Math.Pow(roadObjectClass.y1 - roadObjectClass.y2, 2));
            float angleHouse = roadObjectClass.angle;
            for (int lenIt = 1; lenIt < len && GenerationClass.CheckTime(endDate); ++lenIt) {
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
                (int typeHouse1, int typeHouse2) typesHouse = GetHouseType(new Vector2(posOnRoadX, posOnRoadY), normA, normB, normC);

                float widthHouse1 = (int)BuildsClass.preFubs[typesHouse.typeHouse1].GetComponent<BoxCollider>().size.x * BuildsClass.preFubs[typesHouse.typeHouse1].transform.localScale.x;
                float lenHouse1 = (int)BuildsClass.preFubs[typesHouse.typeHouse1].GetComponent<BoxCollider>().size.z * BuildsClass.preFubs[typesHouse.typeHouse1].transform.localScale.x;
                float dx1 = (float)Math.Cos(Math.Atan(normA / -normB)) * (lenHouse1 / 2 + 2);
                float dy1 = (float)Math.Sin(Math.Atan(normA / -normB)) * (lenHouse1 / 2 + 2);
                Vector3 point1 = RoadsClass.RoundCoodinate(new Vector3(posOnRoadX + dx1, 0, posOnRoadY + dy1));

                float widthHouse2 = (int)BuildsClass.preFubs[typesHouse.typeHouse2].GetComponent<BoxCollider>().size.x * BuildsClass.preFubs[typesHouse.typeHouse2].transform.localScale.x;
                float lenHouse2 = (int)BuildsClass.preFubs[typesHouse.typeHouse2].GetComponent<BoxCollider>().size.z * BuildsClass.preFubs[typesHouse.typeHouse2].transform.localScale.x;
                float dx2 = (float)Math.Cos(Math.Atan(normA / -normB)) * (lenHouse2 / 2 + 2);
                float dy2 = (float)Math.Sin(Math.Atan(normA / -normB)) * (lenHouse2 / 2 + 2);
                Vector3 point2 = RoadsClass.RoundCoodinate(new Vector3(posOnRoadX - dx2, 0, posOnRoadY - dy2));

                if (FieldClass.objects[(int)point1.x + FieldClass.fieldSizeHalf, (int)point1.z + FieldClass.fieldSizeHalf] == null) {
                    float angle = (float)(angleHouse / 57.3);
                    Vector3 side1 = new Vector3(point1.x + (float)Math.Cos(angle) * (widthHouse1 / 2), 0, point1.z + (float)Math.Sin(angle) * (widthHouse1 / 2));
                    Vector3 side2 = new Vector3(point1.x - (float)Math.Cos(angle) * (widthHouse1 / 2), 0, point1.z - (float)Math.Sin(angle) * (widthHouse1 / 2));

                    Vector3 vertex1 = new Vector3(side1.x + (float)Math.Cos(angle + Math.PI / 2) * (lenHouse1 / 2), 0, side1.z + (float)Math.Sin(angle + Math.PI / 2) * (lenHouse1 / 2));
                    Vector3 vertex2 = new Vector3(side1.x - (float)Math.Cos(angle + Math.PI / 2) * (lenHouse1 / 2), 0, side1.z - (float)Math.Sin(angle + Math.PI / 2) * (lenHouse1 / 2));
                    Vector3 vertex3 = new Vector3(side2.x + (float)Math.Cos(angle + Math.PI / 2) * (lenHouse1 / 2), 0, side2.z + (float)Math.Sin(angle + Math.PI / 2) * (lenHouse1 / 2));
                    Vector3 vertex4 = new Vector3(side2.x - (float)Math.Cos(angle + Math.PI / 2) * (lenHouse1 / 2), 0, side2.z - (float)Math.Sin(angle + Math.PI / 2) * (lenHouse1 / 2));

                    bool collision = false;
                    for (int j = 0; j < RoadsClass.objects.Count; ++j) {
                        RoadObject tmpRoadObject = RoadsClass.objects[j].GetComponent<RoadObject>();
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

                    for (int itX = (int)(point1.x - 20); itX <= point1.x + 20; ++itX) {
                        for (int itY = (int)(point1.z - 20); itY <= point1.z + 20; ++itY) {
                            if (itX >= FieldClass.fieldSizeHalf || itY >= FieldClass.fieldSizeHalf || itX <= -FieldClass.fieldSizeHalf || itY <= -FieldClass.fieldSizeHalf ||
                                FieldClass.objects[itX + FieldClass.fieldSizeHalf, itY + FieldClass.fieldSizeHalf] == null ||
                                !(FieldClass.objects[itX + FieldClass.fieldSizeHalf, itY + FieldClass.fieldSizeHalf].GetComponent<BuildObject>())) continue;
                            BuildObject tmpBuildObject = FieldClass.objects[itX + FieldClass.fieldSizeHalf, itY + FieldClass.fieldSizeHalf].GetComponent<BuildObject>();

                            float tmpAngle = (float)(RoadsClass.objects[tmpBuildObject.connectedRoad].GetComponent<RoadObject>().angle / 57.3);
                            float tmpWidthHouse = (int)tmpBuildObject.GetComponent<BoxCollider>().size.x * tmpBuildObject.transform.localScale.x;
                            float tmpLenHouse = (int)tmpBuildObject.GetComponent<BoxCollider>().size.z * tmpBuildObject.transform.localScale.x;

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
                        BuildsClass.CreateObject(point1, angleHouse * -1, typesHouse.typeHouse1, i);
                        FieldClass.objects[(int)point1.x + FieldClass.fieldSizeHalf, (int)point1.z + FieldClass.fieldSizeHalf] = BuildsClass.objects[BuildsClass.objects.Count - 1];
                    }
                }

                if (FieldClass.objects[(int)point2.x + FieldClass.fieldSizeHalf, (int)point2.z + FieldClass.fieldSizeHalf] == null) {
                    float angle = (float)(angleHouse / 57.3);
                    Vector3 side1 = new Vector3(point2.x + (float)Math.Cos(angle) * (widthHouse2 / 2), 0, point2.z + (float)Math.Sin(angle) * (widthHouse2 / 2));
                    Vector3 side2 = new Vector3(point2.x - (float)Math.Cos(angle) * (widthHouse2 / 2), 0, point2.z - (float)Math.Sin(angle) * (widthHouse2 / 2));

                    Vector3 vertex1 = new Vector3(side1.x + (float)Math.Cos(angle + Math.PI / 2) * (lenHouse2 / 2), 0, side1.z + (float)Math.Sin(angle + Math.PI / 2) * (lenHouse2 / 2));
                    Vector3 vertex2 = new Vector3(side1.x - (float)Math.Cos(angle + Math.PI / 2) * (lenHouse2 / 2), 0, side1.z - (float)Math.Sin(angle + Math.PI / 2) * (lenHouse2 / 2));
                    Vector3 vertex3 = new Vector3(side2.x + (float)Math.Cos(angle + Math.PI / 2) * (lenHouse2 / 2), 0, side2.z + (float)Math.Sin(angle + Math.PI / 2) * (lenHouse2 / 2));
                    Vector3 vertex4 = new Vector3(side2.x - (float)Math.Cos(angle + Math.PI / 2) * (lenHouse2 / 2), 0, side2.z - (float)Math.Sin(angle + Math.PI / 2) * (lenHouse2 / 2));

                    bool collision = false;
                    for (int j = 0; j < RoadsClass.objects.Count; ++j) {
                        RoadObject tmpRoadObject = RoadsClass.objects[j].GetComponent<RoadObject>();
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

                    for (int itX = (int)(point2.x - 20); itX <= point2.x + 20; ++itX) {
                        for (int itY = (int)(point2.z - 20); itY <= point2.z + 20; ++itY) {
                            if (itX >= FieldClass.fieldSizeHalf || itY >= FieldClass.fieldSizeHalf || itX <= -FieldClass.fieldSizeHalf || itY <= -FieldClass.fieldSizeHalf ||
                                FieldClass.objects[itX + FieldClass.fieldSizeHalf, itY + FieldClass.fieldSizeHalf] == null ||
                                !(FieldClass.objects[itX + FieldClass.fieldSizeHalf, itY + FieldClass.fieldSizeHalf].GetComponent<BuildObject>())) continue;
                            BuildObject tmpBuildObject = FieldClass.objects[itX + FieldClass.fieldSizeHalf, itY + FieldClass.fieldSizeHalf].GetComponent<BuildObject>();

                            float tmpAngle = (float)(RoadsClass.objects[tmpBuildObject.connectedRoad].GetComponent<RoadObject>().angle / 57.3);
                            float tmpWidthHouse = (int)tmpBuildObject.GetComponent<BoxCollider>().size.x * tmpBuildObject.transform.localScale.x;
                            float tmpLenHouse = (int)tmpBuildObject.GetComponent<BoxCollider>().size.z * tmpBuildObject.transform.localScale.x;

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
                        BuildsClass.CreateObject(point2, angleHouse * -1, typesHouse.typeHouse2, i);
                        FieldClass.objects[(int)point2.x + FieldClass.fieldSizeHalf, (int)point2.z + FieldClass.fieldSizeHalf] = BuildsClass.objects[BuildsClass.objects.Count - 1];
                    }
                }
            }
            yield return null;
        }
        yield return null;
        isOver = true;
    }

    public void StartGeneration() {
        StartCoroutine(AsyncGen());
    }
}
