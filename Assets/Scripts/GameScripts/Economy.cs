using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;

public class Economy : MonoBehaviour {
    private Field FieldClass;
    [SerializeField]
    private Interface InterfaceClass;
    private Builds BuildsClass;
    private Roads RoadsClass;
    private People PeopleClass;

    private InputField CityName;
    private Text Level;
    private Text CntPeople;
    private Text Money;

    private string cityName = "";
    private int level = 0;
    private int cntPeople = 0;
    private long money = 0, optMoney = 0, serviceCost = 0;
    private long science = 0, sciencePerDay = 0;
    private long products = 0, productsPerDay = 0;
    private int averageLoyality = 0;
    private float HCSk = 0, PITk = 0, VATk = 0, CITk = 0; // Коэффиценты налогов
    private float HCSn = 0, PITn = 0, VATn = 0, CITn = 0;
    private bool isStarted = false;
    [SerializeField]
    private double deltaTime = 0.1; // Не более 6 знаков после запятой
    [SerializeField]
    private double deltaTimeDown = 0.1, deltaTimeUp = 0.1;
    
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
    private List <long> postersServiceCostD;
    private List <long> roadsLenD;
    private List <long> sciencePerDayD;
    private List <long> productsPerDayD;
    [SerializeField]
    private List <double> deltaTimeDownD, deltaTimeUpD;

    public GameObject fastStats;
    public SityInfo SityInfoClass;
    public Taxation TaxationClass;
    public Statistic StatisticClass;

    private void Awake() {
        FieldClass = Camera.main.GetComponent <Field> ();
        BuildsClass = Camera.main.GetComponent <Builds> ();
        RoadsClass = Camera.main.GetComponent <Roads> ();
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
        postersServiceCostD = new List <long> () {0, 0, 0, 0};
        roadsLenD = new List <long> () {0, 0, 0, 0};
        sciencePerDayD = new List <long> () {0, 0, 0, 0};
        productsPerDayD = new List <long> () {0, 0, 0, 0};
        deltaTimeDownD = new List <double> () {0, 0, 0, 0};
        deltaTimeUpD = new List <double> () {0, 0, 0, 0};
    }

    IEnumerator AsyncEconomy() {
        for (int i = 0; true; i = (i + 1) % 1000000000) {
            if (deltaTime < 0 && BuildsClass.objectsWithoutAvailableSeats.Count > 0 && BuildsClass.commercesWithoutAvailableSeats.Count > 0) {
                int houseIt = (int)UnityEngine.Random.Range(0f, BuildsClass.objectsWithoutAvailableSeats.Count - 0.01f);
                BuildObject houseClass = BuildsClass.objectsWithoutAvailableSeats[houseIt].GetComponent <BuildObject> ();
                if (houseClass.cntPeople > 0) {
                    int commerceIt = (int)UnityEngine.Random.Range(0f, BuildsClass.commercesWithoutAvailableSeats.Count - 0.01f);
                    BuildObject commerceClass = BuildsClass.commercesWithoutAvailableSeats[commerceIt].GetComponent <BuildObject> ();
                    if (commerceClass.cntPeople > 0) {
                        if (houseClass.cntPeople == houseClass.maxCntPeople) BuildsClass.objectsWithAvailableSeats.Add(BuildsClass.objectsWithoutAvailableSeats[houseIt]);
                        if (commerceClass.cntPeople == commerceClass.maxCntPeople) BuildsClass.commercesWithAvailableSeats.Add(BuildsClass.commercesWithoutAvailableSeats[commerceIt]);
                        AddCntPeople(FieldClass.districts[(int)houseClass.x + FieldClass.fieldSizeHalf, (int)houseClass.y + FieldClass.fieldSizeHalf], -1);
                        --commerceClass.cntPeople;
                        --houseClass.cntPeople;
                    }
                    else BuildsClass.commercesWithoutAvailableSeats.RemoveAt(commerceIt);
                }
                else BuildsClass.objectsWithoutAvailableSeats.RemoveAt(houseIt);
            }
            else if (deltaTime > 0 && BuildsClass.objectsWithAvailableSeats.Count > 0 && BuildsClass.commercesWithAvailableSeats.Count > 0) {
                int houseIt = (int)UnityEngine.Random.Range(0f, BuildsClass.objectsWithAvailableSeats.Count - 0.01f);
                BuildObject houseClass = BuildsClass.objectsWithAvailableSeats[houseIt].GetComponent <BuildObject> ();
                if (houseClass.cntPeople < houseClass.maxCntPeople) {
                    int commerceIt = (int)UnityEngine.Random.Range(0f, BuildsClass.commercesWithAvailableSeats.Count - 0.01f);
                    BuildObject commerceClass = BuildsClass.commercesWithAvailableSeats[commerceIt].GetComponent <BuildObject> ();
                    if (commerceClass.cntPeople < commerceClass.maxCntPeople) {
                        if (houseClass.cntPeople == 0) BuildsClass.objectsWithoutAvailableSeats.Add(BuildsClass.objectsWithAvailableSeats[houseIt]);
                        if (commerceClass.cntPeople == 0) BuildsClass.commercesWithoutAvailableSeats.Add(BuildsClass.commercesWithAvailableSeats[commerceIt]);
                        AddCntPeople(FieldClass.districts[(int)houseClass.x + FieldClass.fieldSizeHalf, (int)houseClass.y + FieldClass.fieldSizeHalf]);
                        ++commerceClass.cntPeople;
                        ++houseClass.cntPeople;
                    }
                    else BuildsClass.commercesWithAvailableSeats.RemoveAt(commerceIt);
                }
                else BuildsClass.objectsWithAvailableSeats.RemoveAt(houseIt);
            }

            // Изменение deltaTime
            CalcDeltaTimeDown();
            CalcDeltaTimeUp();
            if (deltaTimeUp == 0 || deltaTimeDown == 0 || 1 / deltaTimeUp - 1 / deltaTimeDown == 0) deltaTime = 0;
            else deltaTime = 1 / (1 / deltaTimeUp - 1 / deltaTimeDown);

            double d = 1;
            if (deltaTime != 0) d /= Math.Abs(deltaTime);
            if (d <= 1) {
                for (double t = 0; t <= 1; t += d) yield return null;
            }
            else if (i % (int)d == 0) yield return null;
        }
    }

