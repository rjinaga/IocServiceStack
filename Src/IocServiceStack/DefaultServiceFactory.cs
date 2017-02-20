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
    using System.Reflection;

    /// <summary>
    /// Represents system default root service factory.
    /// </summary>
    public class DefaultServiceFactory : BaseServiceFactory, IRootServiceFactory
    {
        private ServiceNotifier _notifier;
        private ServiceCompiler _compiler;

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultServiceFactory"/> class with specified parameters.
        /// </summary>
        /// <param name="namespaces">Specify array of namespaces where services to be searched within specified assemblies.</param>
        /// <param name="assemblies">Specify array of assemblies where services  to be searched.</param>
        /// <param name="strictMode">The value indicating whether strict mode is on or off, if strict mode is true then 
        /// system throws an exception if a contract is implemented by more than one service. this prevents the duplicate 
        /// implementation.
        /// </param>
        /// <param name="dependencyFactory">The dependency factory of the current service factory.</param>
        /// <param name="sharedFactory">The shared factory of the current service factory.</param>
        public DefaultServiceFactory(string[] namespaces, Assembly[] assemblies, bool strictMode, IDependencyFactory dependencyFactory, ISharedFactory sharedFactory)
            : base(namespaces, assemblies, strictMode, dependencyFactory, sharedFactory)
        {

        }

        /// <summary>
        /// Returns <see cref="BaseServiceInfo"/> of specified <paramref name="contractType"/> and <paramref name="serviceName"/>.
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public new BaseServiceInfo GetServiceInfo(Type contractType, string serviceName)
        {
            if (!ServicesMapTable.Contains(contractType))
                throw ExceptionHelper.ThrowServiceNotRegisteredException(contractType.Name);

            if (string.IsNullOrEmpty(serviceName))
            {
                return ServicesMapTable[contractType];
            }
            {
                return ServicesMapTable[contractType, serviceName];
            }
        }

        /// <summary>
        /// Returns <see cref="BaseServiceInfo"/> of specified <paramref name="contractType"/>.
        /// </summary>
        /// <param name="contractType"></param>
        /// <returns></returns>
        public virtual BaseServiceInfo GetServiceInfo(Type contractType)
        {
            return GetServiceInfo(contractType, null);
        }

        /// <summary>
        /// Creates associated service instance of specified contract <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Specify contact type.</typeparam>
        /// <param name="serviceMeta"></param>
        /// <returns></returns>
        public virtual T Create<T>(BaseServiceInfo serviceMeta) where T : class
        {
            if (serviceMeta != null)
            {
                if (serviceMeta.Activator == null)
                {
                    Type interfaceType = typeof(T);
                    //Compile
                    _compiler.Compile<T>(interfaceType, serviceMeta, CreateConstructorExpression);
                }
                return serviceMeta.Activator.CreateInstance<T>();
            }
            return default(T);
        }

        /// <summary>
        /// Creates associated service instance of specified contract.
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="serviceMeta"></param>
        /// <returns></returns>
        public object Create(Type contractType, BaseServiceInfo serviceMeta)
        {
            if (serviceMeta != null)
            {
                if (serviceMeta.Activator == null)
                {
                    //Compile
                    _compiler.Compile<object>(contractType, serviceMeta, CreateConstructorExpression);
                }
                return serviceMeta.Activator.CreateInstance<object>();
            }
            return null;
        }

        /// <summary>
        /// Initializes the service factory and its dependencies.
        /// </summary>
        public void Initialize()
        {
            ContractObserver = new DefaultContractWorkObserver();
            _notifier = new ServiceNotifier();
            _compiler = new ServiceCompiler(_notifier);

            //set contract observer to SharedFactory
            SharedFactory.ContractObserver = ContractObserver;

            ContractObserver.OnUpdate((type) =>
            {
                _notifier.SendUpdate(type);
            });

        }

        /// <summary>
        /// Returns dependency factory of specified <paramref name="factoryName"/>.
        /// </summary>
        /// <param name="factoryName"></param>
        /// <returns></returns>
        public IDependencyFactory GetDependencyFactory(string factoryName)
        {
            return ServiceProviderHelper.GetDependencyFactory(DependencyFactory, factoryName);
        }

        /// <summary>
        /// Returns shared factory.
        /// </summary>
        /// <returns></returns>
        public ISharedFactory GetSharedFactory()
        {
            return SharedFactory;
        }

        /// <summary>
        /// Adds specified <see cref="Func{TC}"/> to the factory which must return the instance of the service.
        /// </summary>
        /// <typeparam name="TC"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IRootContainer Add<TC>(Func<TC> expression) where TC: class
        {
            return AddInternal<TC>(expression, null);
        }

        /// <summary>
        /// Adds specified <see cref="Func{TC}"/> to the factory with the <paramref name="serviceName"/>, which must return the instance of the service.
        /// </summary>
        /// <typeparam name="TC"></typeparam>
        /// <param name="expression"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public IRootContainer Add<TC>(Func<TC> expression, string serviceName) where TC : class
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                ExceptionHelper.ThrowArgumentNullException(nameof(serviceName));
            }

            return AddInternal<TC>(expression, serviceName);
        }

        /// <summary>
        /// Replaces specified <see cref="Func{TC}"/> to the factory which must return the instance of the service.
        /// </summary>
        /// <typeparam name="TC"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public IRootContainer Replace<TC>(Func<TC> expression) where TC : class
        {
            return ReplaceInternal<TC>(expression, null);
        }

        /// <summary>
        /// Replaces specified <see cref="Func{TC}"/> to the factory with the <paramref name="serviceName"/>, which must return the instance of the service.
        /// </summary>
        /// <typeparam name="TC"></typeparam>
        /// <param name="expression"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        public IRootContainer Replace<TC>(Func<TC> expression, string serviceName) where TC : class
        {
            if (string.IsNullOrEmpty(serviceName))
            {
                ExceptionHelper.ThrowArgumentNullException(nameof(serviceName));
            }

            return ReplaceInternal<TC>(expression, serviceName);
        }

        /// <summary>
        /// Adds specified service (TS) for the specified contract (TC) to the collection.
        /// </summary>
        /// <typeparam name="TC">Type of the contract</typeparam>
        /// <typeparam name="TS">Type of the service</typeparam>
        /// <returns>Returns <see cref="IRootContainer"/> </returns>
        public IRootContainer Add<TC, TS>()
           where TC : class
           where TS : TC
        {
            AddService<TC, TS>();
            return this;
        }

        /// <summary>
        /// Adds specified service (TS) with service name for the specified contract (TC) to the collection.
        /// </summary>
        /// <typeparam name="TC">Type of the contract</typeparam>
        /// <typeparam name="TS">Type of the service</typeparam>
        /// <param name="serviceName">Type of service that's a implementation of specified contract.</param>
        /// <returns>Returns <see cref="IRootContainer"/> </returns>
        public IRootContainer Add<TC, TS>(string serviceName)
            where TC : class
            where TS : TC
        {
            AddService<TC, TS>(serviceName);
            return this;
        }

        /// <summary>
        /// Adds specified service for the specified contract T to the collection.
        /// </summary>
        /// <typeparam name="T">Contract type(interface or class)</typeparam>
        /// <param name="service">Type of service that's a implementation of specified contract.</param>
        /// <returns>Returns <see cref="IRootContainer"/> </returns>
        public IRootContainer Add<T>(Type service) where T : class
        {
            AddService<T>(service);
            return this;
        }

        /// <summary>
        /// Adds specified service with a name for the specified contract T to the collection.
        /// This allows to register multiple implementations of same contract those are accessed by a name.
        /// </summary>
        /// <typeparam name="T">Contract type(interface or class)</typeparam>
        /// <param name="service">Type of service that's a implementation of specified contract.</param>
        /// <param name="serviceName">Specify name of the service</param>
        /// <returns>Returns <see cref="IRootContainer"/> </returns>
        public IRootContainer Add<T>(Type service, string serviceName) where T : class
        {
            AddService<T>(service, serviceName);
            return this;
        }

        /// <summary>
        /// Adds specified service (TS) as singleton object for the specified contract (TC) to the collection.
        /// </summary>
        /// <typeparam name="TC">Type of the contract</typeparam>
        /// <typeparam name="TS">Type of the service</typeparam>
        /// <returns>Returns <see cref="IRootContainer"/> </returns>
        public IRootContainer AddSingleton<TC, TS>()
            where TC : class
            where TS : TC
        {
            AddSingletonService<TC, TS>();
            return this;
        }

        /// <summary>
        /// Adds specified service as singleton object with a name for the specified contract T to the collection.
        /// This allows to register multiple implementations of same contract those are accessed by a name.
        /// </summary>
        /// <typeparam name="TC">Type of the contract</typeparam>
        /// <typeparam name="TS">Type of the service</typeparam>
        /// <param name="serviceName">Specify name of the service.</param>
        /// <returns>Returns <see cref="IRootContainer"/> </returns>
        public IRootContainer AddSingleton<TC, TS>(string serviceName)
            where TC : class
            where TS : TC
        {
            AddSingletonService<TC, TS>(serviceName);
            return this;
        }

        /// <summary>
        /// Replaces specified service (TS) for the specified contract (TC).
        /// </summary>
        /// <typeparam name="TC">Type of the contract</typeparam>
        /// <typeparam name="TS">Type of the service</typeparam>
        /// <returns>Returns <see cref="IRootContainer"/> </returns>
        public IRootContainer Replace<TC, TS>()
            where TC : class
            where TS : TC
        {
            ReplaceService<TC, TS>();
            return this;
        }

        /// <summary>
        /// Replaces specified service (<typeparamref name="TS"/>) with service name for the specified contract (<typeparamref name="TC"/>).
        /// </summary>
        /// <typeparam name="TC">Type of the contract</typeparam>
        /// <typeparam name="TS">Type of the service</typeparam>
        /// <param name="serviceName">Type of service that's a implementation of specified contract.</param>
        /// <returns>Returns <see cref="IRootContainer"/> </returns>
        public IRootContainer Replace<TC, TS>(string serviceName)
            where TC : class
            where TS : TC
        {
            ReplaceService<TC, TS>(serviceName);
            return this;
        }

        /// <summary>
        /// Replaces specified service for the specified contract T.
        /// </summary>
        /// <typeparam name="T">Contract type(interface or class)</typeparam>
        /// <param name="service">Type of service that's a implementation of specified contract.</param>
        /// <returns>Returns <see cref="IRootContainer"/> </returns>
        public IRootContainer Replace<T>(Type service) where T : class
        {
            ReplaceService<T>(service);
            return this;
        }

        /// <summary>
        /// Replaces specified service (TS) with service name for the specified contract (TC).
        /// </summary>
        /// <typeparam name="T">Type of the contract</typeparam>
        /// <param name="service">Type of the service</param>
        /// <param name="serviceName">Type of service that's a implementation of specified contract.</param>
        /// <returns>Returns <see cref="IRootContainer"/> </returns>
        public IRootContainer Replace<T>(Type service, string serviceName) where T : class
        {
            ReplaceService<T>(service, serviceName);
            return this;
        }


        /// <summary>
        /// Replaces specified service (TS) as singleton object for the specified contract (TC).
        /// </summary>
        /// <typeparam name="TC">Type of the contract</typeparam>
        /// <typeparam name="TS">Type of the service</typeparam>
        /// <returns>Returns <see cref="IRootContainer"/> </returns>
        public IRootContainer ReplaceSingleton<TC, TS>()
            where TC : class
            where TS : TC
        {
            ReplaceSingletonService<TC, TS>();
            return this;
        }

        /// <summary>
        /// Replaces specified service (<typeparamref name="TS"/>) with service name as singleton object for the specified contract (<typeparamref name="TC"/>).
        /// </summary>
        /// <typeparam name="TC">Type of the contract</typeparam>
        /// <typeparam name="TS">Type of the service</typeparam>
        /// <param name="serviceName">Type of service that's a implementation of specified contract.</param>
        /// <returns>Returns <see cref="IRootContainer"/> </returns>
        public IRootContainer ReplaceSingleton<TC, TS>(string serviceName)
            where TC : class
            where TS : TC
        {
            ReplaceSingletonService<TC, TS>(serviceName);
            return this;
        }

        private IRootContainer AddInternal<TC>(Func<TC> serviceAction, string serviceName) where TC : class
        {
            Type interfaceType = typeof(TC);
            var serviceMeta = new ServiceInfo<TC,TC>(serviceAction, BaseServiceInfo.GetDecorators(interfaceType), serviceName);

            ServicesMapTable.Add(interfaceType, serviceMeta);

            //send update to observer
            ContractObserver.Update(interfaceType);

            return this;
        }

        private IRootContainer ReplaceInternal<TC>(Func<TC> expression, string serviceName) where TC : class
        {
            Type interfaceType = typeof(TC);
            var serviceMeta = new ServiceInfo<TC,TC>(expression, BaseServiceInfo.GetDecorators(interfaceType), serviceName);

            ServicesMapTable.AddOrReplace(interfaceType,serviceMeta);

            //send update to observer
            ContractObserver.Update(interfaceType);

            return this;
        }
      
    }
}
