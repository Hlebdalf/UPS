using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuManager : MonoBehaviour
{
    public int radialMenuButton;
    private UIVisibilityScript UiVisibilityClass;
    void Start()
    {
        UiVisibilityClass = gameObject.GetComponent<UIVisibilityScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchButton(int num)
    {
        switch (num)
        {
            case 0:
                print(0);
                break;
            case 1:
                print(1);
                break;
            case 2:
                print(2);
                break;
            case 3:
                UiVisibilityClass.SetBuildMenuActive();
                print("3");
                break;
            case 4:
                print(4);
                break;
            case 5:
                print(5);
                break;
            case 6:
                print(6);
                break;
            case 7:
                print(7);
                break;
            case 8:
                print(8);
                break;
            case 9:
                print("None");
                break;
        }
    }
}
