using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIVisibilityScript : MonoBehaviour {
    public GameObject BuildPanel;
    private bool isActive = false;

    public void SetBuildMenuActive() {
        isActive = !(isActive);
        BuildPanel.SetActive(isActive);
    }
}
