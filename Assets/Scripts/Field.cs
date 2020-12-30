using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Field : MonoBehaviour {
    public int gridSize, fieldSize, fieldSizeHalf;
    public GameObject[,] objects;

    private void Awake() {
        objects = new GameObject[fieldSize, fieldSize];
        for (int i = 0; i < fieldSize; ++i) {
            for (int j = 0; j < fieldSize; ++j) {
                objects[i, j] = null;
            }
        }
    }
}
