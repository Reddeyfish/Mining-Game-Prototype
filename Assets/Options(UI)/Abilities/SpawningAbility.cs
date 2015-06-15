using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
public class SpawningAbility : BaseActiveAbility {

    GameObject spawnPrefab;
    private int _id;
    public int ID
    {
        get { return _id; }
        set { 
            _id = value;
            spawnPrefab = Resources.Load<GameObject>(theUpgradeData.IDToUpgrade[value].ComponentName + "/" + theUpgradeData.IDToUpgrade[value].ComponentName + " Prefab");
#if UNITY_EDITOR
            if (spawnPrefab == null)
            {
                Debug.Log("Missing ability prefab at: " + theUpgradeData.IDToUpgrade[value].ComponentName + "/" + theUpgradeData.IDToUpgrade[value].ComponentName + " Prefab");
                Assert.IsNotNull(spawnPrefab);
            }
#endif
              }
    }

    protected override void OnActivation()
    {
        Transform spawnedPrefab = SimplePool.Spawn(spawnPrefab).transform;
        spawnedPrefab.SetParent(this.transform);
        spawnedPrefab.localPosition = Vector3.zero;
    }

}
