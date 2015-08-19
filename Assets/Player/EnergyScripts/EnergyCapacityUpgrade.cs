using UnityEngine;
using System.Collections;

public class EnergyCapacityUpgrade : MonoBehaviour {
    private const float multiplier = 3;
    void Start()
    {
        EnergyMeter meter = GetComponent<EnergyMeter>();
        meter.StartDrainTime *= multiplier;
        meter.Add(meter.StartDrainTime);
    }
    void OnDestroy()
    {
        GetComponent<EnergyMeter>().StartDrainTime /= multiplier;
    }
}
