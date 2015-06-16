using UnityEngine;
using System.Collections;

public class DestructibleSoundEffect : MonoBehaviour, ISpawnable {
    AudioSource source;
    public float basePitch;
    public float pitchVariance;

    void Awake()
    {
        source = GetComponent<AudioSource>();
    }
    public void Create()
    {
        source.pitch = basePitch + Random.value * pitchVariance;
        source.Play();
    }
    void FixedUpdate()
    {
        if (!source.isPlaying)
        {
            SimplePool.Despawn(this.gameObject);
        }
    }
}
