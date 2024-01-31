using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

namespace Managers
{
    // 비동기 작업을 위한 클래스
    public class ThreadedDataRequester : MonoBehaviour 
    {
        private static ThreadedDataRequester _instance;
        private readonly Queue<IThreadInfo> _dataQueue = new Queue<IThreadInfo>();

        private void Awake() 
        {
            _instance = this;
        }

        public static void RequestData<T>(Func<T> generateData, Action<T> callback)
        {
            void ThreadStart()
            {
                _instance.DataThread(generateData, callback);
            }

            new Thread(ThreadStart).Start();
        }

        private void DataThread<T>(Func<T> generateData, Action<T> callback) 
        {
            T data = generateData();
            lock (_dataQueue)
            {
                _dataQueue.Enqueue(new ThreadInfo<T>(callback, data));
            }
        }
    
        private void Update()
        {
            lock (_dataQueue)
            {
                while (_dataQueue.Count > 0)
                {
                    var threadInfo = _dataQueue.Dequeue();
                    threadInfo.Execute();
                }
            }
        }

        private interface IThreadInfo
        {
            void Execute();
        }

        private class ThreadInfo<T> : IThreadInfo
        {
            private readonly Action<T> _callback;
            private readonly T _data;

            public ThreadInfo(Action<T> callback, T data)
            {
                _callback = callback;
                _data = data;
            }

            public void Execute()
            {
                _callback(_data);
            }
        }
    }
}
