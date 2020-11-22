using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class LoadPanelBlur : MonoBehaviour
{
    public float blur = 0;
    private Material MaterialClass;

    void Start()
    {
        Image ImageClass = gameObject.GetComponent<Image>();
        MaterialClass = ImageClass.material;
    }

    void FixedUpdate()
    {
        MaterialClass.SetFloat("_Size", blur);
    }
}
