using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Statistic : MonoBehaviour {
    public List <GameObject> CommerceNumObjects = new List <GameObject>(4);
    public List <GameObject> CommerceIncomeObjects = new List <GameObject>(4);
    public List <GameObject> CommerceWorkplacesObjects = new List <GameObject>(4);

    public List <GameObject> ProductionNumObjects = new List <GameObject>(4);
    public List <GameObject> ProductionOutcomeObjects = new List <GameObject>(4);
    public List <GameObject> ProductionWorkplacesObjects = new List <GameObject>(4);

    public List <GameObject> ScienceNumObjects = new List <GameObject>(4);
    public List <GameObject> ScienceOutcomeObjects = new List <GameObject>(4);
    public List <GameObject> ScienceWorkplacesObjects = new List <GameObject>(4);

    public List <GameObject> HouseNumObjects = new List <GameObject>(4);
    public List <GameObject> HouseIncomeObjects = new List <GameObject>(4);
    public List <GameObject> HouseOutcomeObjects = new List <GameObject>(4);
    public List <GameObject> HouseOccupiedPlacesObjects = new List <GameObject>(4);
    public List <GameObject> HouseAllPlacesObjects = new List <GameObject>(4);

    public List <GameObject> PostersNumObjects = new List <GameObject>(4);
    public List <GameObject> PostersOutcomeObjects = new List <GameObject>(4);

    public List <GameObject> AVGLoyaltyObjects = new List <GameObject>(4);

    public List <GameObject> BudgeIncrementObjects = new List <GameObject>(4);
    public List <GameObject> PopulationIncrementObjects = new List <GameObject>(4);
    public List <GameObject> ScienceIncrementObjects = new List <GameObject>(4);
    public List <GameObject> ProductionIncrementObjects = new List <GameObject>(4);

    private List<Text> CommerceNumTexts = new List <Text>(4);
    private List<Text> CommerceIncomeTexts = new List <Text>(4);
    private List<Text> CommerceWorkplacesTexts = new List <Text>(4);

    private List<Text> ProductionNumTexts = new List <Text>(4);
    private List<Text> ProductionOutcomeTexts = new List <Text>(4);
    private List<Text> ProductionWorkplacesTexts = new List <Text>(4);

    private List<Text> ScienceNumTexts = new List <Text>(4);
    private List<Text> ScienceOutcomeTexts = new List <Text>(4);
    private List<Text> ScienceWorkplacesTexts = new List <Text>(4);

    private List<Text> HouseNumTexts = new List <Text>(4);
    private List<Text> HouseIncomeTexts = new List <Text>(4);
    private List<Text> HouseOutcomeTexts = new List <Text>(4);
    private List<Text> HouseOccupiedPlacesTexts = new List <Text>(4);
    private List<Text> HouseAllPlacesTexts = new List <Text>(4);

    private List<Text> PostersNumTexts = new List <Text>(4);
    private List<Text> PostersOutcomeTexts = new List <Text>(4);

    private List<Text> AVGLoyaltyTexts = new List <Text>(4);

    private List<Text> BudgeIncrementTexts = new List <Text>(4);
    private List<Text> PopulationIncrementTexts = new List <Text>(4);
    private List<Text> ScienceIncrementTexts = new List <Text>(4);
    private List<Text> ProductionIncrementTexts = new List <Text>(4);

    private void Awake() {
        for (int i = 0; i < 4; ++i) {
            CommerceNumTexts.Add(CommerceNumObjects[i].GetComponent<Text>());
            CommerceIncomeTexts.Add(CommerceIncomeObjects[i].GetComponent<Text>());
            CommerceWorkplacesTexts.Add(CommerceWorkplacesObjects[i].GetComponent<Text>());

            ProductionNumTexts.Add(ProductionNumObjects[i].GetComponent<Text>());
            ProductionOutcomeTexts.Add(ProductionOutcomeObjects[i].GetComponent<Text>());
            ProductionWorkplacesTexts.Add(ProductionWorkplacesObjects[i].GetComponent<Text>());

            ScienceNumTexts.Add(ScienceNumObjects[i].GetComponent<Text>());
            ScienceOutcomeTexts.Add(ScienceOutcomeObjects[i].GetComponent<Text>());
            ScienceWorkplacesTexts.Add(ScienceWorkplacesObjects[i].GetComponent<Text>());

            HouseNumTexts.Add(HouseNumObjects[i].GetComponent<Text>());
            HouseOutcomeTexts.Add(HouseOutcomeObjects[i].GetComponent<Text>());
            HouseIncomeTexts.Add(HouseIncomeObjects[i].GetComponent<Text>());
            HouseOccupiedPlacesTexts.Add(HouseOccupiedPlacesObjects[i].GetComponent<Text>());
            HouseAllPlacesTexts.Add(HouseAllPlacesObjects[i].GetComponent<Text>());

            PostersNumTexts.Add(PostersNumObjects[i].GetComponent<Text>());
            PostersOutcomeTexts.Add(PostersOutcomeObjects[i].GetComponent<Text>());

            AVGLoyaltyTexts.Add(AVGLoyaltyObjects[i].GetComponent<Text>());

            BudgeIncrementTexts.Add(BudgeIncrementObjects[i].GetComponent<Text>());
            PopulationIncrementTexts.Add(PopulationIncrementObjects[i].GetComponent<Text>());
            ScienceIncrementTexts.Add(PopulationIncrementObjects[i].GetComponent<Text>());
            ProductionIncrementTexts.Add(ScienceIncrementObjects[i].GetComponent<Text>());
        }
    }

    public void SetCommerceNum(List <int> input) {
        for (int i = 0; i < 4; ++i) CommerceNumTexts[i].text = input[i].ToString();
    }

    public void SetCommerceIncome(List <long> input) {
        for (int i = 0; i < 4; ++i) CommerceIncomeTexts[i].text = input[i].ToString();
    }

    public void SetCommerceWorkplaces(List <int> input) {
        for (int i = 0; i < 4; ++i) CommerceWorkplacesTexts[i].text = input[i].ToString();
    }

    public void SetProductionNum(List <int> input) {
        for (int i = 0; i < 4; ++i) ProductionNumTexts[i].text = input[i].ToString();
    }

    public void SetProductionOutcome(List <long> input) {
        for (int i = 0; i < 4; ++i) ProductionOutcomeTexts[i].text = input[i].ToString();
    }

    public void SetProductionWorkplaces(List <int> input) {
        for (int i = 0; i < 4; ++i) ProductionWorkplacesTexts[i].text = input[i].ToString();
    }

    public void SetScienceNum(List <int> input) {
        for (int i = 0; i < 4; ++i) ScienceNumTexts[i].text = input[i].ToString();
    }

    public void SetScienceOutcome(List <long> input) {
        for (int i = 0; i < 4; ++i) ScienceOutcomeTexts[i].text = input[i].ToString();
    }

    public void SetScienceWorkplaces(List <int> input) {
        for (int i = 0; i < 4; ++i) ScienceWorkplacesTexts[i].text = input[i].ToString();
    }

    public void SetHouseNum(List <int> input) {
        for (int i = 0; i < 4; ++i) HouseNumTexts[i].text = input[i].ToString();
    }

    public void SetHouseOutcome(List <long> input) {
        for (int i = 0; i < 4; ++i) HouseOutcomeTexts[i].text = input[i].ToString();
    }

    public void SetHouseIncome(List <long> input) {
        for (int i = 0; i < 4; ++i) HouseIncomeTexts[i].text = input[i].ToString();
    }

    public void SetHouseOccupiedPlaces(List <int> input) {
        for (int i = 0; i < 4; ++i) HouseOccupiedPlacesTexts[i].text = input[i].ToString();
    }

    public void SetHouseAllPlaces(List <int> input) {
        for (int i = 0; i < 4; ++i) HouseAllPlacesTexts[i].text = input[i].ToString();
    }

    public void SetPostersNum(List <int> input) {
        for (int i = 0; i < 4; ++i) PostersNumTexts[i].text = input[i].ToString();
    }

    public void SetPostersOutcome(List <long> input) {
        for (int i = 0; i < 4; ++i) PostersOutcomeTexts[i].text = input[i].ToString();
    }

    public void SetAVGLoyalty(List <int> input) {
        for (int i = 0; i < 4; ++i) AVGLoyaltyTexts[i].text = input[i].ToString();
    }
    
    public void SetBudgetIncrement(List <long> input) {
        for (int i = 0; i < 4; ++i) BudgeIncrementTexts[i].text = input[i].ToString();
    }

    public void SetPopulationIncrement(List <int> input) {
        for (int i = 0; i < 4; ++i) PopulationIncrementTexts[i].text = input[i].ToString();
    }

    public void SetScinceIncrement(List <long> input) {
        for (int i = 0; i < 4; ++i) ScienceIncrementTexts[i].text = input[i].ToString();
    }

    public void SetProductionIncrement(List <long> input) {
        for (int i = 0; i < 4; ++i) ProductionIncrementTexts[i].text = input[i].ToString();
    }
}
