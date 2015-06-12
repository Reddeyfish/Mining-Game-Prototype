using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class HomeBase : Block {
    CanvasGroup UI;
    GameObject player;
    GameObject inspectButton;
    UIKeyboardShortcut keyShortcut;
    ModeSwitch UIMode;
    public KeyCode key = KeyCode.Space;
	// Use this for initialization
	void Awake () {
        UI = transform.Find("UI").GetComponent<CanvasGroup>();
        player = GameObject.FindGameObjectWithTag(Tags.player);
        inspectButton = GameObject.FindGameObjectWithTag(Tags.canvas).transform.Find("InspectButton").gameObject;
        UIMode = GameObject.FindGameObjectWithTag(Tags.canvas).transform.Find("InspectPanel").GetComponent<ModeSwitch>();
	}
    /*
    public override void Create()
    {
        //shouldn't need anything, hopefully
    }
    */
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Tags.player)
        {
            UI.alpha = 1;
            EnergyMeter meter = player.GetComponent<EnergyMeter>();
            meter.Add(meter.StartDrainTime); //energy to max
            if (keyShortcut == null)
            {
                keyShortcut = inspectButton.AddComponent<UIKeyboardShortcut>();
                keyShortcut.key = key;
            }
            UIMode.setStoreMode(true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == Tags.player)
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

    public override bool getDataValue() //counted as an air(false) or dirt(true) block
    {
        return false;
    }
}
