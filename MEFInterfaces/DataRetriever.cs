using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MEFInterfaces
{
    [Export(typeof(IDataRetriever))]
    public class DataRetriever : IDataRetriever
    {
        [ImportMany(typeof(IOperation))]
        public IEnumerable<Lazy<IOperation, IOperationMetadata>> operations; 

        public string GetData(OperationTypeEnum type)
        {
            foreach (var item in operations)
            {
                if (item.Metadata.OperationType == type)
                {
                    return item.Value.GetData(); 
                }
            }
            if (type == OperationTypeEnum.None)
                return $"Operation Type {type} on DataRetriever class is executed.";
            return "could not find any matching parts.";
        }
    }
}
