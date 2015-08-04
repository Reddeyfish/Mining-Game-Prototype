using UnityEngine;
using System.Collections;

public class DiggingTutorialObjective : ResettingObjective, IDigListener
{

    const int wallPos = -13;//the x-position of the wall
    DiggingListenerSystem listener;
	// Use this for initialization
    protected virtual void Awake()
    {
        //spawn the wall
        for(int i = -3; i <= 3; i++)
            WorldController.UpdateBlock(wallPos, i, blockDataType.MAPBLOCK);
        //tutorial tip
        GameObject.FindGameObjectWithTag(Tags.tutorial).GetComponent<TutorialTip>().SetTip("Use the <color=yellow>Space Bar</color> to activate your drill. Drill through this <color=cyan>wall</color>.");


        listener = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<DiggingListenerSystem>();
        listener.Subscribe(this);
	}
	
	// Update is called once per frame
    public void OnNotifyDig(Block block)
    {
        if (block.transform.position.x == wallPos) //then they've dug the correct block
        {
            Callback.FireForNextFrame(completeObjective, this); //delayed for a frame to stop the listener from freaking out about removing the listenee
        }

    }

    protected override void spawnNextObjectives()
    {
        GetComponentInParent<ObjectivesController>().AddObjective(ID : 3);
    }


    public void OnDestroy()
    {
        listener.UnSubscribe(this);
    }

    protected override string getText()
    {
        return "Dig through the wall on the right";
    }

    public override int getID() { return 2; }
}
