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

    void Awake()
    {
        digScript = GetComponent<IDigScript>();
        digScript.SetControlScript(this);

        movementScript = GetComponent<IMovementScript>();
        movementScript.SetControlScript(this);
    }

	void Start () {
        StartCoroutine("Idle");
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
                break; //control flow moves to Digging
            }

            //vertical

            input = Input.GetAxis(Axis.vertical);
            movementScript.DoMovement(input * transform.up, XAxis: false);
            if (digScript.DoDigging(input * transform.up))
            {
                break; //control flow moves to Digging
            }
            yield return new WaitForFixedUpdate();
        }
    }

    public void StartIdle() // to be called by extensions of dig 
    {
        StartCoroutine("Idle");
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