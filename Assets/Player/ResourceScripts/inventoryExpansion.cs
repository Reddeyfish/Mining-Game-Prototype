using UnityEngine;
using System.Collections;

public class inventoryExpansion : MonoBehaviour {
    private const int amount = 50;
    void Start()
    {
        GetComponent<Inventory>().maxSize += amount;
    }
}
