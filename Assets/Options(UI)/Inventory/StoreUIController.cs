using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class StoreUIController : MonoBehaviour {
    List<StoreEntry> entries;
    ItemsView manager;
    public GameObject StoreEntryPrefab;
	// Use this for initialization

    void Awake()
    {
        entries = new List<StoreEntry>();
    }

	void Start () {
        manager = GameObject.FindGameObjectWithTag(Tags.itemSlots).GetComponent<ItemsView>();
        Callback.FireForNextFrame(Initialize, this);
	}

    void Initialize()
    {
        foreach (int i in theUpgradeData.IDToUpgrade.Keys)
        {
            GameObject entry = SimplePool.Spawn(StoreEntryPrefab);
            entry.transform.SetParent(this.transform);
            entry.transform.localScale = Vector3.one;
            StoreEntry storeEntry = entry.GetComponent<StoreEntry>();
            storeEntry.Instantiate(i, manager);
            entries.Add(storeEntry);
        }
    }

    public void recheckCosts()
    {
        foreach(StoreEntry entry in entries)
            entry.recheckCosts();
    }
}
