using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HumanObject : MonoBehaviour {
    private GameObject MainCamera;
    private People PeopleClass;
    private Vector3 vertexTo;
    private bool vertexIsActive = false;
    private int cntTranslate = 0;

    private string nameHuman = "Default";
    private string surname = "Default";
    private int age = -1; // 0 - 99
    private string gender = "Default"; //Муж или Жен
    private string socialStatus = "Default"; // Например: ребенок, школьник, студент, работающий человек, безработный, пенсионер
    private int budget = 0;

    public Queue <Vector3> queuePoints;
    public int idxCommerceType;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        PeopleClass = MainCamera.GetComponent <People> ();
        queuePoints = new Queue <Vector3> ();
    }

    private void Start() {
        transform.position = queuePoints.Dequeue();
        if ((int)UnityEngine.Random.Range(0, 1.99f) == 0) gender = "Муж";
        else gender = "Жен";
        if (idxCommerceType == 0) {
            age = (int)UnityEngine.Random.Range(18, 99.99f);
            if (age < 65) {
                socialStatus = PeopleClass.socialStatusStorage[(int)UnityEngine.Random.Range(3, 4.99f)];
            }
            else {
                socialStatus = PeopleClass.socialStatusStorage[5];
            }
            budget = (int)UnityEngine.Random.Range(-1000000, 1000000);
        }
        if (idxCommerceType == 1) {
            age = (int)UnityEngine.Random.Range(7, 23.99f);
            if (age <= 18) {
                socialStatus = PeopleClass.socialStatusStorage[1];
                budget = (int)UnityEngine.Random.Range(0, 1000);
            }
            else {
                socialStatus = PeopleClass.socialStatusStorage[2];
                budget = (int)UnityEngine.Random.Range(0, 10000);
            }
        }
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

    private void OnMouseDown() {
        print("Check");
        Text txt = PeopleClass.PassportCard.GetComponent <Text> ();
        txt.text = nameHuman + " " + surname + "\n" + gender + ", " + age + "\n" + "Социальный статус: " + socialStatus + "\n" + "Бюджет: " + budget;
    }
}
