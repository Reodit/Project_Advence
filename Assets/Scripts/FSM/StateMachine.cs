using System;
using System.Collections.Generic;
using System.Linq;
using FSM.States;
using UnityEngine;

namespace FSM
{
    public class StateMachine<T> : IStateMachine
    {
        public IState<T> CurrentState { get; set; }
        private IState<T> _previousState;
        private T _owner;
    
        private Dictionary<string, Transition<T>> _transitions;
        private HashSet<string> _triggers;
        private Dictionary<string, IState<T>> _states;
        private HashSet<TransitionParameter> _transitionParameters;

        public StateMachine(T owner)
        {
            this._owner = owner;
            Init();
        }
    
        public IState<T> CreateState(IState<T> state)
        {
            if (state == null)
            {
                throw new ArgumentNullException(nameof(state));
            }

            if (_states.ContainsKey(state.StateName))
            {
                throw new ArgumentException($"A state with the name '{state.StateName}' already exists.");
            }

            _states.Add(state.StateName, state);

            return state;
        }
    
        public IState<T> GetState(string stateName)
        {
            if (_states.TryGetValue(stateName, out var state))
            {
                return state;
            }

            throw new KeyNotFoundException($"No state matches the specified state name: {stateName}.");
        }

        // state와 연결된 모든 transition도 함께 삭제합니다.. 
        public void RemoveState(string stateName)
        {
            if (!_states.TryGetValue(stateName, out var stateToRemove))
            {
                throw new KeyNotFoundException($"No state matches the specified state name: {stateName}.");
            }

            var transitionsToRemove = _transitions
                .Where(kvp => kvp.Value.StartState == stateToRemove || kvp.Value.TargetState == stateToRemove)
                .Select(kvp => kvp.Key)
                .ToList();

            foreach (var transitionName in transitionsToRemove)
            {
                _transitions.Remove(transitionName);
            }

            _states.Remove(stateName);
        }
        
        public void ChangeState(IState<T> newState)
        {
            CurrentState?.Exit(_owner);
            _previousState = CurrentState;
            CurrentState = newState;
            CurrentState.Enter(_owner);
        }
        
        public void RevertToPreviousState()
        {
            ChangeState(_previousState);
        }

        public void Init()
        {
            _transitions = new Dictionary<string, Transition<T>>();
            _triggers = new HashSet<string>();
            _states = new Dictionary<string, IState<T>>();
            _transitionParameters = new HashSet<TransitionParameter>();
            if (CurrentState == null)
            {
                var nullState = new NullState<T>();
                CurrentState = nullState;
            }
        }
        
        // warning : 유니티 Update주기와 Co-Routine 주기가 맞지 않아서, 여전히 딜레이가 존재한다.
        public void Update()
        {
            Transition<T> transition = CheckTransitionConditions();
            
            if (transition != null)
            {
                PerformTransition(transition);
            }
            
            if (CurrentState?.RequiresUpdate ?? true)
            {
                CurrentState?.Execute(_owner);
            }
        }

        // 1. 모든 트랜지션을 순회한다.
        // 2. CurrentState가 트랜지션의 startState 라면 해당 트랜지션을 검사한다.
        // 3. 해당 트랜지션의 해당하는 파라미터를 모두 가져온다.
        // 4. 파라미터들의 조건이 충족되었는지 검사한다.
        // 5. 모두 충족되었다면 해당 트랜지션 리턴.
        private Transition<T> CheckTransitionConditions()
        {
            foreach (var transition in _transitions.Values)
            {
                if (transition.StartState != CurrentState)
                {
                    continue;
                }
                
                if (transition.CheckConditions(_triggers))
                {
                    return transition;
                }
            }

            return null;
        }

        public void OneShotUpdate()
        {
            CurrentState?.Execute(_owner);
            RevertToPreviousState();
        }

        public TransitionParameter GetTransitionParameter(string paramName, ParameterType parameterType)
        {
            var result = _transitionParameters.FirstOrDefault(e =>
                e.ParamName == paramName &&
                e.ParameterType == parameterType);

            if (result.Equals(default(TransitionParameter)))
            {
                throw new Exception();
            }
        
            return result;
        }
    
        public void SetInteger(string paramName, int value)
        {
            var results = _transitions.Values
                .SelectMany(t => t.Conditions
                    .Where(e => e.TransitionParameter.ParamName == paramName 
                                && e.TransitionParameter.ParameterType == ParameterType.Int)
                    .Select(e => new 
                    { 
                        Transition = t, 
                        Condition = e 
                    }))
                .ToList();

            if (results.Count == 0)
            {
                throw new Exception();
            }

            foreach (var e in results)
            {
                e.Condition.Value = value;
            }
        }

