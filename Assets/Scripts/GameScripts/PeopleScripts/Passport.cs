using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passport : MonoBehaviour {
    private GameObject MainCamera;
    private People PeopleClass;
    private TextLoader TextLoaderClass;

    public string nameHuman = "Default";
    public string surnameHuman = "Default";
    public string fatherNameHuman = "Default";
    public int age = -1; // 0 - 99
    public string gender = "Default";
    public string socialStatus = "Default";
    public int budget = 0;
    public int satisfaction = 0;

    public int idxCommerceType;
    public float dist;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        PeopleClass = MainCamera.GetComponent <People> ();
        TextLoaderClass = MainCamera.GetComponent <TextLoader> ();
    }

    private void Start() {
        if ((int)UnityEngine.Random.Range(0, 1.99f) == 0) gender = "Муж";
        else gender = "Жен";
        if (idxCommerceType == 0) {
            age = (int)UnityEngine.Random.Range(5, 99.99f);
            if (age < 7) socialStatus = PeopleClass.socialStatusStorage[0];
            else if (age < 18) socialStatus = PeopleClass.socialStatusStorage[1];
            else if (age < 23) socialStatus = PeopleClass.socialStatusStorage[2];
            else if (age < 65) socialStatus = PeopleClass.socialStatusStorage[(int)UnityEngine.Random.Range(3, 4.99f)];
            else socialStatus = PeopleClass.socialStatusStorage[5];
        }
        if (idxCommerceType == 1) {
            age = (int)UnityEngine.Random.Range(7, 23.99f);
            if (age <= 18) socialStatus = PeopleClass.socialStatusStorage[1];
            else socialStatus = PeopleClass.socialStatusStorage[2];
        }
        SetName();
        SetSurname();
        SetFatherName();
        SetBudget();
        SetSatisfaction();
    }
    
    private void SetSatisfaction() {
        float sat1 = (int)(10000 / dist) % 100;
        float sat2 = budget / 1000;
        if (sat2 > 50) sat2 = 50;
        else if (sat2 < -50) sat2 = -50;
        sat2 += 50;
        satisfaction = (int)((sat1 + sat2) / 2f);
    }

    private void SetBudget() {
        if (age < 7) budget = (int)UnityEngine.Random.Range(0, 1000);
        else if (age < 16) budget = (int)UnityEngine.Random.Range(0, 10000);
        else if (age < 18) budget = (int)UnityEngine.Random.Range(0, 100000);
        else if (age < 20) budget = (int)UnityEngine.Random.Range(-1000000, 1000000);
        else if (age < 23) budget = (int)UnityEngine.Random.Range(-10000000, 10000000);
        else budget = (int)UnityEngine.Random.Range(-10000000, 100000000);
    }

    private void SetName() {
        string text = "";
        if (gender == "Муж") text = TextLoaderClass.LoadText("NamesMale");
        if (gender == "Жен") text = TextLoaderClass.LoadText("NamesFemale");
        if (text != "Error") {
            string[] names = text.Split(new char[] {' '}, System.StringSplitOptions.RemoveEmptyEntries);
            nameHuman = names[(int)UnityEngine.Random.Range(0, names.Length - 0.01f)];
        }
    }

    private void SetSurname() {
        string text = "";
        if (gender == "Муж") text = TextLoaderClass.LoadText("SurnamesMale");
        if (gender == "Жен") text = TextLoaderClass.LoadText("SurnamesFemale");
        if (text != "Error") {
            string[] surnames = text.Split(new char[] {' '}, System.StringSplitOptions.RemoveEmptyEntries);
            surnameHuman = surnames[(int)UnityEngine.Random.Range(0, surnames.Length - 0.01f)];
        }
    }
    
    private void SetFatherName() {
        string text = "";
        if (gender == "Муж") text = TextLoaderClass.LoadText("FatherNamesMale");
        if (gender == "Жен") text = TextLoaderClass.LoadText("FatherNamesFemale");
        if (text != "Error") {
            string[] fatherNames = text.Split(new char[] {' '}, System.StringSplitOptions.RemoveEmptyEntries);
            fatherNameHuman = fatherNames[(int)UnityEngine.Random.Range(0, fatherNames.Length - 0.01f)];
        }
    }
}
