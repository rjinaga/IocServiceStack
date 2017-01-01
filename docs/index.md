# NInterservice

NInterservice is a open source .NET library for multi-services communication through multi-level dependency injection. This separates the concerns of application layers. This makes the layers configurable, but not required to reference the dependent layer at design time. 


### Supports
- .NET Core 1.0 (.NET Standard 1.6)
- .NET Framework 4.6


## [NuGet](https://www.nuget.org/packages/NJet.Interservice/)
```
PM> Install-Package NInterservice -Pre
```
[![NuGet Pre Release](https://img.shields.io/badge/nuget-Pre%20Release-yellow.svg)](https://www.nuget.org/packages/NInterservice/)

[![Gitter](https://badges.gitter.im/NInterservice/Lobby.svg)](https://gitter.im/NInterservice/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=body_badge)


### Services Configuration

```c#
var configRef = ServiceInjector.Configure(config =>
{
    config.Services((opt) =>
    {
	/*if namespaces are not specfied, it finds for services in entire assembly.*/
        opt.Namespaces = new[] { "BusinessService" };
        opt.Assemblies = new[] { "BusinessServiceLibrary" };

        opt.AddDependencies((dopt) =>
        {
            dopt.Namespaces = new[] { "RepositoryService" }; ;
            dopt.Assemblies = new[] { "RepositoryServiceLibrary" };

            dopt.AddDependencies(ddopt =>
            {
                ddopt.Namespaces = new[] { "DataService" };
                ddopt.Assemblies = new[] { "DataServiceLibrary" };
            });
        });

        opt.StrictMode = true;
    });
    //.SetServiceManager(new ProxyServiceManager());
});
```

### Services and Contracts Implementations

> Specify [Contract] and [Service] attributes if you want to support the dependency injection 
> and auto map the contracts and services in your application.

```c#
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
            throw new NotImplementedException();
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


namespace DataContractLibrary
{
    using NInterservice;

    [Contract]
    public interface IDbContext
    {
        
    }
}

namespace DataService
{
    using NInterservice;
    using DataContractLibrary;

    [Service]
    public class AdventureDbContext : IDbContext
    {
        public AdventureDbContext()
        {

        }
    }
}

```

### Accessing service

```c#
var customerService = ServiceManager.GetService<ICustomer>();

```

### Dependency Injection
You can replace with another service which is already discovered by the NInterservice and registered, or add a new service.

```c#
var factoryService = configRef.GetFactoryService();

/*Dependency Injection*/
factoryService.Replace<ICustomer>(typeof(CustomerService2))
              .Subcontract
              .Replace<ICustomerRepository>(typeof(CustomerRepository2));

/*Add new service*/
factoryService.Add<IPayment>(typeof(PaypalPayment));

```

### Web Application Architecture using NIneterservice

https://github.com/rjinaga/Web-App-Architecture-Using-NInterservice

