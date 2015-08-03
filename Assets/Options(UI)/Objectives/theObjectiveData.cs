using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public delegate Objective AddObjectiveComponentDelegate(GameObject objectivePrefab);
public class theObjectiveData : MonoBehaviour {
    
    private static AddObjectiveComponentDelegate[] IDToObjectiveData = new AddObjectiveComponentDelegate[] //static because const hates delegates; is constant
    {
        //0
        delegate(GameObject objectivePrefab) { return objectivePrefab.AddComponent<Objective>(); }, 
        //1
        delegate(GameObject objectivePrefab) { return objectivePrefab.AddComponent<MovementTutorialObjective>(); }, 
        //2
        delegate(GameObject objectivePrefab) { return objectivePrefab.AddComponent<DiggingTutorialObjective>(); }, 
        //3
        delegate(GameObject objectivePrefab) { return objectivePrefab.AddComponent<OreBlockTutorialObjectve>(); }, 
    };

    public static Objective IDToObjective(GameObject target, int ID)
    {
        Assert.IsTrue(ID > 0 && ID < IDToObjectiveData.Length); //assert that ID is in bounds; greater than zero because zero is the abstract objective
        Objective result = IDToObjectiveData[ID](target);
        Assert.Equals(ID, result.getID()); //check that the IDs are set up correctly
        return result;
    }
}
