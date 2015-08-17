using UnityEngine;
using System.Collections;

public class ModeSwitch : MonoBehaviour, IDisabledStart {
    Transform tab;
    InventoryUIController inventoryController;
    CheckSaveScript checkSave;
	// Use this for initialization
	public void StartDisabled () {
        tab = transform.Find("Tabs/Items Tab");
        inventoryController = transform.Find("InventoryOutline/InventoryView/Content").GetComponent<InventoryUIController>();
        checkSave = GetComponent<CheckSaveScript>();
	}

    public void setStoreMode(bool store)
    {
        tab.gameObject.SetActive(store);
        inventoryController.baseMode = store;
        checkSave.baseMode = store;
    }
}
