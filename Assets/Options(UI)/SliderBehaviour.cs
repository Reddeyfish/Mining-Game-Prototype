using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SliderBehaviour : MonoBehaviour {
    public string optionString;
    private Text SliderPercentLabel;
    private Slider slider;
	// Use this for initialization
	void Start () {
        slider = GetComponent<Slider>();
        if (optionString == "")
            Debug.Log("No option string set. This slider will not function");
        if (PlayerPrefs.HasKey(optionString))
            slider.value = PlayerPrefs.GetInt(optionString)/100.0f;
	}

    public void SliderUpdate()
    {
        if (slider == null)
            return; //this gets called when everything loads, before start

        //update visuals
        int newValue = (int)(100*slider.value);

        //update data
        PlayerPrefs.SetInt(optionString, newValue);
        PlayerPrefs.Save();

        //update the actual sound
        VolumeController.DoUpdate(); 
    }
}
