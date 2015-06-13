using UnityEngine;
using System.Collections;

//should not be put on gameObjects


public class Pause : MonoBehaviour {

    private static float? currentTimeScale;

    private static bool paused = false;
	public static bool isPaused()
	{
		return paused;
	}

    private static bool frozen = false;
	public static bool isFrozen()
	{
		return frozen;
	}

	public static void pause(bool toPause = true)
    {
        if (toPause && !paused)
        {
            if (!frozen)
                currentTimeScale = Time.timeScale;
            Time.timeScale = 0;
            paused = true;
        }
        else if (!toPause && paused)
        {
            Time.timeScale = currentTimeScale ?? 1.0f;
            paused = false;
        }

        // maybe warnings if nothing will happen?
    }
	
	public void GUIPause(bool toPause = true) //instanced (not static) wrapper so that UI event systems can call it
	{
		pause (toPause);
	}

    //inverse of Pause, basically.
    public static void unPause(bool toUnPause = true)
    {
        if (!toUnPause && !paused)
        {
            if (!frozen)
                currentTimeScale = Time.timeScale;
            Time.timeScale = 0;
            paused = true;
        }
        else if (toUnPause && paused)
        {
            Time.timeScale = currentTimeScale ?? 1.0f;
            paused = false;
        }
    }

	public void GUIUnPause(bool toUnPause = true) //instanced (not static) wrapper so that UI event systems can call it
	{
		unPause (toUnPause);
	}

    //freeze time for a small amount in order to emphasize an impact or effect
    public static IEnumerator FreezeRoutine(float durationRealSeconds, float timeScale = 0f)
    {
        if(!paused)
            currentTimeScale = Time.timeScale;
        Time.timeScale = timeScale;
        float pauseEndTime = Time.realtimeSinceStartup + durationRealSeconds;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            yield return 0;
        }
        frozen = false;
        Time.timeScale = currentTimeScale ?? 1.0f;
    }

	public static Coroutine Freeze(MonoBehaviour callingScript, float durationRealSeconds, float timeScale = 0f) //wrapper for FreezeRoutine so that we don't need to call StartCoroutine()
	{
        Coroutine result = null;
        if (!frozen)
            result = callingScript.StartCoroutine(FreezeRoutine(durationRealSeconds, timeScale));
        frozen = true;
        return result;
	}

    public static IEnumerator SlowRoutine(float durationRealSeconds, float startTimeScale = 0.5f)
    {
        if (!paused)
            currentTimeScale = Time.timeScale;
        Time.timeScale = startTimeScale;
        float pauseEndTime = Time.realtimeSinceStartup + durationRealSeconds;
        float pauseStartTime = Time.realtimeSinceStartup;
        float endTimeScale = currentTimeScale ?? 1.0f;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            Time.timeScale = Mathf.Lerp(startTimeScale, endTimeScale, Mathf.Pow((Time.realtimeSinceStartup - pauseStartTime) / durationRealSeconds, 3));
            yield return 0;
        }
        frozen = false;
        Time.timeScale = endTimeScale;
    }

    public static Coroutine Slow(MonoBehaviour callingScript, float durationRealSeconds, float startTimeScale = 0.5f) //wrapper for SlowRoutine so that we don't need to call StartCoroutine()
    {
        Coroutine result = null;
        if (!frozen)
            result = callingScript.StartCoroutine(SlowRoutine(durationRealSeconds, startTimeScale));
        frozen = true;
        return result;
    }
}
