using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationDistricts : MonoBehaviour {
    private GameObject MainCamera;
    private Generation GenerationClass;
    private Roads RoadsClass;
    private Field FieldClass;
    private ulong seed;
    private int centerX, centerY;
    private bool[] used = {false, false, false, false, false, false, false, false};

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        RoadsClass = MainCamera.GetComponent <Roads> ();
        FieldClass = MainCamera.GetComponent <Field> ();
        GenerationClass = MainCamera.GetComponent <Generation> ();
    }

    private Vector2 GetStartPos(Vector2 point, int cnt) {
        cnt = (int)(cnt % 8);
        while (used[cnt]) {
            cnt = (int)(GenerationClass.FuncSeed((ulong)cnt) % 8);
        }
        used[cnt] = true;
        if (cnt == 0) {
            point.y += 2;
        }
        if (cnt == 1) {
            point.x += 1;
            point.y += 1;
        }
        if (cnt == 2) {
            point.x += 2;
        }
        if (cnt == 3) {
            point.x += 1;
            point.y -= 1;
        }
        if (cnt == 4) {
            point.y -= 2;
        }
        if (cnt == 5) {
            point.x -= 1;
            point.y -= 1;
        }
        if (cnt == 6) {
            point.x -= 2;
        }
        if (cnt == 7) {
            point.x -= 1;
            point.y += 1;
        }
        return point;
    }

    public void StartGeneration() {
        seed = GenerationClass.GetSeed();
        centerX = FieldClass.centerX;
        centerY = FieldClass.centerY;

        Queue <Vector2> fillPoints = new Queue <Vector2> ();
        Vector2 tmpP = GetStartPos(new Vector2(centerX, centerY), (int)(seed % 1e9));
        seed = (ulong)((GenerationClass.FuncSeed(seed) * seed) % 1e9) + 1;
        fillPoints.Enqueue(tmpP);
        FieldClass.districts[(int)tmpP.x + FieldClass.fieldSizeHalf, (int)tmpP.y + FieldClass.fieldSizeHalf] = 0;

        tmpP = GetStartPos(new Vector2(centerX, centerY), (int)(seed % 1e9));
        seed = (ulong)((GenerationClass.FuncSeed(seed) * seed) % 1e9) + 1;
        fillPoints.Enqueue(tmpP);
        FieldClass.districts[(int)tmpP.x + FieldClass.fieldSizeHalf, (int)tmpP.y + FieldClass.fieldSizeHalf] = 1;

        tmpP = GetStartPos(new Vector2(centerX, centerY), (int)(seed % 1e9));
        seed = (ulong)((GenerationClass.FuncSeed(seed) * seed) % 1e9) + 1;
        fillPoints.Enqueue(tmpP);
        FieldClass.districts[(int)tmpP.x + FieldClass.fieldSizeHalf, (int)tmpP.y + FieldClass.fieldSizeHalf] = 2;
        
        tmpP = GetStartPos(new Vector2(centerX, centerY), (int)(seed % 1e9));
        seed = (ulong)((GenerationClass.FuncSeed(seed) * seed) % 1e9) + 1;
        fillPoints.Enqueue(tmpP);
        FieldClass.districts[(int)tmpP.x + FieldClass.fieldSizeHalf, (int)tmpP.y + FieldClass.fieldSizeHalf] = 3;
        
        while (fillPoints.Count > 0) {
            Vector2 point = fillPoints.Dequeue();
            if (point.x + 1 < FieldClass.fieldSizeHalf && FieldClass.districts[(int)point.x + 1 + FieldClass.fieldSizeHalf, (int)point.y + FieldClass.fieldSizeHalf] == -1) {
                FieldClass.districts[(int)point.x + 1 + FieldClass.fieldSizeHalf, (int)point.y + FieldClass.fieldSizeHalf] = FieldClass.districts[(int)point.x + FieldClass.fieldSizeHalf, (int)point.y + FieldClass.fieldSizeHalf];
                fillPoints.Enqueue(new Vector2(point.x + 1, point.y));
            }
            if (point.y + 1 < FieldClass.fieldSizeHalf && FieldClass.districts[(int)point.x + FieldClass.fieldSizeHalf, (int)point.y + 1 + FieldClass.fieldSizeHalf] == -1) {
                FieldClass.districts[(int)point.x + FieldClass.fieldSizeHalf, (int)point.y + 1 + FieldClass.fieldSizeHalf] = FieldClass.districts[(int)point.x + FieldClass.fieldSizeHalf, (int)point.y + FieldClass.fieldSizeHalf];
                fillPoints.Enqueue(new Vector2(point.x, point.y + 1));
            }
            if (point.x - 1 >= -FieldClass.fieldSizeHalf && FieldClass.districts[(int)point.x - 1 + FieldClass.fieldSizeHalf, (int)point.y + FieldClass.fieldSizeHalf] == -1) {
                FieldClass.districts[(int)point.x - 1 + FieldClass.fieldSizeHalf, (int)point.y + FieldClass.fieldSizeHalf] = FieldClass.districts[(int)point.x + FieldClass.fieldSizeHalf, (int)point.y + FieldClass.fieldSizeHalf];
                fillPoints.Enqueue(new Vector2(point.x - 1, point.y));
            }
            if (point.y - 1 >= -FieldClass.fieldSizeHalf && FieldClass.districts[(int)point.x + FieldClass.fieldSizeHalf, (int)point.y - 1 + FieldClass.fieldSizeHalf] == -1) {
                FieldClass.districts[(int)point.x + FieldClass.fieldSizeHalf, (int)point.y - 1 + FieldClass.fieldSizeHalf] = FieldClass.districts[(int)point.x + FieldClass.fieldSizeHalf, (int)point.y + FieldClass.fieldSizeHalf];
                fillPoints.Enqueue(new Vector2(point.x, point.y - 1));
            }
            if (point.x + 1 < FieldClass.fieldSizeHalf && point.y + 1 < FieldClass.fieldSizeHalf && FieldClass.districts[(int)point.x + 1 + FieldClass.fieldSizeHalf, (int)point.y + 1 + FieldClass.fieldSizeHalf] == -1) {
                FieldClass.districts[(int)point.x + 1 + FieldClass.fieldSizeHalf, (int)point.y + 1 + FieldClass.fieldSizeHalf] = FieldClass.districts[(int)point.x + FieldClass.fieldSizeHalf, (int)point.y + FieldClass.fieldSizeHalf];
                fillPoints.Enqueue(new Vector2(point.x + 1, point.y + 1));
            }
            if (point.x - 1 >= -FieldClass.fieldSizeHalf && point.y + 1 < FieldClass.fieldSizeHalf && FieldClass.districts[(int)point.x - 1 + FieldClass.fieldSizeHalf, (int)point.y + 1 + FieldClass.fieldSizeHalf] == -1) {
                FieldClass.districts[(int)point.x - 1 + FieldClass.fieldSizeHalf, (int)point.y + 1 + FieldClass.fieldSizeHalf] = FieldClass.districts[(int)point.x + FieldClass.fieldSizeHalf, (int)point.y + FieldClass.fieldSizeHalf];
                fillPoints.Enqueue(new Vector2(point.x - 1, point.y + 1));
            }
            if (point.x + 1 < FieldClass.fieldSizeHalf && point.y - 1 >= -FieldClass.fieldSizeHalf && FieldClass.districts[(int)point.x + 1 + FieldClass.fieldSizeHalf, (int)point.y - 1 + FieldClass.fieldSizeHalf] == -1) {
                FieldClass.districts[(int)point.x + 1 + FieldClass.fieldSizeHalf, (int)point.y - 1 + FieldClass.fieldSizeHalf] = FieldClass.districts[(int)point.x + FieldClass.fieldSizeHalf, (int)point.y + FieldClass.fieldSizeHalf];
                fillPoints.Enqueue(new Vector2(point.x + 1, point.y - 1));
            }
            if (point.x - 1 >= -FieldClass.fieldSizeHalf && point.y - 1 >= -FieldClass.fieldSizeHalf && FieldClass.districts[(int)point.x - 1 + FieldClass.fieldSizeHalf, (int)point.y - 1 + FieldClass.fieldSizeHalf] == -1) {
                FieldClass.districts[(int)point.x - 1 + FieldClass.fieldSizeHalf, (int)point.y - 1 + FieldClass.fieldSizeHalf] = FieldClass.districts[(int)point.x + FieldClass.fieldSizeHalf, (int)point.y + FieldClass.fieldSizeHalf];
                fillPoints.Enqueue(new Vector2(point.x - 1, point.y - 1));
            }
        }
    }
}
