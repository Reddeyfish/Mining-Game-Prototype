using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class CameraLerp : MonoBehaviour {
    public float smoothTime = 0.5f;

    private Transform player;
    private Rigidbody rigid;
	// Use this for initialization
	void Start () {
        rigid = GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 velocity = Vector3.zero;
        transform.position = Vector3.SmoothDamp(transform.position, player.position, ref velocity, smoothTime);
        rigid.velocity = velocity;
	}
}
