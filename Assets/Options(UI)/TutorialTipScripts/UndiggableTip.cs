using UnityEngine;
using System.Collections;

public class UndiggableTip : BaseUndiggableListener{
    //place on player
    TutorialTip tips;
    // Use this for initialization
    private const float tipDuration = 5f;
    protected override void Start()
    {
        base.Start();
        tips = GameObject.FindGameObjectWithTag(Tags.tutorial).GetComponent<TutorialTip>();
    }
    public override void OnNotifyUndiggable(Block block)
    {
        if (block.getBlockType() == blockDataType.BOULDER)
        {
            tips.SetTip(TutorialTipType.BOULDER);
            StopAllCoroutines(); //refresh the countdown
            Callback.FireAndForget(EndTip, tipDuration, this); 
        }
    }

    void EndTip()
    {
        tips.EndTip(TutorialTipType.BOULDER);
    }
}
