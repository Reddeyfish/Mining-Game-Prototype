using UnityEngine;
using System.Collections;

public class SolidBlock : Block {
    Material mat;
    public GameObject deathEffect;
    
	// Use this for initialization
    void Awake()
    {
        mat = transform.Find("Visuals").GetComponent<Renderer>().material;
    }

	void Start () {
        transform.Find("Visuals").localScale = new Vector3(1, 1, 0.75f + 0.3f * (Random.value));
        
        Vector2 offset = Random.insideUnitSphere / 4;
        mat.SetTextureOffset("_BumpMap", offset); //only really needs to be one once to randomize all of them
	}

    public override void Create()
    {
        base.Create();
        Vector3 colorValues = RandomLib.PerlinColor(WorldController.ColorSeedX, WorldController.ColorSeedY, (int)(transform.position.x), (int)(transform.position.y));
        mat.color = HSVColor.HSVToRGB(colorValues.x, colorValues.y, colorValues.z);
    }

    public override void Destroy()
    {
        SimplePool.Spawn(deathEffect, this.transform.position);
        base.Destroy();
    }

    public override Color getColor()
    {
        return mat.color;
    }
}
