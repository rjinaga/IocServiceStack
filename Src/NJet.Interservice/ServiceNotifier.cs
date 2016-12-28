namespace NJet.Interservice
{
    using System;

    public class ServiceNotifier : IServiceNotifier
    {
        public event ServiceUpdateHandler ServiceUpdateNofication;
        public void SendUpdate(Type type)
        {
            ServiceUpdateNofication?.Invoke(new ServiceEventArgs(type));
        }
    }
}
