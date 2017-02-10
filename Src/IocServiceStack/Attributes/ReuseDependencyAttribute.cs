namespace IocServiceStack
{
    using System;

    [AttributeUsage(AttributeTargets.Constructor, AllowMultiple = false)]
    public class ReuseDependencyAttribute : Attribute
    {
        public readonly Type[] ReuseTypes;
        public ReuseDependencyAttribute(params Type[] reuseTypes)
        {
            ReuseTypes = reuseTypes;
        }
    }

}
