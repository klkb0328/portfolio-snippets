using UnityEngine;
using System.Collections.Generic;
using Defense.Core;

namespace Defense
{
    /// <summary>
    /// 투사체 본체.
    /// IMovementLogic(이동) + IHitLogic(판정) 등 을 프리팹에 붙여놓으면
    /// Projectile이 Awake에서 자동 발견하여 조율한다.
    /// List인 이유는 뭐 같은 범주라 하더라도 여러개 붙을수 있음. 예를들면 예시코드에는
    /// 직선이동만 처리했는데, 곡사 이동까지 붙이면 직선이동하면서 곡사로 움직임.
    ///
    /// 예시 프리팹 구성
    /// 
    ///   Projectile
    ///     + StraightMovement    (IMovementLogic: 직선 이동)
    ///     + StandardHitLogic    (IHitLogic: 단일 타격 판정)
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    public class Projectile : MonoBehaviour
    {
        [SerializeField] 
        private float maxLifetime = 15.0f;

        [SerializeField]
        private bool isToward = true;

        public IProjectileOwner Owner { get; private set; }
        public Vector2 InitialDirection { get; private set; }
        public bool IsToward => isToward;
        public Rigidbody2D Rb { get; private set; }
        public float ElapsedTime { get; private set; }
        public float MaxLifetime { get; set; }
        public float NormalizedTime => MaxLifetime > 0 ? ElapsedTime / MaxLifetime : 0f;

        #region Logics

        /// <summary>
        /// 충돌 처리용 로직들.
        /// </summary>
        private List<IHitLogic> hits = new();

        /// <summary>
        /// 이동 처리용 로직들.
        /// </summary>
        private List<IMovementLogic> movements = new();

        #endregion

        private bool isActive;

        /// <summary>
        /// Awake에서 GetComponents로 컴포넌트를 자동 수집한다.
        /// 프리팹에 원하는 조합을 붙여놓기만 하면 동작.
        /// </summary>
        private void Awake()
        {
            Rb = GetComponent<Rigidbody2D>();

            // 컴포넌트 자동 수집
            hits.AddRange(GetComponents<IHitLogic>());
            movements.AddRange(GetComponents<IMovementLogic>());
        }

        /// <summary>
        /// 풀에서 꺼낸 뒤 호출. 런타임 데이터 주입 + 하위 컴포넌트 초기화.
        /// </summary>
        public void Initialize(IProjectileOwner owner, TargetSnapshot snapshot)
        {
            Owner = owner;
            InitialDirection = snapshot.Direction;
            isActive = true;
            ElapsedTime = 0f;
            MaxLifetime = maxLifetime;

            // 하위 컴포넌트 초기화 . 각자 Projectile 참조를 받아 세팅
            foreach (var hit in hits)
            {
                hit.OnInitialize(this);
            }

            foreach (var move in movements)
            {
                move.OnInitialize(this);
            }
        }

        /// <summary>
        /// 모든 Movement의 계산 결과를 합산하여 한 번에 적용.
        /// </summary>
        private void FixedUpdate()
        {
            if (!isActive) return;
            float dt = Time.fixedDeltaTime;
            ElapsedTime += dt;

            // 1. 수명 체크
            if (ElapsedTime >= MaxLifetime)
            {
                RequestDespawn(DespawnReason.TimeExpired);
                return;
            }

            // 2. HitLogic 업데이트
            for (int i = 0; i < hits.Count; i++)
            {
                hits[i].OnUpdate(dt);
            }

            // 3. Movement 합산. 각 Movement가 계산만 하고, 적용은 여기서
            var nextPos = Rb.position;
            var nextRot = transform.rotation;

            for (int i = 0; i < movements.Count; i++)
            {
                movements[i].UpdateMovement(dt, ref nextPos, ref nextRot);
            }

            // 4. 최종 물리 적용
            Rb.MovePosition(nextPos);
            Rb.MoveRotation(nextRot.eulerAngles.z);
        }

        /// <summary>
        /// 충돌 감지. 각 HitLogic에 위임하며 레이어 판단은 Logic이 자체적으로 수행한다.
        /// </summary>
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isActive) return;

            foreach (var hit in hits)
            {
                 hit.OnProjectileTriggerEnter(other);
            }
        }

        public void RequestDespawn(DespawnReason reason)
        {
            if (!isActive) return;

            isActive = false;
            PoolManager.Instance?.Return(gameObject);
        }
    }
}
