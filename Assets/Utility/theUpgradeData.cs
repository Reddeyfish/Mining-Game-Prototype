using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate MonoBehaviour AddComponentDelegate(GameObject player);
public class theUpgradeData : MonoBehaviour {
    public const float cellPixelWidth = 40f;
    public const float cellPixelHeight = 30f;
    public static theUpgradeData thi;
    public static Dictionary<int, Upgrade> IDToUpgrade = new Dictionary<int, Upgrade>()
    {
        {1 , new Upgrade("Inventory Upgrade", 2, 1, delegate(GameObject player) { return player.AddComponent<inventoryExpansion>(); })},
    };

    public Sprite[] sprites;

    public static Sprite IDToSprite(int ID)
    {
        return thi.sprites[ID - 1];
    }

    void Awake()
    {
        thi = this;
    }
}

public class Upgrade
{
    public string ComponentName;
    public int cellWidth;
    public int cellHeight;
    public AddComponentDelegate AddComponentTo;
    public Upgrade(string ComponentName, int cellWidth, int cellHeight, AddComponentDelegate AddComponent)
    {
        this.ComponentName = ComponentName;
        this.cellWidth = cellWidth;
        this.cellHeight = cellHeight;
        this.AddComponentTo = AddComponent;
    }
}
