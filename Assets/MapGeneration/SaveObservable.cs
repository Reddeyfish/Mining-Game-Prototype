using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SaveObservable : MonoBehaviour {

    List<ISaveListener> listeners;

    public GameObject popup;

	// Use this for initialization
	void Awake () {
        listeners = new List<ISaveListener>();
	}
	
	// Update is called once per frame
	public void Subscribe (ISaveListener listener) {
        listeners.Add(listener);
	}

    public void UnSubscribe(ISaveListener listener)
    {
        listeners.Remove(listener);
    }

    public void Save()
    {
        SimplePool.Spawn(popup);
        
        foreach (ISaveListener listener in listeners)
            listener.NotifySave();
    }
}

public interface ISaveListener
{
    void NotifySave();
    void OnDestroy();
}