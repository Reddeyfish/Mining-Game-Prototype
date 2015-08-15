using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//maybe optimize with sorting and binary search?
//abilities needs it in sorted order anyways.

/*
var index = TimeList.BinarySearch(dateTimeOffset);
if (index < 0) index = ~index;
TimeList.Insert(index, dateTimeOffset);
 * */


public class ItemsView : MonoBehaviour, IDisabledAwake, IDisabledStart {
    List<Item> items;
    public List<Item> Items { get { return items; } }
    GameObject[,] cells;
    AudioSource source;
    BaseInventory inventory;
    StoreUIController store;
    List<IItemsListener> listeners;

    public void Subscribe(IItemsListener listener) { listeners.Add(listener); }
    public void UnSubscribe(IItemsListener listener) { listeners.Remove(listener); }

    private GameObject player;
    private Rect cellSize;
    private const int rows = 4;
    private const int cols = 4;
    public GameObject cell;
    public GameObject draggable;
    [Range(0, 0.5f)]
    public float tolerance = 0.25f; //zero tolerance == zero sense. Make sure it's greater than zero to enable snap.
    public int destroyRange = 3;
	// Use this for initialization
    public void Awaken()
    {
        source = GetComponent<AudioSource>();
        listeners = new List<IItemsListener>();
        items = new List<Item>();
    }

	public void StartDisabled () {

        
        inventory = transform.parent.parent.Find("InventoryOutline/InventoryView/Content").GetComponent<BaseInventory>();
        player = GameObject.FindGameObjectWithTag(Tags.player);
        store = transform.parent.parent.Find("Store/SlotsBackground/Content").GetComponent<StoreUIController>();
        //spawn cells
        cells = new GameObject[rows, cols];
        cellSize = ((RectTransform)(cell.transform)).rect;
        for(int x = 0; x < rows; x++)
            for (int y = 0; y < cols; y++)
            {
                GameObject cellTrans = SimplePool.Spawn(cell);
                cellTrans.transform.SetParent(this.transform, Vector3.one);
                cells[x, y] = cellTrans;
                cellTrans.transform.localPosition = new Vector2((x - ((float)rows - 1) / 2) * cellSize.width, ((((float)cols - 1) / 2) - y) * cellSize.height);
            }
        //now that the local positioning is done, switch back to global scaling
        cellSize.width *= transform.lossyScale.x;
        cellSize.height *= transform.lossyScale.y;

        //now load our data

        if (PlayerPrefs.HasKey(PlayerPrefKeys.items))
        {
            int[] data = PlayerPrefsX.GetIntArray(PlayerPrefKeys.items);
            Debug.Log("Item Data Found!");
            if (data.Length % 3 != 0)
                Debug.Log("Item Size Mismatch");
            for (int i = 0; i < data.Length / 3; i++)
            {
                Draggable drag = SimplePool.Spawn(draggable).GetComponent<Draggable>();
                drag.ID = data[3 * i];
                setCell(drag, data[3 * i + 1], data[3 * i + 2], false);
                drag.Instantiate(data[3 * i], this, data[3 * i + 1], data[3 * i + 2]);
                setCellRangeFill(data[3 * i + 1], data[3 * i + 2], drag.cellWidth, drag.cellHeight, open: false);
                ((RectTransform)drag.transform).anchoredPosition = Vector3.zero;
            }
        }
        items.Sort();
	}

    


    public void positionToCellIndex(Vector3 position, int cellWidth, int cellHeight, out int? xIndex, out int? yIndex)
    {
        Vector3 local = position - cells[0,0].transform.position;
        int x = Mathf.RoundToInt(local.x / cellSize.width);
        int y = Mathf.RoundToInt(-local.y / cellSize.height);
        if (inModRange(local.x, cellSize.width, tolerance) && inModRange(-local.y, cellSize.height, tolerance))
        {
            if (x >= 0 && y >= 0) //if we're in range of real cells
                if (x + cellWidth - 1 < cells.GetLength(0) && y + cellHeight - 1 < cells.GetLength(1)) //and our max corner is also in the range of real cells
                    if (isRangeEmpty(x, y, cellWidth, cellHeight))
                    {
                        xIndex = x;
                        yIndex = y;
                        return;
                    }
        }
        //else
        if ((x < -destroyRange || x >= rows + destroyRange) || (y < -destroyRange || y >= cols + destroyRange)) //if we're really far away from the cells
        {
            xIndex = -1;
            yIndex = -1;
            return;
        }
        //else
        xIndex = null; //else no match
        yIndex = null;
    }

