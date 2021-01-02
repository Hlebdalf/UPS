using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour {
    public int gridSize, fieldSize, fieldSizeHalf;
    public GameObject[,] objects;
    public int[,] districts;
    public int centerX = 0, centerY = 0;
    public float timeBuildProcess;

    private void Awake() {
        objects = new GameObject[fieldSize, fieldSize];
        districts = new int[fieldSize, fieldSize];
        for (int i = 0; i < fieldSize; ++i) {
            for (int j = 0; j < fieldSize; ++j) {
                objects[i, j] = null;
                districts[i, j] = -1;
            }
        }
    }
}
