using UnityEngine;
using System.Collections;

public class GuffinBehaviour : MonoBehaviour, ISpawnable {
    Transform thisTransform;

    float seedX;
    float seedY;

    const float forwardSpeed = 0.25f;
    const float rotationalSpeed = 20f;
    const float perlinSpeedMultiplier = 0.25f;

	// Use this for initialization
	void Awake () {
        thisTransform = this.transform;
	}

    public void Create()
    {
        thisTransform.localRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
        seedX = Random.Range(-9999, 9999);
        seedY = Random.Range(-9999, 9999);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        thisTransform.Translate(forwardSpeed * Time.fixedDeltaTime, 0, 0);
        thisTransform.Rotate(Vector3.forward * rotationalSpeed * Time.fixedDeltaTime * Mathf.PerlinNoise(seedX, seedY));
        seedX += Time.fixedDeltaTime * perlinSpeedMultiplier;
	}
}
