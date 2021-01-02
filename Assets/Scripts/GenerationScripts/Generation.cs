using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generation : MonoBehaviour {
    private GameObject MainCamera;
    private GenerationRoads GenerationRoadsClass;
    private GenerationDistricts GenerationDistrictsClass;
    private GenerationHouses GenerationHousesClass;
    private GenerationCommerces GenerationCommercesClass;

    public ulong seed;
    public int timeRoadsBuild;
    public int minLenRoads, deltaLenRoads;
    public int averageCntCommercesInDistrict;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        GenerationRoadsClass = MainCamera.GetComponent <GenerationRoads> ();
        GenerationDistrictsClass = MainCamera.GetComponent <GenerationDistricts> ();
        GenerationHousesClass = MainCamera.GetComponent <GenerationHouses> ();
        GenerationCommercesClass = MainCamera.GetComponent <GenerationCommerces> ();
    }

    private void Start() {
        seed = GenerationRoadsClass.StartGeneration(seed);
        seed = GenerationDistrictsClass.StartGeneration(seed);
        seed = GenerationCommercesClass.StartGeneration(seed);
        seed = GenerationHousesClass.StartGeneration(seed);
    }

    public ulong funcSeed(ulong n) {
        ulong ans = n;
        for (ulong i = 2; i * i <= n; ++i)
            if (n % i == 0) {
                while (n % i == 0)
                    n /= i;
                ans -= ans / i;
            }
        if (n > 1) ans -= ans / n;
        return (ulong)((3 * ans + 3) % 1e9 + 1);
    }
}
