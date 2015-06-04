using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//class to notify all the various scripts when something is dug

public class DiggingListenerSystem : MonoBehaviour {

    private List<BaseDigListener> listeners;

	// Use this for initialization
	void Awake () {
        listeners = new List<BaseDigListener>();
	}

    public void Subscribe(BaseDigListener listener)
    {
        listeners.Add(listener);
    }

    public void UnSubscribe(BaseDigListener listener)
    {
        listeners.Remove(listener);
    }

    public void DigNotify(Block block)
    {
        foreach (BaseDigListener listener in listeners)
        {
            listener.OnNotify(block);
        }
    }
}

public abstract class BaseDigListener : MonoBehaviour
{
    private DiggingListenerSystem listener;

    protected virtual void Start()
    {
        listener = GetComponentInChildren<DiggingListenerSystem>();
        listener.Subscribe(this);
    }
    public abstract void OnNotify(Block block);
    void OnDestroy()
    {
        listener.UnSubscribe(this);
    }
}
