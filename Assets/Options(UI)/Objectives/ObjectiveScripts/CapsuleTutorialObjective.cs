using UnityEngine;
using System.Collections;

public class CapsuleTutorialObjective : ResettingObjective
{

    static Vector2 capsulePos = new Vector2(32, 0); //constant

	// Use this for initialization
	protected void Start () {
        //tutorial tip
        GameObject.FindGameObjectWithTag(Tags.tutorial).GetComponent<TutorialTip>().SetTip("Good. Up ahead is a <color=yellow>capsule</color>. Interacting with it will return you to <color=cyan>Home Base</color>.");

        GameObject tutorialCapsulePrefab = Resources.Load("TutorialCapsule") as GameObject;

        GameObject tutorialCapsule = SimplePool.Spawn(tutorialCapsulePrefab, capsulePos);
        tutorialCapsule.GetComponent<TutorialCapsule>().Instantiate(this);


	}

    protected override void spawnNextObjectives()
    {
        GetComponentInParent<ObjectivesController>().AddObjective(ID : 6);
        GameObject.FindGameObjectWithTag(Tags.player).GetComponent<SaveObservable>().Save();
        Application.LoadLevel(Scenes.mainScene);
    }

    protected override string getText()
    {
        return "Interact with the capsule";
    }

    public override int getID() { return 5; }
}
