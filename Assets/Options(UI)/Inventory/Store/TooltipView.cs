using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TooltipView : MonoBehaviour, IDisabledAwake {
    Text title;
    Text description;
    // Use this for initialization
	public void Awaken () {
	    title = transform.Find("TooltipTitle").GetComponent<Text>();
        description = transform.Find("TooltipDescription").GetComponent<Text>();
	}

    public void Initialize(int ID) 
    {
        title.text = theUpgradeData.IDToUpgrade[ID].ComponentName;
        description.text = theUpgradeData.IDToUpgrade[ID].ComponentName;
    }

    public void setVisible(bool enabled)
    {
        gameObject.SetActive(enabled);
        Cursor.visible = !enabled;
    }

	
	// Update is called once per frame when the object is active
	void Update () {
        transform.position = Input.mousePosition;
	}
}
