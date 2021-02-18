using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Taxation : MonoBehaviour
{
    public GameObject GKHObject;
    public GameObject NDSObject;
    public GameObject NDFLObject;
    public GameObject NPPObject;
    public float GKHkoeff = 1;
    public float NDSkoeff = 1;
    public float NDFLkoeff = 1;
    public float NPPkoeff = 1;

    public float GetGKH()
    {
        return GKHObject.GetComponent<Slider>().value * GKHkoeff;
    }

    public float GetNDS()
    {
        return NDSObject.GetComponent<Slider>().value * NDSkoeff;
    }

    public float GetNDFL()
    {
        return NDFLObject.GetComponent<Slider>().value * NDFLkoeff;
    }

    public float GetNPP()
    {
        return NPPObject.GetComponent<Slider>().value * NPPkoeff;
    }
}
