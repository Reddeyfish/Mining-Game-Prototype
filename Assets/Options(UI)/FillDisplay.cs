using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class FillDisplay : MonoBehaviour {
    private Text text;
    void Awake()
    {
        
    }

    public void set(int fill, int max = -1)
    {
        if(text == null)
            text = transform.Find("FillText").GetComponent<Text>(); //issues with set being called before Awake
        if (max > 0)
        {
            text.text = fill.ToString() + "/" + max.ToString();
        }
        else //max has default value; there is no max
        {
            text.text = fill.ToString();
        }
    }
}
