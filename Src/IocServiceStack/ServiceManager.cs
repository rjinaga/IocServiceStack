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
    using System;
    public sealed class ServiceManager : IServiceManager
    {
        T IServiceManager.GetService<T>() 
        {
            var provider = IocContainer.GlobalIocContainer.ServiceProvider;
            return provider.GetService<T>();
        }

        T IServiceManager.GetService<T>(string serviceName)
        {
            var provider = IocContainer.GlobalIocContainer.ServiceProvider;
            return provider.GetService<T>(serviceName);
        }

        object IServiceManager.GetService(Type contractType)
        {
            var provider = IocContainer.GlobalIocContainer.ServiceProvider;
            return provider.GetService(contractType);
        }

        object IServiceManager.GetService(Type contractType, string serviceName)
        {
            var provider = IocContainer.GlobalIocContainer.ServiceProvider;
            return provider.GetService(contractType, serviceName);
        }

        #region Static Members
        private static IServiceManager _serviceManager;

        static ServiceManager()
        {
            _serviceManager = new ServiceManager();
        }

        public static IServiceManager Instance
        {
            get
            {
                return _serviceManager;
            }
        }

        public static T GetService<T>() where T : class
        {
            return _serviceManager.GetService<T>();
        }
        public static T GetService<T>(string serviceName) where T : class
        {
            return _serviceManager.GetService<T>(serviceName);
        }
        public static object GetService(Type contractType)
        {
            return _serviceManager.GetService(contractType);
        }
        public static object GetService(Type contractType, string serviceName)
        {
            return _serviceManager.GetService(contractType, serviceName);
        }
        #endregion
    }
}
