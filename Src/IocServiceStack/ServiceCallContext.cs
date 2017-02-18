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
    /// Represents the context of the service call.
    /// </summary>
    public sealed class ServiceCallContext
    {
        /// <summary>
        /// Gets read-only ContractType
        /// </summary>
        public readonly Type ContractType;

        /// <summary>
        /// Gets read-only ServiceType
        /// </summary>
        public readonly Type ServiceType;

        /// <summary>
        /// Gets read-only IocContainer
        /// </summary>
        public readonly IocContainer IocContainer;

        private ServiceCallContext(Type contractType, Type serviceType, IocContainer container)
        {
            ContractType = contractType;
            ServiceType = serviceType;
        }

        /// <summary>
        /// if ServiceInstance is set then internal service creation will be skipped and returns
        /// the instance that's supplied.
        /// </summary>
        public object ServiceInstance { get; set; }

        #region Static Members  

        /// <summary>
        /// Creates a new instance of <see cref="ServiceCallContext"/>.
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="serviceType"></param>
        /// <param name="container"></param>
        /// <returns></returns>
        public static ServiceCallContext Create(Type contractType, Type serviceType, IocContainer container)
        {
            return new ServiceCallContext(contractType, serviceType, container);
        }
        #endregion

    }
    
}