    public GameObject positionToCell(Vector3 position, int cellWidth, int cellHeight)
    {
        int? x, y;
        positionToCellIndex(position, cellWidth, cellHeight, out x, out y);
        if (x != null && y != null && x >= 0 && y >= 0)
        {
            return cells[x.GetValueOrDefault(), y.GetValueOrDefault()];
        }
        else
            return null;
    }

    public bool isRangeEmpty(int basex, int basey, int cellWidth, int cellHeight)
    {
        for (int x = basex; x < basex + cellWidth; x++)
            for (int y = basey; y < basey + cellHeight; y++)
            {
                if (!cells[x, y].GetComponent<CellEntry>().Empty)
                {
                    return false; //return false if any are filled already
                }
            }
        return true;
    }

    public void setCellRangeFill(int basex, int basey, int cellWidth, int cellHeight, bool open)
    {
        for (int x = basex; x < basex + cellWidth; x++)
            for (int y = basey; y < basey + cellHeight; y++)
                cells[x, y].GetComponent<CellEntry>().Empty = open;
    }

    public void removeCell(Draggable drag, int x, int y, bool notARearrangement)
    {
        Item target = new Item(x, y, drag.ID);
        for (int i = 0; i < items.Count; i++)
            if (items[i].Equals(target))
            {
                Destroy(items[i].component);
                items.RemoveAt(i);
                target = null;
                break;
            }
        if (target != null)
        {
            Debug.Log("Error: We've lost a component!");
        }
        if (notARearrangement)
        {
            source.Play();
            inventory.RefundCosts(theUpgradeData.IDToUpgrade[drag.ID].costs);
            store.recheckCosts();
            //if we add another thing that needs to be notified, refactor this into a listener
        }
        items.Sort();
        NotifyAll(null);
    }

    public void setCell(Draggable drag, int x, int y, bool notARearrangement)
    {
        drag.transform.SetParent(cells[x, y].transform);
        cells[x, y].transform.SetAsLastSibling(); //have the draggable rendered over all other cells
        Item newItem = new Item(x, y, drag.ID);
        newItem.component = theUpgradeData.IDToUpgrade[drag.ID].AddComponentTo(player, drag.ID);
        items.Add(newItem);
        if (notARearrangement)
        {
            source.Play();
            inventory.PayCosts(theUpgradeData.IDToUpgrade[drag.ID].costs);
            //if we add another thing that needs to be notified, refactor this into a listener
            
        }
        items.Sort();
        NotifyAll(newItem);
    }

    private static bool inModRange(float value, float mod, float tolerance)
    {
        return mod - (value % mod) < (mod * tolerance) || (value % mod) < (mod * tolerance);
    }

    private void NotifyAll(Item message)
    {
        foreach (IItemsListener listener in listeners)
            listener.Notify(message);
    }

    void OnDisable()
    {
        //Save our data
        int[] data = new int[items.Count * 3];
        for (int i = 0; i < items.Count; i++)
        {
            data[3 * i] = items[i].ID;
            data[3 * i + 1] = items[i].x;
            data[3 * i + 2] = items[i].y;
        }
        bool success = PlayerPrefsX.SetIntArray(PlayerPrefKeys.items, data);
        if (success)
            Debug.Log("Items Save Complete!");
        else
            Debug.Log("Items Save Failed!");
    }

    public BaseInventory getInventory() { return inventory; }
}

public class Item : System.IEquatable<Item>, System.IComparable<Item>
{
    public int x;
    public int y;
    public int ID;
    public MonoBehaviour component;
    public Item(int x, int y, int ID)
    {
        this.x = x;
        this.y = y;
        this.ID = ID;
    }

    public bool Equals(Item obj)
    {
        // If parameter is null return false.
        if (obj == null)
        {
            return false;
        }

        // If parameter cannot be cast to Point return false.
        Item p = obj as Item;
        if ((System.Object)p == null)
        {
            return false;
        }

        // Return true if the fields match:
        return (x == p.x) && (y == p.y) && (ID == p.ID);
    }

    public int CompareTo(Item other)
    {
        //default is ascending, and we want them in order (left-to-right, top-to-bottom)
        if (this.x - other.x != 0) return this.x - other.x;
        return this.y - other.y;
    }
}

public interface IItemsListener
{
    //item is null for removal
    void Notify(Item item);
    void OnDestroy(); //remind them to unsubscribe
}