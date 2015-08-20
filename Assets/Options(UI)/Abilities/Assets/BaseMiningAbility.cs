using UnityEngine;
using System.Collections;

//put on the prefab spawned by a spawning ability

public class BaseMiningAbility : MonoBehaviour, ISpawnable
{
    AudioSource source;
    DiggingListenerSystem listeners;
    ParticleSystem riser;
    ParticleSystem mainBlast;
    Animator animator;

    public AudioClip riserClip;
    public AudioClip blastClip;
    protected static LayerMask mask; //should be const, but again, compiler errors 
    public float riserTime = 0.5f;
    // Use this for initialization
    public virtual void Awake()
    {
        source = GetComponent<AudioSource>();
        riser = transform.Find("riser Particle").GetComponent<ParticleSystem>();
        mainBlast = transform.Find("mainBlast Particle").GetComponent<ParticleSystem>();
        mask = LayerMask.GetMask(new string[] { Layers.blocks, Layers.transBlocks });
        animator = GameObject.FindGameObjectWithTag(Tags.playerVisuals).GetComponent<Animator>();
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

    IEnumerator MainBehaviour()
    {
        source.clip = riserClip;
        source.Play();
        riser.Play();
        animator.SetTrigger(AnimatorParams.barrelRoll);

        yield return new WaitForSeconds(riserTime);

        source.clip = blastClip;
        source.Play();
        riser.Stop();
        mainBlast.Play();
        ScreenShake.RandomShake(this, 0.1f, 0.3f);

        Collider2D[] hits = getHits();
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

    protected virtual Collider2D[] getHits()
    {
        return new Collider2D[0];
    }
}

