using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
public class BaseActiveAbility : MonoBehaviour {

    //goes on the player

    protected AbilityView view;
    
    protected bool _active;
    public bool Active { get { return _active; } }
    private float _cooldownRemaining = 0;
    public float CooldownRemaining
    {
        get { return _cooldownRemaining; }
        set {_cooldownRemaining = value > 0 ? value : 0; //someone is going to forget to check if it's greater than zero, so do the check for them
        }
    }
    public bool Ready { get { return _cooldownRemaining == 0; } }

    public void Initialize(Transform AbilityUI, int number)
    {
        _cooldownRemaining = 0;
        view = AbilityUI.GetComponent<AbilityView>();
        view.Initialize(number);
        view.Fill = 1;
        view.setReady(true);
    }

    //called when the ability's button is pressed
    public virtual Coroutine Activate()
    {
        //the ability stuff would go here
        if(!Ready) return null;
        return DoActivation();
    }

    //handles the activation and cooldown
    protected Coroutine DoActivation()
    {
        OnActivation();

        //cooldown
        _cooldownRemaining = getCooldown();
        view.setReady(this.Ready);
        return StartCoroutine(CoolDown());
    }

    protected virtual void OnActivation()
    {

    }

    IEnumerator CoolDown()
    {
        while (_cooldownRemaining > 0)
        {
            view.Fill = 1- (_cooldownRemaining / getCooldown());
            yield return new WaitForFixedUpdate();
            _cooldownRemaining -= Time.fixedDeltaTime;
        }
        _cooldownRemaining = 0;
        view.setReady(this.Ready);
        view.Fill = 1;
    }

    public virtual float getCooldown()
    {
        return 10;
    }
}