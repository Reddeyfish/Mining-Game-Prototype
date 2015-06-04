using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class NotificationPopup : BasePopup {
    Text label;
    public string Text { get { return label.text; } set { label.text = value; } }
	// Use this for initialization
    protected override void Awake()
    {
        base.Awake();
        label = transform.Find("UI/Label").GetComponent<Text>();
    }
}
