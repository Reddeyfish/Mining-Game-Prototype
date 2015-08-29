using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//script that controls the player's on-body resource inventory

public class Inventory : BaseInventory
{
    public int maxSize;
    protected override string getKey() { return PlayerPrefKeys.inventory; }
    public GameObject inventoryFullMessage;

    void Start()
    {
        StartDisabled();
    }

    public override int Add(Resource resource)
    {
        int space = spaceRemaining();
        if (space <= 0) //inventory completely full
        {
            SimplePool.Spawn(inventoryFullMessage, this.transform.position);
            return 0;
        }
        //else there is space, add to inventory

        int count = resource.count;
        if (count >= space)
        {
            //inventory now full
            SimplePool.Spawn(inventoryFullMessage, this.transform.position);
        }

        base.Add(resource);

        return count; //calling script now does the UI stuff
    }

    public int spaceRemaining()
    {
        return maxSize - currentSize;
    }

    public void Merge(BaseInventory other)
    {
        foreach (Resource resource in Resources)
            other.Add(resource);
        Wipe();
    }

    public override void Wipe()
    {
        base.Wipe();
        currentSize = 0;
    }
}