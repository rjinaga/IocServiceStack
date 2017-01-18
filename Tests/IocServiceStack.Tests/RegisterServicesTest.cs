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

namespace IocServiceStack.Tests
{
    using NUnit.Framework;
    using System;


    /*This test module doesn't support parallel execution*/
    [SetUpFixture]
    public class SetupTests
    {
        [OneTimeSetUp]
        public void RegisterTest()
        {
            var configRef = IocServiceProvider.Configure(config =>
            {
                config.Services((opt) =>
                {
                    /*if namespaces are not specfied, it finds for services in entire assembly irrespective of namespaces.*/
                    opt.Namespaces = new[] { "BusinessService" };
                    opt.Assemblies = new[] { "BusinessServiceLibrary" };

                    opt.AddDependencies((repository) =>
                    {
                        repository.Name = "Repository"; //optional, but it can be used to access the Factory object.
                        repository.Namespaces = new[] { "RepositoryService" }; ;
                        repository.Assemblies = new[] { "RepositoryServiceLibrary" };

                        repository.AddDependencies(data =>
                        {
                            data.Name = "Data";
                            data.Namespaces = new[] { "DataService" };
                            data.Assemblies = new[] { "DataServiceLibrary" };
                        });
                    });
                    opt.StrictMode = true;
                });
                //.RegisterServiceProvider(new ProxyServiceProvider());
                config.Decorators.Add(new TestDecoratorAttribute());
            });

            
            //Hold the pointer of serviceConfig in a static field to run further tests of dependency injection.
            Helper.TestsHelper.FactoryServicePointer = configRef;
        }
    }
}
