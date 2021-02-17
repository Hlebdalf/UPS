using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Science : MonoBehaviour {
    [SerializeField]
    private long serviceCost = 10000;
    [SerializeField]
    private long scienceRate = 10000;
    
    public long GetServiceCost() {
        return serviceCost;
    }
    
    public long GetScienceRate() {
        return scienceRate;
    }
}
