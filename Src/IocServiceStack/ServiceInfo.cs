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
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;
    
    /// <summary>
    /// Represents service info .
    /// </summary>
    public class ServiceInfo : BaseServiceInfo
    {
        /// <summary>
        /// Initializes a new instance of <see cref="BaseServiceInfo"/> class with specified parameters.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="decorators">Specify array of decorators to be executed.</param>
        public ServiceInfo(Type serviceType, DecoratorAttribute[] decorators) : base(serviceType, decorators)
        {

        }

        /// <summary>
        /// Initializes a new instance of <see cref="BaseServiceInfo"/> class with specified parameters.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="decorators">Specify array of decorators to be executed.</param>
        /// <param name="serviceName">Specify name of the service.</param>
        public ServiceInfo(Type serviceType, DecoratorAttribute[] decorators, string serviceName)
            : base(serviceType, decorators, serviceName)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of <see cref="BaseServiceInfo"/> class with specified parameters.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="decorators">Specify array of decorators to be executed.</param>
        /// <param name="serviceName">Specify name of the service.</param>
        /// <param name="isReusable">Indicates, whether <paramref name="serviceType"/> is reusable.</param>
        public ServiceInfo(Type serviceType, DecoratorAttribute[] decorators, bool isReusable, string serviceName) : base(serviceType, decorators, serviceName)
        {
            
        }

        /// <summary>
        /// Initializes a new instance of <see cref="BaseServiceInfo"/> class with specified parameters.
        /// </summary>
        /// <param name="serviceType">The service type.</param>
        /// <param name="decorators">Specify array of decorators to be executed.</param>
        /// <param name="isReusable">Indicates, whether <paramref name="serviceType"/> is reusable.</param>
        public ServiceInfo(Type serviceType, DecoratorAttribute[] decorators, bool isReusable) : base(serviceType, decorators, isReusable)
        {
            
        }

        /// <summary>
        /// This method to get service instance callback function.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override Func<T> GetServiceInstanceCallback<T>()
        {
            return null;
        }

        /// <summary>
        /// This method to get service instance expression
        /// </summary>
        /// <returns></returns>
        public override Expression GetServiceInstanceExpression()
        {
            return null;
        }
    }
}
