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
    using System.Reflection;

    /// <summary>
    /// Represents default sub contract factory, which holds the services of 
    /// dependencies.
    /// </summary>
    public class DefaultSubcontractFactory : SubcontractFactory, IDependencyFactory
    {

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultSubcontractFactory"/>  class with specified parameters
        /// <paramref name="namespaces"/>, <paramref name="assemblies"/> and <paramref name="strictMode"/>.
        /// </summary>
        /// <param name="name">Specify name of the factory.</param>
        /// <param name="namespaces">The array of namespaces to be searched in for services.</param>
        /// <param name="assemblies">The array of assemblies to be searched in for services.</param>
        /// <param name="strictMode">The value indicating whether strict mode is on or off, if strict mode is true then 
        /// system throws an exception if a contract is implemented by more than one service. this prevents the duplicate 
        /// implementation.
        /// </param>
        /// <param name="dependencyFactory"></param>
        /// <param name="sharedFactory"></param>
        public DefaultSubcontractFactory(string name, string[] namespaces, Assembly[] assemblies, bool strictMode, IDependencyFactory dependencyFactory, ISharedFactory sharedFactory) 
            : base(namespaces,assemblies, strictMode, dependencyFactory, sharedFactory)
        {
            Name = name;
        }

        IContractObserver IDependencyFactory.ContractObserver
        {
            get
            {
                return base.ContractObserver;
            }
            set
            {
                base.ContractObserver = value;
            }
        }

        IDependencyFactory IDependencyFactory.DependencyFactory => base.DependencyFactory;

        /// <summary>
        /// Gets name of the current factory.
        /// </summary>
        public string Name
        {
            get; private set;
        }

        /// <summary>
        /// Create <see cref="Expression"/> for service which is associated with contract.
        /// </summary>
        /// <param name="interfaceType">Type of contract</param>
        /// <param name="register">The ServiceRegister</param>
        /// <param name="state">The ServiceState</param>
        /// <param name="serviceName"></param>
        /// <param name="attributes"></param>
        /// <returns>Returns <see cref="Expression"/> of service constructor.</returns>
        public override Expression Create(Type interfaceType, ServiceRegister register, ServiceState state, string serviceName,  TypeContextAttributes attributes)
        {
            if (interfaceType == null)
                throw new ArgumentNullException(nameof(interfaceType));

            //Register current subcontract service with ServiceRegistrar
            //Root service will refresh with the updated service when there's replacement with new service
            register.Register(interfaceType);

            BaseServiceInfo serviceMeta = GetServiceInfo(interfaceType,serviceName);

            if (serviceMeta == null)
            {
                return SharedFactory.Create(interfaceType, register, state, serviceName, attributes);
            }

            var userdefinedExpression = serviceMeta.GetServiceInstanceExpression();
            if (userdefinedExpression != null)
            {
                return Expression.Invoke(userdefinedExpression);
            }

            return CreateConstructorExpression(interfaceType, serviceMeta.ServiceType, register, state)?? Expression.Default(interfaceType);
        }


    }
   
}
