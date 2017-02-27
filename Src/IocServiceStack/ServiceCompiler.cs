namespace IocServiceStack
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;

    internal sealed class ServiceCompiler
    {
        private ServiceNotifier _notifier;

        public ServiceCompiler(ServiceNotifier notifier)
        {
            if (notifier == null)
            {
                throw new ArgumentNullException(nameof(notifier));
            }

            _notifier = notifier;
        }

        public void Compile<T>(Type interfaceType, BaseServiceInfo serviceMeta, Func<Type, Type,ServiceRegister, ServiceState, Expression> createConstructorExpression) where T : class
        {
            if (serviceMeta.Activator == null)
            {
                lock (serviceMeta.SyncObject)
                {
                    if (serviceMeta.Activator == null)
                    {
                        var registrar = serviceMeta.InitNewRegister(interfaceType, _notifier);

                        Func<T> serviceCreator = serviceMeta.GetServiceInstanceCallback<T>();
                        if (serviceCreator == null)
                        {
                            var state = new ServiceState(); /*Root place for service state instance*/
                            Expression newExpression = createConstructorExpression(interfaceType, serviceMeta.ServiceType, registrar, state);

                            var blockExpression = BuildExpression(state, newExpression);

                            //Set Activator
                            serviceCreator = Expression.Lambda<Func<T>>(blockExpression).Compile();
                        }
                        serviceMeta.Activator = new ServiceActivator<T>(serviceCreator, serviceMeta.IsReusable);
                    }
                }
            }
        }

        private Expression BuildExpression(ServiceState state, Expression returnValue)
        {
            /*we do not need to build block if there are no parameters.
             * it means that there are no re-usable instances within constructor parameters.
             */
            if (!state.HasParameters())
            {
                return returnValue;
            }

            List<Expression> expressions = new List<Expression>(state.GetBinaryExpressions());
            expressions.Add(returnValue);

            BlockExpression blockExpr = Expression.Block(
              state.GetParameters(),
              expressions
            );

            return blockExpr;
        }
    }
}
