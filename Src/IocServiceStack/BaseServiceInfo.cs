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
    using System.Linq.Expressions;
    using System.Reflection;

    /// <summary>
    /// Contains service type, decorators and name of the service.
    /// </summary>
    public abstract class BaseServiceInfo
    {
        private bool? _isReusable;
        private ServiceRegister _register;

        /// <summary>
        /// Synchronization object for <see cref="BaseServiceInfo"/> instance.
        /// </summary>
        public readonly object SyncObject = new object();

        /// <summary>
        /// Gets read-only service type.
        /// </summary>
        public readonly Type ServiceType;

        /// <summary>
        /// Gets read-only array of decorators.
        /// </summary>
        public readonly DecoratorAttribute[] Decorators;

        /// <summary>
        /// Name of the service
        /// </summary>
        public readonly string ServiceName;

        /// <summary>
        /// Initializes a new instance of <see cref="BaseServiceInfo"/> class with specified parameters.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="decorators">Specify array of decorators to be executed.</param>
        public BaseServiceInfo(Type serviceType, DecoratorAttribute[] decorators) : this(serviceType, decorators, null)
        {
            ServiceType = serviceType;
            Decorators = decorators;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="BaseServiceInfo"/> class with specified parameters.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="decorators">Specify array of decorators to be executed.</param>
        /// <param name="serviceName">Specify name of the service.</param>
        public BaseServiceInfo(Type serviceType, DecoratorAttribute[] decorators, string serviceName)
        {
            ServiceType = serviceType;
            Decorators = decorators;
            ServiceName = serviceName;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="BaseServiceInfo"/> class with specified parameters.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="decorators">Specify array of decorators to be executed.</param>
        /// <param name="serviceName">Specify name of the service.</param>
        /// <param name="isReusable">Indicates, whether <paramref name="serviceType"/> is reusable.</param>
        public BaseServiceInfo(Type serviceType, DecoratorAttribute[] decorators, bool isReusable, string serviceName) : this(serviceType, decorators, serviceName)
        {
            _isReusable = isReusable;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="BaseServiceInfo"/> class with specified parameters.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="decorators">Specify array of decorators to be executed.</param>
        /// <param name="isReusable">Indicates, whether <paramref name="serviceType"/> is reusable.</param>
        public BaseServiceInfo(Type serviceType, DecoratorAttribute[] decorators, bool isReusable) : this(serviceType, decorators, null)
        {
            _isReusable = isReusable;
        }

        /// <summary>
        /// if IsReusable set to true then multiple requests are served with the same instance.
        /// </summary>
        public bool IsReusable
        {
            get
            {
                if (_isReusable == null && ServiceType != null)
                {
                    var reusableAttribtue = ServiceType.GetTypeInfo().GetCustomAttribute<ReusableAttribute>();
                    _isReusable = reusableAttribtue != null;
                }
                return _isReusable ?? false;
            }

        }

        /// <summary>
        /// Gets <see cref="IServiceActivator"/> 
        /// </summary>
        public IServiceActivator Activator { get; set; }

        /// <summary>
        /// This method to gets service instance callback function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public abstract Func<T> GetServiceInstanceCallback<T>() where T : class;

        /// <summary>
        /// This method to get service instance expression
        /// </summary>
        /// <returns></returns>
        public abstract Expression GetServiceInstanceExpression();

        /// <summary>
        /// Initializes new register for the specified interface type.
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="notifier"></param>
        /// <returns></returns>
        public ServiceRegister InitNewRegister(Type interfaceType, IServiceNotifier notifier)
        {
            _register = new ServiceRegister();
            //Register self
            _register.Register(interfaceType);

            notifier.ServiceUpdateNofication += ServiceUpdateNofication;

            return _register;
        }

        private void ServiceUpdateNofication(ServiceEventArgs eventArgs)
        {
            if (Activator != null && _register != null && _register.Contains(eventArgs.ServiceType))
            {
                Activator = null;
            }
        }

        #region Static Members
        /// <summary>
        /// Returns array of decorators.
        /// </summary>
        /// <param name="contractType"></param>
        /// <returns></returns>
        public static DecoratorAttribute[] GetDecorators(Type contractType)
        {
            IEnumerable<Attribute> attributes;
            attributes = contractType.GetTypeInfo().GetCustomAttributes();

            if (attributes != null)
            {
                List<DecoratorAttribute> decorators = new List<DecoratorAttribute>();
                foreach (var attribute in attributes)
                {
                    if (typeof(DecoratorAttribute).IsAssignableFrom(attribute.GetType()))
                    { 
                        decorators.Add(attribute as DecoratorAttribute);
                    }
                }
                return decorators.ToArray();
            }
            return null;
        }
        #endregion

    }
    
}
