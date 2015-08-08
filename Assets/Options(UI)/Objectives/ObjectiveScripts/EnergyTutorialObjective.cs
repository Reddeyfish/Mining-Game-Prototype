using UnityEngine;
using System.Collections;

public class EnergyTutorialObjective : Objective {

	// Use this for initialization
	protected override void Start () {
        base.Start();
        EnergyMeter energyMeter = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<EnergyMeter>();
        if(energyMeter != null)
            energyMeter.Add(-2 / 3 * energyMeter.StartDrainTime); //start at 1/3 energy //if it's null, we're still in the tutorial

        ((TriggerObservable)(WorldController. .AddComponent<TriggerObservable>())).Instantiate(this);

        GameObject.FindGameObjectWithTag(Tags.screenFlash).GetComponent<ScreenFlash>().Flash(3, 1); //fade from white, since we just changed scenes
	}

    protected override string getText()
    {
        return "Move to Home Base";
    }

    public void Notify(Collider2D other)
    {
        completeObjective(); //they've moved to base
    }

    public override void Initialize(int data)
    {

    }

    protected override void spawnNextObjectives()
    {
        //GetComponentInParent<ObjectivesController>().AddObjective(ID: 7);
    }

    public override int getID(){return 6;}
	
}
