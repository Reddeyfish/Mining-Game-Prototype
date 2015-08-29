using UnityEngine;
using System.Collections;

public class TriggerObservable : MonoBehaviour {

    EnergyTutorialObjective objective;

    public void Instantiate(EnergyTutorialObjective listener)
    {
        objective = listener;
    }

	// Use this for initialization
    void OnTriggerEnter2D(Collider2D other)
    {
        objective.Notify(other);
        Destroy(this); //job is done
    }
}
