namespace IocServiceStack.Tests
{

    using NUnit.Framework;

    [TestFixture]
    public class OptionalAttributeTests
    {
        [Test]
        public void Optional_Attribute_DependecyNull()
        {
            //Arrange
            var container = IocServicelet.CreateContainer();

            //Act
            //different ways of adding services
            container.GetRootContainer().Add<IEmployee, Employee>();

            var provider = container.ServiceProvider;

            var employee = provider.GetService<IEmployee>();

            //Assert
            Assert.IsInstanceOf<Employee>(employee);
            Assert.IsNull(employee.Address);

        }

        [Test]
        public void Optional_Attribute_Inject_DependencyNotNull()
        {
            //Arrange
            var container = IocServicelet.CreateContainer();

            //Act
            //different ways of adding services
            container.GetRootContainer().Add<IEmployee, Employee>();
            container.GetSharedContainer().Add<IAddress, Address>();

            var provider = container.ServiceProvider;
            var employee = provider.GetService<IEmployee>();

            //Assert
            Assert.IsInstanceOf<Employee>(employee);
            Assert.IsNotNull(employee.Address);

        }


        #region Tests Supporting Members
        interface IEmployee
        {
            IAddress Address { get; set; }
        }
        interface IAddress
        {
        }

        class Address : IAddress { }

        class Employee : IEmployee
        {
            public Employee([Optional]IAddress address)
            {
                Address = address;
            }

            public IAddress Address
            {
                get; set;
            }
        }

        #endregion

    }
}
