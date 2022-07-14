using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveInputDir; // 입력한 방향키의 방향을 반환하는 변수
    private bool isFacingRight = true; // 현재 캐릭터가 바라보는 방향이 오른쪽인 지 체크하는 boolean 변수
    private bool isWalking; // 현재 캐릭터가 걷고있는 지 체크하는 boolean 변수
    private bool isGrounded; // 현재 캐릭터가 땅 위에 있는 지 체크하는 boolean 변수
    private bool isTouchingWall; // 현재 캐릭터가 벽에 닿아 있는 지 체크하는 boolean 변수
    private bool isWallSliding; // 현재 캐릭터가 벽 미끄러지기를 하고 있는 지 체크하는 boolean 변수
    private bool canJump; // 현재 캐릭터가 점프할 수 있는 상태인 지 나타내는 boolean 변수

    public float moveSpeed = 10.0f; // 이동 속도를 정의하는 변수
    public float jumpForce = 16.0f; // 점프력을 정의하는 변수
    public float groundCheckRadius; // 땅 체크를 위해 그려지는 원의 반지름 길이
    public float wallCheckDistance; // 벽 체크를 위해 쏘는 Ray의 길이
    public int amountOfJumps = 1; // 점프 가능한 총 횟수를 나타내는 int형 변수
    private int amountOfJumpsLeft; // 남은 점프 가능 횟수를 나타내는 int형 변수
    public float wallSlideSpeed; // 벽 미끄러지기 속도를 정의하는 변수
    public float moveForceInAir; // 공중 이동 속도를 정의하는 변수
    public float airDragMultiplier = 0.95f; // 공중에서 가만히 떨어질 때 캐릭터 y축 속력 벡터에 곱해지는 가중치 
    public float variableJumpHeightMultiplier = 0.5f; // 점프 높이 조절을 위한 가중치
    public float wallHopForce; // 벽 매달리다가 그냥 땅으로 떨어질 때 부여되는 힘
    public float wallJumpForce; // 벽 매달리다가 반대쪽으로 점프할 때 부여되는 힘
    private float facingDir = 1; // 캐릭터 좌, 우 방향을 나타내기 위한 변수

    private Rigidbody2D rb2d; // Rigidbody2D를 불러오기 위한 변수
    private Animator anim; // Animator를 불러오기 위한 변수
    public Transform groundChecker; // GroundChecker 오브젝트를 담기 위한 변수
    public Transform wallChecker; // WallChecker 오브젝트를 담기 위한 변수
    public LayerMask whatIsGround; // 땅 레이어를 담기 위한 레이어마스크 변수
    public Vector2 wallHopDir; // 벽 매달리다가 그냥 땅으로 떨어지는 방향
    public Vector2 wallJumpDir; // 벽 매달리다가 반대쪽으로 점프하는 방향

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트 불러옴
        anim = GetComponent<Animator>(); // Animator 컴포넌트 불러옴
        amountOfJumpsLeft = amountOfJumps; // 남은 점프 가능 횟수 초기화
        wallHopDir.Normalize();
        wallJumpDir.Normalize();
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
        // 벽 미끄러지기 중인 지 체크
        CheckIfWallSliding();
    }

    void CheckInput()
    {
        // Horizontal 입력 축 값을 반환함
        // -1: Left, 0: None, 1: Right
        moveInputDir = Input.GetAxisRaw("Horizontal");

        // Jump에 배당된 버튼 키 다운 시 Jump 수행
        if(Input.GetButtonDown("Jump"))
            Jump();

        // Jump에 배당된 버튼 키 업 시 캐릭터 y축 속력 벡터 변경
        // 키 업 시점이 빠를 수록 y축 속력 벡터 값이 크므로 감소되는 값도 비례해 커짐
        // 즉 키 업을 빨리할 수록 점프 높이가 낮아짐
        if(Input.GetButtonUp("Jump") && rb2d.velocity.y > 0)
            rb2d.velocity = new Vector2(rb2d.velocity.x, rb2d.velocity.y * variableJumpHeightMultiplier);
    }

    void Jump()
    {
        // canJump 이면서 벽 미끄러지는 중이 아닌 경우 y축 속력 벡터를 설정된 점프 힘만큼 부여
        // 남은 점프 가능 횟수 1 감소
        if(canJump && !isWallSliding)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
            amountOfJumpsLeft--;
        }
        // 벽 미끄러지는 중이면서 방향 입력이 없다면 wallHopForce만큼 캐릭터가 보는 방향의 반대로 힘을 가함
        else if(isWallSliding && moveInputDir == 0 && canJump)
        {
            isWallSliding = false;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallHopForce * wallHopDir.x * -facingDir, wallHopForce * wallHopDir.y);
            rb2d.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
        // 벽 미끄러지는 중이거나 벽에 닿아있으면서 좌우 입력이 있다면 wallJumpForce만큼 입력한 방향으로 힘을 가함
        else if((isWallSliding || isTouchingWall) && moveInputDir != 0 && canJump)
        {
            isWallSliding = false;
            amountOfJumpsLeft--;
            Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDir.x * moveInputDir, wallJumpForce * wallJumpDir.y);
            rb2d.AddForce(forceToAdd, ForceMode2D.Impulse);
        }
    }

    void CheckIfCanJump()
    {
        // 땅에 있으면서 캐릭터의 y축 속력 벡터가 0 이하일 경우
        // 또는 벽 미끄러지는 중일 경우 점프 가능한 남은 횟수 초기화
        if((isGrounded && rb2d.velocity.y <= 0) || isWallSliding)
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
        if(!isWallSliding)
        {
            facingDir *= -1; // -1을 곱해서 정방향에 맞게 값 수정
            isFacingRight = !isFacingRight; // 참 <-> 거짓을 변환해서 정방향에 맞게 수정
            transform.Rotate(0.0f, 180.0f, 0.0f); // 캐릭터를 Y축으로 180도 회전시킴
        }
    }

    void CheckIfWallSliding()
    {
        // 벽에 닿았으면서 땅이 아니고 캐릭터의 y축 속력 벡터가 감소하는 중이라면
        if(isTouchingWall && !isGrounded && rb2d.velocity.y < 0)
            isWallSliding = true;
        else
            isWallSliding = false;
    }

    void UpdateAnimations()
    {
        // isWalking 변수의 참거짓에 따라 animator isWalking 설정
        anim.SetBool("isWalking", isWalking);
        // isGrounded 변수의 참거짓에 따라 animator isGrounded 설정 
        anim.SetBool("isGrounded", isGrounded);
        // 캐릭터 리지드바디의 y축 속력 벡터값만큼 animator yVelocity 설정
        anim.SetFloat("yVelocity", rb2d.velocity.y);
        // isWallSliding 변수의 참거짓에 따라 animator isWallSliding 설정
        anim.SetBool("isWallSliding", isWallSliding);
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
        // 땅에 있지 않으면서 벽 미끄러지는 중이 아니고 좌,우 입력이 없다면
        if(!isGrounded && !isWallSliding && moveInputDir == 0)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x * airDragMultiplier, rb2d.velocity.y);
        }
        // 땅에 있는 경우 입력받은 방향과 이동 속도에 따른 x축 속력 벡터 갱신
        else
        {
            rb2d.velocity = new Vector2(moveSpeed * moveInputDir, rb2d.velocity.y);
        }

        // 이 코드는 사용되지 않음
        /* 
        // 땅에 있지 않으면서 벽 미끄러지는 중이 아니고 좌,우 입력이 있다면 moveForceInAir만큼 힘을 가해줌
        else if(!isGrounded && !isWallSliding && moveInputDir != 0)
        {
            Vector2 forceToAdd = new Vector2(moveForceInAir * moveInputDir, 0);
            rb2d.AddForce(forceToAdd);

            // 가한 힘으로 인한 캐릭터 좌우 이동 속도 상한치 설정
            if(Mathf.Abs(rb2d.velocity.x) > moveSpeed)
                rb2d.velocity = new Vector2(moveSpeed * moveInputDir, rb2d.velocity.y);
        }
        */

        // 벽 미끄러지기 중이면 미끄러지는 속도 최대치를 wallSlideSpeed로 적용
        if(isWallSliding)
        {
            if(rb2d.velocity.y < -wallSlideSpeed)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, -wallSlideSpeed);
            }
        }
    }

    void CheckSurroundings()
    {
        // 캐릭터 하위에 있는 그라운드 체커 오브젝트 기준으로 원을 그려 그 안에 들어온 물체의 레이어가 땅이면 isGrounded 참으로 설정
        isGrounded = Physics2D.OverlapCircle(groundChecker.position, groundCheckRadius, whatIsGround);

        isTouchingWall = Physics2D.Raycast(wallChecker.position, transform.right, wallCheckDistance, whatIsGround);
    }

    void OnDrawGizmos()
    {
        // 그라운드 체커 위치에 그라운드체크 원을 그려줌
        Gizmos.DrawWireSphere(groundChecker.position, groundCheckRadius);    

        Gizmos.DrawLine(wallChecker.position, new Vector3(wallChecker.position.x + wallCheckDistance, wallChecker.position.y, wallChecker.position.z));
    }
}
