using UnityEngine;
using System.Collections;

public class EnergyTutorialObjective : ResettingObjective
{
    int returnValue = 1;
	// Use this for initialization
	protected override void Start () {
        base.Start();
        EnergyMeter energyMeter = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<EnergyMeter>();
        if (energyMeter != null)
        {
            //it's only not null in the main scene
            Callback.FireForFixedUpdate(() => energyMeter.Add((-2f/3f) * energyMeter.StartDrainTime), this); //start at 1/3 energy //callback to ensure it fires after all other starts
        }
        else
        {
            //if it's null, we're still in the tutorial
            returnValue = 0;
        }

        Callback.FireForFixedUpdate(() => ((TriggerObservable)(WorldController.getBlock(0, 0).AddComponent<TriggerObservable>())).Instantiate(this), this); //add observable; callback so that the map initialization finishes first

        GameObject.FindGameObjectWithTag(Tags.screenFlash).GetComponent<ScreenFlash>().Flash(3, 1); //fade from white, since we just changed scenes

        //should probably play the capsule sound

        GameObject.FindGameObjectWithTag(Tags.tutorial).GetComponent<TutorialTip>().SetTip("<size=11>That digging used most of your <color=yellow>energy</color>. Fortunately, Home Base has an infinite supply. Fly next to <color=cyan>Home Base</color> to refill <color=cyan>automatically</color>.</size>");
	}

    protected override string getText()
    {
        return "Move to Home Base";
    }

    public void Notify(Collider2D other)
    {
        completeObjective(); //they've moved to base
    }

    protected override void spawnNextObjectives()
    {
        GetComponentInParent<ObjectivesController>().AddObjective(ID: 7);
    }

    public override int getProgress()
    {
        return returnValue; //if 1, we're in the main scene; don't save. otherwise, we're in the tutorial; save so that we can still have the objective in the main scene
    }

    public override int getID(){return 6;}
	
}
