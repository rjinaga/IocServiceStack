namespace NInterservice.Tests
{
    using BusinessContractLibrary;
    using NUnit.Framework;
    using static ServiceManager;

    public class ReusableServiceInstanceTest
    {
        [Test]
        public void Reusable_ServiceInstance_Test()
        {
            //Arrange
            var referenceService1 = GetService<IReferenceData>();
            var referenceService2 = GetService<IReferenceData>();

            //Act
            var count1 = referenceService1.Increment();
            var count2 = referenceService2.Increment();

            //Assert
            Assert.AreSame(referenceService1, referenceService2);
            Assert.AreEqual(count1, 1);
            Assert.AreEqual(count2, 2);

        }
    }
}
