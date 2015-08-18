using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class CostEntry : MonoBehaviour {
    Text title;
    Image icon;
    Image typeIcon;
    Text countDisplay;
    GameObject blocker;
    private int _count;
    private int Count { get { return _count; }
        set { _count = value;
        countDisplay.text = "x" + value.ToString();
        }
    }
    private resourceType _type;
    private resourceType Type { get { return _type;}
        set {
             typeIcon.sprite = value.UISprite();
             _type = value;
        }
    }
    private costType _cost;
    private costType Cost
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
        icon = transform.Find("Icon").GetComponent<Image>();
        typeIcon = transform.Find("Icon/Image").GetComponent<Image>();
        countDisplay = transform.Find("Count").GetComponent<Text>();
        blocker = transform.Find("Blocker").gameObject;
	}

    void Initialize(resourceType type, int count, costType cost)
    {
        Type = type;
        Count = count;
        Cost = cost;
    }

    public bool Initialize(Cost cost, ItemsView manager)
    {
        Type = cost.type;
        Count = cost.count;
        Cost = cost.cost;

        return affordable(manager);
    }

    public bool affordable(ItemsView manager)
    {
        if (manager.getInventory().canPayCost(new Cost(Type, Count, Cost)))
        {
            blocker.SetActive(false);
            return true;
        }
        else
        {
            blocker.SetActive(true);
        }
        return false;
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