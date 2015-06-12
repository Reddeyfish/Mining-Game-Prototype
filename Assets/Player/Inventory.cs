using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//script that controls the player's on-body resource inventory

public class Inventory : BaseInventory
{
    public int maxSize;
    protected override string getKey() { return PlayerPrefKeys.inventory; }
    public GameObject inventoryFullMessage;

    void Awake()
    {
        Awaken();
    }

    public override int Add(Resource resource)
    {
        int count = spaceRemaining();
        if (count > resource.count)
        {
            count = resource.count;
        }
        else
        {
            //inventory full
            SimplePool.Spawn(inventoryFullMessage, this.transform.position);
        }
        base.Add(resource.Resize(count));
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

    public void Wipe()
    {
        resources = new List<Resource>();
        currentSize = 0;
    }
}