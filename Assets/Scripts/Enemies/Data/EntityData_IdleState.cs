using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "newEntityData_IdleState", menuName = "Data/Entity State Data/Idle Data")]
public class EntityData_IdleState : ScriptableObject
{
    public float minIdleTime = 1f;
    public float maxIdleTime = 2f;
}
