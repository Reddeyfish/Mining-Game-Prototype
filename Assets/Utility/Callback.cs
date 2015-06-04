using UnityEngine;
using System.Collections;

//basically the same as Invoke() and InvokeRepeating(), but no reflection!

//probably will need expansions/reworks when I actually use this in something

public class Callback : MonoBehaviour {

    public delegate void CallbackMethod();

    //basically Invoke
    public static IEnumerator FireAndForgetRoutine(CallbackMethod code, float time)
    {
        yield return new WaitForSeconds(time);
        code();
    }

    //wrapper so that we don't need to call StartCoroutine()
    public static void FireAndForget(MonoBehaviour callingScript, CallbackMethod code, float time)
    {
        callingScript.StartCoroutine(FireAndForgetRoutine(code, time));
    }
}
