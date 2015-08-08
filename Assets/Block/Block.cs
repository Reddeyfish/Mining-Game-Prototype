using UnityEngine;
using System.Collections;

public class Block : MonoBehaviour, ISpawnable, IObliterable {
	// Use this for initialization

    public virtual void Create()
    {

    }

    public virtual void StartDig()
    {
        //stuff to disable movement/physics so the block does not move while digging

        //as well as any other stuff to do when being dug (i.e. animations)
        SpringJoint2D spring = GetComponent<SpringJoint2D>();
        if (spring != null) spring.enabled = false;

        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        if (rigid != null) rigid.constraints = RigidbodyConstraints2D.FreezeAll; 
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

    public virtual void UpdateMap()
    {
        WorldController.UpdateBlock(Mathf.RoundToInt(transform.position.x), Mathf.RoundToInt(transform.position.y), blockDataType.EMPTYBLOCK);
    }

    public virtual bool isSolid() //counted as an air(false) or dirt(true) block
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
    public float digTime()
    {
        return baseDigTime();
    }
}
