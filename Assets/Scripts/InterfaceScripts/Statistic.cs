using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statistic : MonoBehaviour
{
    public List<GameObject> GameObjects = new List<GameObject>(40);
    public struct SocText
    {
        public Text Class1;
        public Text Class2;
        public Text Class3;
        public Text Class4;
    }

    private SocText houseNum;
    private SocText scienceNum;
    private SocText productionNum;
    private SocText budgeIncrement;
    private SocText populationIncrement;
    private SocText scienceIncrement;
    private SocText productionIncrement;
    private SocText avgLoyality;
    private SocText postersNum;
    private SocText commerceNum;

    private void Awake()
    {
        avgLoyality.Class1 = GameObjects[0].GetComponent<Text>();
        commerceNum.Class1 = GameObjects[4].GetComponent<Text>();
        postersNum.Class1 = GameObjects[8].GetComponent<Text>();
        houseNum.Class1 = GameObjects[12].GetComponent<Text>();
        scienceNum.Class1 = GameObjects[16].GetComponent<Text>();
        productionNum.Class1 = GameObjects[20].GetComponent<Text>();
        budgeIncrement.Class1 = GameObjects[24].GetComponent<Text>();
        populationIncrement.Class1 = GameObjects[28].GetComponent<Text>();
        scienceIncrement.Class1 = GameObjects[32].GetComponent<Text>();
        productionIncrement.Class1 = GameObjects[36].GetComponent<Text>();

        avgLoyality.Class2 = GameObjects[1].GetComponent<Text>();
        commerceNum.Class2 = GameObjects[5].GetComponent<Text>();
        postersNum.Class2 = GameObjects[9].GetComponent<Text>();
        houseNum.Class2 = GameObjects[13].GetComponent<Text>();
        scienceNum.Class2 = GameObjects[17].GetComponent<Text>();
        productionNum.Class2 = GameObjects[21].GetComponent<Text>();
        budgeIncrement.Class2 = GameObjects[25].GetComponent<Text>();
        populationIncrement.Class2 = GameObjects[29].GetComponent<Text>();
        scienceIncrement.Class2 = GameObjects[33].GetComponent<Text>();
        productionIncrement.Class2 = GameObjects[37].GetComponent<Text>();

        avgLoyality.Class3 = GameObjects[2].GetComponent<Text>();
        commerceNum.Class3 = GameObjects[6].GetComponent<Text>();
        postersNum.Class3 = GameObjects[10].GetComponent<Text>();
        houseNum.Class3 = GameObjects[14].GetComponent<Text>();
        scienceNum.Class3 = GameObjects[18].GetComponent<Text>();
        productionNum.Class3 = GameObjects[22].GetComponent<Text>();
        budgeIncrement.Class3 = GameObjects[26].GetComponent<Text>();
        populationIncrement.Class3 = GameObjects[30].GetComponent<Text>();
        scienceIncrement.Class3 = GameObjects[34].GetComponent<Text>();
        productionIncrement.Class3 = GameObjects[38].GetComponent<Text>();

        avgLoyality.Class4 = GameObjects[3].GetComponent<Text>();
        commerceNum.Class4 = GameObjects[7].GetComponent<Text>();
        postersNum.Class4 = GameObjects[11].GetComponent<Text>();
        houseNum.Class4 = GameObjects[15].GetComponent<Text>();
        scienceNum.Class4 = GameObjects[19].GetComponent<Text>();
        productionNum.Class4 = GameObjects[23].GetComponent<Text>();
        budgeIncrement.Class4 = GameObjects[27].GetComponent<Text>();
        populationIncrement.Class4 = GameObjects[31].GetComponent<Text>();
        scienceIncrement.Class4 = GameObjects[35].GetComponent<Text>();
        productionIncrement.Class4 = GameObjects[39].GetComponent<Text>();
    }

    public void SetCommerceNum(int CommerceNum, int Class)
    {   
        switch (Class) {
            case 1:
                commerceNum.Class1.text = CommerceNum.ToString() ;
                break;
            case 2:
                commerceNum.Class2.text = CommerceNum.ToString() ;
                break;
            case 3:
                commerceNum.Class3.text = CommerceNum.ToString() ;
                break;
            case 4:
                commerceNum.Class4.text = CommerceNum.ToString() ;
                break;
        }
    }
    public void HouseNum(int HouseNum, int Class)
    {
        switch (Class)
        {
            case 1:
                houseNum.Class1.text = HouseNum.ToString() ;
                break;
            case 2:
                houseNum.Class2.text = HouseNum.ToString() ;
                break;
            case 3:
                houseNum.Class3.text = HouseNum.ToString() ;
                break;
            case 4:
                houseNum.Class4.text = HouseNum.ToString() ;
                break;
        }
    }
    public void SetAVGLoyality(int AVGLoyality, int Class)
    {
        switch (Class)
        {
            case 1:
                avgLoyality.Class1.text = AVGLoyality.ToString();
                break;
            case 2:
                avgLoyality.Class2.text = AVGLoyality.ToString();
                break;
            case 3:
                avgLoyality.Class3.text = AVGLoyality.ToString();
                break;
            case 4:
                avgLoyality.Class4.text = AVGLoyality.ToString();
                break;
        } 
    }

    public void SetPostersNum(int PostersNum, int Class)
    {
        switch (Class)
        {
            case 1:
                postersNum.Class1.text = PostersNum.ToString() ;
                break;
            case 2:
                postersNum.Class2.text = PostersNum.ToString() ;
                break;
            case 3:
                postersNum.Class3.text = PostersNum.ToString() ;
                break;
            case 4:
                postersNum.Class4.text = PostersNum.ToString() ;
                break;
        }
    }

    public void SetScienceNum(long ScienceNum, int Class)
    {
        switch (Class)
        {
            case 1:
                scienceNum.Class1.text = ScienceNum.ToString() ;
                break;
            case 2:
                scienceNum.Class2.text = ScienceNum.ToString() ;
                break;
            case 3:
                scienceNum.Class3.text = ScienceNum.ToString() ;
                break;
            case 4:
                scienceNum.Class4.text = ScienceNum.ToString() ;
                break;
        }
        
    }

    public void SetProductionNum(long ProductionNum, int Class)
    {
        switch (Class)
        {
            case 1:
                productionNum.Class1.text = ProductionNum.ToString() ;
                break;
            case 2:
                productionNum.Class2.text = ProductionNum.ToString() ;
                break;
            case 3:
                productionNum.Class3.text = ProductionNum.ToString() ;
                break;
            case 4:
                productionNum.Class4.text = ProductionNum.ToString() ;
                break;
        }   
    }

    public void SetBudgetIncrement(long BudgeIncrement, int Class)
    {
        switch (Class)
        {
            case 1:
                budgeIncrement.Class1.text = BudgeIncrement.ToString();
                break;
            case 2:
                budgeIncrement.Class2.text = BudgeIncrement.ToString();
                break;
            case 3:
                budgeIncrement.Class3.text = BudgeIncrement.ToString();
                break;
            case 4:
                budgeIncrement.Class4.text = BudgeIncrement.ToString();
                break;
        }   
    }

    public void SetPopulation(int PopulationIncrement, int Class)
    {
        switch (Class)
        {
            case 1:
                populationIncrement.Class1.text = PopulationIncrement.ToString();
                break;
            case 2:
                populationIncrement.Class2.text = PopulationIncrement.ToString();
                break;
            case 3:
                populationIncrement.Class3.text = PopulationIncrement.ToString();
                break;
            case 4:
                populationIncrement.Class4.text = PopulationIncrement.ToString();
                break;
        }      
    }

    public void SetScienceIncrement(long ScienceIncrement, int Class)
    {
        switch (Class)
        {
            case 1:
                scienceIncrement.Class1.text = ScienceIncrement.ToString();
                break;
            case 2:
                scienceIncrement.Class2.text = ScienceIncrement.ToString();
                break;
            case 3:
                scienceIncrement.Class3.text = ScienceIncrement.ToString();
                break;
            case 4:
                scienceIncrement.Class4.text = ScienceIncrement.ToString();
                break;
        }   
    }

    public void SetProductionIncrement(long ProductionIncrement, int Class)
    {
        switch (Class)
        {
            case 1:
                productionIncrement.Class1.text = ProductionIncrement.ToString();
                break;
            case 2:
                productionIncrement.Class2.text = ProductionIncrement.ToString();
                break;
            case 3:
                productionIncrement.Class3.text = ProductionIncrement.ToString();
                break;
            case 4:
                productionIncrement.Class4.text = ProductionIncrement.ToString();
                break;
        }
        
    }
}
