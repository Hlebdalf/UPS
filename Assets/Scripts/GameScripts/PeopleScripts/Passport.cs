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
    public int loyalty = 0;
    public string socialСlass = "Default";

    public int idxCommerceType;
    public int idxSocialСlass;
    public float dist;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        PeopleClass = MainCamera.GetComponent <People> ();
        TextLoaderClass = MainCamera.GetComponent <TextLoader> ();
    }

    private void Start() {
        if ((int)UnityEngine.Random.Range(0, 1.99f) == 0) gender = "Муж";
        else gender = "Жен";
        
        SetName();
        SetSurname();
        SetFatherName();
        SetSocialStatusAndAge();
        SetSocialClass();
        SetBudget();
        SetSatisfaction();
    }

    private void SetSocialClass() {
        if (idxSocialСlass == 1) socialСlass = "Пролетариат";
        if (idxSocialСlass == 2) socialСlass = "Бюрократы";
        if (idxSocialСlass == 3) socialСlass = "Интеллигенция";
        if (idxSocialСlass == 4) socialСlass = "Буржуи";
    }

    private void SetSocialStatusAndAge() {
        if (idxCommerceType == 0) {
            age = (int)UnityEngine.Random.Range(5, 99.99f);
            if (age < 7) socialStatus = PeopleClass.socialStatusStorage[0];
            else if (age < 18) socialStatus = PeopleClass.socialStatusStorage[1];
            else if (age < (int)UnityEngine.Random.Range(18, 27.99f)) socialStatus = PeopleClass.socialStatusStorage[2];
            else if (age < (int)UnityEngine.Random.Range(65, 70.99f)) socialStatus = PeopleClass.socialStatusStorage[(int)UnityEngine.Random.Range(3, 4.99f)];
            else socialStatus = PeopleClass.socialStatusStorage[5];
        }
        if (idxCommerceType == 1) {
            age = (int)UnityEngine.Random.Range(7, 70.99f);
            if (age <= 18) socialStatus = PeopleClass.socialStatusStorage[1];
            else if (age < 30) {
                age = (int)UnityEngine.Random.Range(30, 70.99f);
                socialStatus = PeopleClass.socialStatusStorage[3];
            }
            else socialStatus = PeopleClass.socialStatusStorage[3];
        }
        if (idxCommerceType == 2) {
            age = (int)UnityEngine.Random.Range(18, 70.99f);
            if (age <= 23) socialStatus = PeopleClass.socialStatusStorage[2];
            else if (age < 30) {
                age = (int)UnityEngine.Random.Range(30, 70.99f);
                socialStatus = PeopleClass.socialStatusStorage[3];
            }
            else socialStatus = PeopleClass.socialStatusStorage[3];
        }
        if (idxCommerceType == 3) {
            age = (int)UnityEngine.Random.Range(23, 70.99f);
            if (age <= (int)UnityEngine.Random.Range(25, 27.99f)) socialStatus = PeopleClass.socialStatusStorage[2];
            else socialStatus = PeopleClass.socialStatusStorage[3];
        }
        if (idxCommerceType == 4) {
            age = (int)UnityEngine.Random.Range(5, 70.99f);
            if (age < 7) socialStatus = PeopleClass.socialStatusStorage[0];
            else if (age < 18) socialStatus = PeopleClass.socialStatusStorage[1];
            else if (age < 30) {
                age = (int)UnityEngine.Random.Range(30, 70.99f);
                socialStatus = PeopleClass.socialStatusStorage[3];
            }
            else socialStatus = PeopleClass.socialStatusStorage[3];
        }
        if (idxCommerceType == 5) {
            age = (int)UnityEngine.Random.Range(18, 70.99f);
            if (age < (int)UnityEngine.Random.Range(65, 70.99f)) socialStatus = PeopleClass.socialStatusStorage[3];
            else socialStatus = PeopleClass.socialStatusStorage[5];
        }
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
        if (age < 7) {
            if (idxSocialСlass == 1) budget = (int)UnityEngine.Random.Range(0, 10);
            if (idxSocialСlass == 2) budget = (int)UnityEngine.Random.Range(0, 100);
            if (idxSocialСlass == 3) budget = (int)UnityEngine.Random.Range(0, 1000);
            if (idxSocialСlass == 4) budget = (int)UnityEngine.Random.Range(0, 10000);
        }
        else if (age < 16) {
            if (idxSocialСlass == 1) budget = (int)UnityEngine.Random.Range(0, 100);
            if (idxSocialСlass == 2) budget = (int)UnityEngine.Random.Range(0, 1000);
            if (idxSocialСlass == 3) budget = (int)UnityEngine.Random.Range(0, 10000);
            if (idxSocialСlass == 4) budget = (int)UnityEngine.Random.Range(0, 100000);
        }
        else if (age < 18) {
            if (idxSocialСlass == 1) budget = (int)UnityEngine.Random.Range(0, 1000);
            if (idxSocialСlass == 2) budget = (int)UnityEngine.Random.Range(0, 10000);
            if (idxSocialСlass == 3) budget = (int)UnityEngine.Random.Range(0, 100000);
            if (idxSocialСlass == 4) budget = (int)UnityEngine.Random.Range(0, 1000000);
        }
        else if (age < 23) {
            if (idxSocialСlass == 1) budget = (int)UnityEngine.Random.Range(-1000, 10000);
            if (idxSocialСlass == 2) budget = (int)UnityEngine.Random.Range(-10000, 100000);
            if (idxSocialСlass == 3) budget = (int)UnityEngine.Random.Range(-100000, 1000000);
            if (idxSocialСlass == 4) budget = (int)UnityEngine.Random.Range(-1000000, 10000000);
        }
        else {
            if (idxSocialСlass == 1) budget = (int)UnityEngine.Random.Range(-10000, 100000);
            if (idxSocialСlass == 2) budget = (int)UnityEngine.Random.Range(-100000, 1000000);
            if (idxSocialСlass == 3) budget = (int)UnityEngine.Random.Range(-1000000, 10000000);
            if (idxSocialСlass == 4) budget = (int)UnityEngine.Random.Range(-10000000, 100000000);
        }
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
