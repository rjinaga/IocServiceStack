# IocServiceStack

[![Gitter](https://badges.gitter.im/IocServiceStack/Lobby.svg)](https://gitter.im/IocServiceStack/Lobby?utm_source=badge&utm_medium=badge&utm_campaign=pr-badge&utm_content=body_badge)

IocServiceStack is a open source .NET library for multi-services communication through multi-level dependency injection. This clearly separates the concerns of application layers. This makes the layers configurable, but not required to reference the dependent layer at design time. This library offers several ways to inject dependencies. 

### Features:
- Global IoC Container
- Isolated IoC Containers
- Automatically maps the services to their contracts
- Inject dependencies through decorators
- Multi level dependencies
- Replace or Add dependencies
- Highly extensible
- Strict mode enables that ServiceAttribute attribute cannot be set more than one implemention of same interface while automap the services.


### Supports
- .NET Core 1.0 (.NET Standard 1.6)
- .NET Framework 4.6


## [NuGet](https://www.nuget.org/packages/IocServiceStack/)
```
PM> Install-Package IocServiceStack -Pre
```
[![NuGet Pre Release](https://img.shields.io/badge/nuget-Pre%20Release-yellow.svg)](https://www.nuget.org/packages/IocServiceStack/)


## Usage

### IoC Service Stack Setup

```c#
var configRef = IocServiceProvider.Configure(config =>
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
  //.RegisterServiceProvider(new ProxyServiceProvider());
});
```

### Services and Contracts Implementations

> To automatically map the interface and its implementaion class, 
> set attribute ```[Contract]``` for the interface
> and attribute ```[Service]``` for the class that implements contract interface.


```c#
namespace BusinessContractLibrary
{
    using IocServiceStack;
    using Models;
 
    [Contract]
    public interface ICustomer
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
`ServiceManager` static class is a wrapper for global IoC container.

```c#
var customerService = ServiceManager.GetService<ICustomer>();

```

### Dependency Injection
You can replace with another service which is already discovered by the IocServiceStack, or add a new service.

```c#
var factoryService = configRef.GetFactoryService();

/*Dependency Injection*/
factoryService.Replace<ICustomer, CustomerService2>()
              .Subcontract
              .Replace<ICustomerRepository, CustomerRepository2>();

/*Add new service*/
factoryService.Add<IPayment, PaypalPayment>();

```

### Build and Use Isolated IoC Containers

```c#

/*setup container*/

var container = IocServiceProvider.CreateNewIocContainer(config=> { /* */  });

/* You can add services by calling container.Add<Interface>(()=> new Service()) */
/*set new container to a static field */

Example.AppServiceManager.Container = container;

/* Create IoC service manager class(Eg: AppServiceManager
) to access services in the container. */

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
you can build and configure service decorators globally or contract (interface) level. Decorators will be executed when instance is being created. You can modify the object or inject concrete objects at runtime using decorators.

### Register Decorators with the Global IoC Container
You can also register decorators with the isolated containers.

```c#

var configRef = IocServiceProvider.Configure(config =>
{
  /* ..... */
  config.Decorators.Add(new CustomInjectorDecorator());
}

```

#### Contract (Interface) level decorators

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
            if (context.ServiceInstance is ICustomer)
            {
                dynamic obj = context.ServiceInstance;
                obj.AdditionalData = "Gold Customer";
            }
        }
    }
}
```


### Relationship with the [IocServiceStack.Gateway](https://rjinaga.github.io/IocServiceStack.Gateway) and [IocServiceStack.Client](https://rjinaga.github.io/IocServiceStack.Client) Repositories

>  **IocServiceStack.Gateway** and **IocServiceStack.Client** libraries helps to make the logical layered application into physical layer application that builts using IocServiceStack.


## Web Application Architecture using IocServiceStack

[https://github.com/rjinaga/Web-App-Architecture-Using-IocServiceStack](https://github.com/rjinaga/Web-App-Architecture-Using-IocServiceStack)





