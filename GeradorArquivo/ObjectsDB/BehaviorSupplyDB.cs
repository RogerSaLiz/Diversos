using System.Collections.Generic;
using GeradorArquivo.DB;
using GeradorArquivo.Helper;
using GeradorArquivo.Objects;

namespace GeradorArquivo.ObjectsDB
{
    public class BehaviorSupplyDB
    {
        public List<BehaviorSupply> BuscaTodos()
        {
            var list = new List<BehaviorSupply>();
            var executarDb = new ExecDB();
            executarDb.ReadFromBase("proc_BehaviorSupply_RetrieveAll", reader =>
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new BehaviorSupply()
                        {
                            BehaviorSupplyID = reader["BehaviorSupplyID"].ToIntReader(),
                            Description = reader["Description"].ToStringReader()
                        });
                    }
                }
            });

            return list;
        }
    }
}
