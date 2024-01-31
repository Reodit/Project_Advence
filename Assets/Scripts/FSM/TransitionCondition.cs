using System;

namespace FSM
{
    public enum ParameterType
    {
        Int, 
        Float, 
        Bool,
        Trigger
    }

    public struct TransitionParameter
    {
        public string ParamName;
        public ParameterType ParameterType;

        public TransitionParameter(string paramName, ParameterType parameterType)
        {
            this.ParamName = paramName;
            this.ParameterType = parameterType;
        }
    }

    public class TransitionCondition
    {
        public TransitionParameter TransitionParameter;
        public object Value; 
        public Func<object, bool> Condition;
    
        public TransitionCondition(TransitionParameter transitionParameter, object value, Func<object, bool> condition)
        {
            TransitionParameter = transitionParameter;
            Value = value;
            Condition = condition;
        }

        /// <summary>
        /// Set TransitionCondition for Trigger
        /// </summary>
        /// <param name="paramName"></param>
        public TransitionCondition(string paramName)
        {
            TransitionParameter.ParamName = paramName;
            TransitionParameter.ParameterType = ParameterType.Trigger;
            Value = null;
            Condition = o => true;
        }
    
        public bool Evaluate()
        {
            return Condition(Value);
        }
    }
}