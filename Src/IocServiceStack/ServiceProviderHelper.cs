namespace IocServiceStack
{
    using System;

    internal class ServiceProviderHelper
    {
        public static IDependencyFactory GetDependencyFactory(IDependencyFactory factoryNode, string name)
        {
            while (factoryNode != null)
            {
                if (string.Equals(factoryNode.Name, name, StringComparison.OrdinalIgnoreCase))
                {
                    return factoryNode;
                }
                factoryNode = factoryNode.DependencyFactory;
            }
            return null;
        }
    }
}
