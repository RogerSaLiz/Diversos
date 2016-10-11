using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GeradorArquivo.DB;
using GeradorArquivo.Helper;
using GeradorArquivo.Objects;

namespace GeradorArquivo.ObjectsDB
{
    public class EnterpriseDB
    {

        public void Adicionar(Enterprise objeto, Action completed)
        {
            var executarDb = new ExecDB();
            var parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("EnterpriseName", objeto.EnterpriseName));
            parametros.Add(new SqlParameter("EnterpriseKey", objeto.EnterpriseKey));
            parametros.Add(new SqlParameter("Observation", objeto.Observation));
            executarDb.ExecuteCommandScalar("proc_Enterprise_Insert", completed.Invoke, parametros.ToArray());
        }

        public List<Enterprise> BuscaTodos()
        {
            var list = new List<Enterprise>();
            var executarDb = new ExecDB();
            executarDb.ReadFromBase("proc_Enterprise_RetrieveAll", reader =>
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new Enterprise()
                        {
                            EnterpriseID = reader["EnterpriseID"].ToIntReader(),
                            EnterpriseName = reader["EnterpriseName"].ToStringReader(),
                            EnterpriseKey = reader["EnterpriseKey"].ToStringReader(),
                            Observation = reader["Observation"].ToStringReader()
                        });
                    }
                }
            });

            return list;
        }

        public void Editar(Enterprise objeto, Action completed)
        {
            var executarDb = new ExecDB();
            var parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("EnterpriseID", objeto.EnterpriseID));
            parametros.Add(new SqlParameter("EnterpriseName", objeto.EnterpriseName));
            parametros.Add(new SqlParameter("EnterpriseKey", objeto.EnterpriseKey));
            parametros.Add(new SqlParameter("Observation", objeto.Observation));
            executarDb.ExecuteCommandScalar("proc_Enterprise_Update", completed.Invoke, parametros.ToArray());
        }
    }
}
