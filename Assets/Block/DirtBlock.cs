using UnityEngine;
using System.Collections;

public class DirtBlock : Block {

    public GameObject deathEffect;

	// Use this for initialization
	void Start () {
        transform.Find("Visuals").localScale = new Vector3(1, 1, 0.75f + 0.3f * (Random.value));
        Material material = transform.Find("Visuals").GetComponent<Renderer>().material;
        Vector2 offset = Random.insideUnitSphere / 4;
        material.SetTextureOffset("_BumpMap", offset); //only really needs to be one once to randomize all of them
        material.SetTextureOffset("_MainTex", offset);
	}

    public override void Create()
    {
        Vector3 colorValues = RandomLib.PerlinColor(WorldController.ColorSeedX, WorldController.ColorSeedY, (int)(transform.position.x), (int)(transform.position.y));
        transform.Find("Visuals").GetComponent<Renderer>().material.color = HSVColor.HSVToRGB(colorValues.x, colorValues.y, colorValues.z);
        
    }

    public override void Destroy()
    {
        SimplePool.Spawn(deathEffect, this.transform.position);
        base.Destroy();
    }

    public override Color getColor()
    {
        return transform.Find("Visuals").GetComponent<Renderer>().material.color;
    }
}
