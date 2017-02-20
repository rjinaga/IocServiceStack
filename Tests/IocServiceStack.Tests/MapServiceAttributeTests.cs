namespace IocServiceStack.Tests
{
    using NUnit.Framework;

    [TestFixture]
    public class MapServiceAttributeTests
    {
        [Test]
        public void MapServiceAttribute_Shared_Inject()
        {
            //Arrange
            var container = IocServicelet.CreateContainer();
            
            container.GetRootContainer().Add<IEmployee, Employee>();

            container.GetSharedContainer().Add<IExecutive, CEO>("CEO")
                                          .Add<IExecutive, CTO>("CTO");

            //Act
            var employee = container.ServiceProvider.GetService<IEmployee>();


            //Assert
            Assert.IsInstanceOf<CEO>(employee.CEO);
            Assert.IsInstanceOf<CTO>(employee.CTO);


        }

        [Test]
        public void MapServiceAttribute_Dependency_Inject()
        {
            //Arrange
            var container = IocServicelet.CreateContainer(config=> { config.AddServices(s => s.AddDependencies(opt => { opt.Name = "Data"; })); });

            //Act
            //different ways of adding services
            container.GetRootContainer().Add<IEmployee, Employee>();

            container.GetDependencyContainer("Data").Add<IExecutive, CEO>("CEO")
                                                    .Add<IExecutive, CTO>("CTO");

            //Act
            var employee = container.ServiceProvider.GetService<IEmployee>();


            //Assert
            Assert.IsInstanceOf<CEO>(employee.CEO);
            Assert.IsInstanceOf<CTO>(employee.CTO);
        }


        #region Tests Supporting Members
        interface IEmployee
        {
            IExecutive CEO { get; }
            IExecutive CTO { get; }
        }
        interface IExecutive
        {
        }

        class Employee : IEmployee
        {
            public Employee([MapService("CEO")]IExecutive executive1, [MapService("CTO")] IExecutive executive2)
            {
                CEO = executive1;
                CTO = executive2;
            }

            public IExecutive CEO
            {
                get; private set;
            }

            public IExecutive CTO
            {
                get; private set;
            }
        }

        class CEO : IExecutive
        {

        }
        class CTO : IExecutive
        {

        }

        #endregion

    }
}
