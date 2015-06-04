using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour, ISpawnable {
	// Use this for initialization
    public virtual void Create()
    {

    }
    public virtual void Destroy()
    {
        SimplePool.Despawn(this.gameObject);
        UpdateMap();
    }
    protected virtual void UpdateMap()
    {
        WorldController.UpdateBlock(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), blockDataType.EMPTYBLOCK);
    }

    public virtual int getDataValue()
    {
        return 1;
    }

    public virtual blockDataType getBlockType()
    {
        return blockDataType.MAPBLOCK;
    }

    public virtual Color getColor()
    {
        return Color.clear;
    }

    //attributes
    public virtual bool isMinable() { return true; }
    public virtual float digTime() { return 0.8f; }
}
