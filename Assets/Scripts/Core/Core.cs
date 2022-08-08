using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq; // Query 기능
using UnityEngine;

// 코어 컴포넌트들을 총괄하는 스크립트
public class Core : MonoBehaviour
{
    // 각 컴포넌트들을 직접 프로퍼티로 받아오는 것이 아닌
    // 컴포넌트 리스트를 만들어 거기서 관리하도록 변경

    private readonly List<CoreComponent> components = new List<CoreComponent>();

    public void LogicUpdate()
    {
        // List에 있는 코어 컴포넌트들의 LogicUpdate 수행
        foreach (var component in components)
        {
            component.LogicUpdate();
        }
    }
    
    // Core Component를 리스트에 저장하는 함수
    public void AddComponent(CoreComponent component)
    {
        // List에 해당 컴포넌트가 있지 않다면 List에 추가
        if (!components.Contains(component))
        {
            components.Add(component);
        }
    }

    // Core Component 타입 오브젝트를 받아오는 Generic 함수
    public T GetCoreComponent<T>() where T : CoreComponent
    {
        // T 타입 오브젝트만 필터링해 반환
        // 그 중 (특정 조건에 맞는) 첫 번째 요소 내지 기본값 반환
        var component = components.OfType<T>().FirstOrDefault();

        if (component == null)
        {
            Debug.LogWarning($"{typeof(T)} not found on {transform.parent.name}");
        }

        return component;
    }

    // ref: C++의 참조자 &와 동일한 역할
    public T GetCoreComponentValue<T>(ref T value) where T : CoreComponent
    {
        return value = GetCoreComponent<T>();
    }
}
