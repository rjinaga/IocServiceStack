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
    /// When a class is attributed with ServiceAttribute then auto mapper registers with their base types.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceAttribute : Attribute
    {
        /// <summary>
        /// Initializes new instance of <see cref="ServiceAttribute"/>
        /// </summary>
        public ServiceAttribute()
        {
        }

        /// <summary>
        /// Initializes new instance of <see cref="ServiceAttribute"/> with specified <paramref name="serviceName"/>
        /// </summary>
        /// <param name="serviceName"></param>
        public ServiceAttribute(string serviceName)
        {
            Name = serviceName;
        }

        /// <summary>
        /// Sets or Gets the name of the service. and it must be unique for that particular contract type.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Sets or Gets array of contracts that can be mapped to current service class. This attribute is useful when
        /// service class is inherited from third party interfaces and should map to their contract(s). Even this can be
        /// used with your own contracts which has no contract attribute specified.
        /// </summary>
        public Type[] Contracts { get; set; }

    }
}
