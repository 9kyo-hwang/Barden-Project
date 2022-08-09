using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Interfaces;

public class AggressiveWeapon : Weapon
{
    #region Core Components
    private Movement Movement => movement ?? core.GetCoreComponentValue(ref movement);
    private Movement movement;
    #endregion
    
    // AggressiveWeaopn은 SO_WeaponData를 상속받는 SO_AggressiveData를 가져야 함
    protected SO_AggressiveWeaponData aggressiveWeaponData;

    // IDamageable을 갖고 있는 객체들을 담을 리스트
    private List<IDamageable> detectedDamageables = new List<IDamageable>();
    private List<IKnockbackable> detectedKnockbackables = new List<IKnockbackable>();

    protected override void Awake()
    {
        base.Awake();

        // Weapon에 있는 SO_WeaponData를 역추적해서 현재 들어온 데이터가 SO_AggressiveData가 맞는 지 확인
        // Defensive Weapon, Aggressive Weapon 등 각 무기에 맞는 SO 데이터가 담겨있도록 하는 방지책
        // 참조이기 때문에 상위 SO_WeaponData를 넣어도 Error 발생
        
        if(weaponData.GetType() == typeof(SO_AggressiveWeaponData))
        {
            aggressiveWeaponData = (SO_AggressiveWeaponData)weaponData;
        }
        else
        {
            Debug.LogError("Wrong data for the weapon");
        }
    }

    public override void AnimationActionTrigger()
    {
        base.AnimationActionTrigger();

        CheckMeleeAttack();
    }

    // 근접 공격
    private void CheckMeleeAttack()
    {
        var details = aggressiveWeaponData.Details[attackCount]; // Weapon 데이터의 attackDetails 가져옴

        // detectedDamageable에 담긴 IDamageable 모든 원소 받아옴
        // 데미지를 받아 Destroy 되는 시점에서 foreach 문은 아직 다 리스트 내 모든 오브젝트를 순회하지 못함.
        // 이 때 발생하는 Error를 막기 위해, .ToList()를 사용해 리스트 사본에 접근하게 함. System.Linq 포함 필요
        foreach (var item in detectedDamageables.ToList())
        {
            item.Damage(details.damageAmount); // details의 데미지만큼 IDamageable 오브젝트들의 Damage 수행
        }
        
        // detectedKnockbackable에 담긴 IDamageable 모든 원소 받아옴
        foreach (var item in detectedKnockbackables.ToList())
        {
            item.Knockback(details.knockbackStrength, details.knockbackAngle, Movement.FacingDir); // details의 데미지만큼 IDamageable 오브젝트들의 Damage 수행
        }
    }

    // 공격 범위에 들어온 적의 IDamageable을 리스트에 저장
    public void AddToDetected(Collider2D other)
    {
        Debug.Log("AddToDetected");

        var damageable = other.GetComponent<IDamageable>();
        detectedDamageables?.Add(damageable); // null이 아니라면 추가

        var knockbackable = other.GetComponent<IKnockbackable>();
        detectedKnockbackables?.Add(knockbackable); // null이 아니라면 추가
    }

    // 공격 범위에서 나간 적의 IDamageable을 리스트에서 제거
    public void RemoveFromDetected(Collider2D other)
    {
        Debug.Log("RemoveFromDetected");

        var damageable = other.GetComponent<IDamageable>();
        detectedDamageables?.Remove(damageable); // null이 아니라면 제거

        var knockbackable = other.GetComponent<IKnockbackable>();
        detectedKnockbackables?.Remove(knockbackable); // null이 아니라면 제거
    }
}
