using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class defaultRefining : BaseDigListener {
    private Inventory inventory;
    private List<ResourceConversion> conversionTable = new List<ResourceConversion>() 
    {   new ResourceConversion(blockDataType.OREBLOCK, resourceType.PURECOLOR, 10),
        new ResourceConversion(blockDataType.EXPLOSIVE, resourceType.UNSTABLE, 10),
        new ResourceConversion(blockDataType.BOULDERINTERIOR, resourceType.HARDENED, 10),};
    public GameObject popup;
    // Use this for initialization
    protected override void Start()
    {
        inventory = GetComponent<Inventory>();
        base.Start();
    }

    public override void OnNotifyDig(Block block)
    {
        foreach (ResourceConversion conversion in conversionTable)
        {
            extractResources(block, conversion);
        }
    }

    void extractResources(Block block, ResourceConversion conversion) //blockDataType blockType, resourceType resourceType, int yield)
    {
        if (block.getBlockType() == conversion.blockType)
        {
            Color color = block.getColor();
            colorType type = colorType.RED;
            float max = color.r;
            if (color.g > max)
            {
                max = color.g;
                type = colorType.GREEN;
            }
            if (color.b > max)
            {
                max = color.b;
                type = colorType.BLUE;
            }
            int count = inventory.Add(new Resource(conversion.resourceType, type, Mathf.RoundToInt(conversion.yield * max)));
            if (count > 0)
            {
                PickupPopup UI = (SimplePool.Spawn(popup, this.transform.position) as GameObject).GetComponent<PickupPopup>();
                UI.color = type.ToColor();
                UI.count = count;
            }
        }
    }
}

public class ResourceConversion
{
    public blockDataType blockType;
    public resourceType resourceType;
    public int yield;
    public ResourceConversion(blockDataType blockType, resourceType resourceType, int yield)
    {
        this.blockType = blockType;
        this.resourceType = resourceType;
        this.yield = yield;
    }
}