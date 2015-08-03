using UnityEngine;
using System.Collections;

public class DiggingTutorialObjective : ResettingObjective, IDigListener
{

    Vector2 blockPos; //position of the ore block in the tutorial level
    DiggingListenerSystem listener;
	// Use this for initialization
	protected override void Start () {
        base.Start();
        listener = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<DiggingListenerSystem>();
        listener.Subscribe(this);
	}
	
	// Update is called once per frame
    public void OnNotify(Block block)
    {
        if ((Vector2)(block.transform.position) == blockPos) //then they've dug the correct block
        {
            completeObjective();
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
        throw new System.NotImplementedException();
    }

    public override int getID() { return 2; }
}
