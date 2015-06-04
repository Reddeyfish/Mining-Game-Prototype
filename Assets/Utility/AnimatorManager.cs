using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/**
 * AnimatorManager: This is a service class that acts as an interface for state setting and checking. State logic should be done
 * 					outside of this class.
 * 
 * TODO : using triggers for now, remember if state machine bugs start to happen check the triggers.
 */
public class AnimatorManager : MonoBehaviour
{

    public enum State
    {
        // actual states
        IDLE,
        DIGLEFT,
        DIGRIGHT,
        DIGDOWN,

        // grouped states
        DIGGING,
    }

    // put the animator state string names here for autocomplete and stuff

    // actual states
    private static int idleHash = Animator.StringToHash("Idle");
    private static int digHash = Animator.StringToHash("Digging");

    private static Dictionary<int, HashSet<State>> hashToStates = new Dictionary<int, HashSet<State>>
    {
        {idleHash, new  HashSet<State> {State.IDLE, } },
    };
    private Animator theStateMachine;

    // Use this for initialization
    void Start()
    {
        theStateMachine = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
    }

    // InState: check if animator is in a given state. Can check on multiple substates.
    public bool InState(State state)
    {
        return hashToStates[theStateMachine.GetCurrentAnimatorStateInfo(0).fullPathHash].Contains(state);
    }
}