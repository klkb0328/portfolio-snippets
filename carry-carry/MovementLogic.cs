using UnityEngine;
using Defense.Core;

namespace Defense
{
    /// <summary>
    /// 이동 Logic 예시코드. 투사체 이동의 공용 기반 클래스.
    /// IMovementLogic을 구현하여 Projectile이 자동 발견할 수 있다.
    /// </summary>
    public abstract class BaseMovement : MonoBehaviour, IMovementLogic
    {
        protected Projectile projectile;

        [SerializeField]
         protected float speed = 10f;

        [SerializeField] 
        protected float initialAddAngle = 0f;

        [SerializeField]
         protected AnimationCurve speedCurve = AnimationCurve.Constant(0, 1, 1);

        public virtual void OnInitialize(Projectile host)
        {
            projectile = host;

            if (initialAddAngle != 0)
            {
                host.transform.rotation *= Quaternion.Euler(0, 0, initialAddAngle);
            }
        }

        /// <summary>
        /// 이동 계산만 수행. 실제 적용은 Projectile에서 처리.
        /// </summary>
        public abstract void UpdateMovement(float dt, ref Vector2 position, ref Quaternion rotation);

        protected float GetCurrentSpeed()
        {
            return speed * speedCurve.Evaluate(projectile.NormalizedTime);
        }
    }

    /// <summary>
    /// 직선 이동 Logic. BaseMovement를 상속하여 방향 기반 직선 이동을 구현.
    /// 예시코드 여서 따로 소스파일을 나누진 않음.
    /// </summary>
    public class StraightMovement : BaseMovement
    {
        public override void UpdateMovement(float dt, ref Vector2 position, ref Quaternion rotation)
        {
            if (projectile.IsToward)
            {
                position += (Vector2)((rotation * Vector3.up) * GetCurrentSpeed() * dt);
            }
            else
            {
                Vector2 dir = projectile.InitialDirection;
                position += dir.normalized * GetCurrentSpeed() * dt;
            }
        }
    }
}
