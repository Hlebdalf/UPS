using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InterfaceButton : MonoBehaviour, IPointerClickHandler {
    private GameObject MainCamera;
    private Builds BuildsClass;
    private Roads RoadsClass;

    public string objectName;
    public string objectType;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        BuildsClass = MainCamera.GetComponent <Builds> ();
        RoadsClass = MainCamera.GetComponent <Roads> ();
    }
    
    private Vector3 GetMousePosition() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            return hit.point;
        }
        return new Vector3(0, 0, 0);
    }

    public void Init(string text, string name, string type) {
        objectType = type;
        gameObject.transform.Find("Text").GetComponent <Text> ().text = text;
        objectName = name;
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (objectType == "Road") RoadsClass.RoadType = "Road1";
        if (objectType == "House") BuildsClass.CreateGhost(objectName, GetMousePosition());
    }
}
