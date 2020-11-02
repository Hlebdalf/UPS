﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using UnityEngine.EventSystems;
public class UIVisibilityScript : MonoBehaviour
{
    public GameObject BuildPanel;
    private bool isActive = false;

    void Start()
    {
        
    }

    public void SetBuildMenuActive()
    {
        isActive = !(isActive);
        BuildPanel.SetActive(isActive);
    }

}
