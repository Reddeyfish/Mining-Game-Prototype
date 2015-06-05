using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class theUpgradeData : MonoBehaviour {
    public static Dictionary<int, Upgrade> IDToUpgrade = new Dictionary<int, Upgrade>()
    {
        {1 , new Upgrade(2, 1)},
    };

    public static MonoBehaviour IDToComponent(GameObject player, int ID)
    {
        switch (ID)
        {
            case 1:
                return player.AddComponent<inventoryExpansion>();
            default:
                Debug.Log("Error; ID not registered");
                return null;
        }
    }
}

public class Upgrade
{
    public string ComponentName;
    public int cellWidth;
    public int cellHeight;

    public Upgrade(int cellWidth, int cellHeight)
    {
        this.cellWidth = cellWidth;
        this.cellHeight = cellHeight;
    }
}
