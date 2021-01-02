using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIVisibilityScript : MonoBehaviour {
    private bool isActive = false;
    private bool isActive2 = false;

    public GameObject BuildPanel;
    public GameObject RadialPanel;
    public GameObject RadialImage;

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            isActive2 = !isActive2;
            RadialPanel.GetComponent <RadialMenuVisibility> ().radialMenuDisability = isActive2;
            RadialPanel.SetActive(isActive2);
            RadialPanel.GetComponent <Animator> ().SetBool("isOpened", isActive2);
            RadialImage.GetComponent <Animator> ().SetBool("isOpened", isActive2);
        }
        else if (Input.GetKeyUp(KeyCode.Space)) {   
            isActive2 = !isActive2;
            RadialImage.GetComponent <RadialMenuButtons> ().ReturnAngle();
            RadialPanel.GetComponent <Animator> ().SetBool("isOpened", isActive2);
            RadialImage.GetComponent <Animator> ().SetBool("isOpened", isActive2);
        }
    }

    public void SetBuildMenuActive() {
        isActive = !(isActive);
        BuildPanel.SetActive(isActive);
    }
}
