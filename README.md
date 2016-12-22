# NJet.Interservice
NJet.Interservice is a library for multi-services communication through multi level dependency injection.

### Installing
```
Install-Package NJet.Interservice -Pre
```

### Services Registation

```
             ServiceRouterManager.Configure(config =>
            {
                config.AddServices((serviceConfig) => { serviceConfig.Namespaces = new[] { "PrimaryServiceLibrary" }; serviceConfig.Assemblies = new[] { "PrimaryServiceLibrary" }; })
                      .AddDependentServices((serviceConfig) => { serviceConfig.Namespaces = new[] { "DependentServiceLibrary" }; serviceConfig.Assemblies = new[] { "DependentServiceLibrary" }; });
                
            });
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

/*Accessing service*/
var customerService = ServiceManager.GetService<ICustomer>();

```


