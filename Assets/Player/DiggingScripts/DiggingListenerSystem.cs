using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//class to notify all the various scripts when something is dug

public class DiggingListenerSystem : MonoBehaviour {

    private List<IDigListener> listeners;

	// Use this for initialization
	void Awake () {
        listeners = new List<IDigListener>();
	}

    public void Subscribe(IDigListener listener)
    {
        listeners.Add(listener);
    }

    public void UnSubscribe(IDigListener listener)
    {
        listeners.Remove(listener);
    }

    public void DigNotify(Block block)
    {
        foreach (IDigListener listener in listeners)
        {
            listener.OnNotify(block);
        }
    }
}

public interface IDigListener
{
    void OnNotify(Block block);
    void OnDestroy();
}

public abstract class BaseDigListener : MonoBehaviour, IDigListener
{
    private DiggingListenerSystem listener;

    protected virtual void Start()
    {
        listener = GetComponentInChildren<DiggingListenerSystem>();
        listener.Subscribe(this);
    }
    public abstract void OnNotify(Block block);
    public void OnDestroy()
    {
        listener.UnSubscribe(this);
    }
}
