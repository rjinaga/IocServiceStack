namespace BusinessService
{
    using BusinessContractLibrary;
    using IocServiceStack;

    [Service(Name = "Online")]
    public class OnlineSale : AbastractSale
    {
        public override string ProcessOrder()
        {
            return "Online";
        }
    }
}
