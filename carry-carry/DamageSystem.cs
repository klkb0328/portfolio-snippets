namespace Defense.Systems
{
    /// <summary>
    /// System 예시코드.
    /// Unity에 의존하지 않는 순수 C# 계산처리
    /// Signal을 받아 최종 데미지를 산출하고 Delta로 돌려준다.
    /// 추후 방어력/속성 저항 등도 여기서 처리.
    /// </summary>
    public static class DamageSystem
    {
        public static DamageDelta Evaluate(in DamageSignal signal)
        {
            float finalDamage = signal.BaseAttack * signal.DamageRatio;

            // TODO: 방어력/속성 저항 적용
            return new DamageDelta(
                signal.TargetId,
                finalDamage,
                signal.PushTime,
                signal.PushDirection
            );
        }
    }
}
