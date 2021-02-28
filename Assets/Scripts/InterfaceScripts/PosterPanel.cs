using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PosterPanel : MonoBehaviour
{
    public Text BuildTypeText;
    public Text OccupiedPlacesText;
    public Text AllPlacesText;
    public Text PostersNumText;
    public Text AllPostersNumText;
    public Text AVGLoyaltyText;
    public Text IncomeText;
    public Text IncomeTypeText;
    public Text OutcomeText;
    public Text PosterCostText;
    
    public void SetBuildType(string type)
    {
        BuildTypeText.text = type;
    }
    public void SetOccupiedPlaces(int input)
    {
        OccupiedPlacesText.text = input.ToString();
    }
    public void SetAllPlaces(int input)
    {
        AllPlacesText.text = input.ToString();
    }
    public void SetPostersNum(int input)
    {
        PostersNumText.text = input.ToString();
    }
    public void SetAllPostersNum(int input)
    {
        AllPostersNumText.text = input.ToString();
    }
    public void SetAVGLoyalty(int input)
    {
        AVGLoyaltyText.text = input.ToString();
    }
    public void SetIncome(long input)
    {
        IncomeText.text = input.ToString();
    }
    public void SetIncomeType(string type)
    {
        IncomeTypeText.text = type;
    }
    public void SetOutcome(long input)
    {
        OutcomeText.text = input.ToString();
    }
    public void SetPosterCost(long input)
    {
        PosterCostText.text = input.ToString();
    }
}
