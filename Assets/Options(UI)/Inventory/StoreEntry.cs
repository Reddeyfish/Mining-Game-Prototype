using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class StoreEntry : MonoBehaviour {
    Text itemName;
    RectTransform draggableHolder;
    LayoutElement layout;
    ItemsView manager;
    private int _id;
    private const float wrapperHeight = 27;
    public GameObject draggablePrefab;
    public int ID { 
        get { return _id; }
        set
        {
            _id = value;
            itemName.text = theUpgradeData.IDToUpgrade[value].ComponentName;
            restock(value);
        }
    }
	// Use this for initialization
	void Awake () {
        itemName = transform.Find("Name").GetComponent<Text>();
        draggableHolder = (RectTransform)(transform.Find("DraggableHolder"));
        layout = GetComponent<LayoutElement>();
	    
	}

    public void Instantiate(int ID, ItemsView manager)
    {
        this.manager = manager;
        this.ID = ID;
    }

    public void restock(int ID)
    {
        RectTransform draggable = (RectTransform)SimplePool.Spawn(draggablePrefab).transform;
        draggable.SetParent(draggableHolder);
        draggable.GetComponent<Draggable>().Instantiate(ID, manager);

        layout.preferredHeight = draggable.rect.height + wrapperHeight;
    }
}
