using System;

namespace NInterservice
{
    public interface IServiceConfig
    {
        IServiceConfig Services(Action<ServiceOptions> config);
        IServiceConfig RegisterServiceProvider(IServiceProvider serviceProvider);
    }
}