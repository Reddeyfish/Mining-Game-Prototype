using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class GrapplingHookScript : MonoBehaviour, ILaunchable {
    Rigidbody2D rigid;
    Transform player;
    Transform thisTransform;
    LineRenderer line;

    
    public float speed = 15f;

	// Use this for initialization
	void Awake () {
        rigid = GetComponent<Rigidbody2D>();
        line = GetComponent<LineRenderer>();
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        thisTransform = this.transform;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        line.SetPosition(0, player.position);
        line.SetPosition(1, thisTransform.position);
	}

    public void Instantiate(Vector3 target)
    {
        Debug.Log("Instantiated");
        Vector2 dir = (target - thisTransform.position).normalized;
        thisTransform.rotation = dir.ToRotation();
        rigid.velocity = speed * dir;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("collided!");
        if (!other.transform.CompareTag(Tags.block)) return;
        Debug.Log("Grapple!");
        //turn the hit collider into a grappled block
        SimplePool.Despawn(this.gameObject);
    }
}
