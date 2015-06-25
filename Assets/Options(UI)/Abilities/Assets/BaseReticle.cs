using UnityEngine;
using System.Collections;

    //goes on the child of the ability UI that represents the reticle

public class BaseReticle : MonoBehaviour {
    Transform thisTransform;
	// Use this for initialization
	void Awake () {
        thisTransform = this.transform;
	}
	
	// Update is called once per frame
	void Update () {
        thisTransform.position = Input.mousePosition;
	}

    //for polymorphism. I doubt that there is going to be much shared between reticles to override update
    protected virtual void OnUpdate()
    {

    }
}
