using UnityEngine;
using System.Collections;

public class OreBlockTutorialObjectve : ResettingObjective, IExplosionListener, IDigListener
{
    const int blockPos = 1;
    static Vector2 respawnPoint = new Vector2(-12, 0); //constant

    DiggingListenerSystem listener;

	// Use this for initialization
	protected virtual void Awake () {
        //tutorial tip

        GameObject.FindGameObjectWithTag(Tags.tutorial).GetComponent<TutorialTip>().SetTip("This is a block of <color=yellow>ore</color>. Blocks of ore will <color=yellow>trigger</color> when nearby blocks are mined. <color=cyan>Mine the Ore Block</color>.");

        listener = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<DiggingListenerSystem>();
        listener.Subscribe(this);
	}

    protected override void Start()
    {
       base.Start();
       SpawnObjects();
    }

    private void SpawnObjects()
    {
        //create the actual block
        GameObject tutorialOreBlockPrefab = Resources.Load("TutorialOreBlock(3D)") as GameObject;
        GameObject spawnedTutorialOreBlock = SimplePool.Spawn(tutorialOreBlockPrefab, new Vector3(blockPos, 0, 0));
        spawnedTutorialOreBlock.GetComponent<TutorialOreBlock>().Instantiate(this);

        //surround it with normal blocks
        //left
        WorldController.UpdateBlock(blockPos - 2, -1, blockDataType.MAPBLOCK);
        WorldController.UpdateBlock(blockPos - 2, 0, blockDataType.MAPBLOCK);
        WorldController.UpdateBlock(blockPos - 2, 1, blockDataType.MAPBLOCK);

        //right
        WorldController.UpdateBlock(blockPos + 2, -1, blockDataType.MAPBLOCK);
        WorldController.UpdateBlock(blockPos + 2, 0, blockDataType.MAPBLOCK);
        WorldController.UpdateBlock(blockPos + 2, 1, blockDataType.MAPBLOCK);

        //top
        WorldController.UpdateBlock(blockPos - 1, 2, blockDataType.MAPBLOCK);
        WorldController.UpdateBlock(blockPos, 2, blockDataType.MAPBLOCK);
        WorldController.UpdateBlock(blockPos + 1, 2, blockDataType.MAPBLOCK);

        //bottom
        WorldController.UpdateBlock(blockPos - 1, -2, blockDataType.MAPBLOCK);
        WorldController.UpdateBlock(blockPos, -2, blockDataType.MAPBLOCK);
        WorldController.UpdateBlock(blockPos + 1, -2, blockDataType.MAPBLOCK);
    }

    protected override void spawnNextObjectives()
    {
        GetComponentInParent<ObjectivesController>().AddObjective(ID: 4);
    }

    public void OnNotifyExplosion(ExplosiveBlock block)
    {
        Debug.Log("reset");
        //reset things since it exploded
        StartCoroutine(Reset());
    }

    public void OnNotifyDig(Block block)
    {
        if (block.transform.position == new Vector3(blockPos, 0, 0))
            Callback.FireForNextFrame(completeObjective, this); //keeps it from screwing with the listener
    }

    private IEnumerator Reset()
    {
        GameObject player = GameObject.FindGameObjectWithTag(Tags.player);
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
        yield return GameObject.FindGameObjectWithTag(Tags.screenFlash).GetComponent<ScreenFlash>().Fade(1.5f);

        player.transform.position = respawnPoint;
        Camera.main.transform.parent.parent.position = respawnPoint;
        WorldController.thi.RecreateCreatedBlocks();
        player.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;

        SpawnObjects();
    }

    public void OnDestroy()
    {
        listener.UnSubscribe(this);
    }

    protected override string getText() { return "Mine the Ore"; }

    public override int getID() { return 3; }
}
