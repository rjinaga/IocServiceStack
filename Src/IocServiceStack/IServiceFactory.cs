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
    /// Represents root service factory of main container.
    /// </summary>
    public interface IRootServiceFactory : IRootContainer
    {
        /// <summary>
        /// Initializes the service factory and its dependencies.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Returns <see cref="BaseServiceInfo"/> of specified <paramref name="contractType"/> and <paramref name="serviceName"/>.
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="serviceName"></param>
        /// <returns></returns>
        BaseServiceInfo GetServiceInfo(Type contractType, string serviceName);

        /// <summary>
        /// Creates associated service instance of specified contract <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">Specify contact type.</typeparam>
        /// <param name="serviceMeta"></param>
        /// <returns></returns>
        T Create<T>(BaseServiceInfo serviceMeta) where T : class;

        /// <summary>
        /// Creates associated service instance of specified contract.
        /// </summary>
        /// <param name="contractType"></param>
        /// <param name="serviceMeta"></param>
        /// <returns></returns>
        object Create(Type contractType, BaseServiceInfo serviceMeta);

        /// <summary>
        /// Returns dependency factory of specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IDependencyFactory GetDependencyFactory(string name);

        /// <summary>
        /// Returns shared factory.
        /// </summary>
        /// <returns></returns>
        ISharedFactory GetSharedFactory();

    }
}