using UnityEngine;
using UnityEngine.Jobs;
using Unity.Jobs;
using Unity.Collections;
using System;

public struct HumanMoveJob : IJobParallelForTransform {
    public NativeArray <Vector3> vertexTo;
    public NativeArray <bool> vertexIsActive;
    public NativeArray <int> cntTranslate;
    public NativeArray <int> cntWaitingFrames;
    public float speed;
    public Vector3 cameraPos;
    public float fixedDeltaTime;
    public int cntMissedFrames;

    public void Execute(int idx, TransformAccess transform) {
        double distToCamera = Math.Sqrt(Math.Pow(transform.position.x - cameraPos.x, 2) + Math.Pow(transform.position.z - cameraPos.z, 2));
        if (vertexIsActive[idx] && (distToCamera < 20 || cntWaitingFrames[idx] > 100)) {
            Vector3 vertexFrom = transform.position;
            double dist = Math.Sqrt(Math.Pow(vertexTo[idx].x - vertexFrom.x, 2) + Math.Pow(vertexTo[idx].z - vertexFrom.z, 2));
            if (dist > 0.01) {
                Vector3 move = new Vector3((vertexTo[idx].x - vertexFrom.x) / (float)dist, 0, (vertexTo[idx].z - vertexFrom.z) / (float)dist) * speed * fixedDeltaTime;
                move.x *= (cntMissedFrames + 1) * (cntWaitingFrames[idx] + 1);
                move.z *= (cntMissedFrames + 1) * (cntWaitingFrames[idx] + 1);
                if (move.magnitude >= dist) {
                    transform.position = vertexTo[idx];
                    vertexIsActive[idx] = false;
                    cntTranslate[idx] = 0;
                }
                else transform.position = new Vector3(transform.position.x + move.x, 0, transform.position.z + move.z);
                transform.rotation = Quaternion.Euler(0, (float)Math.Atan2(vertexTo[idx].z - vertexFrom.z, vertexTo[idx].x - vertexFrom.x) * -57.3f + 90f, 0);
                if (dist < 0.1) ++cntTranslate[idx];
                if (cntTranslate[idx] > 10) {
                    vertexIsActive[idx] = false;
                    cntTranslate[idx] = 0;
                }
                cntWaitingFrames[idx] = 0;
            }
            else vertexIsActive[idx] = false;
            cntWaitingFrames[idx] = 0;
        }
        else ++cntWaitingFrames[idx];
    }
}
