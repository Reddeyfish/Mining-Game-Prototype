using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AbilityView : MonoBehaviour {
    protected Image mainIcon;
    protected Image arc;
    Text number;

    protected static Color readyColor = new Color(1f, 1f, 1f); //should really be const, but the compiler throws errors
    protected static Color cooldownColor = new Color(0.5f, 0.5f, 0.5f);
    protected bool _ready;
    public float Fill
    {
        get { return arc.fillAmount; }
        set { arc.fillAmount = value; }
    }
	// Use this for initialization
	protected virtual void Awake () {
        mainIcon = transform.Find("Arc/Icon").GetComponent<Image>();
        arc = transform.Find("Arc").GetComponent<Image>();
        number = transform.Find("Number").GetComponent<Text>();
	}

    public void Initialize(int num)
    {
        number.text = num.ToString();
    }

    public virtual void setReady(bool value)
    {
        _ready = value;
        mainIcon.color = value ? readyColor : cooldownColor;
    }
}
