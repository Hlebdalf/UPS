using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Passport : MonoBehaviour {
    private GameObject MainCamera;
    private People PeopleClass;
    private Field FieldClass;
    private TextLoader TextLoaderClass;

    public string nameHuman = "Default";
    public string surnameHuman = "Default";
    public string fatherNameHuman = "Default";
    public int age = -1; // 0 - 99
    public string gender = "Default";
    public string socialStatus = "Default";
    public int budget = 0;
    public int satisfaction = 0;
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
        FieldClass = MainCamera.GetComponent <Field> ();
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
        WriteSpec();
    }

    private (int, List <string>) GetLoyalty() {
        // Зависимость от количества плакатов, удовлетворённости, числа жителей выше по статусу и т.д.
        List <string> reasonsLoyalty = new List <string> ();
        double loyalty = 0, envy = 0;
        int posX = (int)gameObject.transform.position.x, posZ = (int)gameObject.transform.position.z;
        int radius = 20, cntPosters = 0, cntPostersMax = 0;

        if (satisfaction < 60) reasonsLoyalty.Add("Удовлетворённость: " + (int)satisfaction + " < 60%");

        if (idxSocialСlass == 1) { // Пролетариат
            double ratioWith3, ratioWith4;
            if (PeopleClass.cntPeople3 == 0) ratioWith3 = 0;
            else ratioWith3 = PeopleClass.cntPeople1 / PeopleClass.cntPeople3; // От 2 до 0.5
            if (PeopleClass.cntPeople4 == 0) ratioWith4 = 0;
            else ratioWith4 = PeopleClass.cntPeople1 / PeopleClass.cntPeople4; // От 5 до 2
            double envy1 = 0, envy2 = 0;
            if (ratioWith3 < 2 && ratioWith3 >= 0.5) envy1 = (0.67 / ratioWith3 - 0.335) * 100;
            else if (ratioWith3 < 0.5) envy1 = 100;
            if (ratioWith4 < 5 && ratioWith4 >= 2) envy2 = (3.36 / ratioWith4 - 0.672) * 100;
            else if (ratioWith4 < 2) envy2 = 100;
            envy = (envy1 + envy2) / 2;
            if (envy1 > 40) reasonsLoyalty.Add("Зависть интеллигенции: " + (int)envy1 + " > 40 пунктов");
            if (envy2 > 40) reasonsLoyalty.Add("Зависть буржуям: " + (int)envy2 + " > 40 пунктов");
        }
        if (idxSocialСlass == 2) { // Бюрократия
            double ratioWith4;
            if (PeopleClass.cntPeople4 == 0) ratioWith4 = 0;
            else ratioWith4 = PeopleClass.cntPeople2 / PeopleClass.cntPeople4; // От 2 до 0.5
            if (ratioWith4 < 2 && ratioWith4 >= 0.5) envy = (0.67 / ratioWith4 - 0.335) * 100;
            else if (ratioWith4 < 0.5) envy = 100;
            if (envy > 40) reasonsLoyalty.Add("Зависть буржуям: " + (int)envy + " > 40 пунктов");
        }

        for (int x = posX - radius; x < posX + radius; ++x) {
            for (int z = posZ - radius; z < posZ + radius; ++z) {
                if (x + FieldClass.fieldSizeHalf >= 0 &&
                    z + FieldClass.fieldSizeHalf < FieldClass.fieldSize &&
                    FieldClass.objects[x + FieldClass.fieldSizeHalf, z + FieldClass.fieldSizeHalf] != null &&
                    FieldClass.objects[x + FieldClass.fieldSizeHalf, z + FieldClass.fieldSizeHalf].GetComponent <BuildObject> ()) {
                    cntPosters += FieldClass.objects[x + FieldClass.fieldSizeHalf, z + FieldClass.fieldSizeHalf].GetComponent <BuildObject> ().cntPosters;
                    cntPostersMax += FieldClass.objects[x + FieldClass.fieldSizeHalf, z + FieldClass.fieldSizeHalf].GetComponent <BuildObject> ().cntPosters;
                }
            }
        }
        if (cntPosters / Math.Max(cntPostersMax, 1) * 100 < 50) reasonsLoyalty.Add("Кол-во плакатов в данной области: " + cntPosters + " < 50% от макс. кол-ва");

        loyalty = (2 * satisfaction + (101 - envy) + 3 * (cntPosters / Math.Max(cntPostersMax, 1) * 100)) / 6;
        return ((int)loyalty, reasonsLoyalty);
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
                int it = (int)UnityEngine.Random.Range(0, allProperties.Length - 0.01f);
                while (used[it]) it = (int)UnityEngine.Random.Range(0, allProperties.Length - 0.01f);
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
                int it = (int)UnityEngine.Random.Range(0, allPreferences.Length - 0.01f);
                while (used[it]) it = (int)UnityEngine.Random.Range(0, allPreferences.Length - 0.01f);
                used[it] = true;
                preferences.Add(allPreferences[it]);
            }
            for (int i = 0; i < (int)UnityEngine.Random.Range(1, 4.99f); ++i) {
                int it = (int)UnityEngine.Random.Range(0, allPreferences.Length - 0.01f);
                while (used[it]) it = (int)UnityEngine.Random.Range(0, allPreferences.Length - 0.01f);
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

    public void WriteSpec() {
        Text txt = PeopleClass.PassportCard.GetComponent <Text> ();
        txt.text = "Дело №" + (PeopleClass.objects.IndexOf(gameObject) + 1) + "\n";
        if (age < 9) txt.text += "Октябрёнок ";
        else if (age <= 14) txt.text += "Пионер ";
        else if (age <= 28) txt.text += "Комсомолец ";
        else txt.text += "Товарищ ";
        txt.text += surnameHuman + " " + nameHuman + " " + fatherNameHuman + "\n";
        txt.text += "Пол: " + gender + ", Возраст: " + age + "\n";
        if (age >= 5) txt.text += "Бюджет: " + budget + " руб.\n";
        txt.text += "\nСоциальный статус: " + socialStatus + "\n";
        if (age >= 18) txt.text += "Социальный класс: " + socialСlass + "\n";
        txt.text += "Удовлетворённость: " + satisfaction + "%\n";
        if (age >= 18) {
            (int loyalty, List <string> reasonsLoyalty) loyaltyData = GetLoyalty();
            txt.text += "Преданность партии: " + loyaltyData.loyalty + "%\n";
            for (int i = 0; i < loyaltyData.reasonsLoyalty.Count; ++i) {
                txt.text += " • " + loyaltyData.reasonsLoyalty[i] + "\n";
            }
        }
        txt.text += "\nПредпочтения:\n";
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
        txt.text += "Особенности: ";
        for (int i = 0; i < properties.Count; ++i) {
            txt.text += properties[i];
            if (i + 1 == properties.Count) txt.text += ".";
            else txt.text += "; ";
        }
    }
}
