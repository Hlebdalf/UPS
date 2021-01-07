using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIVisibilityScript : MonoBehaviour {
    private bool isActive = false;

    public GameObject BuildPanel;

    public void SetBuildMenuActive() {
        isActive = !(isActive);
        BuildPanel.SetActive(isActive);
    }
}
