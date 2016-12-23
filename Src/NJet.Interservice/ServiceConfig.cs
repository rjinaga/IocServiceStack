namespace NJet.Interservice
{
    using System;

    public sealed class ServiceConfig : IDependentServiceConfig
    {
        private ServiceOptions _serviceOptions;
        private ServiceDependentOptions _serviceDependentOptions;

        internal ServiceOptions ServiceOptions => _serviceOptions;
        internal ServiceDependentOptions ServiceDependentOptions => _serviceDependentOptions;

        public ServiceConfig()
        {
            _serviceOptions = new ServiceOptions();
            _serviceDependentOptions = new ServiceDependentOptions();
        }

        public IDependentServiceConfig AddServices(Action<ServiceOptions> config)
        {
            config(_serviceOptions);
            return this;
        }

        public IDependentServiceConfig AddDependentServices(Action<ServiceDependentOptions> config)
        {
            config(_serviceDependentOptions);
            return this;
        }
    }
}
