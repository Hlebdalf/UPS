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

    float GetGKH()
    {
        return GKHObject.GetComponent<Slider>().value * GKHkoeff;
    }

    float GetNDS()
    {
        return NDSObject.GetComponent<Slider>().value * NDSkoeff;
    }

    float GetNDFL()
    {
        return NDFLObject.GetComponent<Slider>().value * NDFLkoeff;
    }

    float GetNPP()
    {
        return NPPObject.GetComponent<Slider>().value * NPPkoeff;
    }
}
