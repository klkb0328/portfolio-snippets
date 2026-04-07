using UnityEngine;
using System.Collections.Generic;
using Defense.Core;
using Defense.Systems;

namespace Defense
{
    /// <summary>
    /// 충돌 판정 Logic의 공용 기반 클래스.
    /// </summary>
    public abstract class BaseHitLogic : MonoBehaviour, IHitLogic
    {
        protected Projectile projectile;

        [SerializeField] 
        protected LayerMask attackLayer;

        [SerializeField]
         protected float damageRatio = 1.0f;

        [SerializeField]
         protected int maxHitCount = 1;

        [SerializeField] 
        protected float pushTime = 0.3f;

        [SerializeField] 
        protected float pushSpeed = 3f;

        public virtual void OnInitialize(Projectile host)
        {
            projectile = host;
        }

        public virtual void OnUpdate(float dt) { }

        public abstract void OnProjectileTriggerEnter(Collider2D other);
    }

    /// <summary>
    /// 표준 타격 Logic.
    /// maxHitCount로 관통 여부를 제어한다.
    /// 예시코드여서 따로 소스파일을 나누진 않음.
    /// </summary>
    public class StandardHitLogic : BaseHitLogic
    {
        private int currentHitCount;

        /// <summary>
        /// 충돌시 관통할때마다 데미지 입히기 방지용.
        /// </summary>
        private HashSet<int> insideColliders = new();

        public override void OnInitialize(Projectile host)
        {
            base.OnInitialize(host);
            currentHitCount = maxHitCount;
            insideColliders.Clear();
        }

        public override void OnProjectileTriggerEnter(Collider2D other)
        {
            if (((1 << other.gameObject.layer) & attackLayer) == 0) return;

            // 같은 콜라이더 재진입 방지.
            // ObjectId가 아닌 GetInstanceID()를 쓰는 이유는 ObjectId는 게임 단위(적 유닛) 식별용이고
            // 여기선 콜라이더를 체크하는 용이다.
            int id = other.GetInstanceID();

            // 같은 콜라이더 id면 무시.
            if (!insideColliders.Add(id)) return;

            // Signal, System, Delta, Apply 순서로 처리
            var damageable = other.GetComponent<IDamageable>();
            if (damageable != null)
            {
                var dir = (other.transform.position - projectile.transform.position).normalized * pushSpeed;
                float baseAtk = projectile.Owner?.GetTotalDamage() ?? 0f;

                var signal = new DamageSignal(ObjectId.None, ObjectId.None, baseAtk, damageRatio, pushTime, dir);
                var delta = DamageSystem.Evaluate(signal);
                
                // 여기서 실제 데미지 처리
                damageable.TakeDamage(delta);
            }

            // 타격 횟수 소진 시 소멸 요청
            currentHitCount--;
            if (currentHitCount <= 0)
            {
                projectile.RequestDespawn(DespawnReason.LifeExhausted);
            }
        }
    }
}
