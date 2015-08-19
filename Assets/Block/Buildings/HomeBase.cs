using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class HomeBase : Block {
    AudioSource sound;
    CanvasGroup UI;
    GameObject player;
    GameObject inspectButton;
    UIKeyboardShortcut keyShortcut;
    ModeSwitch UIMode;
    BaseInventory baseInventory;
    public KeyCode key = KeyCode.Space;
    public GameObject energyRefilledPopup;
	// Use this for initialization
	void Awake () {
        sound = GetComponent<AudioSource>();
        UI = transform.Find("UI").GetComponent<CanvasGroup>();
        player = GameObject.FindGameObjectWithTag(Tags.player);
        inspectButton = GameObject.FindGameObjectWithTag(Tags.canvas).transform.Find("InspectButton").gameObject;
        UIMode = GameObject.FindGameObjectWithTag(Tags.canvas).transform.Find("InspectPanel").GetComponent<ModeSwitch>();
        baseInventory = GameObject.FindGameObjectWithTag(Tags.canvas).transform.Find("InspectPanel/InventoryOutline/InventoryView/Content").GetComponent<BaseInventory>();
	}
    /*
    public override void Create()
    {
        //shouldn't need anything, hopefully
    }
    */
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(Tags.player))
        {
            sound.Play();
            UI.alpha = 1;
            EnergyMeter meter = player.GetComponent<EnergyMeter>();
            meter.Add(meter.StartDrainTime); //energy to max
            SimplePool.Spawn(energyRefilledPopup);
            if (keyShortcut == null)
            {
                keyShortcut = inspectButton.AddComponent<UIKeyboardShortcut>();
                keyShortcut.key = key;
            }
            UIMode.setStoreMode(true);

            //transfer inventories
            other.GetComponent<Inventory>().Merge(baseInventory);

            other.GetComponent<SaveObservable>().Save();
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(Tags.player))
        {
            UI.alpha = 0;
            EnergyMeter meter = player.GetComponent<EnergyMeter>();
            meter.Add(meter.StartDrainTime); //energy to max
            Destroy(keyShortcut);
            keyShortcut = null;
            UIMode.setStoreMode(false);
        }
    }

    public override blockDataType getBlockType()
    {
        return blockDataType.BASE;
    }

    public override bool isMinable() { return false; }

    public override bool isSolid() //counted as an air(false) or dirt(true) block
    {
        return false;
    }
}
