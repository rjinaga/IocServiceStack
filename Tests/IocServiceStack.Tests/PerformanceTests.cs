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
    using RepositoryService;
    using DataService;
    using NUnit.Framework;
    using static ServiceManager;

    [TestFixture]
    public class PerformanceTests
    {
        [Test, Order(1)]
        // [Ignore("Ignore this test")]
        public void Warmup()
        {
            //Arrange & Act
            var customerService = GetService<ICustomer>();
        }

        [Test, Order(2)]
        // [Ignore("Ignore this test")]
        public void GetService_Performance_Time()
        {
            //Arrange & Act
            const int OneMillionTimes = 1000000;
            for (int i = 0; i < OneMillionTimes; i++)
            {
                var customer = GetService<ICustomer>();
            }
            
        }

        [Test, Order(3)]
        // [Ignore("Ignore this test")]
        public void GetService_Performance2_Time()
        {
            //Arrange & Act
            const int OneMillionTimes = 1000000;
            for (int i = 0; i < OneMillionTimes; i++)
            {
                var customer = GetService<ICustomer>();
            }

        }

        [Test, Order(3)]
        // [Ignore("Ignore this test")]
        public void GetService_Performance3_Time()
        {
            //Arrange & Act
            const int OneMillionTimes = 1000000;
            for (int i = 0; i < OneMillionTimes; i++)
            {
                var customer = GetService(typeof(ICustomer));
            }
        }

        [Test, Order(4)]
        // [Ignore("Ignore this test")]
        public void Directcode_Performance_Time()
        {
            //Arrange & Act
            const int OneMillionTimes = 1000000;
            for (int i = 0; i < OneMillionTimes; i++)
            {
                var x = CreateService();
            }
        }
     

        private ICustomer CreateService()
        {
             var customer = new CustomerService(new CustomerRepository(new AdventureDbContext()));
            return customer;
        }
    }
}
