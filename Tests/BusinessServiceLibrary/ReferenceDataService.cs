namespace BusinessService
{
    using NInterservice;
    using BusinessContractLibrary;

    [Service(IsReusable = true)]
    public class ReferenceDataService : IReferenceData
    {
        private int _count = 0;

        //This method is for IsReusable service test
        public int Increment()
        {
            return ++_count;
        }
    }
}
