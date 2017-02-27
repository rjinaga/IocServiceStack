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
    using System.Collections.Generic;

    /// <summary>
    /// Represents default service provider, which gets the service from the internal 
    /// service factory of container.
    /// </summary>
    public class DefaultServiceProvider : IServiceProvider
    {
        private InternalServiceManager _internalsm;

        /// <summary>
        /// Initializes a new instance of <see cref="DefaultServiceProvider"/> class with specified 
        /// configuration.
        /// </summary>
        /// <param name="config"></param>
        public DefaultServiceProvider(ContainerConfig config)
        {
            _internalsm = new InternalServiceManager(config);
        }

        /// <summary>
        /// Gets or set <see cref="IDecoratorManager"/>
        /// </summary>
        public IDecoratorManager DecoratorManager
        {
            get; set;
        }

        /// <summary>
        /// Returns service for the requested contract.
        /// </summary>
        /// <typeparam name="T">The type of the contract.</typeparam>
        /// <returns>Returns instance of service that is associated with the specified contract.</returns>
        public virtual T GetService<T>() where T : class
        {
            InvocationInfo info = BeforeInvoke(typeof(T), null);

            //if no service information is returned, we cannot process further return with default value.
            if (info.ServiceInfo == null)
            {
                return default(T);
            }

            var objectInstance = _internalsm.GetServiceFactory()?.Create<T>(info.ServiceInfo);

            AfterInvoke(objectInstance, info.ServiceCallContext, info.ServiceInfo.Decorators);

            return objectInstance;

        }

        /// <summary>
        /// Returns service for the specified contract with name. When there are more implementations for a single contract
        /// then services need to be registered and separated by name.
        /// </summary>
        /// <typeparam name="T">The type of the contract.</typeparam>
        /// <param name="serviceName">Specify name of the service.</param>
        /// <returns>Returns instance of service that is associated with the specified contract.</returns>
        public virtual T GetService<T>(string serviceName) where T : class
        {
            if (string.IsNullOrWhiteSpace(serviceName))
            {
                ExceptionHelper.ThrowArgumentNullException(nameof(serviceName));
            }

            InvocationInfo info = BeforeInvoke(typeof(T), serviceName);

            //if no service information is returned, we cannot process further return with default value.
            if (info.ServiceInfo == null)
            {
                return default(T);
            }

            var objectInstance = _internalsm.GetServiceFactory()?.Create<T>(info.ServiceInfo);

            AfterInvoke(objectInstance, info.ServiceCallContext, info.ServiceInfo.Decorators);

            return objectInstance;

        }

        /// <summary>
        /// Returns service for the requested contract.
        /// </summary>
        /// <param name="contractType">The type of the contract.</param>
        /// <returns>Returns instance of service that is associated with the specified contract.</returns>
        public virtual object GetService(Type contractType)
        {
            if (contractType == null)
            {
                ExceptionHelper.ThrowArgumentNullException(nameof(contractType));
            }

            InvocationInfo info = BeforeInvoke(contractType, null);

            //if no service information is returned, we cannot process further return with default value.
            if (info.ServiceInfo == null)
            {
                return null;
            }

            var objectInstance = _internalsm.GetServiceFactory()?.Create(contractType, info.ServiceInfo);

            AfterInvoke(objectInstance, info.ServiceCallContext, info.ServiceInfo.Decorators);

            return objectInstance;
        }

        /// <summary>
        /// Returns service for the specified contract with name. When there are more implementations for a single contract
        /// then services need to be registered and separated by name.
        /// </summary>
        /// <param name="contractType">The type of the contract.</param>
        /// <param name="serviceName">Specify name of the service.</param>
        /// <returns>Returns instance of service that is associated with the specified contract.</returns>
        public virtual object GetService(Type contractType, string serviceName)
        {
            if (string.IsNullOrWhiteSpace(serviceName))
            {
                ExceptionHelper.ThrowArgumentNullException(nameof(serviceName));
            }

            InvocationInfo info = BeforeInvoke(contractType, serviceName);

            //if no service information is returned, we cannot process further return with default value.
            if (info.ServiceInfo == null)
            {
                return null;
            }

            var objectInstance = _internalsm.GetServiceFactory()?.Create(contractType, info.ServiceInfo);

            AfterInvoke(objectInstance, info.ServiceCallContext, info.ServiceInfo.Decorators);

            return objectInstance;
        }

        /// <summary>
        /// Returns root service factory of the container.
        /// </summary>
        /// <returns></returns>
        public virtual IRootServiceFactory GetRootServiceFactory()
        {
            return _internalsm.GetServiceFactory();
        }

        
        /// <summary>
        /// Returns dependency factory of the root factory by specified name.
        /// </summary>
        /// <param name="name">Specify name of the dependency service.</param>
        /// <returns></returns>
        public IDependencyFactory GetDependencyFactory(string name)
        {
            return GetRootServiceFactory()?.GetDependencyFactory(name);
        }

        /// <summary>
        /// Returns shared factory.
        /// </summary>
        /// <returns></returns>
        public ISharedFactory GetSharedFactory()
        {
            return GetRootServiceFactory().GetSharedFactory();
        }

        /// <summary>
        /// Executes before initiating the service instance.
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        protected InvocationInfo BeforeInvoke(Type contractType, string serviceName)
        {
            var factory = _internalsm.GetServiceFactory();
            BaseServiceInfo serviceInfo = factory.GetServiceInfo(contractType, serviceName);

            if (serviceInfo == null)
            {
                return default(InvocationInfo);
            }

            ServiceCallContext callContext = ServiceCallContext.Create(contractType, serviceInfo.ServiceType, this);
            InvokeDecorator(callContext, InvocationCase.Before, serviceInfo.Decorators);

            return new InvocationInfo() { ServiceInfo = serviceInfo, ServiceCallContext = callContext };
        }

        /// <summary>
        /// Executes after initiating the service instance.
        /// </summary>
        /// <param name="objectInstance"></param>
        /// <param name="context"></param>
        /// <param name="localDecorators"></param>
        protected void AfterInvoke(object objectInstance, ServiceCallContext context, IEnumerable<DecoratorAttribute> localDecorators)
        {
            context.ServiceInstance = objectInstance;
            InvokeDecorator(context, InvocationCase.After, localDecorators);
        }

        private void InvokeDecorator(ServiceCallContext context, InvocationCase @case, IEnumerable<DecoratorAttribute> localDecorators)
        {
            DecoratorManager.Execute(context, @case, localDecorators);
        }

        /// <summary>
        /// This class contains invocation information related objects.
        /// </summary>
        protected struct InvocationInfo
        {
            /// <summary>
            /// Gets or sets service info
            /// </summary>
            public BaseServiceInfo ServiceInfo;

            /// <summary>
            /// Gets or sets service call context.
            /// </summary>
            public ServiceCallContext ServiceCallContext;
        }
    }
}
