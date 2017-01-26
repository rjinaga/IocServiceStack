namespace BusinessService
{
    using BusinessContractLibrary;
    using IocServiceStack;

    [Service(Name = "Online")]
    public class OnlineSale : AbstractSale
    {
        public override string ProcessOrder()
        {
            return "Online";
        }
    }
}
