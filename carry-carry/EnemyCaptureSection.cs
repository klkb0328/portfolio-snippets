using UnityEngine;

namespace Defense
{
    /// <summary>
    /// Section 예시며, 기타 다른것보다 그냥 스타일 상 구분이라 따로 제약을 강하게 둔다거나 하진 않는다.
    /// EnemyController 전용 중력장 등에 의해 "캡처"상태라 부르는 (끌려간 상태라 보면 됩니다.) 처리만 담은 Section.
    /// 순수하게 코드 정리 전용이며, 강제성은 없으며 partial class 대신 별도 클래스로 나눈 이유는 순전히 개인적으로 같은 이름의 파일이 여러 개면 검색 시 불편하기 때문.
    /// </summary>
    public sealed class EnemyCaptureSection
    {
        private readonly EnemyController controller;

        public EnemyCaptureSection(EnemyController ctr)
        {
            controller = ctr;
        }

        /// <summary>
        /// 캡처 게이지 증가. 최대치 도달 시 캡처 상태로 전환.
        /// </summary>
        public void AddCaptureGauge(float amount)
        {
            if (controller.IsCaptured) return;
            if (controller.RemainLife <= 0f) return;

            controller.CurrentCaptureGauge += amount;

            if (controller.CurrentCaptureGauge >= controller.MaxCaptureGauge)
            {
                SetCaptured();
            }
        }

        private void SetCaptured()
        {
            controller.IsCaptured = true;
            // 캡처 연출 재생 ...
        }

        /// <summary>
        /// 캡처 해제 (상태 정리 + 연출)
        /// </summary>
        public void SetRelease()
        {
            controller.IsCaptured = false;
            // 릴리즈 연출 재생 ...
        }

        /// <summary>
        /// 중력 영역 진입/이탈 처리
        /// </summary>
        public void EnterGravityArea() { /* ... */ }
        public void ExitGravityArea() { /* ... */ }
    }
}
