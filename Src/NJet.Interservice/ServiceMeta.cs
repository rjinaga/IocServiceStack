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

namespace NJet.Interservice
{
    using System;
    using System.Linq.Expressions;
    using System.Reflection;

    public class ServiceMeta 
    {
        private Type _serviceType;
        private bool? _isReusable;
        private ServiceRegistrar _registrar;
        private object _lockCompile = new object();
        
        public Type ServiceType
        {
            get
            {
                return _serviceType;
            }
            set
            {
                if (_serviceType != value)
                {
                    _serviceType = value;
                }
            }
        }
        

        /// <summary>
        /// if IsReusable set to true then multiple requests are served with the same instance.
        /// </summary>
        public bool IsReusable
        {
            get
            {
                if (_isReusable == null && _serviceType != null)
                {
                    var serviceAttribtue = _serviceType.GetCustomAttribute<ServiceAttribute>();
                    if (serviceAttribtue != null)
                    {
                        _isReusable = serviceAttribtue.IsReusable;
                    }
                    else
                    {
                        _isReusable = false;
                    }
                }
                return _isReusable??false;
            }

        }
        /// <summary>
        /// Gets <see cref="IServiceActivator"/> 
        /// </summary>
        public IServiceActivator Activator { get; private set; }

        internal void Compile<T>(SubcontractFactory subcontract, IServiceNotifier notifier) where T : class
        {
            if (Activator == null)
            {
                lock (_lockCompile)
                {
                    if (Activator == null)
                    {
                        _registrar = new ServiceRegistrar();

                        //Register self
                        _registrar.Register(typeof(T));

                        notifier.ServiceUpdateNofication += ServiceUpdateNofication;

                        InternalCompile<T>(subcontract);
                    }
                }
            }
        }

        private void ServiceUpdateNofication(ServiceEventArgs eventArgs)
        {
            if (Activator != null && _registrar != null && _registrar.Contains(eventArgs.ServiceType))
            {
                Activator = null;
            }
        }

        private void InternalCompile<T>(SubcontractFactory subcontract) where T : class
        {
            Type interfaceType = typeof(T);
            ConstructorInfo[] serviceConstructors = _serviceType.GetConstructors();
            foreach (var serviceConstructor in serviceConstructors)
            {
                var attribute = serviceConstructor.GetCustomAttribute<ServiceInitAttribute>();
                if (attribute != null)
                {
                    /*Get parameters of service's constructor and inject corresponding repositories values to it */
                    ParameterInfo[] constructorParametersInfo = serviceConstructor.GetParameters();
                    Expression[] arguments = new Expression[constructorParametersInfo.Length];

                    int index = 0;
                    foreach (var constrParameter in constructorParametersInfo)
                    {
                        arguments[index] = subcontract.Create(constrParameter.ParameterType, _registrar) ?? Expression.Default(constrParameter.GetType());
                        index++;
                    }

                    Func<T> serviceCreator = Expression.Lambda<Func<T>>(Expression.New(serviceConstructor, arguments)).Compile();

                    Activator = new ServiceActivator<T>(serviceCreator, IsReusable);

                    return;
                }
            }

            /*if none of them are not decorated with ServiceInitAttribute then initialize default with default constructor*/
            CreateWithDefaultConstructor<T>(_serviceType);
        }

        private void CreateWithDefaultConstructor<T>(Type serviceType) where T : class
        {
            NewExpression newExp = Expression.New(serviceType);

            Func<T> serviceCreator = Expression.Lambda<Func<T>>(Expression.New(serviceType)).Compile();

            // Compile our new lambda expression.
            Activator = new ServiceActivator<T>(serviceCreator, IsReusable );
        }

    }
}
