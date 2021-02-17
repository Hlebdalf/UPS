using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Factory : MonoBehaviour {
    [SerializeField]
    private long serviceCost = 10000;
    [SerializeField]
    private long productsRate = 10000;
    
    public long GetServiceCost() {
        return serviceCost;
    }
    
    public long GetProductsRate() {
        return productsRate;
    }
}
