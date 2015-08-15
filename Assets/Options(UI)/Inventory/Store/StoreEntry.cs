using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class StoreEntry : MonoBehaviour {
    CostEntry[] costEntries;
    Text itemName;
    RectTransform draggableHolder;
    LayoutElement layout;
    ItemsView manager;
    GameObject blocker;
    private int _id;
    private const float wrapperHeight = 27;
    public GameObject draggablePrefab;
    public GameObject costEntryPrefab;
    public int ID { 
        get { return _id; }
        set
        {
            _id = value;
            itemName.text = theUpgradeData.IDToUpgrade[value].ComponentName;
            setCosts(theUpgradeData.IDToUpgrade[value].costs);
            restock(value);
        }
    }
	// Use this for initialization
	void Awake () {
        itemName = transform.Find("Name").GetComponent<Text>();
        draggableHolder = (RectTransform)(transform.Find("DraggableHolder"));
        layout = GetComponent<LayoutElement>();
        blocker = transform.Find("Blocker").gameObject;
	    
	}

    public void Instantiate(int ID, ItemsView manager)
    {
        this.manager = manager;
        this.ID = ID;
    }

    public void restock(int ID)
    {
        RectTransform draggable = (RectTransform)SimplePool.Spawn(draggablePrefab).transform;
        draggable.SetParent(draggableHolder);
        draggable.GetComponent<Draggable>().Instantiate(ID, manager);
        draggableHolder.sizeDelta = draggable.sizeDelta;
        layout.preferredHeight = draggable.rect.height + wrapperHeight;
        recheckCosts();
    }

    void setCosts(Cost[] costs)
    {
        if (costEntries != null)
            foreach (CostEntry costEntry in costEntries)
                SimplePool.Despawn(costEntry.gameObject);
        costEntries = new CostEntry[costs.Length];
        Transform costParent = transform.Find("Costs");
        blocker.SetActive(false);
        for (int i = 0; i < costs.Length; i++ )//(Cost cost in costs)
        {
            Transform newEntry = SimplePool.Spawn(costEntryPrefab).transform;
            newEntry.SetParent(costParent, false);
            CostEntry newCostEntry = newEntry.GetComponent<CostEntry>();
            bool afforded = newCostEntry.Initialize(costs[i], manager);
            if (!afforded)
                blocker.SetActive(true);
            costEntries[i] = newCostEntry;
        }
    }

    public void recheckCosts()
    {
        blocker.SetActive(false);
        foreach (CostEntry cost in costEntries)
        {
            if (!cost.affordable(manager))
            {
                blocker.SetActive(true);
                return;
            }      
        }
    }
}

public class Cost
{
    public resourceType type;
    public int count;
    public costType cost;
    public Cost(resourceType type, int count, costType cost)
    {
        this.type = type;
        this.count = count;
        this.cost = cost;
    }
}