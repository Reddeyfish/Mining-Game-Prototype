using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
//script that controls the player's in-base inventory

public class BaseInventory : MonoBehaviour, IDisabledAwake
{
    protected List<Resource> resources;
    protected int currentSize = 0;
    public int Fill { get { return currentSize; } }
    public List<Resource> Resources { get { return resources; } }
    protected virtual string getKey() { return PlayerPrefKeys.baseInventory; }

    public void Awaken()
    {
        LoadData();
    }

    public virtual int Add(Resource resource)
    {
        int loc = findResource(resource);
        if (loc != -1)
        {
            resources[loc] = resources[loc] + resource.count;
        }
        else
        {
            resources.Add(resource);
        }
        currentSize += resource.count;
        return resource.count; //calling script now does the UI stuff
    }

    private int findResource(Resource newResource)
    {
        for (int i = 0; i < resources.Count; i++)
        {
            if (newResource.Equals(resources[i]))
            {
                return i;
            }
        }
        return -1;
    }

    void OnDestroy()
    {
        // save data

        //could possibly optimize the format by grouping by type, but maybe later
        int[] test = new int[resources.Count * 3];
        for (int i = 0; i < resources.Count; i++)
        {
            test[3 * i] = (int)(resources[i].type);
            test[3 * i + 1] = (int)(resources[i].color);
            test[3 * i + 2] = resources[i].count;

        }
        bool success = PlayerPrefsX.SetIntArray(getKey(), test);
        if (!success)
        {
            Debug.Log("Inventory Save Failed!");
        }
        else
        {
            Debug.Log("Inventory Save Complete!");
        }
    }

    void LoadData()
    {
        resources = new List<Resource>();

        if (PlayerPrefs.HasKey(getKey()))
        {
            int[] data = PlayerPrefsX.GetIntArray(getKey());
            Debug.Log("Inventory Data Found!");
            if (data.Length % 3 != 0)
                Debug.Log("Inventory Size Mismatch");
            for (int i = 0; i < data.Length / 3; i++)
            {
                resources.Add(new Resource((resourceType)data[3 * i], (colorType)data[3 * i + 1], data[3 * i + 2]));
                currentSize += data[3 * i + 2];
            }
        }
    }
}

public enum colorType
{
    RED = 0,
    GREEN = 1,
    BLUE = 2,
}

public enum resourceType
{
    PURECOLOR = 0,
    UNSTABLE = 1,
    HARDENED = 2,
}


public static class resourceTypeExtension
{
    public static string ToReadableString(this resourceType type)
    {
        switch (type)
        {
            case resourceType.PURECOLOR:
                return "Pure Colors";
            case resourceType.UNSTABLE:
                return "Unstable";
            case resourceType.HARDENED:
                return "Hardened";
            default:
                return "Resource";
        }
    }

    public static Color ToColor(this colorType type)
    {
        switch (type)
        {
            case colorType.RED:
                return Color.red;
            case colorType.GREEN:
                return Color.green;
            case colorType.BLUE:
                return Color.blue;
            default:
                return Color.clear;
        }
    }
}

[System.Serializable]
public class Resource : System.IEquatable<Resource>, System.IComparable<Resource>
{
    public resourceType type;
    public colorType color;
    public int count;
    public Resource(resourceType type, colorType color, int count)
    {
        this.type = type;
        this.color = color;
        this.count = count;
    }

    public Resource Resize(int count)
    {
        return new Resource(this.type, this.color, count);
    }

    public bool Equals(Resource obj)
    {
        // If parameter is null return false.
        if (obj == null)
        {
            return false;
        }

        // If parameter cannot be cast to Point return false.
        Resource p = obj as Resource;
        if ((System.Object)p == null)
        {
            return false;
        }

        // Return true if the fields match:
        return (type == p.type) && (color == p.color);
    }

    public static Resource operator +(Resource c1, int count)
    {
        return new Resource(c1.type, c1.color, c1.count + count);
    }

    public int CompareTo(Resource other)
    {
        return (int)type * System.Enum.GetValues(typeof(colorType)).Length + (int)color - (int)(other.type) * System.Enum.GetValues(typeof(colorType)).Length + (int)(other.color);
    }
}