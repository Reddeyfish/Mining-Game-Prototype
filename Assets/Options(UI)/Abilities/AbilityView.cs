using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AbilityView : MonoBehaviour {
    Image mainIcon;

    private static Color readyColor = new Color(0, 0.5f, 0.78f); //should really be const, but the compiler throws errors
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
	// Use this for initialization
	void Start () {
        mainIcon = GetComponent<Image>(); //temp code
	}

    public void Initialize(int number)
    {
        //temp code
    }
}
