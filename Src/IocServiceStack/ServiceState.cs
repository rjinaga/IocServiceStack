namespace IocServiceStack
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    /// <summary>
    /// Represents state of the service.
    /// </summary>
    public class ServiceState
    {
        /// <summary>
        /// container all main and re-usable variables.
        /// </summary>
        List<BinaryExpression> _assignments; 
        private readonly Dictionary<Type, ParameterExpression> _stateData;
        
        /// <summary>
        /// Initializes a new instance of <see cref="ServiceState"/> class.
        /// </summary>
        public ServiceState()
        {
            _stateData = new Dictionary<Type, ParameterExpression>();
            _assignments = new List<BinaryExpression>();

        }

        /// <summary>
        /// Gets or sets the <see cref="Expression"/> associated with the specified <paramref name="contract"/>.
        /// </summary>
        /// <param name="contract">The contract of the <see cref="Expression"/> to get or set.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Adds state data 
        /// </summary>
        /// <param name="contract">The type of the contract.</param>
        /// <param name="reuseExpression">The <see cref="Expression"/> to be reused.</param>
        /// <returns>Returns variable expression for the contract.</returns>
        public Expression AddState(Type contract, Expression reuseExpression)
        {
            var variable = Expression.Variable(contract);
            _stateData.Add(contract, variable);
            _assignments.Add(Expression.Assign(variable, reuseExpression));

            return variable;
        }

        /// <summary>
        /// Gets read-only binary expressions.
        /// </summary>
        /// <returns>Returns read-only list of <see cref="BinaryExpression"/> objects.</returns>
        public IReadOnlyList<BinaryExpression> GetBinaryExpressions()
        {
            return _assignments.AsReadOnly();
        }

        /// <summary>
        /// Gets list of parameters.
        /// </summary>
        /// <returns>Returns collection of <see cref="ParameterExpression"/> objects.</returns>
        public IEnumerable<ParameterExpression> GetParameters()
        {
            return _stateData.Values;
        }

        /// <summary>
        /// Gets whether state has any parameter.
        /// </summary>
        /// <returns>true if state data has any parameter; otherwise false.</returns>
        public bool HasParameters()
        {
            return _stateData.Count != 0;
        }
    }
}
