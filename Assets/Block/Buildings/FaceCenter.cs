using UnityEngine;
using System.Collections;

public class FaceCenter : MonoBehaviour {

	// Use this for initialization
	void OnEnable()
    {
        Vector2 dir = transform.position;
        float _angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(_angle, Vector3.forward);
	}
}
