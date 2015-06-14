using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AbilityView : MonoBehaviour {
    Image mainIcon;
    Image arc;
    Text number;

    private static Color readyColor = new Color(1f, 1f, 1f); //should really be const, but the compiler throws errors
    private static Color cooldownColor = new Color(0.5f, 0.5f, 0.5f);
    private bool _ready;
    public bool Ready
    {
        get { return _ready; }
        set{
            _ready = value;
            mainIcon.color = value ? readyColor : cooldownColor;
        }
    }
    public float Fill
    {
        get { return arc.fillAmount; }
        set { arc.fillAmount = value; }
    }
	// Use this for initialization
	void Awake () {
        mainIcon = transform.Find("Arc/Icon").GetComponent<Image>();
        arc = transform.Find("Arc").GetComponent<Image>();
        number = transform.Find("Number").GetComponent<Text>();
	}

    public void Initialize(int num)
    {
        number.text = num.ToString();
    }
}
