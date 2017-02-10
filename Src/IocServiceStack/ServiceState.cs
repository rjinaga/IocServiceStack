namespace IocServiceStack
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    public class ServiceState
    {
        List<BinaryExpression> _assignments; ////all main and reusable variables
        private readonly Dictionary<Type, ParameterExpression> _stateData;
        //Holds Reuse Service instance expressions
        //
        public ServiceState()
        {
            _stateData = new Dictionary<Type, ParameterExpression>();
            _assignments = new List<BinaryExpression>();
        }

        public Expression this[Type contract]
        {
            get
            {
                if (_stateData.ContainsKey(contract))
                {
                    return _stateData[contract];
                }
                return null;
            }
        }

        public Expression AddState(Type contract, Expression reuseExpression)
        {
            var variable = Expression.Variable(contract);
            _stateData.Add(contract, variable);
            _assignments.Add(Expression.Assign(variable, reuseExpression));

            return variable;
        }

        public IReadOnlyList<BinaryExpression> GetBinaryExpressions()
        {
            return _assignments.AsReadOnly();
        }

        public IEnumerable<ParameterExpression> GetParameters()
        {
            return _stateData.Values;
        }

        public bool HasParameters()
        {
            return _stateData.Count != 0;
        }
    }
}
