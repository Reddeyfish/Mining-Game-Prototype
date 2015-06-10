using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class InspectTab : MonoBehaviour, IDisabledAwake {
    Outline outline;
    private static Color greyColor = new Color(0.6f, 0.6f, 0.6f);
    private static Color activeColor = new Color(0, 0.4f, 0.6f);
	// Use this for initialization
	public void Awaken () {
        outline = GetComponent<Outline>();
	}
	
	// Update is called once per frame
    public void SetGrey(bool grey)
    {
        if (grey)
            outline.effectColor = greyColor;
        else
            outline.effectColor = activeColor;

    }
}
