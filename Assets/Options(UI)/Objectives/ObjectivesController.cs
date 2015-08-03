using UnityEngine;
using System.Collections;
using System.Collections.Generic;
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
                AddObjective(data[i], data[i + 1]);
            }
        }
        //else we do nothing (tutorial is the one that starts the tutorial objectives)
    }

    public void AddObjective(int ID, int progress = 0)
    {
        Debug.Log(ID);
        Debug.Log(progress);
        GameObject newObjective = SimplePool.Spawn(objectivePrefab);
        newObjective.transform.SetParent(this.transform);
        newObjective.transform.localScale = Vector3.one;
        Objective objectiveScript = theObjectiveData.IDToObjective(newObjective, ID);
        Assert.AreEqual<int>(ID, objectiveScript.getID());
        objectiveScript.Initialize(progress);
    }

    void OnDestroy()
    {
        //save the data!

        int[] data = new int[transform.childCount * 2];
        int i = 0;
        foreach(Transform trans in transform)
        {
            if (trans.gameObject.activeSelf)//if it's disabled, then it's a pooled object
            {
                Objective objectiveScript = trans.GetComponent<Objective>();
                data[i] = objectiveScript.getID();
                i++;
                data[i] = objectiveScript.getProgress();
                i++;
            }
        }

        bool success = PlayerPrefsX.SetIntArray(PlayerPrefKeys.objectives, data);
        if (!success)
        {
            Debug.Log("Objective Save Failed!");
        }
        else
        {
            Debug.Log("Objective Save Complete!");
        }
    }
}
