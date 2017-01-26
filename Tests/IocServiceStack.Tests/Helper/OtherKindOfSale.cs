namespace IocServiceStack.Tests.Helper
{
    using BusinessContractLibrary;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class OtherKindOfSale : AbstractSale
    {
        public override string ProcessOrder()
        {
            return "OtherKind";
        }
    }

    public class MiscSale : AbstractSale
    {
        public override string ProcessOrder()
        {
            return "MiscSale";
        }
    }
}
