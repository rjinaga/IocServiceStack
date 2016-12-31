namespace BusinessService
{
    using NInterservice;
    using BusinessContractLibrary;
    using Models;
    using RepositoryService;

    [Service]
    public class CustomerService : ICustomer
    {
        private ICustomerRepository _repository;

        [ServiceInit]
        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }
        
        public ICustomerRepository GetRepository()
        {
            return _repository;
        }

        public void AddCustomer(Customer customer)
        {
            _repository.Add(customer);
        }
    }
}
