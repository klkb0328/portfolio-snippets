using UnityEngine;
using Defense.Core;
using Common.Messaging;

namespace Defense
{
    /// <summary>
    /// Module 예시 코드며 플레이어 전용 중력장 (대표적인 흡입 모듈) 모듈.
    /// 프리팹 기반으로 PlayerController의 자식 오브젝트에 붙여 사용한다.
    /// 시각적 실체(영역 표시, 에너지 잔량)가 있어서 프리팹이 필요했고,
    /// 비슷한 방식의 다른 흡입 요소가 추가될 수 있어 모듈이란 개념이 만들어짐.
    /// </summary>
    public class GravityArea : MonoBehaviour, IModule<PlayerController>
    {
        private PlayerController playerController;

        [SerializeField] 
        private CircleCollider2D circleCollider;

        [SerializeField]
        private Transform areaScaler;

        private float gravityEnergyCurrent;

        private bool attractiveForce;


        #region IModule Implementation

        /// <summary>
        /// PlayerController를 받아 초기화.
        /// </summary>
        public void Initialize(PlayerController host)
        {
            playerController = host;
            ResetState();
        }

        /// <summary>
        /// Host가 Update에서 호출. 중력장은 FixedTick에서만 동작.
        /// </summary>
        public void Tick(float deltaTime)
        {
            // 중력장은 물리 타이밍에서만 동작
        }

        /// <summary>
        /// Host가 FixedUpdate에서 호출. 에너지 소모 + 중력 적용.
        /// </summary>
        public void FixedTick(float fixedDeltaTime)
        {
            if (!attractiveForce) return;

            // 시간 기반 고정 에너지 소모
            AddEnergy(-EnergyDrainPerSecond * fixedDeltaTime);

            // 범위 내 적에게 중력 적용 + 캡처 처리
            ApplyGravityAndCapture(fixedDeltaTime);

            // 에너지 고갈 체크
            if (gravityEnergyCurrent <= 0f)
                Deactivate();
        }

        #endregion

        public void Activate()
        {
            if (attractiveForce) return;
            attractiveForce = true;
            // 에너지 게이지 연출 시작 ...
        }

        public void Deactivate()
        {
            if (!attractiveForce) return;
            attractiveForce = false;
            gravityEnergyCurrent = MaxGravityEnergy; // 즉시 충전
            // 타겟 해제 + 연출 정리 ...
        }

        private void ResetState()
        {
            gravityEnergyCurrent = MaxGravityEnergy;
            attractiveForce = false;
        }

        // ... 중력 적용, 캡처 처리, 에너지 관리 등 생략
    }
}
