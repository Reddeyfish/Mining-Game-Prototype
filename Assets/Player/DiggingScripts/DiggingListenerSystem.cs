using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//class to notify all the various scripts when something is dug

public class DiggingListenerSystem : MonoBehaviour {

    private List<IDigListener> digListeners;
    private List<IUndiggableListener> undiggableListeners;
	// Use this for initialization
	void Awake () {
        digListeners = new List<IDigListener>();
        undiggableListeners = new List<IUndiggableListener>();
	}

    public void Subscribe(IDigListener listener)
    {
        digListeners.Add(listener);
    }

    public void UnSubscribe(IDigListener listener)
    {
        digListeners.Remove(listener);
    }

    public void DigNotify(Block block)
    {
        foreach (IDigListener listener in digListeners)
        {
            listener.OnNotify(block);
        }
    }

    public void Subscribe(IUndiggableListener listener)
    {
        undiggableListeners.Add(listener);
    }

    public void UnSubscribe(IUndiggableListener listener)
    {
        undiggableListeners.Remove(listener);
    }

    public void UndiggableNotify(Block block)
    {
        foreach (IUndiggableListener listener in undiggableListeners)
        {
            listener.OnNotifyUndiggable(block);
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

public interface IUndiggableListener
{
    void OnNotifyUndiggable(Block block);
    void OnDestroy();
}

public abstract class BaseUndiggableListener : MonoBehaviour, IUndiggableListener
{
    private DiggingListenerSystem listener;

    protected virtual void Start()
    {
        listener = GetComponentInChildren<DiggingListenerSystem>();
        listener.Subscribe(this);
    }
    public abstract void OnNotifyUndiggable(Block block);
    public void OnDestroy()
    {
        listener.UnSubscribe(this);
    }
}