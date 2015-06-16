using UnityEngine;
using System.Collections;

// goes on the player

//should probably make some of this stuff public if we want to add upgrades for guffin-tracking

public class GuffinController : MonoBehaviour {

    int guffinNumber; //could just use playerprefs, but that's slower

    static float[] ranges = {
                               50,75,100,125,150,175,200,225,250
                           }; //should be constant
    const float checkFrequency = 30;
    const float checkInterval = 30;
    const float failureResetTime = 120;
    const float guffinSpawnRange = 20;
    const float guffinSpawnVariance = 5;
    const float maxGuffinTrackingRange = 30;

    public GameObject guffin;
    public GameObject guffinTracker;

	// Use this for initialization
	void Start () {
        guffinNumber = PlayerPrefs.HasKey(PlayerPrefKeys.guffin) ? PlayerPrefs.GetInt(PlayerPrefKeys.guffin) : 0;
        distanceTriggerWrapper();
	}

    void distanceTriggerWrapper()
    {
        StartCoroutine(distanceTrigger());
    }

    IEnumerator distanceTrigger()
    {
        while (this.transform.position.magnitude < ranges[guffinNumber])
        {
            yield return new WaitForSeconds(checkFrequency);
        }
        yield return new WaitForSeconds(checkInterval);

        Rigidbody2D ourRigidbody = GetComponent<Rigidbody2D>();

        if (this.transform.position.magnitude > ranges[guffinNumber] && ourRigidbody.velocity.sqrMagnitude != 0)
        {
            //spawn guffin
            Vector3 targetSpawn = this.transform.position + guffinSpawnRange * (Vector3)(ourRigidbody.velocity.normalized) + guffinSpawnVariance * (Vector3)(Random.insideUnitCircle);

            Transform spawnedGuffin = SimplePool.Spawn(guffin, targetSpawn).transform;



            Transform spawnedGuffinTracker = SimplePool.Spawn(guffinTracker).transform;
            spawnedGuffinTracker.SetParent(this.transform);
            spawnedGuffinTracker.localPosition = Vector3.zero;

            

            spawnedGuffinTracker.GetComponent<GuffinTracker>().Initialize(spawnedGuffin, maxGuffinTrackingRange, this);
        }
        else
        {
            //reset
            StartCoroutine(distanceTrigger());
        }
    }

    public void NotifyComplete(bool success)
    {
        if (success)
        {
            //guffinNumber++;
            PlayerPrefs.SetInt(PlayerPrefKeys.guffin, guffinNumber);
            Debug.Log("Guffin Acquired!");
        }
        else
        {
            Debug.Log("Guffin Lost");
            Callback.FireAndForget(distanceTriggerWrapper, failureResetTime, this);
        }
    }
}
