namespace BusinessService
{
    using RepositoryService;
    using IocServiceStack;

    [Contract]
    public interface ISelfService
    {
        ICustomerRepository GetRepository();
    }
}
