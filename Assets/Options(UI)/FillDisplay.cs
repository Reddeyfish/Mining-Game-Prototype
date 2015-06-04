using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class FillDisplay : MonoBehaviour {
    private Text text;
    void Awake()
    {
        
    }

    public void set(int fill, int max)
    {
        if(text == null)
            text = transform.Find("FillText").GetComponent<Text>(); //issues with set being called before Awake
        text.text = fill.ToString() + "/" + max.ToString();
    }
}
