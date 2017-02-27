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
    using System.Collections.Generic;

    /// <summary>
    /// Represents configuration of the container.
    /// </summary>
    public sealed class ContainerConfig : IContainerConfig
    {
        private ContainerOptions _containerOptions;
        private IServiceProvider _serviceProvider;
        private List<DecoratorAttribute> _decorators;

        /// <summary>
        /// Initializes a new instance of <see cref="ContainerConfig"/> class.
        /// </summary>
        public ContainerConfig()
        {
            _containerOptions = new ContainerOptions();
            _decorators = new List<DecoratorAttribute>();
        }

        /// <summary>
        /// Gets collection of decorators of specified type
        /// </summary>
        public List<DecoratorAttribute> Decorators => _decorators;

        /// <summary>
        /// Gets <see cref="ContainerOptions"/> 
        /// </summary>
        internal ContainerOptions ContainerOptions => _containerOptions;

        /// <summary>
        /// Gets <see cref="IServiceProvider"/>
        /// </summary>
        internal IServiceProvider ServiceProvider => _serviceProvider;

        /// <summary>
        /// Adds root services to the container.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public IContainerConfig AddServices(Action<ContainerOptions> config)
        {
            config(_containerOptions);
            return this;
        }

        /// <summary>
        /// Registers service provider for the IoC container.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public IContainerConfig RegisterServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            return this;
        }

    }
}
