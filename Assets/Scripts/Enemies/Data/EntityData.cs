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
    public float maxHealthPoint = 30f;
    
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
    public float attackRadius = 0.5f;
    public float attackDamage = 10f;

    [Header("Melee Attack State")] 
    public float closeRangeActionDistance = 1f;
    public Vector2 knockbackAngle = Vector2.one;
    public float knockbackStrength = 10f;

    [Header("Stun State")] 
    public float stunTime = 3f;
    [FormerlySerializedAs("knockbackTime")] public float stunKnockbackTime = 0.2f;
    [FormerlySerializedAs("knockbackSpeed")] public float stunKnockbackSpeed = 20f;
    public Vector2 stunKnockbackAngle;
    public float stunResistance = 3f;
    public float stunRecoveryTime = 2f;

    [Header("Dodge State")] 
    public float dodgeSpeed = 10f; // 회피 속도
    public float dodgeTime = 0.2f; // 회피 지속 시간
    public float dodgeCooldown = 2f; // 회피 쿨타임
    public Vector2 dodgeAngle;

    [Header("Ranged Attack State")] 
    public GameObject projectile;
    public float projectileDamage = 10f;
    public float projectileSpeed = 12f;
    public float projectileReach; // 발사체 사정 거리
}
