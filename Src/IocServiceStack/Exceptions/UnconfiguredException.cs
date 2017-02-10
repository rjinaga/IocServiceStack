
namespace IocServiceStack
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    /// <summary>
    /// Represents the exception thrown when no configuration is provided.
    /// </summary>
    public class UnconfiguredException : Exception
    {
        public UnconfiguredException(string message) : base(message)
        {

        }
    }
}
