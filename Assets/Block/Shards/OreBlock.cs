using UnityEngine;
using System.Collections;

public class OreBlock : ExplosiveBlock
{
    private const float albedoAlpha = 0.3f;
    protected override float getRadius() { return 2.5f; }
    protected override float detonationTime() { return 5f; }
    private const float defaultSpin = 0.06f;
    private const float defaultEmission = 1f;
    private const float detonationSpinMultiplier = 10;

    public override void setVisuals()
    {
        Transform visuals = transform.Find("Visuals");
        Vector3 colorValues = RandomLib.PerlinColor(WorldController.ColorSeedX, WorldController.ColorSeedY, (int)(transform.position.x), (int)(transform.position.y));
        hue = colorValues.x;
        mat = visuals.GetComponent<Renderer>().material;
        mat.color = HSVColor.HSVToRGB(colorValues.x, colorValues.y, colorValues.z);
        mat.SetColor(ShaderParams.emission, mat.color * defaultEmission);
        
        //add 'crystals'
        Color crystalColor = HSVColor.HSVToRGB(colorValues.x, 1, 1);
        foreach (Transform shard in visuals)
        {
            shard.localPosition = new Vector3(0.7f * (Random.value - 0.5f), 0.7f * (Random.value - 0.5f), -0.6f);
            shard.LookAt(this.transform.position);
            Light light = shard.GetComponent<Light>();
            if(light != null)
                light.color = crystalColor;
            
            Material crystalMat = shard.GetComponent<Renderer>().material;
            hue = (colorValues.x + 0.9f + 0.2f * Random.value) % 1;
            crystalMat.color = HSVColor.HSVToRGB(hue, colorValues.y, colorValues.z, albedoAlpha);
            crystalMat.SetColor("_EmissionColor", HSVColor.HSVToRGB(hue, 1, 1));
            shard.GetComponent<Animator>().speed = RandomLib.RandFloatRange(defaultSpin/2, defaultSpin/2);
        }
    }

    protected override void AlterVisuals()
    {
        Transform visuals = transform.Find("Visuals");
        foreach (Transform shard in visuals)
            shard.GetComponent<Animator>().speed *= detonationSpinMultiplier;
    }

    public override void Destroy()
    {
        SimplePool.Spawn(deathEffect, this.transform.position);
        Despawn();
    }

    public override blockDataType getBlockType()
    {
        return blockDataType.OREBLOCK;
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
