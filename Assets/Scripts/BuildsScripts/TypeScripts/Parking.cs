using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parking : MonoBehaviour {
    public bool carInParking = false;

    private void OnTriggerEnter(Collider collider) {
        if (collider.gameObject.tag == "Car") carInParking = true;
    }

    private void OnTriggerExit(Collider collider) {
        if (collider.gameObject.tag == "Car") carInParking = false;
    }
}
