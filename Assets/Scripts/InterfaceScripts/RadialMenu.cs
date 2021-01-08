using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour {
    private GameObject MainCamera;
    private People PeopleClass;
    private Builds BuildsClass;
    private Roads RoadsClass;
    private Interface InterfaceClass;
    private Image ImageClass;
    private int buttonNum = -1;

    public Sprite[] PreSprites;
    public GameObject RadialPanel;
    public GameObject RadialImage;
    public GameObject InterfaceObject;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        PeopleClass = MainCamera.GetComponent <People> ();
        BuildsClass = MainCamera.GetComponent <Builds> ();
        RoadsClass = MainCamera.GetComponent <Roads> ();
        InterfaceClass = InterfaceObject.GetComponent <Interface> ();
        ImageClass = RadialImage.GetComponent <Image> ();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space) && !RoadsClass.isFollowGhost && !BuildsClass.isFollowGhost) {
            PeopleClass.ClosePassport();
            RadialPanel.SetActive(true);
            InterfaceClass.DeactivateAllMenu();
            RadialPanel.GetComponent <Animator> ().SetBool("isOpened", true);
            RadialImage.GetComponent <Animator> ().SetBool("isOpened", true);
        }
        else if (Input.GetKeyUp(KeyCode.Space)) {
            SwitchButton();
            RadialPanel.GetComponent <Animator> ().SetBool("isOpened", false);
            RadialImage.GetComponent <Animator> ().SetBool("isOpened", false);
        }
    }

    private void FixedUpdate() {
        Vector2 centerToCursor = new Vector2(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2);
        if (centerToCursor.magnitude > 200) {
            buttonNum = ((int)Mathf.Round(Mathf.Atan2(centerToCursor.y, centerToCursor.x) * 57.3f / 45) + 4) % 8;
            ImageClass.sprite = PreSprites[buttonNum];
        }
        else {
            buttonNum = 8;
            ImageClass.sprite = PreSprites[buttonNum];
        }
    }

    private void SwitchButton() {
        switch (buttonNum) {
            case 0:
                // print("0");
                break;
            case 1:
                // print("1");
                break;
            case 2:
                InterfaceClass.ActivateMenu("Other");
                break;
            case 3:
                InterfaceClass.ActivateMenu("Roads");
                break;
            case 4:
                InterfaceClass.ActivateMenu("Commerces");
                break;
            case 5:
                InterfaceClass.ActivateMenu("Builds");
                break;
            case 6:
                gameObject.GetComponent<InfoScript>().SetActivity();
                break;
            case 7:
                // print("7");
                break;
            case 8:
                InterfaceClass.DeactivateAllMenu();
                break;
        }
    }
}
