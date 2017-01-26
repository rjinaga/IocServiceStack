namespace BusinessService
{
    using BusinessContractLibrary;
    using IocServiceStack;

    [Service(Name = "Direct")]
    public class DirectSale : AbstractSale
    {
        public override string ProcessOrder()
        {
            return "Direct";
        }
    }
}
