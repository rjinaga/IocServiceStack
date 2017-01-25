#region License
// Copyright (c) 2016 Rajeswara-Rao-Jinaga
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

    public class DefaultServiceProvider : IServiceProvider
    {
        private InternalServiceManager _internalsm;

        public DefaultServiceProvider(ServiceConfig config)
        {
            _internalsm = new InternalServiceManager(config);
        }

        public IDecoratorManager DecoratorManager
        {
            get; set;
        }

        public IocContainer IocContainer
        {
            get; set;
        }

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

        public virtual IServiceFactory GetServiceFactory()
        {
            return _internalsm.GetServiceFactory();
        }

        /// <summary>
        /// Gets dependency factory by specified name
        /// </summary>
        /// <param name="name">The name of the dependency factory. Value of name is not case sensitive</param>
        /// <returns>Returns <see cref="IContainerService"/> if found, otherwise returns null</returns>
        public IContainerService GetDependencyFactory(string name)
        {
            var factoryNode = GetServiceFactory().DependencyFactory;
            while (factoryNode != null)
            {
                if (string.Equals(factoryNode.Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    return factoryNode;
                }
                factoryNode = factoryNode.DependencyFactory;
            }
            return null;
        }


        protected InvocationInfo BeforeInvoke(Type contractType, string serviceName)
        {
            var factory = _internalsm.GetServiceFactory();
            ServiceInfo serviceInfo = factory.GetServiceInfo(contractType, serviceName);

            if (serviceInfo == null)
            {
                return default(InvocationInfo);
            }

            ServiceCallContext callContext = ServiceCallContext.Create(contractType, serviceInfo.ServiceType, IocContainer);
            InvokeDecorator(callContext, InvocationCase.Before, serviceInfo.Decorators);

            return new InvocationInfo() { ServiceInfo = serviceInfo, ServiceCallContext = callContext };
        }

        protected void AfterInvoke(object objectInstance, ServiceCallContext context, IEnumerable<DecoratorAttribute> localDecorators)
        {
            context.ServiceInstance = objectInstance;
            InvokeDecorator(context, InvocationCase.After, localDecorators);
        }

        private void InvokeDecorator(ServiceCallContext context, InvocationCase @case, IEnumerable<DecoratorAttribute> localDecorators)
        {
            DecoratorManager.Execute(context, @case, localDecorators);
        }

        protected struct InvocationInfo
        {
            public ServiceInfo ServiceInfo;
            public ServiceCallContext ServiceCallContext;
        }
    }
}
