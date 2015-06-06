using UnityEngine;
using System.Collections;

public class DisabledAwake : MonoBehaviour {

    void Start()
    {
        foreach (IDisabledAwake sleeper in GetComponentsInChildren<IDisabledAwake>(includeInactive : true))
        {
            sleeper.Awaken();
        }
    }
}

public interface IDisabledAwake
{
    void Awaken();
}
