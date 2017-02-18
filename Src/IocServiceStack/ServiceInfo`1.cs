#region License
// Copyright (c) 2016-2017 Rajeswara Rao Jinaga
//
// Permission is hereby granted, free of charge, to any person
// obtaining a copy of this software and associated documentation
// files (the "Software"), to deal in the Software without
// restriction, including without limitation the rights to use,
// copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the
// Software is furnished to do so, subject to the following
// conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES
// OF MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT
// HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY,
// WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING
// FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
#endregion

namespace IocServiceStack
{
    using System;
    using System.Linq.Expressions;

    /// <summary>
    /// This class contains the service of the contract, decorators and activator.
    /// </summary>
    /// <typeparam name="TC">The type of the contract.</typeparam>
    /// <typeparam name="TS">The type of the service.</typeparam>
    public class ServiceInfo<TC, TS> : BaseServiceInfo where TC: class where TS: TC
    {
        private readonly Func<TS> _actionInfo;
        private readonly Expression<Func<TS>> _expressionCallback;

        /// <summary>
        /// Initializes a new instance of <see cref="ServiceInfo"/> class with specified <paramref name="decorators"/> and 
        /// <paramref name="serviceName"/>.
        /// </summary>
        /// <param name="decorators">The array of decorators</param>
        /// <param name="serviceName">The name of the service.</param>
        public ServiceInfo(DecoratorAttribute[] decorators, string serviceName) : base(typeof(TS), decorators, serviceName)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ServiceInfo"/> class. 
        /// </summary>
        /// <param name="isReusable"></param>
        /// <param name="decorators"></param>
        /// <param name="serviceName"></param>
        public ServiceInfo(bool isReusable, DecoratorAttribute[] decorators, string serviceName) : base(typeof(TS), decorators, isReusable, serviceName)
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ServiceInfo"/> class.
        /// </summary>
        /// <param name="serviceAction"></param>
        /// <param name="decorators"></param>
        /// <param name="serviceName"></param>
        public ServiceInfo(Func<TS> serviceAction, DecoratorAttribute[] decorators, string serviceName) : base(null, decorators, serviceName)
        {
            _actionInfo = serviceAction;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ServiceInfo"/> class.
        /// </summary>
        /// <param name="serviceAction"></param>
        /// <param name="decorators"></param>
        /// <param name="isReusable"></param>
        /// <param name="serviceName"></param>
        public ServiceInfo(Func<TS> serviceAction, DecoratorAttribute[] decorators, bool isReusable, string serviceName) : base(null, decorators, isReusable, serviceName)
        {
            _actionInfo = serviceAction;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ServiceInfo"/> class.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="decorators"></param>
        /// <param name="serviceName"></param>
        public ServiceInfo(Expression<Func<TS>> expression, DecoratorAttribute[] decorators, string serviceName) : base(null, decorators, serviceName)
        {
            _expressionCallback = expression;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="ServiceInfo"/> class.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="decorators"></param>
        /// <param name="isReusable"></param>
        /// <param name="serviceName"></param>
        public ServiceInfo(Expression<Func<TS>> expression, DecoratorAttribute[] decorators, bool isReusable, string serviceName) : base(null, decorators, isReusable, serviceName)
        {
            _expressionCallback = expression;
        }

        /// <summary>
        /// This method to get service instance callback function.
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <returns></returns>
        public override Func<T1> GetServiceInstanceCallback<T1>() 
        {
            if (_actionInfo == null)
            {
                return null;
            }

            var typeCastedAction = _actionInfo as Func<T1>;
            if (typeCastedAction == null)
            {
                throw new InvalidCastException($"{_actionInfo.ToString()} to Func<{typeof(T1).ToString()}> ");
            }

            return typeCastedAction;
        }

        /// <summary>
        /// This method to get service instance expression
        /// </summary>
        /// <returns></returns>
        public override Expression GetServiceInstanceExpression()
        {
            return _expressionCallback ;
        }

    }
}
