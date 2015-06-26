using UnityEngine;
using System.Collections;

[RequireComponent(typeof(LineRenderer))]
public class GrapplingReticle : BaseReticle {
    Transform player;
    LineRenderer line;
    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag(Tags.player).transform;
        line = GetComponent<LineRenderer>();
    }

    protected override void OnUpdate()
    {
        base.OnUpdate();
        line.SetPosition(0, player.position);
        Debug.Log(Format.mousePosInWorld());
        line.SetPosition(1, Format.mousePosInWorld());
    }
}
