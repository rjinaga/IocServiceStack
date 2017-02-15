namespace IocServiceStack.Tests
{
    using BusinessContractLibrary;
    using BusinessService;
    using DataContractLibrary;
    using DataService;
    using NUnit.Framework;
    using RepositoryService;

    [TestFixture]
    public class ContainerAddServicesTests
    {
        [Test]
        public void Add_GetWhatYouSet_InstanceOf()
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
                                        .Add<IDbContext, AdventureDbContext>();
                                        

            var provider = container.ServiceProvider;
            var employee = provider.GetService<IEmployee>();
            var employee1 = provider.GetService<IEmployee>("Employee1");
            var employee2 = provider.GetService<IEmployee>("Employee2");
            var employee3 = provider.GetService<IEmployee>("Employee3");
            var seniorEmployee = container.ServiceProvider.GetService<IEmployee>("Senior");
            var executive = provider.GetService<IExecutive>();
            var executive1 = provider.GetService<IExecutive>("NoExecutive");

            var specialCustomer = provider.GetService<ICustomer>();

            //Assert
            Assert.IsInstanceOf<Employee>(employee);
            Assert.IsInstanceOf<Employee1>(employee1);
            Assert.IsInstanceOf<Employee2>(employee2);
            Assert.IsInstanceOf<Employee3>(employee3);
            Assert.IsInstanceOf<SeniorEmployee>(seniorEmployee);
            Assert.IsInstanceOf<Executive>(executive);

            Assert.IsInstanceOf<CustomerService>(specialCustomer);
            //write assert to validate repository and dbcontext


            Assert.IsNull(executive1);
        }

        [Test]
        public void Add_InvalidServiceType_ThrowsException()
        {
            //Arrange
            var container = IocServicelet.CreateContainer(config => { /*No auto setup*/ });

            //Act
            TestDelegate test = ()=> container.GetRootContainer().Add<IEmployee>(typeof(Executive));

            //Assert
            Assert.Throws<InvalidServiceTypeException>(test);

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
