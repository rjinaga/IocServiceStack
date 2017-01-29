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
    public interface IServiceProvider
    {
        /// <summary>
        /// Sets decorator manager
        /// </summary>
        IDecoratorManager DecoratorManager { set; }

        /// <summary>
        /// Sets current instance's container
        /// </summary>
        IocContainer IocContainer { set; }

        /// <summary>
        /// Get root service factory
        /// </summary>
        /// <returns></returns>
        IServiceFactory GetServiceFactory();

        /// <summary>
        /// Get dependency factory by specified name.
        /// </summary>
        /// <param name="name">The name of the dependency factory</param>
        /// <returns></returns>
        IContainerService GetDependencyFactory(string name);


        T GetService<T>() where T : class;
        T GetService<T>(string serviceName) where T : class;

        object GetService(Type contractType);
        object GetService(Type contractType, string serviceName);

    }
}
