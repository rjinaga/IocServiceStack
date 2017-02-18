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
    using System.Linq.Expressions;
   
    /// <summary>
    /// Represents dependency factory
    /// </summary>
    public interface IDependencyFactory : ISubContainer
    {
        /// <summary>
        /// Gets or sets name of the factory.
        /// </summary>
        string Name { get;  }

        /// <summary>
        /// Gets dependency factory of the current factory.
        /// </summary>
        IDependencyFactory DependencyFactory { get; }

        /// <summary>
        /// Create <see cref="Expression"/> for service which is associated with contract.
        /// </summary>
        /// <param name="interfaceType">Type of contract</param>
        /// <param name="register">The ServiceRegister</param>
        /// <param name="state">The ServiceState</param>
        /// <returns>Returns <see cref="Expression"/> of service constructor.</returns>
        Expression Create(Type interfaceType, ServiceRegister register, ServiceState state);

        /// <summary>
        /// Gets or sets contract observer.
        /// </summary>
        IContractObserver ContractObserver { get; set; }

        
    }

}
