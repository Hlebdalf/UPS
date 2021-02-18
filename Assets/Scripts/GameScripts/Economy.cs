using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Economy : MonoBehaviour {
    private Field FieldClass;
    private Builds BuildsClass;
    private People PeopleClass;

    private InputField CityName;
    private Text Level;
    private Text CntPeople;
    private Text Money;

    private string cityName = "";
    private int level = 0;
    private int cntPeople = 0, cntPeoplePerDay = 0;
    private long money = 0, optMoney = 0, moneyPerDay = 0;
    private long science = 0, sciencePerDay = 0;
    private long products = 0, productsPerDay = 0;
    private float averageLoyality = 0;
    private float HCS = 0, PIT = 0, VAT = 0, CIT = 0; // Коэффиценты налогов
    private bool isStarted = false;
    // private float deltaTime = 0.1f;
    
    private List <int> cntHousesD;
    private List <int> cntShopsD;
    private List <int> cntSciencesD;
    private List <int> cntFactoriesD;
    private List <int> cntPeopleD;
    private List <int> cntPostersD;
    private List <int> averageLoyalityD;
    private List <long> moneyPerDayD;
    private List <long> sciencePerDayD;
    private List <long> productsPerDayD;

    public GameObject fastStats;

    private void Awake() {
        FieldClass = Camera.main.GetComponent <Field> ();
        BuildsClass = Camera.main.GetComponent <Builds> ();
        PeopleClass = Camera.main.GetComponent <People> ();
        CityName = fastStats.transform.Find("CityName").gameObject.GetComponent <InputField> ();
        Level = fastStats.transform.Find("Level").gameObject.GetComponent <Text> ();
        CntPeople = fastStats.transform.Find("CntPeople").gameObject.GetComponent <Text> ();
        Money = fastStats.transform.Find("Money").gameObject.GetComponent <Text> ();
        cntHousesD = new List <int> () {0, 0, 0, 0};
        cntShopsD = new List <int> () {0, 0, 0, 0};
        cntSciencesD = new List <int> () {0, 0, 0, 0};
        cntFactoriesD = new List <int> () {0, 0, 0, 0};
        cntPeopleD = new List <int> () {0, 0, 0, 0};
        cntPostersD = new List <int> () {0, 0, 0, 0};
        averageLoyalityD = new List <int> () {0, 0, 0, 0};
        moneyPerDayD = new List <long> () {0, 0, 0, 0};
        sciencePerDayD = new List <long> () {0, 0, 0, 0};
        productsPerDayD = new List <long> () {0, 0, 0, 0};
    }
    
    private void Start() {
        money = (long)UnityEngine.Random.Range(0f, 1000000000000f);
        LevelUp();
        SetMoney(money);
        WriteCntPeople();
    }

    IEnumerator AsyncStartEconomy() {
        for (int i = 0; true; ++i) {
            if (BuildsClass.objectsWithAvailableSeats.Count > 0 && BuildsClass.commercesWithAvailableSeats.Count > 0) {
                int houseIt = (int)UnityEngine.Random.Range(0f, BuildsClass.objectsWithAvailableSeats.Count - 0.01f);
                BuildObject houseClass = BuildsClass.objectsWithAvailableSeats[houseIt].GetComponent <BuildObject> ();
                if (houseClass.cntPeople < houseClass.maxCntPeople) {
                    int commerceIt = (int)UnityEngine.Random.Range(0f, BuildsClass.commercesWithAvailableSeats.Count - 0.01f);
                    BuildObject commerceClass = BuildsClass.commercesWithAvailableSeats[commerceIt].GetComponent <BuildObject> ();
                    if (commerceClass.cntPeople < commerceClass.maxCntPeople) {
                        AddCntPeople(FieldClass.districts[(int)houseClass.x + FieldClass.fieldSizeHalf, (int)houseClass.y + FieldClass.fieldSizeHalf]);
                        ++commerceClass.cntPeople;
                        ++houseClass.cntPeople;
                    }
                    else BuildsClass.commercesWithAvailableSeats.RemoveAt(commerceIt);
                }
                else BuildsClass.objectsWithAvailableSeats.RemoveAt(houseIt);
            }
            if (i % 10 == 0) yield return null;
        }
    }

    IEnumerator AddMoneyWithDelay() {
        WriteMoney(false, true);
        yield return new WaitForSeconds(5f);
        WriteMoney(false);
    }

    private void WriteMoney(bool deleteOptMoney = true, bool writePerDay = false) {
        Money.text = money + " ₽";
        if (deleteOptMoney) optMoney = 0;
        if (optMoney > 0) Money.text += "\n+ " + Math.Abs(optMoney) + " ₽";
        if (optMoney < 0) Money.text += "\n- " + Math.Abs(optMoney) + " ₽";
        if (writePerDay) {
            if (optMoney != 0) Money.text += "\n";
            if (moneyPerDay > 0) Money.text += "+ " + Math.Abs(moneyPerDay) + " ₽";
            if (moneyPerDay < 0) Money.text += "- " + Math.Abs(moneyPerDay) + " ₽";
        }
    }

    private void WriteCntPeople() {
        CntPeople.text = cntPeople + " чел";
    }

    private void CalcStatsPerDay() {
        moneyPerDay = sciencePerDay = productsPerDay = 0;
        moneyPerDayD = new List <long> () {0, 0, 0, 0};
        sciencePerDayD = new List <long> () {0, 0, 0, 0};
        productsPerDayD = new List <long> () {0, 0, 0, 0};
        for (int houseIt = 0; houseIt < BuildsClass.objects.Count; ++houseIt) {
            BuildObject buildClass = BuildsClass.objects[houseIt].GetComponent <BuildObject> ();
            House houseClass = BuildsClass.objects[houseIt].GetComponent <House> ();
            int districtNum = FieldClass.districts[(int)buildClass.x + FieldClass.fieldSizeHalf, (int)buildClass.y + FieldClass.fieldSizeHalf];
            moneyPerDay += houseClass.GetTaxRate();
            moneyPerDay -= houseClass.GetServiceCost();
            moneyPerDayD[districtNum] += houseClass.GetTaxRate();
            moneyPerDayD[districtNum] -= houseClass.GetServiceCost();
            
        }
        for (int commerceIt = 0; commerceIt < BuildsClass.commerces.Count; ++commerceIt) {
            GameObject commerceObj = BuildsClass.commerces[commerceIt];
            BuildObject buildClass = BuildsClass.commerces[commerceIt].GetComponent <BuildObject> ();
            int districtNum = FieldClass.districts[(int)buildClass.x + FieldClass.fieldSizeHalf, (int)buildClass.y + FieldClass.fieldSizeHalf];
            if (commerceObj.GetComponent <Factory> ()) {
                Factory factoryClass = commerceObj.GetComponent <Factory> ();
                moneyPerDay -= factoryClass.GetServiceCost();
                productsPerDay += factoryClass.GetProductsRate();
                moneyPerDayD[districtNum] -= factoryClass.GetServiceCost();
                productsPerDayD[districtNum] += factoryClass.GetProductsRate();
            }
            else if (commerceObj.GetComponent <Science> ()) {
                Science scienceClass = commerceObj.GetComponent <Science> ();
                moneyPerDay -= scienceClass.GetServiceCost();
                sciencePerDay += scienceClass.GetScienceRate();
                moneyPerDayD[districtNum] -= scienceClass.GetServiceCost();
                sciencePerDayD[districtNum] += scienceClass.GetScienceRate();
            }
            else if (commerceObj.GetComponent <Shop> ()) {
                Shop shopClass = commerceObj.GetComponent <Shop> ();
                moneyPerDay += shopClass.GetTaxRate();
                moneyPerDayD[districtNum] += shopClass.GetTaxRate();
            }
        }
    }

    private void CalcPeopleStats() {
        averageLoyality = 0;
        averageLoyalityD = new List <int> () {0, 0, 0, 0};
        List <int> counting = new List <int> () {0, 0, 0, 0};
        for (int i = 0; i < PeopleClass.objects.Count; ++i) {
            Passport passportClass = PeopleClass.objects[i].GetComponent <Passport> ();
            if (passportClass.idxSocialСlass < 1 || passportClass.idxSocialСlass > 4) {
                Debug.Log("Incorrect: passportClass.idxSocialСlass = " + passportClass.idxSocialСlass);
                continue;
            }
            averageLoyalityD[passportClass.idxSocialСlass - 1] += passportClass.GetLoyalty().loyalty;
            ++counting[passportClass.idxSocialСlass - 1];
        }
        if (counting[0] > 0) averageLoyalityD[0] /= counting[0];
        if (counting[1] > 0) averageLoyalityD[1] /= counting[1];
        if (counting[2] > 0) averageLoyalityD[2] /= counting[2];
        if (counting[3] > 0) averageLoyalityD[3] /= counting[3];
        if (counting[0] + counting[1] + counting[2] + counting[3] > 0) {
            averageLoyality = (averageLoyalityD[0] + averageLoyalityD[1] + averageLoyalityD[2] + averageLoyalityD[3]) / (counting[0] + counting[1] + counting[2] + counting[3]);
        }
    }

    private void AddScience() {
        science += sciencePerDay;
    }

    private void AddProducts() {
        products += productsPerDay;
    }

    public void StartEconomy() {
        isStarted = true;
        CalcStatsPerDay();
        CalcPeopleStats();
        StartCoroutine(AsyncStartEconomy());
    }

    public void NewDay() {
        if (isStarted) {
            CalcStatsPerDay();
            CalcPeopleStats();
            StartCoroutine(AddMoneyWithDelay());
            AddScience();
            AddProducts();
        }
    }

    public int GetLevel() { return level; }
    public int GetCntPeople() { return cntPeople; }
    public long GetMoney() { return money; }
    public string GetCityName() { return cityName; }

    public void LevelUp() {
        Level.text = ++level + "";
    }
    
    public void AddCntPeople(int districtNum, int dCntPeople = 1) {
        if (districtNum < 0 || districtNum > 3) {
            Debug.Log("Incorrect: districtNum = " + districtNum);
            return;
        }
        cntPeople += dCntPeople;
        cntPeopleD[districtNum] += dCntPeople;
        WriteCntPeople();
    }

    public void AddMoney(long dMoney = 0) {
        if (dMoney == 0) money += optMoney;
        else money += dMoney;
        WriteMoney();
    }

    public void SetMoney(long _money) {
        money = _money;
        WriteMoney(false);
    }

    public void AddBuild(int districtNum, int idxPreFub) {
        if (districtNum < 0 || districtNum > 3) {
            Debug.Log("Incorrect: districtNum = " + districtNum);
            return;
        }
        if (idxPreFub <= 7) ++cntHousesD[districtNum];
        else if (idxPreFub == 8) ++cntShopsD[districtNum];
        else if (idxPreFub == 10 || idxPreFub == 11 || idxPreFub == 12 || idxPreFub == 13 || idxPreFub == 15) ++cntSciencesD[districtNum];
        else if (idxPreFub == 14) ++cntFactoriesD[districtNum];
    }

    public void AddPoster(int districtNum) {
        if (districtNum < 0 || districtNum > 3) {
            Debug.Log("Incorrect: districtNum = " + districtNum);
            return;
        }
        ++cntPostersD[districtNum];
    }
}
