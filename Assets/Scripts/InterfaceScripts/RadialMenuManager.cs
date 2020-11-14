using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuManager : MonoBehaviour
{
    public int radialMenuDirection = 8;
    private UIVisibilityScript UiVisibilityClass;
    void Update() {
    }
    void Start()
    {
        UiVisibilityClass = gameObject.GetComponent<UIVisibilityScript>();
    }

    public void SwitchButton(int type)
    {
        radialMenuDirection = type;
        /*switch (type)
        {
            case 0:
                //some logic
                break;
            case 1:
                //some logic
                break;
            case 2:
                //some logic
                break;
            case 3:
                //some logic
                break;
            case 4:
                //some logic
                break;
            case 5:
                //some logic
                break;
            case 6:
                //some logic
                break;
            case 7:
                //some logic
                break;
            case 8:
                //some logic
                break;
        }*/
    }
}
