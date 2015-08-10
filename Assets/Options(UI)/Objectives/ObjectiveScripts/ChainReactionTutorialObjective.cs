using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class ChainReactionTutorialObjective : OreBlockTutorialObjectve
{
    static Vector2 blockPos = new Vector2(18, 0); //constant
    new protected static Vector2 respawnPoint = new Vector2(2, 0); //constant
    static Vector2[] blockOffsets =  //constant
    {
        new Vector2(0, 0),
        new Vector2(-2, 0),
        new Vector2(-2, 2),
        new Vector2(-2, -2),
        new Vector2(2, 0),
        new Vector2(2, 1),
        new Vector2(2, -1),
    };

    TutorialOreBlock[] spawnedBlocks;

    protected override void setTip()
    {
        GameObject.FindGameObjectWithTag(Tags.tutorial).GetComponent<TutorialTip>().SetTip("<color=yellow>Plan</color> your mining carefully to mine these blocks quickly, before they explode.");
    }

    protected override void SpawnObjects()
    {
        if (spawnedBlocks != null) //remove the previous set
        {
            for (int i = 0; i < spawnedBlocks.Length; i++)
                if (spawnedBlocks[i] != null)
                    spawnedBlocks[i].Despawn();

            for (int i = 0; i < blockOffsets.Length; i++)
            {
                Vector2 pos = blockPos + blockOffsets[i];
                WorldController.ModifyBlock(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), blockDataType.EMPTYBLOCK);
            }
        }
        //create the actual blocks
        GameObject tutorialOreBlockPrefab = Resources.Load("TutorialOreBlock(3D)") as GameObject;

        spawnedBlocks = new TutorialOreBlock[blockOffsets.Length];

        for (int i = 0; i < blockOffsets.Length; i++)
        {
            GameObject spawnedTutorialOreBlock = SimplePool.Spawn(tutorialOreBlockPrefab, blockPos + blockOffsets[i]);
            spawnedBlocks[i] = spawnedTutorialOreBlock.GetComponent<TutorialOreBlock>();
            spawnedBlocks[i].Instantiate(this);
        }
    }

    protected override void spawnNextObjectives()
    {
        GetComponentInParent<ObjectivesController>().AddObjective(ID: 5);
    }

    public override void OnNotifyExplosion(ExplosiveBlock block)
    {
        for (int i = 0; i < spawnedBlocks.Length; i++)
        {
            if (block.Equals(spawnedBlocks[i]))
            {
                spawnedBlocks[i] = null;
            }
        }
        base.OnNotifyExplosion(block);
    }

    public override void OnNotifyDig(Block block)
    {
        for (int i = 0; i < spawnedBlocks.Length; i++)
        {
            if (block.Equals(spawnedBlocks[i]))
            {
                spawnedBlocks[i] = null;
                checkObjectiveComplete();
            }
        }
    }

    private void checkObjectiveComplete()
    {
        for (int i = 0; i < spawnedBlocks.Length; i++)
            if (spawnedBlocks[i] != null)
                return;

        //else all are null; all have been mined
        Callback.FireForNextFrame(completeObjective, this);
    }

    protected override Vector2 getRespawnPoint()
    {
        return respawnPoint;
    }

    protected override string getText() { return "Mine all 9 blocks with no explosions"; }

    public override int getID() { return 4; }
}
