using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialTip : MonoBehaviour {

    Text text;
    CanvasGroup group;
    IEnumerator fade;
    string currentTip; //the text doesn't always match the tip, because of the fade out/fade in needed to change tips

    private const float fadeTime = 0.25f;

	// Use this for initialization
	void Awake () {
        text = transform.Find("Text").GetComponent<Text>();
        group = GetComponent<CanvasGroup>();
	}

    public Coroutine SetTip(string tip)
    {
        if (currentTip == tip && group.alpha != 0f)
            return null;
        return StartCoroutine(SetTipRoutine(tip));
    }

    IEnumerator SetTipRoutine(string tip)
    {
        currentTip = tip;
        if (group.alpha != 0)
            yield return SetVisible(false);
        text.text = tip;
        SetVisible(true);
    }

    public Coroutine SetTimedTip(string tip, float time)
    {
        SetTip(tip);
        return StartCoroutine(SetTimedEndTip(tip, time));
    }

    public Coroutine EndTip(string tip)
    {
        Coroutine result = null;
        if (currentTip == tip)
            result = SetVisible(false);
        return result;
    }

    IEnumerator SetTimedEndTip(string tip, float time)
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
}
/*
 * switch (tip)
        {
            case TutorialTipType.LOWENERGY:
                return "You are low on <color=yellow>energy</color>. Return to base and refill your energy or you will die.";
            case TutorialTipType.ENERGYDEATH:
                return "You ran out of <color=yellow>energy</color> and died.";
            case TutorialTipType.MOVEMENT:
                return "Welcome to Chromatose! Use <color=yellow>WASD</color> or the <color=yellow>Arrow Keys</color> to move, and press the <color=yellow>Space Bar</color> to activate your drill.";
            case TutorialTipType.BOULDER:
                return "<color=cyan>Boulders</color> must be cracked open with an <color=yellow>explosion</color> before they can be mined.";
            case TutorialTipType.GUFFIN:
                return "You've detected a <color=cyan>Guffin</color>. Use the pings to track it down. Faster pinging means you're getting closer.";
            case TutorialTipType.CAPSULE:
                return "This is a <color=cyan>Capsule</color>. When activated, it transports you back to base. The transport requires some energy, but it's always better (and faster) than just flying back.";
            default:
                return "";
        }
*/