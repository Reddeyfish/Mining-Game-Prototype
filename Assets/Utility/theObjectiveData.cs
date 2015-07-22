using UnityEngine;
using System.Collections;

public delegate Objective AddObjectiveComponentDelegate(GameObject objectivePrefab);
public class theObjectiveData : MonoBehaviour {

    public static AddObjectiveComponentDelegate[] IDToObjective = new AddObjectiveComponentDelegate[]
    {
        //0
        delegate(GameObject objectivePrefab) { return objectivePrefab.AddComponent<Objective>(); }, 
    };
}
