namespace BusinessService
{
    using BusinessContractLibrary;
    using IocServiceStack;

    [Service(Name = "Direct")]
    public class DirectSale : AbastractSale
    {
        public override string ProcessOrder()
        {
            return "Direct";
        }
    }
}
