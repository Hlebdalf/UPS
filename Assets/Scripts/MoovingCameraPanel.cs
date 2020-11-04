using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoovingCameraPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CameraMooving CameraMoovingClass;


    void Start()
    {
        CameraMoovingClass = CameraMoovingClass.GetComponent<CameraMooving>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        CameraMoovingClass.SwitchDirection("Begin");
        print("Begin");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CameraMoovingClass.SwitchDirection("Finish");
        print("Finish");
    }

    void Update()
    {

    }
}
