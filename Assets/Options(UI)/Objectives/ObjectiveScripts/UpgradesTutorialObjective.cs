using UnityEngine;
using System.Collections;

public class UpgradesTutorialObjective : ResettingObjective, IItemsListener {

    ItemsView itemsView;

    void Awake()
    {
        itemsView = GameObject.FindGameObjectWithTag(Tags.canvas).transform.Find("InspectPanel/SlotsOutline/SlotsBackground").GetComponent<ItemsView>();
        itemsView.Subscribe(this);
        setTip();
        GameObject.FindGameObjectWithTag(Tags.canvas).transform.Find("InspectPanel/InventoryOutline/InventoryView/Content").GetComponent<BaseInventory>().Add(new Resource(resourceType.PURECOLOR, colorType.GREEN, 40));
    }

	// Use this for initialization

    protected virtual void setTip()
    {
        GameObject.FindGameObjectWithTag(Tags.tutorial).GetComponent<TutorialTip>().SetTip("At Home Base, you can also use any resources you've obtained to <color=yellow>buy items and upgrades</color> for your ship.");
    }

    public void Notify(Item item)
    {
        if (item != null) //null for moving or selling
            completeObjective();
    }

    protected override void spawnNextObjectives()
    {
        PlayerPrefsX.SetBool(PlayerPrefKeys.tutorialComplete, true);
        GameObject.FindGameObjectWithTag(Tags.tutorial).GetComponent<TutorialTip>().SetTimedTip("Congratulations! You have <color=yellow>completed</color> the <color=cyan>tutorial</color>!", 10);
        //GetComponentInParent<ObjectivesController>().AddObjective(ID: 8);
    }

    protected override string getText() { return "Buy an Upgrade"; }

    public override int getID() { return 7; }

    public void OnDestroy()
    {
        itemsView.UnSubscribe(this);
    }
	
	// Update is called once per frame
}
