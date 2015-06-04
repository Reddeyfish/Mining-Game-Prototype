using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
//script that controls the player's on-body resource inventory

public class Inventory : MonoBehaviour
{
    private List<Resource> resources;
    public int maxSize;
    private int currentSize = 0;
    public int Fill { get { return currentSize; } }
    public List<Resource> Resources { get { return resources; } }

    public GameObject inventoryFullMessage;

    void Awake()
    {
        resources = new List<Resource>();
    }

    public int Add(Resource resource)
    {
        int count = spaceRemaining();
        if (count > resource.count)
        {
            count = resource.count;
        }
        else
        {
            //inventory full
            SimplePool.Spawn(inventoryFullMessage, this.transform.position);
        }
        int loc = findResource(resource);
        if (loc != -1)
        {
            resources[loc] = resources[loc] + count;
        }
        else
        {
            resources.Add(resource.Resize(count));
        }
        currentSize += count;
        return count; //calling script now does the UI stuff
    }

    private int findResource(Resource newResource)
    {
        for (int i = 0; i < resources.Count; i++ )
        {
            if (newResource.Equals(resources[i]))
            {
                return i;
            }
        }
        return -1;
    }

    public int spaceRemaining()
    {
        return maxSize - currentSize;
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
}


public static class resourceTypeExtension
{
    public static string ToReadableString(this resourceType type)
    {
        switch (type)
        {
            case resourceType.PURECOLOR:
                return "Pure Colors";
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