using UnityEngine;
using System.Collections;

public class UndiggableTip : BaseUndiggableListener{
    //place on player
    TutorialTip tips;
    // Use this for initialization
    public float tipDuration = 5f;
    public string undiggableTip = "This block cannot be mined.";
    protected override void Start()
    {
        base.Start();
        tips = GameObject.FindGameObjectWithTag(Tags.tutorial).GetComponent<TutorialTip>();
    }
    public override void OnNotifyUndiggable(Block block)
    {
        if (block.getBlockType() == blockDataType.BOULDER)
        {
            tips.SetTip(undiggableTip);
            StopAllCoroutines(); //refresh the countdown
            Callback.FireAndForget((() => tips.EndTip(undiggableTip)), tipDuration, this); 
        }
    }
}
