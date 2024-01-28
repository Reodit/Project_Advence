using System;
using System.Collections.Generic;
using System.Threading;
using FSM;
using UnityEditor;
using UnityEngine;

namespace Managers
{
    public class StateMachineManager : MonoBehaviour
    {
        private Dictionary<int, IStateMachine> _stateMachines;
        
        public static StateMachineManager Instance;
        public static bool PauseStateMachineUpdate;
        private StateMachineManager() { }

        public void Awake()
        {
            Instance = this;
            _stateMachines = new Dictionary<int, IStateMachine>();
            PauseStateMachineUpdate = false;
        }
        private void Update()
        {
            if (PauseStateMachineUpdate)
            {
                return;
            }
            
            foreach (var stateMachine in _stateMachines)
            {
                try
                {
                    stateMachine.Value.Update();
                }
                catch (Exception e)
                {
                    Debug.LogError($"Exception occurred in state machine {stateMachine.Key}: {e}");
                }
            }
        }

        public void Register(int key, IStateMachine stateMachine)
        {
            if (_stateMachines.ContainsKey(key))
            {
                throw new InvalidOperationException("A state machine with the same key is already registered.");
            }
            
            _stateMachines[key] = stateMachine ?? throw new ArgumentException("Invalid arguments for registration.");
        }

        public void Unregister(int key)
        {
            if (!_stateMachines.ContainsKey(key))
            {
                throw new KeyNotFoundException($"State machine with key {key} not found.");
            }

            _stateMachines.Remove(key);
        }

        public IStateMachine GetStateMachine(int key)
        {
            if (!_stateMachines.TryGetValue(key, out var stateMachine))
            {
                throw new KeyNotFoundException($"State machine with key {key} not found.");
            }

            return stateMachine;
        }
    }
}
