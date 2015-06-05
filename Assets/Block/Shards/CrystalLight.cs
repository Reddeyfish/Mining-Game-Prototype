using UnityEngine;
using System.Collections;

public class CrystalLight : Block {
    private const float albedoAlpha = 0.3f;
    public override bool isMinable() { return false; }
    public override void Create()
    {
        this.transform.localPosition += new Vector3(RandomLib.RandFloatRange(0, 0.5f), RandomLib.RandFloatRange(0, 0.5f), RandomLib.RandFloatRange(-1.5f, 0.5f));
        GetComponent<Animator>().speed = RandomLib.RandFloatRange(0, 0.06f);
        Vector3 colorValues = RandomLib.PerlinColor(WorldController.ColorSeedX, WorldController.ColorSeedY, (int)(transform.position.x), (int)(transform.position.y));
        Material mat = GetComponent<Renderer>().material;
        mat.color = HSVColor.HSVToRGB(colorValues.x, colorValues.y, colorValues.z, albedoAlpha);
        mat.SetColor("_EmissionColor", HSVColor.HSVToRGB(colorValues.x, 1, 1));
    }

    public override bool getDataValue()
    {
        return false;
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
