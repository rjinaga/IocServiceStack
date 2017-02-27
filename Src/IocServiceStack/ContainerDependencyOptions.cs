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
    /// Contains options for container dependencies.
    /// </summary>
    public sealed class ContainerDependencyOptions
    {
        private bool _readOnly;
        private string[] _namespaces, _assemblies;
        private ContainerDependencyOptions _dependencies;
        private string _name;

        /// <summary>
        /// Gets or sets name of the dependency container.
        /// </summary>
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                if (!_readOnly)
                {
                    _name = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets array of namespaces.
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
        /// Gets or sets array of assemblies.
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
       /// Gets dependency options 
       /// </summary>
        public ContainerDependencyOptions DependencyOptions
        {
            get
            {
                return _dependencies;
            }
        }

        /// <summary>
        /// Adds dependency assemblies. Specify the namespaces for dependencies to be searched,
        /// if don't want to add all available services.
        /// </summary>
        /// <param name="options"></param>
        public void AddDependencies(Action<ContainerDependencyOptions> options)
        {
            _dependencies = new ContainerDependencyOptions();
            options(_dependencies);
        }

        internal void MakeReadOnly()
        {
            _readOnly = true;
            _dependencies?.MakeReadOnly();
        }
    }

}
