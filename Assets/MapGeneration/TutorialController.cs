using UnityEngine;
using System.Collections;
using System.IO;

    //unlike the world controller, loads a pre-set map. This particular scripts will load the tutorial level, but this script can be extended to load other levels (just ovrride the InitializeMat() method)

public class TutorialController : WorldController, IDigListener {

    private DiggingListenerSystem listener;

    protected override void Start()
    {
        base.Start();
        listener = GameObject.FindGameObjectWithTag(Tags.player).GetComponentInChildren<DiggingListenerSystem>();
        listener.Subscribe(this);
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
                theMap[x][y] = enumToBlock(blockDataType.TRANSPARENTMAP);
            }
        }

        for (int x = -3; x <= 3; x++)
            for (int y = -3; y <= 3; y++)
                theMap[x][y] = enumToBlock(blockDataType.EMPTYBLOCK);
    }

    public void OnNotify(Block block)
    {

    }
    public void OnDestroy()
    {
        listener.UnSubscribe(this);
        theMap = null;
    }
}
