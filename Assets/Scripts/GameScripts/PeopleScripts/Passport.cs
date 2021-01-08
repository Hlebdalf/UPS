using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public List <string> preferences;
    public List <string> notPreferences;
    public List <string> properties;

    public int idxPreFub;
    public int idxCommerceType;
    public int idxSocialСlass;
    public float dist;

    private void Awake() {
        MainCamera = GameObject.FindGameObjectWithTag("MainCamera");
        PeopleClass = MainCamera.GetComponent <People> ();
        TextLoaderClass = MainCamera.GetComponent <TextLoader> ();
        preferences = new List <string> ();
        notPreferences = new List <string> ();
        properties = new List <string> ();
    }

    private void Start() {
        if ((int)UnityEngine.Random.Range(0, 1.99f) == 0) gender = "Муж";
        else gender = "Жен";
        
        SetGender();
        SetName();
        SetSurname();
        SetFatherName();
        SetSocialStatusAndAge();
        SetSocialClass();
        SetBudget();
        SetSatisfaction();
        SetPreference();
        SetProperties();
    }

    private void OnMouseDown() {
        PeopleClass.OpenPassport();
        Text txt = PeopleClass.PassportCard.GetComponent <Text> ();
        txt.text = "Дело №" + (PeopleClass.objects.IndexOf(gameObject) + 1) + "\n";
        if (age < 9) txt.text += "Октябрёнок ";
        else if (age <= 14) txt.text += "Пионер ";
        else if (age <= 28) txt.text += "Комсомолец ";
        else txt.text += "Товарищ ";
        txt.text += surnameHuman + " " + nameHuman + " " + fatherNameHuman + "\n";
        txt.text += "Пол: " + gender + ", Возраст: " + age + "\n";
        txt.text += "Социальный статус: " + socialStatus + "\n";
        if (age >= 18) txt.text += "Социальный класс: " + socialСlass + "\n";
        txt.text += "Удовлетворённость: " + satisfaction + "\n";
        if (age >= 14) txt.text += "Преданность партии: " + loyalty + "\n";
        if (age >= 5) txt.text += "Бюджет: " + budget + "\n";
        txt.text += "Предпочтения:\n";
        txt.text += " • Любит: ";
        for (int i = 0; i < preferences.Count; ++i) {
            txt.text += preferences[i];
            if (i + 1 == preferences.Count) txt.text += ".\n";
            else txt.text += "; ";
        }
        txt.text += " • Не любит: ";
        for (int i = 0; i < notPreferences.Count; ++i) {
            txt.text += notPreferences[i];
            if (i + 1 == notPreferences.Count) txt.text += ".\n";
            else txt.text += "; ";
        }
        txt.text += "Особенности:\n";
        for (int i = 0; i < properties.Count; ++i) {
            txt.text += " • " + properties[i];
            if (i + 1 == properties.Count) txt.text += ".";
            else txt.text += ";\n";
        }

    }

    private void SetGender() {
        if (idxPreFub == 0) gender = "Муж";
        if (idxPreFub == 1) gender = "Жен";
        if (idxPreFub == 2) gender = "Жен";
        if (idxPreFub == 3) gender = "Муж";
        if (idxPreFub == 4) gender = "Жен";
        if (idxPreFub == 5) gender = "Жен";
        if (idxPreFub == 6) gender = "Муж";
        if (idxPreFub == 7) gender = "Жен";
    }

    private void SetProperties() {
        string text = TextLoaderClass.LoadText("Properties");
        if (text != "Error") {
            string[] allProperties = text.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
            List <bool> used = new List <bool> ();
            for (int i = 0; i < allProperties.Length; ++i) used.Add(false);
            for (int i = 0; i < (int)UnityEngine.Random.Range(1, 5.99f); ++i) {
                int it = (int)UnityEngine.Random.Range(0, allProperties.Length - 0.1f);
                while (used[it]) it = (int)UnityEngine.Random.Range(0, allProperties.Length - 0.1f);
                used[it] = true;
                properties.Add(allProperties[it]);
            }
        }
    }

    private void SetPreference() {
        string text = TextLoaderClass.LoadText("Preferences");
        if (text != "Error") {
            string[] allPreferences = text.Split(new char[] {'\n'}, System.StringSplitOptions.RemoveEmptyEntries);
            List <bool> used = new List <bool> ();
            for (int i = 0; i < allPreferences.Length; ++i) used.Add(false);
            for (int i = 0; i < (int)UnityEngine.Random.Range(2, 5.99f); ++i) {
                int it = (int)UnityEngine.Random.Range(0, allPreferences.Length - 0.1f);
                while (used[it]) it = (int)UnityEngine.Random.Range(0, allPreferences.Length - 0.1f);
                used[it] = true;
                preferences.Add(allPreferences[it]);
            }
            for (int i = 0; i < (int)UnityEngine.Random.Range(1, 4.99f); ++i) {
                int it = (int)UnityEngine.Random.Range(0, allPreferences.Length - 0.1f);
                while (used[it]) it = (int)UnityEngine.Random.Range(0, allPreferences.Length - 0.1f);
                used[it] = true;
                notPreferences.Add(allPreferences[it]);
            }
        }
    }

    private void SetSocialClass() {
        if (idxSocialСlass == 1) socialСlass = "Пролетариат";
        if (idxSocialСlass == 2) socialСlass = "Бюрократы";
        if (idxSocialСlass == 3) socialСlass = "Интеллигенция";
        if (idxSocialСlass == 4) socialСlass = "Буржуи";
    }

    private void SetSocialStatusAndAge() {
        if (idxCommerceType == 0) {
            if (idxPreFub == 0 || idxPreFub == 1 || idxPreFub == 2) age = (int)UnityEngine.Random.Range(5, 16.99f);
            else if (idxPreFub == 3 || idxPreFub == 4 || idxPreFub == 5) age = (int)UnityEngine.Random.Range(16, 70.99f);
            else age = (int)UnityEngine.Random.Range(70, 99.99f);
            if (age < 7) socialStatus = PeopleClass.socialStatusStorage[0];
            else if (age < 18) socialStatus = PeopleClass.socialStatusStorage[1];
            else if (age < (int)UnityEngine.Random.Range(18, 27.99f)) socialStatus = PeopleClass.socialStatusStorage[2];
            else if (age < (int)UnityEngine.Random.Range(65, 70.99f)) socialStatus = PeopleClass.socialStatusStorage[(int)UnityEngine.Random.Range(3, 4.99f)];
            else socialStatus = PeopleClass.socialStatusStorage[5];
        }
        if (idxCommerceType == 1) {
            if (idxPreFub == 0 || idxPreFub == 1 || idxPreFub == 2) age = (int)UnityEngine.Random.Range(7, 16.99f);
            else age = (int)UnityEngine.Random.Range(16, 70.99f);
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
            if (idxPreFub == 0 || idxPreFub == 1 || idxPreFub == 2) age = (int)UnityEngine.Random.Range(5, 16.99f);
            else age = (int)UnityEngine.Random.Range(16, 70.99f);
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
        if (idxCommerceType == 6) {
            if (idxPreFub == 0 || idxPreFub == 1 || idxPreFub == 2) age = (int)UnityEngine.Random.Range(2, 7.99f);
            else age = (int)UnityEngine.Random.Range(30, 70.99f);
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
