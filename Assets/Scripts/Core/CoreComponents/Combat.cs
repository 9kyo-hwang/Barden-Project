using System.Collections;
using System.Collections.Generic;
using System.Security;
using Interfaces;
using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{
    [SerializeField] private GameObject damageParticles;
    [SerializeField] private float maxKnockbackTime = 0.2f;
    private bool isActiveKnockback;
    private float knockbackStartTime;

    #region Core Components
    private Movement Movement => core.GetCoreComponentValue(ref movement);
    private CollisionSenses CollisionSenses => core.GetCoreComponentValue(ref collisionSenses);
    private Stats Stats => core.GetCoreComponentValue(ref stats);
    private ParticleManager ParticleManager => core.GetCoreComponentValue(ref particleManager);
    
    private Movement movement;
    private CollisionSenses collisionSenses;
    private Stats stats;
    private ParticleManager particleManager;
    #endregion

    public override void LogicUpdate()
    {
        CheckKnockbackFinish();
    }
    
    public void Damage(float amount)
    {
        Debug.Log(core.transform.parent.name + " Damaged!");
        Stats?.DecreaseHealthPoint(amount);
        ParticleManager?.RandomRotationParticles(damageParticles);
    }

    public void Knockback(float strength, Vector2 angle, int direction)
    {
        Movement?.SetVelocityDirection(strength, angle, direction);
        Movement.CanSetVelocity = false; // 넉백 발동 시 일시적으로 Velocity 적용 불가
        isActiveKnockback = true;
        knockbackStartTime = Time.time;
    }

    // 넉백이 끝났는 지 판단하는 함수
    private void CheckKnockbackFinish()
    {
        // 넉백 중에 땅에 닿았거나 넉백 유지 시간을 초과했다면 넉백 끝
        if (isActiveKnockback && ((Movement?.CurrentVelocity.y <= 0.01f && CollisionSenses.GetGround) || Time.time >= knockbackStartTime + maxKnockbackTime))
        {
            isActiveKnockback = false;
            Movement.CanSetVelocity = true;
        }
    }
}
