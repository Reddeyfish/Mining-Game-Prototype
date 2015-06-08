using UnityEngine;
using System.Collections;

public class Boulder : DirtBlock {
    public override bool isMinable() { return false; }
    public override blockDataType getBlockType()
    {
        return blockDataType.BOULDER;
    }
}