using MEFInterfaces;
using System.ComponentModel.Composition;

namespace ProductOperationLibrary
{
    [Export(typeof(IOperation))]
    [ExportMetadata("OperationType", OperationTypeEnum.Product)]
    public class Products : IOperation
    {
        public string GetData()
        {
            return "Product Operation goes here.";
        }
    }
}
