﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class WorldController : MonoBehaviour, ISaveListener {
    SaveObservable observable;
    protected static Map theMap;
    protected virtual int mapSize() { return 300; }
    private Point loadedTopRight = new Point(0, 0);
    private Point loadedBottomLeft = new Point(0, 0);
    [HideInInspector] //public for the random library to use
    public static float ColorSeedX = -288;
    [HideInInspector]
    public static float ColorSeedY = -800;
    private static float OreSeedX = 0;
    private static float OreSeedY = 0;
    [HideInInspector]
    public static float TransSeedX = 0;
    [HideInInspector]
    public static float TransSeedY = 0;
    private static float ObstacleSeed = 0;

    private const float tunnelFrequency = 0.97f;
    private const int tunnelLengthMin = 3;
    private const int tunnelLengthMax = 8;

    //have these frequencies descending, because they're used in an if/elseif

    private const float plusExplosiveFrequency = 0.95f;
    private const float plusExplosiveMinRange = 60f;
    private const float explosiveFrequency = 0.9f; 
    private const float explosiveMinRange = 15f;
    private const float boulderFrequency = 0.8f;
    private const float boulderMinRange = 30f;
    private const float oreFrequency = 0.65f;
    public const float transFrequency = 0.9f;
    public int loadedRange = 16;
    public int hierarchyNoiseRange = 3;

    [Header("the prefabs for each block type")]
    public GameObject DirtBlock;
    public GameObject OreBlock;
    public GameObject CrystalLight;
    public GameObject Boulder;
    public GameObject BoulderInterior;
    public GameObject ExplosiveBlock;
    public GameObject PlusExplosiveBlock;
    public GameObject TransparentBlock;
    public GameObject homeBase;
    public GameObject capsule;
    
    public static WorldController thi;
	// Use this for initialization

	protected virtual void Start () {
        thi = this;

        observable = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<SaveObservable>();
        observable.Subscribe(this);

        LoadData();

        RecreateCreatedBlocks();

        //start the update
        StartCoroutine(UpdateCallback());
        
	}

    protected virtual void InitializeMap()
    {
         // definitely something to optimize

        if (theMap != null)
        {
            Debug.Log("ERROR: THERE SHOULD ONLY BE ONE MAP");
            Destroy(this.gameObject);
        }
        theMap = new Map(mapSize());

        //initialize data blocks
        for (int x = -mapSize() + 1; x < mapSize(); x++)
            for (int y = -mapSize() + 1; y < mapSize(); y++)
            {
                if (Random.value > tunnelFrequency) //tunnel in x direction
                {
                    int start = x - Random.Range(tunnelLengthMin, tunnelLengthMax);
                    if(start < -mapSize() + 1)
                        start = -mapSize() + 1;
                    for (int i = start; i <= x; i++)
                        theMap[i][y] = new EmptyBlock();
                }
                else if (Random.value > tunnelFrequency) //tunnel in y direction
                {
                    int start = y - Random.Range(tunnelLengthMin, tunnelLengthMax);
                    if (start < -mapSize() + 1)
                        start = -mapSize() + 1;
                    for (int i = start; i <= y; i++)
                        theMap[x][i] = new EmptyBlock();
                }
                else
                {
                    theMap[x][y] = new FullBlock();
                }
            }

        // spawning area
        for (int x = -3; x <= 3; x++)
            for (int y = -3; y <= 3; y++)
                theMap[x][y] = new EmptyBlock();
        //now the real map

        LoadMapFromArray(theMap.toArray());
    }

    private void LoadMapFromArray(bool[] boolArray)
    {
        if (boolArray.Length != (((2 * mapSize()) - 1) * ((2 * mapSize()) - 1)))
        {
            Debug.Log("Data array size mismatch");
            Debug.Log(mapSize());
            Debug.Log((((2 * mapSize()) - 1) * ((2 * mapSize()) - 1)));
            Debug.Log(boolArray.Length);
        }
        theMap = new Map(mapSize());

        int newRandomSeed = Random.Range(-9999, 9999);

        Random.seed = (int)ObstacleSeed;

        int i = 0;
        for (int x = -mapSize() + 1; x < mapSize(); x++)
        {
            for (int y = -mapSize() + 1; y < mapSize(); y++)
            {
                theMap[x][y] = getBlockTypeByPosition(x, y, boolArray[i]);
                i++;
            }
        }

        Random.seed = newRandomSeed;
    }

    private EmptyBlock getBlockTypeByPosition(float x, float y, bool data)
    {
        float obstacleValue = Random.value;
        if (!data)
        {
            if (x == 0 && y == 0)
            {
                return enumToBlock(blockDataType.BASE);
            }
            else if (x % 40 == 0 && y % 40 == 0) //I will make this more randomized, but this works for now
            {
                return enumToBlock(blockDataType.CAPSULE);
            }
            if (obstacleValue > boulderFrequency && new Vector2(x, y).magnitude > boulderMinRange)
            {
                return enumToBlock(blockDataType.EMPTYBLOCK);
            }
            else if (Mathf.PerlinNoise(OreSeedX + 19 * x / 51, OreSeedY + 19 * y / 51) > oreFrequency)
            {
                return enumToBlock(blockDataType.LIGHTBLOCK);
            }
            else
            {
                return enumToBlock(blockDataType.EMPTYBLOCK);
            }
        }
        else
        {
            if (x % 40 == 0 && y % 40 == 0) //I will make this more randomized, but this works for now
            {
                return enumToBlock(blockDataType.CAPSULE);
            }

            if (obstacleValue > plusExplosiveFrequency && new Vector2(x, y).magnitude > plusExplosiveMinRange)
            {
                return enumToBlock(blockDataType.PLUSEXPLOSIVE);
            }
            else if (obstacleValue > explosiveFrequency && new Vector2(x, y).magnitude > explosiveMinRange)
            {
                return enumToBlock(blockDataType.EXPLOSIVE);
            }
            else if (obstacleValue > boulderFrequency && new Vector2(x, y).magnitude > boulderMinRange)
            {
                return enumToBlock(blockDataType.BOULDER);
            }
            else if (Mathf.PerlinNoise(OreSeedX + 19 * x / 51, OreSeedY + 19 * y / 51) > oreFrequency)
            {
                return enumToBlock(blockDataType.OREBLOCK);
            }
            else if (Mathf.PerlinNoise(TransSeedX + 3 * x / 181, TransSeedY + 3 * y / 181) > transFrequency)
            {
                return enumToBlock(blockDataType.TRANSPARENTMAP);
            }
            else
            {
                return enumToBlock(blockDataType.MAPBLOCK);
            }
        }
    }

    private IEnumerator UpdateCallback()
    {
        while (true)
        {
            yield return new WaitForFixedUpdate();
            UpdateCreatedBlocks();
        }

    }

    protected virtual void LoadData()
    {
        //load our seeds
        LoadSeed(out ColorSeedX, PlayerPrefKeys.ColorX);
        LoadSeed(out ColorSeedY, PlayerPrefKeys.ColorY);
        LoadSeed(out OreSeedX, PlayerPrefKeys.OreX);
        LoadSeed(out OreSeedY, PlayerPrefKeys.OreY);
        LoadSeed(out TransSeedX, PlayerPrefKeys.TransX);
        LoadSeed(out TransSeedY, PlayerPrefKeys.TransY);
        LoadSeed(out ObstacleSeed, PlayerPrefKeys.obstacle);

        if (PlayerPrefs.HasKey(PlayerPrefKeys.map))
        {
            bool[] x = PlayerPrefsX.GetBoolArray(PlayerPrefKeys.map);
            Debug.Log("Map Data Found!");
            LoadMapFromArray(x);
        }
        else
        {
            InitializeMap();
        }
    }

    private void LoadSeed(out float seed, string key)
    {
        if (PlayerPrefs.HasKey(key))
        {
            seed = PlayerPrefs.GetFloat(key);
        }
        else
        {
            seed = Random.Range(-9999, 9999) / 11;
            PlayerPrefs.SetFloat(key, seed);
        }
    }

    public virtual void OnDestroy()
    {
        observable.UnSubscribe(this);
    }

    public virtual void NotifySave()
    {
        bool[] test = theMap.toArray();
        bool success = PlayerPrefsX.SetBoolArray(PlayerPrefKeys.map, test);
        if (!success)
        {
            Debug.Log("Map Save Failed!");
        }
        else
        {
            Debug.Log("Map Save Complete!");
        }
    }

    //updates the loaded blocks by trimming and adding to the edges. This breaks down if the shared area between old and new is zero (i.e. in case of a teleport)

    //For those cases, call RecreateCreatedBlocks()
    private void UpdateCreatedBlocks()
    {
        Transform target = Camera.main.transform;
        Point newTopRight = new Point(Mathf.Clamp((int)(target.position.x) + loadedRange, 1-mapSize(), mapSize() - 1), Mathf.Clamp((int)(target.position.y) + loadedRange, 1-mapSize(), mapSize() - 1));
        Point newBottomLeft = new Point(Mathf.Clamp((int)(target.position.x) - loadedRange, 1-mapSize(), mapSize() - 1), Mathf.Clamp((int)(target.position.y) - loadedRange, 1-mapSize(), mapSize() - 1));

        //add the new edges (invert the order of the points from removal)
        theMap.LoadRange(new Point(loadedTopRight.x + 1, loadedTopRight.y + 1), newTopRight); //top right
        theMap.LoadRange(new Point(loadedTopRight.x + 1, loadedBottomLeft.y), new Point(newTopRight.x, loadedTopRight.y)); //right
        theMap.LoadRange(new Point(loadedTopRight.x + 1, newBottomLeft.y), new Point(newTopRight.x, loadedBottomLeft.y - 1)); //bottom right
        theMap.LoadRange(new Point(loadedBottomLeft.x, newBottomLeft.y), new Point(loadedTopRight.x, loadedBottomLeft.y - 1)); //bottom
        theMap.LoadRange(newBottomLeft, new Point(loadedBottomLeft.x - 1, loadedBottomLeft.y - 1)); //bottom left
        theMap.LoadRange(new Point(newBottomLeft.x, loadedBottomLeft.y), new Point(loadedBottomLeft.x - 1, loadedTopRight.y)); //left
        theMap.LoadRange(new Point(newBottomLeft.x, loadedTopRight.y + 1), new Point(loadedBottomLeft.x - 1, newTopRight.y)); //top left
        theMap.LoadRange(new Point(loadedBottomLeft.x, loadedTopRight.y + 1), new Point(loadedTopRight.x, newTopRight.y)); // top

        //trim off the old edges
        theMap.RemoveRange(new Point(newTopRight.x + 1, newTopRight.y + 1), loadedTopRight); //top right
        theMap.RemoveRange(new Point(newTopRight.x + 1, newBottomLeft.y), new Point(loadedTopRight.x, newTopRight.y)); //right
        theMap.RemoveRange(new Point(newTopRight.x + 1, loadedBottomLeft.y), new Point(loadedTopRight.x, newBottomLeft.y - 1)); //bottom right
        theMap.RemoveRange(new Point(newBottomLeft.x, loadedBottomLeft.y), new Point(newTopRight.x, newBottomLeft.y - 1)); //bottom
        theMap.RemoveRange(loadedBottomLeft, new Point(newBottomLeft.x - 1, newBottomLeft.y - 1)); //bottom left
        theMap.RemoveRange(new Point(loadedBottomLeft.x, newBottomLeft.y), new Point(newBottomLeft.x - 1, newTopRight.y)); //left
        theMap.RemoveRange(new Point(loadedBottomLeft.x, newTopRight.y + 1), new Point(newBottomLeft.x - 1, loadedTopRight.y)); //top left
        theMap.RemoveRange(new Point(newBottomLeft.x, newTopRight.y + 1), new Point(newTopRight.x, loadedTopRight.y)); // top

        loadedTopRight = newTopRight;
        loadedBottomLeft = newBottomLeft;
  
    }

    //assumes that there is no overlap between old and new
    public void RecreateCreatedBlocks()
    {
        Transform target = Camera.main.transform;
        Point newTopRight = new Point(Mathf.Clamp(Mathf.RoundToInt(target.position.x) + loadedRange, -mapSize(), mapSize()), Mathf.Clamp(Mathf.RoundToInt(target.position.y) + loadedRange, -mapSize(), mapSize()));
        Point newBottomLeft = new Point(Mathf.Clamp(Mathf.RoundToInt(target.position.x) - loadedRange, -mapSize(), mapSize()), Mathf.Clamp(Mathf.RoundToInt(target.position.y) - loadedRange, -mapSize(), mapSize()));

        theMap.RemoveRange(loadedBottomLeft, loadedTopRight);

        theMap.LoadRange(newBottomLeft, newTopRight);

        loadedTopRight = newTopRight;
        loadedBottomLeft = newBottomLeft;
    }

    public static EmptyBlock enumToBlock(blockDataType data)
    {
        switch(data)
        {
            case blockDataType.EMPTYBLOCK:
                return new EmptyBlock();
            case blockDataType.MAPBLOCK:
                return new MapBlock(thi.DirtBlock);
            case blockDataType.OREBLOCK:
                return new MapBlock(thi.OreBlock);
            case blockDataType.LIGHTBLOCK:
                return new MapBlock(thi.CrystalLight);
            case blockDataType.BOULDER:
                return new MapBlock(thi.Boulder);
            case blockDataType.BOULDERINTERIOR:
                return new MapBlock(thi.BoulderInterior);
            case blockDataType.EXPLOSIVE:
                return new MapBlock(thi.ExplosiveBlock);
            case blockDataType.PLUSEXPLOSIVE:
                return new MapBlock(thi.PlusExplosiveBlock);
            case blockDataType.TRANSPARENTMAP:
                return new MapBlock(thi.TransparentBlock);
            case blockDataType.BASE:
                return new MapBlock(thi.homeBase);
            case blockDataType.CAPSULE:
                return new MapBlock(thi.capsule);
            default:
                Debug.Log("Data error");
                return null;
        }
    }


    //returns the block at the specified locations if it exists, else returns null
    public static GameObject getBlock(int x, int y)
    {
        return theMap[x][y].getBlock();
    }

    public static void UpdateBlock(int x, int y, blockDataType newBlock)
    {
        theMap[x][y] = enumToBlock(newBlock);
        theMap[x][y].Create(x, y);
    }

    //same as modify block, but does checking to see when things should be spawned/despawned
    public static void ModifyBlock(int x, int y, blockDataType newBlock, bool checkDespawn = true)
    {
        bool loaded = x <= thi.loadedTopRight.x && x >= thi.loadedBottomLeft.x && y <= thi.loadedTopRight.y && y >= thi.loadedBottomLeft.y;

        if (loaded && checkDespawn)
        {
            theMap[x][y].Remove();
        }

        theMap[x][y] = enumToBlock(newBlock);
        if (loaded)
        {
            theMap[x][y].Create(x, y);
        }
    }

    //gives the hierarchy value for a certain position
    public static int PerlinHierarchy(float seedX, float seedY, float x, float y)
    {
        //function of distance combined with noise
        float distance = Mathf.Sqrt(Mathf.Pow(x, 2) + Mathf.Pow(y, 2));
        return Mathf.RoundToInt(distance * Mathf.Log(distance)/100 + thi.hierarchyNoiseRange * Mathf.PerlinNoise(seedX + x / 97, seedY + y / 97));
    }
}

