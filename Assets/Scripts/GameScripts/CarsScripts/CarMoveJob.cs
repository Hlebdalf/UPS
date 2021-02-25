using UnityEngine;
using UnityEngine.Jobs;
using Unity.Jobs;
using Unity.Collections;
using System;

public struct CarMoveJob : IJobParallelForTransform  {
    public NativeArray <Vector3> vertexFrom;
    public NativeArray <Vector3> vertexTo;
    public NativeArray <bool> isShiftVector;
    public NativeArray <int> numOfLanes;
    public NativeArray <bool> vertexIsActive;
    public NativeArray <bool> onVisibleInCamera;
    public NativeArray <int> cntWaitingFrames;
    public NativeArray <float> speeds;
    public Vector3 cameraPos;
    public float fixedDeltaTime;
    public int cntMissedFrames;

    public void Execute(int idx, TransformAccess transform) {
        double distToCamera = Math.Sqrt(Math.Pow(transform.position.x - cameraPos.x, 2) + Math.Pow(transform.position.z - cameraPos.z, 2));
        if (vertexIsActive[idx] && ((onVisibleInCamera[idx] && distToCamera < 200) || cntWaitingFrames[idx] > 30)) {
            float sideX = 0, sideZ = 0;
            if (isShiftVector[idx]) {
                if (numOfLanes[idx] == 1) {
                    sideX = (float)Math.Cos(Math.Atan2(vertexTo[idx].x - vertexFrom[idx].x, vertexTo[idx].z - vertexFrom[idx].z)) * (0.22f);
                    sideZ = -(float)Math.Sin(Math.Atan2(vertexTo[idx].x - vertexFrom[idx].x, vertexTo[idx].z - vertexFrom[idx].z)) * (0.22f);
                }
                else if (numOfLanes[idx] == 2) {
                    sideX = (float)Math.Cos(Math.Atan2(vertexTo[idx].x - vertexFrom[idx].x, vertexTo[idx].z - vertexFrom[idx].z)) * (0.58f);
                    sideZ = -(float)Math.Sin(Math.Atan2(vertexTo[idx].x - vertexFrom[idx].x, vertexTo[idx].z - vertexFrom[idx].z)) * (0.58f);
                }
            }

            float mainRoadA = vertexFrom[idx].z - vertexTo[idx].z, mainRoadB = vertexTo[idx].x - vertexFrom[idx].x,
                  mainRoadC = vertexFrom[idx].x * vertexTo[idx].z - vertexTo[idx].x * vertexFrom[idx].z; // main road line
            float normA = -mainRoadB, normB = mainRoadA, normC = -(normA * transform.position.x + normB * transform.position.z); // norm
            float normCrossMainRoadX = -(mainRoadC * normB - normC * mainRoadB) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate
            float normCrossMainRoadZ = -(mainRoadA * normC - normA * mainRoadC) / (mainRoadA * normB - normA * mainRoadB); // rounded coordinate

            Vector3 From = new Vector3(normCrossMainRoadX, 0f, normCrossMainRoadZ);
            Vector3 To = new Vector3(vertexTo[idx].x, 0f, vertexTo[idx].z);
            double dist = Math.Sqrt(Math.Pow(To.x - From.x, 2) + Math.Pow(To.z - From.z, 2));
            if (dist > 0.1) {
                Vector3 move = new Vector3((To.x - From.x) / (float)dist, 0, (To.z - From.z) / (float)dist) * speeds[idx] * fixedDeltaTime;
                move.x *= (cntMissedFrames + 1) * (cntWaitingFrames[idx] + 1);
                move.z *= (cntMissedFrames + 1) * (cntWaitingFrames[idx] + 1);
                if (move.magnitude >= dist) {
                    To.x += sideX;
                    To.z += sideZ;
                    transform.position = To;
                    vertexIsActive[idx] = false;
                }
                else transform.position = new Vector3(normCrossMainRoadX + move.x + sideX, 0, normCrossMainRoadZ + move.z + sideZ);
                transform.rotation = Quaternion.Euler(0, (float)Math.Atan2(To.z - From.z, To.x - From.x) * -57.3f + 90f, 0);
            }
            else vertexIsActive[idx] = false;
            cntWaitingFrames[idx] = 0;
        }
        else ++cntWaitingFrames[idx];
    }
}
