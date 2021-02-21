using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class GenerationPeople : MonoBehaviour {
    private GameObject MainCamera;
    private Field FieldClass;
    private Builds BuildsClass;
    private Economy EconomyClass;
    
    public bool isOver = false;
    
    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        FieldClass = MainCamera.GetComponent <Field> ();
        BuildsClass = MainCamera.GetComponent <Builds> ();
        EconomyClass = MainCamera.GetComponent <Economy> ();
    }

    IEnumerator AsyncGen() {
        bool pass = true, prePass = true;
        int cnt = (int)UnityEngine.Random.Range((float)BuildsClass.objects.Count, 10f * (float)BuildsClass.objects.Count);
        for (int delay = 0; delay < cnt && pass; ++delay) {
            if (!prePass) pass = false;
            for (int buildIt = 0; buildIt < BuildsClass.objects.Count; ++buildIt) {
                BuildObject houseClass = BuildsClass.objects[buildIt].GetComponent <BuildObject> ();
                if (houseClass.cntPeople < houseClass.maxCntPeople) {
                    prePass = true;
                    int commerceIt = (int)UnityEngine.Random.Range(0f, BuildsClass.commerces.Count - 0.01f);
                    BuildObject commerceClass = BuildsClass.commerces[commerceIt].GetComponent <BuildObject> ();
                    if (commerceClass.cntPeople < commerceClass.maxCntPeople) {
                        if (houseClass.cntPeople == 0) BuildsClass.objectsWithoutAvailableSeats.Add(BuildsClass.objects[buildIt]);
                        if (commerceClass.cntPeople == 0) BuildsClass.commercesWithoutAvailableSeats.Add(BuildsClass.commerces[commerceIt]);
                        EconomyClass.AddCntPeople(FieldClass.districts[(int)houseClass.x + FieldClass.fieldSizeHalf, (int)houseClass.y + FieldClass.fieldSizeHalf]);
                        ++commerceClass.cntPeople;
                        ++houseClass.cntPeople;
                    }
                    else {
                        int it = -1;
                        for (int i = 0; i < BuildsClass.commerces.Count; ++i) {
                            BuildObject commerceClassTmp = BuildsClass.commerces[i].GetComponent <BuildObject> ();
                            if (commerceClassTmp.cntPeople < commerceClassTmp.maxCntPeople) it = i;
                        }
                        if (it == -1) prePass = false;
                        else {
                            EconomyClass.AddCntPeople(FieldClass.districts[(int)houseClass.x + FieldClass.fieldSizeHalf, (int)houseClass.y + FieldClass.fieldSizeHalf]);
                            ++BuildsClass.commerces[it].GetComponent <BuildObject> ().cntPeople;
                            ++houseClass.cntPeople;
                        }
                    }
                }
                else prePass = false;
            }
            if (prePass) pass = true;
            if (delay % 100 == 0) yield return null;
        }
        isOver = true;
    }
    
    public void StartGeneration() {
        StartCoroutine(AsyncGen());
    }
}
