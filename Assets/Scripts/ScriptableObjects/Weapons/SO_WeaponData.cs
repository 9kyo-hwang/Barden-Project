using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 대량의 데이터를 저장하는 데 사용되는 데이터 컨테이너
// 값의 사본이 생성되는 것을 방지, 프로젝트의 메모리 사용량 감소 가능
// MonoBehaviour 스크립트에서 변경되지 않는 데이터를 저장하는 프리팹에 주로 이용
[CreateAssetMenu(fileName = "newWeaponData", menuName = "Data/Weapon Data/Weapon")]
public class SO_WeaponData : ScriptableObject
{
    public float[] moveSpeed;
}
