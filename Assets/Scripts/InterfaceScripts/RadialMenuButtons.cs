using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class RadialMenuButtons : MonoBehaviour
{
    public Sprite[] PreSprites;
    public RadialMenuManager RadialMenuManagerClass;
    private List<Sprite> Sprites;
    private Image ImageClass;
    private int angle;

    void Start()
    {   
        RadialMenuManagerClass = RadialMenuManagerClass.GetComponent<RadialMenuManager>();
        ImageClass = gameObject.GetComponent<Image>();
    }
    void FixedUpdate()
    {   
        Vector2 mousePositionVector = new Vector2(Input.mousePosition.x - Screen.width/2, Input.mousePosition.y - Screen.height / 2);
        angle = (int)Mathf.Round((Mathf.Atan2(mousePositionVector.y, mousePositionVector.x) / (Mathf.PI / 180)) /45)+4;
        if (mousePositionVector.magnitude > 200)
        {
            ImageClass.sprite = PreSprites[angle];
            
        }
        else
        {
            angle = 8;
            ImageClass.sprite = PreSprites[angle];
        }
    }

    public void ReturnAngle()
    {
        RadialMenuManagerClass.SwitchButton(angle);
    }
}
