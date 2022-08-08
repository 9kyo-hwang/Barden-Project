using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// entity에 다양한 위치, 회전값으로 particle을 생성하게 하는 함수들의 집합
public class ParticleManager : CoreComponent
{
    // 스폰될 파티클들을 담을 부모 오브젝트 위치
    private Transform particleContainer;

    protected override void Awake()
    {
        base.Awake();

        // ParticleContainer란 태그를 가진 게임 오브젝트를 찾아 해당 position 반환
        particleContainer = GameObject.FindGameObjectWithTag("ParticleContainer").transform;
    }
    
    // 원하는 위치와 회전값으로 파티클 생성
    public GameObject InstantiateParticles(GameObject particlesPrefab, Vector2 position, Quaternion rotation)
    {
        return Instantiate(particlesPrefab, position, rotation, particleContainer);
    }

    // 회전없이 파티클 생성
    public GameObject NoRotationParticles(GameObject particlesPrefab)
    {
        return InstantiateParticles(particlesPrefab, transform.position, Quaternion.identity);
    }

    // 랜덤한 회전값으로 파티클 생성
    public GameObject RandomRotationParticles(GameObject particlesPrefab)
    {
        var randomRotation = Quaternion.Euler(0f, 0f, Random.Range(0f, 360f));
        return InstantiateParticles(particlesPrefab, transform.position, randomRotation);
    }
}
