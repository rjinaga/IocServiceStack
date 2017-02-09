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
    /// Represents basic service methods of factory
    /// </summary>
    //public interface IBaseServiceFactory : IDependencyAttribute
    //{
       
    //    void AddService<TC, TS>() where TC : class where TS : TC;
    //    void AddService<TC, TS>(string serviceName) where TC : class where TS : TC;

    //    /// <summary>
    //    /// Adds the specified service to the factory
    //    /// </summary>
    //    /// <typeparam name="T">The class of the service</typeparam>
    //    /// <param name="service">The type of the service</param>
    //    /// <returns>Instance <see cref="IContainerService"/> of current object</returns>
    //    void AddService<T>(Type service) where T : class;
    //    void AddService<T>(Type service, string serviceName) where T : class;

    //    void AddSingletonService<TC, TS>() where TC : class where TS : TC;
    //    void AddSingletonService<TC, TS>(string serviceName) where TC : class where TS : TC;


    //    void ReplaceService<TC, TS>() where TC : class where TS : TC;
    //    void ReplaceService<TC, TS>(string serviceName) where TC : class where TS : TC;

    //    /// <summary>
    //    /// Replaces the specified service in the factory.
    //    /// </summary>
    //    /// <typeparam name="T">The class of the service</typeparam>
    //    /// <param name="service">The type of the service</param>
    //    /// <returns>Instance <see cref="IContainerService"/> of current object</returns>
    //    void ReplaceService<T>(Type service) where T : class;
    //    void ReplaceService<T>(Type service, string serviceName) where T : class;

    //    void ReplaceSingletonService<TC, TS>() where TC : class where TS : TC;
    //    void ReplaceSingletonService<TC, TS>(string serviceName) where TC : class where TS : TC;


    //    void AddService<TC>(Expression<Func<TC>> expression) where TC : class;
    //    void AddService<TC>(Expression<Func<TC>> expression, string serviceName) where TC : class;

    //    void ReplaceService<TC>(Expression<Func<TC>> expression) where TC : class;
    //    void ReplaceService<TC>(Expression<Func<TC>> expression, string serviceName) where TC : class;
    //}
}