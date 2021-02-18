﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Economy : MonoBehaviour {
    private Builds BuildsClass;

    private InputField CityName;
    private Text Level;
    private Text CntPeople;
    private Text Money;

    private string cityName = "";
    private int level = 0;
    private int cntPeople = 0, cntPeoplePerDay = 0;
    private long money = 0, optMoney = 0, moneyPerDay = 0;
    private long cntScience = 0, cntSciencePerDay = 0;
    private long cntProducts = 0, cntProductsPerDay = 0;
    private float averageLoyality = 0;
    private float HCS = 0, PIT = 0, VAT = 0, CIT = 0; // Коэффиценты налогов
    private bool isStarted = false;
    // private float deltaTime = 0.1f;
    
    private List <int> cntHouses;
    private List <int> cntShops;
    private List <int> cntSciences;
    private List <int> cntFactories;
    private int cntPeople1 = 0, cntPeople2 = 0, cntPeople3 = 0, cntPeople4 = 0;
    private int cntPosters1 = 0, cntPosters2 = 0, cntPosters3 = 0, cntPosters4 = 0;
    private float averageLoyality1 = 0, averageLoyality2 = 0, averageLoyality3 = 0, averageLoyality4 = 0;
    private int sciensePerDay1 = 0, sciensePerDay2 = 0, sciensePerDay3 = 0, sciensePerDay4 = 0;
    private int productsPreDay1 = 0, productsPreDay2 = 0, productsPreDay3 = 0, productsPreDay4 = 0;
    private long moneyPerDay1 = 0, moneyPerDay2 = 0, moneyPerDay3 = 0, moneyPerDay4 = 0;

    public GameObject fastStats;

    private void Awake() {
        BuildsClass = Camera.main.GetComponent <Builds> ();
        CityName = fastStats.transform.Find("CityName").gameObject.GetComponent <InputField> ();
        Level = fastStats.transform.Find("Level").gameObject.GetComponent <Text> ();
        CntPeople = fastStats.transform.Find("CntPeople").gameObject.GetComponent <Text> ();
        Money = fastStats.transform.Find("Money").gameObject.GetComponent <Text> ();
        cntHouses = new List <int> () {0, 0, 0, 0};
        cntShops = new List <int> () {0, 0, 0, 0};
        cntSciences = new List <int> () {0, 0, 0, 0};
        cntFactories = new List <int> () {0, 0, 0, 0};
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
                        AddCntPeople();
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
    
    private void Start() {
        money = (long)UnityEngine.Random.Range(0f, 1000000000000f);
        LevelUp();
        SetMoney(money);
        SetCntPeople(cntPeople);
    }

    private void WriteMoney(bool deleteOptMoney = true) {
        Money.text = money + " ₽";
        if (deleteOptMoney) optMoney = 0;
        if (optMoney > 0) Money.text += "\n+ " + Math.Abs(optMoney) + " ₽";
        if (optMoney < 0) Money.text += "\n- " + Math.Abs(optMoney) + " ₽";
    }

    private void WriteCntPeople() {
        CntPeople.text = cntPeople + " чел";
    }

    public void StartEconomy() {
        isStarted = true;
        StartCoroutine(AsyncStartEconomy());
    }

    IEnumerator PayTax() {
        long tax = 0;
        for (int houseIt = 0; houseIt < BuildsClass.objects.Count; ++houseIt) {
            House houseClass = BuildsClass.objects[houseIt].GetComponent <House> ();
            tax += houseClass.GetTaxRate();
            tax -= houseClass.GetServiceCost();
        }
        yield return null;
        for (int commerceIt = 0; commerceIt < BuildsClass.commerces.Count; ++commerceIt) {
            GameObject commerceObj = BuildsClass.commerces[commerceIt];
            if (commerceObj.GetComponent <Factory> ()) {
                Factory factoryClass = commerceObj.GetComponent <Factory> ();
                tax -= factoryClass.GetServiceCost();
                cntProducts += factoryClass.GetProductsRate();
            }
            else if (commerceObj.GetComponent <Science> ()) {
                Science scienceClass = commerceObj.GetComponent <Science> ();
                tax -= scienceClass.GetServiceCost();
                cntScience += scienceClass.GetScienceRate();
            }
            else if (commerceObj.GetComponent <Shop> ()) {
                Shop shopClass = commerceObj.GetComponent <Shop> ();
                tax += shopClass.GetTaxRate();
            }
        }
        optMoney = tax;
        WriteMoney(false);
        yield return new WaitForSeconds(5f);
        AddMoney();
    }

    public void NewDay() {
        if (isStarted) {
            StartCoroutine(PayTax());
        }
    }

    public int GetLevel() { return level; }
    public int GetCntPeople() { return cntPeople; }
    public long GetMoney() { return money; }
    public string GetCityName() { return cityName; }

    public void LevelUp() {
        Level.text = ++level + "";
    }
    
    public void AddCntPeople() {
        ++cntPeople;
        WriteCntPeople();
    }
    
    public void AddCntPeople(int dCntPeople) {
        cntPeople += dCntPeople;
        WriteCntPeople();
    }
    
    public void SetCntPeople(int _cntPeople) {
        cntPeople = _cntPeople;
        WriteCntPeople();
    }

    public void AddMoney() {
        money += optMoney;
        WriteMoney();
    }

    public void SetMoney(long _money) {
        money = _money;
        WriteMoney(false);
    }

    public void AddBuild(int districtNum, int idxPreFub) {
        if (districtNum < 0 || districtNum > 3) return;
        if (idxPreFub <= 7) ++cntHouses[districtNum];
        else if (idxPreFub == 8) ++cntShops[districtNum];
        else if (idxPreFub == 10 || idxPreFub == 11 || idxPreFub == 12 || idxPreFub == 13 || idxPreFub == 15) ++cntSciences[districtNum];
        else if (idxPreFub == 14) ++cntFactories[districtNum];
    }
}
