using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class MiningBlastScript : MonoBehaviour, ISpawnable {
    AudioSource source;
    DiggingListenerSystem listeners;
    ParticleSystem riser;
    ParticleSystem mainBlast;
    Transform rotate;

    public AudioClip riserClio;
    public AudioClip blastClip;
    private const float range = 1.25f;
    private const float rotationSpeed = 300f;
    private static LayerMask mask; //should be const, but again, compiler errors 
	// Use this for initialization
	void Awake () {
        source = GetComponent<AudioSource>();
        riser = transform.Find("riser Particle").GetComponent<ParticleSystem>();
        mainBlast = transform.Find("mainBlast Particle").GetComponent<ParticleSystem>();
        mask = LayerMask.GetMask(new string[] { Layers.blocks, Layers.transBlocks });
        rotate = transform.Find("mainBlast Particle/Rotating Particles");
	}

    void Start()
    {
        //gotta wait for the parent to be set
        listeners = GetComponentInParent<DiggingListenerSystem>();
    }

    public void Create()
    {
        StartCoroutine(MainBehaviour());
    }

    void Update()
    {
        rotate.Rotate(new Vector3(0, 0, rotationSpeed * Time.deltaTime));
    }

    IEnumerator MainBehaviour()
    {
        /*
        source.clip = riserClip;
         * source.Play();
         * */
        riser.Play();
        yield return new WaitForSeconds(0.5f);
        /*
        source.clip = mainBlast;
         * source.Play();
         * */
        riser.Stop();
        mainBlast.Play();
        ScreenShake.RandomShake(this, 0.1f, 0.3f);
        Collider2D[] hits = Physics2D.OverlapAreaAll((Vector2)(this.transform.position) + (range * Vector2.one), (Vector2)(this.transform.position) - (range * Vector2.one), mask);
        foreach (Collider2D hit in hits)
        {
            Block hitBlock = hit.GetComponent<Block>();
            if (hitBlock.isMinable())
            {
                listeners.DigNotify(hitBlock);
                hitBlock.Destroy();
            }
        }

        yield return new WaitForSeconds(5f); //wait for particle effects to die
        mainBlast.Stop();
        SimplePool.Despawn(this.gameObject);
    }
}
