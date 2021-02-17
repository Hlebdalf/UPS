using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class House : MonoBehaviour {
    [SerializeField]
    private long taxRate = 10000;
    private long serviceCost = 10000;
    
    public long GetTaxRate() {
        return taxRate;
    }
    
    public long GetServiceCost() {
        return serviceCost;
    }
}
