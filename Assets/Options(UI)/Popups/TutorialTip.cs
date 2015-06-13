using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialTip : MonoBehaviour {

    Text text;
    CanvasGroup group;
    IEnumerator fade;
    TutorialTipType currentTip;

    private const string UIblue = "0083C8FF";
    private const float fadeTime = 0.25f;

	// Use this for initialization
	void Start () {
        text = transform.Find("Text").GetComponent<Text>();
        group = GetComponent<CanvasGroup>();
	}

    public Coroutine SetTip(TutorialTipType tip)
    {
        return StartCoroutine(SetTipRoutine(tip));
    }

    IEnumerator SetTipRoutine(TutorialTipType tip)
    {
        currentTip = tip;
        if (group.alpha != 0)
            yield return SetVisible(false);
        text.text = TipToText(tip);
        SetVisible(true);
    }

    public Coroutine SetTimedTip(TutorialTipType tip, float time)
    {
        SetTip(tip);
        return StartCoroutine(SetTimedEndTip(tip, time));
    }

    public Coroutine EndTip(TutorialTipType tip)
    {
        Coroutine result = null;
        if (currentTip == tip)
            result = SetVisible(false);
        return result;
    }

    IEnumerator SetTimedEndTip(TutorialTipType tip, float time)
    {
        yield return new WaitForSeconds(time);
        EndTip(tip);
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
        fade = null;
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
        fade = null;
    }

    public Coroutine SetVisible(bool visible)
    {
        if (fade != null)
            StopCoroutine(fade);
        fade = visible ? fadeInRoutine() : fadeOutRoutine();
        return StartCoroutine(fade);
    }

    string TipToText(TutorialTipType tip)
    {
        switch (tip)
        {
            case TutorialTipType.LOWENERGY:
                return "You are low on <color=yellow>energy</color>. Return to base and refill your energy or you will die.";
            case TutorialTipType.ENERGYDEATH:
                return "You ran out of <color=yellow>energy</color> and died.";
            default:
                return "";
        }
    }
}

public enum TutorialTipType
{
    LOWENERGY = 0,
    ENERGYDEATH = 1,
}