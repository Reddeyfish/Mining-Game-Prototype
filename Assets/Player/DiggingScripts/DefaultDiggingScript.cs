using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(DiggingListenerSystem))]
public class DefaultDiggingScript : MonoBehaviour , IDigScript {
    public KeyCode digKey = KeyCode.Space;
    public float digPower = 100f;

    protected static LayerMask blocks;

    private Controls control;
    private Rigidbody2D rigid;
    private AudioSource drillSound; //is a looping source
    private ParticleSystem drillParticles;
    private DiggingListenerSystem listeners;
    private IEnumerator digroutine;

    float colliderRadius;

    private bool interrupt = false; //flag to interupt the digging coroutine

    private const float screenShakeIntensity = 0.03f;
    public void SetControlScript(Controls control)
    {
        this.control = control;
    }

    void Awake()
    {
        blocks = LayerMask.GetMask(new string[] {Layers.blocks, Layers.transBlocks});
        rigid = GetComponent<Rigidbody2D>();
        colliderRadius = GetComponent<CircleCollider2D>().radius;
    }

    void Start()
    {
        listeners = GetComponent<DiggingListenerSystem>();
        drillSound = transform.Find("DrillSounds").GetComponent<AudioSource>();
        drillParticles = transform.Find("DrillSounds").GetComponent<ParticleSystem>();
    }

    public bool DoDigging(Vector2 direction) //could refactor to return the coroutine
    {
        if (direction.sqrMagnitude != 0 && Input.GetKey(digKey))
        {
            RaycastHit2D hit = Physics2D.Raycast(this.transform.position, direction, 0.75f, blocks);
            if (hit)
            {
                Block hitBlock = hit.transform.gameObject.GetComponent<Block>();
                if (hitBlock.isMinable() && Physics2D.CircleCast(this.transform.position, colliderRadius, direction, 0.75f, blocks) == hit)
                {

                    //do digging
                    if (digroutine != null)
                    {
                        StopCoroutine(digroutine); //halts cleanup of the visual/audio effects
                    }
                    digroutine = Dig(hitBlock);
                    StartCoroutine(digroutine);
                    return true;
                }
                else
                {
                    listeners.UndiggableNotify(hitBlock);
                }
            }
        }
        return false;
    }

    protected virtual float getDigTimeMultiplier()
    {
        return (1 + this.transform.position.magnitude / digPower); //drill gets slower the farther away we are from the center
    }

    //returns true if digging was stopped; false if digging was already stopped
    public void interruptDigging()
    {
        interrupt = true;
    }

    IEnumerator Dig(Block target)
    {
        

        //sound start
        drillSound.Play();
        //particle effect positioning/directioning
        drillSound.transform.LookAt(2 * drillSound.transform.position - target.transform.position);
        drillSound.transform.localPosition = (target.transform.position - drillSound.transform.position)/2;
        //particle effect coloring
        drillParticles.startColor = target.GetComponent<Block>().getColor();
        drillParticles.Play();

        float digTime = target.digTime() * getDigTimeMultiplier(); 

        Collider2D otherCollider = target.transform.GetComponent<Collider2D>();
        otherCollider.enabled = false;
        target.StartDig();
        Vector3 targetPos = target.transform.position;

        //clear interrupt
        interrupt = false;

        while (!interrupt && (this.transform.position - targetPos).magnitude > 0.03f)
        {
            rigid.velocity = (targetPos - this.transform.position).normalized / digTime; //note : the direction isn't always constant
            Camera.main.transform.localPosition = Random.insideUnitCircle * screenShakeIntensity;
            yield return new WaitForFixedUpdate();
        }
        
        if(!interrupt) //if interrupted, digging didn't complete
            listeners.DigNotify(target);

        otherCollider.enabled = true; //reset it's collider so it can be reused

        if(!interrupt)
            target.Destroy(); //these three lines (DigNotify, collider enable, and destroy) have to be called in order.

        rigid.velocity = Vector3.zero; //stop moving
        Camera.main.transform.localPosition = Vector3.zero; //reset screen position to default (b/c of screen shake)
        drillSound.Pause(); //sound stop
        drillParticles.Stop(); //particles stop
        drillSound.transform.localPosition = Vector3.zero; //reset particle positioning
        control.StartIdle(); //return control to movement
        yield return new WaitForSeconds(1f);

        //call this when we've stopped digging for a while; the stop-routine will keep it from being called if we quickly dig another block
        drillSound.Stop();
        digroutine = null;
    }
}
