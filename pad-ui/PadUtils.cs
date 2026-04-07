using System.Collections;
using UnityEngine;

namespace PadUI
{
    public static class PadUtils
    {
        /// <summary>
        /// 패드 사용 가능 여부 체크.
        /// 에러 팝업, 튜토리얼, 씬 전환 중에는 패드 입력을 차단한다.
        /// </summary>
        public static bool CheckGamePadEnable()
        {
            if (!Game.Instance.InputManager.GamepadEnabled || ErrorPopUp.Instance.IsRunning ||
                UISceneManager.Instance.CurrentTransition != null || UISceneManager.Instance.IsEventReceiveBlock)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 상하좌우 이동 + A버튼 클릭을 공통으로 처리하는 헬퍼.
        /// 대부분의 IPadFocusable 구현체가 이 메서드를 호출하면 기본 동작이 완성된다.
        /// </summary>
        public static void DefaultGamePadKeyDown(GamepadEvent gpe, IPadFocusable target)
        {
            if (!CheckGamePadEnable()) return;

            switch (gpe.GamepadEventType)
            {
                // 방향키 입력 시 다음 버튼 탐색 + 커서 이동
                case GamepadEventType.LeftStickLeftButtonDown:
                case GamepadEventType.LeftStickRightButtonDown:
                case GamepadEventType.LeftStickUpButtonDown:
                case GamepadEventType.LeftStickDownButtonDown:

                    if (target.PadSelectUI.SelectPadButton != null && target.PadSelectUI.SelectPadButton.gameObject.activeInHierarchy)
                    {
                        var newButton = target.PadSelectUI.SelectPadButton.OnNextSelectableButton(gpe);
                        if (newButton != null)
                        {
                            UpdateSelectPadButton(newButton, target.PadSelectUI);
                        }
                    }
                    break;

                // A버튼 입력 시 현재 선택된 버튼 클릭
                case GamepadEventType.AButtonDown:
                    
                    if (target.PadSelectUI.SelectPadButton != null && target.PadSelectUI.SelectPadButton.gameObject.activeInHierarchy)
                    {
                        target.PadSelectUI.SelectPadButton.OnGamePadKeyDown(gpe);
                    }
                    break;
            }
        }

        /// <summary>
        /// 선택 버튼을 갱신하고 커서 UI를 새 버튼으로 이동시킨다.
        /// 이전 버튼의 커스텀 스프라이트 원복 + 새 버튼의 가이드 설정도 처리.
        /// </summary>
        public static void UpdateSelectPadButton(PadSelectableBase newButton, PadSelectButtonUI target, bool noAnimation = false)
        {
            if (newButton == null) return;
            if (target.SelectPadButton == newButton) return;

            // 이전 버튼 정리
            if (target.SelectPadButton != null && target.SelectPadButton.ButtonSpriteType == PadSelectButtonUI.ButtonSpriteType.Custom)
            {
                // 원복
                target.SelectPadButton.SetCustomButtonSprite(0);
            }

            target.SelectPadButton = newButton;

            // 커서 이동 애니메이션 (필요 시)
            if (!noAnimation)
            {
                // 손가락 이동 애니메이션 ...
            }

            target.SetUI(newButton);
            target.ShowUI(true);
        }
    }
}
