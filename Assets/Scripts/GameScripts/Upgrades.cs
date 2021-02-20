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
}
