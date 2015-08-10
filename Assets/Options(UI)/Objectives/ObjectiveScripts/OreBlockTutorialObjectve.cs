using UnityEngine;
using System.Collections;

public class OreBlockTutorialObjectve : ResettingObjective, IExplosionListener, IDigListener
{
    const int blockPos = 2;
    protected static Vector2 respawnPoint = new Vector2(-12, 0); //constant

    DiggingListenerSystem listener;

    protected IEnumerator reset;

	// Use this for initialization
	protected virtual void Awake () {
        //tutorial tip

        setTip();

        listener = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<DiggingListenerSystem>();
        listener.Subscribe(this);
	}

    protected virtual void setTip()
    {
        GameObject.FindGameObjectWithTag(Tags.tutorial).GetComponent<TutorialTip>().SetTip("This is a block of <color=yellow>ore</color>. Blocks of ore will <color=yellow>trigger</color> when nearby blocks are mined. <color=cyan>Mine the Ore Block</color>.");
    }

    protected override void Start()
    {
       base.Start();
       SpawnObjects();
    }

    protected virtual void SpawnObjects()
    {
        //create the actual block
        GameObject tutorialOreBlockPrefab = Resources.Load("TutorialOreBlock(3D)") as GameObject;
        GameObject spawnedTutorialOreBlock = SimplePool.Spawn(tutorialOreBlockPrefab, new Vector3(blockPos, 0, 0));
        spawnedTutorialOreBlock.GetComponent<TutorialOreBlock>().Instantiate(this);

        //surround it with normal blocks
        //left
        WorldController.ModifyBlock(blockPos - 2, -1, blockDataType.MAPBLOCK);
        WorldController.ModifyBlock(blockPos - 2, 0, blockDataType.MAPBLOCK);
        WorldController.ModifyBlock(blockPos - 2, 1, blockDataType.MAPBLOCK);

        //right
        WorldController.ModifyBlock(blockPos + 2, -1, blockDataType.MAPBLOCK);
        WorldController.ModifyBlock(blockPos + 2, 0, blockDataType.MAPBLOCK);
        WorldController.ModifyBlock(blockPos + 2, 1, blockDataType.MAPBLOCK);

        //top
        WorldController.ModifyBlock(blockPos - 1, 2, blockDataType.MAPBLOCK);
        WorldController.ModifyBlock(blockPos, 2, blockDataType.MAPBLOCK);
        WorldController.ModifyBlock(blockPos + 1, 2, blockDataType.MAPBLOCK);

        //bottom
        WorldController.ModifyBlock(blockPos - 1, -2, blockDataType.MAPBLOCK);
        WorldController.ModifyBlock(blockPos, -2, blockDataType.MAPBLOCK);
        WorldController.ModifyBlock(blockPos + 1, -2, blockDataType.MAPBLOCK);

        //and a wall in front of left to demonstrate ore blocks not triggering
        WorldController.ModifyBlock(blockPos - 2, -3, blockDataType.MAPBLOCK);
        WorldController.ModifyBlock(blockPos - 2, -2, blockDataType.MAPBLOCK);

        WorldController.ModifyBlock(blockPos - 3, -1, blockDataType.MAPBLOCK);
        WorldController.ModifyBlock(blockPos - 3, 0, blockDataType.MAPBLOCK);
        WorldController.ModifyBlock(blockPos - 3, 1, blockDataType.MAPBLOCK);

        WorldController.ModifyBlock(blockPos - 2, 2, blockDataType.MAPBLOCK);
        WorldController.ModifyBlock(blockPos - 2, 3, blockDataType.MAPBLOCK);
    }

    protected override void spawnNextObjectives()
    {
        GetComponentInParent<ObjectivesController>().AddObjective(ID: 4);
    }

    public virtual void OnNotifyExplosion(ExplosiveBlock block)
    {
        
        //reset things since it exploded
        if (reset == null) //else we're already resetting
        {
            Debug.Log("reset");
            reset = Reset();
            StartCoroutine(reset);
        }
    }

    public virtual void OnNotifyDig(Block block)
    {
        if (block.transform.position == new Vector3(blockPos, 0, 0))
            Callback.FireForNextFrame(completeObjective, this); //keeps it from screwing with the listener
    }

    protected IEnumerator Reset()
    {
        GameObject player = GameObject.FindGameObjectWithTag(Tags.player);
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        player.GetComponent<Inventory>().Wipe();
        yield return GameObject.FindGameObjectWithTag(Tags.screenFlash).GetComponent<ScreenFlash>().Fade(1.5f);

        player.GetComponent<DefaultDiggingScript>().interruptDigging(); //stop digging state

        player.transform.position = getRespawnPoint();
        Camera.main.transform.parent.parent.position = getRespawnPoint();
        WorldController.thi.RecreateCreatedBlocks();
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

        SpawnObjects();

        reset = null;
    }

    protected virtual Vector2 getRespawnPoint() //for inheritance
    {
        return respawnPoint;
    }

    public void OnDestroy()
    {
        listener.UnSubscribe(this);
    }

    protected override string getText() { return "Mine the Ore"; }

    public override int getID() { return 3; }
}
