using UnityEngine;
using System.Collections;

public class Boulder : DirtBlock {
    public override bool isMinable() { return false; }
    public override blockDataType getBlockType()
    {
        return blockDataType.BOULDER;
    }
    protected override void UpdateMap()
    {
        WorldController.UpdateBlock(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), blockDataType.BOULDERINTERIOR);
    }

    public override void Destroy()
    {
        Debug.Log("Error; boulders are NOT minable");
    }
}