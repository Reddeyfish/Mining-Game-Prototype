using UnityEngine;
using System.Collections;

public class ItemsView : MonoBehaviour {
    GameObject[,] cells;
    private Rect cellSize;
    private const int rows = 3;
    private const int cols = 3;
    public GameObject cell;
    [Range(0, 0.5f)]
    public float tolerance = 0.25f; //zero tolerance == zero sense. Make sure it's greater than zero to enable snap.

	// Use this for initialization
	void Start () {
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
                cellTrans.transform.localPosition = new Vector2((x - (float)rows / 2) * cellSize.width, (((float)cols / 2) - y) * cellSize.height);
            }
        //now that the local positioning is done, switch back to global scaling
        cellSize.width *= transform.lossyScale.x;
        cellSize.height *= transform.lossyScale.y;
	}

    public void positionToCellIndex(Vector3 position, int cellWidth, int cellHeight, out int? xIndex, out int? yIndex)
    {
        Vector3 local = position - cells[0,0].transform.position;
        if (inModRange(local.x, cellSize.width, tolerance) && inModRange(-local.y, cellSize.height, tolerance))
        {
            int x = Mathf.RoundToInt(local.x / cellSize.width);
            int y = Mathf.RoundToInt(-local.y / cellSize.height);
            if(x >= 0 && y >= 0) //if we're in range of real cells
                if (x + cellWidth - 1 < cells.GetLength(0) && y + cellHeight - 1 < cells.GetLength(1)) //and our max corner is also in the range of real cells
                    if(isRangeEmpty(x, y, cellWidth, cellHeight))
                    {
                        xIndex = x;
                        yIndex = y;
                        return;
                    }
        }
        xIndex = null; //else no match
        yIndex = null;
    }

    public GameObject positionToCell(Vector3 position, int cellWidth, int cellHeight)
    {
        int? x, y;
        positionToCellIndex(position, cellWidth, cellHeight, out x, out y);
        if (x != null && y != null)
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

    public void setCellRangeFill(int basex, int basey, int cellHeight, int cellWidth, bool open)
    {
        for (int x = basex; x < basex + cellWidth; x++)
            for (int y = basey; y < basey + cellHeight; y++)
                cells[x, y].GetComponent<CellEntry>().Empty = open;
    }

    public void setCell(Draggable drag, int x, int y)
    {
        drag.transform.SetParent(cells[x, y].transform);
        cells[x, y].transform.SetAsLastSibling();
        Debug.Log("Set!");
        //add to some sort of list?
    }

    private static bool inModRange(float value, float mod, float tolerance)
    {
        return mod - (value % mod) < (mod * tolerance) || (value % mod) < (mod * tolerance);
    }
}
