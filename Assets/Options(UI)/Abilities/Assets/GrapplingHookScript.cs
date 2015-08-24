using UnityEngine;
using System.Collections;

//goes on the grappling hook object spawned by the ability

[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(Rigidbody2D))]
public class GrapplingHookScript : MonoBehaviour, ILaunchable {
    Rigidbody2D rigid;
    Transform player;
    Transform thisTransform;
    LineRenderer line;
    bool collided;

    static LayerMask mask; //constant
    
    public float speed = 15f;

    public GameObject hookCollisionEffectPrefab;
    public GameObject hookDestructionEffectPrefab;

    [Tooltip("Material for the grappling line")]
    public Material mat;

	// Use this for initialization
	void Awake () {
        rigid = GetComponent<Rigidbody2D>();
        line = GetComponent<LineRenderer>();
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        thisTransform = this.transform;
        mask = LayerMask.GetMask(new string[] { Layers.blocks, Layers.transBlocks });
	}
	
	// Update is called once per frame
	IEnumerator UpdateVisuals () {
        collided = false;
        while (!collided)
        {
            line.SetPosition(0, player.position);
            line.SetPosition(1, thisTransform.position);
            yield return new WaitForFixedUpdate();
        }

        //deactivate everything except audio
        rigid.velocity = Vector2.zero;
        line.enabled = false;
        Collider2D collider = GetComponent<Collider2D>();
        collider.enabled = false;
        MeshRenderer renderer = GetComponent<MeshRenderer>();
        renderer.enabled = false;

        AudioSource audio = GetComponent<AudioSource>();
        yield return new WaitForSeconds(audio.clip.length);

        //re-enable stuff so this object can be reused

        line.enabled = true;
        collider.enabled = true;
        renderer.enabled = true;

        SimplePool.Despawn(this.gameObject);
	}

    public void Instantiate(Vector3 target)
    {
        Debug.Log("Instantiated");
        Vector2 dir = (target - thisTransform.position).normalized;
        thisTransform.rotation = dir.ToRotation();
        rigid.velocity = speed * dir;

        StartCoroutine(UpdateVisuals());
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log("collided!");
        if (!other.transform.CompareTag(Tags.block)) return;
        Debug.Log("Grapple!");

        //turn the hit collider into a grappled block
        GrappledBehaviour grapple = other.gameObject.AddComponent<GrappledBehaviour>();
        
        //raycast to find the precise collision point; tell that to the grappled block
        grapple.Instantiate(Physics2D.Raycast(thisTransform.position, other.transform.position - thisTransform.position, 1, mask).point, mat, hookCollisionEffectPrefab, hookDestructionEffectPrefab);

        collided = true; //have our effects progress
    }

}
