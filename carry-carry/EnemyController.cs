using UnityEngine;
using Defense.Core;

namespace Defense
{
    /// <summary>
    /// Controller 예시
    /// 적 유닛의 고유 로직을 소유하는 현장 담당자며 위로 올라가면서 성(Castle)을 공격한다.
    /// </summary>
    public class EnemyController : MonoBehaviour, IDamageable, IController<EnemyContext>
    {
        private WallController cachedWall;

        public ObjectId ObjectId { get; private set; }


        /// <summary>
        /// 끌려가는 처리를 담은 코드(원래는 여기에 그대로 있었음). 재사용 목적이 아니라 파일 분리용.
        /// </summary>
        private EnemyCaptureSection captureSection;

        private void Awake()
        {
            captureSection = new EnemyCaptureSection(this);
        }

        /// <summary>
        /// 필요한 정보를 받아서 캐시해둔다.
        /// </summary>
        public void Initialize(EnemyContext context)
        {
            cachedWall = context.WallController;
            ObjectId = context.ObjectId;
        }

        public void ResetState()
        {
            // 풀 복귀 시 상태 정리 (HP바, 이펙트 등)
        }

        /// <summary>
        /// Section에 캡처 게이지 처리를 위임하는 예시 코드.
        /// </summary>
        public void AddCaptureGauge(float amount) => captureSection.AddCaptureGauge(amount);
    }

    /// <summary>
    /// EnemyController 전용 초기화 컨텍스트.
    /// </summary>
    public class EnemyContext : IContext
    {
        public WallController WallController;
        public ObjectId ObjectId;
    }
}
