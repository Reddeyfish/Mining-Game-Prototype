using UnityEngine;
using System.Collections;

public class VisualsFaceVelocityScript : MonoBehaviour {
    Rigidbody2D rigid;
    Transform thisTransform;
    private Quaternion _targetRotation = Quaternion.identity;
    public Quaternion Rotation
    {
        get
        {
            return _targetRotation;
        }
    }
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
        float _angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        _targetRotation = Quaternion.AngleAxis(_angle, Vector3.forward);
        thisTransform.rotation = Quaternion.RotateTowards(thisTransform.rotation, _targetRotation, rotateSpeed * Time.deltaTime);
    }
}
