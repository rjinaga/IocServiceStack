using NJet.Interservice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependentServiceLibrary
{
    [Service]
    public class CustomerRepository : ICustomerRepository
    {
        public CustomerRepository()
        {
        }

        public void AddCustomer()
        {
            throw new NotImplementedException();
        }

        public void DeleteCustomer()
        {
            throw new NotImplementedException();
        }

        public void GetCustomer(int customerId)
        {
            throw new NotImplementedException();
        }

        public void UpdateCustomer()
        {
            throw new NotImplementedException();
        }
    }
}
