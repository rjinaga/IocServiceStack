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
    using System.Linq.Expressions;
    using System.Reflection;

    public class DefaultServiceFactory : AbstractFactory, IServiceFactory, IContainerExtension
    {
        private ServiceNotifier _notifier;

        public DefaultServiceFactory(string[] namespaces, Assembly[] assemblies, bool strictMode)
            : base(namespaces, assemblies, strictMode)
        {

        }

        public virtual ServiceInfo GetServiceInfo(Type contractType)
        {
            if (!ServicesMapTable.Contains(contractType))
                throw ExceptionHelper.ThrowServiceNotRegisteredException(contractType.Name);

            return ServicesMapTable[contractType];
        }

        public virtual T Create<T>(ServiceInfo serviceMeta) where T : class
        {
            if (serviceMeta != null)
            {
                if (serviceMeta.Activator == null)
                {
                    Type interfaceType = typeof(T);
                    //Compile
                    Compile<T>(interfaceType, serviceMeta);
                }
                return serviceMeta.Activator.CreateInstance<T>();
            }
            return default(T);
        }

        public object Create(Type contractType, ServiceInfo serviceMeta)
        {
            if (serviceMeta != null)
            {
                if (serviceMeta.Activator == null)
                {
                    //Compile
                    Compile<object>(contractType, serviceMeta);
                }
                return serviceMeta.Activator.CreateInstance<object>();
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        public void StartWork()
        {
            ContractObserver = new DefaultContractWorkObserver();
            _notifier = new ServiceNotifier();

            ContractObserver.OnUpdate((type) =>
            {
                _notifier.SendUpdate(type);
            });

        }

        public IContainerExtension Add<TC>(Func<TC> serviceAction) where TC : class
        {
            Type interfaceType = typeof(TC);
            var serviceMeta = new ServiceInfo<TC>(serviceAction, ServiceInfo.GetDecorators(interfaceType));

            ServicesMapTable.Add(interfaceType, serviceMeta);

            //send update to observer
            ContractObserver.Update(interfaceType);

            return this;
        }

        public IContainerExtension Replace<TC>(Func<TC> expression) where TC : class
        {
            Type interfaceType = typeof(TC);
            var serviceMeta = new ServiceInfo<TC>(expression, ServiceInfo.GetDecorators(interfaceType));

            ServicesMapTable[interfaceType] = serviceMeta;

            //send update to observer
            ContractObserver.Update(interfaceType);

            return this;
        }

        private void Compile<T>(Type interfaceType, ServiceInfo serviceMeta) where T : class
        {
            if (serviceMeta.Activator == null)
            {
                lock (serviceMeta.SyncObject)
                {
                    if (serviceMeta.Activator == null)
                    {
                        var registrar = serviceMeta.InitNewRegistrar(interfaceType, _notifier);

                        Func<T> serviceCreator  = serviceMeta.GetActionInfo<T>();
                        if (serviceCreator == null)
                        {
                            Expression newExpression = CreateConstructorExpression(interfaceType, serviceMeta.ServiceType, registrar);
                            //Set Activator
                            serviceCreator = Expression.Lambda<Func<T>>(newExpression).Compile();
                        }
                        serviceMeta.Activator = new ServiceActivator<T>(serviceCreator, serviceMeta.IsReusable);
                    }
                }
            }
        }
    }
}
