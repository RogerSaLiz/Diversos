using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GeradorArquivo.DB;
using GeradorArquivo.Objects;

namespace GeradorArquivo.ObjectsDB
{
    public class SupplyFunctionTypesDB
    {
        public List<SupplyFunctionTypes> BuscaTodos()
        {
            var list = new List<SupplyFunctionTypes>();
            var executarDb = new ExecDB();
            executarDb.ReadFromBase("proc_SupplyFunctions_RetrieveAll", reader =>
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new SupplyFunctionTypes()
                        {
                            SupplyFunctionTypeID = Convert.ToInt32(reader["SupplyFunctionTypeID"]),
                            SupplyFunctionTypeName = Convert.ToString(reader["SupplyFunctionTypeName"]),
                            SupplyFunction = new SupplyFunctions(){SupplyFunctionID = Convert.ToInt32(reader["SupplyFunctionID"]),}
                        });
                    }
                }
            });

            return list;
        }

        public List<SupplyFunctionTypes> BuscaEspecifica(int supplyFunctionTypeID)
        {
            var list = new List<SupplyFunctionTypes>();
            var executarDb = new ExecDB();
            var parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("supplyFunctionTypeID", supplyFunctionTypeID));
            executarDb.ReadFromBase("proc_SupplyFunctionTypes_RetrieveAllEspecific", reader =>
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new SupplyFunctionTypes()
                        {
                            SupplyFunctionTypeID = Convert.ToInt32(reader["SupplyFunctionTypeID"]),
                            SupplyFunctionTypeName = Convert.ToString(reader["SupplyFunctionTypeName"]),
                            SupplyFunction = new SupplyFunctions() { SupplyFunctionID = Convert.ToInt32(reader["SupplyFunctionID"]), }
                        });
                    }
                }
            }, parametros.ToArray());

            return list;
        }
    }
}
