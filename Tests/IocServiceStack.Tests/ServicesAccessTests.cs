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
    using NUnit.Framework;
    using static ServiceManager;

    public class ServicesAccessTests
    {
        [Test]
        [Order(1)]
        public void CustomerService_Test()
        {
            //Arrange & Act
            var customerService = GetService<ICustomer>();

            //Assert
            Assert.IsInstanceOf<CustomerService>(customerService);

            //Test Custom Decorator
            Assert.AreEqual(customerService.AdditionalData, "Gold Customer");

            //Assert.IsInstanceOf<CustomerRepository>(customerService.GetRepository());
        }

        [Test]
        [Order(2)]
        public void Sale_DefaultService_Test()
        {
            //Arrange & Act
            var sale = GetService<AbstractSale>();

            //Assert
            Assert.IsNull(sale);

        }

        [Test]
        [TestCase("Direct", typeof(DirectSale))]
        [TestCase("Online", typeof(OnlineSale))]
        [Order(3)]
        public void Sale_Abstract_Test(string serviceName, System.Type expected)
        {
            //Arrange & Act
            var sale = GetService<AbstractSale>(serviceName);

            //Assert
            Assert.IsNotNull(sale);

            var orderSuccess = sale.ProcessOrder();
            Assert.IsInstanceOf(expected,sale);
            Assert.AreEqual(orderSuccess, serviceName);
        }

        [Test]
        [Order(4)]
        public void Inject_OtherKindOfSale_Test()
        {
            //Arrange
            var serviceFactory = Helper.TestsHelper.FactoryServicePointer.GetServiceFactory();
            serviceFactory.Add<AbstractSale>(() => new Helper.OtherKindOfSale(), "OtherKind");

            //Act
            var sale = GetService<AbstractSale>("OtherKind");

            //Assert
            Assert.IsInstanceOf<Helper.OtherKindOfSale>(sale);
        }

        [Test, Order(5)]
        //[Ignore("Ignore this test")]
        public void GetService_Performance_DirectExpression_Test()
        {
            //Arrange & Act
            const int OneMillionTimes = 1000000;
            for (int i = 0; i < OneMillionTimes; i++)
            {
                var sale = GetService<AbstractSale>("OtherKind");
            }
        }
    }
}
