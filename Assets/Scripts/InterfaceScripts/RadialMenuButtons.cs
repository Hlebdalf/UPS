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

    void Start()
    {   
        RadialMenuManagerClass = RadialMenuManagerClass.GetComponent<RadialMenuManager>();
        ImageClass = gameObject.GetComponent<Image>();
    }
    void Update()
    {   
        Vector2 mousePositionVector = new Vector2(Input.mousePosition.x - Screen.width/2, Input.mousePosition.y - Screen.height / 2);
        int angle = (int)Mathf.Round((Mathf.Atan2(mousePositionVector.y, mousePositionVector.x) / (Mathf.PI / 180)) /45);
        print(angle);
        if (true)
        {
            if (mousePositionVector.magnitude > 200)
            {
                switch (angle+4)
                {
                    case 0:
                        RadialMenuManagerClass.SwitchButton(0);
                        ImageClass.sprite = PreSprites[0];
                        break;
                    case 1:
                        RadialMenuManagerClass.SwitchButton(1);
                        ImageClass.sprite = PreSprites[1];
                        break;
                    case 2:
                        RadialMenuManagerClass.SwitchButton(2);
                        ImageClass.sprite = PreSprites[2];
                        break;
                    case 3:
                        RadialMenuManagerClass.SwitchButton(3);
                        ImageClass.sprite = PreSprites[3];
                        break;
                    case 4:
                        RadialMenuManagerClass.SwitchButton(4);
                        ImageClass.sprite = PreSprites[4];
                        break;
                    case 5:
                        RadialMenuManagerClass.SwitchButton(5);
                        ImageClass.sprite = PreSprites[5];
                        break;
                    case 6:
                        RadialMenuManagerClass.SwitchButton(6);
                        ImageClass.sprite = PreSprites[6];
                        break;
                    case 7:
                        RadialMenuManagerClass.SwitchButton(7);
                        ImageClass.sprite = PreSprites[7];
                        break;
                }
            }
            else
            {
                RadialMenuManagerClass.SwitchButton(8);
                ImageClass.sprite = PreSprites[8];
            }
            
        }
    }
}
