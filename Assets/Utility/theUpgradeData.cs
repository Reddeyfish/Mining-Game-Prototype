using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate MonoBehaviour AddComponentDelegate(GameObject player);
public class theUpgradeData : MonoBehaviour {
    public static Dictionary<int, Upgrade> IDToUpgrade = new Dictionary<int, Upgrade>()
    {
        {1 , new Upgrade(2, 1, delegate(GameObject player) { return player.AddComponent<inventoryExpansion>(); })},
    };
}

public class Upgrade
{
    public string ComponentName;
    public int cellWidth;
    public int cellHeight;
    public AddComponentDelegate AddComponentTo;
    public Upgrade(int cellWidth, int cellHeight, AddComponentDelegate AddComponent)
    {
        this.cellWidth = cellWidth;
        this.cellHeight = cellHeight;
        this.AddComponentTo = AddComponent;
    }
}
