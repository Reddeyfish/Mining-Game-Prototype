using UnityEngine;
using System.Collections;

public abstract class ResettingObjective : Objective {

    public override void Initialize(int progress)
    {
        if (progress == 1)
            destroySelf(); //this tutorial does not save
    }

    public override int getProgress()
    {
        return 1; //this tutorial does not save
    }
}
