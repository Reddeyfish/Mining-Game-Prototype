using UnityEngine;
using System.Collections;

//basically the same as Invoke() and InvokeRepeating(), but no reflection!

//probably will need expansions/reworks when I actually use this in something


public static class Callback {
    public delegate void CallbackMethod();

    //basically Invoke
    public static IEnumerator FireAndForgetRoutine(CallbackMethod code, float time)
    {
        yield return new WaitForSeconds(time);
        code();
    }

    //wrapper so that we don't need to call StartCoroutine()
    public static void FireAndForget(this CallbackMethod code, float time, MonoBehaviour callingScript)
    {
        callingScript.StartCoroutine(FireAndForgetRoutine(code, time));
    }

    public static IEnumerator FireForFixedUpdateRoutine(CallbackMethod code)
    {
        yield return new WaitForFixedUpdate();
        code();
    }
    //wrapper so that we don't need to call StartCoroutine()
    public static void FireForFixedUpdate(this CallbackMethod code, MonoBehaviour callingScript)
    {
        callingScript.StartCoroutine(FireForFixedUpdateRoutine(code));
    }

    public static IEnumerator FireForNextFrameRoutine(CallbackMethod code)
    {
        yield return 0;
        code();
    }

    public static void FireForNextFrame(this CallbackMethod code, MonoBehaviour callingScript)
    {
        callingScript.StartCoroutine(FireForNextFrameRoutine(code));
    }
}
