using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class MiningBlastScript : BaseMiningAbility {
    Transform rotate;
    private const float range = 1.25f;
    private const float rotationSpeed = 300f;
	// Use this for initialization
	public override void Awake () {
        base.Awake();
        rotate = transform.Find("mainBlast Particle/Rotating Particles");
	}

    void Update()
    {
        rotate.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
    }

    protected override Collider2D[] getHits()
    {
        return Physics2D.OverlapAreaAll((Vector2)(this.transform.position) + (range * Vector2.one), (Vector2)(this.transform.position) - (range * Vector2.one), mask);
    }
}
