using MEFInterfaces;
using System.ComponentModel.Composition;

namespace AccountOperationLibrary
{
    [Export(typeof(IOperation))]
    [ExportMetadata("OperationType", OperationTypeEnum.Account)]
    public class Accounts : IOperation
    {
        public string GetData()
        {
            return "Accounts Operation goes here.";
        }
    }
}
