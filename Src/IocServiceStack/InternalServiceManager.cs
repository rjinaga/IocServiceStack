#region License
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
    using System.Reflection;

    internal class InternalServiceManager
    {
        private IRootServiceFactory _serviceFactory;
        private readonly object _factorySyncObject = new object();
        private ContainerConfig _config;

        public InternalServiceManager(ContainerConfig config)
        {
            if (config == null)
                ExceptionHelper.ThrowArgumentNullException(nameof(config));

            _config = config;
        }

        public IRootServiceFactory GetServiceFactory()
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
                    Assembly[] assmblies = GetAssemblies(_config.ContainerOptions.Assemblies);

                    //if user defined ServiceFactory is configured then set that factory, otherise set the default one
                    if (_config.ContainerOptions.ServiceFactory != null)
                    {
                        _serviceFactory = _config.ContainerOptions.ServiceFactory;
                    }
                    else
                    {
                        //Prepare for shared factory
                        var sharedDeps = _config.ContainerOptions.SharedDependencies;
                        Assembly[] sharedAssmblies = GetAssemblies(sharedDeps?.Assemblies);
                        var sharedFactory = new DefaultSharedFactory(sharedDeps?.Namespaces, sharedAssmblies, _config.ContainerOptions.StrictMode);

                        //dependency factory
                        IDependencyFactory dependencyFactory = InitChainOfSubctractFactories(sharedFactory);

                        //Root factory
                        _serviceFactory = new DefaultServiceFactory(_config.ContainerOptions.Namespaces, assmblies, _config.ContainerOptions.StrictMode, dependencyFactory, sharedFactory);
                    }

                    //Start work of service factory and chain of dependent factories
                    _serviceFactory.Initialize();
                }
            }
        }

        private IDependencyFactory InitChainOfSubctractFactories(ISharedFactory sharedFactory)
        {
            ContainerDependencyOptions dependencies = _config.ContainerOptions.Dependencies;
            System.Collections.Generic.Stack<ContainerDependencyOptions> stack = new System.Collections.Generic.Stack<ContainerDependencyOptions>();

            while (dependencies  != null)
            {
                stack.Push(dependencies);
                dependencies = dependencies.Dependencies;
            }

            IDependencyFactory lastDependencyFactory = null;
            foreach (var item in stack)
            {
                Assembly[] subcontractAssmblies = GetAssemblies(item.Assemblies);
                lastDependencyFactory = new DefaultSubcontractFactory(item.Name, item.Namespaces, subcontractAssmblies, _config.ContainerOptions.StrictMode, lastDependencyFactory, sharedFactory);
            }
            return lastDependencyFactory;

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
