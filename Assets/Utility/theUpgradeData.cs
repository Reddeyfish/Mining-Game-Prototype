using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public delegate MonoBehaviour AddUpgradeComponentDelegate(GameObject player, int ID);
public class theUpgradeData : MonoBehaviour {
    public const float cellPixelWidth = 40f;
    public const float cellPixelHeight = 30f;
    public static theUpgradeData thi;
    public static Upgrade[] IDToUpgrade = new Upgrade[]  //static because const hates delegates; is constant
    {
        new Upgrade("Energy Capacity", 2, 1, 
            delegate(GameObject player, int ID) { return player.AddComponent<EnergyCapacityUpgrade>(); }, 
            new Cost[]{
            new Cost(resourceType.PURECOLOR, 80, costType.GREEN), 
            },
@"Increases your energy capacity. A necessity for
making it back to base alive"),
        new Upgrade("Inventory Space", 2, 1, 
            delegate(GameObject player, int ID) { return player.AddComponent<inventoryExpansion>(); }, 
            new Cost[]{
            new Cost(resourceType.PURECOLOR, 80, costType.BLUE), 
            },
@"Expands the amount of resources you can hold 
before you have to return to base"),
        new Upgrade("Drill Toughness", 1, 2, 
            delegate(GameObject player, int ID) { return player.AddComponent<UDrillToughness>(); }, 
            new Cost[]{
            new Cost(resourceType.PURECOLOR, 80, costType.RED), 
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
                new Cost(resourceType.PURECOLOR, 160, costType.ANY), 
                new Cost(resourceType.UNSTABLE, 80, costType.RED), 
            },
@"When activated, instantly mines the eight blocks
adjacent to you as if you had used your drill"),
        new Upgrade("Directional Mining Blast", 2, 3, 
            delegate(GameObject player, int ID) {
                SpawningAbility result = player.AddComponent<SpawningAbility>();
                result.ID = ID;
                return result; }, 
            new Cost[]{
                new Cost(resourceType.PURECOLOR, 160, costType.BLUE), 
                new Cost(resourceType.UNSTABLE, 80, costType.BLUE), 
            },
@"When activated, instantly mines eight blocks in a
straight line as if you had used your drill"),
       new Upgrade("Grappling Drone", 3, 2, 
            delegate(GameObject player, int ID) {
                LaunchingAbility result = player.AddComponent<LaunchingAbility>();
                result.ID = ID;
                return result; }, 
            new Cost[]{
                new Cost(resourceType.PURECOLOR, 160, costType.GREEN), 
                new Cost(resourceType.UNSTABLE, 80, costType.GREEN), 
            },
@"Grapples onto a block, allowing it to be moved and used as a wrecking ball."),
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
    public AddUpgradeComponentDelegate AddComponentTo;
    public Cost[] costs;
    public string description;
    public Upgrade(string ComponentName, int cellWidth, int cellHeight, AddUpgradeComponentDelegate AddComponent, Cost[] costs, string description)
    {
        this.ComponentName = ComponentName;
        this.cellWidth = cellWidth;
        this.cellHeight = cellHeight;
        this.AddComponentTo = AddComponent;
        this.costs = costs;
        this.description = description;
    }
}
