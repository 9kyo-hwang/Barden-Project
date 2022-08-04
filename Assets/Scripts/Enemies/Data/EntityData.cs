using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class EntityData : ScriptableObject
{
    [Header("Move State")]
    public float movementSpeed = 3f;
    public float wallCheckDistance = 0.2f;
    public float ledgeCheckDistance = 0.4f;

    [Header("Idle State")]
    public float minIdleTime = 1f;
    public float maxIdleTime = 2f;

    [Header("Detect Player State")]
    public float longRangeActionTime = 1.5f;
    public float minDetectionDistance = 3f;
    public float maxDetectionDistance = 4f;

    [Header("Charge State")]
    public float chargeSpeed = 6f;
    public float chargeTime = 2f;

    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;

    [Header("Look For Player State")]
    public int maxTurnCount = 2;
    public float timeBetweenTurn = 0.75f;
}
