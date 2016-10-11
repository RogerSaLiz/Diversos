using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GeradorArquivo.DB;
using GeradorArquivo.Objects;

namespace GeradorArquivo.ObjectsDB
{
    public class SupplyColorsDB
    {
        public List<SupplyColors> BuscaTodos()
        {
            var list = new List<SupplyColors>();
            var executarDb = new ExecDB();
            executarDb.ReadFromBase("proc_SupplyColors_RetrieveAll", reader =>
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new SupplyColors()
                        {
                            SupplyColorID = Convert.ToInt32(reader["SupplyColorID"]),
                            SupplyColorName = Convert.ToString(reader["SupplyColorName"]),
                        });
                    }
                }
            });
            return list;
        }

        public List<SupplyColors> BuscaEspecifica(int supplyFunctionTypeID)
        {
            var list = new List<SupplyColors>();
            var executarDb = new ExecDB();
            var parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("SupplyFunctionTypeID", supplyFunctionTypeID));
            executarDb.ReadFromBase("[proc_SupplyColors_RetrieveAllEspecific]", reader =>
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new SupplyColors()
                        {
                            SupplyColorID = Convert.ToInt32(reader["SupplyColorID"]),
                            SupplySlotID = Convert.ToInt32(reader["SupplySlotID"]),
                            SupplyColorName = Convert.ToString(reader["SupplyColorName"]),
                        });
                    }
                }
            },parametros.ToArray());
            return list;
        }
    }
}
