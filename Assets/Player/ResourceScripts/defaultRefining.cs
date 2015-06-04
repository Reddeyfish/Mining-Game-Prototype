using UnityEngine;
using System.Collections;

public class defaultRefining : BaseDigListener {
    private Inventory inventory;
    private const float yield = 20.0f;
    public GameObject popup;
    // Use this for initialization
    protected override void Start()
    {
        inventory = GetComponent<Inventory>();
        base.Start();
    }

    public override void OnNotify(Block block)
    {
        if (block.getBlockType() == blockDataType.OREBLOCK)
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
            int count = inventory.Add(new Resource(resourceType.PURECOLOR, type, Mathf.RoundToInt(yield * max)));
            if (count > 0)
            {
                PickupPopup UI = (SimplePool.Spawn(popup, this.transform.position) as GameObject).GetComponent<PickupPopup>();
                UI.color = type.ToColor();
                UI.count = count;
            }
        }
    }
}
