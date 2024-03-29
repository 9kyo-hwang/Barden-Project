using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : CoreComponent
{
    public Rigidbody2D Rb2d { get; private set; }
    public int FacingDir { get; private set; }
    public bool CanSetVelocity { get; set; }
    public Vector2 CurrentVelocity { get; private set; }
    private Vector2 workspace;

    protected override void Awake()
    {
        base.Awake();

        Rb2d = GetComponentInParent<Rigidbody2D>();

        FacingDir = 1;

        CanSetVelocity = true;
    }

    public override void LogicUpdate()
    {
        CurrentVelocity = Rb2d.velocity;
    }
    
    #region Set Functions
    public void SetVelocityX(float velocity)
    {
        workspace.Set(velocity, CurrentVelocity.y);
        SetVelocityFinal();
    }

    public void SetVelocityY(float velocity)
    {
        workspace.Set(CurrentVelocity.x, velocity);
        SetVelocityFinal();
    }
    
    public void SetVelocityZero()
    {
        workspace.Set(0, 0);
        SetVelocityFinal();
    }

    // Vector2 방향을 정해주는 함수
    public void SetVelocityAngle(float velocity, Vector2 angle)
    {
        workspace.Set(angle.x * velocity, angle.y * velocity);
        SetVelocityFinal();
    }
    
    // Velocity, Angle, x축 Direction까지 정하는 함수
    public void SetVelocityDirection(float velocity, Vector2 angle, int direction)
    {
        angle.Normalize(); // 벡터 정규화 필요
        workspace.Set(angle.x * velocity * direction, angle.y * velocity);
        SetVelocityFinal();
    }
    
    // Velocity 변경을 최종적으로 적용시키는 함수
    public void SetVelocityFinal()
    {
        // Velocity를 적용시킬 수 있을 때에만 적용
        if (CanSetVelocity)
        {
            Rb2d.velocity = CurrentVelocity = workspace;
        }
    }
    #endregion
    
    public void CheckFlip(int xInput)
    {
        if (xInput != 0 && xInput != FacingDir)
            Flip();
    }
    
    public void Flip()
    {
        FacingDir *= -1;
        // 움직임을 담당하는 코드가 플레이어의 하위 오브젝트로 내려옴에 따라
        // movement object의 transform이 아닌 Player의 Rigidbody2D Component를 거쳐 transform을 받아오도록 변경
        Rb2d.transform.Rotate(0.0f, 180.0f, 0.0f);
    }
}
