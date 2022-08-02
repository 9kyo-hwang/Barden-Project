using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D Rb2d { get; private set; }
    public Vector2 CurVelocity { get; private set; }
    public int FacingDir { get; private set; }
    private Vector2 workspace;

    protected override void Awake()
    {
        base.Awake();

        Rb2d = GetComponentInParent<Rigidbody2D>();

        FacingDir = 1;
    }

    public void LogicUpdate()
    {
        CurVelocity = Rb2d.velocity;
    }
    
    #region Set Functions
    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, CurVelocity.y);
        Rb2d.velocity = workspace;
        CurVelocity = workspace;
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(CurVelocity.x, velocity);
        Rb2d.velocity = CurVelocity = workspace;
    }

    // 속력뿐만 아니라 방향까지 정하는 함수
    public void SetVelocity(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize(); // 벡터 정규화 필요
        workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        Rb2d.velocity = CurVelocity = workspace;
    }

    public void SetVelocityZero()
    {
        Rb2d.velocity = CurVelocity = Vector2.zero;
    }

    // Vector2 방향을 정해주는 함수
    public void SetVelocity(float velocity, Vector2 direction)
    {
        workspace = direction * velocity;
        Rb2d.velocity = CurVelocity = workspace;
    }
    #endregion
    
    public void CheckFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDir)
            Flip();
    }
    
    private void Flip()
    {
        FacingDir *= -1;
        // 움직임을 담당하는 코드가 플레이어의 하위 오브젝트로 내려옴에 따라
        // movement object의 transform이 아닌 Player의 Rigidbody2D Component를 거쳐 transform을 받아오도록 변경
        Rb2d.transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}
