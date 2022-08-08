using UnityEngine;

namespace Interfaces
{
    public interface IKnockbackable
    {
        // 넉백 함수
        // 넉백 각도, 넉백 힘, 넉백 방향
        void Knockback(float strength, Vector2 angle, int direction);
    }
}
