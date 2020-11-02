using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builds : MonoBehaviour {

    public GameObject[] preFubs;
    private List <GameObject> objects;

    void Start() {
        objects = new List <GameObject> ();
        for (int i = 0; i < preFubs.Length; ++i) {
            objects.Add(Instantiate(preFubs[i], new Vector3(0, 0, 0), Quaternion.identity));
        }
    }

    void Update() {
        
    }
}
