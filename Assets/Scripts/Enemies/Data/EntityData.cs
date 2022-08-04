using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class EntityData : ScriptableObject
{
    public float wallCheckDistance = 0.2f;
    public float ledgeCheckDistance = 0.4f;

    public float minDetectionDistance = 3f;
    public float maxDetectionDistance = 4f;

    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;
}
