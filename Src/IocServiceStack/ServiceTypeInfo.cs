namespace IocServiceStack
{
    using System;

    /// <summary>
    /// Represents name and type of the service
    /// </summary>
    public class ServiceTypeInfo
    {
        /// <summary>
        /// Gets or sets the name of service
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of service
        /// </summary>
        public Type Type { get; set; }
    }
}
