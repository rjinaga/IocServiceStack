namespace IocServiceStack.Tests
{
    using System;
    using NUnit.Framework;

    public class ReusableDependeciesTests
    {
        [Test]
        public void ReusableInstance()
        {
            var container = IocServicelet.CreateContainer(config =>
                {
                    config.AddServices(service =>
                    {
                        service.AddDependencies(opt =>
                            {
                                opt.Name = "C1";
                            });
                    });
                }, ContainerModel.MultiLevel);

            container.GetRootContainer().Add<ILevel0, Level0>();

            container.GetDependencyContainer("C1")
                    .Add<ILevel1, Level1>()
                    .Add<ILevel2, Level2>()
                    .Add<ILevel3>(()=>new Level3("TEst"));

            TestDelegate test = () => container.ServiceProvider.GetService<ILevel0>();


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
            ILevel1 GetLevel1();
        }

        interface ILevel3
        {
            
        }

        class Level0 : ILevel0
        {
            [ReuseDependency(typeof(ILevel1))]
            public Level0(ILevel1 level1, ILevel2 level2, ILevel3 levl3)
            {
                if (!object.ReferenceEquals(level1, level2.GetLevel1()))
                {
                    throw new InvalidServiceTypeException("ReferenceNotEqual");
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
            readonly ILevel1 _level1;
            public Level2(ILevel1 level1)
            {
                _level1 = level1;
            }
            public ILevel1 GetLevel1()
            {
                return _level1;
            }
        }

        class Level3 : ILevel3
        {
            public Level3(string name)
            {
                
            }
            
        }

    }
}
