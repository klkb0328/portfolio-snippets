using System;

namespace PadUI
{
    /// <summary>
    /// 패드 입력의 중앙 처리자.
    /// GamepadEvent를 받으면 현재 최상위 IPadFocusable을 찾아 전달한다.
    /// </summary>
    public class PadUIHandler : IDisposable
    {
        public PadUIHandler()
        {
            MessageSystem.Instance.Subscribe<GamepadEvent>(OnGamePadEvent);
        }

        public void Dispose()
        {
            MessageSystem.Instance.Unsubscribe<GamepadEvent>(OnGamePadEvent);
        }

        public bool OnGamePadEvent(Event e)
        {
            var gpe = e as GamepadEvent;
            if (gpe == null) return false;

            if (TryGetTopPadFocusable(out var focusable, gpe))
            {
                focusable.OnGamePadKeyDown(gpe);
            }

            return false;
        }

        /// <summary>
        /// 현재 가장 위에 보이는 IPadFocusable을 찾는다.
        /// Overlay(팝업), NavigationBar, 현재 씬 순으로 찾는다.
        /// Focusable이 아닌 UI가 올라와 있으면 입력을 무시한다.
        /// </summary>
        public static bool TryGetTopPadFocusable(out IPadFocusable focusable, GamepadEvent gpe)
        {
            // 1. Overlay(=팝업) 체크
            var overlay = UISceneManager.Instance.CurrentOverlay;
            if (overlay is MonoBehaviour and IPadFocusable overlayFocusable)
            {
                focusable = overlayFocusable;
                return true;
            }

            // 2. NavigationBar 체크
            if (NavigationBar.Instance != null && NavigationBar.Instance.gameObject.activeInHierarchy)
            {
                var naviFocusable = NavigationBar.Instance as IPadFocusable;
                if (naviFocusable.ShowPadFocusable)
                {
                    focusable = naviFocusable;
                    return true;
                }
            }

            // 3. 현재 씬 체크
            var scene = UISceneManager.Instance.CurrentScene;
            if (scene is MonoBehaviour and IPadFocusable sceneFocusable)
            {
                focusable = sceneFocusable;
                return true;
            }

            focusable = null;
            return false;
        }
    }
}
