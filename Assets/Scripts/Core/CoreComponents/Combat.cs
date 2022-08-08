using System.Collections;
using System.Collections.Generic;
using Interfaces;
using UnityEngine;

public class Combat : CoreComponent, IDamageable, IKnockbackable
{
    private bool isActiveKnockback;
    private float knockbackStartTime;

    public void LogicUpdate()
    {
        CheckKnockbackFinish();
    }
    
    public void Damage(float amount)
    {
        Debug.Log(core.transform.parent.name + " Damaged!");
    }

    public void Knockback(float strength, Vector2 angle, int direction)
    {
        core.Movement.SetVelocityDirection(strength, angle, direction);
        core.Movement.CanSetVelocity = false; // 넉백 발동 시 일시적으로 Velocity 적용 불가
        isActiveKnockback = true;
        knockbackStartTime = Time.time;
    }

    // 넉백이 끝났는 지 판단하는 함수
    private void CheckKnockbackFinish()
    {
        // 넉백 중 땅에 닿았다면
        if (isActiveKnockback && core.Movement.CurrentVelocity.y <= 0.01f && core.CollisionSenses.GetGround)
        {
            isActiveKnockback = false;
            core.Movement.CanSetVelocity = true;
        }
    }
}
