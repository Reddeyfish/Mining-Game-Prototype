using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class UpgradesTutorialObjective : ResettingObjective, IItemsListener {

    ItemsView itemsView;
    Transform inventoryLabel;
    Transform grabber;
    Transform dragNDropTip;
    Button inventoryTab;
    Button itemsTab;
    Button close;
    UnityEngine.Events.UnityAction enable;
    UnityEngine.Events.UnityAction disable;

    protected override void Awake()
    {
        base.Awake();
        itemsView = GameObject.FindGameObjectWithTag(Tags.canvas).transform.Find("InspectPanel/SlotsOutline/SlotsBackground").GetComponent<ItemsView>();
        itemsView.Subscribe(this);
        setTip();
        GameObject.FindGameObjectWithTag(Tags.canvas).transform.Find("InspectPanel/InventoryOutline/InventoryView/Content").GetComponent<BaseInventory>().Add(new Resource(resourceType.PURECOLOR, colorType.GREEN, 55));
        spawnObjects();
    }

	// Use this for initialization
    protected void spawnObjects()
    {
        Transform parent = GameObject.FindGameObjectWithTag(Tags.canvas).transform.Find("InspectPanel");

        GameObject inventoryLabelPrefab = Resources.Load("InventoryLabel") as GameObject;
        inventoryLabel = Instantiate(inventoryLabelPrefab).transform;
        inventoryLabel.SetParent(parent, Vector3.one);

        GameObject grabberPrefab = Resources.Load("Grabber") as GameObject;
        grabber = Instantiate(grabberPrefab).transform;
        grabber.SetParent(parent.Find("Tabs/Items Tab"), Vector3.one);

        GameObject dragDropPrefab = Resources.Load("DragNDropTip") as GameObject;
        dragNDropTip = Instantiate(dragDropPrefab).transform;
        dragNDropTip.SetParent(parent.Find("SlotsOutline"), Vector3.one);

        //link to buttons
        enable = () =>
        {
            inventoryLabel.gameObject.SetActive(true);
            grabber.gameObject.SetActive(true);
            dragNDropTip.gameObject.SetActive(false);
        };

        disable = () =>
        {
            inventoryLabel.gameObject.SetActive(false);
            grabber.gameObject.SetActive(false);
            dragNDropTip.gameObject.SetActive(true);
        };
        inventoryTab = parent.Find("Tabs/Inventory Tab").GetComponent<Button>();
        inventoryTab.onClick.AddListener(enable);
        itemsTab = parent.Find("Tabs/Items Tab").GetComponent<Button>();
        itemsTab.onClick.AddListener(disable);
        close = parent.Find("Close Button").GetComponent<Button>();
        close.onClick.AddListener(enable);
    }


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
        GetComponentInParent<ObjectivesController>().AddObjective(ID: 8);
    }

    protected override string getText() { return "Buy an Item"; }

    public override int getID() { return 7; }

    public void OnDestroy()
    {
        itemsView.UnSubscribe(this);

        inventoryTab.onClick.RemoveListener(enable);
        itemsTab.onClick.RemoveListener(disable);
        close.onClick.RemoveListener(enable);

        Destroy(dragNDropTip.gameObject);
        Destroy(grabber.gameObject);
        Destroy(inventoryLabel.gameObject);

    }
	
}
