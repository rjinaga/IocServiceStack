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
    using System.Reflection;

    public class ServiceInfo
    {
        private bool? _isReusable;
        private ServiceRegistrar _registrar;

        public readonly object SyncObject = new object();
        public readonly Type ServiceType;
        public readonly DecoratorAttribute[] Decorators;

        /// <summary>
        /// Name of the service
        /// </summary>
        public readonly string ServiceName;


        public ServiceInfo(Type serviceType, DecoratorAttribute[] decorators) : this(serviceType, decorators, null)
        {
            ServiceType = serviceType;
            Decorators = decorators;
        }

        public ServiceInfo(Type serviceType, DecoratorAttribute[] decorators, string serviceName)
        {
            ServiceType = serviceType;
            Decorators = decorators;
        }

        public ServiceInfo(Type serviceType, DecoratorAttribute[] decorators, bool isReusable, string serviceName) : this(serviceType, decorators, serviceName)
        {
            _isReusable = isReusable;
        }

        public ServiceInfo(Type serviceType, DecoratorAttribute[] decorators, bool isReusable) : this(serviceType, decorators, null)
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

#if NET46
                    var serviceAttribtue = ServiceType.GetCustomAttribute<ServiceAttribute>();
#else
                    var serviceAttribtue = ServiceType.GetTypeInfo().GetCustomAttribute<ServiceAttribute>();
#endif

                    if (serviceAttribtue != null)
                    {
                        _isReusable = serviceAttribtue.IsReusable;
                    }
                    else
                    {
                        _isReusable = false;
                    }
                }
                return _isReusable ?? false;
            }

        }

        /// <summary>
        /// Gets <see cref="IServiceActivator"/> 
        /// </summary>
        public IServiceActivator Activator { get; set; }


        
        public virtual Func<T> GetActionInfo<T>() where T: class
        {
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="interfaceType"></param>
        /// <param name="notifier"></param>
        /// <returns></returns>
        public ServiceRegistrar InitNewRegistrar(Type interfaceType, IServiceNotifier notifier)
        {
            _registrar = new ServiceRegistrar();
            //Register self
            _registrar.Register(interfaceType);

            notifier.ServiceUpdateNofication += ServiceUpdateNofication;

            return _registrar;
        }

        private void ServiceUpdateNofication(ServiceEventArgs eventArgs)
        {
            if (Activator != null && _registrar != null && _registrar.Contains(eventArgs.ServiceType))
            {
                Activator = null;
            }
        }

        public static DecoratorAttribute[] GetDecorators(Type contractType)
        {
            IEnumerable<Attribute> attributes;

#if NET46
            attributes = contractType.GetCustomAttributes();
#else
            attributes = contractType.GetTypeInfo().GetCustomAttributes();
#endif
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

    }
}
