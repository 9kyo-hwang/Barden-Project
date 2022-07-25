using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어에 사용될 각종 변수 및 값을 저장하는 클래스
// 스크립트로부터 애셋 생성 가능
[CreateAssetMenu(fileName = "newPlayerData", menuName = "Data/Player Data/Base Data")]
public class PlayerData : ScriptableObject
{
    [Header("Move State")] 
    public float moveVelocity = 10f;
}