//bunch of classes

public enum blockDataType
{
    EMPTYBLOCK = 0,
    MAPBLOCK = 1,
    OREBLOCK = 2,
    LIGHTBLOCK = 3,
    BOULDER = 4,
    BOULDERINTERIOR = 5,
    EXPLOSIVE = 6,
    PLUSEXPLOSIVE = 7,
    TRANSPARENTMAP = 8,
    BASE = 9,
    CAPSULE = 10,
}

public class Point
{
    public int x;
    public int y;
    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

public class MapBlock : EmptyBlock //just a dirt block, nothing special
{
    GameObject self;
    GameObject prefab;
    private bool created = false;

    public MapBlock(GameObject prefab)
    {
        this.prefab = prefab;
    }

    public override void Create(int x, int y)
    {
        if (!created)
        {
            created = true;
            self = SimplePool.Spawn(prefab, new Vector2(x, y)) as GameObject;
#if UNITY_EDITOR
            self.transform.SetParent(WorldController.thi.transform); //hide in the inspector
#endif
        }
        else
        {
            Debug.Log(x);
            Debug.Log(y);
            Debug.Log("Already Created!");
        }
    }

    public override bool isSolid()
    {
        return (bool)(prefab.GetComponent<Block>().isSolid());
    }

    public override blockDataType getBlockDataType()
    {
        return prefab.GetComponent<Block>().getBlockType();
    }

