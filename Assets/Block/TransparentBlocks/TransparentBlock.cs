using UnityEngine;
using System.Collections;

public class TransparentBlock : DirtBlock
{
    public override void Create()
    {
        Vector3 colorValues = RandomLib.PerlinColor(WorldController.ColorSeedX, WorldController.ColorSeedY, (int)(transform.position.x), (int)(transform.position.y));
        transform.Find("Visuals").GetComponent<Renderer>().material.SetColor(ShaderParams.color, HSVColor.HSVToRGB(colorValues.x, colorValues.y, colorValues.z, Mathf.PerlinNoise(WorldController.TransSeedX + 3 * transform.position.x / 233, ((WorldController.TransSeedY + 3 * transform.position.y / 233) - WorldController.transFrequency) / (2 - 2*WorldController.transFrequency))));
    }

    public override blockDataType getBlockType()
    {
        return blockDataType.TRANSPARENTMAP;
    }
}
