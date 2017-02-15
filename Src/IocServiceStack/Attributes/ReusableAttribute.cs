namespace IocServiceStack
{
    using System;

    /// <summary>
    /// ReusableAttribute makes the service as singleton in the container. So that same 
    /// instance will be used to serve that exists in the container.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple =false)]
    public class ReusableAttribute : Attribute
    {

    }
}
