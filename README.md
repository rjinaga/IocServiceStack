# NInterservice

NInterservice is a open source .NET library for multi-services communication through multi-level dependency injection. This separates the concerns of application layers. This makes the layers configurable, but not required to reference dependent layer at compile or design time. 


### Supports
- .NET Core 1.0 (.NET Standard 1.6)
- .NET Framework 4.6.2 


## [NuGet](https://www.nuget.org/packages/NJet.Interservice/)
```
PM> Install-Package NJet.Interservice -Pre
```
[![NuGet Pre Release](https://img.shields.io/badge/nuget-Pre%20Release-yellow.svg)](https://www.nuget.org/packages/NJet.Interservice/)

[![Gitter](https://badges.gitter.im/NInterservice/Lobby.svg)](https://gitter.im/NInterservice/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=body_badge)


### Services Registation

```c#
var configRef = ServiceInjector.Configure(config =>
{
  config.AddServices((serviceConfig) => 
                    { serviceConfig.Assemblies = new[] { "PrimaryServiceLibrary" }; })
        .AddDependencies((serviceConfig) => 
                    { serviceConfig.Assemblies = new[] { "DependentServiceLibrary" }; });
                    
  config.EnableStrictMode();
});
```

### Dependency Injection

```c#
var factoryService = configRef.GetFactoryService();

/*Dependency Injection*/
factoryService.Replace<ICustomer>(typeof(CustomerService2))
              .Subcontract
              .Replace<ICustomerRepository>(typeof(CustomerRepository2));

```

### Services and Contracts Implementations

```c#
namespace PrimaryServiceLibrary
{
    [Contract]
    public interface ICustomer
    {
        ICustomerRepository GetRepository();
    }
    
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

//DependentServiceLibrary Library
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
```
### Accessing service

```c#
var customerService = ServiceManager.GetService<ICustomer>();

```
### Web Application Architecture using NIneterservice

https://github.com/rjinaga/Web-App-Architecture-Using-NInterservice

