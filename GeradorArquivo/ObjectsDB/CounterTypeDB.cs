using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GeradorArquivo.DB;
using GeradorArquivo.Helper;
using GeradorArquivo.Objects;

namespace GeradorArquivo.ObjectsDB
{
    public class CounterTypeDB
    {

        public void Adicionar(CounterType objeto, List<int> listPrintersIds, Action completed)
        {
            var executarDb = new ExecDB();
            var parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("CounterTypeName", objeto.CounterTypeName));
            parametros.Add(new SqlParameter("Observation", objeto.Observation));
            var counterID = executarDb.ExecuteCommandScalar("proc_CounterTypes_Insert", ()=>{}, parametros.ToArray());
            InserirCounterPrinters(counterID, listPrintersIds, () =>
            {
                completed.Invoke();
            });


        }

        public void InserirCounterPrinters(int counterID, List<int> listPrintersIds, Action completed)
        {
            if (listPrintersIds.Count > 0)
            {
                foreach (var id in listPrintersIds)
                {
                    var executarDb = new ExecDB();
                    var parametros = new List<SqlParameter>();
                    parametros.Add(new SqlParameter("CounterTypeID", counterID));
                    parametros.Add(new SqlParameter("PrinterModelID", id));
                    executarDb.ExecuteCommandScalar("[proc_CounterPrinter_Insert]", () => { }, parametros.ToArray());
                }
                completed.Invoke();
            }
            else
                completed.Invoke();



        }

        public void RemoverCounterPrinters(int counterID, List<int> listPrintersIds, Action completed)
        {
            if (listPrintersIds.Count > 0)
            {
                foreach (var id in listPrintersIds)
                {
                    var executarDb = new ExecDB();
                    var parametros = new List<SqlParameter>();
                    parametros.Add(new SqlParameter("CounterTypeID", counterID));
                    parametros.Add(new SqlParameter("PrinterModelID", id));
                    executarDb.ExecuteCommandScalar("[proc_CounterPrinter_Remove]", () => { },
                        parametros.ToArray());
                }
                completed.Invoke();
            }
            else

                completed.Invoke();
        }

        public List<CounterType> BuscaTodos()
        {
            var list = new List<CounterType>();
            var executarDb = new ExecDB();
            executarDb.ReadFromBase("proc_CounterTypes_RetrieveAll", reader =>
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {

                        list.Add(new CounterType()
                        {
                            CounterTypeID = reader["CounterTypeID"].ToIntReader(),
                            CounterTypeName = reader["CounterTypeName"].ToStringReader(),
                            Observation = reader["Observation"].ToStringReader(),
                        });
                    }
                }
            });

            return list;
        }

        public void Editar(CounterType objeto, List<int> idsPrintersAdd, List<int> idsPrintersRemove, Action completed)
        {
            var executarDb = new ExecDB();
            var parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("CounterTypeID", objeto.CounterTypeID));
            parametros.Add(new SqlParameter("CounterTypeName", objeto.CounterTypeName));
            parametros.Add(new SqlParameter("Observation", objeto.Observation));
            executarDb.ExecuteCommandScalar("proc_CounterTypes_Update", ()=>{}, parametros.ToArray());
            InserirCounterPrinters(objeto.CounterTypeID, idsPrintersAdd, () =>
            {
                RemoverCounterPrinters(objeto.CounterTypeID, idsPrintersRemove, () =>
                {
                   completed.Invoke();
                });
            });



        }

        public List<PrinterSupplyModelCounter> BuscaTodasImpressorasDoContador(int counterID)
        {
            var list = new List<PrinterSupplyModelCounter>();
            var executarDb = new ExecDB();
            var parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("CounterTypeID", counterID));
            executarDb.ReadFromBase("proc_CounterPrinter_RetrieveAll", reader =>
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new PrinterSupplyModelCounter()
                        {
                            PrinteModelID = Convert.ToInt32(reader["PrinterModelID"]),
                            ModelName = Convert.ToString(reader["ModelName"]),
                            BrandName = Convert.ToString(reader["BrandName"]),
                        });
                    }
                }
            }, parametros.ToArray());
            return list;
        }


       
    }
}
