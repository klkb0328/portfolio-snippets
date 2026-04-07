using UnityEngine;
using Defense.Core;

namespace Defense
{
    /// <summary>
    /// System에서 계산에 필요한 원본 데이터를 담는 구조체.
    /// 공격자/피격자 식별자, 기본 공격력, 데미지 배율 등 System이 계산에 쓸 값만 포함.
    /// 계산시에서 쓸 예정이라 그냥 읽기전용 구조체로..
    /// </summary>
    public readonly struct DamageSignal : ISignal
    {
        public readonly ObjectId AttackerId;
        public readonly ObjectId TargetId;
        public readonly float BaseAttack;
        public readonly float DamageRatio;
        public readonly float PushTime;
        public readonly Vector2 PushDirection;

        public DamageSignal(ObjectId attackerId, ObjectId targetId, float baseAttack,
            float damageRatio, float pushTime, Vector2 pushDirection)
        {
            AttackerId = attackerId;
            TargetId = targetId;
            BaseAttack = baseAttack;
            DamageRatio = damageRatio;
            PushTime = pushTime;
            PushDirection = pushDirection;
        }
    }

    /// <summary>
    /// System이 산출한 최종 결과를 담는 구조체.
    /// Apply 단계에서 Controller가 받아 HP 감소, 이펙트 재생 등에 사용.
    /// 이것도 델타를 한번 정하면 따로 안바꿀 예정이라서 읽기전용으로 함.
    /// </summary>
    public readonly struct DamageDelta : IDelta
    {
        public readonly ObjectId TargetId;
        public readonly float FinalDamage;
        public readonly float PushTime;
        public readonly Vector2 PushDirection;

        public DamageDelta(ObjectId targetId, float finalDamage, float pushTime, Vector2 pushDirection)
        {
            TargetId = targetId;
            FinalDamage = finalDamage;
            PushTime = pushTime;
            PushDirection = pushDirection;
        }
    }
}
