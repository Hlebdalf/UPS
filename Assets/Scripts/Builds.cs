using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Builds : MonoBehaviour {

    public GameObject[] preFubs;
    private List <GameObject> objects;

    void Start() {
        objects = new List <GameObject> ();
    }

    public void BuildObject(string type) {
        switch (type) {
            case "House1":
                objects.Add(Instantiate(preFubs[0], new Vector3(0, 0, 10), preFubs[0].transform.rotation));
                break;
            case "House2":
                //objects.Add(Instantiate());
                break;
            case "Road":
                //objects.Add(Instantiate());
                break;
        }
    }
}
