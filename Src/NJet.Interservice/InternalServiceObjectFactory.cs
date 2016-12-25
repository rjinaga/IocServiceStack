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

namespace NJet.Interservice
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Reflection;

    internal class InternalServiceObjectFactory
    {
        private static Dictionary<Type, Func<object>> _repositories;

        static InternalServiceObjectFactory()
        {
            _repositories = new Dictionary<Type, Func<object>>();
        }

        public static bool Contains(Type interfaceType)
        {
            return _repositories.ContainsKey(interfaceType);
        }

        public static void AddOrReplace<T>(Type interfaceType, Type serviceType, SubcontractFactory subcontract) where T : class
        {
            ConstructorInfo[] serviceConstructors = serviceType.GetConstructors();
            foreach (var serviceConstructor in serviceConstructors)
            {
                var attribute = serviceConstructor.GetCustomAttribute<ServiceInitAttribute>();
                if (attribute != null)
                {
                    /*Get parameters of service's constructor and inject corresponding repositories values to it */
                    ParameterInfo[] constructorParametersInfo = serviceConstructor.GetParameters();
                    Expression[] arguments = new Expression[constructorParametersInfo.Length];

                    int index = 0;
                    foreach (var constrParameter in constructorParametersInfo)
                    {
                        arguments[index] = subcontract.Create(constrParameter.ParameterType) ?? Expression.Default(constrParameter.GetType());
                        index++;
                    }

                    Func<T> serviceCreator = Expression.Lambda<Func<T>>(Expression.New(serviceConstructor, arguments)).Compile();
                    _repositories[interfaceType] = serviceCreator;

                    return;
                }
            }

            /*if none of them are not decorated with ServiceInitAttribute then initialize default with default constructor*/
            CreateWithDefaultConstructor(interfaceType, serviceType);
        }
        private static void CreateWithDefaultConstructor(Type interfaceType, Type serviceType)
        {
            NewExpression newExp = Expression.New(serviceType);
            // Create a new lambda expression with the NewExpression as the body.
            var lambda = Expression.Lambda<Func<object>>(newExp);
            // Compile our new lambda expression.
            _repositories[interfaceType] = lambda.Compile();
        }

        public static T Get<T>()
        {
            var type = typeof(T);
            if (_repositories.ContainsKey(type))
            {
                return (T)_repositories[type]();
            }

            return default(T);
        }
    }

}
