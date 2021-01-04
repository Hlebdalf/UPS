using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class HumanObject : MonoBehaviour {
    private GameObject MainCamera;
    private People PeopleClass;
    private Vector3 vertexTo;
    private bool vertexIsActive = false;
    private int cntTranslate = 0;

    public Queue <Vector3> queuePoints;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        PeopleClass = MainCamera.GetComponent <People> ();
        queuePoints = new Queue <Vector3> ();
    }

    private void Start() {
        transform.position = queuePoints.Dequeue();
    }

    private void FixedUpdate() {
        if (vertexIsActive) {
            Vector3 vertexFrom = transform.position;
            float dist = (float)Math.Sqrt(Math.Pow(vertexTo.x - vertexFrom.x, 2) + Math.Pow(vertexTo.z - vertexFrom.z, 2));
            if (dist > PeopleClass.eps) {
                Vector3 move = new Vector3((vertexTo.x - vertexFrom.x) / dist, 0, (vertexTo.z - vertexFrom.z) / dist);
                transform.Find("Human").gameObject.transform.rotation = Quaternion.Euler(0, (float)Math.Atan2(vertexTo.z - vertexFrom.z, vertexTo.x - vertexFrom.x) * -57.3f + 90f, 0);
                transform.Translate(move * PeopleClass.speed * Time.fixedDeltaTime);
                if(dist < 0.1f) ++cntTranslate;
                if (cntTranslate > 10) vertexIsActive = false;
            }
            else vertexIsActive = false;
        }
        if (!vertexIsActive) {
            if (queuePoints.Count > 0) {
                vertexTo = queuePoints.Dequeue();
                vertexIsActive = true;
                cntTranslate = 0;
            }
            else {
                PeopleClass.DeleteObject(gameObject);
            }
        }
    }
}
