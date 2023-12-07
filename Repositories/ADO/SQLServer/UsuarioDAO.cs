using LocFarma.Models;
using Microsoft.Data.SqlClient;
using LocFarma.Configurations;

namespace LocFarma.Repositories.ADO.SQLServer
{
    public class UsuarioDAO
    {
        #region [ Conexão ]

        private readonly string connectionString;
        private const string fotoPadraoUsuario = "perfil.svg";

        public UsuarioDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        #endregion [ Conexão ]

        #region [ Usuário ]

        public List<Usuario> GetUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT U.id, nome, email, senha, foto, ativo, id_tipo_usuario, tipo FROM Usuario AS U INNER JOIN TipoUsuario AS TP ON U.id_tipo_usuario = TP.id;";

                    SqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        Usuario usuario = new Usuario();
                        TipoUsuario tipoUsuario = new TipoUsuario();

                        tipoUsuario.Id = (int)dr["id_tipo_usuario"];
                        tipoUsuario.Tipo = (string)dr["tipo"];

                        usuario.Id = (int)dr["id"];
                        usuario.Nome = (string)dr["nome"];
                        usuario.Email = (string)dr["email"];
                        usuario.Senha = (string)dr["senha"];
                        usuario.NomeFoto = dr["foto"] != DBNull.Value ? (string?)dr["foto"] : null;
                        usuario.Ativo = (bool)dr["ativo"];
                        usuario.Tipo = tipoUsuario;

