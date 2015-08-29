using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class EnergyTutorialObjective : ResettingObjective
{
    EnergyBarLabel label;
    Text homeBaseLabel;
    string previousHomeBaseText;

    int returnValue = 0;
	// Use this for initialization
	protected void Start () {
        EnergyMeter energyMeter = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<EnergyMeter>();
        if (energyMeter != null)
        {

            //it's only not null in the main scene
            returnValue = 1;

            Callback.FireForFixedUpdate(() => energyMeter.Add((-2f/3f) * energyMeter.StartDrainTime), this); //start at 1/3 energy //callback to ensure it fires after all other starts

            //label the energy bar
            GameObject labelPrefab = Resources.Load("EnergyBarLabel") as GameObject;
            Transform labelObject = Instantiate(labelPrefab).transform;
            
            Transform energyBar = GameObject.FindGameObjectWithTag(Tags.energyUI).transform;
            labelObject.SetParent(energyBar, Vector3.one);
            ((RectTransform)(labelObject)).anchoredPosition = Vector3.zero;
            label = labelObject.GetComponent<EnergyBarLabel>();
            EnergyView view = energyBar.GetComponent<EnergyView>();
            IEnumerator flash = energyBarFlash(view);
            view.setWarnRoutine(flash);
            StartCoroutine(flash);
        }
        else
        {
            //if it's null, we're still in the tutorial
            returnValue = 0;
        }

        Callback.FireForFixedUpdate(() => 
            {
                GameObject homeBase = WorldController.getBlock(0, 0);
                homeBase.transform.Find("UI").GetComponent<CanvasGroup>().alpha = 1;
                homeBaseLabel = homeBase.transform.Find("UI/Label").GetComponent<Text>();
                previousHomeBaseText = homeBaseLabel.text;
                homeBaseLabel.text = "Home Base";
                ((TriggerObservable)(homeBase.AddComponent<TriggerObservable>())).Instantiate(this);

            }, this); //add observable; callback so that the map initialization finishes first

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
        homeBaseLabel.text = previousHomeBaseText;
        completeObjective(); //they've moved to base
    }

    protected override void spawnNextObjectives()
    {
        GameObject.FindGameObjectWithTag(Tags.energyUI).GetComponent<EnergyView>().setWarnRoutine(null);
        label.destroy();
        GetComponentInParent<ObjectivesController>().AddObjective(ID: 7);
    }

    public override int getProgress()
    {
        return returnValue; //if 1, we're in the main scene; don't save. otherwise, we're in the tutorial; save so that we can still have the objective in the main scene
    }

    private IEnumerator energyBarFlash(EnergyView bar)
    {
        while(true)
            yield return StartCoroutine(bar.flashRoutine());
    }

    public override int getID(){return 6;}
	
}
