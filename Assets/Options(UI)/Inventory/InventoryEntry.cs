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

    private const int greyedAlpha = 50;
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
            color = color.setAlphaFloat(normalAlpha);
        else
            color = color.setAlphaFloat(greyedAlpha);
        return color;
    }

    private resourceType _type;
    public resourceType Type
    {
        get { return _type; }
        set
        {
            _type = value;
            nameText.text = value.ToReadableString();
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

    public void set(Resource resource)
    {
        //probably should assert that the types are equal. Ah well.

        switch (resource.color)
        {
            case colorType.RED:
                this.Red = resource.count;
                break;
            case colorType.GREEN:
                this.Green = resource.count;
                break;
            case colorType.BLUE:
                this.Blue = resource.count;
                break;
        }
    }
}
