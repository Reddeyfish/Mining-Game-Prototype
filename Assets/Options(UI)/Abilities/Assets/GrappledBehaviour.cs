using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody2D))] //it'll spawn a rigidbody 2d when this script is added
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(SpringJoint2D))]
    //this script is spawned onto blocks when they are grappled

    //this can't be done by adding a poolable child object to the block because of the rigidbody and using the parent block's collider

public class GrappledBehaviour : MonoBehaviour {
    Rigidbody2D rigid;
    LineRenderer line;
    SpringJoint2D spring;
    Transform thisTransform;
    Vector2 grappledPoint; //location of the grappling point in local space

    const float lerpToPointerTime = 1f;
    static Color lineColor = new Color(100f / 255f, 100f / 255f, 100f / 255f); //constant

	// Use this for initialization
	void Awake () {
        rigid = GetComponent<Rigidbody2D>();
        rigid.gravityScale = 0;
        rigid.drag = 0f;
        rigid.angularDrag = 0.2f;

        line = GetComponent<LineRenderer>();
        line.SetWidth(0.1f, 0.1f);
        line.SetColors(lineColor, lineColor);

        spring = GetComponent<SpringJoint2D>();
        gameObject.isStatic = false;
        spring.enableCollision = true;
        spring.distance = 0;
        spring.dampingRatio = 0.5f;

        thisTransform = this.transform;
        StartCoroutine(DrawGrapplingLine());

        GetComponent<Block>().UpdateMap(); //the block is going to be destroyed anyway, so might as well update it now.
	}

    IEnumerator DrawGrapplingLine()
    {
        float time = 0;

        Transform player = GameObject.FindGameObjectWithTag(Tags.player).transform;

        while (time < lerpToPointerTime)
        {
            //the player end of the grappling line is being lerped from the player to the mouse pointer
            line.SetPosition(0, thisTransform.TransformPoint(grappledPoint)); //the block end of the grappling line
            line.SetPosition(1, Vector2.Lerp(player.position, Format.mousePosInWorld(), 1 - Mathf.Pow(time / lerpToPointerTime - 1, 2))); //player end of grappling line
            //1 - (x-1)^2
            yield return 0;
            time += Time.deltaTime;
        }

        //now just draw from the block to the mouse
        while (true)
        {
            line.SetPosition(0, thisTransform.TransformPoint(grappledPoint)); //the block end of the grappling line
            line.SetPosition(1, Format.mousePosInWorld()); //player end of grappling line
            yield return 0;
        }
    }

    void FixedUpdate()
    {
        spring.connectedAnchor = Format.mousePosInWorld();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        Debug.Log(other.relativeVelocity);
    }

    public void Instantiate(Vector2 collisionPoint, Material spriteMat) //set the grappled point; collisionPoint is in world space
    {
        //addComponent scripts can't have serialized references to assets, so as a workaround I'm passing in the material from the grappling object.
        line.material = spriteMat;

        spring.anchor = grappledPoint = thisTransform.InverseTransformPoint(collisionPoint);
    }

    void OnDisable()
    {
        //when the object is disabled, it's despawned for pooling

        //reset the object to a normal block
        Destroy(this);
       
        Destroy(line);
        gameObject.isStatic = true;
        Destroy(spring);
        Destroy(rigid);
        
    }
}
