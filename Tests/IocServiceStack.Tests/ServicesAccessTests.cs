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
        public void GetService_ICustomer_CustomerService()
        {
            //Arrange & Act
            var customerService = GetService<ICustomer>();

            //Assert
            Assert.IsInstanceOf<CustomerService>(customerService);
        }

        [Test]
        [Order(2)]
        public void GetService_ICustomer_Decorator()
        {
            //Arrange & Act
            var customerService = GetService<ICustomer>();

            //Assert
            Assert.IsInstanceOf<CustomerService>(customerService);
            //Test Custom Decorator
            Assert.AreEqual(customerService.AdditionalData, "Gold Customer");
        }

        [Test]
        [Order(3)]
        public void GetService_AbstractSale_Null()
        {
            //Arrange & Act
            var sale = GetService<AbstractSale>();

            //Assert
            Assert.IsNull(sale);

        }

        [Test]
        [TestCase("Direct", typeof(DirectSale))]
        [TestCase("Online", typeof(OnlineSale))]
        [Order(4)]
        public void GetService_Abstract_NotNull(string serviceName, System.Type expected)
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
        [Order(5)]
        public void Inject_OtherKindOfSale_InstanceCheck()
        {
            //Arrange
            var serviceFactory = Helper.TestsHelper.FactoryServicePointer.GetRootContainer();

            serviceFactory.Add<AbstractSale>(() => new Helper.OtherKindOfSale(), "OtherKind")
                          .Add<AbstractSale, Helper.MiscSale>("MiscSale");

            //Act
            var sale = GetService<AbstractSale>("OtherKind");
            var miscSale = GetService<AbstractSale>("MiscSale");

            //Assert
            Assert.IsInstanceOf<Helper.OtherKindOfSale>(sale);
            Assert.IsInstanceOf<Helper.MiscSale>(miscSale);
        }
    }
}
