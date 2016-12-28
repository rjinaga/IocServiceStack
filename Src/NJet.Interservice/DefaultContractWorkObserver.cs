namespace NJet.Interservice
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class DefaultContractWorkObserver : IContractObserver
    {
        Action<Type> _updateAction;
        public void OnUpdate(Action<Type> updateAction)
        {
            _updateAction = updateAction;
        }

        public void Update(Type serviceContractType)
        {
            _updateAction(serviceContractType);
        }
    }
}
