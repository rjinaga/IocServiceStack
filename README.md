# NJet.Interservice
NJet.Interservice is a .NET library for multi-services communication through multi level dependency injection.

### prerequisite
- .NET Core
- .NET Framework 4.6.2 


### Installing
```
Install-Package NJet.Interservice -Pre
```

### Services Registation

```
  var configRef = ServiceInjector.Configure(config =>
  {
    config.AddServices((serviceConfig) => 
                      { serviceConfig.Assemblies = new[] { "PrimaryServiceLibrary" }; })
          .AddDependentServices((serviceConfig) => 
                      { serviceConfig.Assemblies = new[] { "DependentServiceLibrary" }; });
  });
```

### Dependency Injection

```
    var factoryService = configRef.GetFactoryService();

    /*Dependency Injection*/
    factoryService.Replace<ICustomer>(typeof(CustomerService2))
                  .Subcontract
                  .Replace<ICustomerRepository>(typeof(CustomerRepository2));

```

### Service and Contract Declaration

```
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

```
var customerService = ServiceManager.GetService<ICustomer>();

```

