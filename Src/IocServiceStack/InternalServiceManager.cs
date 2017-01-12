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
    using System.Reflection;

    internal class InternalServiceManager
    {
        private IServiceFactory _serviceFactory;
        private readonly object _factorySyncObject = new object();
        private ServiceConfig _config;

        public InternalServiceManager(ServiceConfig config)
        {
            if (config == null)
                ExceptionHelper.ThrowArgumentNullException(nameof(config));

            _config = config;
        }

        public IServiceFactory GetServiceFactory()
        {
            if (_serviceFactory == null)
            {
                InitServiceFactory();
            }
            return _serviceFactory;
        }

        private  void InitServiceFactory()
        {
            lock (_factorySyncObject)
            {
                if (_serviceFactory == null)
                {
                    Assembly[] assmblies = GetAssemblies(_config.ServiceOptions.Assemblies);

                    //if user defined ServiceFactory is configured then set that factory, otherise set the default one
                    if (_config.ServiceOptions.ServiceFactory != null)
                    {
                        _serviceFactory = _config.ServiceOptions.ServiceFactory;
                    }
                    else
                    {
                        _serviceFactory = new DefaultServiceFactory(_config.ServiceOptions.Namespaces, assmblies, _config.ServiceOptions.StrictMode);
                    }

                    InitChainOfSubctractFactories();

                    //Start work of service factory and chain of dependent factories
                    _serviceFactory.StartWork();
                }
            }
        }

        private void InitChainOfSubctractFactories()
        {
            ServiceDependencyOptions dependencies = _config.ServiceOptions.Dependencies;
            IBasicService serviceNode = _serviceFactory;

            while (dependencies  != null)
            {
                //if user defined ServiceFactory is configured then set that factory, otherise set the default one
                if (dependencies.ServiceFactory != null)
                {
                    serviceNode.Subcontract = dependencies.ServiceFactory;
                }
                //if Subcontract is defined along with ServiceFactory configuration, then don't set the default contract factory.
                else if (serviceNode.Subcontract == null)
                {
                    Assembly[] subcontractAssmblies = GetAssemblies(dependencies.Assemblies);
                    serviceNode.Subcontract = new DefaultSubcontractFactory(dependencies.Namespaces, subcontractAssmblies, _config.ServiceOptions.StrictMode);
                }

                //set child node of current dependencies
                dependencies = dependencies.Dependencies;
                serviceNode = serviceNode.Subcontract;
            }
        }

        private Assembly[] GetAssemblies(string[] assmblynames)
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
