using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Taxation : MonoBehaviour {
    [SerializeField]
    private Economy EconomyClass;
    [SerializeField]
    private GameObject GKHObject;
    [SerializeField]
    private GameObject NDSObject;
    [SerializeField]
    private GameObject NDFLObject;
    [SerializeField]
    private GameObject NPPObject;
    [SerializeField]
    private float GKHkoeff = 1;
    [SerializeField]
    private float NDSkoeff = 1;
    [SerializeField]
    private float NDFLkoeff = 1;
    [SerializeField]
    private float NPPkoeff = 1;

    private float GetGKH() { return GKHObject.GetComponent<Slider>().value * GKHkoeff; }
    private float GetNDS() { return NDSObject.GetComponent<Slider>().value * NDSkoeff; }
    private float GetNDFL() { return NDFLObject.GetComponent<Slider>().value * NDFLkoeff; }
    private float GetNPP() { return NPPObject.GetComponent<Slider>().value * NPPkoeff; }

    public (float, float, float, float) GetGDPk() {
        return (GetGKH(), GetNDFL(), GetNDS(), GetNPP());
    }

    public void ChangeGDPk() {
        EconomyClass.ChangeGDP();
    }
}
