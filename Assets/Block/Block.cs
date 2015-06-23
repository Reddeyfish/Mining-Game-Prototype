using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour, ISpawnable, IObliterable {
	// Use this for initialization
    protected int hierarchy;

    public virtual void Create()
    {
        setHierarchy();
    }

    protected virtual void setHierarchy()
    {
        hierarchy = WorldController.PerlinHierarchy(WorldController.ColorSeedX, WorldController.ColorSeedY, (int)(transform.position.x), (int)(transform.position.y));
    }

    public virtual void Destroy()
    {
        Despawn();
    }

    public virtual void Despawn()
    {
        SimplePool.Despawn(this.gameObject);
        UpdateMap();
    }

    public virtual void Obliterate()
    {
        Despawn();
    }

    protected virtual void UpdateMap()
    {
        WorldController.UpdateBlock(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), blockDataType.EMPTYBLOCK);
    }

    public virtual bool getDataValue() //counted as an air(false) or dirt(true) block
    {
        return true;
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
    public virtual float baseDigTime() { return 0.8f; }
    public float digTime(int diggerHierarchy)
    {
        Debug.Log(hierarchy);
        if (diggerHierarchy > hierarchy)
            return baseDigTime();
        else
        {
            return baseDigTime() * Mathf.Pow(2, hierarchy + 1 - diggerHierarchy);
        }
    }
}
