using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//class to notify all the various scripts when combos happen

//maybe I should subdivide this by level? (dictionary<int level, List<Listener>>)

public class ComboListenerSystem : MonoBehaviour
{

    private List<BaseComboListener> listeners;

    // Use this for initialization
    void Awake()
    {
        listeners = new List<BaseComboListener>();
    }

    public void Subscribe(BaseComboListener listener)
    {
        listeners.Add(listener);
    }

    public void UnSubscribe(BaseComboListener listener)
    {
        listeners.Remove(listener);
    }

    public void ComboNotify(int level)
    {
        foreach (BaseComboListener listener in listeners)
        {
            listener.OnNotify(level);
        }
    }
}

public abstract class BaseComboListener : MonoBehaviour
{
    private ComboListenerSystem listener;

    protected virtual void Start()
    {
        listener = GetComponent<ComboListenerSystem>();
        listener.Subscribe(this);
    }
    public abstract void OnNotify(int comboLevel);
    void OnDestroy()
    {
        listener.UnSubscribe(this);
    }
}
