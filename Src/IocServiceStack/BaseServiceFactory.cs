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
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Represents abstract factory of container service.
    /// </summary>
    public abstract class BaseServiceFactory 
    {
        private IContractObserver _observer;

        /// <summary>
        /// Get read-only ServiceMapTable object
        /// </summary>
        protected readonly ContractServiceAutoMapper ServicesMapTable;

        /// <summary>
        /// Gets factory of Subcontract of the current service
        /// </summary>
        protected readonly IDependencyFactory DependencyFactory;

        /// <summary>
        /// Gets or sets factory of Subcontract of the current service
        /// </summary>
        protected readonly ISharedFactory SharedFactory;


        /// <summary>
        /// Initializes a new instance of <see cref="BaseServiceFactory"/> class with specified parameters. 
        /// </summary>
        /// <param name="namespaces">The array of namespaces to be searched for services.</param>
        /// <param name="assemblies">The array of assemblies to be searched for services.</param>
        /// <param name="strictMode">The value indicating whether strict mode is on or off, if strict mode is true then 
        /// system throws an exception if a contract is implemented by more than one service. this prevents the duplicate 
        /// implementation.
        /// </param>
        /// <param name="dependencyFactory">The dependency factory of the current service factory.</param>
        /// <param name="sharedFactory">The shared factory of the current service factory.</param>
        public BaseServiceFactory(string[] namespaces, Assembly[] assemblies, bool strictMode, IDependencyFactory dependencyFactory, ISharedFactory sharedFactory)
        {
            ServicesMapTable = new ContractServiceAutoMapper(namespaces, assemblies, strictMode);
            ServicesMapTable.Map();

            DependencyFactory = dependencyFactory;
            SharedFactory = sharedFactory;

        }

        /// <summary>
        /// Gets or sets the ContractObserver
        /// </summary>
        protected IContractObserver ContractObserver
        {
            get
            {
                return _observer;
            }
            set
            {
                if (_observer == null)
                {
                    _observer = value;
                    /*Set observers to all its sub contractors*/
                    if (DependencyFactory != null)
                        DependencyFactory.ContractObserver = _observer;
                }
                else
                {
                    ExceptionHelper.ThrowOverrideObserverExpection();
                }
            }
        }

        /// <summary>
        /// Adds service to the factory.
        /// </summary>
        /// <typeparam name="TC">The contract (interface/class)</typeparam>
        /// <typeparam name="TS">The service (class)</typeparam>
        protected void AddService<TC, TS>() where TC : class where TS : TC
        {
            AddServiceInternal<TC>(typeof(TS), null);
        }

        /// <summary>
        /// Adds the specified service to the factory.
        /// </summary>
        /// <typeparam name="T">The contact (interface/class)</typeparam>
        /// <param name="service">The type (class) of the service</param>
        protected virtual void AddService<T>(Type service) where T : class
        {
            AddServiceInternal<T>(service, null);
        }

        /// <summary>
        /// Adds service to the factory with the specified name. This allows you to 
        /// add multiple implementation of the same interface by providing unique service name to the each one of them.
        /// </summary>
        /// <typeparam name="TC"></typeparam>
        /// <typeparam name="TS"></typeparam>
        /// <param name="serviceName"></param>
        protected void AddService<TC, TS>(string serviceName) where TC : class where TS : TC
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                ExceptionHelper.ThrowArgumentNullException(nameof(serviceName));
            }

            AddServiceInternal<TC>(typeof(TS), serviceName);
        }

        /// <summary>
        /// Adds specified service with name.
        /// </summary>
        /// <typeparam name="T">The contract (interface/class)</typeparam>
        /// <param name="service">The service (implementation of contract)</param>
        /// <param name="serviceName">The name of the service in order to support multiple types of services of the same contract.</param>
        protected void AddService<T>(Type service, string serviceName) where T : class
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                ExceptionHelper.ThrowArgumentNullException(nameof(serviceName));
            }
            AddServiceInternal<T>(service, serviceName);
        }

        /// <summary>
        /// Adds specified <see cref="Expression"/> for the specified contract <typeparamref name="TC"/>
        /// </summary>
        /// <typeparam name="TC">Type of the contract.</typeparam>
        /// <param name="expression"><see cref="Func{TC}"/>, in which it must pass the instance of service.</param>
        protected void AddService<TC>(Expression<Func<TC>> expression) where TC : class
        {
            AddServiceInternal<TC>(expression, null);
        }

        /// <summary>
        /// Adds specified <see cref="Expression"/> for the specified contract <typeparamref name="TC"/>
        /// </summary>
        /// <typeparam name="TC">Type of the contract.</typeparam>
        /// <param name="expression"><see cref="Func{TC}"/>, in which it must pass the instance of service.</param>
        /// <param name="serviceName">Specifies name of the service</param>
        protected void AddService<TC>(Expression<Func<TC>> expression, string serviceName) where TC : class
        {
            if (serviceName == null)
            {
                throw new ArgumentNullException(nameof(serviceName));
            }

            AddServiceInternal<TC>(expression, serviceName);
        }

        /// <summary>
        /// Adds service as singleton in the factory.
        /// </summary>
        /// <typeparam name="TC">The contract type</typeparam>
        /// <typeparam name="TS">The service type</typeparam>
        protected void AddSingletonService<TC, TS>() where TC : class where TS : TC
        {
            AddSingletonServiceInternal<TC, TS>(null);
        }

        /// <summary>
        /// Adds service as singleton in the factory. 
        /// </summary>
        /// <typeparam name="TC">The contract type.</typeparam>
        /// <typeparam name="TS">The service type.</typeparam>
        /// <param name="serviceName">The name of the service.</param>
        protected void AddSingletonService<TC, TS>(string serviceName) where TC : class where TS : TC
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                ExceptionHelper.ThrowArgumentNullException(nameof(serviceName));
            }
            AddSingletonServiceInternal<TC, TS>(serviceName);
        }

        /// <summary>
        /// Replaces specified service (TS) for the specified contract (TC).
        /// </summary>
        /// <typeparam name="TC">Type of the contract</typeparam>
        /// <typeparam name="TS">Type of the service</typeparam>
        protected void ReplaceService<TC, TS>() where TC : class where TS : TC
        {
            ReplaceServiceInternal<TC>(typeof(TS), null);
        }

        /// <summary>
        /// Replaces the specified service in the factory.
        /// </summary>
        /// <typeparam name="T">The class of the service</typeparam>
        /// <param name="service"></param>
        /// <returns></returns>
        protected void ReplaceService<T>(Type service) where T : class
        {
            ReplaceServiceInternal<T>(service, null);
        }

        /// <summary>
        /// Replaces specified service (<typeparamref name="TS"/>) with service name for the specified contract (<typeparamref name="TC"/>).
        /// </summary>
        /// <typeparam name="TC">Type of the contract</typeparam>
        /// <typeparam name="TS">Type of the service</typeparam>
        /// <param name="serviceName">Type of service that's a implementation of specified contract.</param>
        protected void ReplaceService<TC, TS>(string serviceName) where TC : class where TS : TC
        {
            ReplaceServiceInternal<TC>(typeof(TS), serviceName);
        }

        /// <summary>
        /// Replaces specified service for the specified contract T.
        /// </summary>
        /// <typeparam name="T">Contract type(interface or class)</typeparam>
        /// <param name="service">Type of service that's a implementation of specified contract.</param>
        /// <param name="serviceName">The name of the service.</param>
        protected void ReplaceService<T>(Type service, string serviceName) where T : class
        {
            ReplaceServiceInternal<T>(service, serviceName);
        }

        /// <summary>
        /// Replaces specified singleton service for the specified contract T.
        /// </summary>
        /// <typeparam name="TC">The type of contract.</typeparam>
        /// <typeparam name="TS">The type of service.</typeparam>
        /// <param name="serviceName">The name of the service</param>
        protected void ReplaceSingletonService<TC, TS>(string serviceName) where TC : class where TS : TC
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                ExceptionHelper.ThrowArgumentNullException(nameof(serviceName));
            }
            ReplaceSingletonServiceInternal<TC, TS>(serviceName);
        }

        /// <summary>
        /// Replaces specified singleton service for the specified contract T.
        /// </summary>
        /// <typeparam name="TC">The type of contract.</typeparam>
        /// <typeparam name="TS">The type of service.</typeparam>
        protected void ReplaceSingletonService<TC, TS>() where TC : class where TS : TC
        {
            ReplaceSingletonServiceInternal<TC, TS>(null);
        }

        /// <summary>
        /// Replaces specified <see cref="Expression"/> for the specified contract <typeparamref name="TC"/>
        /// </summary>
        /// <typeparam name="TC">Type of the contract.</typeparam>
        /// <param name="expression"><see cref="Func{TC}"/>, in which it must pass the instance of service.</param>
        /// <returns>Returns <see cref="IDependencyFactory"/> </returns>
        protected void ReplaceService<TC>(Expression<Func<TC>> expression) where TC : class
        {
            ReplaceServiceInternal<TC>(expression, null);
        }

        /// <summary>
        /// Replaces specified <see cref="Expression"/> for the specified contract <typeparamref name="TC"/>
        /// </summary>
        /// <typeparam name="TC">The type of the contract.</typeparam>
        /// <param name="expression"><see cref="Func{TC}"/>, in which it must pass the instance of service.</param>
        /// <param name="serviceName">Specifies name of the service</param>
        /// <returns>Returns <see cref="IDependencyFactory"/> </returns>
        protected void ReplaceService<TC>(Expression<Func<TC>> expression, string serviceName) where TC : class
        {
            if (serviceName == null)
            {
                throw new ArgumentNullException(nameof(serviceName));
            }

            ReplaceServiceInternal<TC>(expression, serviceName);
        }
        
        /// <summary>
        /// Creates expression object for specified service type constructor.
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="serviceType"></param>
        /// <param name="registrar"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        protected virtual Expression CreateConstructorExpression(Type interfaceType, Type serviceType, ServiceRegister registrar, ServiceState state)
        {
            VerifyNull(interfaceType, nameof(interfaceType));
            VerifyNull(serviceType, nameof(serviceType));
            VerifyNull(registrar, nameof(registrar));
            VerifyNull(state, nameof(state));

            ConstructorInfo[] serviceConstructors = serviceType.GetConstructors();
            ConstructorInfo serviceConstructor = GetEligibleConstructorToInvoke(serviceConstructors);

            if (serviceConstructor != null)
            {
                Type[] reusableDependencies = GetReuseDependencies(serviceConstructor);

                /* Get IgnoreAttribute for the constructor, find the constructor parameters 
                  with the specified names, if parameter name is matched  then set NULL to the parameter. */
                var ignoreAttribute = serviceConstructor.GetCustomAttribute<IgnoreAttribute>();

                /*Get parameters of service's constructor and inject corresponding repositories values to it */
                ParameterInfo[] constructorParametersInfo = serviceConstructor.GetParameters();
                Expression[] arguments = new Expression[constructorParametersInfo.Length];

                int index = 0;
                foreach (ParameterInfo constrParameter in constructorParametersInfo)
                {
                    //if current parameter type is listed under reusable type, then get the expression
                    Expression reuseDependency = GetReuseDependencyIfExists(state, constrParameter.ParameterType);
                    if (reuseDependency != null)
                    {
                        arguments[index] = reuseDependency;
                    }
                    else
                    {
                         var dependencyArgument = GetDependencyObject(constrParameter, registrar, ignoreAttribute, state);
                        
                        //maintain state, if the parameter is listed in reusable list
                        arguments[index] = MaintainState(reusableDependencies, state, constrParameter.ParameterType, dependencyArgument);
                    }

                    index++;
                }
                return Expression.New(serviceConstructor, arguments);
            }

            //Default constructor
            return Expression.New(serviceType);
        }

        /// <summary>
        /// Creates dependency <see cref="Expression"/> object. if no dependency factory is set and shared factory exists then
        /// creates dependency from the shared factory; otherwise creates from the dependency factory.
        /// </summary>
        /// <param name="interfaceType">The type of the contract.</param>
        /// <param name="register">The service register object.</param>
        /// <param name="state">The service state.</param>
        /// <param name="serviceName"></param>
        /// <param name="attributes"></param>
        /// <returns>Dependency expression</returns>
        protected virtual Expression CreateDependency(Type interfaceType, ServiceRegister register, ServiceState state, string serviceName, TypeContextAttributes attributes)
        {
            //let's subcontract take responsibility to create dependency objects 
            if (DependencyFactory == null && SharedFactory != null)
            {
                return SharedFactory.Create(interfaceType, register, state, serviceName, attributes) ?? Expression.Default(interfaceType);
            }
            return DependencyFactory?.Create(interfaceType, register, state, serviceName, attributes) ?? Expression.Default(interfaceType);
        }

        /// <summary>
        /// Returns <see cref="BaseServiceInfo"/> object, if service is null then it returns default service info.
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        protected BaseServiceInfo GetServiceInfo(Type interfaceType, string serviceName)
        {
            BaseServiceInfo serviceInfo = null;

            if (!string.IsNullOrEmpty(serviceName))
            {
                serviceInfo = ServicesMapTable[interfaceType, serviceName];
            }
            else
            {
                serviceInfo = ServicesMapTable[interfaceType];
            }
            return serviceInfo;
        }

        private void VerifyNull(object o, string name)
        {
            if (o == null)
            {
                throw new ArgumentNullException(name);
            }
        }

        private Type[] GetReuseDependencies(ConstructorInfo serviceConstructor)
        {
            var reuseAttribute = serviceConstructor.GetCustomAttribute<ReuseDependencyAttribute>();
            if (reuseAttribute != null)
            {
                return reuseAttribute.ReuseTypes;
            }
            return null;
        }

       
        private Expression MaintainState(Type[] reusables, ServiceState state, Type contract, Expression expression)
        {
            if (reusables != null && reusables.Contains(contract))
            {
                return state.AddState(contract, expression); //return parameter expression
            }
            return expression; //if it's not re-usable then set original expression.
        }

        private Expression GetReuseDependencyIfExists(ServiceState state, Type contract)
        {
            return state[contract];
        }

        private ConstructorInfo GetEligibleConstructorToInvoke(ConstructorInfo[] serviceConstructors)
        {
            foreach (var serviceConstructor in serviceConstructors)
            {
                var attribute = serviceConstructor.GetCustomAttribute<ServiceInitAttribute>();
                if (attribute != null)
                {
                    return serviceConstructor;
                }
            }

            //if no constructor is matched, and if exists only one constructor, then return it.
            if (serviceConstructors.Length == 1)
            {
                return serviceConstructors[0];
            }
            return null;
        }

        private Expression GetDependencyObject(ParameterInfo constrParameter, ServiceRegister registrar, IgnoreAttribute ignoreAttribute, ServiceState state)
        {
            bool ignore = ignoreAttribute?.ParameterNames.Contains(constrParameter.Name) ?? false;

            if (ignore)
            {
                return Expression.Default(constrParameter.ParameterType);
            }
            else
            {
                var mappedServiceName = GetMappedServiceName(constrParameter);

                /*
                 * if container model is "Single" then it will search within the same container not in dependencies.
                 * if parameter is decorated with FromSelfAttribute, then find object within the current factory */
                if (IsSelfDependent(constrParameter))
                {
                    //Find it in same 
                    return GetObjectFromSelf(constrParameter.ParameterType, registrar, state, mappedServiceName);
                }

                /*if parameter is decorated with FromDependencyAttribute, then find object within the specified factory */
                string factoryName;
                if (HasDependencyAttribute(constrParameter, out factoryName))
                {
                    return GetObjectFromDependencyFactory(constrParameter.ParameterType, factoryName, registrar, state, mappedServiceName, new TypeContextAttributes(constrParameter.GetCustomAttributes()));
                }

                //let's subcontract take responsibility to create dependency objects 
                return CreateDependency(constrParameter.ParameterType, registrar, state, mappedServiceName, new TypeContextAttributes(constrParameter.GetCustomAttributes()));
            }
        }

        private bool IsSelfDependent(ParameterInfo constrParameter)
        {
            //FromSelf
            var fromSelfAttr = constrParameter.GetCustomAttribute<InternalAttribute>();
            return fromSelfAttr != null;
        }

        private string GetMappedServiceName(ParameterInfo constrParameter)
        {
            var mapServiceAttr = constrParameter.GetCustomAttribute<MapServiceAttribute>();
            return mapServiceAttr?.Name;
        }

        private bool HasDependencyAttribute(ParameterInfo constrParameter, out string factoryName)
        {
            //FromSelf
            var fromDepAttr = constrParameter.GetCustomAttribute<ExternalAttribute>();
            factoryName = fromDepAttr?.ContainerName;
            return fromDepAttr != null;
        }

        private Expression GetObjectFromDependencyFactory(Type interfaceType, string factoryName, ServiceRegister registrar, ServiceState state, string mappedServiceName, TypeContextAttributes attributes)
        {
            var subcontractFactory = ServiceProviderHelper.GetDependencyFactory(DependencyFactory, factoryName);
            return subcontractFactory?.Create(interfaceType, registrar, state, mappedServiceName, attributes) ?? Expression.Default(interfaceType);
        }

        

        private Expression GetObjectFromSelf(Type interfaceType, ServiceRegister registrar, ServiceState state, string serviceName)
        {
            var serviceInfo = GetServiceInfo(interfaceType, serviceName);

            if (serviceInfo == null)
            {
                ExceptionHelper.ThrowContractNotRegisteredException(interfaceType.FullName);
            }

            var instanceCallback = serviceInfo.GetServiceInstanceCallback<object>();
            if (instanceCallback != null)
            {
                throw new Exception("Internal service injection does not support for Func<> delegate.");
            }

            var expression = serviceInfo.GetServiceInstanceExpression();

            if (expression == null)
            {
                expression = CreateConstructorExpression(interfaceType, serviceInfo.ServiceType, registrar, state);
            }

            return expression;
        }

        

        private void AddServiceInternal<T>(Type service, string serviceName) where T : class
        {
            Type interfaceType = typeof(T);
            var serviceMeta = new ServiceInfo(service, BaseServiceInfo.GetDecorators(interfaceType), serviceName);
            ServicesMapTable.Add(interfaceType, serviceMeta);

            //send update to observer
            ContractObserver.Update(interfaceType);
        }

        private void AddServiceInternal<TC>(Expression<Func<TC>> expression, string serviceName) where TC : class
        {
            Type interfaceType = typeof(TC);
            var serviceMeta = new ServiceInfo<TC, TC>(expression, BaseServiceInfo.GetDecorators(interfaceType), serviceName);

            ServicesMapTable.Add(interfaceType, serviceMeta);

            //send update to observer
            ContractObserver.Update(interfaceType);
        }

        private void ReplaceServiceInternal<T>(Type service, string serviceName) where T : class
        {
            Type interfaceType = typeof(T);
            ServicesMapTable.AddOrReplace(interfaceType, new ServiceInfo(service, BaseServiceInfo.GetDecorators(interfaceType), serviceName));

            //send update to observer
            ContractObserver.Update(interfaceType);
        }

        private void ReplaceServiceInternal<TC>(Expression<Func<TC>> expression, string serviceName) where TC : class
        {
            Type interfaceType = typeof(TC);
            var serviceMeta = new ServiceInfo<TC, TC>(expression, BaseServiceInfo.GetDecorators(interfaceType), serviceName);

            ServicesMapTable.AddOrReplace(interfaceType, serviceMeta);

            //send update to observer
            ContractObserver.Update(interfaceType);
        }

        private void AddSingletonServiceInternal<TC, TS>(string serviceName) where TC : class where TS : TC
        {
            Type interfaceType = typeof(TC);
            var serviceMeta = new ServiceInfo<TC, TS>(isReusable: true, decorators: BaseServiceInfo.GetDecorators(interfaceType), serviceName: serviceName);

            ServicesMapTable.Add(interfaceType, serviceMeta);

            //send update to observer
            ContractObserver.Update(interfaceType);

        }

        private void ReplaceSingletonServiceInternal<TC, TS>(string serviceName) where TC : class where TS : TC
        {
            Type interfaceType = typeof(TC);
            var serviceMeta = new ServiceInfo<TC, TS>(isReusable: true, decorators: BaseServiceInfo.GetDecorators(interfaceType), serviceName: serviceName);

            ServicesMapTable.AddOrReplace(interfaceType, serviceMeta);

            //send update to observer
            ContractObserver.Update(interfaceType);
        }

    }
}
