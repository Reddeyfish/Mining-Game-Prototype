using UnityEngine;
using System.Collections;

public class EnergyTutorialObjective : ResettingObjective
{

	// Use this for initialization
	protected override void Start () {
        base.Start();
        EnergyMeter energyMeter = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<EnergyMeter>();
        if(energyMeter != null)
            energyMeter.Add(-2 / 3 * energyMeter.StartDrainTime); //start at 1/3 energy //if it's null, we're still in the tutorial

        ((TriggerObservable)(WorldController.getBlock(0, 0).AddComponent<TriggerObservable>())).Instantiate(this); //add observable

        GameObject.FindGameObjectWithTag(Tags.screenFlash).GetComponent<ScreenFlash>().Flash(3, 1); //fade from white, since we just changed scenes

        GameObject.FindGameObjectWithTag(Tags.tutorial).GetComponent<TutorialTip>().SetTip("That digging used up most of your <color=yellow>energy</color>. Fortunately, Home Base has an infinite supply. Fly right next to <color=cyan>Home Base</color>, and your energy will be <color=cyan>automatically refilled</color>.");
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
        //GetComponentInParent<ObjectivesController>().AddObjective(ID: 7);
    }

    public override int getID(){return 6;}
	
}
