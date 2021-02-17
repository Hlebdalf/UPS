using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour {
    [SerializeField]
    private long serviceCost = 10000;
    
    public long GetServiceCost() {
        return serviceCost;
    }
}
