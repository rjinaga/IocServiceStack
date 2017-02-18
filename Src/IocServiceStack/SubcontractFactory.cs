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
    using System.Reflection;

    /// <summary>
    /// Represents the factory of dependency.
    /// </summary>
    public abstract class SubcontractFactory : BaseServiceFactory, ISubContainer
    {
        /// <summary>
        /// Initializes a new instance of <see cref="SubcontractFactory"/>  class with specified parameters
        /// <paramref name="namespaces"/>, <paramref name="assemblies"/> and <paramref name="strictMode"/>.
        /// </summary>
        /// <param name="namespaces">The array of namespaces to be searched in for services.</param>
        /// <param name="assemblies">The array of assemblies to be searched in for services.</param>
        /// <param name="strictMode">The value indicating whether strict mode is on or off, if strict mode is true then 
        /// system throws an exception if a contract is implemented by more than one service. this prevents the duplicate 
        /// implementation.
        /// </param>
        /// <param name="dependencyFactory"></param>
        /// <param name="sharedFactory"></param>
        public SubcontractFactory(string[] namespaces, Assembly[] assemblies, bool strictMode, IDependencyFactory dependencyFactory, ISharedFactory sharedFactory) : base(namespaces, assemblies, strictMode, dependencyFactory, sharedFactory)
        {
            
        }

        /// <summary>
        /// Create <see cref="Expression"/> for service which is associated with contract.
        /// </summary>
        /// <param name="interfaceType">Type of contract</param>
        /// <param name="register">The ServiceRegister</param>
        /// <param name="state">The ServiceState</param>
        /// <returns>Returns <see cref="Expression"/> of service constructor.</returns>
        public abstract Expression Create(Type interfaceType, ServiceRegister register, ServiceState state);

        /// <summary>
        /// Adds specified service for the specified contract T to the collection.
        /// </summary>
        /// <typeparam name="T">Contract type(interface or class)</typeparam>
        /// <param name="service">Type of service that's a implementation of specified contract.</param>
        /// <returns>Returns <see cref="IDependencyFactory"/> </returns>
        public ISubContainer Add<T>(Type service) where T : class
        {
            AddService<T>(service);
            return this;
        }

        /// <summary>
        /// Adds specified service with a name for the specified contract T to the collection.
        /// This allows to register multiple implementations of same contract those are accessed by a name.
        /// </summary>
        /// <typeparam name="T">Contract type(interface or class)</typeparam>
        /// <param name="service">Type of service that's a implementation of specified contract.</param>
        /// <param name="serviceName">Name of the service</param>
        /// <returns>Returns <see cref="IDependencyFactory"/> </returns>
        public ISubContainer Add<T>(Type service, string serviceName) where T : class
        {
            AddService<T>(service, serviceName);
            return this;
        }

        /// <summary>
        /// Adds specified service (TS) for the specified contract (TC) to the collection.
        /// </summary>
        /// <typeparam name="TC">Type of the contract</typeparam>
        /// <typeparam name="TS">Type of the service</typeparam>
        /// <returns>Returns <see cref="IDependencyFactory"/> </returns>
        public ISubContainer Add<TC, TS>()
            where TC : class
            where TS : TC
        {
            AddService<TC,TS>();
            return this;
        }

        /// <summary>
        /// Adds specified service (TS) with service name for the specified contract (TC) to the collection.
        /// </summary>
        /// <typeparam name="TC">Type of the contract</typeparam>
        /// <typeparam name="TS">Type of the service</typeparam>
        /// <param name="serviceName">Type of service that's a implementation of specified contract.</param>
        /// <returns>Returns <see cref="IDependencyFactory"/> </returns>
        public ISubContainer Add<TC, TS>(string serviceName)
            where TC : class
            where TS : TC
        {
            AddService<TC, TS>(serviceName);
            return this;
        }


        /// <summary>
        /// Replaces specified service for the specified contract T.
        /// </summary>
        /// <typeparam name="T">Contract type(interface or class)</typeparam>
        /// <param name="service">Type of service that's a implementation of specified contract.</param>
        /// <returns>Returns <see cref="IDependencyFactory"/> </returns>
        public ISubContainer Replace<T>(Type service) where T : class
        {
            ReplaceService<T>(service);
            return this;
        }

        /// <summary>
        /// Replaces specified service (TS) with service name for the specified contract (TC).
        /// </summary>
        /// <typeparam name="T">Type of the contract</typeparam>
        /// <param name="service">Type of the service</param>
        /// <param name="serviceName">Type of service that's a implementation of specified contract.</param>
        /// <returns>Returns <see cref="IDependencyFactory"/> </returns>
        public ISubContainer Replace<T>(Type service, string serviceName) where T : class
        {
            ReplaceService<T>(service, serviceName);
            return this;
        }

        /// <summary>
        /// Replaces specified service (TS) for the specified contract (TC).
        /// </summary>
        /// <typeparam name="TC">Type of the contract</typeparam>
        /// <typeparam name="TS">Type of the service</typeparam>
        /// <returns>Returns <see cref="IDependencyFactory"/> </returns>
        public ISubContainer Replace<TC, TS>()
            where TC : class
            where TS : TC
        {
            ReplaceService<TC, TS>();
            return this;
        }

        /// <summary>
        /// Replaces specified service (<typeparamref name="TS"/>) with service name for the specified contract (<typeparamref name="TC"/>).
        /// </summary>
        /// <typeparam name="TC">Type of the contract</typeparam>
        /// <typeparam name="TS">Type of the service</typeparam>
        /// <param name="serviceName">Type of service that's a implementation of specified contract.</param>
        /// <returns>Returns <see cref="IDependencyFactory"/> </returns>
        public ISubContainer Replace<TC, TS>(string serviceName)
            where TC : class
            where TS : TC
        {
            ReplaceService<TC, TS>(serviceName);
            return this;
        }

        /// <summary>
        /// Adds specified <see cref="Expression"/> for the specified contract <typeparamref name="TC"/>
        /// </summary>
        /// <typeparam name="TC">Type of the contract.</typeparam>
        /// <param name="expression"><see cref="Func{TC}"/>, in which it must pass the instance of service.</param>
        /// <returns>Returns <see cref="IDependencyFactory"/> </returns>
        public ISubContainer Add<TC>(Expression<Func<TC>> expression) where TC : class
        {
            AddService<TC>(expression);
            return this;
        }

        /// <summary>
        /// Adds specified <see cref="Expression"/> for the specified contract <typeparamref name="TC"/>
        /// </summary>
        /// <typeparam name="TC">Type of the contract.</typeparam>
        /// <param name="expression"><see cref="Func{TC}"/>, in which it must pass the instance of service.</param>
        /// <param name="serviceName">Specifies name of the service</param>
        /// <returns>Returns <see cref="IDependencyFactory"/> </returns>
        public ISubContainer Add<TC>(Expression<Func<TC>> expression, string serviceName) where TC : class
        {
            AddService<TC>(expression, serviceName);
            return this;
        }

        /// <summary>
        /// Replaces specified <see cref="Expression"/> for the specified contract <typeparamref name="TC"/>
        /// </summary>
        /// <typeparam name="TC">Type of the contract.</typeparam>
        /// <param name="expression"><see cref="Func{TC}"/>, in which it must pass the instance of service.</param>
        /// <returns>Returns <see cref="IDependencyFactory"/> </returns>
        public ISubContainer Replace<TC>(Expression<Func<TC>> expression) where TC : class
        {
            ReplaceService<TC>(expression);
            return this;
        }

        /// <summary>
        /// Replaces specified <see cref="Expression"/> for the specified contract <typeparamref name="TC"/>
        /// </summary>
        /// <typeparam name="TC">Type of the contract.</typeparam>
        /// <param name="expression"><see cref="Func{TC}"/>, in which it must pass the instance of service.</param>
        /// <param name="serviceName">Specifies name of the service</param>
        /// <returns>Returns <see cref="IDependencyFactory"/> </returns>
        public ISubContainer Replace<TC>(Expression<Func<TC>> expression, string serviceName) where TC : class
        {
            ReplaceService<TC>(expression, serviceName);
            return this;
        }
    }
}
