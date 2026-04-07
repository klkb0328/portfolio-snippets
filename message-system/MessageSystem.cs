using System;
using System.Collections.Generic;
using UnityEngine;

namespace Common.Messaging
{
    /// <summary>
    /// 가독성 용으로 액션으로 안함
    /// </summary>
    public delegate void MessageHandler(IMessage message);

    public class MessageSystem
    {
        public static MessageSystem Instance { get; } = new MessageSystem();

        private readonly Dictionary<Type, List<MessageHandler>> subscribers = new();
        private readonly Dictionary<Type, HashSet<MessageHandler>> onceSubscribers = new();

        private readonly Queue<IMessage> publishQueue = new();
        private readonly Queue<(Type type, MessageHandler handler)> subscribeQueue = new();
        private readonly Queue<(Type type, MessageHandler handler)> subscribeOnceQueue = new();
        private readonly Queue<(Type type, MessageHandler handler)> unsubscribeQueue = new();

        private MessageSystem() { }

        public void Subscribe<T>(MessageHandler handler) where T : IMessage
        {
            subscribeQueue.Enqueue((typeof(T), handler));
        }

        public void SubscribeOnce<T>(MessageHandler handler) where T : IMessage
        {
            subscribeOnceQueue.Enqueue((typeof(T), handler));
        }

        public void Unsubscribe<T>(MessageHandler handler) where T : IMessage
        {
            unsubscribeQueue.Enqueue((typeof(T), handler));
        }

        public void Publish(IMessage message)
        {
            publishQueue.Enqueue(message);
        }

        public void ClearAllQueues()
        {
            publishQueue.Clear();
            subscribeQueue.Clear();
            subscribeOnceQueue.Clear();
            unsubscribeQueue.Clear();
            subscribers.Clear();
            onceSubscribers.Clear();
        }

        /// <summary>
        /// 매 프레임 LateUpdate에서 호출.
        /// 순서는 구독/해제 먼저, 그 다음 발송 (Flush 순서 고정)
        /// 같은 프레임 내 Publish 순서는 FIFO 보장.
        /// </summary>
        public void Flush()
        {
            // 1. 구독 요청 처리
            while (subscribeQueue.TryDequeue(out var sub))
            {
                if (!subscribers.TryGetValue(sub.type, out var list))
                {
                    list = new List<MessageHandler>();
                    subscribers[sub.type] = list;
                }

                if (!list.Contains(sub.handler))
                {
                    list.Add(sub.handler);
                }
            }

            while (subscribeOnceQueue.TryDequeue(out var sub))
            {
                if (!onceSubscribers.TryGetValue(sub.type, out var set))
                {
                    set = new HashSet<MessageHandler>();
                    onceSubscribers[sub.type] = set;
                }
                set.Add(sub.handler);
            }

            // 2. 구독 해제 처리
            while (unsubscribeQueue.TryDequeue(out var unsub))
            {
                if (subscribers.TryGetValue(unsub.type, out var list))
                    list.Remove(unsub.handler);
                if (onceSubscribers.TryGetValue(unsub.type, out var set))
                    set.Remove(unsub.handler);
            }

            // 3. 퍼블리시 처리 (즉 최종적으로 구독된 애들에게만 보낸다.)
            while (publishQueue.TryDequeue(out var message))
            {
                Dispatch(message);
            }
        }

        private void Dispatch(IMessage message)
        {
            var type = message.GetType();

            // 일반 구독자
            if (subscribers.TryGetValue(type, out var list))
            {
                for (int i = 0; i < list.Count; i++)
                {
                    try 
                    { 
                       list[i](message);
                    }
                    catch (Exception e)
                    {
                       Debug.LogError(e);
                    }
                }
            }

            // 1회 구독자
            if (onceSubscribers.TryGetValue(type, out var set))
            {
                foreach (var handler in set)
                {
                    try
                    {
                        handler(message);
                    }
                    catch (Exception e)
                    {
                        Debug.LogError(e);
                    }
                }

                set.Clear();
                onceSubscribers.Remove(type);
            }

            // 풀링 메시지 반납
            if (message is Message pooledMessage)
            {
                MessagePool.Release(pooledMessage);
            }
        }
    }
}
