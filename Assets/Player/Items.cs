using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Items : MonoBehaviour {

}

public class Stat
{
    public float Base = 0;
    public float multiplier = 1;

    public static implicit operator float(Stat stat)
    {
        return stat.Base * stat.multiplier;
    }
}