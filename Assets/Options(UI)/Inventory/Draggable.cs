using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class Draggable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler {
    ItemsView manager;
    Vector3 offset;
    Vector3 centerToTopLeftCell;
    private int cellX = -1;
    private int cellY = -1;
    public int cellWidth = 1;
    public int cellHeight = 1;
    public int ID;
	// Use this for initialization
	void Start () {
        manager = GetComponentInParent<ItemsView>();
        offset = new Vector3(-((RectTransform)transform).rect.width * transform.lossyScale.x/2, ((RectTransform)transform).rect.height * transform.lossyScale.y/2, 0);
        centerToTopLeftCell = offset - new Vector3(-((RectTransform)transform).rect.width * transform.lossyScale.x / (2 * cellWidth), ((RectTransform)transform).rect.height * transform.lossyScale.y / (2 * cellHeight), 0);
	}

    public void OnBeginDrag(UnityEngine.EventSystems.PointerEventData data)
    {
        //Cursor.visible = false;
        transform.parent.SetAsLastSibling(); //the dragged thing renders over all the other cells
        if (cellX != -1 && cellY != -1)
            manager.setCellRangeFill(cellX, cellY, cellHeight, cellWidth, open : true); //so we can be placed right where we started
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
         //switch parents!
         if (cellX != -1 && cellY != -1)
         {
             manager.removeCell(this, cellX, cellY);
             manager.setCellRangeFill(cellX, cellY, cellHeight, cellWidth, open: true);
         }

         manager.setCell(this, x.GetValueOrDefault(), y.GetValueOrDefault());
         
         cellX = x.GetValueOrDefault();
         cellY = y.GetValueOrDefault();
     }
     ((RectTransform)transform).anchoredPosition = Vector3.zero;
     if (cellX != -1 && cellY != -1)
        manager.setCellRangeFill(cellX, cellY, cellHeight, cellWidth, open : false);
     Cursor.visible = true;
 }


}
