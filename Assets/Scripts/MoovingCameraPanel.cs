using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MoovingCameraPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public CameraMooving CameraMoovingClass;

    private void Start() {
        CameraMoovingClass = CameraMoovingClass.GetComponent <CameraMooving> ();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        CameraMoovingClass.SwitchDirection("Begin");
    }

    public void OnPointerExit(PointerEventData eventData) {
        CameraMoovingClass.SwitchDirection("Finish");
    }
}
