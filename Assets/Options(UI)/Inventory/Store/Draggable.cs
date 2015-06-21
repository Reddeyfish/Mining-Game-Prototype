using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler { //, ISpawnable {
    ItemsView manager;
    Image icon;
    Vector3 offset;
    Vector3 centerToTopLeftCell;
    Transform parent;
    GameObject outline;
    private int cellX = -1;
    private int cellY = -1;
    public int cellWidth = 1;
    public int cellHeight = 1;
    public int ID;
    
	// Use this for initialization
	void Awake () {
        icon = GetComponent<Image>();
        outline = transform.Find("DraggableOutline").gameObject;
	}
    /*
    public void Create()
    {
        cellX = -1;
        cellY = -1;
    }
    */
    public void Instantiate(int ID, ItemsView manager, int cellX = -1, int cellY = -1)
    {
        transform.localScale = Vector3.one;

        this.ID = ID;
        this.manager = manager;
        this.cellWidth = theUpgradeData.IDToUpgrade[ID].cellWidth;
        this.cellHeight = theUpgradeData.IDToUpgrade[ID].cellHeight;
        RectTransform trans = ((RectTransform)transform);
        trans.sizeDelta = new Vector2(cellWidth * theUpgradeData.cellPixelWidth, cellHeight * theUpgradeData.cellPixelHeight);
        icon.sprite = theUpgradeData.IDToSprite(ID);
        ((RectTransform)transform).anchoredPosition = Vector3.zero;
        offset = new Vector3(-((RectTransform)transform).rect.width * transform.lossyScale.x / 2, ((RectTransform)transform).rect.height * transform.lossyScale.y / 2, 0);
        centerToTopLeftCell = offset - new Vector3(-((RectTransform)transform).rect.width * transform.lossyScale.x / (2 * cellWidth), ((RectTransform)transform).rect.height * transform.lossyScale.y / (2 * cellHeight), 0);
        this.cellX = cellX;
        this.cellY = cellY;
    }

    public void OnMouseEnter()
    {
        outline.SetActive(true);
        TooltipView tooltip = GameObject.FindGameObjectWithTag(Tags.mouseParent).transform.Find("ItemTooltip").GetComponent<TooltipView>();
        tooltip.Initialize(ID);
        Debug.Log("tooltip!");
        tooltip.setVisible(true);
    }

    public void OnBeginDrag(UnityEngine.EventSystems.PointerEventData data)
    {
        OnMouseExit();
        //Cursor.visible = false;
        parent = transform.parent;
        transform.SetParent(GameObject.FindGameObjectWithTag(Tags.mouseParent).transform);
        //transform.parent.SetAsLastSibling(); //the dragged thing renders over all the other cells; no longer needed due to parenting to the canvas
        if (cellX != -1 && cellY != -1)
            manager.setCellRangeFill(cellX, cellY, cellWidth, cellHeight, open: true); //so we can be placed right where we started
    }

    public void OnDrag (PointerEventData eventData)
    {
        GameObject testPosition = manager.positionToCell(centerToTopLeftCell + Input.mousePosition, cellWidth, cellHeight);
        transform.position = testPosition != null ? (offset - centerToTopLeftCell) + testPosition.transform.position : offset + Input.mousePosition;
    }
 
 public void OnEndDrag (PointerEventData eventData)
 {
     int? x, y;
     manager.positionToCellIndex(centerToTopLeftCell + Input.mousePosition, cellWidth, cellHeight, out x, out y);
     if (x != null && y != null)
     {
         if (x == -1 && y == -1)
         {
             //remove ourselves
             SimplePool.Despawn(this.gameObject);
         }
         else
         {
             manager.setCell(this, x.GetValueOrDefault(), y.GetValueOrDefault(), notARearrangement : cellX == -1 && cellY == -1);
         }

         //switch parents!
         if (cellX != -1 && cellY != -1)
         {
             //remove ourself from the old parent cell
             manager.removeCell(this, cellX, cellY, notARearrangement: x == -1 && y == -1);
             manager.setCellRangeFill(cellX, cellY, cellWidth, cellHeight, open: true);
         }
         else
         {
             //if we don't have a cell parent, then we have a store parent
             parent.parent.GetComponent<StoreEntry>().restock(ID);
         }

         cellX = x.GetValueOrDefault();
         cellY = y.GetValueOrDefault();
     }
     else
     {
         //re-parent ourselves
         transform.SetParent(parent);
     }
     ((RectTransform)transform).anchoredPosition = Vector3.zero;
     if (cellX != -1 && cellY != -1)
         manager.setCellRangeFill(cellX, cellY, cellWidth, cellHeight, open: false);
     Cursor.visible = true;
 }

 public void OnMouseExit()
 {
     outline.SetActive(false);
     GameObject.FindGameObjectWithTag(Tags.mouseParent).transform.Find("ItemTooltip").GetComponent<TooltipView>().setVisible(false);
 }


}
