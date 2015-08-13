using UnityEngine;
using System.Collections;

public class UpgradesTutorialObjective : ResettingObjective {

	// Use this for initialization

    protected virtual void setTip()
    {
        GameObject.FindGameObjectWithTag(Tags.tutorial).GetComponent<TutorialTip>().SetTip("At Home Base, you can also use any resources you've obtained to <color=yellow>buy items and upgrades</color> for your ship");
    }

    protected override void spawnNextObjectives()
    {
        GetComponentInParent<ObjectivesController>().AddObjective(ID: 8);
    }

    protected override string getText() { return "Buy an Upgrade"; }

    public override int getID() { return 7; }
	
	// Update is called once per frame
}