    IEnumerator AsyncUpdatePeopleStats() {
        while (true) {
            CalcPeopleStats();
            yield return new WaitForSeconds(1);
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

    private void CalcDeltaTimeDown() {
        deltaTimeDown = 0;
        List <int> avgLoyality = GetAverageLoyalityD();
        for (int i = 0; i < 4; ++i) {
            double a = avgLoyality[i] + 10;
            deltaTimeDownD[i] = a * a / 10000;
            deltaTimeDown += deltaTimeDownD[i];
        }
        deltaTimeDown /= 4;
    }

    private void CalcDeltaTimeUp() {
        deltaTimeUp = 0;
        List <int> cntPosters = GetCntPostersD();
        List <int> cntMaxPeople = GetHousesCntMaxPeopleD();
        List <int> cntPeople = GetHousesCntPeopleD();
        for (int i = 0; i < 4; ++i) {
            double a = Math.Max(Math.Min(cntMaxPeople[i] - cntPeople[i], 4000), 500);
            double b = Math.Max(Math.Min(cntPosters[i], 4000), 500);
            deltaTimeUpD[i] = a * a * b / 10000000000;
            deltaTimeUp += deltaTimeUpD[i];
        }
        deltaTimeUp /= 4;
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
        postersServiceCostD = new List <long> () {0, 0, 0, 0};
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

            postersServiceCostD[districtNum] -= buildClass.cntPosters * buildClass.posterCost;
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
            
            postersServiceCostD[districtNum] -= buildClass.cntPosters * buildClass.posterCost;
        }
    }

    private void CalcPeopleStats() {
        PITn = VATn = averageLoyality = 0;
        averageLoyalityD = new List <int> () {0, 0, 0, 0};
        PITnD = new List <float> () {0, 0, 0, 0};
        VATnD = new List <float> () {0, 0, 0, 0};
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
            averageLoyality = (int)((averageLoyalityD[0] + averageLoyalityD[1] + averageLoyalityD[2] + averageLoyalityD[3]) / (countingAvgLoyality[0] + countingAvgLoyality[1] + countingAvgLoyality[2] + countingAvgLoyality[3]));
        }
        if (countingAvgLoyality[0] > 0) averageLoyalityD[0] /= countingAvgLoyality[0];
        if (countingAvgLoyality[1] > 0) averageLoyalityD[1] /= countingAvgLoyality[1];
        if (countingAvgLoyality[2] > 0) averageLoyalityD[2] /= countingAvgLoyality[2];
        if (countingAvgLoyality[3] > 0) averageLoyalityD[3] /= countingAvgLoyality[3];

        if (countingPIT[0] + countingPIT[1] + countingPIT[2] + countingPIT[3] > 0) {
            PITn = (PITnD[0] + PITnD[1] + PITnD[2] + PITnD[3]) / (countingPIT[0] + countingPIT[1] + countingPIT[2] + countingPIT[3]) * cntPeople;
        }
        if (countingPIT[0] > 0) PITnD[0] = PITnD[0] / countingPIT[0] * cntPeopleD[0];
        if (countingPIT[1] > 0) PITnD[1] = PITnD[1] / countingPIT[1] * cntPeopleD[1];
        if (countingPIT[2] > 0) PITnD[2] = PITnD[2] / countingPIT[2] * cntPeopleD[2];
        if (countingPIT[3] > 0) PITnD[3] = PITnD[3] / countingPIT[3] * cntPeopleD[3];

        if (countingVAT[0] + countingVAT[1] + countingVAT[2] + countingVAT[3] > 0) {
            VATn = (VATnD[0] + VATnD[1] + VATnD[2] + VATnD[3]) / (countingVAT[0] + countingVAT[1] + countingVAT[2] + countingVAT[3]) * cntPeople;
        }
        if (countingVAT[0] > 0) VATnD[0] = VATnD[0] / countingVAT[0] * cntPeopleD[0];
        if (countingVAT[1] > 0) VATnD[1] = VATnD[1] / countingVAT[1] * cntPeopleD[1];
        if (countingVAT[2] > 0) VATnD[2] = VATnD[2] / countingVAT[2] * cntPeopleD[2];
        if (countingVAT[3] > 0) VATnD[3] = VATnD[3] / countingVAT[3] * cntPeopleD[3];
    }

