using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
public class ObjectivesController : MonoBehaviour, ISaveListener {

    //mainly just handles saving/loading

    SaveObservable observable;

    public GameObject objectivePrefab;

	// Use this for initialization
	void Start () {
        observable = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<SaveObservable>();
        observable.Subscribe(this);
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
                AddObjective(data[i], data[i + 1]);
            }
        }
        //else we do nothing (tutorial is the one that starts the tutorial objectives)
    }

    public void AddObjective(int ID, int progress = 0)
    {
        GameObject newObjective = SimplePool.Spawn(objectivePrefab);
        newObjective.transform.SetParent(this.transform, false);
        Objective objectiveScript = theObjectiveData.IDToObjective(newObjective, ID);
        Assert.AreEqual<int>(ID, objectiveScript.getID());
        objectiveScript.Initialize(progress);
    }

    public void NotifySave()
    {
        List<int> data = new List<int>();

        foreach (Transform trans in transform)
        {
            if (trans.gameObject.activeSelf) //inactive ones are pooled
            {
                Objective objectiveScript = trans.GetComponent<Objective>();
                data.Add(objectiveScript.getID());
                data.Add(objectiveScript.getProgress());
            }
        }

        bool success = PlayerPrefsX.SetIntArray(PlayerPrefKeys.objectives, data.ToArray());
        if (!success)
        {
            Debug.Log("Objective Save Failed!");
        }
        else
        {
            Debug.Log("Objective Save Complete!");
        }
    }

    public void OnDestroy()
    {
        observable.UnSubscribe(this);
    }
}
