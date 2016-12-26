# NJet.Interservice
NJet.Interservice is a .NET library for multi-services communication through multi-level dependency injection. This separates the concerns of application layers. This makes the layers configurable, but not required to reference dependent layer at compile or design time. References will be added virtually at runtime.


### Prerequisite
- .NET Core
- .NET Framework 4.6.2 


### Installing
```
Install-Package NJet.Interservice -Pre
```

### Services Registation

```c#
var configRef = ServiceInjector.Configure(config =>
{
  config.AddServices((serviceConfig) => 
                    { serviceConfig.Assemblies = new[] { "PrimaryServiceLibrary" }; })
        .AddDependentServices((serviceConfig) => 
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
### Web Application Architecture using NJet.Ineterservice

https://github.com/rjinaga/Web-App-Architecture-With-NJet-Interservice

