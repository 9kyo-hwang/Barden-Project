using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput; // Player Input component
    private Camera camera; // Camera Component
    
    // property 변수들
    public Vector2 InputMovementRaw { get; private set; }
    public Vector2 InputDashDirectionRaw { get; private set; }
    public Vector2Int InputDashDirectionInt { get; private set; }
    public int InputXNormalize { get; private set; }
    public int InputYNormalize { get; private set; }
    public bool IsInputJumpStarted { get; private set; }
    public bool IsInputJumpCanceled { get; private set; }
    public bool IsInputGrabStarted { get; private set; }
    public bool IsInputDashStarted { get; private set; }
    public bool IsInputDashCanceled { get; private set; }
    public bool[] IsInputAttackArray { get; private set; }

    // input true 유지 시간(짧은 간격으로 키 입력 반복 오류를 막기 위한 변수)
    [SerializeField] private float inputHoldTime = 0.2f;
    private float jumpInputStartTime;
    private float dashInputStartTime;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();

        int attackInputCount = Enum.GetValues(typeof(AttackInput)).Length; // enum에 등록된 공격 키 개수 반환
        IsInputAttackArray = new bool[attackInputCount]; // 공격 키 개수만큼 boolean 배열 생성
        
        camera = Camera.main;
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
    }

    public void OnPrimaryAttackInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            IsInputAttackArray[(int)AttackInput.primary] = true;
        }

        if(context.canceled)
        {
            IsInputAttackArray[(int)AttackInput.primary] = false;
        }
    }

    public void OnSecondaryAttackInput(InputAction.CallbackContext context)
    {
        if(context.started)
        {
            IsInputAttackArray[(int)AttackInput.secondary] = true;
        }

        if(context.canceled)
        {
            IsInputAttackArray[(int)AttackInput.secondary] = false;
        }
    }

    // InputAction에 지정된 키 context를 읽어 거기에 지정된 value 값을 넣음
    // 입력키: WASD, 방향키
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        InputMovementRaw = context.ReadValue<Vector2>(); // 키다운 한 버튼의 Vector2 값 대입

        // 소수점 반올림으로 값 정규화(-1, 0, 1)
        InputXNormalize = Mathf.RoundToInt(InputMovementRaw.x);
        InputYNormalize = Mathf.RoundToInt(InputMovementRaw.y);
    }
    
    // 점프 버튼으로 지정한 키가 입력될 경우 수행하게 되는 함수
    // 점프 키: Space
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        // jump 키다운 시
        if (context.started)
        {
            IsInputJumpStarted = true;
            IsInputJumpCanceled = false;
            jumpInputStartTime = Time.time;
        }

        // jump 키업 시
        if (context.canceled)
        {
            IsInputJumpCanceled = true;
        }
    }

    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsInputGrabStarted = true;
        }

        if (context.canceled)
        {
            IsInputGrabStarted = false;
        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            IsInputDashStarted = true;
            IsInputDashCanceled = false;
            dashInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            IsInputDashCanceled = true;
        }
    }

    public void OnDashDirectionInput(InputAction.CallbackContext context)
    {
        InputDashDirectionRaw = context.ReadValue<Vector2>();

        if (playerInput.currentControlScheme == "PC")
        {
            InputDashDirectionRaw = camera.ScreenToWorldPoint(InputDashDirectionRaw) - transform.position;
        }

        // int 반올림(0, 0 || 0, 1 || 1, 0 || 1, 1 || etc)
        InputDashDirectionInt = Vector2Int.RoundToInt(InputDashDirectionRaw.normalized);
    }

    // 점프 키 입력 참 판정 체크를 한 번만 하기 위해
    // false로 해제시키는 용도의 함수
    public void UsedJumpInput()
    {
        IsInputJumpStarted = false;
    }

    public void UsedDashInput()
    {
        IsInputDashStarted = false;
    }

    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
        {
            IsInputJumpStarted = false;
        }
    }

    private void CheckDashInputHoldTime()
    {
        if (Time.time >= dashInputStartTime + inputHoldTime)
        {
            IsInputDashStarted = false;
        }
    }
}

public enum AttackInput { primary, secondary }