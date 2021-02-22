using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HouseMenu : MonoBehaviour {
    private GameObject MainCamera;
    private GameObject BuildObjectObject = null;
    private BuildObject BuildObjectClass = null;
    private RectTransform rectTransfrom;
    private float buildSz = 1;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        rectTransfrom = gameObject.GetComponent <RectTransform> ();
    }

    private void Start() {
        float szX = BuildObjectObject.GetComponent <BoxCollider> ().size.x * BuildObjectObject.transform.localScale.x;
        float szZ = BuildObjectObject.GetComponent <BoxCollider> ().size.z * BuildObjectObject.transform.localScale.x;
        buildSz = Math.Max(szX, szZ);
    }

    private void FixedUpdate() {
        if (BuildObjectClass) {
            Vector3 newPos = new Vector3(BuildObjectClass.x, BuildObjectObject.GetComponent <BoxCollider> ().size.y * BuildObjectObject.transform.localScale.x, BuildObjectClass.y);
            double dist = Math.Max(Math.Sqrt(Math.Pow(newPos.x - MainCamera.transform.position.x, 2) + Math.Pow(newPos.y - MainCamera.transform.position.y, 2) +
                                                Math.Pow(newPos.z - MainCamera.transform.position.z, 2)), 1);
            Vector3 newPosToScreen = Camera.main.WorldToScreenPoint(newPos);
            newPosToScreen.x -= buildSz * (float)(750 / dist);
            gameObject.transform.position = newPosToScreen;
            rectTransfrom.sizeDelta = new Vector2(BuildObjectClass.startMenuSizeW, BuildObjectClass.startMenuSizeH) * (float)(50 / dist);
        }
    }

    public void ActivateMenu(GameObject BuildObject) {
        BuildObjectObject = BuildObject;
        BuildObjectClass = BuildObject.GetComponent <BuildObject> ();
    }

    public void DestroyMenu() {
        BuildObjectClass.DestroyMenu();
    }

    public void AddPoster() {
        BuildObjectClass.AddPoster();
    }
}
