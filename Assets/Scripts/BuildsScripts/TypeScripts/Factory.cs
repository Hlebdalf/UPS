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

    public void ChangeServiceCost(long _serviceCost) {
        serviceCost = _serviceCost;
    }
    
    public void ChangeProductsRate(long _productsRate) {
        productsRate = _productsRate;
    }
    
    public void LevelUpgrade() {}
}
