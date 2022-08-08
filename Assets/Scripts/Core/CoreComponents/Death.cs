using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Death : CoreComponent
{
    [SerializeField] private GameObject[] deathParticles;
    
    private ParticleManager ParticleManager => particleManager ?? core.GetCoreComponentValue(ref particleManager);
    private ParticleManager particleManager;
    
    private Stats Stats => stats ?? core.GetCoreComponentValue(ref stats);
    private Stats stats;

    public override void Init(Core core)
    {
        base.Init(core);

        Stats.HealthZero += Die; // +=으로 이벤트 구독
    }
    
    public void Die()
    {
        // 죽은 몹들한테서 particle을 발생시킴
        foreach (var particle in deathParticles)
        {
            ParticleManager.NoRotationParticles(particle);
        }
        // core의 부모 게임 오브젝트 비활성화
        core.transform.parent.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        Stats.HealthZero += Die; // 활성화 시 이벤트 구독
    }

    private void OnDisable()
    {
        Stats.HealthZero -= Die; // 비활성화 시 이벤트 구독 취소
    }
}
