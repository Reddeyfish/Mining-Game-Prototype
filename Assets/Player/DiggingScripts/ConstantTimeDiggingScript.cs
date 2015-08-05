using UnityEngine;
using System.Collections;

public class ConstantTimeDiggingScript : DefaultDiggingScript {

    protected override float getDigTimeMultiplier()
    {
        return (1 + 100 / digPower); //drill gets slower the farther away we are from the center
    }
}
