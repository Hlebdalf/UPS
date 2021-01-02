using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenuVisibility : MonoBehaviour {
    private Material MaterialClass;

    public float blur = 0;
    public bool radialMenuDisability = true;
    
    private void Start() {
        Image ImageClass = gameObject.GetComponent <Image> ();
        MaterialClass = ImageClass.material;
    } 

    private void FixedUpdate() {
        MaterialClass.SetFloat("_Size", blur);
        gameObject.SetActive(radialMenuDisability);
    }

}
