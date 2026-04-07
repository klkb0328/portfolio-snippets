using UnityEngine;

namespace PadUI
{
    /// <summary>
    /// 패드로 선택 가능한 UI 버튼의 베이스.
    /// 실제 테두리 표시는 하지 않고, 방향키 입력 시 다음 버튼 정보를 제공한다.
    /// </summary>
    public abstract class PadSelectableBase : MonoBehaviour
    {
        [SerializeField] 
        protected bool isButton = true;
        
        [SerializeField]
        public Vector3 positionOffset;
        
        [SerializeField]
        public Vector2 sizeOffset;

        [SerializeField]
        public PadSelectButtonUI.FingerType fingerType;
       
        [SerializeField]
        public PadSelectButtonUI.ButtonSpriteType buttonSpriteType;

        public PadSelectButtonUI.FingerType FingerType => fingerType;

        public PadSelectButtonUI.ButtonSpriteType ButtonSpriteType => buttonSpriteType;

        public int Width { get; protected set; }
        
        public int Height { get; protected set; }
        
        protected UIButton currentButton;

        /// <summary>
        /// 패드 키 입력 시 동작 처리 (A버튼 클릭 등)
        /// </summary>
        public abstract void OnGamePadKeyDown(GamepadEvent keyDown);

        /// <summary>
        /// 방향키 입력 시 다음으로 이동할 버튼을 반환
        /// </summary>
        public abstract PadSelectableBase OnNextSelectableButton(GamepadEvent keyDown);

        /// <summary>
        /// 커스텀 타입일 때 버튼 스프라이트 변경
        /// </summary>
        public abstract void SetCustomButtonSprite(int stateIndex);
    }
}
