﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Economy : MonoBehaviour {
    private Field FieldClass;
    [SerializeField]
    private Interface InterfaceClass;
    private Builds BuildsClass;
    private People PeopleClass;

    private InputField CityName;
    private Text Level;
    private Text CntPeople;
    private Text Money;

    private string cityName = "";
    private int level = 0;
    private int cntPeople = 0, cntPeoplePerDay = 0;
    private long money = 0, optMoney = 0, serviceCost = 0;
    private long science = 0, sciencePerDay = 0;
    private long products = 0, productsPerDay = 0;
    private float averageLoyality = 0;
    private float HCSk = 0, PITk = 0, VATk = 0, CITk = 0; // Коэффиценты налогов
    private float HCSn = 0, PITn = 0, VATn = 0, CITn = 0;
    private bool isStarted = false;
    // private float deltaTime = 0.1f;
    
    private List <int> cntHousesD;
    private List <int> cntShopsD;
    private List <int> cntSciencesD;
    private List <int> cntFactoriesD;
    private List <int> cntPeopleD;
    private List <int> housesCntPeopleD;
    private List <int> housesCntMaxPeopleD;
    private List <int> factoriesCntPeopleD;
    private List <int> sciencesCntPeopleD;
    private List <int> commercesCntPeopleD;
    private List <int> cntPostersD;
    private List <int> averageLoyalityD;
    private List <float> HCSnD;
    private List <float> PITnD;
    private List <float> VATnD;
    private List <float> CITnD;
    private List <long> housesServiceCostD;
    private List <long> factoriesServiceCostD;
    private List <long> sciencesServiceCostD;
    private List <long> sciencePerDayD;
    private List <long> productsPerDayD;

    public GameObject fastStats;
    public SityInfo SityInfoClass;
    public Taxation TaxationClass;
    public Statistic StatisticClass;

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
        housesCntPeopleD = new List <int> () {0, 0, 0, 0};
        housesCntMaxPeopleD = new List <int> () {0, 0, 0, 0};
        factoriesCntPeopleD = new List <int> () {0, 0, 0, 0};
        sciencesCntPeopleD = new List <int> () {0, 0, 0, 0};
        commercesCntPeopleD = new List <int> () {0, 0, 0, 0};
        cntPostersD = new List <int> () {0, 0, 0, 0};
        averageLoyalityD = new List <int> () {0, 0, 0, 0};
        HCSnD = new List <float> () {0, 0, 0, 0};
        PITnD = new List <float> () {0, 0, 0, 0};
        VATnD = new List <float> () {0, 0, 0, 0};
        CITnD = new List <float> () {0, 0, 0, 0};
        housesServiceCostD = new List <long> () {0, 0, 0, 0};
        factoriesServiceCostD = new List <long> () {0, 0, 0, 0};
        sciencesServiceCostD = new List <long> () {0, 0, 0, 0};
        sciencePerDayD = new List <long> () {0, 0, 0, 0};
        productsPerDayD = new List <long> () {0, 0, 0, 0};
    }

    IEnumerator AsyncEconomy() {
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
        money += GetMoneyPerDay();
        WriteMoney(false);
    }
    
    IEnumerator AddMoneyWithDelay(long dMoney) {
        optMoney += dMoney;
        WriteMoney(false);
        yield return new WaitForSeconds(5f);
        money += dMoney;
        WriteMoney(true);
    }

    private void WriteMoney(bool deleteOptMoney = true, bool writePerDay = false) {
        Money.text = money + " ₽";
        if (deleteOptMoney) optMoney = 0;
        if (optMoney > 0) Money.text += "\n+ " + Math.Abs(optMoney) + " ₽";
        if (optMoney < 0) Money.text += "\n- " + Math.Abs(optMoney) + " ₽";
        if (writePerDay) {
            if (optMoney != 0) Money.text += "\n";
            if (GetMoneyPerDay() > 0) Money.text += "+ " + Math.Abs(GetMoneyPerDay()) + " ₽";
            if (GetMoneyPerDay() < 0) Money.text += "- " + Math.Abs(GetMoneyPerDay()) + " ₽";
        }
    }

    private void WriteCntPeople() {
        CntPeople.text = cntPeople + " чел";
    }

    private void CalcStatsPerDay() {
        HCSn = PITn = VATn = CITn = 0;
        serviceCost = sciencePerDay = productsPerDay = 0;

        HCSnD = new List <float> () {0, 0, 0, 0};
        PITnD = new List <float> () {0, 0, 0, 0};
        VATnD = new List <float> () {0, 0, 0, 0};
        CITnD = new List <float> () {0, 0, 0, 0};
        housesServiceCostD = new List <long> () {0, 0, 0, 0};
        factoriesServiceCostD = new List <long> () {0, 0, 0, 0};
        sciencesServiceCostD = new List <long> () {0, 0, 0, 0};
        sciencePerDayD = new List <long> () {0, 0, 0, 0};
        productsPerDayD = new List <long> () {0, 0, 0, 0};

        housesCntPeopleD = new List <int> () {0, 0, 0, 0};
        housesCntMaxPeopleD = new List <int> () {0, 0, 0, 0};
        factoriesCntPeopleD = new List <int> () {0, 0, 0, 0};
        sciencesCntPeopleD = new List <int> () {0, 0, 0, 0};
        commercesCntPeopleD = new List <int> () {0, 0, 0, 0};

        for (int houseIt = 0; houseIt < BuildsClass.objects.Count; ++houseIt) {
            BuildObject buildClass = BuildsClass.objects[houseIt].GetComponent <BuildObject> ();
            House houseClass = BuildsClass.objects[houseIt].GetComponent <House> ();
            int districtNum = FieldClass.districts[(int)buildClass.x + FieldClass.fieldSizeHalf, (int)buildClass.y + FieldClass.fieldSizeHalf];

            HCSn += houseClass.GetTaxRate();
            serviceCost -= houseClass.GetServiceCost();

            HCSnD[districtNum] += (long)(houseClass.GetTaxRate());
            housesServiceCostD[districtNum] -= houseClass.GetServiceCost();

            housesCntPeopleD[districtNum] += buildClass.cntPeople;
            housesCntMaxPeopleD[districtNum] += buildClass.maxCntPeople;
        }

        for (int commerceIt = 0; commerceIt < BuildsClass.commerces.Count; ++commerceIt) {
            GameObject commerceObj = BuildsClass.commerces[commerceIt];
            BuildObject buildClass = BuildsClass.commerces[commerceIt].GetComponent <BuildObject> ();
            int districtNum = FieldClass.districts[(int)buildClass.x + FieldClass.fieldSizeHalf, (int)buildClass.y + FieldClass.fieldSizeHalf];

            if (commerceObj.GetComponent <Factory> ()) {
                Factory factoryClass = commerceObj.GetComponent <Factory> ();

                serviceCost -= factoryClass.GetServiceCost();
                productsPerDay += factoryClass.GetProductsRate();

                factoriesServiceCostD[districtNum] -= factoryClass.GetServiceCost();
                productsPerDayD[districtNum] += factoryClass.GetProductsRate();

                factoriesCntPeopleD[districtNum] += buildClass.cntPeople;
            }
            else if (commerceObj.GetComponent <Science> ()) {
                Science scienceClass = commerceObj.GetComponent <Science> ();

                serviceCost -= scienceClass.GetServiceCost();
                sciencePerDay += scienceClass.GetScienceRate();

                sciencesServiceCostD[districtNum] -= scienceClass.GetServiceCost();
                sciencePerDayD[districtNum] += scienceClass.GetScienceRate();

                sciencesCntPeopleD[districtNum] += buildClass.cntPeople;
            }
            else if (commerceObj.GetComponent <Shop> ()) {
                Shop shopClass = commerceObj.GetComponent <Shop> ();

                CITn += shopClass.GetTaxRate();

                CITnD[districtNum] += (long)(shopClass.GetTaxRate());

                commercesCntPeopleD[districtNum] += buildClass.cntPeople;
            }
        }
    }

    private void CalcPeopleStats() {
        PITn = VATn = averageLoyality = 0;
        averageLoyalityD = new List <int> () {0, 0, 0, 0};
        List <int> countingAvgLoyality = new List <int> () {0, 0, 0, 0};
        List <int> countingPIT = new List <int> () {0, 0, 0, 0};
        List <int> countingVAT = new List <int> () {0, 0, 0, 0};
        for (int i = 0; i < PeopleClass.objects.Count; ++i) {
            Passport passportClass = PeopleClass.objects[i].GetComponent <Passport> ();
            if (passportClass.idxSocialСlass < 1 || passportClass.idxSocialСlass > 4) {
                Debug.Log("Incorrect: passportClass.idxSocialСlass = " + passportClass.idxSocialСlass);
                continue;
            }
            averageLoyalityD[passportClass.idxSocialСlass - 1] += passportClass.GetLoyalty().loyalty;
            ++countingAvgLoyality[passportClass.idxSocialСlass - 1];

            PITnD[passportClass.idxSocialСlass - 1] += (float)passportClass.salary;
            ++countingPIT[passportClass.idxSocialСlass - 1];

            if (passportClass.idxCommerceType == 0) {
                VATnD[passportClass.idxSocialСlass - 1] += UnityEngine.Random.Range((float)passportClass.salary / 100f + 1f, (float)passportClass.salary / 2f);
                ++countingVAT[passportClass.idxSocialСlass - 1];
            }
        }
        if (countingAvgLoyality[0] + countingAvgLoyality[1] + countingAvgLoyality[2] + countingAvgLoyality[3] > 0) {
            averageLoyality = (averageLoyalityD[0] + averageLoyalityD[1] + averageLoyalityD[2] + averageLoyalityD[3]) / (countingAvgLoyality[0] + countingAvgLoyality[1] + countingAvgLoyality[2] + countingAvgLoyality[3]);
        }
        if (countingAvgLoyality[0] > 0) averageLoyalityD[0] /= countingAvgLoyality[0];
        if (countingAvgLoyality[1] > 0) averageLoyalityD[1] /= countingAvgLoyality[1];
        if (countingAvgLoyality[2] > 0) averageLoyalityD[2] /= countingAvgLoyality[2];
        if (countingAvgLoyality[3] > 0) averageLoyalityD[3] /= countingAvgLoyality[3];

        if (countingPIT[0] + countingPIT[1] + countingPIT[2] + countingPIT[3] > 0) {
            PITn = (PITnD[0] + PITnD[1] + PITnD[2] + PITnD[3]) / (countingPIT[0] + countingPIT[1] + countingPIT[2] + countingPIT[3]) * cntPeople;
        }
        if (countingPIT[0] > 0) PITnD[0] = PITnD[0] / countingPIT[0] * cntPeople;
        if (countingPIT[1] > 0) PITnD[1] = PITnD[1] / countingPIT[1] * cntPeople;
        if (countingPIT[2] > 0) PITnD[2] = PITnD[2] / countingPIT[2] * cntPeople;
        if (countingPIT[3] > 0) PITnD[3] = PITnD[3] / countingPIT[3] * cntPeople;

        if (countingVAT[0] + countingVAT[1] + countingVAT[2] + countingVAT[3] > 0) {
            VATn = (VATnD[0] + VATnD[1] + VATnD[2] + VATnD[3]) / (countingVAT[0] + countingVAT[1] + countingVAT[2] + countingVAT[3]) * cntPeople;
        }
        if (countingVAT[0] > 0) VATnD[0] = VATnD[0] / countingVAT[0] * cntPeople;
        if (countingVAT[1] > 0) VATnD[1] = VATnD[1] / countingVAT[1] * cntPeople;
        if (countingVAT[2] > 0) VATnD[2] = VATnD[2] / countingVAT[2] * cntPeople;
        if (countingVAT[3] > 0) VATnD[3] = VATnD[3] / countingVAT[3] * cntPeople;
    }

    private void PaySalaries() {
        for (int i = 0; i < PeopleClass.objects.Count; ++i) {
            PeopleClass.objects[i].GetComponent <Passport> ().PaySalary();
        }
    }

    private void AddScience() {
        science += sciencePerDay;
    }

    private void AddProducts() {
        products += productsPerDay;
    }

    private void SetGDP() {
        (float HCS, float PIT, float VAT, float CIT) k = TaxationClass.GetGDPk();
        HCSk = k.HCS; PITk = k.PIT; VATk = k.VAT; CITk = k.CIT;
    }

    public void StartEconomy() {
        money = (long)UnityEngine.Random.Range(0f, 1000000000000f);
        LevelUp();
        SetMoney(money);
        WriteCntPeople();
        isStarted = true;
        SetGDP();
        CalcStatsPerDay();
        CalcPeopleStats();
        StartCoroutine(AsyncEconomy());
    }

    public void NewDay() {
        if (isStarted) {
            SetGDP();
            CalcStatsPerDay();
            CalcPeopleStats();
            StartCoroutine(AddMoneyWithDelay());
            PaySalaries();
            AddScience();
            AddProducts();
            if (InterfaceClass.EconomyPanelActivity) FillInTheMenuWithStatistics();
        }
    }

    // Получение основной информации о городе
    public string GetCityName() { return cityName; }
    public int GetLevel() { return level; }
    public long GetMoney() { return money; }
    public int GetCntPeople() { return cntPeople; }
    public long GetScience() { return science; }
    public long GetProducts() { return products; }
    public long GetMoneyPerDay() { return (long)(HCSn * HCSk + PITn * PITk + VATn * VATk + CITn * CITk) + serviceCost; }
    public int GetPeoplePerDay() { return cntPeoplePerDay; }
    public long GetSciencePerDay() { return sciencePerDay; }
    public long GetProductsPerDay() { return productsPerDay; }

    // Получение информации о коммерции
    public int GetCntShopsD(int idx) { return cntShopsD[idx]; }
    public long GetShopsMoneyPerDayD(int idx) { return (long)(CITnD[idx] * CITk); }
    public int GetShopsCntPeopleD(int idx) { return commercesCntPeopleD[idx]; }
    
    // Получение информации о заводах
    public long GetCntFactoriesD(int idx) { return cntFactoriesD[idx]; }
    public long GetFactoriesServicesCostD(int idx) { return factoriesServiceCostD[idx]; }
    public long GetFactoriesCntPeopleD(int idx) { return factoriesCntPeopleD[idx]; }
    
    // Получение информации о науке
    public long GetCntSciencesD(int idx) { return cntSciencesD[idx]; }
    public long GetSciencesServicesCostD(int idx) { return sciencesServiceCostD[idx]; }
    public long GetSciencesCntPeopleD(int idx) { return sciencesCntPeopleD[idx]; }

    // Получение информации о домах
    public int GetCntHousesD(int idx) { return cntHousesD[idx]; }
    public long GetHousesMoneyPerDayD(int idx) { return (long)(HCSnD[idx] * HCSk); }
    public long GetHousesServicesCostD(int idx) { return housesServiceCostD[idx]; }
    public int GetHousesCntPeopleD(int idx) { return housesCntPeopleD[idx]; }
    public int GetHousesCntMaxPeopleD(int idx) { return housesCntMaxPeopleD[idx]; }

    // Получение информации о плакатах
    public int GetCntPostersD(int idx) { return cntPostersD[idx]; }
    public long GetPostersServicesCostD(int idx) { return cntHousesD[idx]; } // !
    public float GetAverageLoyalityD(int idx) { return averageLoyalityD[idx]; }

    // Получение общей информации о квартале
    public long GetUpMoneyPerDayD(int idx) { return (long)(HCSnD[idx] * HCSk + PITnD[idx] * PITk + VATnD[idx] * VATk + CITnD[idx] * CITk); }
    public long GetDownMoneyPerDayD(int idx) { return housesServiceCostD[idx] + factoriesServiceCostD[idx] + sciencesServiceCostD[idx]; }
    public long GetMoneyPerDayD(int idx) { return GetUpMoneyPerDayD(idx) + GetDownMoneyPerDayD(idx); }
    public int GetCntPeopleD(int idx) { return cntPeopleD[idx]; }
    public long GetSciencePerDayD(int idx) { return sciencePerDayD[idx]; }
    public long GetProductsPerDayD(int idx) { return productsPerDayD[idx]; }

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
        if (idxPreFub == 5 || idxPreFub == 6) ++cntHousesD[0];
        else if (idxPreFub == 0 || idxPreFub == 1) ++cntHousesD[1];
        else if (idxPreFub == 2 || idxPreFub == 3) ++cntHousesD[2];
        else if (idxPreFub == 4 || idxPreFub == 7) ++cntHousesD[3];
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

    public void ChangeGDP() {
        SetGDP();
        SityInfoClass.SetBudgetIncrement(GetMoneyPerDay());
        StatisticClass.SetBudgetIncrement(GetMoneyPerDayD(0), 1);
        StatisticClass.SetBudgetIncrement(GetMoneyPerDayD(1), 2);
        StatisticClass.SetBudgetIncrement(GetMoneyPerDayD(2), 3);
        StatisticClass.SetBudgetIncrement(GetMoneyPerDayD(3), 4);
        SityInfoClass.SetGDP(HCSn * HCSk, PITn * PITk, VATn * VATk, CITn * CITk); // Цена без обслуживания
    }

    public void SellProducts(long price) {
        StartCoroutine(AddMoneyWithDelay(price * GetProducts()));
        products = 0;
        SityInfoClass.SetBudget(GetMoney());
        SityInfoClass.SetProduction(GetProducts());
    }

    public void FillInTheMenuWithStatistics() {
        SetGDP();
        CalcStatsPerDay();
        CalcPeopleStats();

        SityInfoClass.SetName(GetCityName());
        SityInfoClass.SetLevel(GetLevel());
        SityInfoClass.SetBudget(GetMoney());
        SityInfoClass.SetPopulation(GetCntPeople());
        SityInfoClass.SetScience(GetScience());
        SityInfoClass.SetProduction(GetProducts());
        SityInfoClass.SetBudgetIncrement(GetMoneyPerDay());
        SityInfoClass.SetPopulationIncrement(GetPeoplePerDay()); // Don't working
        SityInfoClass.SetScienceIncrement(GetSciencePerDay());
        SityInfoClass.SetProductionIncrement(GetProductsPerDay());
        SityInfoClass.SetGDP(HCSn * HCSk, PITn * PITk, VATn * VATk, CITn * CITk); // Цена без обслуживания

        StatisticClass.SetCommerceNum(GetCntShopsD(0), 1);
        StatisticClass.SetCommerceNum(GetCntShopsD(1), 2);
        StatisticClass.SetCommerceNum(GetCntShopsD(2), 3);
        StatisticClass.SetCommerceNum(GetCntShopsD(3), 4);

        StatisticClass.HouseNum(GetCntHousesD(0), 1);
        StatisticClass.HouseNum(GetCntHousesD(1), 2);
        StatisticClass.HouseNum(GetCntHousesD(2), 3);
        StatisticClass.HouseNum(GetCntHousesD(3), 4);

        StatisticClass.SetAVGLoyality((int)GetAverageLoyalityD(0), 1);
        StatisticClass.SetAVGLoyality((int)GetAverageLoyalityD(1), 2);
        StatisticClass.SetAVGLoyality((int)GetAverageLoyalityD(2), 3);
        StatisticClass.SetAVGLoyality((int)GetAverageLoyalityD(3), 4);

        StatisticClass.SetPostersNum(GetCntPostersD(0), 1);
        StatisticClass.SetPostersNum(GetCntPostersD(1), 2);
        StatisticClass.SetPostersNum(GetCntPostersD(2), 3);
        StatisticClass.SetPostersNum(GetCntPostersD(3), 4);

        StatisticClass.SetScienceNum(GetCntSciencesD(0), 1);
        StatisticClass.SetScienceNum(GetCntSciencesD(1), 2);
        StatisticClass.SetScienceNum(GetCntSciencesD(2), 3);
        StatisticClass.SetScienceNum(GetCntSciencesD(3), 4);

        StatisticClass.SetProductionNum(GetCntFactoriesD(0), 1);
        StatisticClass.SetProductionNum(GetCntFactoriesD(1), 2);
        StatisticClass.SetProductionNum(GetCntFactoriesD(2), 3);
        StatisticClass.SetProductionNum(GetCntFactoriesD(3), 4);

        StatisticClass.SetBudgetIncrement(GetMoneyPerDayD(0), 1);
        StatisticClass.SetBudgetIncrement(GetMoneyPerDayD(1), 2);
        StatisticClass.SetBudgetIncrement(GetMoneyPerDayD(2), 3);
        StatisticClass.SetBudgetIncrement(GetMoneyPerDayD(3), 4);

        StatisticClass.SetPopulation(GetCntPeopleD(0), 1);
        StatisticClass.SetPopulation(GetCntPeopleD(1), 2);
        StatisticClass.SetPopulation(GetCntPeopleD(2), 3);
        StatisticClass.SetPopulation(GetCntPeopleD(3), 4);

        StatisticClass.SetScienceIncrement(GetSciencePerDayD(0), 1);
        StatisticClass.SetScienceIncrement(GetSciencePerDayD(1), 2);
        StatisticClass.SetScienceIncrement(GetSciencePerDayD(2), 3);
        StatisticClass.SetScienceIncrement(GetSciencePerDayD(3), 4);

        StatisticClass.SetProductionIncrement(GetProductsPerDayD(0), 1);
        StatisticClass.SetProductionIncrement(GetProductsPerDayD(1), 2);
        StatisticClass.SetProductionIncrement(GetProductsPerDayD(2), 3);
        StatisticClass.SetProductionIncrement(GetProductsPerDayD(3), 4);
    }
}
