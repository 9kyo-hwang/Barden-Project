using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// SO_WeaponData를 상속받는 공격형 무기 전용 데이터 컨테이너
[CreateAssetMenu(fileName = "newAggressiveWeaponData", menuName = "Data/Weapon Data/Aggressive Weapon")]
public class SO_AggressiveWeaponData : SO_WeaponData
{
    // 무기 공격에 필요한 변수가 담긴 구조체 배열 선언
    [SerializeField] private WeaponAttackDetails[] attackDetails;
    public WeaponAttackDetails[] AttackDetails { get => attackDetails; set => attackDetails = value; }

    private void OnEnable() // Awake 대신 사용(일반 스크립트와 달리 매 게임 시작할 때마다 호출되지 않기 때문에)
    {
        AttackCount = attackDetails.Length; // 세부 공격 길이만큼 공격 횟수 설정
        MovementSpeed = new float[AttackCount]; // 각 공격에 적용되는 움직임 속도 배열 생성

        for(int i = 0; i < AttackCount; i++)
        {
            MovementSpeed[i] = attackDetails[i].moveSpeed;
        }
    }
}
