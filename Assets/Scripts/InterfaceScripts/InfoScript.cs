using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoScript : MonoBehaviour
{
    private bool isActivity = true;
    private bool isBusy = false;
    public GameObject PanelClass;
    public void SetActivity()
    {
        PanelClass.SetActive(true);
        isActivity = true;
    }
    public void SetDisability()
    {
        PanelClass.SetActive(false);
        isActivity = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1) && !isBusy )
        {   
            isBusy = true;
            SwitchActivity();
        }
        else isBusy = false;
    }

    private void SwitchActivity()
    {
        if (isActivity)
        {
            SetDisability();
        }
        else
        {
            SetActivity();
        }
    }
}
