using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputHandler : MonoBehaviour
{
    // 각종 상태에서 값을 읽을 수 있어야 함
    public Vector2 movementInputRaw { get; private set; }
    public int normalizedInputX { get; private set; }
    public int normalizedInputY { get; private set; }

    // InputAction에 지정된 키 context를 읽어 거기에 지정된 value 값을 넣음
    // 입력키: WASD, 방향키
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        movementInputRaw = context.ReadValue<Vector2>();

        // 값 정규화(-1, 0, 1)
        normalizedInputX = (int)(movementInputRaw * Vector2.right).normalized.x;
        normalizedInputY = (int)(movementInputRaw * Vector2.up).normalized.y;
    }
    
    // 점프키로 지정한 키가 입력될 경우 수행하게 되는 함수
    // 점프키: Space
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Debug.Log("Jump button pushed down now");
        }

        if (context.performed)
        {
            Debug.Log("Jump button is being held down");
        }

        if (context.canceled)
        {
            Debug.Log("Jump button has been released");
        }
    }
}
