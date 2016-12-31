namespace NInterservice
{
    using System;

    public sealed class ServiceConfig : IServiceConfig
    {
        private ServiceOptions _serviceOptions;
        private IServiceManagerAssociate _manager;

        internal ServiceOptions ServiceOptions => _serviceOptions;
        internal IServiceManagerAssociate ServiceManager => _manager;

        public ServiceConfig()
        {
            _serviceOptions = new ServiceOptions();
        }

        public IServiceConfig Services(Action<ServiceOptions> config)
        {
            config(_serviceOptions);
            return this;
        }
       
        public IServiceConfig SetServiceManager(IServiceManagerAssociate managerAssociate)
        {
            _manager = managerAssociate;
            return this;
        }

    }
}
