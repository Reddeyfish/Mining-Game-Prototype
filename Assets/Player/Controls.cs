using UnityEngine;
using System.Collections;


//Derek Edrich
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(CircleCollider2D))]
[RequireComponent(typeof(IDigScript))]
[RequireComponent(typeof(IMovementScript))]
public class Controls : MonoBehaviour {
    private IDigScript digScript;
    private IMovementScript movementScript;

    IEnumerator currentCoroutine; //current control coroutine, or null if control is in a delegate
    IEnumerator queuedCoroutine; //queued routine. May expand into a list if we need more than one in a queue
    void Awake()
    {
        digScript = GetComponent<IDigScript>();
        digScript.SetControlScript(this);

        movementScript = GetComponent<IMovementScript>();
        movementScript.SetControlScript(this);
    }

	void Start () {
        currentCoroutine = Idle();
        StartCoroutine(currentCoroutine);
	}

    IEnumerator Idle()
    {
        
        while (true)
        {
            //horizontal

            float input = Input.GetAxis(Axis.horizontal);
            movementScript.DoMovement(input * transform.right, XAxis: true);
            if (digScript.DoDigging(input * transform.right))
            {
                currentCoroutine = null;
                break; //control flow moves to Digging
            }

            //vertical

            input = Input.GetAxis(Axis.vertical);
            movementScript.DoMovement(input * transform.up, XAxis: false);
            if (digScript.DoDigging(input * transform.up))
            {
                currentCoroutine = null;
                break; //control flow moves to Digging
            }
            yield return 0;
        }
    }

    public void StartIdle() // to be called by extensions of dig 
    {

        if (queuedCoroutine == null) //no coroutine was queued
        {
            currentCoroutine = Idle();
            StartCoroutine(currentCoroutine);
        }
        else
        {
            StartCoroutine(DelegateCoroutine(queuedCoroutine));
            queuedCoroutine = null; //clear the queue, since we're starting the queued routine
        }
        
    }

    //queues an (unstarted) coroutine for when control is within control
    public void QueueCoroutine(IEnumerator next)
    {
        if (currentCoroutine != null) //if we have control
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
            StartCoroutine(DelegateCoroutine(next));
        }
        else
        {
            //we don't have control; queue the coroutine until we do
            queuedCoroutine = next;
        }
    }

    IEnumerator DelegateCoroutine(IEnumerator delegat)
    {
        yield return StartCoroutine(delegat); //wait until the delegate coroutine is done
        StartIdle();
    }
}

public interface IDigScript
{
    void SetControlScript(Controls control);
    bool DoDigging(Vector2 direction);
}

public interface IMovementScript
{
    void SetControlScript(Controls control);
    void DoMovement(Vector2 direction, bool XAxis);
}