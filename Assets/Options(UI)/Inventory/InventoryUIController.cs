﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InventoryUIController : MonoBehaviour {
    List<InventoryEntry> entries;
    FillDisplay fill;
    public GameObject InventoryEntryPrefab;

    void Awake()
    {
        entries = new List<InventoryEntry>();
        fill = transform.parent.parent.Find("FillLevel").GetComponent<FillDisplay>();
    }

	// Use this for initialization
    void OnEnable()
    {
        Inventory inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<Inventory>();
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

        fill.set(inventory.Fill, inventory.maxSize);
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
