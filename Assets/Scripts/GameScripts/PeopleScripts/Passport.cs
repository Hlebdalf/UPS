using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passport : MonoBehaviour {
    private GameObject MainCamera;
    private People PeopleClass;

    public string nameHuman = "Default";
    public string surname = "Default";
    public int age = -1; // 0 - 99
    public string gender = "Default";
    public string socialStatus = "Default";
    public int budget = 0;

    public int idxCommerceType;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        PeopleClass = MainCamera.GetComponent <People> ();
    }

    private void Start() {
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
}
