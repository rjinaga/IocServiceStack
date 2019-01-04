namespace IocServiceStack.Tests
{
    using System.Linq;
    using BusinessContractLibrary;
    using BusinessService;
    using DataContractLibrary;
    using DataService;
    using NUnit.Framework;
    using RepositoryService;

    [TestFixture]
    public class ContainerGetAllContractsTests
    {
        [Test]
        public void Add_GetAllContracts_Count()
        {
            //Arrange
            var container = IocServicelet.CreateContainer();
            int employeeId = 5;

            //Act
            //different ways of adding services
            container.GetRootContainer()
                                        .Add<IEmployee>(() => new Employee1(), "Employee1")
                                        .Add<IEmployee>(() => new Employee2(), "Employee2")
                                        .Add<IEmployee>(typeof(Employee3), "Employee3")
                                        .Add<IEmployee, SeniorEmployee>("Senior")
                                        .Add<IExecutive>(typeof(Executive))
                                        .Add<IEmployee>(() => new Employee(employeeId))
                                        .Add<ICustomer, CustomerService>(); /*this depends on the ICustomerRepository*/

            container.GetSharedContainer()
                                        .Add<ICustomerRepository, CustomerRepository>() /*this depends on the IDbContext*/
                                        .Add<IDbContext>(()=> new AdventureDbContext())
                                        ;
                                        

            var provider = container.ServiceProvider;

            var contracts = provider.GetRootServiceFactory().GetAllInterfaces();
            Assert.AreEqual(3, contracts.Count());

        }
        

        #region Tests Supporting Members
        interface IEmployee
        {
        }
        interface IExecutive
        {
        }
        class Executive : IExecutive
        {
        }
        class Employee : IEmployee
        {
            public Employee(int x) { }
        }
        class Employee1 : IEmployee
        {
        }
        class Employee2 : IEmployee
        {
        }
        class Employee3 : IEmployee
        {
        }
        class SeniorEmployee : IEmployee
        {

        }
        #endregion

    }
}
