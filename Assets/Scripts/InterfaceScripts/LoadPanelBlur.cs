using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadPanelBlur : MonoBehaviour {
    private Material MaterialClass;
    
    public float blur = 0;

    private void Start() {
        Image ImageClass = gameObject.GetComponent <Image> ();
        MaterialClass = ImageClass.material;
    }

    private void FixedUpdate() {
        MaterialClass.SetFloat("_Size", blur);
    }
}
