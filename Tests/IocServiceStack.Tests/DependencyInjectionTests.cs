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
    using BusinessContractLibrary;
    using BusinessService;
    using Helper;
    using IocServiceStack;
    using NUnit.Framework;
    using RepositoryService;

    public class DependencyInjectionTests
    {
        [Test]
        public void ReplaceService_Test()
        {
            //Arrange
            var factoryService = Helper.TestsHelper.FactoryServicePointer.GetFactoryService();

            /*Dependency Injection*/
            factoryService.Replace<ICustomer, CustomerService2>()
                          .Subcontract
                          .Replace<ICustomerRepository, CustomerRepository2>();
            //Act
            var service = ServiceManager.GetService<ICustomer>();

            //Assert
            Assert.IsInstanceOf<CustomerService2>(service);
            //Assert.IsInstanceOf<CustomerRepository2>(service.GetRepository());

            //Reset for other tests
            RevertToOrignal();
        }

        [Test]
        public void ReplaceService_DirectInstance_Inject_Test()
        {
            //Arrange
            var factoryService = Helper.TestsHelper.FactoryServicePointer.GetFactoryService();

            /*Dependency Injection*/
            factoryService.Replace<ICustomer>(() => new CustomerService2(new CustomerRepository3()));

            //Act
            var service = ServiceManager.GetService<ICustomer>();

            //Assert
            Assert.IsInstanceOf<CustomerService2>(service);
            //Assert.IsInstanceOf<CustomerRepository2>(service.GetRepository());

            //Reset for other tests
            RevertToOrignal();
        }

        

        private void RevertToOrignal()
        {
            var factoryService = Helper.TestsHelper.FactoryServicePointer.GetFactoryService();

            factoryService.Replace<ICustomer, CustomerService>()
                          .Subcontract
                          .Replace<ICustomerRepository, CustomerRepository>();

        }
    }
}
