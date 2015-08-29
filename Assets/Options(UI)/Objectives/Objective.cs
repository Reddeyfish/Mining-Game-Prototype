using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public abstract class Objective : MonoBehaviour {

    //the main meat of an objective
    const float fadeTime = 0.5f;
    Text text;
    CanvasGroup group;
    static Color completeColor = new Color(0.4f, 1f, 0,2f); //light green, constant
    static Color normalColor = new Color(0.8f, 0.8f, 0.8f);  //almost white, constant
    protected string screenText
    {
        set 
        {
            text.text = "‣ " + value;
        }
        get
        {
            return text.text;
        }
    }

	// Use this for initialization
	protected virtual void Awake () {
        group = transform.GetComponentInChildren<CanvasGroup>();
        text = transform.GetComponentInChildren<Text>();
        screenText = getText();

        StartCoroutine(fadeIn());
	}

    protected abstract string getText();

    IEnumerator fadeIn()
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

	// Update is called once per frame
    public abstract void Initialize(int progress);
    public void completeObjective()
    {
        StartCoroutine(completeObjectiveRoutine()); //might need a flag to ensure this only gets started once
    }

    private IEnumerator completeObjectiveRoutine()
    {
        onComplete();
        
        //fade out
        text.color = completeColor;
        float time = fadeTime;
        while (time > 0)
        {
            group.alpha = time / fadeTime;
            yield return new WaitForFixedUpdate();
            time -= Time.fixedDeltaTime;
        }
        group.alpha = 0;
        text.color = normalColor;
        spawnNextObjectives();
        destroySelf();
    }

    protected void destroySelf()
    {
        Destroy(this); //reset this to be a generic objective
        SimplePool.Despawn(this.gameObject);
    }
    protected virtual void onComplete()
    {
    }

    protected abstract void spawnNextObjectives();

    public abstract int getID();

    //returns an int representing this objective's progress state. The int will be fed into Initialize in order to load the objective again
    public virtual int getProgress()
    {
        return 0;
    }
}
