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
    public class SupplyDB
    {
        public void Adicionar(SupplyModel objeto, List<int> idsPrinters, Action completed)
        {
            var executarDb = new ExecDB();
            var parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("PartNumber", objeto.PartNumber));
            parametros.Add(new SqlParameter("Capacity", objeto.Capacity));
            parametros.Add(new SqlParameter("Description", objeto.Description));
            parametros.Add(new SqlParameter("BrandID", objeto.Brand.BrandID));
            parametros.Add(new SqlParameter("SupplySubFunctionTypeID", objeto.SupplySubFunctionType.SupplySubFunctionTypeID));
            parametros.Add(new SqlParameter("SupplySlotID", objeto.SupplySlot.SupplySlotID));
            parametros.Add(new SqlParameter("BeharviorSupplyID", objeto.BehSupply.BehaviorSupplyID));
            var supplyID = executarDb.ExecuteCommandScalar("[proc_SupplyModel_Insert]", completed.Invoke, parametros.ToArray());
            if (idsPrinters.Count > 0)
                InserirPrinterSupply(supplyID, idsPrinters, completed.Invoke);
            else
                completed.Invoke();
        }

        public void InserirPrinterSupply(int supplyID, List<int> idsPrinters, Action completed)
        {
            foreach (var idsPrinter in idsPrinters)
            {
                var executarDb = new ExecDB();
                var parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("SupplyModelID", supplyID));
                parametros.Add(new SqlParameter("PrinterModelID", idsPrinter));
                executarDb.ExecuteCommandScalar("[proc_PrinterSupplyModel_Insert]", completed.Invoke, parametros.ToArray());
            }
            completed.Invoke();
        }

        public void RemoverPrinterSupply(int supplyID, List<int> idsPrinters, Action completed)
        {
            foreach (var idsPrinter in idsPrinters)
            {
                var executarDb = new ExecDB();
                var parametros = new List<SqlParameter>();
                parametros.Add(new SqlParameter("SupplyModelID", supplyID));
                parametros.Add(new SqlParameter("PrinterModelID", idsPrinter));
                executarDb.ExecuteCommandScalar("[proc_PrinterSupplyModel_Remove]", completed.Invoke, parametros.ToArray());
            }
            completed.Invoke();
        }


        public List<SupplyModel> BuscaTodos()
        {
            var list = new List<SupplyModel>();
            var executarDb = new ExecDB();
            executarDb.ReadFromBase("proc_SupplyModel_RetrieveAll", reader =>
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new SupplyModel()
                        {
                            SupplyModelId = Convert.ToInt32(reader["SupplyModelId"]),
                            PartNumber = Convert.ToString(reader["PartNumber"]),
                            Capacity = Convert.ToInt32(reader["Capacity"]),
                            Description = Convert.ToString(reader["Description"]),
                            Brand = new Brand() { BrandID = Convert.ToInt32(reader["BrandID"]), BrandName = Convert.ToString(reader["BrandName"]), },
                            SupplySubFunctionType = new SupplySubFunctionTypes()
                            {
                                SupplySubFunctionTypeID = Convert.ToInt32(reader["SupplySubFunctionTypeID"]),
                                SupplySubFunctionTypeName = Convert.ToString(reader["SupplySubFunctionTypeName"]),
                                SupplyFunctionType = new SupplyFunctionTypes()
                                {
                                    SupplyFunctionTypeID = Convert.ToInt32(reader["SupplyFunctionTypeID"]),
                                    SupplyFunctionTypeName = Convert.ToString(reader["SupplyFunctionTypeName"]),
                                    SupplyFunction = new SupplyFunctions()
                                    {
                                        SupplyFunctionID = Convert.ToInt32(reader["SupplyFunctionID"]),
                                        SupplyFunctionName = Convert.ToString(reader["SupplyFunctionName"]),
                                    }
                                }
                            },

                            BehSupply = new BehaviorSupply()
                            {
                                BehaviorSupplyID = reader["BehaviorSupplyID"].ToIntReader(),
                                Description = reader["BDescription"].ToString()
                            },

                            SupplySlot = new SupplySlots()
                            {
                                SupplySlotID = Convert.ToInt32(reader["SupplySlotID"]),
                                SupplyColor = new SupplyColors()
                                {
                                    SupplyColorID = Convert.ToInt32(reader["SupplyColorID"]),
                                    SupplyColorName = Convert.ToString(reader["SupplyColorName"]),
                                },
                                SupplyFunctionType = new SupplyFunctionTypes()
                                {
                                    SupplyFunctionTypeID = Convert.ToInt32(reader["SupplyFunctionTypeID"]),
                                    SupplyFunctionTypeName = Convert.ToString(reader["SupplyFunctionTypeName"]),
                                }
                            }
                        });
                    }
                }
            });

            return list;
        }

        public void Editar(SupplyModel objeto, List<int> idsPrintersAdd, List<int> idsPrintersRemove, Action completed)
        {
            var executarDb = new ExecDB();
            var parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("SupplyModelId", objeto.SupplyModelId));
            parametros.Add(new SqlParameter("PartNumber", objeto.PartNumber));
            parametros.Add(new SqlParameter("Capacity", objeto.Capacity));
            parametros.Add(new SqlParameter("Description", objeto.Description));
            parametros.Add(new SqlParameter("BrandID", objeto.Brand.BrandID));
            parametros.Add(new SqlParameter("SupplySubFunctionTypeID", objeto.SupplySubFunctionType.SupplySubFunctionTypeID));
            parametros.Add(new SqlParameter("SupplySlotID", objeto.SupplySlot.SupplySlotID));
            parametros.Add(new SqlParameter("BeharviorSupplyID", objeto.BehSupply.BehaviorSupplyID));
            executarDb.ExecuteCommandScalar("[proc_SupplyModel_Update]", completed.Invoke, parametros.ToArray());
            if (idsPrintersAdd.Count > 0 && idsPrintersRemove.Count == 0)//Adicionar
                InserirPrinterSupply(objeto.SupplyModelId, idsPrintersAdd, completed.Invoke);
            else if (idsPrintersAdd.Count == 0 && idsPrintersRemove.Count > 0)//Remover
                RemoverPrinterSupply(objeto.SupplyModelId, idsPrintersRemove, completed.Invoke);
            else if (idsPrintersAdd.Count > 0 && idsPrintersRemove.Count > 0)//Adicionar e Remover
                RemoverPrinterSupply(objeto.SupplyModelId, idsPrintersRemove,
                    () => InserirPrinterSupply(objeto.SupplyModelId, idsPrintersAdd, completed.Invoke));
            else
                completed.Invoke();


        }
    }
}
