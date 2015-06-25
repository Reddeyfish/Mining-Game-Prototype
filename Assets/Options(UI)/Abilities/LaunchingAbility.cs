using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;

public class LaunchingAbility : BaseActiveAbility
{
    GameObject spawnPrefab;
    private int _id;
    private IEnumerator targetingRoutine;
    public int ID
    {
        get { return _id; }
        set
        {
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

    public override Coroutine Activate()
    {
        if (!Ready) return null; //if on cooldown

        if (targetingRoutine == null)
        {
            ((LaunchAbilityView)view).setIsTargeting(true);
            targetingRoutine = TargetingRoutine(); //start targeting
            return StartCoroutine(targetingRoutine);
        }
        else
        {
            ((LaunchAbilityView)view).setIsTargeting(false);
            //else stop targeting, go back to being simply ready
            StopCoroutine(targetingRoutine);
            targetingRoutine = null;

            return null;
        }
    }

    IEnumerator TargetingRoutine()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0)) //if left click, then actually activate the ability
            {
                Activate(); // reset ourselves to not targeting
                DoActivation();
                break;
            }
            else if(Input.GetMouseButtonDown(1)) //if right click, cancel the targeting
            {
                Activate();// reset ourselves to not targeting
                break;
            }
            yield return null;
        }
    }

    protected override void OnActivation()
    {
        Debug.Log("Activated!");
        /*
        Transform spawnedPrefab = SimplePool.Spawn(spawnPrefab).transform;
        spawnedPrefab.SetParent(this.transform);
        spawnedPrefab.localPosition = Vector3.zero;
      * */
    }

}