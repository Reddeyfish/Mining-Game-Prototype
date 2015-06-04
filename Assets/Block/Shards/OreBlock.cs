using UnityEngine;
using System.Collections;

public class OreBlock : Block
{
    private const int numCrystals = 4;
    public GameObject deathEffect;
    public override void Create()
    {
        Transform visuals = transform.Find("Visuals");
        Vector3 colorValues = RandomLib.PerlinColor(WorldController.ColorSeedX, WorldController.ColorSeedY, (int)(transform.position.x), (int)(transform.position.y));
        visuals.GetComponent<Renderer>().material.color = HSVColor.HSVToRGB(colorValues.x, colorValues.y, colorValues.z);
        
        //add 'crystals'
        Color crystalColor = HSVColor.HSVToRGB(colorValues.x, 1, 1);
        foreach (Transform shard in visuals)
        {
            shard.localPosition = new Vector3(0.7f * (Random.value - 0.5f), 0.7f * (Random.value - 0.5f), -0.6f);
            shard.LookAt(this.transform.position);
            Light light = shard.GetComponent<Light>();
            if(light != null)
                light.color = crystalColor;
            shard.GetComponent<Animator>().speed = RandomLib.RandFloatRange(0, 0.06f);
        }
    }

    public override blockDataType getBlockType()
    {
        return blockDataType.OREBLOCK;
    }

    public override void Destroy()
    {
        SimplePool.Spawn(deathEffect, this.transform.position);
 	    base.Destroy();
    }

    protected override void UpdateMap()
    {
        WorldController.UpdateBlock(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), blockDataType.LIGHTBLOCK);
    }

    public override Color getColor()
    {
        return transform.Find("Visuals").GetComponent<Renderer>().material.color;
    }

    public override float digTime() { return 1f; }
}
