namespace IocServiceStack.Tests
{
    using BusinessContractLibrary;
    using BusinessService;
    using DataContractLibrary;
    using DataService;
    using NUnit.Framework;
    using RepositoryService;

    [TestFixture]
    public class ContainerAddServicesTests2
    {
        [Test]
        public void Add2_GetWhatYouSet_InstanceOf()
        {
            int maxConnections = 1;
            //Arrange
            var container = IocServicelet.CreateContainer();
            

            //Act
            //different ways of adding services
            container.GetRootContainer()
                                       .Add<IEmployee, Employee1>();


            container.GetSharedContainer()
                                        .Add<ICustomerRepository, CustomerRepository>() /*this depends on the IDbContext*/
                                        .Add<IDbContext>(() => new AdventureDbContext(maxConnections));
                                        
                                        
            //Act
            var provider = container.ServiceProvider;
            var employee = provider.GetService<IEmployee>();



            //Assert
            Assert.IsNotNull(employee);
           
        }


        #region Tests Supporting Members
       
        interface IEmployee
        {

        }

        class Employee1 : IEmployee
        {
            public Employee1(ICustomerRepository repo, IDbContext context )
            {

            }
        }

        #endregion

    }
}
