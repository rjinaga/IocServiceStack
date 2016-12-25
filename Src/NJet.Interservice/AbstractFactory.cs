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
    using System.Reflection;

    public abstract class AbstractFactory : IBasicService
    {
        protected readonly ContractServiceAutoMapTable ServicesMapTable;

        public AbstractFactory(string[] namespaces, Assembly[] assemblies, bool strictMode)
        {
            ServicesMapTable = new ContractServiceAutoMapTable(namespaces, assemblies, strictMode);
            ServicesMapTable.Map();
        }

        public SubcontractFactory Subcontract
        {
            get; set;
        }


        public virtual IBasicService Add<T>(Type service) where T : class
        {
            Type interfaceType = typeof(T);
            ServicesMapTable.Add(interfaceType, new ServiceMeta { ServiceType = service });
            return this;
        }

        public virtual IBasicService Replace<T>(Type service) where T : class
        {
            Type interfaceType = typeof(T);
            ServicesMapTable[interfaceType].ServiceType = service;

            return this;
        }
    }
}
