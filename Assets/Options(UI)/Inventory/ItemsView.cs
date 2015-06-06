using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemsView : MonoBehaviour, IDisabledAwake {
    List<Item> items;
    GameObject[,] cells;
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
	public void Awaken () {
        player = GameObject.FindGameObjectWithTag(Tags.player);

        //spawn cells
        cells = new GameObject[rows, cols];
        cellSize = ((RectTransform)(cell.transform)).rect;
        for(int x = 0; x < rows; x++)
            for (int y = 0; y < cols; y++)
            {
                GameObject cellTrans = SimplePool.Spawn(cell);
                cellTrans.transform.SetParent(this.transform);
                cellTrans.transform.localScale = Vector3.one;
                cells[x, y] = cellTrans;
                cellTrans.transform.localPosition = new Vector2((x - ((float)rows - 1) / 2) * cellSize.width, ((((float)cols - 1) / 2) - y) * cellSize.height);
            }
        //now that the local positioning is done, switch back to global scaling
        cellSize.width *= transform.lossyScale.x;
        cellSize.height *= transform.lossyScale.y;

        //now load our data
        items = new List<Item>();

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
                setCell(drag, data[3 * i + 1], data[3 * i + 2]);
                drag.Instantiate(data[3 * i], this, data[3 * i + 1], data[3 * i + 2]);
                setCellRangeFill(data[3 * i + 1], data[3 * i + 2], drag.cellWidth, drag.cellHeight, open: false);
                ((RectTransform)drag.transform).anchoredPosition = Vector3.zero;
            }
        }
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

    public void removeCell(Draggable drag, int x, int y)
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
        Debug.Log(items.Count);
    }

    public void setCell(Draggable drag, int x, int y)
    {
        drag.transform.SetParent(cells[x, y].transform);
        Debug.Log("Set!");
        cells[x, y].transform.SetAsLastSibling(); //have the draggable rendered over all other cells
        Item newItem = new Item(x, y, drag.ID);
        newItem.component = theUpgradeData.IDToUpgrade[drag.ID].AddComponentTo(player);
        items.Add(newItem);
        Debug.Log(items.Count);
    }

    private static bool inModRange(float value, float mod, float tolerance)
    {
        return mod - (value % mod) < (mod * tolerance) || (value % mod) < (mod * tolerance);
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
}

public class Item : System.IEquatable<Item>
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
}