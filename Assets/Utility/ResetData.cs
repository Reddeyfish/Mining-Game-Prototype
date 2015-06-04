using UnityEngine;
using System.Collections;
[ExecuteInEditMode]
public class ResetData : MonoBehaviour {
    public void Start()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("Data Reset!");
    }
}
