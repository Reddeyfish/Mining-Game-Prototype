using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))]
public class DefaultMovementScript : MonoBehaviour , IMovementScript
{
    [Header("Movement Speed Values")]
    public float powerConstant = 25.0f;
    public float minSpeed = 5.0f;
    public float minAccel = 15.0f;
    
    //private Controls control; //don't need it for this version of movement
    private Rigidbody2D rigid;

    public void SetControlScript(Controls control)
    {
        //this.control = control; //don't need it for this version of movement
    }

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void DoMovement(Vector2 direction, bool XAxis)
    {
        if (direction.sqrMagnitude != 0 && Vector3.Dot(rigid.velocity, direction) >= 0)
        {
            if (Vector3.Project(rigid.velocity, direction).magnitude < minSpeed)
            {
                rigid.velocity = rigid.velocity + minAccel * Time.deltaTime * direction.normalized;
            }
            else
            {
                rigid.AddForce(direction * powerConstant * Time.deltaTime / rigid.velocity.magnitude, ForceMode2D.Impulse);
            }
        }
        else
        {
            if (XAxis)
            {
                rigid.velocity = new Vector2(0, rigid.velocity.y);
            }
            else
                rigid.velocity = new Vector2(rigid.velocity.x, 0);
        }
    }
}
