using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]

public class GuffinTracker : MonoBehaviour, ISpawnable {

    Transform thisTransform;
    Transform targetGuffin;
    float maxRange;
    GuffinController controller;
    ParticleSystem pingVisuals;
    AudioSource pingSounds;

    const float speedMultiplier = 0.1f;
    const float captureRange = 2;

	// Use this for initialization
	void Awake () {
        pingVisuals = GetComponent<ParticleSystem>();
        pingSounds = GetComponent<AudioSource>();
        thisTransform = this.transform;
	}

    public void Create()
    {
        StartCoroutine(mainRoutine());
    }

    public void Initialize(Transform targetGuffin, float maxRange, GuffinController controller)
    {
        this.targetGuffin = targetGuffin;
        this.maxRange = maxRange;
        this.controller = controller;
    }

    void Complete(bool success)
    {
        SimplePool.Despawn(targetGuffin.gameObject);
        controller.NotifyComplete(success);
        SimplePool.Despawn(this.gameObject);
    }

    IEnumerator mainRoutine()
    {
        yield return new WaitForFixedUpdate();
        while (true)
        {
            pingVisuals.Play();
            pingSounds.Play();
            float distance = (targetGuffin.position - thisTransform.position).magnitude;
            if (distance > maxRange)
                Complete(success: false); //guffin lost
            yield return new WaitForSeconds(speedMultiplier * distance);
        }
    }

    void FixedUpdate()
    {
        if ((targetGuffin.position - thisTransform.position).magnitude < captureRange)
        {
            //capture the guffin

            //animation?

            Complete(success : true);
        }
    }
}
