using UnityEngine;
using System.Collections;

public class CrystalLight : Block {
    public override bool isMinable() { return false; }
    public override void Create()
    {
        this.transform.localPosition += new Vector3(RandomLib.RandFloatRange(0, 0.5f), RandomLib.RandFloatRange(0, 0.5f), RandomLib.RandFloatRange(-1.5f, 0.5f));
        GetComponent<Animator>().speed = RandomLib.RandFloatRange(0, 0.06f);
    }

    public override int getDataValue()
    {
        return 0;
    }

    public override blockDataType getBlockType()
    {
        return blockDataType.LIGHTBLOCK;
    }

    public override Color getColor()
    {
        return Color.white;
    }
}
