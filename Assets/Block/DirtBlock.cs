using UnityEngine;
using System.Collections;

public class DirtBlock : SolidBlock {
    public GameObject obliterateEffect;

    public override void Obliterate()
    {
        SimplePool.Spawn(obliterateEffect, this.transform.position);
        base.Obliterate();
    }
}
