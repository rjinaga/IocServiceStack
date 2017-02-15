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
    /// The purpose the external attribute is, the nature of this framework with multilevel model, 
    /// system injects the immediate dependencies of the current container but it does not inject the 
    /// next level dependencies. it works with only immediate dependencies.
    /// ExternalAttribute solves this problem, by setting this attribute to the parameter in the constructor, which can be 
    /// injected by the system from the specified dependency container.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class ExternalAttribute : Attribute
    {
        /// <summary>
        /// Gets name of the container.
        /// </summary>
        public readonly string ContainerName;

        /// <summary>
        /// Initializes new instance of <see cref="ExternalAttribute"/> with  <paramref name="dependencyContainerName"/> parameter.
        /// </summary>
        /// <param name="dependencyContainerName">The name of the dependency container, that has been set in the <see cref="IocContainer"/> configuration.</param>
        public ExternalAttribute(string dependencyContainerName)
        {
            if (string.IsNullOrWhiteSpace(dependencyContainerName))
            {
                throw new ArgumentNullException(dependencyContainerName);
            }
            ContainerName = dependencyContainerName;
        }
    }

}
