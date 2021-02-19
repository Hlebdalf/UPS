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

    void Awake()
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
        if (Name.Length > 0) selfName.text = Name;
    }
    public void SetLevel(int Level)
    {
        level.text = Level + " LVL";
    }

    public void SetBudget(long Budget)
    {
        budget.text = Budget.ToString();
    }

    public void SetPopulation(int Population)
    {
        population.text = Population.ToString();
    }

    public void SetScience(long Science)
    {
        science.text = Science.ToString();
    }

    public void SetProduction(long Production)
    {
        production.text = Production.ToString();
    }

    public void SetBudgetIncrement(long BudgeIncrement)
    {
        budgeIncrement.text = BudgeIncrement.ToString();
    }

    public void SetPopulationIncrement(int PopulationIncrement)
    {
        populationIncrement.text = PopulationIncrement.ToString();
    }

    public void SetScienceIncrement(long ScienceIncrement)
    {
        scienceIncrement.text = ScienceIncrement.ToString();
    }

    public void SetProductionIncrement(long ProductionIncrement)
    {
        productionIncrement.text = ProductionIncrement.ToString();
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
