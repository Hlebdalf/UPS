using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class HumanObject : MonoBehaviour {
    private GameObject MainCamera;
    private People PeopleClass;
    private Passport PassportClass;
    private Vector3 vertexTo;
    private bool vertexIsActive = false;
    private int cntTranslate = 0;

    public Queue <Vector3> queuePoints;
    public Vector3 move;
    public float angle;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        PeopleClass = MainCamera.GetComponent <People> ();
        queuePoints = new Queue <Vector3> ();
    }

    private void Start() {
        transform.position = queuePoints.Dequeue();
        PassportClass = gameObject.GetComponent <Passport> ();
    }

    private void FixedUpdate() {
        if (vertexIsActive) {
            Vector3 vertexFrom = transform.position;
            float dist = (float)Math.Sqrt(Math.Pow(vertexTo.x - vertexFrom.x, 2) + Math.Pow(vertexTo.z - vertexFrom.z, 2));
            if (dist > PeopleClass.eps) {
                move = new Vector3((vertexTo.x - vertexFrom.x) / dist, 0, (vertexTo.z - vertexFrom.z) / dist) * PeopleClass.speed * Time.fixedDeltaTime;
                angle = (float)Math.Atan2(vertexTo.z - vertexFrom.z, vertexTo.x - vertexFrom.x) * -57.3f + 90f;
                // transform.Find("Human").gameObject.transform.rotation = Quaternion.Euler(0, (float)Math.Atan2(vertexTo.z - vertexFrom.z, vertexTo.x - vertexFrom.x) * -57.3f + 90f, 0);
                // transform.Translate(move * PeopleClass.speed * Time.fixedDeltaTime);
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
        Text txt = PeopleClass.PassportCard.GetComponent <Text> ();
        txt.text = "Дело №" + (PeopleClass.objects.IndexOf(gameObject) + 1) + "\n";
        if (PassportClass.age < 9) txt.text += "Октябрёнок ";
        if (PassportClass.age <= 14) txt.text += "Пионер ";
        if (PassportClass.age <= 28) txt.text += "Комсомолец ";
        else txt.text += "Товарищ ";
        txt.text += PassportClass.surnameHuman + " " + PassportClass.nameHuman + " " + PassportClass.fatherNameHuman + "\n";
        txt.text += "Пол: " + PassportClass.gender + ", Возраст: " + PassportClass.age + "\n";
        txt.text += "Социальный статус: " + PassportClass.socialStatus + "\n";
        if (PassportClass.age >= 18) txt.text += "Социальный класс: " + PassportClass.socialСlass + "\n";
        txt.text += "Удовлетворённость: " + PassportClass.satisfaction + "\n";
        if (PassportClass.age >= 14) txt.text += "Преданность партии: " + PassportClass.loyalty + "\n";
        if (PassportClass.age >= 5) txt.text += "Бюджет: " + PassportClass.budget + "\n";
        txt.text += "Предпочтения: \n";
        txt.text += " • Любит: ";
        for (int i = 0; i < PassportClass.preferences.Count; ++i) {
            txt.text += PassportClass.preferences[i];
            if (i + 1 == PassportClass.preferences.Count) txt.text += ".\n";
            else txt.text += ", ";
        }
        txt.text += " • Не любит: ";
        for (int i = 0; i < PassportClass.notPreferences.Count; ++i) {
            txt.text += PassportClass.notPreferences[i];
            if (i + 1 == PassportClass.notPreferences.Count) txt.text += ".\n";
            else txt.text += ", ";
        }
    }
}
