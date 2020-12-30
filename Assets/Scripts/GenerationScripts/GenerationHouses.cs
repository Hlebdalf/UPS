using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationHouses : MonoBehaviour {
    private GameObject MainCamera;
    private Generation GenerationClass;
    private Roads RoadsClass;
    private ulong seed;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        RoadsClass = MainCamera.GetComponent <Roads> ();
        GenerationClass = MainCamera.GetComponent <Generation> ();
    }

    public ulong StartGeneration(ulong newSeed) {
        seed = newSeed;
        return seed;
    }
}
