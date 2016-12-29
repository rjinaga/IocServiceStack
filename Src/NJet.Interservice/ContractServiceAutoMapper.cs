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
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Represents auto mapper of contact and it's corrospendet service.
    /// </summary>
    public class ContractServiceAutoMapper
    {
        private readonly Assembly[] _assemblies;
        private readonly string[] _namespaces;
        private readonly bool _strictMode;
        private readonly Dictionary<Type, ServiceMeta> _mapTable;

        /// <summary>
        /// Initializes a new instance of ContractServiceAutoMapper class with the specified namespaces, assemblies and strictmode
        /// </summary>
        /// <param name="namespaces">The namespaces of services</param>
        /// <param name="assemblies">The assemblies of services</param>
        /// <param name="strictMode">The value indicating whether strict mode is on or off</param>
        public ContractServiceAutoMapper(string[] namespaces, Assembly[] assemblies, bool strictMode)
        {
            _namespaces = namespaces;
            _assemblies = assemblies;
            _strictMode = strictMode;
            _mapTable = new Dictionary<Type, ServiceMeta>();
        }

        /// <summary>
        /// Maps contracts with services 
        /// </summary>
        public void Map()
        {
            InternalMap(_namespaces, _assemblies, _mapTable);
        }

        /// <summary>
        /// Gets or sets the <see cref="ServiceMeta"/> object associated with the specified contractType.
        /// </summary>
        /// <param name="contractType"></param>
        /// <returns></returns>
        public ServiceMeta this[Type contractType]
        {
            get
            {
                return _mapTable[contractType];
            }
            set
            {
                _mapTable[contractType] = value;
            }
        }

        /// <summary>
        /// Determines whether <see cref="ContractServiceAutoMapper"/> contains specfied contract type.
        /// </summary>
        /// <param name="contractType">The <paramref name="contractType"/> locate in the <see cref="ContractServiceAutoMapper"/> </param>
        /// <returns>true, if <see cref="ContractServiceAutoMapper"/> contains an element with the specified <paramref name="contractType"/>; otherwise false.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="contractType"/> is null.</exception>
        public bool Contains(Type contractType)
        {
            if (contractType == null)
                throw new ArgumentNullException(nameof(contractType));

            return _mapTable.ContainsKey(contractType);
        }

        /// <summary>
        /// Adds the specified <paramref name="contractType"/> and <paramref name="serviceMeta"/> to the mapper.
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="serviceMeta"></param>
        public void Add(Type contractType, ServiceMeta serviceMeta)
        {
            _mapTable.Add(contractType, serviceMeta);
        }

        private void InternalMap(string[] namespaces, Assembly[] aseemblies, Dictionary<Type, ServiceMeta> services)
        {
            if (aseemblies == null)
                return;

            foreach (var asm in aseemblies)
            {
#if NET46
                IEnumerable<Type> serviceTypes = from type in asm.GetTypes()
                                                 where type.GetCustomAttribute<ServiceAttribute>() != null
                                                 select type;
#else
                IEnumerable<Type> serviceTypes = from type in asm.GetTypes()
                                                 where type.GetTypeInfo().GetCustomAttribute<ServiceAttribute>() != null
                                                 select type;           

#endif
                FillServicesDictionary(serviceTypes, services, namespaces);
            }
        }

        private void FillServicesDictionary(IEnumerable<Type> serviceTypes, Dictionary<Type, ServiceMeta> services, string[] namespaces)
        {
            foreach (var serviceType in serviceTypes)
            {
                /*Allow when no namespaces are configured or if it's configured then check the namespace of the service is matched with namespaces declared in config via ServiceInjector.*/
                if (namespaces == null || namespaces.Length == 0 || namespaces.Contains(serviceType.Namespace))
                {
#if NET46
                    /*Fetch implemented intefaces of service whose interface is decorated with  ContractAttribute.*/
                    IEnumerable<Type> interfaces = serviceType.GetInterfaces()
                                                              .Where(@interface => @interface.GetCustomAttribute<ContractAttribute>() != null);
#else
                   /*Fetch implemented intefaces of service whose interface is decorated with  ContractAttribute.*/
                    IEnumerable<Type> interfaces = serviceType.GetInterfaces()
                                                              .Where(@interface => @interface.GetTypeInfo().GetCustomAttribute<ContractAttribute>() != null);               
#endif
                    /*Map service type with the contract interfaces*/
                    foreach (var item in interfaces)
                    {
                        if (_strictMode)
                        {
                            if (services.ContainsKey(item))
                                ExceptionHelper.ThrowDuplicateServiceException($"{serviceType.FullName} implements {item.FullName}");
                        }
                        services[item] = new ServiceMeta { ServiceType = serviceType };
                    }
                }
            }
        }
    }
}