    public override GameObject getBlock()
    {
        if (created)
            return self;
        else
            return null;
    }

    public override void Remove()
    {
        if (created)
        {
            created = false;
            SimplePool.Despawn(self.gameObject);
        }
        else
        {
            Debug.Log("Already Removed!");
        }
    }
}

public class EmptyBlock
{
    public EmptyBlock()
    {
    }
    public virtual void Create(int x, int y)
    {
    }
    public virtual bool isSolid()
    {
        return false;
    }

    public virtual blockDataType getBlockDataType()
    {
        return blockDataType.EMPTYBLOCK;
    }

    public virtual GameObject getBlock()
    {
        return null;
    }

    public virtual void Remove()
    {
    }
}

public class FullBlock : EmptyBlock
{
    public override bool isSolid()
    {
        return true;
    }
}

public class Map
{
    private BidirectionalMapArray[] positives;
    private BidirectionalMapArray[] negatives;
    private int size;
    public Map(int size)
    {
        this.size = size;
        this.positives = new BidirectionalMapArray[size];
        this.negatives = new BidirectionalMapArray[size - 1];
        for (int i = 0; i < size - 1; i++)
        {
            positives[i] = new BidirectionalMapArray(size);
            negatives[i] = new BidirectionalMapArray(size);
        }
        //and include an extra one because positives is one larger than negatives
        positives[size - 1] = new BidirectionalMapArray(size);
    }

