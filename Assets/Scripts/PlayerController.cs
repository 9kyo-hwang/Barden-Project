using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveInputDir; // 입력한 방향키의 방향을 반환하는 변수
    private bool isFacingRight = true; // 현재 캐릭터가 바라보는 방향이 오른쪽인 지 체크하는 boolean 변수
    public float moveSpeed = 10.0f; // 이동 속도를 정의하는 변수

    public float jumpForce = 16.0f;
    private Rigidbody2D rb2d; // Rigidbody2D를 불러오기 위한 변수

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트 불러옴        
    }

    // 매 프레임마다 호출
    // 키 입력 등을 받을 때 사용
    void Update()
    {   
        // 입력 체크
        CheckInput();
        // 캐릭터 방향 체크
        CheckMoveDir();
    }

    // 매 Timestep마다 호출
    // 물리 효과가 적용된 오브젝트를 조정할 때 사용
    void FixedUpdate()
    {
        // 매 Timestep마다 움직임 적용
        ApplyMovement();    
    }

    void CheckInput()
    {
        // Horizontal 입력 축 값을 반환함
        // -1: Left, 0: None, 1: Right
        moveInputDir = Input.GetAxisRaw("Horizontal");

        // Jump에 배당된 버튼 키다운 시 Jump 수행
        if(Input.GetButtonDown("Jump"))
            Jump();
    }

    void Jump()
    {
        // 속력 벡터 갱신
        // x축: 기존 x축 속력 벡터 유지
        // y축: 설정된 점프 힘만큼 부여
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
    }

    void CheckMoveDir()
    {
        // 현재 바라보는 방향과 입력한 방향키의 방향이 반대일 경우 뒤집는 함수 수행
        if((isFacingRight && moveInputDir < 0) || (!isFacingRight && moveInputDir > 0))
            Flip();
    }

    void Flip()
    {
        isFacingRight = !isFacingRight; // 참 <-> 거짓을 변환해서 정방향에 맞게 수정
        transform.Rotate(0.0f, 180.0f, 0.0f); // 캐릭터를 Y축으로 180도 회전시킴
    }

    void ApplyMovement()
    {
        // 속력 벡터 갱신
        // x축: 입력받은 방향 * 설정된 이동 속도
        // y축: 기존 y축 속력 벡터 유지
        rb2d.velocity = new Vector2(moveSpeed * moveInputDir, rb2d.velocity.y);
    }
}
