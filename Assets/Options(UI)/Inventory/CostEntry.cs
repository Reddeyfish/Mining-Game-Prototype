using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CostEntry : MonoBehaviour {
    Text title;
    Image icon;
    Text countDisplay;
    private int _count;
    public int Count { get { return _count; }
        set { _count = value;
        countDisplay.text = "x" + value.ToString();
        }
    }
    private resourceType _type;
    public resourceType Type { get { return _type;}
        set { _type = value;
        title.text = value.ToReadableString();
        }
    }
    private costType _cost;
    public costType Cost
    {
        get { return _cost; }
        set
        {
            _cost = value;
            switch (value)
            {
                case costType.RED:
                    icon.color = Color.red;
                    icon.sprite = null;
                    break;
                case costType.GREEN:
                    icon.color = Color.green;
                    icon.sprite = null;
                    break;
                case costType.BLUE:
                    icon.color = Color.blue;
                    icon.sprite = null;
                    break;
                case costType.WHITE:
                    icon.color = Color.white;
                    icon.sprite = null;
                    break;
                case costType.ANY:
                    icon.color = Color.white;
                    icon.sprite = multihue;
                    break;
                default:
                    icon.color = Color.clear;
                    icon.sprite = null;
                    break;
            }
        }
    }

    public Sprite multihue;

	// Use this for initialization
	void Awake () {
        title = transform.Find("Title").GetComponent<Text>();
        icon = transform.Find("Icon").GetComponent<Image>();
        countDisplay = transform.Find("Count").GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void Initialize(resourceType type, int count, costType cost)
    {
        Type = type;
        Count = count;
        Cost = cost;
    }

    public void Initialize(Cost cost)
    {
        Type = cost.type;
        Count = cost.count;
        Cost = cost.cost;
    }
}

public enum costType
{
    RED,
    GREEN,
    BLUE,
    WHITE,
    ANY,
}