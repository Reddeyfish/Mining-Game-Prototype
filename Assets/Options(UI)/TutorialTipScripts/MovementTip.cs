using UnityEngine;
using System.Collections;

//place on Player

public class MovementTip : BaseDigListener {
    TutorialTip tips;
	// Use this for initialization
	protected override void Start () {
        base.Start();
        tips = GameObject.FindGameObjectWithTag(Tags.tutorial).GetComponent<TutorialTip>();
        tips.SetTip(TutorialTipType.MOVEMENT);
	}
    public override void OnNotify(Block block)
    {
        tips.EndTip(TutorialTipType.MOVEMENT);
        Destroy(this);
    }
}
