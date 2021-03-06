﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GeradorArquivo.DB;
using GeradorArquivo.Helper;
using GeradorArquivo.Objects;

namespace GeradorArquivo.ObjectsDB
{
    public class PrinterModelDB
    {
        public void Adicionar(PrinterModel objeto, List<PrinterSupplyModelCounter> suppliesNews, List<PrinterSupplyModelCounter> counters, Action completed)
        {
            var executarDb = new ExecDB();
            var parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("ModelName", objeto.ModelName));
            parametros.Add(new SqlParameter("BrandID", objeto.Brand.BrandID));
            parametros.Add(new SqlParameter("NameXML", objeto.NameXML));
            parametros.Add(new SqlParameter("Observation", objeto.Observation));
            parametros.Add(new SqlParameter("IsColor", objeto.IsColor));
            parametros.Add(new SqlParameter("IsCopier", objeto.IsCopier));
            parametros.Add(new SqlParameter("IsDuplex", objeto.IsDuplex));
            parametros.Add(new SqlParameter("IsLargeMedia", objeto.IsLargeMedia));
            parametros.Add(new SqlParameter("IsNetwork", objeto.IsNetwork));
            parametros.Add(new SqlParameter("IsScan", objeto.IsScan));
            parametros.Add(new SqlParameter("Location", objeto.Location));
            parametros.Add(new SqlParameter("AddressMac", objeto.AddressMac));
            parametros.Add(new SqlParameter("AddressName", objeto.AddressName));
            parametros.Add(new SqlParameter("AddressPort", objeto.AddressPort));
            var printerModelID = executarDb.ExecuteCommandScalar("proc_PrinterModels_Insert", completed.Invoke, parametros.ToArray());
            InserirPrinterSupply(printerModelID, suppliesNews, () =>
            {
                InserirCounter(printerModelID, counters, completed.Invoke);
            });

        }

        public void InserirCounter(int printerID, List<PrinterSupplyModelCounter> counters, Action completed)
        {
            if (counters.Count != 0)
            {
                foreach (var item in counters)
                {
                    var executarDb = new ExecDB();
                    var parametros = new List<SqlParameter>();
                    parametros.Add(new SqlParameter("CounterTypeID", item.CounterTypeID));
                    parametros.Add(new SqlParameter("PrinterModelID", printerID));
                    parametros.Add(new SqlParameter("Mono", item.Mono));
                    parametros.Add(new SqlParameter("Color", item.Color));
                    parametros.Add(new SqlParameter("Total", item.Total));
                    executarDb.ExecuteCommandScalar("[proc_CounterPrinter_Insert]", () => { },
                        parametros.ToArray());
                }
                completed.Invoke();
            }
            else
                completed.Invoke();
        }

        public void UpdateCounter(int printerID, List<PrinterSupplyModelCounter> counters, Action completed)
        {
            if (counters.Count != 0)
            {
                foreach (var item in counters)
                {
                    var executarDb = new ExecDB();
                    var parametros = new List<SqlParameter>();
                    parametros.Add(new SqlParameter("CounterTypeID", item.CounterTypeID));
                    parametros.Add(new SqlParameter("PrinterModelID", printerID));
                    parametros.Add(new SqlParameter("Mono", item.Mono));
                    parametros.Add(new SqlParameter("Color", item.Color));
                    parametros.Add(new SqlParameter("Total", item.Total));
                    executarDb.ExecuteCommandScalar("[proc_CounterPrinter_Update]", () => { },
                        parametros.ToArray());
                }
                completed.Invoke();
            }
            else
                completed.Invoke();
        }

        public void RemoverCounter(int printerID, List<int> idsCounters, Action completed)
        {
            if (idsCounters.Count != 0)
            {
                foreach (var id in idsCounters)
                {
                    var executarDb = new ExecDB();
                    var parametros = new List<SqlParameter>();
                    parametros.Add(new SqlParameter("CounterTypeID", id));
                    parametros.Add(new SqlParameter("PrinterModelID", printerID));
                    executarDb.ExecuteCommandScalar("[proc_CounterPrinter_Remove]", () => { },
                        parametros.ToArray());
                }
                completed.Invoke();
            }
            else
                completed.Invoke();
        }

        public void InserirPrinterSupply(int printerID, List<PrinterSupplyModelCounter> idsSupplies, Action completed)
        {
            if (idsSupplies.Count != 0)
            {
                foreach (var item in idsSupplies)
                {
                    var executarDb = new ExecDB();
                    var parametros = new List<SqlParameter>();
                    parametros.Add(new SqlParameter("SupplyModelID", item.SupplyModelId));
                    parametros.Add(new SqlParameter("PrinterModelID", printerID));
                    parametros.Add(new SqlParameter("BehaviorSupplyID", item.BehSupply.BehaviorSupplyID));
                    executarDb.ExecuteCommandScalar("[proc_PrinterSupplyModel_Insert]", () => { },
                        parametros.ToArray());
                }
                completed.Invoke();
            }
            else
                completed.Invoke();
        }

        public void RemoverPrinterSupply(int printerID, List<int> idsSupplies, Action completed)
        {
            if (idsSupplies.Count != 0)
            {
                foreach (var idsupply in idsSupplies)
                {
                    var executarDb = new ExecDB();
                    var parametros = new List<SqlParameter>();
                    parametros.Add(new SqlParameter("SupplyModelID", idsupply));
                    parametros.Add(new SqlParameter("PrinterModelID", printerID));
                    executarDb.ExecuteCommandScalar("[proc_PrinterSupplyModel_Remove]", () => { },
                        parametros.ToArray());
                }
                completed.Invoke();
            }
            else
                completed.Invoke();
        }

