using UnityEngine;
using System.Collections;

public class BoulderInterior : Block
{
    Color color;
    Rigidbody rigid;
    private const float accelScalar = 0.05f;
    private const float maxSpeed = 5f;
	// Use this for initialization
	void Awake () {
        rigid = transform.Find("Cube").GetComponent<Rigidbody>();
	}

    public override void Create() //change to create once I get that set up
    {
        setHierarchy();
        rigid.angularVelocity = (accelScalar / 2) * Random.insideUnitSphere;
        Vector3 colorValues = RandomLib.PerlinColor(WorldController.ColorSeedX, WorldController.ColorSeedY, (int)(transform.position.x), (int)(transform.position.y));
        
        rigid.GetComponent<Renderer>().material.color = HSVColor.HSVToRGB(colorValues.x, 0.75f, 0.25f);
        
        ParticleSystem particles = rigid.GetComponent<ParticleSystem>();
        particles.Stop();
        particles.startColor = HSVColor.HSVToRGB(colorValues.x, 1f, 1); ;
        particles.Play();

        color = HSVColor.HSVToRGB(colorValues.x, 0.6f, 1);
        foreach (Transform trans in rigid.transform)
        {
            particles = trans.GetComponent<ParticleSystem>();
            particles.Stop(); //do the prewarm with the right particle color, instead of the previous color
            particles.startColor = color;
            particles.Play();
        }
    }

	// Update is called once per frame
	void FixedUpdate () {
        rigid.angularVelocity = Vector3.ClampMagnitude(rigid.angularVelocity + (accelScalar * Random.insideUnitSphere), maxSpeed);
	}

    public override blockDataType getBlockType()
    {
        return blockDataType.BOULDERINTERIOR;
    }

    public override Color getColor()
    {
        return color;
    }

    public override bool getDataValue() //counted as an air(false) or dirt(true) block
    {
        return false; //turns into nothing if you leave
    }
}
