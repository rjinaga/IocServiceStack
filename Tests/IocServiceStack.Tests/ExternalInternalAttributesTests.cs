namespace IocServiceStack.Tests
{
    using System;
    using NUnit.Framework;

    public class ExternalInternalAttributesTests
    {
        [Test]
        public void ExternalAttribute()
        {
            //Arrange
            var container = IocServicelet.CreateContainer(config =>
            {
                config.AddServices(service =>
                {
                    service.AddDependencies(opt =>
                    {
                        opt.Name = "C1";

                        opt.AddDependencies(dopt =>
                        {
                            dopt.Name = "C2";
                        });
                    });
                });
            });

            container.GetRootContainer().Add<ILevel0, Level0>()
                                        .Add<ILevel2, Level2>();

            container.GetDependencyContainer("C1").Add<ILevel1, Level1>();
            container.GetDependencyContainer("C2").Add<ILevel3, Level3>();

            //Act

            TestDelegate test = ()=> container.ServiceProvider.GetService<ILevel0>();

            //Assert

            Assert.DoesNotThrow(test);

        }

        interface ILevel0
        {
        }
        interface ILevel1
        {
            string Value { get; set; }
        }
        interface ILevel2
        {
            
        }

        interface ILevel3
        {

        }

        class Level0 : ILevel0
        {
            public Level0(ILevel1 level1, [Internal]ILevel2 level2, [External("C2")]ILevel3 levl3)
            {
                if (level1 == null || level2 == null || levl3 == null)
                {
                    throw new Exception("Internal/External is failed.");
                }
            }
        }

        class Level1 : ILevel1
        {
            public Level1()
            {
                Value = Guid.NewGuid().ToString();
            }
            public string Value
            {
                get;

                set;
            }
        }

        class Level2 : ILevel2
        {
           
        }

        class Level3 : ILevel3
        {
            public Level3(string name)
            {

            }

        }
    }
}
