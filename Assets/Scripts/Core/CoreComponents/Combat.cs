using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class Combat : CoreComponent, IDamageable
{
    public void Damage(float amount)
    {
        Debug.Log(core.transform.parent.name + " Damaged!");
    }
}
