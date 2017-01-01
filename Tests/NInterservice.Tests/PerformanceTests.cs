namespace NInterservice.Tests
{
    using BusinessContractLibrary;
    using BusinessService;
    using RepositoryService;
    using DataService;
    using NUnit.Framework;
    using static ServiceManager;


    public class PerformanceTests
    {

        [Test, Order(1)]
        // [Ignore("Ignore this test")]
        public void Warmup_Test()
        {
            //Arrange & Act
            var customerService = GetService<ICustomer>();
        }

        [Test, Order(2)]
        // [Ignore("Ignore this test")]
        public void GetService_Performance_Test()
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
        public void GetService_Performance2_Test()
        {
            //Arrange & Act
            const int OneMillionTimes = 1000000;
            for (int i = 0; i < OneMillionTimes; i++)
            {
                var customer = GetService<ICustomer>();
            }

        }

        [Test, Order(4)]
        // [Ignore("Ignore this test")]
        public void Handcode_Performance_Test()
        {
            //Arrange & Act
            const int OneMillionTimes = 1000000;
            for (int i = 0; i < OneMillionTimes; i++)
            {
                ICustomer customer = new CustomerService(new CustomerRepository(new AdventureDbContext()));
            }
        }
    }
}
