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

    /// <summary>
    /// Represents basic service methods of factory
    /// </summary>
    public interface IContainerService
    {
        /// <summary>
        /// Gets or sets dependency factory
        /// </summary>
        SubcontractFactory DependencyFactory { get; set; }

        IContainerService Add<TC, TS>() where TC : class where TS : class;
        
        /// <summary>
        /// Adds the specified service to the factory
        /// </summary>
        /// <typeparam name="T">The class of the service</typeparam>
        /// <param name="service">The type of the service</param>
        /// <returns>Instance <see cref="IContainerService"/> of current object</returns>
        IContainerService Add<T>(Type service) where T : class;

        IContainerService AddSingleton<TC, TS>() where TC : class where TS : class;


        IContainerService Replace<TC, TS>() where TC : class where TS : class;
        
        
        /// <summary>
        /// Replaces the specified service in the factory.
        /// </summary>
        /// <typeparam name="T">The class of the service</typeparam>
        /// <param name="service">The type of the service</param>
        /// <returns>Instance <see cref="IContainerService"/> of current object</returns>
        IContainerService Replace<T>(Type service) where T : class;

        IContainerService ReplaceSingleton<TC, TS>() where TC : class where TS : class;
    }
}