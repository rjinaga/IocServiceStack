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
    /// Represents abstract factory of container service
    /// </summary>
    public abstract class BaseServiceFactory : IDependencyAttribute
    {
        private IContractObserver _observer;

        /// <summary>
        /// Get read-only ServiceMapTable object
        /// </summary>
        protected readonly ContractServiceAutoMapper ServicesMapTable;

        /// <summary>
        /// Initializes a new instance of AbstractFactory class with the specified namespaces, assemblies and strictmode
        /// </summary>
        /// <param name="namespaces">The namespaces of services</param>
        /// <param name="assemblies">The assemblies of services</param>
        /// <param name="strictMode">The value indicating whether strict mode is on or off</param>
        public BaseServiceFactory(string[] namespaces, Assembly[] assemblies, bool strictMode)
        {
            ServicesMapTable = new ContractServiceAutoMapper(namespaces, assemblies, strictMode);
            ServicesMapTable.Map();
        }

        /// <summary>
        /// Gets or sets factory of Subcontract of the current service
        /// </summary>
        public IDependencyFactory DependencyFactory
        {
            get; set;
        }

        /// <summary>
        /// 
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

        protected void AddService<TC, TS>() where TC : class where TS : TC
        {
            AddInternal<TC>(typeof(TS), null);
        }

        /// <summary>
        /// Adds the specified service to the factory
        /// </summary>
        /// <typeparam name="T">The class of the service</typeparam>
        /// <param name="service">The type of the service</param>
        /// <returns>Instance <see cref="IBaseServiceFactory"/> of current object</returns>
        protected virtual void AddService<T>(Type service) where T : class
        {
            AddInternal<T>(service, null);
        }

        protected void AddService<TC, TS>(string serviceName) where TC : class where TS : TC
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                ExceptionHelper.ThrowArgumentNullException(nameof(serviceName));
            }

            AddInternal<TC>(typeof(TS), serviceName);
        }

        protected void AddService<T>(Type service, string serviceName) where T : class
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                ExceptionHelper.ThrowArgumentNullException(nameof(serviceName));
            }
            AddInternal<T>(service, serviceName);
        }

        protected void AddSingletonService<TC, TS>() where TC : class where TS : TC
        {
            AddSingletonInternal<TC, TS>(null);
        }

        protected void AddSingletonService<TC, TS>(string serviceName) where TC : class where TS : TC
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                ExceptionHelper.ThrowArgumentNullException(nameof(serviceName));
            }
            AddSingletonInternal<TC, TS>(serviceName);
        }

        protected void ReplaceService<TC, TS>() where TC : class where TS : TC
        {
            ReplaceInternal<TC>(typeof(TS), null);
        }

        /// <summary>
        /// Replaces the specified service in the factory.
        /// </summary>
        /// <typeparam name="T">The class of the service</typeparam>
        /// <param name="service"></param>
        /// <returns></returns>
        protected void ReplaceService<T>(Type service) where T : class
        {
            ReplaceInternal<T>(service, null);
        }

        protected void ReplaceService<TC, TS>(string serviceName) where TC : class where TS : TC
        {
             ReplaceInternal<TC>(typeof(TS), serviceName);
        }

        protected void ReplaceService<T>(Type service, string serviceName) where T : class
        {
             ReplaceInternal<T>(service, serviceName);
        }

        protected void ReplaceSingletonService<TC, TS>(string serviceName) where TC : class where TS : TC
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                ExceptionHelper.ThrowArgumentNullException(nameof(serviceName));
            }
             ReplaceSingletonInternal<TC, TS>(serviceName);
        }

        protected void ReplaceSingletonService<TC, TS>() where TC : class where TS : TC
        {
             ReplaceSingletonInternal<TC, TS>(null);
        }


        protected void AddService<TC>(Expression<Func<TC>> expression) where TC : class
        {
            Type interfaceType = typeof(TC);
            var serviceMeta = new ServiceInfo<TC, TC>(expression, ServiceInfo.GetDecorators(interfaceType), null);

            ServicesMapTable.Add(interfaceType, serviceMeta);

            //send update to observer
            ContractObserver.Update(interfaceType);
        }

        protected void AddService<TC>(Expression<Func<TC>> expression, string serviceName) where TC : class
        {
            throw new NotImplementedException();
        }

        protected void ReplaceService<TC>(Expression<Func<TC>> expression) where TC : class
        {
            Type interfaceType = typeof(TC);
            var serviceMeta = new ServiceInfo<TC, TC>(expression, ServiceInfo.GetDecorators(interfaceType), null);

            ServicesMapTable.AddOrReplace(interfaceType, serviceMeta);

            //send update to observer
            ContractObserver.Update(interfaceType);
        }

        protected void ReplaceService<TC>(Expression<Func<TC>> expression, string serviceName) where TC : class
        {
            throw new NotImplementedException();
        }

        protected Expression CreateConstructorExpression(Type interfaceType, Type serviceType, ServiceRegistrar registrar)
        {
            ConstructorInfo[] serviceConstructors = serviceType.GetConstructors();
            foreach (var serviceConstructor in serviceConstructors)
            {
                var attribute = serviceConstructor.GetCustomAttribute<ServiceInitAttribute>();
                if (attribute != null)
                {
                    /* Get IgnoreAttribute for the constructor, find the constructor parameters 
                      with the specified names, if parameter name is matched  then set NULL to the parameter. */
                    var ignoreAttribute = serviceConstructor.GetCustomAttribute<IgnoreAttribute>();

                    /*Get parameters of service's constructor and inject corresponding repositories values to it */
                    ParameterInfo[] constructorParametersInfo = serviceConstructor.GetParameters();
                    Expression[] arguments = new Expression[constructorParametersInfo.Length];

                    int index = 0;
                    foreach (ParameterInfo constrParameter in constructorParametersInfo)
                    {
                        arguments[index] = GetDependencyObject(constrParameter, registrar, ignoreAttribute);
                        index++;
                    }
                    return Expression.New(serviceConstructor, arguments);
                }
            }

            //Default constructor
            return Expression.New(serviceType);
        }

        private Expression GetDependencyObject(ParameterInfo constrParameter, ServiceRegistrar registrar, IgnoreAttribute ignoreAttribute)
        {
            bool ignore = ignoreAttribute?.ParameterNames.Contains(constrParameter.Name) ?? false;

            if (ignore)
            {
                return Expression.Default(constrParameter.ParameterType);
            }
            else
            {
                /*if parameter is decorated with FromSelfAttribute, then find object within the current factory */
                if (IsSelfDependent(constrParameter))
                {
                    //Find it in same 
                    return GetObjectFromSelf(constrParameter.ParameterType, registrar);
                }

                /*if parameter is decorated with FromDependencyAttribute, then find object within the specified factory */
                string factoryName;
                if (HasDependencyAttribute(constrParameter, out factoryName))
                {
                    return GetObjectFromDependencyFactory(constrParameter.ParameterType, factoryName, registrar);
                }

                //let's subcontract take responsibility to create dependency objects 
                return DependencyFactory?.Create(constrParameter.ParameterType, registrar) ?? Expression.Default(constrParameter.ParameterType);
            }
        }

        private bool IsSelfDependent(ParameterInfo constrParameter)
        {
            //FromSelf
            var fromSelfAttr = constrParameter.GetCustomAttribute<FromSelfAttribute>();
            return fromSelfAttr != null;
        }

        private bool HasDependencyAttribute(ParameterInfo constrParameter, out string factoryName)
        {
            //FromSelf
            var fromDepAttr = constrParameter.GetCustomAttribute<FromDependencyAttribute>();
            factoryName = fromDepAttr?.FactoryName;
            return fromDepAttr != null;
        }

        private Expression GetObjectFromDependencyFactory(Type interfaceType, string factoryName, ServiceRegistrar registrar)
        {
            var subcontractFactory = ServiceProviderHelper.GetDependencyFactory(DependencyFactory, factoryName);
            return subcontractFactory?.Create(interfaceType, registrar) ?? Expression.Default(interfaceType);
        }
        
        private Expression GetObjectFromSelf(Type interfaceType, ServiceRegistrar registrar)
        {
            var serviceInfo = ServicesMapTable[interfaceType];
            return CreateConstructorExpression(interfaceType, serviceInfo.ServiceType, registrar);
        }

        private void AddInternal<T>(Type service, string serviceName) where T : class
        {
            Type interfaceType = typeof(T);
            var serviceMeta = new ServiceInfo(service, ServiceInfo.GetDecorators(interfaceType), serviceName);
            ServicesMapTable.Add(interfaceType, serviceMeta);

            //send update to observer
            ContractObserver.Update(interfaceType);

            
        }

        private void ReplaceInternal<T>(Type service, string serviceName) where T : class
        {
            Type interfaceType = typeof(T);
            ServicesMapTable.AddOrReplace(interfaceType, new ServiceInfo(service, ServiceInfo.GetDecorators(interfaceType), serviceName));

            //send update to observer
            ContractObserver.Update(interfaceType);

            
        }

        private void AddSingletonInternal<TC, TS>(string serviceName) where TC : class where TS : TC 
        {
            Type interfaceType = typeof(TC);
            var serviceMeta = new ServiceInfo<TC,TS>(isReusable: true, decorators: ServiceInfo.GetDecorators(interfaceType), serviceName: serviceName);

            ServicesMapTable.Add(interfaceType, serviceMeta);

            //send update to observer
            ContractObserver.Update(interfaceType);
            
        }

        private void ReplaceSingletonInternal<TC, TS>(string serviceName) where TC : class where TS : TC
        {
            Type interfaceType = typeof(TC);
            var serviceMeta = new ServiceInfo<TC,TS>(isReusable: true, decorators: ServiceInfo.GetDecorators(interfaceType), serviceName: serviceName);

            ServicesMapTable.AddOrReplace(interfaceType, serviceMeta);

            //send update to observer
            ContractObserver.Update(interfaceType);

            
        }

      
    }
}
