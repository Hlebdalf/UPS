using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bureaucracy : MonoBehaviour
{
    public GameObject HouseNumObject;
    public GameObject PopulationIncrementObject;
    public GameObject ScienceNumObject;
    public GameObject ProductionNumObject;
    public GameObject BudgeIncrementObject;
    public GameObject ScienceIncrementObject;
    public GameObject ProductionIncrementObject;
    public GameObject AVGLoyalityObject;
    public GameObject PostersNumObject;
    public GameObject CommerceNumObject;

    private Text houseNum;
    private Text scienceNum;
    private Text productionNum;
    private Text budgeIncrement;
    private Text populationIncrement;
    private Text scienceIncrement;
    private Text productionIncrement;
    private Text avgLoyality;
    private Text postersNum;
    private Text commerceNum;
    void Start()
    {
        commerceNum = CommerceNumObject.GetComponent<Text>();
        avgLoyality = avgLoyality.GetComponent<Text>();
        postersNum = PostersNumObject.GetComponent<Text>();
        houseNum = HouseNumObject.GetComponent<Text>();
        scienceNum = ScienceNumObject.GetComponent<Text>();
        productionNum = ProductionNumObject.GetComponent<Text>();
        budgeIncrement = BudgeIncrementObject.GetComponent<Text>();
        populationIncrement = PopulationIncrementObject.GetComponent<Text>();
        scienceIncrement = ScienceIncrementObject.GetComponent<Text>();
        productionIncrement = ProductionIncrementObject.GetComponent<Text>();
    }
    
    public void SetCommerceNum(int CommerceNum)
    {
        commerceNum.text = CommerceNum.ToString() + " ШТ.";
    }
    public void HouseNum(int HouseNum)
    {
        houseNum.text = HouseNum.ToString() + " ШТ.";
    }
    public void SetAVGLoyality(int AVGLoyality)
    {
        avgLoyality.text = AVGLoyality.ToString();
    }

    public void SetPostersNum(int PostersNum)
    {
        postersNum.text = postersNum.ToString() + " ШТ.";
    }

    public void SetScienceNum(int ScienceNum)
    {
        scienceNum.text = ScienceNum.ToString() + " ШТ.";
    }

    public void SetProductionNum(int ProductionNum)
    {
        productionNum.text = ProductionNum.ToString() + " ШТ.";
    }

    public void SetBudgeIncrement(int BudgeIncrement)
    {
        budgeIncrement.text = BudgeIncrement.ToString() + " ₽/Д";
    }

    public void SetPopulationIncrement(int PopulationIncrement)
    {
        populationIncrement.text = PopulationIncrement.ToString() + " Ч/Д";
    }

    public void SetScienceIncrement(int ScienceIncrement)
    {
        scienceIncrement.text = ScienceIncrement.ToString() + " Н/Д";
    }

    public void SetProductionIncrement(int ProductionIncrement)
    {
        productionIncrement.text = ProductionIncrement.ToString() + " П/Д";
    }
}
