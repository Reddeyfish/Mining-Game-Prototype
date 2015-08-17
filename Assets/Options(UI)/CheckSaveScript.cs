using UnityEngine;
using System.Collections;

public class CheckSaveScript : MonoBehaviour {

	//for the close button closing the inspect panel to call

    public bool baseMode { get; set; }
    SaveObservable save;
    void Awake()
    {
        baseMode = false;
        save = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<SaveObservable>();
    }

    public void checkSave()
    {
        if (baseMode)
            save.Save();
    }
}
