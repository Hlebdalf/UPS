using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossroadObject : MonoBehaviour {
    public float x, y;
    public int idx, xr, yr;
    public List <int> connectedRoads;

    private void Awake() {
        connectedRoads = new List <int> ();
    }
}
