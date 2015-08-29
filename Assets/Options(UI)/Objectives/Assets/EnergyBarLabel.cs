using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnergyBarLabel : MonoBehaviour {

    CanvasGroup group;
    private const float fadeTime = 0.25f;

    void Awake()
    {
        group = GetComponent<CanvasGroup>();
        StartCoroutine(fadeInRoutine());
    }

    public void destroy()
    {
        StartCoroutine(fadeOutRoutine());

    }

    IEnumerator fadeInRoutine()
    {
        float time = 0;
        while (time < fadeTime)
        {
            group.alpha = time / fadeTime;
            yield return new WaitForFixedUpdate();
            time += Time.fixedDeltaTime;
        }
        group.alpha = 1;
    }

    IEnumerator fadeOutRoutine()
    {
        float time = fadeTime;
        while (time > 0)
        {
            group.alpha = time / fadeTime;
            yield return new WaitForFixedUpdate();
            time -= Time.fixedDeltaTime;
        }
        group.alpha = 0;
        Destroy(this.gameObject);
    }
}
