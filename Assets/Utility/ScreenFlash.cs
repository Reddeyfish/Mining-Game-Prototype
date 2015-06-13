using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ScreenFlash : MonoBehaviour {
    Image image;
    CanvasGroup group;
    IEnumerator flash;
    void Awake()
    {
        image = GetComponent<Image>();
        group = GetComponent<CanvasGroup>();
    }
    private IEnumerator FlashRoutine(float durationRealSeconds, float startAlpha = 0.5f)
    {
        //this effect is likely to be called alongside time-warp effects, so we'll use real time
        float pauseEndTime = Time.realtimeSinceStartup + durationRealSeconds;
        float pauseStartTime = Time.realtimeSinceStartup;
        while (Time.realtimeSinceStartup < pauseEndTime)
        {
            group.alpha = (Mathf.Lerp(startAlpha, 0, (Time.realtimeSinceStartup - pauseStartTime) / durationRealSeconds));
            yield return 0;
        }
        group.alpha = 0;
        flash = null;
    }

    public Coroutine Flash(float durationRealSeconds, float startAlpha = 0.5f)
    {
        if(flash != null)
            StopCoroutine(flash);
        flash = FlashRoutine(durationRealSeconds, startAlpha);
        return StartCoroutine(flash);
    }

    IEnumerator FadeRoutine(float duration)
    {
        float time = 0;
        while (time < duration - 0.25f)
        {
            group.alpha = time / (duration - 0.25f);
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime;
        }
        group.alpha = 1;
        yield return new WaitForSeconds(0.25f);
        group.alpha = 0;
    }

    public Coroutine Fade(float duration)
    {
        return StartCoroutine(FadeRoutine(duration));
    }
}
