using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveInputDir; // 입력한 방향키의 방향을 반환하는 변수
    private bool isFacingRight = true; // 현재 캐릭터가 바라보는 방향이 오른쪽인 지 체크하는 boolean 변수
    private bool isWalking; // 현재 캐릭터가 걷고있는 지 체크하는 boolean 변수
    private bool isGrounded; // 현재 캐릭터가 땅 위에 있는 지 체크하는 boolean 변수
    private bool canJump; // 현재 캐릭터가 점프할 수 있는 상태인 지 나타내는 boolean 변수

    public float moveSpeed = 10.0f; // 이동 속도를 정의하는 변수
    public float jumpForce = 16.0f; // 점프력을 정의하는 변수
    public float groundCheckRadius; // 땅 체크를 위해 그려지는 원의 반지름 크기
    public int amountOfJumps = 1; // 점프 가능한 총 횟수를 나타내는 int형 변수
    private int amountOfJumpsLeft; // 남은 점프 가능 횟수를 나타내는 int형 변수
    
    private Rigidbody2D rb2d; // Rigidbody2D를 불러오기 위한 변수
    private Animator anim; // Animator를 불러오기 위한 변수
    public Transform groundChecker; // GroundChecker 오브젝트를 담기 위한 변수
    public LayerMask whatIsGround; // 땅 레이어를 담기 위한 레이어마스크 변수

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트 불러옴
        anim = GetComponent<Animator>(); // Animator 컴포넌트 불러옴
        amountOfJumpsLeft = amountOfJumps; // 남은 점프 가능 횟수 초기화
    }

    // 매 프레임마다 호출
    // 키 입력 등을 받을 때 사용
    void Update()
    {   
        // 입력 체크
        CheckInput();
        // 캐릭터 방향 체크
        CheckMoveDir();
        // 캐릭터 애니메이션 갱신
        UpdateAnimations();
        // 점프 가능 상태인 지 체크
        CheckIfCanJump();
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
        // canJump 상태일 경우 속력 벡터 갱신
        // x축: 기존 x축 속력 벡터 유지
        // y축: 설정된 점프 힘만큼 부여
        // 남은 점프 가능 횟수 1 감소
        if(canJump)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            amountOfJumpsLeft--;
        }
    }

    void CheckIfCanJump()
    {
        // 땅에 있으면서 캐릭터의 y축 속력 벡터가 0 이하일 경우 점프 가능한 남은 횟수 초기화
        if(isGrounded && rb2d.velocity.y <= 0)
            amountOfJumpsLeft = amountOfJumps;

        if(amountOfJumpsLeft <= 0) // 남은 점프 가능 횟수가 0 이하면 점프 불가능
            canJump = false;
        else // 아니라면 점프 가능
            canJump = true;
    }

    void CheckMoveDir()
    {
        // 현재 바라보는 방향과 입력한 방향키의 방향이 반대일 경우 뒤집는 함수 수행
        if((isFacingRight && moveInputDir < 0) || (!isFacingRight && moveInputDir > 0))
            Flip();
        
        // 캐릭터의 x축 속력 벡터가 0이 아니면 Walk, 아니라면 Idle
        if(rb2d.velocity.x != 0)
            isWalking = true;
        else
            isWalking = false;
    }

    void Flip()
    {
        isFacingRight = !isFacingRight; // 참 <-> 거짓을 변환해서 정방향에 맞게 수정
        transform.Rotate(0.0f, 180.0f, 0.0f); // 캐릭터를 Y축으로 180도 회전시킴
    }

    void UpdateAnimations()
    {
        // isWalking 변수의 참거짓에 따라 animator isWalking 설정
        anim.SetBool("isWalking", isWalking);
        // isGrounded 변수의 참거짓에 따라 animator isGrounded 설정 
        anim.SetBool("isGrounded", isGrounded);
        // 캐릭터 리지드바디의 y축 속력 벡터값만큼 animator yVelocity 설정
        anim.SetFloat("yVelocity", rb2d.velocity.y);
    }

    // 매 Timestep마다 호출
    // 물리 효과가 적용된 오브젝트를 조정할 때 사용
    void FixedUpdate()
    {
        // 움직임 적용
        ApplyMovement();
        // 캐릭터 주위에 있는 오브젝트를 체크하는 함수
        CheckSurroundings();
    }

    void ApplyMovement()
    {
        // 속력 벡터 갱신
        // x축: 입력받은 방향 * 설정된 이동 속도
        // y축: 기존 y축 속력 벡터 유지
        rb2d.velocity = new Vector2(moveSpeed * moveInputDir, rb2d.velocity.y);
    }

    void CheckSurroundings()
    {
        // 캐릭터 하위에 있는 그라운드 체커 오브젝트 기준으로 원을 그려 그 안에 들어온 물체의 레이어가 땅이면 isGrounded 참으로 설정
        isGrounded = Physics2D.OverlapCircle(groundChecker.position, groundCheckRadius, whatIsGround);
    }

    void OnDrawGizmos()
    {
        // 그라운드 체커 위치에 그라운드체크 원을 그려줌
        Gizmos.DrawWireSphere(groundChecker.position, groundCheckRadius);    
    }
}
