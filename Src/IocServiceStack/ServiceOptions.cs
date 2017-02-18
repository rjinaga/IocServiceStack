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
    /// The class contains the properties of the container.
    /// </summary>
    public class ContainerOptions
    {
        private bool _readOnly;
        private bool _strictMode;
        private string[] _namespaces, _assemblies;
        private IRootServiceFactory _serviceFactory;
        private ContainerDependencyOptions _dependencies;
        private SharedDependencyOptions _sharedDependencies;

        /// <summary>
        /// StrictMode applies the one contract with one service policy. This means if more than one service is implemented single contract interface
        /// then it will throw an exception. It ensures a contract interface is implemented by a single service. By default it's not enabled, mapping table wll have the last service in the list of inteface implementaions services.
        /// </summary>
        public bool StrictMode
        {
            get
            {
                return _strictMode;
            }
            set
            {
                if (!_readOnly)
                {
                    _strictMode = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the array of namespaces.
        /// </summary>
        public string[] Namespaces
        {
            get
            {
                return _namespaces;
            }
            set
            {
                if (!_readOnly)
                {
                    _namespaces = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets array of assemblies
        /// </summary>
        public string[] Assemblies
        {
            get
            {
                return _assemblies;
            }
            set
            {
                if (!_readOnly)
                {
                    _assemblies = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets root service factory of the container.
        /// </summary>
        public IRootServiceFactory ServiceFactory
        {
            get
            {
                return _serviceFactory;
            }
            set
            {
                if (!_readOnly)
                {
                    _serviceFactory = value;
                }
            }
        }

        /// <summary>
        /// Gets container dependency options.
        /// </summary>
        public ContainerDependencyOptions Dependencies
        {
            get
            {
                return _dependencies;
            }
        }

        /// <summary>
        /// Gets shared dependencies
        /// </summary>
        public SharedDependencyOptions SharedDependencies
        {
            get
            {
                return _sharedDependencies;
            }
        }

        /// <summary>
        /// Adds dependencies options that are set through the options callback.
        /// </summary>
        /// <param name="options">The callback action.</param>
        public void AddDependencies(Action<ContainerDependencyOptions> options)
        {
            _dependencies = new ContainerDependencyOptions();
            options(_dependencies);
        }

        /// <summary>
        /// Adds shared services
        /// </summary>
        /// <param name="options"></param>
        public void AddSharedServices(Action<SharedDependencyOptions> options)
        {
            _sharedDependencies = new SharedDependencyOptions();
            options(_sharedDependencies);
        }

        internal void MakeReadOnly()
        {
            _readOnly = true;
            _dependencies?.MakeReadOnly();
        }

    }
}
