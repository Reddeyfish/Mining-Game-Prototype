using UnityEngine;
using System.Collections;

public class DefaultComboRewardL0 : BaseComboListener
{
    public GameObject popup;
	// Use this for initialization
	
	// Update is called once per frame
	public override void OnNotify(int comboLevel)
    {
        Debug.Log(comboLevel);
        SimplePool.Spawn(popup, this.transform.position);
	}
}
