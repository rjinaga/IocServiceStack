namespace IocServiceStack.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class ContainerAddServicesTests
    {
        [Test]
        public void Add_GetWhatYouSet_InstanceOf()
        {
            //Arrange
            var configRef = IocServicelet.CreateIocContainer(config => { /*No auto setup*/ });

            //Act
            //different ways of adding services
            configRef.GetServiceFactory().Add<IEmployee, Employee>()
                                         .Add<IEmployee>(() => new Employee1(), "Employee1") 
                                         .Add<IEmployee>(() => new Employee2(), "Employee2")
                                         .Add<IEmployee>(typeof(Employee3), "Employee3")
                                         .Add<IEmployee, SeniorEmployee>("Senior")
                                         .Add<IExecutive>(typeof(Executive));

            var provider = configRef.GetIocContainer().ServiceProvider;
            var employee = provider.GetService<IEmployee>();
            var employee1 = provider.GetService<IEmployee>("Employee1");
            var employee2 = provider.GetService<IEmployee>("Employee2");
            var employee3 = provider.GetService<IEmployee>("Employee3");
            var seniorEmployee = configRef.GetIocContainer().ServiceProvider.GetService<IEmployee>("Senior");
            var executive = provider.GetService<IExecutive>();
            var executive1 = provider.GetService<IExecutive>("NoExecutive");

            //Assert
            Assert.IsInstanceOf<Employee>(employee);
            Assert.IsInstanceOf<Employee1>(employee1);
            Assert.IsInstanceOf<Employee2>(employee2);
            Assert.IsInstanceOf<Employee3>(employee3);
            Assert.IsInstanceOf<SeniorEmployee>(seniorEmployee);
            Assert.IsInstanceOf<Executive>(executive);

            Assert.IsNull(executive1);
        }

        [Test]
        public void Add_InvalidServiceType_ThrowsException()
        {
            //Arrange
            var configRef = IocServicelet.CreateIocContainer(config => { /*No auto setup*/ });

            //Act
            TestDelegate test = ()=> configRef.GetServiceFactory().Add<IEmployee>(typeof(Executive));

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
