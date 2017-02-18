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
    /// Represents shared factory.
    /// </summary>
    public class DefaultSharedFactory : SubcontractFactory, ISharedFactory
    {
        /// <summary>
        /// Initializes a new instance of <see cref="DefaultSharedFactory"/> class with 
        /// specified parameters.
        /// </summary>
        /// <param name="namespaces">The array of namespaces to be searched in for services.</param>
        /// <param name="assemblies">The array of assemblies to be searched in for services.</param>
        /// <param name="strictMode">The value indicating whether strict mode is on or off, if strict mode is true then 
        /// system throws an exception if a contract is implemented by more than one service. this prevents the duplicate 
        /// implementation.
        /// </param>
        public DefaultSharedFactory(string[] namespaces, Assembly[] assemblies, bool strictMode) : base(namespaces, assemblies, strictMode, null, null)
        {

        }

        IContractObserver ISharedFactory.ContractObserver
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

        /// <summary>
        /// Create <see cref="Expression"/> for service which is associated with contract.
        /// </summary>
        /// <param name="interfaceType">Type of contract</param>
        /// <param name="register">The ServiceRegister</param>
        /// <param name="state">The ServiceState</param>
        /// <returns>Returns <see cref="Expression"/> of service constructor.</returns>
        public override Expression Create(Type interfaceType, ServiceRegister register, ServiceState state)
        {
            if (interfaceType == null)
                throw new ArgumentNullException(nameof(interfaceType));

            //Register current subcontract service with ServiceRegistrar
            //Root service will refresh with the updated service when there's replacement with new service
            register.Register(interfaceType);

            BaseServiceInfo serviceMeta = ServicesMapTable?[interfaceType];

            //Throw exception if service meta not found in the service map table
            if (serviceMeta == null)
            {
                ExceptionHelper.ThrowContractNotRegisteredException($"Type '{interfaceType.FullName}' was not registered with shared factory.");
            }

            var userdefinedExpression = serviceMeta.GetServiceInstanceExpression();
            if (userdefinedExpression != null)
            {
                return Expression.Invoke(userdefinedExpression);
            }

            return CreateConstructorExpression(interfaceType, serviceMeta.ServiceType, register, state) ?? Expression.Default(interfaceType);
        }

        /// <summary>
        /// Creates dependency <see cref="Expression"/> object. if no dependency factory is set and shared factory exists then
        /// creates dependency from the shared factory; otherwise creates from the dependency factory.
        /// </summary>
        /// <param name="interfaceType">The type of the contract.</param>
        /// <param name="register">The service register object.</param>
        /// <param name="state">The service state.</param>
        /// <returns>Dependency expression</returns>
        protected override Expression CreateDependency(Type interfaceType, ServiceRegister register, ServiceState state)
        {
            return Create(interfaceType, register, state) ?? Expression.Default(interfaceType);
        }
    }
}
