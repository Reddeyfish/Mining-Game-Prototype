using UnityEngine;
using System.Collections;

public class VisualsFaceVelocityScript : MonoBehaviour {
    Rigidbody2D rigid;
    Transform thisTransform;

    private float _angle;
    public float Angle { get { return _angle; } }

    public float rotateSpeed = 360;
	// Use this for initialization
	void Start () {
        rigid = GetComponentInParent<Rigidbody2D>();
        thisTransform = this.transform;
	}
	
	// Update is called once per frame
    public void FixedUpdate()
    {
        var dir = rigid.velocity;
        if (dir.sqrMagnitude == 0) //if there is no movement, we shouldn't be rotating
        {
            return;
        }
        _angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        var q = Quaternion.AngleAxis(_angle, Vector3.forward);
        thisTransform.rotation = Quaternion.RotateTowards(thisTransform.rotation, q, rotateSpeed * Time.deltaTime);
    }
}
