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

    /// <summary>
    /// Represents abstract factory of services
    /// </summary>
    public abstract class AbstractFactory : IBasicService
    {
        private IContractObserver _observer;

        /// <summary>
        /// Get readonly ServiceMapTable object
        /// </summary>
        protected readonly ContractServiceAutoMapper ServicesMapTable;

        /// <summary>
        /// Initializes a new instance of AbstractFactory class with the specified namespaces, assemblies and strictmode
        /// </summary>
        /// <param name="namespaces">The namespaces of services</param>
        /// <param name="assemblies">The assemblies of services</param>
        /// <param name="strictMode">The value indicating whether strict mode is on or off</param>
        public AbstractFactory(string[] namespaces, Assembly[] assemblies, bool strictMode)
        {
            ServicesMapTable = new ContractServiceAutoMapper(namespaces, assemblies, strictMode);
            ServicesMapTable.Map();
        }

        /// <summary>
        /// Gets or sets factory of Subcontract of the current service
        /// </summary>
        public SubcontractFactory Subcontract
        {
            get; set;
        }

        /// <summary>
        /// Adds the specified service to the factory
        /// </summary>
        /// <typeparam name="T">The class of the service</typeparam>
        /// <param name="service">The type of the service</param>
        /// <returns>Instance <see cref="IBasicService"/> of current object</returns>
        public virtual IBasicService Add<T>(Type service) where T : class
        {
            Type interfaceType = typeof(T);
            var serviceMeta = new ServiceMeta { ServiceType = service };
            ServicesMapTable.Add(interfaceType, serviceMeta);

            //send update to observer
            ContractObserver.Update(interfaceType);

            return this;
        }
        
        /// <summary>
        /// Replaces the specified service in the factory.
        /// </summary>
        /// <typeparam name="T">The class of the service</typeparam>
        /// <param name="service"></param>
        /// <returns></returns>
        public virtual IBasicService Replace<T>(Type service) where T : class
        {
            Type interfaceType = typeof(T);
            var serviceMeta = ServicesMapTable[interfaceType];
            serviceMeta.ServiceType = service;

            //send update to observer
            ContractObserver.Update(interfaceType);

            return this;
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
                    if (Subcontract != null)
                        Subcontract.ContractObserver = _observer;
                }
                else
                {
                    //TODO
                    //ExceptionHelper.ThrowOverrideObserverExpection
                }
            }
        }
    }
}
