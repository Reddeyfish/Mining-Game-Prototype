using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryUIController : MonoBehaviour {
    List<InventoryEntry> entries;
    FillDisplay fill;
    public bool baseMode { get; set; }
    public GameObject InventoryEntryPrefab;

    void Awake()
    {
        entries = new List<InventoryEntry>();
        fill = transform.parent.parent.Find("FillLevel").GetComponent<FillDisplay>();
    }

	// Use this for initialization
    void OnEnable()
    {
        BaseInventory inventory;
        if(baseMode)
        {
            Debug.Log("base");
            inventory = GetComponent<BaseInventory>();
            GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>().Merge(inventory);
            fill.set(inventory.Fill);
        }
        else
        {
            Debug.Log("notbase");
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
            fill.set(inventory.Fill, ((Inventory)inventory).maxSize);
        }
        List<Resource> resources = inventory.Resources;
        if (resources.Count == 0)
        {
            foreach (InventoryEntry entry in entries)
            {
                SimplePool.Despawn(entry.gameObject);
            }
            entries = new List<InventoryEntry>();
        }
        else
        {
            resources.Sort();

            InventoryEntry entry = entries.Count > 0 ? entry = entries[0] : SpawnNewEntry();

            entry.Type = resources[0].type;
            int entryIndex = 1; //for going through entries

            foreach (Resource resource in resources)
            {
                if (resource.type != entry.Type)
                {
                    //get new/next entry
                    entry = entryIndex < entries.Count ? entry = entries[entryIndex] : SpawnNewEntry();
                    entry.Type = resource.type;
                    entryIndex++;
                }
                entry.set(resource);
            }

            //remove extra entries
            for (int i = entryIndex; i < entries.Count; i++)
            {
                SimplePool.Despawn(entries[i].gameObject);
            }
            entries.RemoveRange(entryIndex, entries.Count - entryIndex);
        }

        
    }

    private InventoryEntry SpawnNewEntry()
    {
        Transform newEntry = SimplePool.Spawn(InventoryEntryPrefab).transform;
        newEntry.SetParent(this.transform); //layout manager will take care of position
        newEntry.localScale = Vector3.one;
        InventoryEntry result = newEntry.GetComponent<InventoryEntry>();
        entries.Add(result);
        return result;
    }
}
