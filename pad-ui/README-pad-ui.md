Pad UI (포커스 시스템)

가디언 테일즈의 기존 가상 커서 방식에서, 스위치 스타일의 게임패드 UI 포커스 시스템을 추가한 작업의 코드 스니펫입니다.

- 구조

- PadUIHandler
입력의 중앙 처리자로, 최상위 IPadFocusable을 찾아 입력을 전달합니다.

- IPadFocusable
씬 또는 팝업이 구현하는 포커스 인터페이스입니다.

- PadSelectableDirection
방향별 다음 버튼을 인스펙터에서 연결하여 포커스 이동을 처리합니다.

- PadSelectButtonUI
씬 또는 팝업이 하나씩 가지고 있으며 그 UI 안에서 사용되는 포커스 UI 입니다.

- 입력 흐름

GamepadEvent -> PadUIHandler -> 최상위 IPadFocusable -> PadSelectableDirection -> 다음 버튼 탐색

- 파일 구성

- PadUIHandler.cs : 입력 중앙 처리 및 최상위 포커스 탐색
- IPadFocusable.cs : 포커스 대상 인터페이스
- PadSelectableBase.cs : 선택 가능 UI의 기본 클래스
- PadSelectableDirection.cs : 방향 기반 네비게이션 구현
- PadSelectButtonUI.cs : 포커스 표시 UI (테두리 및 아이콘)
- PadUtils.cs : 공통 유틸리티 (입력 처리 등)