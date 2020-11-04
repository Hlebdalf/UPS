using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMooving : MonoBehaviour
{
    public float XMoovingMP;
    public float ZMoovingMP;
    private Vector3 XMoovingVector = new Vector3(0, 0, 0);
    private Vector3 ZMoovingVector = new Vector3(0, 0, 0);

    float XSpeedMultiplyer(float x)
    {
        if (x > 960)
        {
            return Mathf.Pow((x -1720) / 200, 2);
        }
        else
        {
            return Mathf.Pow((200 - x) / 200, 2);
        }
    }

    float ZSpeedMultiplyer(float z)
    {
        if (z > 540)
        {
            return Mathf.Pow((z - 880) / 200, 2);
        }
        else
        {
            return Mathf.Pow((200 - z) / 200, 2);
        }
    }

    public void SwitchDirection(string direction)
    {
        switch (direction)
        {
            case "Left":
                XMoovingVector = new Vector3(-XMoovingMP, 0, 0);
                break;
            case "Right":
                XMoovingVector = new Vector3 (XMoovingMP, 0, 0);
                break;
            case "Forward":
                ZMoovingVector = new Vector3(0, 0, ZMoovingMP);
                break;
            case "Back":
                ZMoovingVector = new Vector3(0, 0, -ZMoovingMP);
                break;
            /*case "Forward-Left":
                ZMoovingVector = new Vector3(-XMoovingMP, 0, ZMoovingMP);
                break;
            case "Back-Left":
                ZMoovingVector = new Vector3(-XMoovingMP, 0, -ZMoovingMP);
                break;
            case "Back-Right":
                ZMoovingVector = new Vector3(XMoovingMP, 0, -ZMoovingMP);
                break;
            case "Forward-Right":
                ZMoovingVector = new Vector3(XMoovingMP, 0, ZMoovingMP);
                break;*/
            case "None":
                XMoovingVector = new Vector3(0, 0, 0);
                ZMoovingVector = new Vector3(0, 0, 0);
                break;
        }
    }
    void Start()
    {

    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            if (Input.GetKey(KeyCode.W))
            {
                transform.position += new Vector3(0, 0, ZMoovingMP);
            }
            if (Input.GetKey(KeyCode.S))
            {
                transform.position += new Vector3(0, 0, -ZMoovingMP);
            }
            if (Input.GetKey(KeyCode.A))
            {
                transform.position += new Vector3(-XMoovingMP, 0, 0);
            }
            if (Input.GetKey(KeyCode.D))
            {
                transform.position += new Vector3(XMoovingMP, 0, 0);
            }
        }
        else
        {
            transform.position += XMoovingVector * XSpeedMultiplyer(Input.mousePosition.x);
            transform.position += ZMoovingVector * ZSpeedMultiplyer(Input.mousePosition.y);
        }
    }
}
