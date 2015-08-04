using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//class to notify all the various scripts when something is dug

[DisallowMultipleComponent]
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
            listener.OnNotifyDig(block);
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

    //note: the listeners can't remove themselves immediately; it'll cause a problem with the for-loop. use Callback.FireForNextFrame on the removal if necessary
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
    void OnNotifyDig(Block block);
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
    public abstract void OnNotifyDig(Block block);
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