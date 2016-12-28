namespace PrimaryServiceLibrary
{
    using NJet.Interservice;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using DependentServiceLibrary;

    [Contract]
    public interface ICustomer
    {
        ICustomerRepository GetRepository();
    }
}
