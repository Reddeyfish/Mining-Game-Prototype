using UnityEngine;
using System.Collections;

public class OreBlockTutorialObjectve : ResettingObjective, IExplosionListener
{
    const int blockPos = 1;
	// Use this for initialization
	protected override void Start () {
        base.Start();
        //tutorial tip

        GameObject.FindGameObjectWithTag(Tags.tutorial).GetComponent<TutorialTip>().SetTip("This is a block of <color=yellow>ore</color>. Blocks of ore will <color=yellow>trigger</color> when nearby blocks are mined. <color=cyan>Mine the Ore Block</color>.");

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
        Debug.Log("rest");
        //reset things since it exploded
    }

    protected override string getText() { return "Mine the Ore"; }

    public override int getID() { return 3; }
}
