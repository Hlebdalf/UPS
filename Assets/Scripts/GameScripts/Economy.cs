using System.Collections;
using System.Collections.Generic;
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
    private int cntPeople = 0;
    private long money = 0;
    // private float deltaTime = 0.1f;

    public GameObject fastStats;

    private void Awake() {
        BuildsClass = Camera.main.GetComponent <Builds> ();

        CityName = fastStats.transform.Find("CityName").gameObject.GetComponent <InputField> ();
        Level = fastStats.transform.Find("Level").gameObject.GetComponent <Text> ();
        CntPeople = fastStats.transform.Find("CntPeople").gameObject.GetComponent <Text> ();
        Money = fastStats.transform.Find("Money").gameObject.GetComponent <Text> ();
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
                        ++commerceClass.cntPeople;
                        ++houseClass.cntPeople;
                    }
                    else BuildsClass.commercesWithAvailableSeats.RemoveAt(commerceIt);
                }
                else BuildsClass.objectsWithAvailableSeats.RemoveAt(houseIt);
            }
            if (i % 100 == 0) yield return null;
        }
    }
    
    private void Start() {
        LevelUp();
        SetCntPeople(cntPeople);
        SetMoney(money);
    }

    public void StartEconomy() {
        StartCoroutine(AsyncStartEconomy());
    }

    public int GetLevel() { return level; }
    public int GetCntPeople() { return cntPeople; }
    public long GetMoney() { return money; }
    public string GetCityName() { return cityName; }

    public void LevelUp() {
        Level.text = ++level + "";
    }
    
    public void SetCntPeople(int _cntPeople) {
        cntPeople = _cntPeople;
        CntPeople.text = cntPeople + "";
    }

    public void SetMoney(long _money) {
        money = _money;
        Money.text = _money + " ₽";
    }
}
