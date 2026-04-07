CarryCarry

모바일 디펜스 게임 캐리캐리의 뼈대 구조를 구성하며 작성한 코드 스니펫입니다.
각 역할을 분리하고, 각 요소가 자신의 범위 내에서 동작하도록 구성했습니다.

- 역할 구분

- Controller
씬에 존재하는 오브젝트의 진입점으로, Context를 받아 초기화하고 결과를 적용합니다.
(EnemyController.cs)

- Module
Controller에 종속된 프리팹 기반 기능 단위입니다.
(GravityArea.cs)

- Logic
대분류 범위 내에서 재사용 가능한 기능 단위입니다.
(MovementLogic.cs, HitLogic.cs)

- Section
클래스 내부를 기능 단위로 나누기 위한 분리 단위입니다.
(EnemyCaptureSection.cs)

- System
Unity에 의존하지 않는 순수 C# 계산 로직입니다.
(DamageSystem.cs)

- Signal / Delta
System 처리에 사용하는 입력 / 출력 데이터 구조입니다.
(BattleSignals.cs)

- 처리 흐름

Signal -> System -> Delta -> Apply (Controller 등 에서 적용)

- 파일 구성

- Contracts.cs : 인터페이스 정의 (IController, IModule, ILogic 등)
- EnemyController.cs : Controller 예시
- GravityArea.cs : Module 예시
- MovementLogic.cs : 이동 로직 예시
- HitLogic.cs : 타격 로직 예시
- Projectile.cs : Logic 조합 패턴 예시
- EnemyCaptureSection.cs : Section 예시
- BattleSignals.cs : Signal / Delta 구조체
- DamageSystem.cs : System 예시
- ObjectId.cs : 풀링 등에 사용하는 식별자