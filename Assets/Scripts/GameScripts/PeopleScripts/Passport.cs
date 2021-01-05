using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Passport : MonoBehaviour {
    private GameObject MainCamera;
    private People PeopleClass;
    private TextLoader TextLoaderClass;

    public string nameHuman = "Default";
    public string surnameHuman = "Default";
    public int age = -1; // 0 - 99
    public string gender = "Default";
    public string socialStatus = "Default";
    public int budget = 0;

    public int idxCommerceType;

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
            if (age < 65) {
                socialStatus = PeopleClass.socialStatusStorage[(int)UnityEngine.Random.Range(0, 4.99f)];
            }
            else {
                socialStatus = PeopleClass.socialStatusStorage[5];
            }
            budget = (int)UnityEngine.Random.Range(-10000000, 100000000);
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
        nameHuman = GetName();
        surnameHuman = GetSurname();
    }

    private string GetName() {
        string text = "";
        if (gender == "Муж") text = TextLoaderClass.LoadText("NamesMale");
        if (gender == "Жен") text = TextLoaderClass.LoadText("NamesFemale");
        if (text != "Error") {
            string[] names = text.Split(new char[] {' '}, System.StringSplitOptions.RemoveEmptyEntries);
            return names[(int)UnityEngine.Random.Range(0, names.Length - 0.01f)];
        }
        return nameHuman;
    }

    private string GetSurname() {
        string text = "";
        if (gender == "Муж") text = TextLoaderClass.LoadText("SurnamesMale");
        if (gender == "Жен") text = TextLoaderClass.LoadText("SurnamesFemale");
        if (text != "Error") {
            string[] surnames = text.Split(new char[] {' '}, System.StringSplitOptions.RemoveEmptyEntries);
            return surnames[(int)UnityEngine.Random.Range(0, surnames.Length - 0.01f)];
        }
        return surnameHuman;
    }
}
