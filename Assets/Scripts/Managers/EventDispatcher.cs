using System;
using System.Collections.Generic;
using UnityEngine;

namespace Managers
{
    public class EventDispatcher : MonoBehaviour
    {
        public static EventDispatcher Instance;
        private readonly Dictionary<string, Delegate> eventTable = new();
        private readonly Queue<Action> eventQueue = new();
        private EventDispatcher() {}
        public void Awake()
        {
            Instance = this;
        }

        public void Update()
        {
            while (eventQueue.Count > 0)
            {
                var action = eventQueue.Dequeue();
                action();
            }
        }

        public void RegisterEvent<T>(string eventName, Action<T> handler, T eventData)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentException("Event name must be valid.");
            }
    
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }
    
            // 동일한 이름의 이벤트가 등록되었을 때 처리
            if (eventTable.TryGetValue(eventName, out var existing))
            {
                if (existing is not Action<T>)
                {
                    throw new InvalidOperationException($"Event with name {eventName} already registered with a different type.");
                }

                eventTable[eventName] = Delegate.Combine(existing, handler);
            }

            else
            {
                eventTable[eventName] = handler;
            }
    
            eventQueue.Enqueue(() => ((Action<T>)eventTable[eventName])(eventData));
        }

        public void UnregisterEvent<T>(string eventName, Action<T> handler)
        {
            if (string.IsNullOrEmpty(eventName))
            {
                throw new ArgumentException("Event name must be valid.");
            }
    
            if (handler == null)
            {
                throw new ArgumentNullException(nameof(handler));
            }

            if (!eventTable.ContainsKey(eventName))
            {
                throw new InvalidOperationException($"Event with name {eventName} not found");
            }
    
            eventTable[eventName] = Delegate.Remove(eventTable[eventName], handler);

            if (eventTable[eventName] == null)
            {
                eventTable.Remove(eventName);
            }
        }
    }
}