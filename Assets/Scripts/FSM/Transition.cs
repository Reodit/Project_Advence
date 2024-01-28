using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FSM
{
    public class Transition<T>
    {
        public string TransitionName;
        public IState<T> StartState;
        public IState<T> TargetState;
        public List<TransitionCondition> Conditions { get; private set; }

        public Transition(string transitionName, IState<T> startState, IState<T> targetState)
        {
            if (string.IsNullOrEmpty(transitionName))
            {
                throw new ArgumentException("Transition name cannot be null or empty.", nameof(transitionName));
            }

            TransitionName = transitionName;            
            StartState = startState ?? throw new ArgumentNullException(nameof(startState), "Start state cannot be null.");
            TargetState = targetState ?? throw new ArgumentNullException(nameof(targetState), "Target state cannot be null.");
            Conditions = new List<TransitionCondition>();
        }

        public TransitionCondition GetTransitionCondition(string paramName)
        {
            return Conditions.FirstOrDefault(e => 
                       e.TransitionParameter.ParamName == paramName) 
                   ?? throw new Exception($"Condition {paramName} not found");
        }

        public TransitionCondition AddTransitionCondition(TransitionParameter transitionParameter, Func<object, bool> condition)
        {
            if (string.IsNullOrEmpty(transitionParameter.ParamName))
            {
                throw new ArgumentException("Transition Parameter name cannot be null or empty.", nameof(transitionParameter.ParamName));
            }

            if (transitionParameter.ParameterType != ParameterType.Trigger)
            {
                if (Conditions == null)
                {
                    throw new Exception("Conditions can't be null for non-trigger parameters.");
                }
            }

            object value = null;

            switch (transitionParameter.ParameterType)
            {
                case ParameterType.Bool:
                    value = default(bool);
                    break;
                case ParameterType.Float:
                    value = default(float);
                    break;
                case ParameterType.Int:
                    value = default(int);
                    break; 
                default:
                    break;
            }
            
            TransitionCondition newCondition = new TransitionCondition(transitionParameter, value, condition);
            Conditions.Add(newCondition);

            return newCondition;
        }

        public void RemoveTransitionCondition(TransitionParameter transitionParameter)
        {
            var condition = Conditions.FirstOrDefault(e =>
                e.TransitionParameter.ParamName == transitionParameter.ParamName &&
                e.TransitionParameter.ParameterType == transitionParameter.ParameterType);

            if (condition == null)
            {
                throw new KeyNotFoundException("Condition not found for given TransitionParameter.");
            }
        
            Conditions.Remove(condition);
        }
    
        // 컨디션이 여러개인 경우 모두 체크를 해야함.
        // 현재 중복체크 중.. 둘중 하나는 빼도록
        public bool CheckConditions(HashSet<string> triggers)
        {
            foreach (var condition in Conditions)
            {
                if (condition.TransitionParameter.ParameterType == ParameterType.Trigger)
                {
                    if (!triggers.Contains(condition.TransitionParameter.ParamName))
                    {
                        return false;
                    }
                }

                else
                {
                    if (!condition.Evaluate())
                    {
                        return false;
                    }
                }
            }

            // TriggerKey 찾아서 지우기
            var triggerKeys = Conditions
                .Where(e => e.TransitionParameter.ParameterType == ParameterType.Trigger)
                .Select(e => e.TransitionParameter.ParamName)
                .ToList();

            foreach (var e in triggerKeys)
            {
                triggers.Remove(e);
            }
            return true;
        }
    }
}
