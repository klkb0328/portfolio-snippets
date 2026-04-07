using System;
using System.Collections.Generic;

namespace Common.Messaging
{
    /// <summary>
    /// Message 인스턴스 풀링.
    /// </summary>
    public static class MessagePool
    {
        private static readonly List<Action> clearActions = new();

        private static class PoolCache<T> where T : Message
        {
            static PoolCache()
            {
                clearActions.Add(Clear);
            }

            internal static readonly Stack<T> pool = new();

            private static void Clear() => pool.Clear();
        }

        public static void ClearAllPools()
        {
            foreach (var action in clearActions)
                action();
        }

        public static T Get<T>() where T : Message, new()
        {
            var pool = PoolCache<T>.pool;
            return pool.Count > 0 ? pool.Pop() : new T();
        }

        public static void Release<T>(T message) where T : Message
        {
            message.Dispose();
            PoolCache<T>.pool.Push(message);
        }
    }
}
