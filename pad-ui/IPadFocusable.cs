using System.Collections.Generic;

namespace PadUI
{
    /// <summary>
    /// 패드 포커스를 받을 수 있는 씬/팝업에 붙이는 인터페이스.
    /// PadUIHandler가 현재 최상위 IPadFocusable을 찾아 입력을 전달한다.
    /// </summary>
    public interface IPadFocusable
    {
        /// <summary>
        /// 현재 포커스된 버튼을 시각적으로 보여주는 커서 UI.
        /// 씬이나 팝업에 해당 ui가 하나는 들어있어야 함.
        /// </summary>
        PadSelectButtonUI PadSelectUI { get; }

        /// <summary>
        /// 패드 키 입력 처리. PadUIHandler가 최상위 Focusable을 찾은 뒤 호출한다.
        /// 개별 씬/팝업이 GamepadEvent를 직접 구독할 필요가 없다.
        /// </summary>
        void OnGamePadKeyDown(GamepadEvent gpe);

        /// <summary>
        /// 포커스 표시 여부. 특정 상황에서만 보여줘야 할 때 사용.
        /// </summary>
        bool ShowPadFocusable { get; }

        List<PadSelectableBase> GetPadSelectables();
    }
}
