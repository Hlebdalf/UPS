using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour {
    [SerializeField]
    private long taxRate = 10000;
    
    public long GetTaxRate() {
        return taxRate;
    }
}
