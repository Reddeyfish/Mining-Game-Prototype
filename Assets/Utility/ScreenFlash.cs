using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ScreenFlash : MonoBehaviour {
    Image image;
    CanvasGroup group;
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
    }

    public void Flash(float durationRealSeconds, float startAlpha = 0.5f)
    {
        StopAllCoroutines();
        StartCoroutine(FlashRoutine(durationRealSeconds, startAlpha));
    }
}
