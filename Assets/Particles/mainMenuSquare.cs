using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class mainMenuSquare : MonoBehaviour {
	// Use this for initialization

    const float distanceScalar = 100;

    RectTransform thisTransform;
    Vector2 maxSize;
    Image myImage;

    int x;
    int y;

    private Vector2 actualSize
    {
        set { thisTransform.sizeDelta = value; }
        get { return thisTransform.sizeDelta; }
    }

	void Awake () {
        thisTransform = (RectTransform)(this.transform);
        maxSize = actualSize;
        myImage = GetComponent<Image>();
	}

    public void Initialize(Vector2 maxSize, int x, int y)
    {
        this.maxSize = maxSize;
        this.x = x;
        this.y = y;
    }

    float getMouseDistance() { return (thisTransform.position - Input.mousePosition).magnitude / distanceScalar; }

	// Update is called once per frame
	void Update () {
        // 1/(1+x)
        actualSize = (1 / (1 + getMouseDistance())) * maxSize;

        Vector3 colorValues = RandomLib.PerlinColor(backgroundSquares.perlinSeedX, backgroundSquares.perlinSeedY, x + backgroundSquares.perlinSeedXOffset, y);
        myImage.color = HSVColor.HSVToRGB(colorValues.x, colorValues.y, colorValues.z);
	}

    
}
