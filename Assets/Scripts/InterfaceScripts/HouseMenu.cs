using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseMenu : MonoBehaviour {
    private GameObject BuildObjectObject = null;
    private BuildObject BuildObjectClass = null;

    private void Update() {
        if (BuildObjectClass) {
            Vector3 newPos = new Vector3(BuildObjectClass.x, BuildObjectObject.GetComponent <BoxCollider> ().size.y * 0.1f, BuildObjectClass.y);
            gameObject.transform.position = Camera.main.WorldToScreenPoint(newPos);
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
