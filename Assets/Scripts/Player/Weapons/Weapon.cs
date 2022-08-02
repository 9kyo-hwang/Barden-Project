using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player - Weapons - 각 무기에 부착되는 스크립트
public class Weapon : MonoBehaviour
{
    // SO_WeaponData를 상속받는 하위 SO들도 다 넣을 수 있음
    // 그렇기 때문에 하위 SO 스크립트에서 데이터를 잘못 넣을 경우를 차단할 방도 요구
    [SerializeField] protected SO_WeaponData weaponData;

    protected Animator baseAnim;
    protected Animator weaponAnim;

    protected PlayerAttackState attackState;

    protected int attackCount;

    protected virtual void Awake()
    {
        // Weapon의 하위 오브젝트에 animator 컴포넌트 존재
        baseAnim = transform.Find("Base").GetComponent<Animator>();
        weaponAnim = transform.Find("Weapon").GetComponent<Animator>();

        gameObject.SetActive(false);
    }

    public virtual void EnterWeapon()
    {
        gameObject.SetActive(true);

        // weapon data에서 정의한 MovementSpeed 길이보다 클 경우 공격 횟수 초과
        if(attackCount >= weaponData.AttackCount)
        {
            attackCount = 0;
        }

        baseAnim.SetBool("attack", true);
        weaponAnim.SetBool("attack", true);

        baseAnim.SetInteger("attackCount", attackCount);
        weaponAnim.SetInteger("attackCount", attackCount);
    }

    public virtual void ExitWeapon()
    {
        baseAnim.SetBool("attack", false);
        weaponAnim.SetBool("attack", false);

        attackCount++; // 현재 무기 공격 상태를 나갈 때 공격 횟수 1 증가

        gameObject.SetActive(false);
    }

    #region Animation Triggers

    public virtual void AnimationFinishTrigger()
    {
        attackState.AnimationFinishTrigger();
    }

    // Animation에 찍힌 Trigger 지점부터 공격 모션에 따라 움직이게 하는 함수
    public virtual void AnimationStartMovementTrigger()
    {
        attackState.SetPlayerVelocity(weaponData.MovementSpeed[attackCount]); // Weapon Data에 정의된 현재 공격 횟수의 움직임 속도로 플레이어 Velocity 설정
    }

    // Animation에 찍힌 Trigger 지점부터 공격 모션에 따른 움직임을 멈추게 하는 함수
    public virtual void AnimationStopMovementTrigger()
    {
        attackState.SetPlayerVelocity(0f); // 현재 공격 상태가 끝날 때 Velocity 0으로 설정
    }

    // Animation에 찍힌 Trigger 지점부터 Flip을 할 수 없게 하는 함수
    public virtual void AnimationTurnOffFlipTrigger()
    {
        attackState.SetFlipCheck(false);
    }

    // Animation에 찍힌 Trigger 지점부터 Flip을 할 수 있게 하는 함수
    public virtual void AnimationTurnOnFlipTrigger()
    {
        attackState.SetFlipCheck(true);
    }
    
    public virtual void AnimationActionTrigger()
    {

    }

    #endregion

    public void InitializeWeapon(PlayerAttackState attackState)
    {
        this.attackState = attackState;
    }
}