        public void SetFloat(string paramName, float value)
        {
            var results = _transitions.Values
                .SelectMany(t => t.Conditions
                    .Where(e => e.TransitionParameter.ParamName == paramName 
                                && e.TransitionParameter.ParameterType == ParameterType.Float)
                    .Select(e => new 
                    { 
                        Transition = t, 
                        Condition = e 
                    }))
                .ToList();

            if (results.Count == 0)
            {
                throw new Exception();
            }

            foreach (var e in results)
            {
                e.Condition.Value = value;
            }
        }

        public void SetBool(string paramName, bool value)
        {
            var results = _transitions.Values
                .SelectMany(t => t.Conditions
                    .Where(e => e.TransitionParameter.ParamName == paramName 
                                && e.TransitionParameter.ParameterType == ParameterType.Bool)
                    .Select(e => new 
                    { 
                        Transition = t, 
                        Condition = e 
                    }))
                .ToList();

            if (results.Count == 0)
            {
                throw new Exception();
            }

            foreach (var e in results)
            {
                e.Condition.Value = value;
            }
        }

        public void SetTrigger(string triggerName)
        {
            if (string.IsNullOrEmpty(triggerName))
            {
                throw new ArgumentException("Trigger name must not be null or empty", nameof(triggerName));
            }

            var result = _transitions.Values
                .FirstOrDefault(t => t.Conditions
                    .Any(e => e.TransitionParameter.ParamName == triggerName 
                              && e.TransitionParameter.ParameterType == ParameterType.Trigger));

            if (result == null)
            {
                throw new Exception();
            }

            if (_triggers.Add(triggerName) &&
                result.CheckConditions(_triggers))
            {
                PerformTransition(result, true);
            }
        }
    
        public Transition<T> CreateTransition(string transitionName, IState<T> startState, IState<T> targetState)
        {
            Transition<T> transition = new Transition<T>(transitionName, startState, targetState);
            _transitions.Add(transition.TransitionName, transition);
        
            return transition;
        }

        public TransitionCondition AddTransitionCondition(Transition<T> transition, TransitionParameter transitionParameter,
            Func<object, bool> condition)
        {
            if (transition == null)
            {
                throw new Exception();
            }
        
            var transitionCondition = transition.AddTransitionCondition(transitionParameter, condition);
            _transitionParameters.Add(transitionParameter);
            return transitionCondition;
        }
    
        public TransitionCondition AddTransitionCondition(string transitionName, TransitionParameter transitionParameter, object value,
            Func<object, bool> condition)
        {
            if (_transitionParameters.Contains(transitionParameter))
            { 
                Debug.LogWarning($"A transition parameter with the same values already exists. : {nameof(transitionParameter)}");
            }
        
            var transitionCondition = GetTransition(transitionName).AddTransitionCondition(transitionParameter, condition);

            _transitionParameters.Add(transitionParameter);
            return transitionCondition;
        }
        
        /// <summary>
        /// Transition for Trigger 
        /// </summary>
        /// <param name="transition"></param>
        /// <param name="transitionParameter"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public TransitionCondition AddTransitionCondition(Transition<T> transition, TransitionParameter transitionParameter)
        {
            if (transition == null)
            {
                throw new Exception();
            }
        
            var transitionCondition = transition.AddTransitionCondition(transitionParameter, null);
            _transitionParameters.Add(transitionParameter);
            return transitionCondition;
        }

        public Transition<T> GetTransition(string transitionName)
        {
            var transition = _transitions.Values.FirstOrDefault(t =>
                t.TransitionName == transitionName);

            if (transition == null)
            {
                throw new Exception();
            }
        
            return transition;
        }

        public void RemoveTransition(string transitionName)
        {
            if (!_transitions.ContainsKey(transitionName))
            {
                throw new KeyNotFoundException($"Transition with name {transitionName} is not Found");
            }
        
            _transitions.Remove(transitionName);
            _triggers.Remove(transitionName);
        }

        public void ClearTransitions()
        {
            _transitions.Clear();
            _triggers.Clear();
        }
    
        private void PerformTransition(Transition<T> transition, bool isTrigger = false)
        {
            CurrentState?.Exit(_owner);
            _previousState = transition.StartState;
            CurrentState = transition.TargetState;
            CurrentState?.Enter(_owner);

            if (isTrigger)
            {
                OneShotUpdate();
            }
        }
    }
}