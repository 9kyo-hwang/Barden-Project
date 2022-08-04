using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityData : ScriptableObject
{
    public float wallCheckDistance;
    public float ledgeCheckDistance;

    public LayerMask whatIsGround;
}
