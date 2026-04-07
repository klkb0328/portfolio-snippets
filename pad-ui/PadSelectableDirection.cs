using System.Collections.Generic;
using UnityEngine;

namespace PadUI
{
    /// <summary>
    /// 방향 기반 포커스 네비게이션 구현체.
    /// 인스펙터에서 상하좌우 방향별로 다음 버튼을 연결한다.
    /// 활성화된 첫 번째 버튼을 찾아 이동하며, 이동할 곳이 없으면 쉐이킹 연출.
    /// </summary>
    public class PadSelectableDirection : PadSelectableBase
    {
        public enum SelectableDirection
        {
            Top = 0,
            Bottom = 1,
            Left = 2,
            Right
        }

        [Header("방향별 다음 버튼 연결 (인스펙터)")]
        [SerializeField] private List<PadSelectableBase> topDefaultList;
        [SerializeField] private List<PadSelectableBase> bottomDefaultList;
        [SerializeField] private List<PadSelectableBase> leftDefaultList;
        [SerializeField] private List<PadSelectableBase> rightDefaultList;

        [SerializeField] private GameObject shakeTarget;

        public override void OnGamePadKeyDown(GamepadEvent gpe)
        {
            if (!PadUtils.CheckGamePadEnable()) return;

            switch (gpe.GamepadEventType)
            {
                case GamepadEventType.AButtonDown:
                {
                    // A버튼 입력 시 현재 선택된 버튼 클릭
                    if (isButton)
                        EventDelegate.Execute(currentButton.onClick);
                    break;
                }
            }
        }

        /// <summary>
        /// 방향키 입력 시 해당 방향의 활성화된 첫 번째 버튼을 반환.
        /// null이면 이동할 곳이 없으므로 쉐이킹 연출 재생.
        /// </summary>
        public override PadSelectableBase OnNextSelectableButton(GamepadEvent gpe)
        {
            if (!PadUtils.CheckGamePadEnable()) return null;

            PadSelectableBase target = null;

            switch (gpe.GamepadEventType)
            {
                case GamepadEventType.LeftStickLeftButtonDown:
                case GamepadEventType.LeftStickRightButtonDown:
                case GamepadEventType.LeftStickUpButtonDown:
                case GamepadEventType.LeftStickDownButtonDown:
                    target = FindDirectionButton(gpe.GamepadEventType);
                    break;
            }

            // 이동할 곳이 없으면 쉐이킹 연출
            if (target == null && shakeTarget != null)
            {
                StartShakeAnimation(gpe.GamepadEventType);
            }

            return target;
        }

        /// <summary>
        /// 입력 방향에 맞는 버튼 리스트에서 활성화된 첫 번째를 찾아 반환.
        /// 리스트에 여러 개 연결되어 있으면 순서대로 탐색하여 fallback 처리.
        /// </summary>
        public PadSelectableBase FindDirectionButton(GamepadEventType gpe)
        {
            PadSelectableBase target = null;

            List<PadSelectableBase> searchList = gpe switch
            {
                GamepadEventType.LeftStickLeftButtonDown => leftDefaultList,
                GamepadEventType.LeftStickRightButtonDown => rightDefaultList,
                GamepadEventType.LeftStickUpButtonDown => topDefaultList,
                GamepadEventType.LeftStickDownButtonDown => bottomDefaultList,
                _ => null
            };

            if (searchList == null) return null;

            for (int i = 0; i < searchList.Count; i++)
            {
                if (searchList[i] != null && searchList[i].gameObject.activeInHierarchy)
                {
                    target = searchList[i];
                    break;
                }
            }

            return target;
        }

        // ... 쉐이킹 연출, Swap 기능 등 생략

        public override void SetCustomButtonSprite(int stateIndex) { /* ... */ }
    }
}
