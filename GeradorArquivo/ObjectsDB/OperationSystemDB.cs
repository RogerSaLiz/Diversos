using System.Collections.Generic;
using GeradorArquivo.DB;
using GeradorArquivo.Helper;
using GeradorArquivo.Objects;

namespace GeradorArquivo.ObjectsDB
{
    public class OperationSystemDB
    {

        public List<OperationSystem> BuscaTodos()
        {
            var list = new List<OperationSystem>();
            var executarDb = new ExecDB();
            executarDb.ReadFromBase("proc_OperationSystems_RetrieveAll", reader =>
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new OperationSystem()
                        {
                            OperationSystemID = reader["OperationSystemID"].ToIntReader(),
                            OperationSystemName = reader["OperationSystemName"].ToStringReader(),
                        });
                    }
                }
            });

            return list;
        }
    }
}