        public void UpdatePrinterSupply(int printerID, List<int> idsSupplies, Action completed)
        {
            if (idsSupplies.Count != 0)
            {
                foreach (var idsupply in idsSupplies)
                {
                    var executarDb = new ExecDB();
                    var parametros = new List<SqlParameter>();
                    parametros.Add(new SqlParameter("SupplyModelID", idsupply));
                    parametros.Add(new SqlParameter("PrinterModelID", printerID));
                    executarDb.ExecuteCommandScalar("[proc_PrinterSupplyModel_Remove]", () => { },
                        parametros.ToArray());
                }
                completed.Invoke();
            }
            else
                completed.Invoke();
        }

        public List<PrinterModel> BuscaTodos(bool loadCounters = false)
        {
            var list = new List<PrinterModel>();
            var executarDb = new ExecDB();
            executarDb.ReadFromBase("proc_PrinterModels_RetrieveAll", reader =>
            {
                if (reader.HasRows)
                {
                    var index = -1;
                    while (reader.Read())
                    {
                        index++;


                        var printerModel = new PrinterModel();

                        printerModel.PrinteModelID = reader["PrinterModelID"].ToIntReader();
                        printerModel.ModelName = reader["ModelName"].ToStringReader();
                        printerModel.Brand = new Brand() { BrandID = reader.GetInt32(2), BrandName = reader.GetString(3) };
                        printerModel.NameXML = reader["NameXML"].ToStringReader();
                        printerModel.Observation = reader["Observation"].ToStringReader();
                        printerModel.IsColor = reader["IsColor"].ToBoolReader();
                        printerModel.IsDuplex = reader["IsDuplex"].ToBoolReader();
                        printerModel.IsCopier = reader["IsCopier"].ToBoolReader();
                        printerModel.IsLargeMedia = reader["IsLargeMedia"].ToBoolReader();
                        printerModel.IsNetwork = reader["IsNetwork"].ToBoolReader();
                        printerModel.IsScan = reader["IsScan"].ToBoolReader();
                        printerModel.Location = reader["Location"].ToStringReader();
                        printerModel.SerialNumber = reader["SerialNumber"].ToStringReader();
                        printerModel.AddressMac = reader["AddressMac"].ToStringReader();
                        printerModel.AddressName = reader["AddressName"].ToStringReader();
                        printerModel.AddressPort = reader["AddressPort"].ToStringReader();
                        printerModel.Index = index;
                        if (loadCounters)
                          printerModel.ListCounters.AddRange(BuscaTodosContadores(printerModel.PrinteModelID));
                        list.Add(printerModel);
                        

                    }
                }
            });

            return list;
        }

        public void Editar(PrinterModel objeto, List<PrinterSupplyModelCounter> suppliesNews, List<int> idsSuppliesRemove, List<PrinterSupplyModelCounter> counters, List<int> idsCountersRemove,
            List<PrinterSupplyModelCounter> countersChange, Action completed)
        {
            var executarDb = new ExecDB();
            var parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("PrinterModelID", objeto.PrinteModelID));
            parametros.Add(new SqlParameter("ModelName", objeto.ModelName));
            parametros.Add(new SqlParameter("BrandID", objeto.Brand.BrandID));
            parametros.Add(new SqlParameter("NameXML", objeto.NameXML));
            parametros.Add(new SqlParameter("Observation", objeto.Observation));
            parametros.Add(new SqlParameter("IsColor", objeto.IsColor));
            parametros.Add(new SqlParameter("IsCopier", objeto.IsCopier));
            parametros.Add(new SqlParameter("IsDuplex", objeto.IsDuplex));
            parametros.Add(new SqlParameter("IsLargeMedia", objeto.IsLargeMedia));
            parametros.Add(new SqlParameter("IsNetwork", objeto.IsNetwork));
            parametros.Add(new SqlParameter("IsScan", objeto.IsScan));
            parametros.Add(new SqlParameter("Location", objeto.Location));
            parametros.Add(new SqlParameter("AddressMac", objeto.AddressMac));
            parametros.Add(new SqlParameter("AddressName", objeto.AddressName));
            parametros.Add(new SqlParameter("AddressPort", objeto.AddressPort));
            parametros.Add(new SqlParameter("SerialNumber", objeto.SerialNumber));
            executarDb.ExecuteCommandScalar("proc_PrinterModels_Update", completed.Invoke, parametros.ToArray());
            InserirPrinterSupply(objeto.PrinteModelID, suppliesNews, () =>
            {
                RemoverPrinterSupply(objeto.PrinteModelID, idsSuppliesRemove, () =>
                {
                    InserirCounter(objeto.PrinteModelID, counters, () =>
                    {
                        RemoverCounter(objeto.PrinteModelID, idsCountersRemove, () =>
                        {
                            UpdateCounter(objeto.PrinteModelID, countersChange, completed.Invoke);
                        });
                    });
                });
            });


        }

        public List<PrinterSupplyModelCounter> BuscaTodosContadores(int printerModelID)
        {
            var list = new List<PrinterSupplyModelCounter>();
            var executarDb = new ExecDB();
            var parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("PrinterModelID", printerModelID));
            executarDb.ReadFromBase("proc_CounterPrinter_RetrieveAll", reader =>
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new PrinterSupplyModelCounter()
                        {
                            CounterTypeID = Convert.ToInt32(reader["CounterTypeID"]),
                            CounterTypeName = Convert.ToString(reader["CounterTypeName"]),
                            Mono = reader["Mono"].ToIntReader(),
                            Color = reader["Color"].ToIntReader(),
                            Total = reader["Total"].ToIntReader(),
                        });
                    }
                }
            }, parametros.ToArray());
            return list;
        }
    }
}
