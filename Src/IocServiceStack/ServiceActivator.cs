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
    /// ServiceActivator class has functionality to activates the service for the specified contract.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceActivator<T> : IServiceActivator where T: class
    {
        private T _reusableInstance;
        private readonly object _syncObject = new object();
        private readonly Func<T> _creator;
        private readonly bool _isReusable;

        /// <summary>
        /// Initializes a new instance of <see cref="ServiceActivator{T}"/> class with specified <paramref name="creator"/> and <paramref name="isReusable"/> parameters.
        /// </summary>
        /// <param name="creator"></param>
        /// <param name="isReusable"></param>
        public ServiceActivator(Func<T> creator, bool isReusable)
        {
            _creator = creator;
            _isReusable = isReusable;
        }

        TService IServiceActivator.CreateInstance<TService>() 
        {
            if (_isReusable)
            {
                if (_reusableInstance == null)
                {
                    lock (_syncObject)
                    {
                        if (_reusableInstance == null)
                        {
                            _reusableInstance = _creator();
                        }
                    }
                }
                return _reusableInstance as TService;
            }
            else
            {
                return _creator() as TService;
            }
        }
    }
}
