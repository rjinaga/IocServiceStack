namespace IocServiceStack
{
    public enum ContainerModel
    {
        /// <summary>
        /// All dependencies of a class will be searched hierarchically.
        /// </summary>
        MultiLevel,

        /// <summary>
        /// All dependencies of a class will be searched within the same container.
        /// </summary>
        Single,

    }
}
