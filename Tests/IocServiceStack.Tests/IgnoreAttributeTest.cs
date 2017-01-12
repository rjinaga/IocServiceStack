namespace IocServiceStack.Tests
{
    using BusinessService;
    using NUnit.Framework;
    using static IocServiceStack.ServiceManager;

    public class IgnoreAttributeTest
    {
        [Test]
        public void IgnoreAttribute_Test()
        {
            //Arrange & Act
            var customerService = GetService<ISelfService>();
            var repository = customerService.GetRepository();

            //Assert
            Assert.IsInstanceOf<SelfCustomerService>(customerService);
            Assert.IsNull(repository);
        }
    }
}
