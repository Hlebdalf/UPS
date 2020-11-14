using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadialMenuButtons : MonoBehaviour
{
    public RadialMenuManager RadialMenuManagerClass;
    private int preangle = 9;

    void Start()
    {
        RadialMenuManagerClass = RadialMenuManagerClass.GetComponent<RadialMenuManager>();
    }
    void Update()
    {   
        Vector2 mousePositionVector = new Vector2(Input.mousePosition.x - Screen.width/2, Input.mousePosition.y - Screen.height / 2);
        //float angle = Mathf.Atan2(mousePositionVector.y, mousePositionVector.x) / (Mathf.PI / 180);
        int angle = (int)Mathf.Round((Mathf.Atan2(mousePositionVector.y, mousePositionVector.x) / (Mathf.PI / 180)) /45);
        //print(mousePositionVector.magnitude);
        if (angle != preangle)
        {
            preangle = angle;
            if (mousePositionVector.magnitude > 200)
            {
                RadialMenuManagerClass.SwitchButton(angle);
            }
            else
            {
                RadialMenuManagerClass.SwitchButton(9);
            }
        }
    }
}
