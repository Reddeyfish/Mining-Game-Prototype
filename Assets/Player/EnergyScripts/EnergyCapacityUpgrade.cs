using UnityEngine;
using System.Collections;

public class EnergyCapacityUpgrade : MonoBehaviour {
    private const int amount = 50;
    void Start()
    {
        GetComponent<EnergyMeter>().StartDrainTime += amount;
    }
    void OnDestroy()
    {
        GetComponent<EnergyMeter>().StartDrainTime -= amount;
    }
}
