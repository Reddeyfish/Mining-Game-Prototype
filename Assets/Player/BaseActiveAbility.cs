using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
public class BaseActiveAbility : MonoBehaviour {

    //goes on the player
    AbilityView view;
    
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

    public GameObject AbilityUIPrefab;
    public int testPublicField;
    public Sprite testPublicSprite;

    public void Initialize(Transform AbilityUI, int number)
    {
        view = AbilityUI.GetComponent<AbilityView>();
        view.Initialize(number);
    }

    public Coroutine Activate()
    {
        //the ability stuff would go here
        if(!Ready) return null;
        OnActivation();

        //cooldown
        _cooldownRemaining = getCooldown();
        view.Ready = Ready;
        return StartCoroutine(CoolDown());
    }

    protected virtual void OnActivation()
    {

    }

    IEnumerator CoolDown()
    {
        while (_cooldownRemaining > 0)
        {
            yield return new WaitForFixedUpdate();
            _cooldownRemaining -= Time.fixedDeltaTime;
        }
        _cooldownRemaining = 0;
        view.Ready = Ready;
    }

    public virtual float getCooldown()
    {
        return 10;
    }
}