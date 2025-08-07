namespace MEFInterfaces
{
    public enum OperationTypeEnum {  None, Account, Product}; 

    public interface IDataRetriever
    {
        string GetData(OperationTypeEnum type);
    }
    public interface IOperation
    {
        string GetData(); 
    }
    public interface IOperationMetadata
    {
        OperationTypeEnum OperationType { get; }    
    }
}
