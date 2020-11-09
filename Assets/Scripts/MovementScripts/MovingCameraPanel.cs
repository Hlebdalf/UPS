using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MovingCameraPanel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public CameraMoving CameraMovingClass;

    private void Start() {
        CameraMovingClass = CameraMovingClass.GetComponent <CameraMoving> ();
    }

    public void OnPointerEnter(PointerEventData eventData) {
        CameraMovingClass.SwitchDirection("Begin");
    }

    public void OnPointerExit(PointerEventData eventData) {
        CameraMovingClass.SwitchDirection("Finish");
    }
}
