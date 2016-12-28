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
    using System.Reflection;

    public class DefaultServiceFactory : AbstractFactory, IServiceFactory
    {
        private ServiceNotifier _notifier;

        public DefaultServiceFactory(string[] namespaces, Assembly[] assemblies, bool strictMode) : base(namespaces, assemblies, strictMode)
        {

        }

        public T Create<T>() where T : class
        {
            Type interfaceType = typeof(T);

            if (!ServicesMapTable.Contains(interfaceType))
                throw ExceptionHelper.ThrowServiceNotRegisteredException(interfaceType.Name);

            ServiceMeta serviceMeta = ServicesMapTable[interfaceType];

            if (serviceMeta != null)
            {
                if (serviceMeta.Activator == null)
                {
                    //Attach register
                    serviceMeta.Compile<T>(Subcontract, _notifier);

                }
                return serviceMeta.Activator.GetInstance<T>();
            }
            return default(T);
        }

        public void StartWork()
        {
            ContractObserver = new DefaultContractWorkObserver();
            _notifier = new ServiceNotifier();

            ContractObserver.OnUpdate((type) =>
            {
                _notifier.SendUpdate(type);
            });

        }
    }
}
