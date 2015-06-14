using UnityEngine;
using System.Collections;

//upgrade

public class UDrillToughness : MonoBehaviour {
    private const int amount = 100;
    void Start()
    {
        GetComponent<DefaultDiggingScript>().digPower += amount;
    }
    void OnDestroy()
    {
        GetComponent<DefaultDiggingScript>().digPower -= amount;
    }
}
