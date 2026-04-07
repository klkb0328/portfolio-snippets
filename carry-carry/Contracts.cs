namespace Defense.Core
{
    /// <summary>
    /// 현장 오브젝트 담당자(MonoBehaviour).
    /// 게임 내 독립적인 오브젝트의 고유 로직을 소유한다.
    /// </summary>
    public interface IController
    {
        Transform Transform { get; }
        GameObject GameObject { get; }
        void Initialize(IContext context);
        void ResetState();
    }

    public interface IController<TContext> : IController where TContext : IContext
    {
        void Initialize(TContext context);

        void IController.Initialize(IContext context)
        {
            if (context is TContext typed)
            {
                Initialize(typed);
            }
        }
    }

    /// <summary>
    /// Controller 초기화 시 필요한 의존성을 전달하는 컨텍스트.
    /// 각 Controller별로 구체 Context 타입을 정의한다. (예: EnemyContext)
    /// </summary>
    public interface IContext
    {
        // 내용 생략
    }

    /// <summary>
    /// 특정 Controller에 종속되어 전용 기능을 제공해준다.
    /// 반드시 프리팹 기반으로 Controller의 자식 오브젝트로 붙여 사용해야 함.
    /// </summary>
    public interface IModule<TController> where TController : IController
    {
        void Initialize(TController host);
        void Tick(float deltaTime);
        void FixedTick(float fixedDeltaTime);
    }

    /// <summary>
    /// 순수 C# 계산코드. Unity에 의존하지 않는 static class.
    /// </summary>
    public interface ISystem
    {
        // 내용 생략
    }

    /// <summary>
    /// 계산 전 데이터. System에 전달할 원본 값을 담는 구조체용 인터페이스
    /// </summary>
    public interface ISignal
    {
        // 내용 생략
    }

    /// <summary>
    /// 계산 결과. System이 산출한 최종 결과를 담는 구조체용 인터페이스
    /// </summary>
    public interface IDelta
    {
        // 내용 생략
    }

    /// <summary>
    /// Logic은 대분류 내 교체 가능한 스크립트 단위 기능.
    /// Module과 달리 프리팹이 아닌 스크립트만으로 동작하며,
    /// 호스트가 GetComponents로 자동 발견하여 조합한다.
    /// </summary>
    public interface ILogic
    {
        // 내용 생략
    }

    /// <summary>
    /// Projectile 전용 Logic. 투사체를 호스트로 받아 동작한다.
    /// </summary>
    public interface IProjectileLogic : ILogic
    {
        void OnInitialize(Projectile host);
    }

    /// <summary>
    /// 이동 계산 Logic. 계산만 수행하고 적용은 호스트가 한다.
    /// </summary>
    public interface IMovementLogic : IProjectileLogic
    {
        void UpdateMovement(float dt, ref Vector2 position, ref Quaternion rotation);
    }

    /// <summary>
    /// 충돌 판정 Logic. 적과의 충돌 시 어떻게 처리할지 결정한다.
    /// </summary>
    public interface IHitLogic : IProjectileLogic
    {
        void OnUpdate(float dt);
        void OnProjectileTriggerEnter(Collider2D other);
    }
}
