using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class InventoryEntry : MonoBehaviour {
    private Image red;
    private Image green;
    private Image blue;
    private Text redText;
    private Text greenText;
    private Text blueText;

    private Text nameText;

    //maybe tooltips later

    private const int greyedAlpha = 75;
    private const float normalAlpha = 1f;

    private int _red = 0;
    private int _green = 0;
    private int _blue = 0;
    public int Red {
        get { return _red; }
        set
        {
            _red = value;
            redText.text = Format.makeReadable(value);
            red.color = greyColoring(value, red.color);
        }
    }
    public int Green
    {
        get { return _green; }
        set
        {
            _green = value;
            greenText.text = Format.makeReadable(value);
            green.color = greyColoring(value, green.color);
        }
    }
    public int Blue
    {
        get { return _blue; }
        set
        {
            _blue = value;
            blueText.text = Format.makeReadable(value);
            blue.color = greyColoring(value, blue.color);
        }
    }

    private Color greyColoring(int value, Color color)
    {
        if (value != 0)
            color = HSVColor.setAlphaFloat(color, normalAlpha);
        else
            color = HSVColor.setAlphaInt(color, greyedAlpha);
        return color;
    }

    private resourceType _type;
    public resourceType Type
    {
        get { return _type; }
        set
        {
            _type = value;
            nameText.text = Resource.TypeToString(value);
        }
    }

    



    void Awake()
    {
        red = transform.Find("Red").GetComponent<Image>();
        redText = transform.Find("Red/Count").GetComponent<Text>();
        green = transform.Find("Green").GetComponent<Image>();
        greenText = transform.Find("Green/Count").GetComponent<Text>();
        blue = transform.Find("Blue").GetComponent<Image>();
        blueText = transform.Find("Blue/Count").GetComponent<Text>();
        nameText = transform.Find("Text").GetComponent<Text>();
    }

    public void Initialize(int red, int green, int blue, resourceType type) //not sure if this is necessary, but eh
    {
        this.Red = red;
        this.Green = green;
        this.Blue = blue;
        this.Type = type;
    }
}
