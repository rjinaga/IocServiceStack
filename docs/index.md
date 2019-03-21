# IocServiceStack 
> **Multi-service and multi-level dependency injection framework for .NET**

[![Build status](https://ci.appveyor.com/api/projects/status/bylhcbchnjas953q?svg=true)](https://ci.appveyor.com/project/rjinaga/iocservicestack)

IocServiceStack is open source .NET dependency injection framework. IoC container allows to add multiple services for the same contract by name and dependencies can be added at different levels. This makes the concerns of application layers separated, and configurable. So, the client-specific logic can be plugged easily or inject the specific dependencies for specific client implementations. And, it offers several ways to inject dependencies. This framework contains the main container in which there are three types of internal containers such as root container, shared and dependency container.

### Features:
- Global IoC Container
- Isolated IoC Containers
- Shared dependencies.
- Automatically maps the services to their contracts
- Add or Replace dependencies
- Inject dependencies through decorators
- Multi level dependencies
- Multiple services capabilities

### Supports
- .NET Core 1.0 (.NET Standard 1.6)
- .NET Framework 4.6

## [NuGet](https://www.nuget.org/packages/IocServiceStack/)
```
PM> Install-Package IocServiceStack
```
[![NuGet Release](https://img.shields.io/badge/nuget-v2.0.4-blue.svg)](https://www.nuget.org/packages/IocServiceStack/)

## Usage Examples

### IocServiceStack Setup 

#### Automatically wires up the services - Setup

```c#
var container = IocServicelet.Configure(config =>
{
    config.AddServices((service) =>
    {
	/*if namespaces are not specfied, it finds for services in entire assembly.*/
        service.Namespaces = new[] { "BusinessService" };
        service.Assemblies = new[] { "BusinessServiceLibrary" };

        service.AddDependencies((repository) =>
        {
            repository.Name = "Repository";
            repository.Namespaces = new[] { "RepositoryService" }; ;
            repository.Assemblies = new[] { "RepositoryServiceLibrary" };

            repository.AddDependencies((data) =>
            {
                data.Name = "Data";
                data.Namespaces = new[] { "DataService" };
                data.Assemblies = new[] { "DataServiceLibrary" };
            });
        });
        //service.AddSharedServices(shared=> { shared.Assemblies =  new {""} });
        service.StrictMode = true;
    });
  //.RegisterServiceProvider(new ProxyServiceProvider());
});
```

#### Manually wires up the services - Setup
```c#
//Initialization
var container = IocServicelet.Configure(config =>
{
    config.AddServices((service) =>
    {
       service.AddDependencies((dopt) => {
            dopt.Name = "DataContext"; /*Dependency factory name */ 
       });
    });
});

//Add dependencies at level - 1 
container.GetRootContainer()
         .Add<ICustomer, CustomerService>()
         .Add<IOrder, OrderService>()
	 .Add<AbstractSale, OnlineSale>("Online")   /*Access: var onlineSale = ServiceManager.GetService<AbstractSale>("Online");  //Get OnlineSale object */
	 .Add<AbstractSale, PointOfSale>("Pos");    /*Access: var posSale = ServiceManager.GetService<AbstractSale>("Pos");  //Get PointOfSale object */

//Check multiple implementations of interface -> https://github.com/rjinaga/IocServiceStack/wiki/Accessing%20service%20by%20Name

// Add dependencies at level - 2 
container.GetDependencyContainer("DataContext") /* get dependency factory by name */
         .Add<IDbContext>(()=>new AdventureDbContext());
```

### Services and Contracts Implementations

IocServiceStack can map the services to their interfaces automatically, in order to work this function, set the Contract attribute to the interface and Service attribute to the class that implements the interface. ServiceInit attribute must be set to the constructor to be invoked.

#### Business Service Layer

```c#
namespace BusinessService
{
    using IocServiceStack;
    using Models;
 
    [Contract]
    public interface ICustomerService
    {
        void AddCustomer(Customer customer);
    }
}

namespace BusinessService
{
    using IocServiceStack;
    using BusinessContractLibrary;
    using Models;
    using RepositoryService;

    [Service]
    public class CustomerService : ICustomerService
    {
        private ICustomerRepository _repository;

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
```

#### Repository Service Layer

```c#
namespace RepositoryService
{
    using IocServiceStack;
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
    using IocServiceStack;
    using DataContractLibrary;
    using Models;

    [Service]
    public class CustomerRepository : ICustomerRepository
    {

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

```

#### Data Service Layer

```c#
namespace DataService
{
    using IocServiceStack;

    [Contract]
    public interface IDbContext
    {
        
    }
}

namespace DataService
{
    using IocServiceStack;
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

### Accessing service from the Global IoC Container

```c#
var customerService = ServiceManager.GetService<ICustomer>();
```

### Dependency Injection
You can replace with another service which is already discovered by the IocServiceStack, or add a new service.

```c#
var rootContainer = container.GetRootContainer();

/*Dependency Injection*/
rootContainer.Replace<ICustomer, CustomerService2>();
container.GetDependencyContainer("Repository").Replace<ICustomerRepository, CustomerRepository2>();

/*Add new service*/
rootContainer.Add<IPayment, PaypalPayment>();

```

### Build and Use Isolated IoC Containers

```c#

/*setup container*/

var container = IocServicelet.CreateContainer(config=> { /* */  });

/* You can add services by calling container.Add<Interface>(()=> new Service()) */
/*set a new container to a static field */
Example.AppServiceManager.Container = container;

/* Create IoC service manager class(Eg: AppServiceManager) to access services in the container. */

namespace Example 
{
    using System;
    public static class AppServiceManager
    {
    	public static IocContainer Container;
        public static T GetService<T>() where T : class
        {
            var provider = Container.ServiceProvider;
            return provider.GetService<T>();
        }
        public static object GetService(Type contractType) 
        {
            var provider = Container.ServiceProvider;
            return provider.GetService(contractType);
        }
    }
}

```

### Decorators 
You can build and configure service decorators globally or interface level. Decorators of the interface will be executed when the instance is being created. It provides the flexibility of modifying the object or inject concrete objects at runtime.

### Register decorators with the global IoC container
> You can also register decorators with the isolated containers.

```c#

var configRef = IocServicelet.Configure(config =>
{
  /* ..... */
  config.Decorators.Add(new CustomInjectorDecorator());
}

```

#### Register and implement Interface level decorators

```c#

/*use decorator*/
namespace BusinessContractLibrary
{
    using IocServiceStack;
    using Models;

    [Contract, CustomerDecorator]
    public interface ICustomer
    {
        string AdditionalData { get; set; }
        void AddCustomer(Customer customer);
    }
}

/*Implement decorator*/
namespace BusinessContractLibrary
{
    using IocServiceStack;

    public class CustomerDecoratorAttribute : DecoratorAttribute
    {
        public override void OnBeforeInvoke(ServiceCallContext context)
        {
            base.OnBeforeInvoke(context);
        }
        public override void OnAfterInvoke(ServiceCallContext context)
        {
            //Set Default Value
	    var customer = context.ServiceInstance as ICustomer;	
            if (customer != null)
            {
                customer.AdditionalData = "Gold Customer";
            }
        }
    }
}

```
### Shared Dependencies
IocServiceStack finds the dependencies of root service from the immediate dependency sub container, if it could not be found then system searches in the shared sub container. Shared container provides flexibility to add common dependencies across the system.

```c#

container.GetSharedContainer()
	 .Add<ICustomerRepository, CustomerRepository>() /*this depends on the IDbContext*/
         .Add<IDbContext>(()=> new AdventureDbContext());

```

### Wiki
[https://github.com/rjinaga/IocServiceStack/wiki](https://github.com/rjinaga/IocServiceStack/wiki)



