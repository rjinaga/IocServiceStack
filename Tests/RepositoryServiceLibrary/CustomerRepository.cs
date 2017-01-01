namespace RepositoryService
{
    using System;
    using NInterservice;
    using DataContractLibrary;
    using Models;

    [Service]
    public class CustomerRepository : ICustomerRepository
    {
        [ServiceInit]
        public CustomerRepository(IDbContext dbcontext)
        {
        }

        public void Add(Customer customer)
        {
            
        }

        public void Delete(Customer customer)
        {
            throw new NotImplementedException();
        }

        public Customer GetCustomer(int customerId)
        {
            throw new NotImplementedException();
        }

        public void Update(Customer customer)
        {
            throw new NotImplementedException();
        }
    }
}
