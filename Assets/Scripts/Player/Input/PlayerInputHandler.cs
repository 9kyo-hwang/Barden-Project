using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    private PlayerInput playerInput; // Player Input component
    private Camera camera; // Camera Component
    
    // 각종 상태에서 값을 읽을 수 있어야 함
    public Vector2 movementInputRaw { get; private set; }
    public Vector2 dashDirectionInputRaw { get; private set; }
    public Vector2Int dashDirectionInputInt { get; private set; }
    public int normalizedInputX { get; private set; }
    public int normalizedInputY { get; private set; }
    public bool isJumpInputStarted { get; private set; }
    public bool isJumpInputCanceled { get; private set; }
    public bool isGrabInputStarted { get; private set; }
    public bool isDashInputStarted { get; private set; }
    public bool isDashInputCanceled { get; private set; }

    // input true 유지 시간(짧은 간격으로 키 입력 반복 오류를 막기 위한 변수)
    [SerializeField]
    private float inputHoldTime = 0.2f;
    
    private float jumpInputStartTime;
    private float dashInputStartTime;

    private void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        camera = Camera.main;
    }

    private void Update()
    {
        CheckJumpInputHoldTime();
        CheckDashInputHoldTime();
    }

    // InputAction에 지정된 키 context를 읽어 거기에 지정된 value 값을 넣음
    // 입력키: WASD, 방향키
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        movementInputRaw = context.ReadValue<Vector2>(); // 키다운 한 버튼의 Vector2 값 대입

        // 값 정규화(-1, 0, 1)
        if (Mathf.Abs(movementInputRaw.x) > 0.5f)
        {
            normalizedInputX = (int)(movementInputRaw * Vector2.right).normalized.x;
        }
        else
        {
            normalizedInputX = 0;
        }

        if (Mathf.Abs(movementInputRaw.y) > 0.5f)
        {
            normalizedInputY = (int)(movementInputRaw * Vector2.up).normalized.y;
        }
        else
        {
            normalizedInputY = 0;
        }


    }
    
    // 점프 버튼으로 지정한 키가 입력될 경우 수행하게 되는 함수
    // 점프 키: Space
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        // jump 키다운 시
        if (context.started)
        {
            isJumpInputStarted = true;
            isJumpInputCanceled = false;
            jumpInputStartTime = Time.time;
        }

        // jump 키업 시
        if (context.canceled)
        {
            isJumpInputCanceled = true;
        }
    }

    public void OnGrabInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isGrabInputStarted = true;
        }

        if (context.canceled)
        {
            isGrabInputStarted = false;
        }
    }

    public void OnDashInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            isDashInputStarted = true;
            isDashInputCanceled = false;
            dashInputStartTime = Time.time;
        }
        else if (context.canceled)
        {
            isDashInputCanceled = true;
        }
    }

    public void OnDashDirectionInput(InputAction.CallbackContext context)
    {
        dashDirectionInputRaw = context.ReadValue<Vector2>();

        if (playerInput.currentControlScheme == "PC")
        {
            dashDirectionInputRaw = camera.ScreenToWorldPoint(dashDirectionInputRaw) - transform.position;
        }

        // int 반올림(0, 0 || 0, 1 || 1, 0 || 1, 1 || etc)
        dashDirectionInputInt = Vector2Int.RoundToInt(dashDirectionInputRaw.normalized);
    }

    // 점프 키 입력 참 판정 체크를 한 번만 하기 위해
    // false로 해제시키는 용도의 함수
    public void UsedJumpInput()
    {
        isJumpInputStarted = false;
    }

    public void UsedDashInput()
    {
        isDashInputStarted = false;
    }

    private void CheckJumpInputHoldTime()
    {
        if (Time.time >= jumpInputStartTime + inputHoldTime)
        {
            isJumpInputStarted = false;
        }
    }

    private void CheckDashInputHoldTime()
    {
        if (Time.time >= dashInputStartTime + inputHoldTime)
        {
            isDashInputStarted = false;
        }
    }
}
