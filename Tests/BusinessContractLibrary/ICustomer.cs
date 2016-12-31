namespace BusinessContractLibrary
{
    using NInterservice;
    using Models;
 
    [Contract]
    public interface ICustomer
    {
        void AddCustomer(Customer customer);
    }
}
