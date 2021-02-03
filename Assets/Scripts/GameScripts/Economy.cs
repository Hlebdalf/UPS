using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Economy : MonoBehaviour {
    private InputField CityName;
    private Text Level;
    private Text CntPeople;
    private Text Money;

    private string cityName = "";
    private int level = 0;
    private int cntPeople = 0;
    private long money = 0;

    public GameObject fastStats;

    private void Awake() {
        CityName = fastStats.transform.Find("CityName").gameObject.GetComponent <InputField> ();
        Level = fastStats.transform.Find("Level").gameObject.GetComponent <Text> ();
        CntPeople = fastStats.transform.Find("CntPeople").gameObject.GetComponent <Text> ();
        Money = fastStats.transform.Find("Money").gameObject.GetComponent <Text> ();
    }
    
    private void Start() {
        LevelUp();
        SetCntPeople(cntPeople);
        SetMoney(money);
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
