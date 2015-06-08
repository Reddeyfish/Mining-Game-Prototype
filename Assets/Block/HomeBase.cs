using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class HomeBase : Block {
    CanvasGroup UI;
    public KeyCode key = KeyCode.Space;
	// Use this for initialization
	void Awake () {
        UI = transform.Find("UI").GetComponent<CanvasGroup>();
	}

    public void Create()
    {
        UI.alpha = 0;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == Tags.player)
        {
            UI.alpha = 1;
            StartCoroutine(InputRoutine());
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == Tags.player)
        {
            UI.alpha = 0;
            StopAllCoroutines();
        }
    }

    IEnumerator InputRoutine()
    {
        while (!Input.GetKeyDown(key))
        {
            yield return 0;
        }

        //they pressed space!
        GameObject player = GameObject.FindGameObjectWithTag(Tags.player);
        EnergyMeter meter = player.GetComponent<EnergyMeter>();
        meter.Add(meter.StartDrainTime); //energy to max
    }

    public override blockDataType getBlockType()
    {
        return blockDataType.BASE;
    }

    public override bool isMinable() { return false; }

    public virtual bool getDataValue() //counted as an air(false) or dirt(true) block
    {
        return false;
    }
}
