using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player - Weapons - 무기 - Base에 부착될 스크립트
// base의 animation trigger를 동작시키는 중간자 역할 스크립트
public class WeaponAnimationToWeapon : MonoBehaviour
{
    private Weapon weapon;

    private void Start() 
    {
        // 부모 오브젝트에 있는 weapon 컴포넌트 반환
        weapon = GetComponentInParent<Weapon>();   
    }

    private void AnimationFinishTrigger()
    {
        weapon.AnimationFinishTrigger();
    }

    private void AnimationStartMovementTrigger()
    {
        weapon.AnimationStartMovementTrigger();
    }

    private void AnimationStopMovementTrigger()
    {
        weapon.AnimationStopMovementTrigger();
    }

    private void AnimationTurnOffFlipTrigger()
    {
        weapon.AnimationTurnOffFlipTrigger();
    }

    private void AnimationTurnOnFlipTrigger()
    {
        weapon.AnimationTurnOnFlipTrigger();
    }
}
