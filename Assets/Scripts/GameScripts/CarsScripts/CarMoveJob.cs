using UnityEngine;
using UnityEngine.Jobs;
using Unity.Jobs;
using Unity.Collections;
using System;

public struct CarMoveJob : IJobParallelForTransform  {
    public NativeArray <Vector3> vertexTo;
    public NativeArray <bool> vertexIsActive;
    public float speed;
    public float fixedDeltaTime;
    public int cntMissedFrames;

    public void Execute(int idx, TransformAccess transform) {
        if (vertexIsActive[idx]) {
            Vector3 vertexFrom = transform.position;
            float dist = (float)Math.Sqrt(Math.Pow(vertexTo[idx].x - vertexFrom.x, 2) + Math.Pow(vertexTo[idx].z - vertexFrom.z, 2));
            if (dist > 0.5f) {
                Vector3 move = new Vector3((vertexTo[idx].x - vertexFrom.x) / dist, 0, (vertexTo[idx].z - vertexFrom.z) / dist) * speed * fixedDeltaTime;
                transform.rotation = Quaternion.Euler(0, (float)Math.Atan2(vertexTo[idx].z - vertexFrom.z, vertexTo[idx].x - vertexFrom.x) * -57.3f + 90f, 0);
                transform.position = new Vector3(transform.position.x + move.x * (cntMissedFrames + 1), 0, transform.position.z + move.z * (cntMissedFrames + 1));
            }
            else vertexIsActive[idx] = false;
        }
    }
}
