using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "newEntityData", menuName = "Data/Entity Data/Base Data")]
public class EntityData : ScriptableObject
{
    [Header("Layer Mask")]
    public LayerMask whatIsGround;
    public LayerMask whatIsPlayer;
    
    [Header("Checker Object")]
    public float wallCheckDistance = 0.2f;
    public float ledgeCheckDistance = 0.4f;
    public float groundCheckRadius = 0.3f;

    [Header("Status")] 
    public float maxHp = 30f;

    [Header("Move State")]
    public float movementSpeed = 3f;

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

    [Header("Look For Player State")]
    public int maxTurnCount = 2;
    public float timeBetweenTurn = 0.75f;

    [Header("Attack State")] 
    public float closeRangeActionDistance = 1f;

    [Header("Melee Attack State")] 
    [FormerlySerializedAs("atkRadius")] public float attackRadius = 0.5f;
    [FormerlySerializedAs("atkDamage")] public float attackDamage = 10f;

    [Header("Stun State")] 
    public float stunTime = 3f;
    public float knockbackTime = 0.2f;
    public float knockbackSpeed = 20f;
    public float stunResistance = 3f;
    public float stunRecoveryTime = 2f;
    public Vector2 knockbackAngle;

    [Header("Dead State")] 
    public GameObject deathChunkParticle;
    public GameObject deathBloodParticle;
    public GameObject hitParticle;
}