    private void StartRoads() {
        for (int i = 0; i < RoadsClass.objects.Count; ++i) {
            RoadObject roadClass = RoadsClass.objects[i].GetComponent <RoadObject> ();
            AddRoad(FieldClass.districts[(int)((roadClass.x1 + roadClass.x2) / 2) + FieldClass.fieldSizeHalf, (int)((roadClass.y1 + roadClass.y2) / 2) + FieldClass.fieldSizeHalf], (int)roadClass.len);
        }
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
        StartRoads();
        StartCoroutine(AsyncEconomy());
        StartCoroutine(AsyncUpdatePeopleStats());
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
    public float GetHCSk() { return HCSk; }
    public float GetPITk() { return PITk; }
    public float GetVATk() { return VATk; }
    public float GetCITk() { return CITk; }
    public int GetCntPeoplePerDay() { return (int)(5 / Math.Sqrt(deltaTime) * 360); }
    public int GetAverageLoyality() { return averageLoyality; }
    public long GetSciencePerDay() { return sciencePerDay; }
    public long GetProductsPerDay() { return productsPerDay; }
    public long GetRoadsServicesCost() { return RoadsClass.roadsLen * RoadsClass.serviceCost; }

    // Получение информации о коммерции
    public List <int> GetCntShopsD() { return cntShopsD; }
    public List <long> GetShopsMoneyPerDayD() {
        List <long> ans = new List <long> ();
        for (int i = 0; i < 4; ++i) ans.Add((long)(CITnD[i] * CITk));
        return ans;
    }
    public List <int> GetShopsCntPeopleD() { return commercesCntPeopleD; }
    
    // Получение информации о заводах
    public List <int> GetCntFactoriesD() { return cntFactoriesD; }
    public List <long> GetFactoriesServicesCostD() { return factoriesServiceCostD; }
    public List <int> GetFactoriesCntPeopleD() { return factoriesCntPeopleD; }
    
    // Получение информации о науке
    public List <int> GetCntSciencesD() { return cntSciencesD; }
    public List <long> GetSciencesServicesCostD() { return sciencesServiceCostD; }
    public List <int> GetSciencesCntPeopleD() { return sciencesCntPeopleD; }

    // Получение информации о домах
    public List <int> GetCntHousesD() { return cntHousesD; }
    public List <long> GetHousesMoneyPerDayD() {
        List <long> ans = new List <long> ();
        for (int i = 0; i < 4; ++i) ans.Add((long)(HCSnD[i] * HCSk));
        return ans;
    }
    public List <long> GetHousesServicesCostD() { return housesServiceCostD; }
    public List <int> GetHousesCntPeopleD() { return housesCntPeopleD; }
    public List <int> GetHousesCntMaxPeopleD() { return housesCntMaxPeopleD; }

    // Получение информации о плакатах
    public List <int> GetCntPostersD() { return cntPostersD; }
    public List <int> GetAverageLoyalityD() { return averageLoyalityD; }

    // Получение общей информации о квартале
    public List <long> GetUpMoneyPerDayD() {
        List <long> ans = new List <long> ();
        for (int i = 0; i < 4; ++i) ans.Add((long)(HCSnD[i] * HCSk + PITnD[i] * PITk + VATnD[i] * VATk + CITnD[i] * CITk));
        return ans;
    }
    public List <long> GetDownMoneyPerDayD() {
        List <long> ans = new List <long> ();
        for (int i = 0; i < 4; ++i) ans.Add(housesServiceCostD[i] + factoriesServiceCostD[i] + sciencesServiceCostD[i] + postersServiceCostD[i] - roadsLenD[i] * RoadsClass.serviceCost);
        return ans;
    }
    public List <long> GetMoneyPerDayD() {
        List <long> up = GetUpMoneyPerDayD();
        List <long> down = GetDownMoneyPerDayD();
        List <long> ans = new List <long> ();
        for (int i = 0; i < 4; ++i) ans.Add(up[i] + down[i]);
        return ans;
    }
    // Скорость: 1 - 5 чел/сек.р.вр
    // Скорость: 5 - 2 чел/сек.р.вр
    // Один день = 6 мин.р.вр.
    public List <int> GetUpCntPeoplePerDayD() {
        List <int> ans = new List <int> ();
        for (int i = 0; i < 4; ++i) ans.Add((int)(5 / Math.Sqrt(deltaTimeUpD[i]) * 360));
        return ans;
    }
    public List <int> GetDownCntPeoplePerDayD() {
        List <int> ans = new List <int> ();
        for (int i = 0; i < 4; ++i) ans.Add((int)(5 / Math.Sqrt(deltaTimeDownD[i]) * 360));
        return ans;
    }
    public List <int> GetCntPeopleD() { return cntPeopleD; }
    public List <long> GetSciencePerDayD() { return sciencePerDayD; }
    public List <long> GetProductsPerDayD() { return productsPerDayD; }
    public List <long> GetPostersServicesCostD() { return postersServiceCostD; }
    public List <long> GetRoadsServicesCostD() {
        List <long> ans = new List <long> ();
        for (int i = 0; i < 4; ++i) ans.Add(-roadsLenD[i] * RoadsClass.serviceCost);
        return ans;
    }

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

    public void AddRoad(int districtNum, int len) {
        if (districtNum < 0 || districtNum > 3) {
            Debug.Log("Incorrect: districtNum = " + districtNum);
            return;
        }
        roadsLenD[districtNum] += len;
    }

    public void ChangeGDP() {
        SetGDP();
        SityInfoClass.SetBudgetIncrement(GetMoneyPerDay());
        StatisticClass.SetBudgetIncrement(GetUpMoneyPerDayD());
        StatisticClass.SetBudgetDecrement(GetDownMoneyPerDayD());
        StatisticClass.SetAVGLoyalty(GetAverageLoyalityD());
        StatisticClass.SetPopulationIncrement(GetUpCntPeoplePerDayD());
        StatisticClass.SetPopulationDecrement(GetDownCntPeoplePerDayD());
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
        SityInfoClass.SetPopulationIncrement(GetCntPeoplePerDay());
        SityInfoClass.SetScienceIncrement(GetSciencePerDay());
        SityInfoClass.SetProductionIncrement(GetProductsPerDay());
        SityInfoClass.SetAVGLoyalty(GetAverageLoyality());
        SityInfoClass.SetGDP(HCSn * HCSk, PITn * PITk, VATn * VATk, CITn * CITk); // Цена без обслуживания

        StatisticClass.SetCommerceNum(GetCntShopsD());
        StatisticClass.SetCommerceIncome(GetShopsMoneyPerDayD());
        StatisticClass.SetCommerceWorkplaces(GetShopsCntPeopleD());

        StatisticClass.SetProductionNum(GetCntFactoriesD());
        StatisticClass.SetProductionOutcome(GetFactoriesServicesCostD());
        StatisticClass.SetProductionWorkplaces(GetFactoriesCntPeopleD());

        StatisticClass.SetScienceNum(GetCntSciencesD());
        StatisticClass.SetScienceOutcome(GetSciencesServicesCostD());
        StatisticClass.SetScienceWorkplaces(GetSciencesCntPeopleD());

        StatisticClass.SetHouseNum(GetCntHousesD());
        StatisticClass.SetHouseOutcome(GetHousesServicesCostD());
        StatisticClass.SetHouseIncome(GetHousesMoneyPerDayD());
        StatisticClass.SetHouseOccupiedPlaces(GetHousesCntPeopleD());
        StatisticClass.SetHouseAllPlaces(GetHousesCntMaxPeopleD());

        StatisticClass.SetPostersNum(GetCntPostersD());
        StatisticClass.SetPostersOutcome(GetPostersServicesCostD());

        StatisticClass.SetRoadsOutcome(GetRoadsServicesCostD());
        StatisticClass.SetAVGLoyalty(GetAverageLoyalityD());

        StatisticClass.SetBudgetIncrement(GetUpMoneyPerDayD());
        StatisticClass.SetBudgetDecrement(GetDownMoneyPerDayD());

        StatisticClass.SetPopulationIncrement(GetUpCntPeoplePerDayD());
        StatisticClass.SetPopulationDecrement(GetDownCntPeoplePerDayD());
        StatisticClass.SetPopulationNum(GetCntPeopleD());

        StatisticClass.SetScinceIncrement(GetSciencePerDayD());
        StatisticClass.SetProductionIncrement(GetProductsPerDayD());
    }
}
