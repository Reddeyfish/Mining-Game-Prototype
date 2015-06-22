﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate MonoBehaviour AddComponentDelegate(GameObject player, int ID);
public class theUpgradeData : MonoBehaviour {
    public const float cellPixelWidth = 40f;
    public const float cellPixelHeight = 30f;
    public static theUpgradeData thi;
    public static Upgrade[] IDToUpgrade = new Upgrade[]
    {
        new Upgrade("Inventory Space", 2, 1, 
            delegate(GameObject player, int ID) { return player.AddComponent<inventoryExpansion>(); }, 
            new Cost[]{
            new Cost(resourceType.PURECOLOR, 12, costType.ANY), 
            },
@"Expands the amount of resources you can hold 
before you have to return to base"),
        new Upgrade("Drill Toughness", 1, 2, 
            delegate(GameObject player, int ID) { return player.AddComponent<UDrillToughness>(); }, 
            new Cost[]{
            new Cost(resourceType.HARDENED, 12, costType.WHITE), 
            },
@"Blocks become more dense and harder to dig 
through as you move farther away from the main base.
A harder drill bit minimizes the effect this has on
your digging speed"),
        new Upgrade("Mining Blast", 2, 3, 
            delegate(GameObject player, int ID) {
                SpawningAbility result = player.AddComponent<SpawningAbility>();
                result.ID = ID;
                return result; }, 
            new Cost[]{
                new Cost(resourceType.PURECOLOR, 24, costType.WHITE), 
                new Cost(resourceType.UNSTABLE, 30, costType.WHITE), 
            },
@"When activated, instantly mines the eight blocks
adjacent to you as if you had used your drill"),
        new Upgrade("Energy Capacity", 2, 1, 
            delegate(GameObject player, int ID) { return player.AddComponent<EnergyCapacityUpgrade>(); }, 
            new Cost[]{
            new Cost(resourceType.PURECOLOR, 12, costType.WHITE), 
            },
@"Increases your energy capacity. A necessity for
making it back to base alive"),
        new Upgrade("Directional Mining Blast", 2, 3, 
            delegate(GameObject player, int ID) {
                SpawningAbility result = player.AddComponent<SpawningAbility>();
                result.ID = ID;
                return result; }, 
            new Cost[]{
                new Cost(resourceType.PURECOLOR, 24, costType.WHITE), 
                new Cost(resourceType.UNSTABLE, 30, costType.WHITE), 
            },
@"When activated, instantly mines eight blocks in a
straight line as if you had used your drill"),
    };

    public static Sprite IDToSprite(int ID)
    {
        return Resources.Load<Sprite>(IDToUpgrade[ID].ComponentName + "/" + IDToUpgrade[ID].ComponentName);
    }

    public static GameObject IDToUI(int ID)
    {
        return Resources.Load<GameObject>(IDToUpgrade[ID].ComponentName + "/" + IDToUpgrade[ID].ComponentName);
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
    public Cost[] costs;
    public string description;
    public Upgrade(string ComponentName, int cellWidth, int cellHeight, AddComponentDelegate AddComponent, Cost[] costs, string description)
    {
        this.ComponentName = ComponentName;
        this.cellWidth = cellWidth;
        this.cellHeight = cellHeight;
        this.AddComponentTo = AddComponent;
        this.costs = costs;
        this.description = description;
    }
}
