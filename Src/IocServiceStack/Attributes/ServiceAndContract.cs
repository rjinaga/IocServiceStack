namespace IocServiceStack.Attributes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public class ServiceContractAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="contractTypes"></param>
        public ServiceContractAttribute(params Type[] contractTypes)
        {

        }
    }
}
