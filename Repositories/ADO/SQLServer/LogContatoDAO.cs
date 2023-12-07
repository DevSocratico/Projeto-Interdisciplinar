using LocFarma.Models;
using Microsoft.Data.SqlClient;

namespace LocFarma.Repositories.ADO.SQLServer
{
    public class LogContatoDAO
    {
        #region [ Conexão ]

        private readonly string connectionString;

        public LogContatoDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        #endregion [ Conexão ]

        #region [ Log Contato ]

        public List<LogContato> GetLogContatos()
        {
            List<LogContato> logsContato = new List<LogContato>();
            
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT id, nome, email, mensagem, data_envio FROM LogContato;";

                    SqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        LogContato logContato = new LogContato();

                        logContato.Id = (int)dr["id"];
                        logContato.Nome = (string)dr["nome"];
                        logContato.Email = (string)dr["email"];
                        logContato.Mensagem = (string)dr["mensagem"];
                        logContato.DataEnvio = (DateTime)dr["data_envio"];

                        logsContato.Add(logContato);
                    }
                }
            }

            return logsContato;
        }

        public LogContato GetLogContato(int id)
        {
            LogContato logContato = new LogContato();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT id, nome, email, mensagem, data_envio FROM LogContato WHERE id = @id;";
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = id;

                    SqlDataReader dr = command.ExecuteReader();

                    if (dr.Read())
                    {
                        logContato.Id = (int)dr["id"];
                        logContato.Nome = (string)dr["nome"];
                        logContato.Email = (string)dr["email"];
                        logContato.Mensagem = (string)dr["mensagem"];
                        logContato.DataEnvio = (DateTime)dr["data_envio"];
                    }
                }
            }

            return logContato;
        }

        public void SalvarLogContato(LogContato logContato)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO LogContato (nome, email, mensagem, data_envio) VALUES (@nome, @email, @mensagem, @data_envio); SELECT CONVERT(INT, @@identity) AS id;";

                    command.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = logContato.Nome;
                    command.Parameters.Add(new SqlParameter("@email", System.Data.SqlDbType.VarChar)).Value = logContato.Email;
                    command.Parameters.Add(new SqlParameter("@mensagem", System.Data.SqlDbType.VarChar)).Value = logContato.Mensagem;
                    command.Parameters.Add(new SqlParameter("@data_envio", System.Data.SqlDbType.DateTime2)).Value = logContato.DataEnvio;

                    logContato.Id = (int)command.ExecuteScalar();
                }
            }
        }

        #endregion [ Log Contato ]
    }
}
