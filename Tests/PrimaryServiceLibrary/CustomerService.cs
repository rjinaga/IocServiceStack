namespace PrimaryServiceLibrary.Test
{
    using NInterservice;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DependentServiceLibrary;

    [Service]
    public class CustomerService : ICustomer
    {
        private ICustomerRepository _repository;

        [ServiceInit]
        public CustomerService(ICustomerRepository repository)
        {
            _repository = repository;
        }

        public void AddCustomer()
        {
            _repository.AddCustomer();
        }

        public ICustomerRepository GetRepository()
        {
            return _repository;
        }
    }
}
