using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public abstract class Objective : MonoBehaviour {

    //the main meat of an objective

    Text text;

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
	protected virtual void Start () {
        text = transform.GetComponentInChildren<Text>();
        screenText = getText();
	}

    protected abstract string getText();

	// Update is called once per frame
    public abstract void Initialize(int progress);
    public void completeObjective()
    {
        spawnNextObjectives();
        destroySelf();
    }

    protected void destroySelf()
    {
        Destroy(this); //reset this to be a generic objective
        SimplePool.Despawn(this.gameObject);
    }

    protected abstract void spawnNextObjectives();

    public abstract int getID();

    //returns an int representing this objective's progress state. The int will be fed into Initialize in order to load the objective again
    public virtual int getProgress()
    {
        return 0;
    }
}
