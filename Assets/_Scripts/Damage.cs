using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Damage
{

    public float amount;
    public GameObject source;
    public float kickMult;

    public Damage(float amt, GameObject src, float kickM = 1)
    {
        amount = amt;
        source = src;
        kickMult = kickM;
    }
}
