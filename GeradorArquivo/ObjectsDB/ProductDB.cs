using System.Collections.Generic;
using GeradorArquivo.DB;
using GeradorArquivo.Helper;
using GeradorArquivo.Objects;

namespace GeradorArquivo.ObjectsDB
{
    public class ProductDB
    {
        public List<Product> BuscaTodos()
        {
            var list = new List<Product>();
            var executarDb = new ExecDB();
            executarDb.ReadFromBase("proc_Product_RetrieveAll", reader =>
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new Product()
                        {
                            ProductID = reader["ProductID"].ToIntReader(),
                            ProductName = reader["ProductName"].ToStringReader()
                        });
                    }
                }
            });

            return list;
        }
    }
}
