using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMoovingRight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CameraMooving CameraMoovingClass;


    void Start()
    {
        CameraMoovingClass = CameraMoovingClass.GetComponent<CameraMooving>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        CameraMoovingClass.SwitchDirection("Right");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CameraMoovingClass.SwitchDirection("None");
    }

    void Update()
    {

    }
}
