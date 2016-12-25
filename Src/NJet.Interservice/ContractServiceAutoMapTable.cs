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
    
    public class ContractServiceAutoMapTable
    {
        private readonly Assembly[] _aseemblies;
        private readonly string[] _namespaces;
        private readonly bool _strictMode;
        private readonly Dictionary<Type, ServiceMeta> _mapTable;

        public ContractServiceAutoMapTable(string[] namespaces, Assembly[] aseemblies, bool strictMode)
        {
            _namespaces = namespaces;
            _aseemblies = aseemblies;
            _strictMode = strictMode;
            _mapTable = new Dictionary<Type, ServiceMeta>();
        }

        /// <summary>
        /// Maps contracts with services in specified assemblies and namespaces
        /// </summary>
        public void Map()
        {
            InternalMap(_namespaces, _aseemblies, _mapTable);
        }

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

        public bool Contains(Type contractType)
        {
            return _mapTable.ContainsKey(contractType);
        }

        public void Add(Type contractType, ServiceMeta serviceMeta)
        {
            _mapTable.Add(contractType, serviceMeta);
        }

        private void InternalMap(string[] namespaces, Assembly[] aseemblies, Dictionary<Type, ServiceMeta> services)
        {
            if (aseemblies == null)
                return;

            Array.ForEach(aseemblies, (asm) =>
            {
                IEnumerable<Type> serviceTypes = from type in asm.GetTypes()
                                                 where type.GetCustomAttribute<ServiceAttribute>() != null
                                                 select type;

                FillServicesDictionary(serviceTypes, services, namespaces);
            });
        }

        public void FillServicesDictionary(IEnumerable<Type> serviceTypes, Dictionary<Type, ServiceMeta> services, string[] namespaces)
        {
            foreach (var serviceType in serviceTypes)
            {
                /*Allow when no namespaces are configured or if it's configured then check the namespace of the service is matched with namespaces declared in config via ServiceInjector.*/
                if (namespaces == null || namespaces.Length == 0 || namespaces.Contains(serviceType.Namespace))
                {
                    /*Fetch implemented intefaces of service whose interface is decorated with  ContractAttribute.*/
                    IEnumerable<Type> interfaces = serviceType.GetInterfaces()
                                                              .Where(@interface => @interface.GetCustomAttribute<ContractAttribute>() != null);

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
