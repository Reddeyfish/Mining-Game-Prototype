using UnityEngine;
using System.Collections;

public class ExplosiveBlockExplosion : MonoBehaviour, ISpawnable
{
    AudioSource source;
    ParticleSystem particles;
    public float basePitch;
    public float pitchVariance;
    private const float duration = 3;
    private const float range = 1.25f;

    private const float debrisSaturation = 0.8f;
    private const float debrisValue = 1f;

    private const float smokeSaturation = 1f;
    private const float smokeValue = 1f;
    void Awake()
    {
        source = GetComponent<AudioSource>();
        particles = GetComponent<ParticleSystem>();
    }


    public void Create()
    {
        //effects
        source.pitch = basePitch + Random.value * pitchVariance;
        source.Play();
        ScreenShake.RandomShake(this, 0.1f, 0.4f);
        Pause.Slow(this, 0.5f);
        //actual explosion
        Collider2D[] hits = Physics2D.OverlapAreaAll((Vector2)(this.transform.position) + (range * Vector2.one), (Vector2)(this.transform.position) - (range * Vector2.one));
        foreach (Collider2D hit in hits)
            foreach(IObliterable target in hit.GetComponents<IObliterable>())
                target.Obliterate();

        //cleanup
        Callback.FireAndForget(Remove, duration, this);
    }

    public void Instantiate(float hue)
    {
        GetComponent<ParticleSystem>().startColor = HSVColor.HSVToRGB(hue, debrisSaturation, debrisValue);
        transform.Find("ExplosionSmoke").GetComponent<ParticleSystem>().startColor = HSVColor.HSVToRGB(hue, smokeSaturation, smokeValue);
        particles.Play();
    }

    void Remove()
    {
        SimplePool.Despawn(this.gameObject);
    }
}

public interface IObliterable
{
    void Obliterate();
}