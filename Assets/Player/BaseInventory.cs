using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
//script that controls the player's in-base inventory

public class BaseInventory : MonoBehaviour, IDisabledStart, ISaveListener
{
    SaveObservable observable;
    protected List<Resource> resources;
    protected int currentSize = 0;
    public float refundRate = 0.6f;
    public int Fill { get { return currentSize; } }
    public List<Resource> Resources { get { return resources; } }
    protected virtual string getKey() { return PlayerPrefKeys.baseInventory; }

    public void StartDisabled()
    {
        observable = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<SaveObservable>();
        observable.Subscribe(this);
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

    private bool containsResource(Resource resource)
    {
        int index = findResource(resource);
        if (index < 0)
        {
            return false;
        }
        if (resources[index].count < resource.count)
        {
            return false;
        }
        return true;
    }

    private int getResourceCount(Resource resource)
    {
        int index = findResource(resource);
        return index < 0 ? 0 : resources[index].count;
    }

    private void Subtract(Resource resource)
    {
        int index = findResource(resource);
        resources[index] = resources[index].Deduct(resource);
        currentSize -= resource.count;
    }

    public bool canPayCost(Cost cost)
    {
        //assume only one mode; we won't have white + any costs on the same resource
            switch (cost.cost)
            {
                case costType.RED:
                    if(!containsResource(new Resource(cost.type, colorType.RED, cost.count))) return false;    
                    break;

                case costType.GREEN:
                    if(!containsResource(new Resource(cost.type, colorType.GREEN, cost.count))) return false;    
                    break;

                case costType.BLUE:
                    if(!containsResource(new Resource(cost.type, colorType.BLUE, cost.count))) return false;    
                    break;

                case costType.WHITE:
                    //all 3 resources
                    if (!containsResource(new Resource(cost.type, colorType.RED, cost.count))) return false;
                    if (!containsResource(new Resource(cost.type, colorType.GREEN, cost.count))) return false;
                    if (!containsResource(new Resource(cost.type, colorType.BLUE, cost.count))) return false;
                    break;
  
                case costType.ANY:
                    if (getResourceCount(new Resource(cost.type, colorType.RED)) 
                        + getResourceCount(new Resource(cost.type, colorType.GREEN)) 
                        + getResourceCount(new Resource(cost.type, colorType.BLUE)) 
                        < cost.count) 
                        return false;
                    break;
            }
        return true;
    }

    public void PayCosts(Cost[] costs)
    {
        
        foreach (Cost cost in costs)
        {
            switch (cost.cost)
            {
                case costType.RED:
                    Subtract(new Resource(cost.type, colorType.RED, cost.count));
                    break;
                case costType.GREEN:
                    Subtract(new Resource(cost.type, colorType.GREEN, cost.count));
                    break;
                case costType.BLUE:
                    Subtract(new Resource(cost.type, colorType.BLUE, cost.count));
                    break;
                case costType.WHITE:
                    Subtract(new Resource(cost.type, colorType.RED, cost.count));
                    Subtract(new Resource(cost.type, colorType.GREEN, cost.count));
                    Subtract(new Resource(cost.type, colorType.BLUE, cost.count));
                    break;
                case costType.ANY:
                    /*remove the resources in such a way that the resulting values are as close as equal as possible.
                    
                     * in other words:
                     * while(cost.count > 0)
                     * //deduct one from the tallest resource
                     * //cost.count--;
                     * 
                     * different algorithm, but the same result
                    */
                    int[] relevantResources = {findResource(new Resource(cost.type, colorType.RED)),
                                                   findResource(new Resource(cost.type, colorType.GREEN)),
                                                   findResource(new Resource(cost.type, colorType.BLUE)),};
                    int full = 0;
                    for (int i = 0; i < 3; i++)
                        if (relevantResources[i] != -1)
                            full++;
                    switch (full)
                    {
                            //there shouldn't be a case 0; if there is, then there is either a zero cost, or someone didn't check that this was payable
                        case 1:
                            int index = relevantResources[0];
                            if (index == -1)
                            {
                                index = relevantResources[1];
                                if (index == -1)
                                    index = relevantResources[2];
                            }
                            resources[index] = resources[index].Deduct(cost.count);
                            break;
                        
                        case 2:
                            int[] newRelevantResources = new int[2];
                            int offset = 0;
                            for (int i = 0; i < 3; i++)
                            {
                                if (relevantResources[i] != -1)
                                {
                                    newRelevantResources[i + offset] = relevantResources[i];
                                }
                                else
                                {
                                    offset -= 1;
                                }
                            }
                            if (resources[newRelevantResources[0]] > resources[newRelevantResources[1]])
                                newRelevantResources = new int[] { newRelevantResources[1], newRelevantResources[0] };
                            //now sorted, ascending
                            int max = resources[newRelevantResources[1]].count - resources[newRelevantResources[0]].count;
                            if (max >= cost.count)
                            {
                                resources[newRelevantResources[1]] = resources[newRelevantResources[1]].Deduct(cost.count);
                                break;
                            }

                            int min = resources[relevantResources[0]].count;

                            min = (cost.count - max) / 2;
                            resources[newRelevantResources[1]] = resources[newRelevantResources[1]].Deduct(max + min + ((cost.count - max) % 2));
                            resources[newRelevantResources[0]] = resources[newRelevantResources[0]].Deduct(min);
                            break;

                        case 3:
                            //sort
                            if (resources[relevantResources[0]] <= resources[relevantResources[1]])
                            {
                                if (resources[relevantResources[1]] > resources[relevantResources[2]])
                                {
                                    if (resources[relevantResources[0]] > resources[relevantResources[2]])
                                    {
                                        //swap
                                        int temp = relevantResources[0];
                                        relevantResources[0] = relevantResources[2];
                                        relevantResources[2] = relevantResources[1];
                                        relevantResources[1] = temp;
                                    }
                                    else
                                    {
                                        int temp = relevantResources[1];
                                        relevantResources[1] = relevantResources[2];
                                        relevantResources[2] = temp;
                                    }
                                }
                                else
                                {
                                    // do nothing; correctly sorted
                                }
                            }
                            else
                            {
                                if (resources[relevantResources[0]] > resources[relevantResources[2]])
                                {
                                    if (resources[relevantResources[1]] < resources[relevantResources[2]])
                                    {
                                        int temp = relevantResources[0];
                                        relevantResources[0] = relevantResources[1];
                                        relevantResources[1] = relevantResources[2];
                                        relevantResources[2] = temp;
                                    }
                                    else
                                    {
                                        int temp = relevantResources[0];
                                        relevantResources[0] = relevantResources[2];
                                        relevantResources[2] = temp;
                                    }
                                }
                                else
                                {
                                    int temp = relevantResources[0];
                                    relevantResources[0] = relevantResources[1];
                                    relevantResources[1] = temp;
                                }
                            }
                            //now sorted in ascending order
                            int two = resources[relevantResources[2]].count - resources[relevantResources[1]].count;
                            Debug.Log(two);
                            if (two >= cost.count)
                            {
                                Debug.Log(cost.count);
                                resources[relevantResources[2]] = resources[relevantResources[2]].Deduct(cost.count);
                                break;
                            }

                            int one = resources[relevantResources[1]].count - resources[relevantResources[0]].count;

                            if (two + 2 * one >= cost.count)
                            {
                                one = (cost.count - two) / 2; //deduction for each of the two resources in the 'one' group (leaving out remainders)
                                resources[relevantResources[2]] = resources[relevantResources[2]].Deduct(two + one + ((cost.count - two) % 2));
                                resources[relevantResources[1]] = resources[relevantResources[1]].Deduct(one);
                                break;
                            }

                            //no need for an if; this is the last one

                            int zero = (cost.count - two - 2 * one) / 3;
                            int remainder = (cost.count - two - 2 * one) % 3;
                            resources[relevantResources[2]] = resources[relevantResources[2]].Deduct(two + one + zero + (remainder > 0 ? 1 : 0));
                            resources[relevantResources[1]] = resources[relevantResources[1]].Deduct(one + zero + (remainder > 1 ? 1 : 0));
                            resources[relevantResources[0]] = resources[relevantResources[0]].Deduct(zero);

                            break;
                    }
                    break;
            }
        }
    }

    public void RefundCosts(Cost[] costs)
    {
        foreach (Cost cost in costs)
        {
            switch (cost.cost)
            {
                case costType.RED:
                    Add(new Resource(cost.type, colorType.RED, (int)(refundRate * cost.count)));
                    break;
                case costType.GREEN:
                    Add(new Resource(cost.type, colorType.GREEN, (int)(refundRate * cost.count)));
                    break;
                case costType.BLUE:
                    Add(new Resource(cost.type, colorType.BLUE, (int)(refundRate * cost.count)));
                    break;
                case costType.WHITE:
                    Add(new Resource(cost.type, colorType.RED, (int)(refundRate * cost.count)));
                    Add(new Resource(cost.type, colorType.GREEN, (int)(refundRate * cost.count)));
                    Add(new Resource(cost.type, colorType.BLUE, (int)(refundRate * cost.count)));
                    break;
                case costType.ANY:
                    Add(new Resource(cost.type, colorType.RED, (int)(refundRate * cost.count / 3)));
                    Add(new Resource(cost.type, colorType.GREEN, (int)(refundRate * cost.count / 3)));
                    Add(new Resource(cost.type, colorType.BLUE, (int)(refundRate * cost.count / 3)));
                    break;
            }
        }
    }

    public void OnDestroy()
    {
        observable.UnSubscribe(this);
    }

    public void NotifySave()
    {
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
                return "Pure";
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

    public static Sprite UISprite(this resourceType type)
    {
        return Resources.Load<Sprite>("ResourceUIGlyphs/" + type.ToReadableString() + "UIGlyph");
    }
}

[System.Serializable]
public class Resource : System.IEquatable<Resource>, System.IComparable<Resource>
{
    public resourceType type;
    public colorType color;
    public int count;
    public Resource(resourceType type, colorType color, int count = 0)
    {
        this.type = type;
        this.color = color;
        this.count = count;
    }

    public Resource Resize(int count)
    {
        return new Resource(this.type, this.color, count);
    }

    public Resource Deduct(int count)
    {
        return new Resource(this.type, this.color, this.count - count);
    }

    public Resource Deduct(Resource other)
    {
        return new Resource(this.type, this.color, this.count - other.count);
    }

    public Resource Add(Resource other)
    {
        return new Resource(this.type, this.color, this.count + other.count);
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

    public static bool operator <(Resource r1, Resource r2)
    {
        return r1.count < r2.count;
    }

    public static bool operator <=(Resource r1, Resource r2)
    {
        return r1.count <= r2.count;
    }

    public static bool operator >(Resource r1, Resource r2)
    {
        return r1.count > r2.count;
    }

    public static bool operator >=(Resource r1, Resource r2)
    {
        return r1.count >= r2.count;
    }

    public int CompareTo(Resource other)
    {
        if ((int)type - (int)(other.type) != 0)
        {
            return (int)type - (int)(other.type);
        }
        return  (int)color - (int)(other.color);
    }
}