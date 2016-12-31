using System;

namespace NInterservice
{
    public interface IServiceConfig
    {
        IServiceConfig Services(Action<ServiceOptions> config);
        IServiceConfig SetServiceManager(IServiceManagerAssociate managerAssociate);
    }
}