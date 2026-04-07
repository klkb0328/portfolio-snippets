using System;

namespace Defense.Core
{
    /// <summary>
    /// Unity 오브젝트를 가리키는 ID.
    ///  
    /// GetInstanceID()와 다른 점은
    /// - GetInstanceID()는 Unity 내부 ID (에디터/런타임 상황에 따라 다름)
    /// - ObjectId는 게임 로직용 식별자
    /// - 아무래도 풀링같은거 할때 (다만 모든 풀링대상이 쓰는건아님. 투사체가 예시) 안정적인 식별자가 필요해서 추가.
    /// </summary>
    [Serializable]
    public readonly struct ObjectId : IEquatable<ObjectId>
    {
        public static ObjectId None => new ObjectId(0);

        public int Value { get; }
        public bool IsValid => Value != 0;

        public ObjectId(int value) => Value = value;

        public bool Equals(ObjectId other) => Value == other.Value;
        public override bool Equals(object obj) => obj is ObjectId other && Equals(other);
        public override int GetHashCode() => Value;

        public static bool operator ==(ObjectId left, ObjectId right) => left.Value == right.Value;
        public static bool operator !=(ObjectId left, ObjectId right) => left.Value != right.Value;

        public override string ToString() => IsValid ? $"ObjectId({Value})" : "ObjectId(None)";
    }
}
