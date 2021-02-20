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

    public void ChangeServiceCost(long _serviceCost) {
        serviceCost = _serviceCost;
    }
    
    public void ChangeScienceRate(long _scienceRate) {
        scienceRate = _scienceRate;
    }

    public void LevelUpgrade() {}
}
