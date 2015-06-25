using UnityEngine;
using System.Collections;

public class LaunchAbilityView : AbilityView {
    protected IEnumerator targetingRoutine;
    protected const float lerpTime = 3f;
    protected const float targetingSaturation = 0.5f; //how close the targeting pulse color gets to being white
    protected bool targeting = false;
	// Use this for initialization
	void Start () {
	
	}

    public void setIsTargeting(bool value)
    {
        targeting = value;
        if (value && targetingRoutine == null)
        {
            //start targeting
            targetingRoutine = TargetingRoutine();
            StartCoroutine(targetingRoutine);
        }
        //else we are already in the correct state and don't need to do anything
    }

    IEnumerator TargetingRoutine()
    {
        Color mainColor = arc.color; //store the original color
        float lerpValue = 0;
        while (targeting)
        {
            while (targeting && lerpValue < lerpTime / 2) //lerp to white
            {
                arc.color = Color.Lerp(mainColor, Color.white, lerpValue / lerpTime);
                yield return new WaitForFixedUpdate();
                lerpValue += Time.fixedDeltaTime;
            }
            lerpValue = lerpTime / 2;
            while (targeting && lerpValue > 0) //lerp back to being saturated
            {
                arc.color = Color.Lerp(mainColor, Color.white, lerpValue / lerpTime);
                yield return new WaitForFixedUpdate();
                lerpValue -= Time.fixedDeltaTime;
            }
            lerpValue = 0;
        }
        //targeting is now false, stop the coroutine and reset everything
        arc.color = mainColor;
        targetingRoutine = null;
    }
}
