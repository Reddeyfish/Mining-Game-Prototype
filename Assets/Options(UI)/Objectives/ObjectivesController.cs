using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
public class ObjectivesController : MonoBehaviour {

    //mainly just handles saving/loading

    public GameObject objectivePrefab;

	// Use this for initialization
	void Start () {
        loadData();
	}

    void loadData()
    {
        if (PlayerPrefs.HasKey(PlayerPrefKeys.objectives))
        {
            int[] data = PlayerPrefsX.GetIntArray(PlayerPrefKeys.objectives);
            if (data.Length % 2 != 0)
                Debug.Log("Objective data formatted incorrectly");
            for (int i = 0; i < data.Length; i += 2)
            {
                //data[i] is the objective ID type; dat[i+1] is the 'save progress data' for that objective
                GameObject objectiveTransform = SimplePool.Spawn(objectivePrefab);
                Objective objectiveScript = theObjectiveData.IDToObjective[data[i]](objectiveTransform);
                objectiveScript.Initialize(data[i + 1]);
            }
        }
        //else we do nothing (tutorial is the one that starts the tutorial objectives)
    }

	// Update is called once per frame
	void Update () {
	
	}

    void OnDestroy()
    {

    }
}
