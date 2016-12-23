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


    internal static class BootstrapHelper
    {
        public static void Bootstrap(string[] namespaces, Assembly[] aseemblies, Dictionary<Type, ServiceMeta> services)
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

        public static void FillServicesDictionary(IEnumerable<Type> serviceTypes, Dictionary<Type, ServiceMeta> services, string[] namespaces)
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
                        services[item] = new ServiceMeta { ServiceType = serviceType };
                    }
                }
            }
        }
    }

}
