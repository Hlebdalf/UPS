using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuManager : MonoBehaviour
{
    public int radialMenuDirection;
    private UIVisibilityScript UiVisibilityClass;
    void Start()
    {
        UiVisibilityClass = gameObject.GetComponent<UIVisibilityScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SwitchButton(int type)
    {
        radialMenuDirection = type;
        print(type);
    }
}
