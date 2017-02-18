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
    /// <summary>
    /// Main container of the inversion of control framework.
    /// </summary>
    public class IocContainer
    {
        private IServiceProvider _serviceProvider;
        private readonly ContainerConfig _config;
        
        internal IocContainer(ContainerConfig config)
        {
            _config = config;
            Prepare();
        }

        /// <summary>
        /// Gets <see cref="ContainerConfig"/> 
        /// </summary>
        internal ContainerConfig Config => _config;

        /// <summary>
        /// Gets <see cref="IServiceProvider"/> 
        /// </summary>
        public IServiceProvider ServiceProvider => _serviceProvider;

        private void Prepare()
        {
            _serviceProvider = _config.ServiceProvider ?? new DefaultServiceProvider(_config);
            //set decorators to service provider
            _serviceProvider.DecoratorManager = new DecoratorManager() { GlobalDecorators = _config.Decorators };
        }

        /// <summary>
        /// Returns root or front container within the main container.
        /// </summary>
        /// <returns></returns>
        public IRootContainer GetRootContainer()
        {
            return ServiceProvider.GetServiceFactory();
        }

        /// <summary>
        /// Returns shared container of the root container and its dependencies.
        /// </summary>
        /// <returns></returns>
        public ISubContainer GetSharedContainer()
        {
            return ServiceProvider.GetSharedFactory();
        }

        /// <summary>
        /// Returns dependency container  by specified name.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ISubContainer GetDependencyContainer(string name)
        {
            var container = ServiceProvider.GetDependencyFactory(name);
            if (container == null)
            {
                throw new System.Exception($"No container is found with the name '{name}'.");
            }
            return container;
        }

        #region Static Members
        /// <summary>
        /// Gets global IoC container. There's no special behavior in global
        /// container but only thing framework offers built-in access point for the global 
        /// IoC container.
        /// </summary>
        public static IocContainer GlobalIocContainer { get; internal set; }
        #endregion
    }

    

}
