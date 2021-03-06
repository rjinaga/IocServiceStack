﻿#region License
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
    using System.Linq;
    using System.Reflection;

    /// <summary>
    /// Represents auto mapper for contacts and correspondent services.
    /// </summary>
    public class ContractServiceAutoMapper
    {
        private readonly Assembly[] _assemblies;
        private readonly string[] _namespaces;
        private readonly bool _strictMode;
        private readonly ServiceMapTable _mapTable;

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
            _mapTable = new ServiceMapTable();
        }

        /// <summary>
        /// Maps contracts with services 
        /// </summary>
        public void Map()
        {
            InternalMap(_namespaces, _assemblies, _mapTable);
        }

        /// <summary>
        /// Gets the <see cref="BaseServiceInfo"/> object associated with the specified contractType.
        /// </summary>
        /// <param name="contractType"></param>
        /// <returns><see cref="BaseServiceInfo"/></returns>
        public BaseServiceInfo this[Type contractType]
        {
            get
            {
                if (!_mapTable.ContainsKey(contractType))
                {
                    return null;
                }

                return _mapTable[contractType].DefaultService;
            }
        }
        
        /// <summary>
        /// Gets the <see cref="BaseServiceInfo"/> object associated with the specified <paramref name="contractType"/> and <paramref name="serviceName"/>
        /// </summary>
        /// <param name="contractType">The type of the contract to be found.</param>
        /// <param name="serviceName">The name of the service to be found.</param>
        /// <returns><see cref="BaseServiceInfo"/></returns>
        public BaseServiceInfo this[Type contractType, string serviceName]
        {
            get
            {
                if (!_mapTable.ContainsKey(contractType))
                {
                    return null;
                }

                if (string.IsNullOrEmpty(serviceName))
                {
                    ExceptionHelper.ThrowArgumentNullException(nameof(serviceName));
                }
                return _mapTable[contractType]?.Services?[serviceName];
            }
        }

        /// <summary>
        /// Get all list of contracts
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Type> GetAllContracts()
        {
            return _mapTable.Keys.ToArray();
        }

        /// <summary>
        /// Get list of services for the given contract type
        /// </summary>
        /// <param name="contractType"></param>
        /// <returns></returns>
        public IEnumerable<ServiceTypeInfo> GetServiceTypes(Type contractType)
        {
            var contract = _mapTable[contractType];

            List<ServiceTypeInfo> types = new List<ServiceTypeInfo>();
            types.Add(new ServiceTypeInfo { Type = contract.DefaultService.ServiceType });

            var serviceTypes = _mapTable[contractType]?.Services;
            if (serviceTypes != null)
            {
                foreach (var item in serviceTypes)
                {
                    types.Add(new ServiceTypeInfo { Name = item.Key, Type = item.Value.ServiceType });
                }
            }
            return types;
        }

        /// <summary>
        /// Determines whether <see cref="ContractServiceAutoMapper"/> contains specified contract type.
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
        /// Determines whether the <see cref="ContractServiceAutoMapper"/> contains the specified
        /// <paramref name="contractType"/> and <paramref name="serviceName"/> and 
        /// </summary>
        /// <param name="contractType">The contractType to locate in the <see cref="ContractServiceAutoMapper"/>. </param>
        /// <param name="serviceName">The serviceName to locate in the <see cref="ContractServiceAutoMapper"/>. </param>
        /// <returns>true if found;otherwise false.</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public bool Contains(Type contractType, string serviceName)
        {
            if (contractType == null)
            {
                throw new ArgumentNullException(nameof(contractType));
            }

            if (serviceName == null)
            {
                throw new ArgumentNullException(nameof(serviceName));
            }

            if (_mapTable.ContainsKey(contractType))
            {
                return _mapTable[contractType].Services?.ContainsKey(serviceName) ?? false;
            };
            return false;
        }

        /// <summary>
        /// Adds the specified <paramref name="contractType"/> and <paramref name="serviceMeta"/> to the mapper.
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="serviceMeta"></param>
        public void Add(Type contractType, BaseServiceInfo serviceMeta)
        {
            if (serviceMeta.ServiceType != null && !CanTypeCast(contractType, serviceMeta.ServiceType))
            {
                ExceptionHelper.ThrowInvalidServiceType(contractType, serviceMeta.ServiceType);
            }

            if (string.IsNullOrEmpty(serviceMeta.ServiceName)) //if there's no default service
            {
                if (_mapTable.ContainsKey(contractType))
                {
                    if (_mapTable[contractType].DefaultService != null)
                    {
                        var msg = $"An element with the key '{contractType.FullName}' already exists.";
                        ExceptionHelper.ThrowArgumentException(msg);
                    }
                    _mapTable[contractType].DefaultService = serviceMeta;
                }
                else
                {
                    _mapTable.Add(contractType, new ServiceMapInfo() { DefaultService = serviceMeta });
                }
            }
            else
            {
                if (_mapTable.ContainsKey(contractType))
                {
                    _mapTable[contractType].CreateServicesIfNotInitialized();
                    if (_mapTable[contractType].Services.ContainsKey(serviceMeta.ServiceName))
                    {
                        var msg = $"An element with the key '{contractType.FullName}' already exists.";
                        ExceptionHelper.ThrowArgumentException(msg);
                    }
                    _mapTable[contractType].Services.Add(serviceMeta.ServiceName, serviceMeta);
                }
                else
                {
                    _mapTable.Add(contractType, new ServiceMapInfo() { Services = new ServicesPoint() { [serviceMeta.ServiceName] = serviceMeta } });
                }
            }
        }

        /// <summary>
        /// Add or replace if exists <paramref name="contractType"/> and <paramref name="serviceMeta"/> to the collection.
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="serviceMeta"></param>
        public void AddOrReplace(Type contractType, BaseServiceInfo serviceMeta)
        {
            if (serviceMeta.ServiceType != null && !CanTypeCast(contractType, serviceMeta.ServiceType))
            {
                ExceptionHelper.ThrowInvalidServiceType(contractType, serviceMeta.ServiceType);
            }

            if (_mapTable.ContainsKey(contractType))
            {
                if (string.IsNullOrEmpty(serviceMeta.ServiceName))
                {
                    _mapTable[contractType].DefaultService = serviceMeta;
                }
                else
                {
                    if (_mapTable[contractType].Services == null)
                    {
                        _mapTable[contractType].Services = new ServicesPoint();
                    }
                    _mapTable[contractType].Services[serviceMeta.ServiceName] = serviceMeta;
                }
            }
            else
            {
                Add(contractType, serviceMeta);
            }
        }

        private bool CanTypeCast(Type contractType, Type serviceType)
        {
            return contractType.IsAssignableFrom(serviceType);
        }

        private void InternalMap(string[] namespaces, Assembly[] aseemblies, ServiceMapTable services)
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

        private void FillServicesDictionary(IEnumerable<Type> serviceTypes, ServiceMapTable services, string[] namespaces)
        {
            foreach (Type serviceType in serviceTypes)
            {
                /*Allow when no namespaces are configured or if it's configured then check the namespace of the service is matched with namespaces declared in config via ServiceInjector.*/
                if (namespaces == null || namespaces.Length == 0 || namespaces.Contains(serviceType.Namespace))
                {

                    var serviceAttribute = serviceType.GetTypeInfo().GetCustomAttributes<ServiceAttribute>().FirstOrDefault();

                    /*1. First check auto - if auto specified, rest all ignored
                      2. Contracts at service attribute level - if contracts specified  Individual Interface  level contract ignored
                      3. Individual Interface  level */
                    if (serviceAttribute.Auto)
                    {
                        MapAllBaseAndInterfaceTypes(serviceType, services);
                    }
                    else
                    {
                        MapSelectiveBaseAndInterfaceTypes(serviceType, services, serviceAttribute);
                    }
                }
            }
        }

        private void MapAllBaseAndInterfaceTypes(Type serviceType, ServiceMapTable services)
        {
            //Map base type with the service
            if (serviceType.GetTypeInfo().BaseType != typeof(object))
            {
                MapService(services, interfaceType: serviceType.GetTypeInfo().BaseType, serviceType: serviceType);
            }
            IEnumerable<Type> interfaces = serviceType.GetInterfaces();

            /*Maps the interfaces (which are decorated with contract attribute) of the service */
            /*Map service type with the contract interfaces*/
            foreach (var interfaceType in interfaces)
            {
                MapService(services, interfaceType, serviceType);
            }
        }

        private void MapSelectiveBaseAndInterfaceTypes(Type serviceType, ServiceMapTable services, ServiceAttribute serviceAttribute)
        {

#if NET46
            /*Fetch implemented interfaces of service whose interface is decorated with  ContractAttribute.*/
            IEnumerable<Type> interfaces = serviceType.GetInterfaces()
                                                      .Where(@interface => @interface.GetCustomAttribute<ContractAttribute>() != null);
#else
                    /*Fetch implemented interfaces of service whose interface is decorated with  ContractAttribute.*/
                    IEnumerable<Type> interfaces = serviceType.GetInterfaces()
                                                              .Where(@interface => @interface.GetTypeInfo().GetCustomAttribute<ContractAttribute>() != null);
#endif
            /*Maps the base class (which is decorated with contract attribute) of the service*/
            if (serviceType.GetTypeInfo().BaseType != typeof(object))
            {
                //if base type is decorated with contract attribute then add that to service.
                var baseTypeInfo = serviceType.GetTypeInfo().BaseType.GetTypeInfo();
                if (baseTypeInfo.GetCustomAttribute<ContractAttribute>() != null)
                {
                    MapService(services, interfaceType: serviceType.GetTypeInfo().BaseType, serviceType: serviceType);
                }
            }

            /*Maps the interfaces (which are decorated with contract attribute) of the service */
            /*Map service type with the contract interfaces*/
            foreach (var interfaceType in interfaces)
            {
                MapService(services, interfaceType, serviceType);
            }

            /*Maps inherit type (base class/interface) if the type is set to the property contract of ServiceAttribute  */
            MapServiceDefinedAtAttributeLevel(services, serviceType, serviceAttribute);
        }



        private void MapServiceDefinedAtAttributeLevel(ServiceMapTable services, Type serviceType, ServiceAttribute serviceAttribute)
        {
            if (serviceAttribute.Contracts != null)
            {
                foreach (var contract in serviceAttribute.Contracts)
                {
                    if (contract != null)
                    {
                        //First we need to check whether service class is inherited from this contract
                        if (contract.GetTypeInfo().IsAssignableFrom(serviceType))
                        {
                            MapService(services, contract, serviceType);
                        }
                    }
                }
            }
        }


        private void MapService(ServiceMapTable services, Type interfaceType, Type serviceType)
        {
            /*Check for ServiceAttribute, if not found, we don't need to process */
            var serviceAttribute = serviceType.GetTypeInfo().GetCustomAttributes<ServiceAttribute>().FirstOrDefault();
            if (serviceAttribute == null)
            {
                return;
            }

            /*if the contract already exists in the services collection, try to update the services.
             if strict mode is enabled and service already exists then it throw an exception, otherwise it updates.
             if service name is not specified then it will be consider as DefaultService 
             */
            if (services.ContainsKey(interfaceType))
            {
                MapWithExistingService(serviceAttribute, services, interfaceType, serviceType);
            }
            else
            {
                //If service name is not specified then add it as default service
                if (string.IsNullOrEmpty(serviceAttribute.Name))
                {
                    services.Add(interfaceType, new ServiceMapInfo()
                    {
                        DefaultService = new ServiceInfo(serviceType, BaseServiceInfo.GetDecorators(interfaceType))
                    });
                }
                else
                {
                    services.Add(interfaceType, new ServiceMapInfo()
                    {
                        Services = new ServicesPoint()
                        {
                            [serviceAttribute.Name] = new ServiceInfo(serviceType, BaseServiceInfo.GetDecorators(interfaceType))
                        }
                    });
                }
            }
        }

        private void MapWithExistingService(ServiceAttribute serviceAttribute, ServiceMapTable services, Type interfaceType, Type serviceType)
        {
            var mapInfo = services[interfaceType];

            //Duplicate Service exception check for default service
            if (_strictMode && string.IsNullOrEmpty(serviceAttribute.Name) && mapInfo.DefaultService != null)
            {
                ExceptionHelper.ThrowDuplicateServiceException($"{serviceType.FullName} implements {interfaceType.FullName}");
            }
            //Set default service if service name is not set
            if (string.IsNullOrEmpty(serviceAttribute.Name))
            {
                mapInfo.DefaultService = new ServiceInfo(serviceType, BaseServiceInfo.GetDecorators(interfaceType));
            }
            else //named service
            {
                if (mapInfo.Services == null)
                {
                    mapInfo.Services = new ServicesPoint();
                }
                if (_strictMode)
                {
                    //Duplicate Service exception check for named services
                    if (mapInfo.Services.ContainsKey(serviceAttribute.Name))
                    {
                        ExceptionHelper.ThrowDuplicateServiceException($"{serviceType.FullName} implements {interfaceType.FullName}");
                    }
                }
                mapInfo.Services[serviceAttribute.Name] = new ServiceInfo(serviceType, BaseServiceInfo.GetDecorators(interfaceType));
            }
        }
    }
}

