using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 코어 컴포넌트들을 총괄하는 스크립트
public class Core : MonoBehaviour
{
    public Movement movement {get; private set;}
    public CollisionSenses colSenses {get; private set;}

    // 하위 오브젝트에 있는 코어 컴포넌트들의 할당
    private void Awake()
    {
        movement = GetComponentInChildren<Movement>();
        colSenses = GetComponentInChildren<CollisionSenses>();

        if(!movement || !colSenses)
        {
            Debug.LogError("Missing Core Component");
        }
    }

    // 코어 컴포넌트들의 LogicUpdate 수행
    public void LogicUpdate()
    {
        movement.LogicUpdate();
    }
}
