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
    private List <Block> SqrtDecomp;
    private int sqrtSize = 20;
    private ulong seed;
    private int maxX = 0, minX = 0;
    private int maxZ = 0, minZ = 0;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        RoadsClass = MainCamera.GetComponent <Roads> ();
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

    private bool CheckCollision() {
        return true;
    }

    public ulong StartGeneration(ulong newSeed) {
        seed = newSeed;
        RoadsClass.CreateObject("Road1", new Vector3(0, 0, 100), new Vector3 (0, 0, 110), 90);
        AddBlock((int)(seed % 1e9), new Vector3 (0, 0, 110), RoadsClass.objects.Count - 1);
        int n = 100 + (int)(seed % 50);
        seed = (ulong)((GenerationClass.phi(seed) * seed) % 1e9) + 1;
        for (int i = 0; i < n; ++i) {
            (Vector3 point, int roadIdx) startPoint = GetMinBlock();

            RoadObject RoadObjectClass = RoadsClass.objects[startPoint.roadIdx].GetComponent <RoadObject> ();
            float startAngle = RoadObjectClass.angle;
            float angle = startAngle + (-135 + (int)(seed % 270));
            float len = 10 + (float)(seed % 10);
            float x = (float)Math.Cos(angle / 57.3) * len;
            float y = (float)Math.Sin(angle / 57.3) * len;

            Vector3 endPoint = RoadsClass.RoundCoodinate(new Vector3(startPoint.point.x + x, 0, startPoint.point.z + y));

            if (CheckCollision()) {
                RoadsClass.CreateObject("Road1", startPoint.point, endPoint, angle, startPoint.roadIdx);
                AddBlock((int)(seed % 1e9), endPoint, RoadsClass.objects.Count - 1);
                seed = (ulong)((GenerationClass.phi(seed) * seed) % 1e9) + 1;
            }
        }
        return seed;
    }
}
