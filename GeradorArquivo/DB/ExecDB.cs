using System;
using System.Data;
using System.Data.SqlClient;

namespace GeradorArquivo.DB
{
    public class ExecDB
    {

        private const string ConexaoPrimaria =
            @"Data Source=172.31.251.82\sql2012;Initial Catalog=GeradorArquivo;User ID=sa;Password=321";
       
        public void ReadFromBase(string commandText, Action<SqlDataReader> onRead, params SqlParameter[] parameters)
        {

            using (var connection = new SqlConnection(ConexaoPrimaria))
            {
                var command = new SqlCommand(commandText);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddRange(parameters);
                command.Connection = connection;

                SqlDataReader reader = null;
                try
                {
                    connection.Open();
                    {
                        reader = command.ExecuteReader();
                    }
                    onRead(reader);
                    command.Connection.Close();
                }

                catch (SqlException ex)
                {


                }
            }
        }

        public int ExecuteCommand(string procedure,Action completed, params SqlParameter[] parameters)
        {

            using (var connection = new SqlConnection(ConexaoPrimaria))
            {
                var command = new SqlCommand(procedure) {CommandType = CommandType.StoredProcedure};
                command.Parameters.AddRange(parameters);
                command.Connection = connection;
                int result = 0;
                command.Connection.Open();
                {
                    try
                    {
                        command.ExecuteNonQuery();
                        result = 1;
                        completed.Invoke();
                    }
                    catch (SqlException ex)
                    {
                        result = 0;
                        completed.Invoke();
                    }
                }
                command.Connection.Close();
                return result;
            }
        }

        public int ExecuteCommandScalar(string procedure,Action completed,  params SqlParameter[] parameters)
        {
            using (var connection = new SqlConnection(ConexaoPrimaria))
            {
                var command = new SqlCommand(procedure) {CommandType = CommandType.StoredProcedure};
                command.Parameters.AddRange(parameters);
                command.Connection = connection;
                var result = 0;
                command.Connection.Open();
                {
                    try
                    {
                        int.TryParse(Convert.ToString(command.ExecuteScalar()), out result);
                        completed.Invoke();
                    }
                    catch (SqlException ex)
                    {
                        
                        completed.Invoke();
                    }
                }
                command.Connection.Close();
                return result;
            }
        }
    }
}
