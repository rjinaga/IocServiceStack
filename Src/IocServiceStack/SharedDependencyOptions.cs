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
    /// <summary>
    /// Represents shared dependency options.
    /// </summary>
    public sealed class SharedDependencyOptions
    {
        private bool _readOnly;
        private string[] _namespaces, _assemblies;

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
        /// Gets or sets array of assemblies names.
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
       
        internal void MakeReadOnly()
        {
            _readOnly = true;
        }
    }
}
