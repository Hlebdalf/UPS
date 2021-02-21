using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upgrades : MonoBehaviour {
    private Economy EconomyClass;
    private Builds BuildsClass;
    private People PeopleClass;

    private void Awake() {
        EconomyClass = Camera.main.GetComponent <Economy> ();
        BuildsClass = Camera.main.GetComponent <Builds> ();
        PeopleClass = Camera.main.GetComponent <People> ();
    }

    // Глобальные улучшения города
    public void TheReformOfTheRoad() {
        for (int i = 0; i < BuildsClass.commerces.Count; ++i) {
            if (BuildsClass.commerces[i].GetComponent <Factory> ()) {
                Factory factoryClass = BuildsClass.commerces[i].GetComponent <Factory> ();
                // factoryClass.ChangeProductsRate(x);
            }
            else if (BuildsClass.commerces[i].GetComponent <Science> ()) {
                Science scienceClass = BuildsClass.commerces[i].GetComponent <Science> ();
                // scienceClass.ChangeScienceRate(x);
            }
        }
    }

    public void ImprovingTheQualityOfHolograms() {
        for (int i = 0; i < PeopleClass.objects.Count; ++i) {
            Passport passportClass = PeopleClass.objects[i].GetComponent <Passport> ();
            // passportClass.qualityHologram += k;
        }
    }

    public void EGovernmentServices() {
        for (int i = 0; i < PeopleClass.objects.Count; ++i) {
            Passport passportClass = PeopleClass.objects[i].GetComponent <Passport> ();
            
        }
    }

    // Глобальные улучшения науки
    public void Science_EducationReform() {
        for (int i = 0; i < BuildsClass.commerces.Count; ++i) {
            if (BuildsClass.commerces[i].GetComponent <Science> ()) {
                Science scienceClass = BuildsClass.commerces[i].GetComponent <Science> ();
                // scienceClass.ChangeScienceRate(x);
            }
        }
    }

    public void Science_IncreasedFunding() {
        for (int i = 0; i < BuildsClass.commerces.Count; ++i) {
            if (BuildsClass.commerces[i].GetComponent <Science> ()) {
                Science scienceClass = BuildsClass.commerces[i].GetComponent <Science> ();
                // scienceClass.ChangeScienceRate(x);
                // scienceClass.ChangeServiceCost(x);
            }
        }
    }
    
    public void Science_PromotionOfScience() {
        for (int i = 0; i < BuildsClass.commerces.Count; ++i) {
            if (BuildsClass.commerces[i].GetComponent <Science> ()) {
                Science scienceClass = BuildsClass.commerces[i].GetComponent <Science> ();
                // scienceClass.ChangeScienceRate(x);
            }
        }
    }

    public void Science_ForeignSpecialists() {
        for (int i = 0; i < BuildsClass.commerces.Count; ++i) {
            if (BuildsClass.commerces[i].GetComponent <Science> ()) {
                Science scienceClass = BuildsClass.commerces[i].GetComponent <Science> ();
                // scienceClass.ChangeScienceRate(x);
            }
        }
    }

    public void Science_StudentExchangePrograms() {
        for (int i = 0; i < BuildsClass.commerces.Count; ++i) {
            if (BuildsClass.commerces[i].GetComponent <Science> ()) {
                Science scienceClass = BuildsClass.commerces[i].GetComponent <Science> ();
                // scienceClass.ChangeScienceRate(x);
            }
        }
    }

    // Глобальные улучшения заводов
    public void Factory_NewProductionMethods() {
        for (int i = 0; i < BuildsClass.commerces.Count; ++i) {
            if (BuildsClass.commerces[i].GetComponent <Factory> ()) {
                Factory factoryClass = BuildsClass.commerces[i].GetComponent <Factory> ();
                // factoryClass.ChangeProductsRate(x);
            }
        }
    }

    public void Factory_NewPartners() {}

    public void Factory_NewSuppliers() {
        for (int i = 0; i < BuildsClass.commerces.Count; ++i) {
            if (BuildsClass.commerces[i].GetComponent <Factory> ()) {
                Factory factoryClass = BuildsClass.commerces[i].GetComponent <Factory> ();
                // factoryClass.ChangeServiceCost(x);
            }
        }
    }
    
    public void Factory_IncreasedFunding() {
        for (int i = 0; i < BuildsClass.commerces.Count; ++i) {
            if (BuildsClass.commerces[i].GetComponent <Factory> ()) {
                Factory factoryClass = BuildsClass.commerces[i].GetComponent <Factory> ();
                // factoryClass.ChangeProductsRate(x);
                // factoryClass.ChangeServiceCost(x);
            }
        }
    }

    // Глобальные улучшения домов
    public void House_CentralHeatingSystem() {
        for (int i = 0; i < PeopleClass.objects.Count; ++i) {
            Passport passportClass = PeopleClass.objects[i].GetComponent <Passport> ();
            // passportClass.satisfaction += k;
        }
    }

    // Глобальные улучшения людей
    public void People_SimplificationOfBorderControl() {}

    public void People_AdvertisingOfTheCity() {}

    public void People_BenefitsForLargeFamilies() {}

    public void People_PersonalIncomeTaxReduction() {}

    public void People_PromotionOfLargeFamilies() {}

    public void People_PreferentialHousingServices() {}
}
