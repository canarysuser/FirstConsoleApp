using MEFInterfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FirstConsoleApp
{
    internal class MEFClient
    {
        [Import(typeof(IDataRetriever))]
        public IDataRetriever DataRetriever;

        private CompositionContainer _container; 
        public MEFClient()
        {
            var catObj = new AggregateCatalog(); 
            catObj.Catalogs.Add(new AssemblyCatalog(typeof(IDataRetriever).Assembly));
            catObj.Catalogs.Add(new DirectoryCatalog(@"../../../Plugins")); 
            _container = new CompositionContainer(catObj);
            try
            {
                _container.ComposeParts(this);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        internal static void Test()
        {
            MEFClient client = new MEFClient(); 
            string result = client.DataRetriever.GetData(OperationTypeEnum.Account);
            Console.WriteLine("Account Result: {0}", result);
            result = client.DataRetriever.GetData(OperationTypeEnum.Product);
            Console.WriteLine("Product Result: {0}", result);
        }
    }
}
