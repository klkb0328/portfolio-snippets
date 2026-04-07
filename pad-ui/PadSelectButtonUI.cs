using UnityEngine;

namespace PadUI
{
    /// <summary>
    /// 현재 선택된 버튼에 테두리/손가락 아이콘을 표시하는 커서 UI.
    /// IPadFocusable이 보유하며, 선택된 PadSelectableBase의 위치/크기에 맞춰 갱신.
    /// </summary>
    public class PadSelectButtonUI : MonoBehaviour
    {
        public enum FingerType
        {
            None = -1,
            Right = 0,
            Center = 1,
            Top = 2,
            LeftCenter = 3,
        }

        public enum ButtonSpriteType
        {
            Custom = -1,
            R8 = 0,
            R22 = 1
        }

        [SerializeField]
        private GameObject container;

        [SerializeField]
        private UISprite selectBoxSprite;

        [SerializeField]
        private UISprite fingerSprite;

        [SerializeField]
        private PadSelectableBase[] initPadSelectable;

        public PadSelectableBase SelectPadButton { get; set; }

        /// <summary>
        /// 패드 연결 시 초기 버튼으로 커서를 세팅한다.
        /// </summary>
        public void InitUI()
        {
            if (Game.Instance.InputManager.GamepadEnabled)
            {
                PadUtils.UpdateSelectPadButton(FindInitBase(), this, noAnimation: true);
            }
            else
            {
                ShowUI(false);
            }
        }

        /// <summary>
        /// 선택된 버튼의 위치/크기에 맞춰 커서 UI를 갱신한다.
        /// 커스텀 타입이면 버튼 자체의 스프라이트를 변경한다.
        /// </summary>
        public void SetUI(PadSelectableBase baseSelector)
        {
            container.transform.position = baseSelector.transform.position;
            container.transform.localPosition += baseSelector.positionOffset;

            if (baseSelector.ButtonSpriteType == ButtonSpriteType.Custom)
            {
                selectBoxSprite.gameObject.SetActive(false);
                baseSelector.SetCustomButtonSprite(1);
            }
            else
            {
                selectBoxSprite.gameObject.SetActive(true);
                selectBoxSprite.width = baseSelector.Width + 10 + (int)baseSelector.sizeOffset.x;
                selectBoxSprite.height = baseSelector.Height + 10 + (int)baseSelector.sizeOffset.y;
            }

            // 손가락 아이콘 위치 설정 생략
        }

        public void ShowUI(bool isOn) => container.SetActive(isOn);

        /// <summary>
        /// initPadSelectable 배열에서 활성화된 첫 번째 버튼을 찾는다.
        /// </summary>
        public PadSelectableBase FindInitBase()
        {
            foreach (var item in initPadSelectable)
            {
                if (item != null && item.gameObject.activeInHierarchy)
                    return item;
            }

            return initPadSelectable?[0];
        }

        // ... 애니메이션, 이벤트 구독 등 생략
    }
}
