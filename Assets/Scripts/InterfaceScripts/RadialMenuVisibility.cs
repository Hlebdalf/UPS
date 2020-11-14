using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuVisibility : MonoBehaviour
{
    public float blur = 0;
    private Material MaterialClass;
    public bool radialMenuDisability = true;
    
    void Start()
    {
        Image ImageClass = gameObject.GetComponent<Image>();
        MaterialClass = ImageClass.material;
    }

    void FixedUpdate()
    {
            MaterialClass.SetFloat("_Size", blur);
            gameObject.SetActive(radialMenuDisability);
    }

    public void IsDisabled(bool dis)
    {
        
    }
}
