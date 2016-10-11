using System;
using System.Collections.Generic;
using GeradorArquivo.DB;
using GeradorArquivo.Objects;

namespace GeradorArquivo.ObjectsDB
{
    public class SupplySubFunctionTypesDB
    {
        public List<SupplySubFunctionTypes> BuscaTodos()
        {
            var list = new List<SupplySubFunctionTypes>();
            var executarDb = new ExecDB();
            executarDb.ReadFromBase("proc_SupplySubFunctionTypes_RetrieveAll", reader =>
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new SupplySubFunctionTypes()
                        {
                            SupplySubFunctionTypeID = Convert.ToInt32(reader["SupplySubFunctionTypeID"]),
                            SupplySubFunctionTypeName = Convert.ToString(reader["SupplySubFunctionTypeName"]),
                            SupplyFunctionType = new SupplyFunctionTypes() { SupplyFunctionTypeID = Convert.ToInt32(reader["SupplyFunctionTypeID"]), }
                        });
                    }
                }
            });

            return list;
        }
    }
}