    public BidirectionalMapArray this[int index] {
        get {
            if (index >= 0)
                return positives[index];
            else
            {
                return negatives[Mathf.Abs(index) - 1];
            }
        }
        set {
            if (index >= 0)
                positives[index] = value;
            else
                negatives[Mathf.Abs(index) - 1] = value;
        }
    }

    //ranges are inclusive
    public void RemoveRange(Point bottomLeft, Point topRight)
    {
        for(int x = bottomLeft.x; x <=topRight.x; x++)
            for (int y = bottomLeft.y; y <= topRight.y; y++)
                this[x][y].Remove();
    }

    public void LoadRange(Point bottomLeft, Point topRight)
    {
        /**
        Debug.Log("range");
        Debug.Log(bottomLeft.x);
        Debug.Log(bottomLeft.y);
        Debug.Log(topRight.x);
        Debug.Log(topRight.y);
        */
        
        for (int x = bottomLeft.x; x <= topRight.x; x++)
            for (int y = bottomLeft.y; y <= topRight.y; y++)
                this[x][y].Create(x, y);
    }

    public bool[] toArray()
    {
        bool[] result = new bool[((2 * size) - 1) * ((2 * size) - 1)];
        int i = 0;
        for (int x = -size + 1; x < size; x++)
            for (int y = -size + 1; y < size; y++)
            {
                result[i] = this[x][y].isSolid();
                i++;
            }
        return result;
    }
}

public class BidirectionalMapArray
{   
    private EmptyBlock[] positives;
    private EmptyBlock[] negatives;
    public BidirectionalMapArray(int size)
    {
        this.positives = new EmptyBlock[size];
        this.negatives = new EmptyBlock[size - 1];
    }

    public EmptyBlock this[int index]
    {
        get {
            if (index >= 0)
                return positives[index];
            else
                return negatives[Mathf.Abs(index) - 1];
        }
        set {
            if (index >= 0)
                positives[index] = value;
            else
                negatives[Mathf.Abs(index) - 1] = value;
        }
    }
}

