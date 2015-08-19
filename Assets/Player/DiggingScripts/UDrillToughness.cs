using UnityEngine;
using System.Collections;

//upgrade

public class UDrillToughness : MonoBehaviour {
    private const float multiplier = 2;
    void Start()
    {
        GetComponent<DefaultDiggingScript>().digPower *= multiplier;
    }
    void OnDestroy()
    {
        GetComponent<DefaultDiggingScript>().digPower /= multiplier;
    }
}
