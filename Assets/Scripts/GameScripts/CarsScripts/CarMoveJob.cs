using UnityEngine;
using UnityEngine.Jobs;
using Unity.Jobs;
using Unity.Collections;

public struct CarMoveJob : IJobParallelForTransform  {
    public NativeArray <Vector3> moveArray;
    public NativeArray <float> angleArray;
    public int cntMissedFrames;

    public void Execute(int idx, TransformAccess transform) {
        transform.rotation = Quaternion.Euler(0, angleArray[idx], 0);
        transform.position = new Vector3(transform.position.x + moveArray[idx].x * (cntMissedFrames + 1), 0, transform.position.z + moveArray[idx].z * (cntMissedFrames + 1));
    }
}