                        usuarios.Add(usuario);
                    }
                }
            }

            return usuarios;
        }

        public List<Usuario> GetUsuariosContaPessoal()
        {
            List<Usuario> usuarios = new List<Usuario>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT U.id, nome, email, senha, foto, ativo, id_tipo_usuario, tipo FROM Usuario AS U INNER JOIN TipoUsuario AS TP ON U.id_tipo_usuario = TP.id WHERE U.id_tipo_usuario = 2;";

                    SqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        Usuario usuario = new Usuario();
                        TipoUsuario tipoUsuario = new TipoUsuario();

                        tipoUsuario.Id = (int)dr["id_tipo_usuario"];
                        tipoUsuario.Tipo = (string)dr["tipo"];

                        usuario.Id = (int)dr["id"];
                        usuario.Nome = (string)dr["nome"];
                        usuario.Email = (string)dr["email"];
                        usuario.Senha = (string)dr["senha"];
                        usuario.NomeFoto = dr["foto"] != DBNull.Value ? (string?)dr["foto"] : null;
                        usuario.Ativo = (bool)dr["ativo"];
                        usuario.Tipo = tipoUsuario;

                        usuarios.Add(usuario);
                    }
                }
            }

            return usuarios;
        }

        public Usuario GetUsuario(int id)
        {
            Usuario usuario = new Usuario();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT U.id, nome, email, senha, foto, ativo, id_tipo_usuario, tipo FROM Usuario AS U INNER JOIN TipoUsuario AS TP ON U.id_tipo_usuario = TP.id WHERE U.id = @id;";
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = id;

                    SqlDataReader dr = command.ExecuteReader();

                    if (dr.Read())
                    {
                        TipoUsuario tipoUsuario = new TipoUsuario();

                        tipoUsuario.Id = (int)dr["id_tipo_usuario"];
                        tipoUsuario.Tipo = (string)dr["tipo"];

                        usuario.Id = (int)dr["id"];
                        usuario.Nome = (string)dr["nome"];
                        usuario.Email = (string)dr["email"];
                        usuario.Senha = (string)dr["senha"];
                        usuario.NomeFoto = dr["foto"] != DBNull.Value ? (string?)dr["foto"] : null;
                        usuario.Ativo = (bool)dr["ativo"];
                        usuario.Tipo = tipoUsuario;
                    }
                }
            }

            return usuario;
        }

        public Usuario GetUsuario(string email, string senha)
        {
            Usuario usuario = new Usuario();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT U.id, nome, email, senha, foto, ativo, id_tipo_usuario, tipo FROM Usuario AS U INNER JOIN TipoUsuario AS TP ON U.id_tipo_usuario = TP.id WHERE email = @email AND senha = @senha AND ativo = 1;";
                    command.Parameters.Add(new SqlParameter("@email", System.Data.SqlDbType.VarChar)).Value = email;
                    command.Parameters.Add(new SqlParameter("@senha", System.Data.SqlDbType.VarChar)).Value = senha;

                    SqlDataReader dr = command.ExecuteReader();

                    if (dr.Read())
                    {
                        TipoUsuario tipoUsuario = new TipoUsuario();

                        tipoUsuario.Id = (int)dr["id_tipo_usuario"];
                        tipoUsuario.Tipo = (string)dr["tipo"];

                        usuario.Id = (int)dr["id"];
                        usuario.Nome = (string)dr["nome"];
                        usuario.Email = (string)dr["email"];
                        usuario.Senha = (string)dr["senha"];
                        usuario.NomeFoto = dr.IsDBNull(dr.GetOrdinal("foto")) ? null : (string)dr["foto"];
                        usuario.Ativo = (bool)dr["ativo"];
                        usuario.Tipo = tipoUsuario;
                    }
                }
            }

            return usuario;
        }

        public void AdicionarContaPessoal(Usuario usuario)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Usuario(id_tipo_usuario, nome, email, senha, foto) VALUES (@id_tipo_usuario, @nome, @email, @senha, @foto); SELECT CONVERT(INT, @@IDENTITY) AS id;";

                    command.Parameters.Add(new SqlParameter("@id_tipo_usuario", System.Data.SqlDbType.Int)).Value = usuario.Tipo.Id;
                    command.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = usuario.Nome;
                    command.Parameters.Add(new SqlParameter("@email", System.Data.SqlDbType.VarChar)).Value = usuario.Email;
                    command.Parameters.Add(new SqlParameter("@senha", System.Data.SqlDbType.VarChar)).Value = usuario.Senha;
                    command.Parameters.Add(new SqlParameter("@foto", System.Data.SqlDbType.VarChar)).Value = !string.IsNullOrEmpty(usuario.NomeFoto) ? usuario.NomeFoto : fotoPadraoUsuario;

                    usuario.Id = (int)command.ExecuteScalar();
                }
            }
        }

        public void AdicionarFarmacia(Farmacia farmacia)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Usuario(id_tipo_usuario, nome, email, senha, foto) VALUES (@id_tipo_usuario, @nome, @email, @senha, @foto); SELECT CONVERT(INT, @@IDENTITY) AS id;";

                    command.Parameters.Add(new SqlParameter("@id_tipo_usuario", System.Data.SqlDbType.Int)).Value = farmacia.Tipo.Id;
                    command.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = farmacia.Nome;
                    command.Parameters.Add(new SqlParameter("@email", System.Data.SqlDbType.VarChar)).Value = farmacia.Email;
                    command.Parameters.Add(new SqlParameter("@senha", System.Data.SqlDbType.VarChar)).Value = farmacia.Senha;
                    command.Parameters.Add(new SqlParameter("@foto", System.Data.SqlDbType.VarChar)).Value = !string.IsNullOrEmpty(farmacia.NomeFoto) ? farmacia.NomeFoto : fotoPadraoUsuario;

                    farmacia.Id = (int)command.ExecuteScalar();

                    command.CommandText = "INSERT INTO Farmacia(id_usuario, cnpj) VALUES (@id_usuario, @cnpj);";

                    command.Parameters.Add(new SqlParameter("@id_usuario", System.Data.SqlDbType.VarChar)).Value = farmacia.Id;
                    command.Parameters.Add(new SqlParameter("@cnpj", System.Data.SqlDbType.VarChar)).Value = farmacia.CNPJ;

                    command.ExecuteScalar();
                }
            }
        }

        public void AlterarUsuario(Usuario usuario)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE Usuario SET nome = @nome, foto = @foto WHERE id = @id;";
                    command.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = usuario.Nome;
                    
                    if (!string.IsNullOrEmpty(usuario.NomeFoto))
                        command.Parameters.Add(new SqlParameter("@foto", System.Data.SqlDbType.VarChar)).Value = usuario.NomeFoto;
                    
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = usuario.Id;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void AtivarUsuario(int id)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE Usuario SET ativo = 1 WHERE id = @id;";
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = id;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void DesativarUsuario(int id)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE Usuario SET ativo = 0 WHERE id = @id;";
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = id;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void LoginUsuario(int id)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE Usuario SET logado = 1 WHERE id = @id;";
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = id;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void LogoutUsuario(int id)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE Usuario SET logado = 0 WHERE id = @id;";
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = id;

                    command.ExecuteNonQuery();
                }
            }
        }

        public static void LogoutUsuarios()
        {
            string connectionString = Appsettings.GetConnectionString();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE Usuario SET logado = 0;";

                    command.ExecuteNonQuery();
                }
            }
        }

        public static bool IsLogado(int id)
        {
            string connectionString = Appsettings.GetConnectionString();
            bool isLogado = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT logado FROM Usuario WHERE id = @id AND ativo = 1;";
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = id;

                    SqlDataReader dr = command.ExecuteReader();

                    if (dr.Read())
                    {
                        isLogado = (bool)dr["logado"];
                    }
                }
            }

            return isLogado;
        }

        public bool CheckEmail(string email)
        {
            bool emailOk = false;

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT email FROM Usuario WHERE email = @email;";
                    command.Parameters.Add(new SqlParameter("@email", System.Data.SqlDbType.VarChar)).Value = email;

                    SqlDataReader dr = command.ExecuteReader();

                    emailOk = !dr.Read();
                }
            }

            return emailOk;
        }

        public bool CheckSenha(int idUsuario, string senha)
        {
            bool senhaOk = false;

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT 1 FROM Usuario WHERE id = @id AND senha = @senha;";
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = idUsuario;
                    command.Parameters.Add(new SqlParameter("@senha", System.Data.SqlDbType.VarChar)).Value = senha;

                    SqlDataReader dr = command.ExecuteReader();

                    senhaOk = dr.HasRows;
                }
            }

            return senhaOk;
        }

        public void AlterarSenha(int idUsuario, string novaSenha)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE Usuario SET senha = @senha WHERE id = @id;";
                    command.Parameters.Add(new SqlParameter("@senha", System.Data.SqlDbType.VarChar)).Value = novaSenha;
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = idUsuario;

                    command.ExecuteNonQuery();
                }
            }
        }

        public string GetNomeFoto(int idUsuario)
        {
            string nomeFoto = fotoPadraoUsuario;

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT foto FROM Usuario WHERE id = @id;";
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = idUsuario;

                    SqlDataReader dr = command.ExecuteReader();

                    if (dr.Read())
                        nomeFoto = (string)dr["foto"];
                }
            }

            return nomeFoto;
        }

        #endregion [ Usuário ]

        #region [ Endereço ]

        public List<Endereco> GetEnderecos(int idUsuario)
        {
            List<Endereco> enderecos = new List<Endereco>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT id, nome_local, id_logradouro, cep, numero, complemento, latitude, longitude FROM Endereco WHERE id_usuario = @id_usuario;";
                    command.Parameters.Add(new SqlParameter("@id_usuario", System.Data.SqlDbType.Int)).Value = idUsuario;

                    SqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        Endereco endereco = new Endereco();

                        endereco.Id = (int)dr["id"];
                        endereco.NomeLocal = (string)dr["nome_local"];
                        endereco.Logradouro = GetLogradouro((int)dr["id_logradouro"]);
                        endereco.CEP = (string)dr["cep"];
                        endereco.Numero = (string)dr["numero"];
                        endereco.Complemento = dr["complemento"] != DBNull.Value ? (string)dr["complemento"] : null;
                        endereco.Latitude = dr["latitude"] != DBNull.Value ? (string)dr["latitude"] : null;
                        endereco.Longitude = dr["longitude"] != DBNull.Value ? (string)dr["longitude"] : null;

                        enderecos.Add(endereco);
                    }
                }
            }

            return enderecos;
        }

        public Endereco GetEndereco(int idEndereco)
        {
            Endereco endereco = new Endereco();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT id, nome_local, id_logradouro, cep, numero, complemento, latitude, longitude FROM Endereco WHERE id = @id;";
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = idEndereco;

                    SqlDataReader dr = command.ExecuteReader();

                    if (dr.Read())
                    {
                        endereco.Id = (int)dr["id"];
                        endereco.NomeLocal = (string)dr["nome_local"];
                        endereco.Logradouro = GetLogradouro((int)dr["id_logradouro"]);
                        endereco.CEP = (string)dr["cep"];
                        endereco.Numero = (string)dr["numero"];
                        endereco.Complemento = dr["complemento"] != DBNull.Value ? (string)dr["complemento"] : null;
                        endereco.Latitude = dr["latitude"] != DBNull.Value ? (string)dr["latitude"] : null;
                        endereco.Longitude = dr["longitude"] != DBNull.Value ? (string)dr["longitude"] : null;
                    }
                }
            }

            return endereco;
        }

        public void NovoEndereco(int idUsuario, Endereco endereco)
        {
            VerificarUF(endereco.Logradouro.Bairro.Cidade.UF);
            VerificarCidade(endereco.Logradouro.Bairro.Cidade);
            VerificarBairro(endereco.Logradouro.Bairro);
            VerificarLogradouro(endereco.Logradouro);

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Endereco (nome_local, id_usuario, id_logradouro, cep, numero, complemento, latitude, longitude) VALUES (@nome_local, @id_usuario, @id_logradouro, @cep, @numero, @complemento, @latitude, @longitude); SELECT CONVERT(INT, @@IDENTITY) AS id;";

                    command.Parameters.Add(new SqlParameter("@nome_local", System.Data.SqlDbType.VarChar)).Value = endereco.NomeLocal;
                    command.Parameters.Add(new SqlParameter("@id_usuario", System.Data.SqlDbType.Int)).Value = idUsuario;
                    command.Parameters.Add(new SqlParameter("@id_logradouro", System.Data.SqlDbType.Int)).Value = endereco.Logradouro.Id;
                    command.Parameters.Add(new SqlParameter("@cep", System.Data.SqlDbType.VarChar)).Value = endereco.CEP;
                    command.Parameters.Add(new SqlParameter("@numero", System.Data.SqlDbType.Int)).Value = endereco.Numero;
                    command.Parameters.Add(new SqlParameter("@complemento", System.Data.SqlDbType.VarChar)).Value = !string.IsNullOrEmpty(endereco.Complemento) ? endereco.Complemento : DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@latitude", System.Data.SqlDbType.VarChar)).Value = !string.IsNullOrEmpty(endereco.Latitude) ? endereco.Latitude : DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@longitude", System.Data.SqlDbType.VarChar)).Value = !string.IsNullOrEmpty(endereco.Longitude) ? endereco.Longitude : DBNull.Value;

                    endereco.Id = (int)command.ExecuteScalar();
                }
            }
        }

        public void AlterarEndereco(Endereco endereco)
        {
            VerificarUF(endereco.Logradouro.Bairro.Cidade.UF);
            VerificarCidade(endereco.Logradouro.Bairro.Cidade);
            VerificarBairro(endereco.Logradouro.Bairro);
            VerificarLogradouro(endereco.Logradouro);

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE Endereco SET nome_local = @nome_local, id_logradouro = @id_logradouro, cep = @cep, numero = @numero, complemento = @complemento, latitude = @latitude, @longitude = longitude WHERE id = @id;";
                    command.Parameters.Add(new SqlParameter("@nome_local", System.Data.SqlDbType.VarChar)).Value = endereco.NomeLocal;
                    command.Parameters.Add(new SqlParameter("@id_logradouro", System.Data.SqlDbType.Int)).Value = endereco.Logradouro.Id;
                    command.Parameters.Add(new SqlParameter("@cep", System.Data.SqlDbType.VarChar)).Value = endereco.CEP;
                    command.Parameters.Add(new SqlParameter("@numero", System.Data.SqlDbType.VarChar)).Value = endereco.Numero;
                    command.Parameters.Add(new SqlParameter("@complemento", System.Data.SqlDbType.VarChar)).Value = !string.IsNullOrEmpty(endereco.Complemento) ? endereco.Complemento : DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@latitude", System.Data.SqlDbType.VarChar)).Value = !string.IsNullOrEmpty(endereco.Latitude) ? endereco.Latitude : DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@longitude", System.Data.SqlDbType.VarChar)).Value = !string.IsNullOrEmpty(endereco.Longitude) ? endereco.Longitude : DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = endereco.Id;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void ExcluirEndereco(int idEndereco)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM Endereco WHERE id = @id;";
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = idEndereco;

                    command.ExecuteNonQuery();
                }
            }
        }

        public static List<Endereco> GetLocaisEnderecos(int id_usuario)
        {
            string connectionString = Appsettings.GetConnectionString();
            List<Endereco> locais = new List<Endereco>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT id, nome_local, latitude, longitude FROM Endereco WHERE id_usuario = @id_usuario;";
                    command.Parameters.Add(new SqlParameter("@id_usuario", System.Data.SqlDbType.Int)).Value = id_usuario;

                    SqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        Endereco endereco = new Endereco();

                        endereco.Id = (int)dr["id"];
                        endereco.NomeLocal = (string)dr["nome_local"];
                        endereco.Latitude = (string)dr["latitude"];
                        endereco.Longitude = (string)dr["longitude"];

                        locais.Add(endereco);
                    }
                }
            }

            return locais;
        }

        #endregion [ Endereço ]

        #region [ Logradouro ]

        public Logradouro GetLogradouro(int idLogradouro)
        {
            Logradouro logradouro = new Logradouro();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT id, id_bairro, nome FROM Logradouro WHERE id = @id;";
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = idLogradouro;

                    SqlDataReader dr = command.ExecuteReader();

                    if (dr.Read())
                    {
                        logradouro.Id = (int)dr["id"];
                        logradouro.Bairro = GetBairro((int)dr["id_bairro"]);
                        logradouro.Nome = (string)dr["nome"];
                    }
                }
            }

            return logradouro;
        }

        public Logradouro GetLogradouro(string nome)
        {
            Logradouro logradouro = new Logradouro();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT id, id_bairro, nome FROM Logradouro WHERE nome = @nome;";
                    command.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = nome;

                    SqlDataReader dr = command.ExecuteReader();

                    if (dr.Read())
                    {
                        logradouro.Id = (int)dr["id"];
                        logradouro.Bairro = GetBairro((int)dr["id_bairro"]);
                        logradouro.Nome = (string)dr["nome"];
                    }
                }
            }

            return logradouro;
        }

        public void NovoLogradouro(Logradouro logradouro)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Logradouro (id_bairro, nome) VALUES (@id_bairro, @nome); SELECT CONVERT(INT, @@IDENTITY) AS id;";

                    command.Parameters.Add(new SqlParameter("@id_bairro", System.Data.SqlDbType.Int)).Value = logradouro.Bairro.Id;
                    command.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = logradouro.Nome;

                    logradouro.Id = (int)command.ExecuteScalar();
                }
            }
        }

        public void VerificarLogradouro(Logradouro logradouro)
        {
            Logradouro _logradouro = GetLogradouro(logradouro.Nome);

            if (_logradouro.Id != 0)
                logradouro.Id = _logradouro.Id;
            else
                NovoLogradouro(logradouro);
        }

        #endregion [ Logradouro ]

        #region [ Bairro ]

        public Bairro GetBairro(int idBairro)
        {
            Bairro bairro = new Bairro();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT id, id_cidade, nome FROM Bairro WHERE id = @id;";
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = idBairro;

                    SqlDataReader dr = command.ExecuteReader();

                    if (dr.Read())
                    {
                        bairro.Id = (int)dr["id"];
                        bairro.Cidade = GetCidade((int)dr["id_cidade"]);
                        bairro.Nome = (string)dr["nome"];
                    }
                }
            }

            return bairro;
        }

        public Bairro GetBairro(string nome)
        {
            Bairro bairro = new Bairro();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT id, id_cidade, nome FROM Bairro WHERE nome = @nome;";
                    command.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = nome;

                    SqlDataReader dr = command.ExecuteReader();

                    if (dr.Read())
                    {
                        bairro.Id = (int)dr["id"];
                        bairro.Cidade = GetCidade((int)dr["id_cidade"]);
                        bairro.Nome = (string)dr["nome"];
                    }
                }
            }

            return bairro;
        }

        public void NovoBairro(Bairro bairro)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Bairro (id_cidade, nome) VALUES (@id_cidade, @nome); SELECT CONVERT(INT, @@IDENTITY) AS id;";

                    command.Parameters.Add(new SqlParameter("@id_cidade", System.Data.SqlDbType.Int)).Value = bairro.Cidade.Id;
                    command.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = bairro.Nome;

                    bairro.Id = (int)command.ExecuteScalar();
                }
            }
        }

        public void VerificarBairro(Bairro bairro)
        {
            Bairro _bairro = GetBairro(bairro.Nome);

            if (_bairro.Id != 0)
                bairro.Id = _bairro.Id;
            else
                NovoBairro(bairro);
        }

        #endregion [ Bairro ]

        #region [ Cidade ]

        public Cidade GetCidade(int idCidade)
        {
            Cidade cidade = new Cidade();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT id, id_uf, nome FROM Cidade WHERE id = @id;";
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = idCidade;

                    SqlDataReader dr = command.ExecuteReader();

                    if (dr.Read())
                    {
                        cidade.Id = (int)dr["id"];
                        cidade.UF = GetUF((int)dr["id_uf"]);
                        cidade.Nome = (string)dr["nome"];
                    }
                }
            }

            return cidade;
        }

        public Cidade GetCidade(string nome)
        {
            Cidade cidade = new Cidade();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT id, id_uf, nome FROM Cidade WHERE nome = @nome;";
                    command.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = nome;

                    SqlDataReader dr = command.ExecuteReader();

                    if (dr.Read())
                    {
                        cidade.Id = (int)dr["id"];
                        cidade.UF = GetUF((int)dr["id_uf"]);
                        cidade.Nome = (string)dr["nome"];
                    }
                }
            }

            return cidade;
        }

        public void NovaCidade(Cidade cidade)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Cidade (id_uf, nome) VALUES (@id_uf, @nome); SELECT CONVERT(INT, @@IDENTITY) AS id;";

                    command.Parameters.Add(new SqlParameter("@id_uf", System.Data.SqlDbType.Int)).Value = cidade.UF.Id;
                    command.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = cidade.Nome;

                    cidade.Id = (int)command.ExecuteScalar();
                }
            }
        }

        public void VerificarCidade(Cidade cidade)
        {
            Cidade _cidade = GetCidade(cidade.Nome);

            if (_cidade.Id != 0)
                cidade.Id = _cidade.Id;
            else
                NovaCidade(cidade);
        }

        #endregion [ Cidade ]

        #region [ UF ]

        public UF GetUF(int idUF)
        {
            UF uf = new UF();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT id, sigla FROM UF WHERE id = @id;";
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = idUF;

                    SqlDataReader dr = command.ExecuteReader();

                    if (dr.Read())
                    {
                        uf.Id = (int)dr["id"];
                        uf.Sigla = (string)dr["sigla"];
                    }
                }
            }

            return uf;
        }

        public UF GetUF(string sigla)
        {
            UF uf = new UF();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT id, sigla FROM UF WHERE sigla = @sigla;";
                    command.Parameters.Add(new SqlParameter("@sigla", System.Data.SqlDbType.VarChar)).Value = sigla;

                    SqlDataReader dr = command.ExecuteReader();

                    if (dr.Read())
                    {
                        uf.Id = (int)dr["id"];
                        uf.Sigla = (string)dr["sigla"];
                    }
                }
            }

            return uf;
        }

        public void NovoUF(UF uf)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO UF (sigla) VALUES (@sigla); SELECT CONVERT(INT, @@IDENTITY) AS id;";

                    command.Parameters.Add(new SqlParameter("@sigla", System.Data.SqlDbType.VarChar)).Value = uf.Sigla;

                    uf.Id = (int)command.ExecuteScalar();
                }
            }
        }

        public void VerificarUF(UF uf)
        {
            UF _uf = GetUF(uf.Sigla);

            if (_uf.Id != 0)
                uf.Id = _uf.Id;
            else
                NovoUF(uf);
        }

        #endregion [ UF ]
    }
}
