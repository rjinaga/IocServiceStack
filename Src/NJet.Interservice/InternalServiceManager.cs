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

using System.Reflection;

namespace NJet.Interservice
{
    internal class InternalServiceManager
    {
        private static IServiceFactory _serviceFactory;
        private static readonly object _factorySyncObject = new object();

        internal static ServiceConfig Config { get; set; }

        internal static IServiceFactory GetServiceFactory()
        {
            if (_serviceFactory == null)
            {
                InitServiceFactory();
            }
            return _serviceFactory;
        }

        private static void InitServiceFactory()
        {
            lock (_factorySyncObject)
            {
                if (_serviceFactory == null)
                {
                    Assembly[] assmblies = GetAssemblies(Config.ServiceOptions.Assemblies);
                    _serviceFactory = new DefaultServiceFactory(Config.ServiceOptions.Namespaces, assmblies);
                    
                    //Set subcontract factory 
                    Assembly[] subcontractAssmblies = GetAssemblies(Config.ServiceDependentOptions?.Assemblies);
                    _serviceFactory.Subcontract = new RepositoryFactory(Config.ServiceDependentOptions?.Namespaces, subcontractAssmblies);
                }
            }
        }

        private static Assembly[] GetAssemblies(string[] assmblynames)
        {
            if (assmblynames == null || assmblynames.Length == 0)
            {
                return null;
            }

            Assembly[] assmblies = new Assembly[assmblynames.Length];
            for (int index = 0; index < assmblynames.Length; index++)
            {
                assmblies[index] = Assembly.Load(new AssemblyName(assmblynames[index]));
            }
            return assmblies;
        }
    }
}
