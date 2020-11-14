using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour
{
    public float blurRoof = 1.5f;
    public float blur = 0;
    private Material MaterialClass;
    
    void Start()
    {
        Image ImageClass = gameObject.GetComponent<Image>();
        MaterialClass = ImageClass.material;
        /*Color imageColor = ImageClass.color;
        imageColor.a = 0;
        ImageClass.color = imageColor;*/
    }

    void Update()
    {
        if (blur < blurRoof)
        {
            blur += Time.deltaTime * 2;
            MaterialClass.SetFloat("_Size", blur);
        }
    }

    public void SetBlur(float num)
    {
        blur = num;
        MaterialClass.SetFloat("_Size", blur);
    }

}
