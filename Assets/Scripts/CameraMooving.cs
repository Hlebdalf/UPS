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
    private bool IsMooving = false;

    

    public void SwitchDirection(string direction)
    {
        switch (direction)
        {
            case "Begin":
                MoovingVector = new Vector3(1, 0, 1);
                IsMooving = true;
                break;
            case "Finish":
                IsMooving = false;
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
        else if (IsMooving)
        {
            float x = Input.mousePosition.x;
            float y = Input.mousePosition.y;
            float MoovingDirectionX = MoovingVector.x * (x - 980)/16f*9f /1500 ;
            float MoovingDirectionZ = MoovingVector.z * (y - 540) / 1500;

            transform.position += new Vector3(MoovingDirectionX / Sensivity1, 0, MoovingDirectionZ / Sensivity2);


            print(MoovingDirectionX/160);
        }
    }
}
