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
    using System;

    /// <summary>
    /// Represents service accessor associated with the container.
    /// </summary>
    public sealed class ServiceManager : IServiceManager
    {
        T IServiceManager.GetService<T>() 
        {
            ThrowIfContainerNotConfigured();
            var provider = IocContainer.GlobalIocContainer.ServiceProvider;
            return provider.GetService<T>();
        }

        T IServiceManager.GetService<T>(string serviceName)
        {
            ThrowIfContainerNotConfigured();
            var provider = IocContainer.GlobalIocContainer.ServiceProvider;
            return provider.GetService<T>(serviceName);
        }

        object IServiceManager.GetService(Type contractType)
        {
            ThrowIfContainerNotConfigured();
            var provider = IocContainer.GlobalIocContainer.ServiceProvider;
            return provider.GetService(contractType);
        }

        object IServiceManager.GetService(Type contractType, string serviceName)
        {
            ThrowIfContainerNotConfigured();
            var provider = IocContainer.GlobalIocContainer.ServiceProvider;
            return provider.GetService(contractType, serviceName);
        }

        private void ThrowIfContainerNotConfigured()
        {
            if (IocContainer.GlobalIocContainer == null)
            {
                throw new UnconfiguredException("Global IoC container is not configured. Use IocServiceStack.Configure method to configure the container that can be accessed through this method.");
            }
        }

        #region Static Members

        private static IServiceManager _serviceManager;

        static ServiceManager()
        {
            _serviceManager = new ServiceManager();
        }

        /// <summary>
        /// Gets single instance of <see cref="IServiceManager"/>
        /// </summary>
        public static IServiceManager Instance
        {
            get
            {
                return _serviceManager;
            }
        }

        /// <summary>
        /// Get service of the specified contract.
        /// </summary>
        /// <typeparam name="T">The type of the contract.</typeparam>
        /// <returns>Returns service that's associated with the contract.</returns>
        public static T GetService<T>() where T : class
        {
            return _serviceManager.GetService<T>();
        }

        /// <summary>
        /// Get service of the specified contract with service name.
        /// </summary>
        /// <typeparam name="T">The type of the contract.</typeparam>
        /// <param name="serviceName">The name of the service in specified contract family.</param>
        /// <returns>Returns service that's associated with the contract.</returns>
        public static T GetService<T>(string serviceName) where T : class
        {
            return _serviceManager.GetService<T>(serviceName);
        }

        /// <summary>
        /// Get service of the specified contract with service name.
        /// </summary>
        /// <param name="contractType">The type of the contract.</param>
        /// <returns>Returns service that's associated with the contract.</returns>
        public static object GetService(Type contractType)
        {
            return _serviceManager.GetService(contractType);
        }

        /// <summary>
        /// Get service of the specified contract with service name.
        /// </summary>
        /// <param name="T">The type of the contract.</param>
        /// <param name="serviceName">The name of the service in specified contract family.</param>
        /// <returns>Returns service that's associated with the contract.</returns>
        public static object GetService(Type contractType, string serviceName)
        {
            return _serviceManager.GetService(contractType, serviceName);
        }

        #endregion
    }
}
