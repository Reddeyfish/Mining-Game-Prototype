using UnityEngine;
using System.Collections;
using System.IO;

    //unlike the world controller, loads a pre-set map. This particular scripts will load the tutorial level, but this script can be extended to load other levels (just ovrride the InitializeMat() method)

public class TutorialController : WorldController {

    protected override void Start()
    {
        base.Start();
        GameObject.FindGameObjectWithTag(Tags.objective).GetComponent<ObjectivesController>().AddObjective(ID: 1); //start the tutorial objectives
    }

    protected override void LoadData()
    {
        //the map is fixed, so there isn't any data to load.
        InitializeMap();
    }

    protected override void InitializeMap()
    {
        mapSize = 50;
        theMap = new Map(mapSize);

        Debug.Log(mapSize);

        for (int x = -mapSize + 1; x < mapSize; x++)
        {
            for (int y = -mapSize + 1; y < mapSize; y++)
            {
                theMap[x][y] = enumToBlock(blockDataType.MAPBLOCK);
            }
        }

        for (int x = -mapSize + 15; x <= mapSize - 15; x++)
            for (int y = -3; y <= 3; y++)
                theMap[x][y] = enumToBlock(blockDataType.EMPTYBLOCK);

        for (int x = 0; x <= mapSize - 15; x += 5)
        {
            theMap[x][4] = enumToBlock(blockDataType.LIGHTBLOCK);
            theMap[x][-4] = enumToBlock(blockDataType.LIGHTBLOCK);
            theMap[-x][4] = enumToBlock(blockDataType.LIGHTBLOCK);
            theMap[-x][-4] = enumToBlock(blockDataType.LIGHTBLOCK);
            theMap[x-1][4] = enumToBlock(blockDataType.LIGHTBLOCK);
            theMap[x-1][-4] = enumToBlock(blockDataType.LIGHTBLOCK);
            theMap[1-x][4] = enumToBlock(blockDataType.LIGHTBLOCK);
            theMap[1-x][-4] = enumToBlock(blockDataType.LIGHTBLOCK);
        }
    }

    public void OnDestroy()
    {
        theMap = null;
    }
}
