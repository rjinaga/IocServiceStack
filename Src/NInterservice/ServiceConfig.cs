namespace NInterservice
{
    using System;

    public sealed class ServiceConfig : IServiceConfig
    {
        private ServiceOptions _serviceOptions;
        private IServiceProvider _serviceProvider;

        internal ServiceOptions ServiceOptions => _serviceOptions;
        internal IServiceProvider ServiceProvider => _serviceProvider;

        public ServiceConfig()
        {
            _serviceOptions = new ServiceOptions();
        }

        public IServiceConfig Services(Action<ServiceOptions> config)
        {
            config(_serviceOptions);
            return this;
        }
       
        public IServiceConfig RegisterServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            return this;
        }

    }
}
