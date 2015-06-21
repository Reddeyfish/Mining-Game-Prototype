using UnityEngine;
using System.Collections;

public class EnergyCapacityUpgrade : MonoBehaviour {
    private const int amount = 50;
    void Start()
    {
        EnergyMeter meter = GetComponent<EnergyMeter>();
        meter.StartDrainTime += amount;
        meter.Add(amount);
    }
    void OnDestroy()
    {
        GetComponent<EnergyMeter>().StartDrainTime -= amount;
    }
}
