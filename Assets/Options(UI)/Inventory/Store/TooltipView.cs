using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TooltipView : MonoBehaviour, IDisabledAwake {
    Text title;
    Text description;
    Rect currentSize =  new Rect();
    Transform thisTransform;
    // Use this for initialization
	public void Awaken () {
        thisTransform = this.transform;
        title = thisTransform.Find("TooltipTitle").GetComponent<Text>();
        description = thisTransform.Find("TooltipDescription").GetComponent<Text>();
	}

    public void Initialize(int ID) 
    {
        title.text = theUpgradeData.IDToUpgrade[ID].ComponentName;
        description.text = theUpgradeData.IDToUpgrade[ID].description;
    }

    public void setVisible(bool enabled)
    {
        gameObject.SetActive(enabled);
        Cursor.visible = !enabled;
        Callback.FireForNextFrame(UpdateCurrentSize, this); //we have to wait for the text to update itself after it's first frame being active; doing this immediately will yield an incorrect size
    }

    void UpdateCurrentSize()
    {
        currentSize = ((RectTransform)(thisTransform)).rect;
        currentSize.width *= thisTransform.lossyScale.x;
        currentSize.height *= thisTransform.lossyScale.y;
    }

	// Update is called once per frame when the object is active
	void Update () {
        Vector2 newPosition = Input.mousePosition;
        if (newPosition.x + currentSize.width > Screen.width)
            newPosition.x = Screen.width - currentSize.width;

        if (newPosition.y + currentSize.height > Screen.height)
            newPosition.y = Screen.height - currentSize.height;

        thisTransform.position = newPosition;
	}
}
