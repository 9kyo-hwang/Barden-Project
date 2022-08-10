using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// weapon의 히트박스 판정을 실제로 수행하는 중간자 스크립트
public class WeaponHitboxToWeapon : MonoBehaviour
{
    private AggressiveWeapon weapon;

    private void Awake() 
    {
        weapon = GetComponentInParent<AggressiveWeapon>();    
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {
        Debug.Log("OnTriggerEnter2D");
        weapon.AddToDetected(other);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        Debug.Log("OnTriggerExit2D");
        weapon.RemoveFromDetected(other);   
    }
}
