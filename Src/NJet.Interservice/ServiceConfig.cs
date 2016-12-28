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

        public IDependentServiceConfig AddDependencies(Action<ServiceDependentOptions> config)
        {
            config(_serviceDependentOptions);
            return this;
        }
        /// <summary>
        /// EnableStrictMode applies the one contract with one service policy. This means if more than one service is implemented single contract interface
        /// then it will throw an exception. It ensures a contract interface is implemented by a single service. By default it's not enabled, mapping table wll have the last service in the list of inteface implementaions services.
        /// </summary>
        public void EnableStrictMode()
        {
            _serviceOptions.StrictMode = true;
        }
    }
}
