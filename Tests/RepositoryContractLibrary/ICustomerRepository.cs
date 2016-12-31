namespace RepositoryService
{
    using NInterservice;
    using Models;

    [Contract]
    public interface ICustomerRepository
    {
        void Add(Customer customer);
        void Update(Customer customer);
        void Delete(Customer customer);
        Customer GetCustomer(int customerId);
    }
}
