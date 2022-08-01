using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 움직일 수 있는 모든 오브젝트가 참조하는 움직임 코어 컴포넌트
public class Movement : CoreComponent
{
    public int facingDir {get; private set;}
    public Rigidbody2D rb2d {get; private set;}
    public Vector2 curVelocity {get; private set;}
    private Vector2 workspace;

    protected override void Awake()
    {
        base.Awake();

        rb2d = GetComponentInParent<Rigidbody2D>();

        facingDir = 1;
    }

    public void LogicUpdate()
    {
        curVelocity = rb2d.velocity;
    }

    public void CheckFlip(int xInput)
    {
        if (xInput != 0 && xInput != facingDir)
            Flip();
    }

    #region Set Functions
    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, curVelocity.y);
        rb2d.velocity = workspace;
        curVelocity = workspace;
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(curVelocity.x, velocity);
        rb2d.velocity = curVelocity = workspace;
    }

    // 속력뿐만 아니라 방향까지 정하는 함수
    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize(); // 벡터 정규화 필요
        workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        rb2d.velocity = curVelocity = workspace;
    }

    public void SetVelocityZero()
    {
        rb2d.velocity = curVelocity = Vector2.zero;
    }

    // Vector2 방향을 정해주는 함수
    public void SetVelocity(float velocity, Vector2 direction)
    {
        workspace = direction * velocity;
        rb2d.velocity = curVelocity = workspace;
    }

    #endregion
    
    private void Flip()
    {
        facingDir *= -1;
        rb2d.transform.Rotate(0.0f, 180.0f, 0.0f); // movement 클래스가 플레이어의 하위로 내려감에 따라 rigidbody를 거쳐 player의 transform을 건드림
    }
}
