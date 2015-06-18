using UnityEngine;
using System.Collections;

public class PlusExplosiveBlock : ExplosiveBlock {
    private const float range = 3.5f;

    protected override bool willTrigger(Block block)
    {
        if (Mathf.RoundToInt(block.transform.position.x) == Mathf.RoundToInt(this.transform.position.x))
        {
            return Mathf.Abs(block.transform.position.y - this.transform.position.y) < range;
        }
        else if (Mathf.RoundToInt(block.transform.position.y) == Mathf.RoundToInt(this.transform.position.y))
        {
            return Mathf.Abs(block.transform.position.x - this.transform.position.x) < range;
        }
        return false;
    }

    public override blockDataType getBlockType()
    {
        return blockDataType.PLUSEXPLOSIVE;
    }
}
