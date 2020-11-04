using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMooving : MonoBehaviour
{
    public float XMoovingMP;
    public float ZMoovingMP;
    public float Sensivity1;
    public float Sensivity2;
    private Vector3 MoovingVector = new Vector3(0, 0, 0);

    float Stepen(float a, int b)
    {
        float c = a;
        for (int i = 1; i < b; i++)
        {
            c = c * a;
        }
        return c;
    }
    public void SwitchDirection(string direction)
    {
        switch (direction)
        {
            case "Begin":
                MoovingVector = new Vector3(1, 0, 1);
                break;
            case "Finish":
                MoovingVector = new Vector3(0, 0, 0);
                break;
        }
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
            float MoovingDirectionX = MoovingVector.x * (Input.mousePosition.x - 980) / 2000;
            float MoovingDirectionZ = MoovingVector.z * (Input.mousePosition.y - 540)/1600;
            transform.position += new Vector3(MoovingDirectionX,0,MoovingDirectionZ);
            //print(MoovingDirectionX);
            print(MoovingDirectionX/160);
        }
    }
}
