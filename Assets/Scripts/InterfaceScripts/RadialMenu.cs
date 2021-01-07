﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadialMenu : MonoBehaviour {
    private Image ImageClass;
    private int buttonNum = -1;

    public Sprite[] PreSprites;
    public GameObject RadialPanel;
    public GameObject RadialImage;

    private void Start() {   
        ImageClass = RadialImage.GetComponent <Image> ();
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            RadialPanel.SetActive(true);
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
                // print("2");
                break;
            case 3:
                // print("3");
                break;
            case 4:
                // print("4");
                break;
            case 5:
                // print("5");
                break;
            case 6:
                // print("6");
                break;
            case 7:
                // print("7");
                break;
            case 8:
                // print("8");
                break;
        }
    }
}
