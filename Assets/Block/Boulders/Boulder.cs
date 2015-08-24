using UnityEngine;
using System.Collections;

public class Boulder : SolidBlock {
    public override bool isMinable() { return false; }
    public override blockDataType getBlockType()
    {
        return blockDataType.BOULDER;
    }
    public override void UpdateMap()
    {
        WorldController.ModifyBlock(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), blockDataType.BOULDERINTERIOR, false);
    }

    public override void Destroy()
    {
        Debug.Log("Error; boulders are NOT minable");
    }

    public override float getImpactTolerance()
    {
        return 40f;
    }
}