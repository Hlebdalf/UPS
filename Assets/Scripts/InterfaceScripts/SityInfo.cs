using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SityInfo : MonoBehaviour
{
    public GameObject NameObject;
    public GameObject LevelObject;
    public GameObject BudgetObject;
    public GameObject PopulationObject;
    public GameObject ScienceObject;
    public GameObject ProductionObject;
    public GameObject BudgeIncrementObject;
    public GameObject PopulationIncrementObject;
    public GameObject ScienceIncrementObject;
    public GameObject ProductionIncrementObject;
    public GameObject RadialDiagramObjectA;
    public GameObject RadialDiagramObjectB;
    public GameObject RadialDiagramObjectC;
    public GameObject RadialDiagramObjectD;

    private Text selfName;
    private Text level;
    private Text budget;
    private Text population;
    private Text science;
    private Text production;
    private Text budgeIncrement;
    private Text populationIncrement;
    private Text scienceIncrement;
    private Text productionIncrement;


    void Start()
    {
        selfName = NameObject.GetComponent<Text>();
        level = LevelObject.GetComponent<Text>();
        budget = BudgetObject.GetComponent<Text>();
        population = PopulationObject.GetComponent<Text>();
        science = ScienceObject.GetComponent<Text>();
        production = ProductionObject.GetComponent<Text>();
        budgeIncrement = BudgeIncrementObject.GetComponent<Text>();
        populationIncrement = PopulationIncrementObject.GetComponent<Text>();
        scienceIncrement = ScienceIncrementObject.GetComponent<Text>();
        productionIncrement = ProductionIncrementObject.GetComponent<Text>();
    }

    public void SetName(string Name)
    {
        selfName.text = Name;
    }
    public void SetLevel(int Level)
    {
        level.text = Level.ToString() + " LVL";
    }

    public void SetBudget(int Budget)
    {
        budget.text = Budget.ToString() + " ₽";
    }

    public void SetPopulation(int Population)
    {
        population.text = Population.ToString() + " Ч.";
    }

    public void SetScience(int Science)
    {
        science.text = Science.ToString() + " Н.";
    }

    public void SetProduction(int Production)
    {
        production.text = Production.ToString() + " П.";
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

    public void SetGDP(float A, float B, float C, float D)
    {
        float sum = (A+B+C+D);
        float a = A / sum; float b = B / sum; float c = C / sum; float d = D / sum;
        RadialDiagramObjectA.GetComponent<Image>().fillAmount = a;
        RadialDiagramObjectB.GetComponent<Image>().fillAmount = a + b;
        RadialDiagramObjectC.GetComponent<Image>().fillAmount = a + b + c;
        RadialDiagramObjectD.GetComponent<Image>().fillAmount = a + b + c + d;
    }

}
