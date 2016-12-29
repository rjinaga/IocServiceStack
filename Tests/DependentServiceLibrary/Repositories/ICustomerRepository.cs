using NInterservice;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DependentServiceLibrary
{
    [Contract]
    public interface ICustomerRepository
    {
        void AddCustomer();
        void UpdateCustomer();
        void DeleteCustomer();
        void GetCustomer(int customerId);
    }
}
