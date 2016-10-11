using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GeradorArquivo.DB;
using GeradorArquivo.Helper;
using GeradorArquivo.Objects;

namespace GeradorArquivo.ObjectsDB
{
    public class PrinterSupplyModelDB
    {

        public List<PrinterSupplyModelCounter> BuscaTodos(int printerModelId, int supplyModelId)
        {
            var list = new List<PrinterSupplyModelCounter>();
            var parametros = new List<SqlParameter>();
            if (printerModelId > 0)
                parametros.Add(new SqlParameter("PrinterModelID", printerModelId));
            if (supplyModelId > 0)
                parametros.Add(new SqlParameter("SupplyModelID", supplyModelId));
            var executarDb = new ExecDB();
            executarDb.ReadFromBase("proc_PrinterSupplyModel_RetrieveAll", reader =>
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new PrinterSupplyModelCounter()
                        {
                            PrinteModelID = Convert.ToInt32(reader["PrinterModelID"]),
                            ModelName = reader["ModelName"].ToStringReader(),
                            SupplyModelId = reader["SupplyModelId"].ToIntReader(),
                            PartNumber =reader["PartNumber"].ToStringReader(),
                            Capacity = reader["Capacity"].ToIntReader(),
                            Description = reader["Description"].ToStringReader(),
                            SupplyColorName = reader["SupplyColorName"].ToStringReader(),
                            BrandName = reader["BrandName"].ToStringReader(),
                            NameXML = reader["NameXML"].ToStringReader(),
                            BehSupply = new BehaviorSupply() { BehaviorSupplyID = reader["BehaviorSupplyID"].ToIntReader(), Description = reader["BDescription"].ToStringReader() }
                        });
                    }
                }
            }, parametros.ToArray());

            return list;
        }
    }
}
