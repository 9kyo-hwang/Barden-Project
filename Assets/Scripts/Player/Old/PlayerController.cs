using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float moveInputDir; // 입력한 방향키의 방향을 반환하는 변수
    private int facingDir = 1; // 캐릭터 좌, 우 방향을 나타내기 위한 변수
    private int amountOfJumpsLeft; // 남은 점프 가능 횟수를 나타내는 int형 변수
    private float jumpTimer;
    private float turnTimer;
    private float wallJumpTimer;
    private int lastWallJumpDir;
    
    private bool isFacingRight = true; // 현재 캐릭터가 바라보는 방향이 오른쪽인 지 체크하는 boolean 변수
    private bool isWalking; // 현재 캐릭터가 걷고있는 지 체크하는 boolean 변수
    private bool isGrounded; // 현재 캐릭터가 땅 위에 있는 지 체크하는 boolean 변수
    private bool isTouchingWall; // 현재 캐릭터가 벽에 닿아 있는 지 체크하는 boolean 변수
    private bool isWallSliding; // 현재 캐릭터가 벽 미끄러지기를 하고 있는 지 체크하는 boolean 변수
    private bool canNormalJump; // 현재 캐릭터가 점프할 수 있는 상태인 지 나타내는 boolean 변수
    private bool canWallJump; // 현재 캐릭터가 벽점프를 할 수 있는 상태인 지 나타내는 boolean 변수
    private bool isAttemptingToJump;
    private bool checkJumpMultiplier;
    private bool canMove;
    private bool canFlip;
    private bool hasWallJumped;

    public float moveSpeed = 10.0f; // 이동 속도를 정의하는 변수
    public float jumpForce = 16.0f; // 점프력을 정의하는 변수
    public float jumpTimerSet = 0.15f;
    public float wallJumpForce; // 벽 매달리다가 반대쪽으로 점프할 때 부여되는 힘
    public int amountOfJumps = 2; // 점프 가능한 총 횟수를 나타내는 int형 변수
    public float airDragMultiplier = 0.95f; // 공중에서 가만히 떨어질 때 캐릭터 y축 속력 벡터에 곱해지는 가중치 
    public float variableJumpHeightMultiplier = 0.5f; // 점프 높이 조절을 위한 가중치
    public float groundCheckRadius; // 땅 체크를 위해 그려지는 원의 반지름 길이
    public float wallCheckDistance; // 벽 체크를 위해 쏘는 Ray의 길이
    public float wallSlideSpeed; // 벽 미끄러지기 속도를 정의하는 변수
    public float turnTimerSet = 0.1f;
    public float wallJumpTimerSet = 0.5f;

    private Rigidbody2D rb2d; // Rigidbody2D를 불러오기 위한 변수
    private Animator anim; // Animator를 불러오기 위한 변수
    public Transform groundChecker; // GroundChecker 오브젝트를 담기 위한 변수
    public Transform wallChecker; // WallChecker 오브젝트를 담기 위한 변수
    public LayerMask whatIsGround; // 땅 레이어를 담기 위한 레이어마스크 변수
    public Vector2 wallJumpDir; // 벽 매달리다가 반대쪽으로 점프하는 방향

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>(); // Rigidbody2D 컴포넌트 불러옴
        anim = GetComponent<Animator>(); // Animator 컴포넌트 불러옴
        amountOfJumpsLeft = amountOfJumps; // 남은 점프 가능 횟수 초기화
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
        CheckJump();
    }

    void CheckInput()
    {
        // Horizontal 입력 축 값을 반환함
        // -1: Left, 0: None, 1: Right
        moveInputDir = Input.GetAxisRaw("Horizontal");

        // Jump에 배당된 버튼 키 다운 시 Jump 수행
        if (Input.GetButtonDown("Jump"))
        {
            if(isGrounded || (amountOfJumpsLeft > 0 && isTouchingWall))
                NormalJump();
            else
            {
                jumpTimer = jumpTimerSet;
                isAttemptingToJump = true;
            }
        }

        if (Input.GetButtonDown("Horizontal") && isTouchingWall)
        {
            if (!isGrounded && moveInputDir != facingDir)
            {
                canMove = false;
                canFlip = false;

                turnTimer = turnTimerSet;
            }
        }

        if (!canMove)
        {
            turnTimer -= Time.deltaTime;
            if (turnTimer <= 0)
            {
                canMove = true;
                canFlip = true;
            }
        }

        // checkJumpMultiplier 이면서 점프 버튼을 누른 게 아니라면
        // 키 업 시점이 빠를 수록 y축 속력 벡터 값이 크므로 감소되는 값도 비례해 커짐
        // 즉 키 업을 빨리할 수록 점프 높이가 낮아짐
        if (checkJumpMultiplier && !Input.GetButton("Jump"))
        {
            checkJumpMultiplier = false;
            
            var velocity = rb2d.velocity;
            velocity = new Vector2(velocity.x, velocity.y * variableJumpHeightMultiplier);
            rb2d.velocity = velocity;
        }
    }
    
    void CheckMoveDir()
    {
        // 현재 바라보는 방향과 입력한 방향키의 방향이 반대일 경우 뒤집는 함수 수행
        if((isFacingRight && moveInputDir < 0) || (!isFacingRight && moveInputDir > 0))
            Flip();
        
        // 캐릭터의 x축 속력 벡터가 0이 아니면 Walk, 아니라면 Idle
        isWalking = rb2d.velocity.x != 0;
    }

    void CheckJump()
    {
        if (jumpTimer > 0)
        {
            // WallJump
            if (!isGrounded && isTouchingWall && moveInputDir != 0 && moveInputDir != facingDir)
                WallJump();
            else if (isGrounded)
                NormalJump();
        }
        
        if(isAttemptingToJump)
            jumpTimer -= Time.deltaTime;

        if (wallJumpTimer > 0)
        {
            if (hasWallJumped && moveInputDir == -lastWallJumpDir)
            {
                rb2d.velocity = new Vector2(rb2d.velocity.x, 0.0f);
                hasWallJumped = false;
            }
            else if (wallJumpTimer <= 0)
            {
                hasWallJumped = false;
            }
            else
            {
                wallJumpTimer -= Time.deltaTime;
            }
        }
    }
    
    void CheckIfCanJump()
    {
        // 땅에 있으면서 캐릭터의 y축 속력 벡터가 0.01 이하일 경우
        // 점프 가능한 남은 횟수 초기화
        if(isGrounded && rb2d.velocity.y <= 0.01f)
            amountOfJumpsLeft = amountOfJumps;

        // 벽에 닿아있는 상태라면 canWallJump = true
        if (isTouchingWall)
            canWallJump = true;

        // 남은 점프 횟수가 0보다 크면 점프 가능, 아니라면 불가능.
        canNormalJump = amountOfJumpsLeft > 0;
    }
    
    void CheckIfWallSliding()
    {
        // 벽에 닿았으면서 키보드 입력 방향이 바라보는 방향과 같고 캐릭터가 내려가는 중이라면
        if(isTouchingWall && moveInputDir == facingDir && rb2d.velocity.y < 0)
            isWallSliding = true;
        else
            isWallSliding = false;
    }

    void NormalJump()
    {
        // !canNormalJump이면 불가능
        if (!canNormalJump) return;
        
        // y축 속력 벡터를 설정된 점프 힘만큼 부여
        // 남은 점프 가능 횟수 1 감소
        rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
        amountOfJumpsLeft--;
        jumpTimer = 0;
        isAttemptingToJump = false;
        checkJumpMultiplier = true;
    }

    void WallJump()
    {
        // !canWallJump이면 불가능
        if (!canWallJump) return;

        // wallJumpForce만큼 입력한 방향으로 힘을 가함
        rb2d.velocity = new Vector2(rb2d.velocity.x, 0.0f);
        isWallSliding = false;
        amountOfJumpsLeft = amountOfJumps;
        amountOfJumpsLeft--;
        Vector2 forceToAdd = new Vector2(wallJumpForce * wallJumpDir.x * moveInputDir, wallJumpForce * wallJumpDir.y);
        rb2d.AddForce(forceToAdd, ForceMode2D.Impulse);
        jumpTimer = 0;
        isAttemptingToJump = false;
        checkJumpMultiplier = true;
        turnTimer = 0;
        canMove = true;
        canFlip = true;
        hasWallJumped = true;
        wallJumpTimer = wallJumpTimerSet;
        lastWallJumpDir = -facingDir;
    }

    void Flip()
    {
        // 벽 슬라이딩 상태가 아니면서 canFlip == true이면 Flip 가능
        if (isWallSliding || !canFlip) return;
        
        facingDir *= -1; // -1을 곱해서 정방향에 맞게 값 수정
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
            var velocity = rb2d.velocity;
            velocity = new Vector2(velocity.x * airDragMultiplier, velocity.y);
            rb2d.velocity = velocity;
        }
        // 땅에 있는 경우 입력받은 방향과 이동 속도에 따른 x축 속력 벡터 갱신
        else if(canMove)
        {
            var velocity = rb2d.velocity;
            velocity = new Vector2(moveSpeed * moveInputDir, velocity.y);
            rb2d.velocity = velocity;
        }

        // 벽 미끄러지기 중이면서 미끄러지는 속도가 wallSlideSpeed보다 클 경우
        if(isWallSliding && rb2d.velocity.y < -wallSlideSpeed)
        {
            rb2d.velocity = new Vector2(rb2d.velocity.x, -wallSlideSpeed);
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

        var position = wallChecker.position;
        Gizmos.DrawLine(position, new Vector3(position.x + wallCheckDistance, position.y, position.z));
    }
}
