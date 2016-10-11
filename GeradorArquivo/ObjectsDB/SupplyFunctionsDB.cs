using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GeradorArquivo.DB;
using GeradorArquivo.Objects;

namespace GeradorArquivo.ObjectsDB
{
    public class SupplyFunctionsDB
    {
        public List<SupplyFunctions> BuscaTodos()
        {
            var list = new List<SupplyFunctions>();
            var executarDb = new ExecDB();
            executarDb.ReadFromBase("proc_SupplyFunctions_RetrieveAll", reader =>
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new SupplyFunctions()
                        {
                            SupplyFunctionID = Convert.ToInt32(reader["SupplyFunctionID"]),
                            SupplyFunctionName = Convert.ToString(reader["SupplyFunctionName"]),
                        });
                    }
                }
            });
            return list;
        }

        public List<SupplyFunctions> BuscaEspecifica(int supplyFunctionID)
        {
            var list = new List<SupplyFunctions>();
            var executarDb = new ExecDB();
            var parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("supplyFunctionID", supplyFunctionID));
            executarDb.ReadFromBase("proc_SupplyFunctions_RetrieveAllEspecific", reader =>
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new SupplyFunctions()
                        {
                            SupplyFunctionID = Convert.ToInt32(reader["SupplyFunctionID"]),
                            SupplyFunctionName = Convert.ToString(reader["SupplyFunctionName"]),
                        });
                    }
                }
            }, parametros.ToArray());
            return list;
        }
    }
}
