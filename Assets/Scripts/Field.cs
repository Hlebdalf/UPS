using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Field : MonoBehaviour {
    private GameObject MainCamera;
    private People PeopleClass;

    public int gridSize, fieldSize, fieldSizeHalf;
    public GameObject[,] objects;
    public int[,] districts;
    public Dictionary <GameObject, int> numInGraph;
    public Dictionary <int, GameObject> objInGraph;
    public List <(int v, float w)>[] graph;
    public int centerX = 0, centerY = 0;
    public float timeBuildProcess;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        PeopleClass = MainCamera.GetComponent <People> ();
        objects = new GameObject[fieldSize, fieldSize];
        districts = new int[fieldSize, fieldSize];
        numInGraph = new Dictionary <GameObject, int> ();
        objInGraph = new Dictionary <int, GameObject> ();
        for (int i = 0; i < fieldSize; ++i) {
            for (int j = 0; j < fieldSize; ++j) {
                objects[i, j] = null;
                districts[i, j] = -1;
            }
        }
    }

    // private void Update() {
    //     if (Input.GetMouseButtonDown(0)) {
    //         Text txt = PeopleClass.PassportCard.GetComponent <Text> ();
    //         txt.text = "";
    //     }
    // }
}
