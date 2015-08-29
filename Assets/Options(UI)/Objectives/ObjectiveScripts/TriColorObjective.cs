using UnityEngine;
using System.Collections;

public class TriColorObjective : Objective, IDigListener
{
    colorType progress = colorType.NONE; //bitmask
    const int complete = 7; //1 + 2 + 4, or 2^3 - 1

    DiggingListenerSystem listener;

    public override void Initialize(int progress)
    {
        if(progress >= 0 && progress < complete) //error checking
        {
            this.progress = (colorType)progress;
        }
        //otherwise, keeps default value of 0
    }

    public override int getProgress()
    {
        return (int)progress;
    }

    // Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        listener = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<DiggingListenerSystem>();
        listener.Subscribe(this);
    }

    protected override void spawnNextObjectives()
    {
        //GetComponentInParent<ObjectivesController>().AddObjective(ID: 9);
    }

    public virtual void OnNotifyDig(Block block)
    {
        colorType color = block.getColorType();
        if ((block.getBlockType() == blockDataType.OREBLOCK || 
            block.getBlockType() == blockDataType.EXPLOSIVE || 
            block.getBlockType() == blockDataType.BOULDERINTERIOR || 
            block.getBlockType() == blockDataType.PLUSEXPLOSIVE) 
            && (progress & color) == colorType.NONE) //if a resource block not already in the bitmask
        {
            progress |= color;

            if (progress == (colorType)complete)
                completeObjective();
            else
            {
               //update text
                screenText = getText();
            }
        }
    }

    public void OnDestroy()
    {
        listener.UnSubscribe(this);
    }

    protected override string getText() 
    {
        int count = 0;
        for (int i = 0; i < 3; i++)
        {
            if ((progress & (colorType)Mathf.Pow(2,i)) != colorType.NONE)//bitmask checking
                count++;
        }
            return "Gather resources of each of the three colors.\n<color=yellow>(" + count +"/3)</color>"; 
    }

    public override int getID() { return 8; }
}
