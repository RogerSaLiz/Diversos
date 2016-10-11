using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GeradorArquivo.DB;
using GeradorArquivo.Objects;

namespace GeradorArquivo.ObjectsDB
{
    public class BrandDB
    {
        public void Adicionar(Brand objeto, Action completed)
        {
            var executarDb = new ExecDB();
            var parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("BrandName", objeto.BrandName));
            executarDb.ExecuteCommandScalar("proc_Brands_Insert", completed.Invoke, parametros.ToArray());
        }

        public List<Brand> BuscaTodos()
        {
            var list = new List<Brand>();
            var executarDb = new ExecDB();
            executarDb.ReadFromBase("proc_Brands_RetrieveAll", reader =>
            {
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        list.Add(new Brand()
                        {
                            BrandID = reader.GetInt32(0),
                            BrandName = reader.GetString(1)
                        });
                    }
                }
            });

            return list;
        }

        public void Editar(Brand objeto, Action completed)
        {
            var executarDb = new ExecDB();
            var parametros = new List<SqlParameter>();
            parametros.Add(new SqlParameter("BrandID", objeto.BrandID));
            parametros.Add(new SqlParameter("BrandName", objeto.BrandName));
            executarDb.ExecuteCommandScalar("proc_Brands_Update", completed.Invoke, parametros.ToArray());
        }
    }
}
