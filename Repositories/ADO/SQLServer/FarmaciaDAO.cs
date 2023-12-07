using LocFarma.Models;
using Microsoft.Data.SqlClient;

namespace LocFarma.Repositories.ADO.SQLServer
{
    public class FarmaciaDAO
    {
        #region [ Conexão ]

        private readonly string connectionString;
        private const string fotoPadraoUsuario = "perfil.svg";
        private const string fotoPadraoProduto = "produto_padrao.svg";

        public FarmaciaDAO(string connectionString)
        {
            this.connectionString = connectionString;
        }

        #endregion [ Conexão ]

        #region [ Farmácia ]

        public List<Farmacia> GetFarmacias()
        {
            List<Farmacia> farmacias = new List<Farmacia>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT U.id, nome, email, senha, foto, ativo, id_tipo_usuario, tipo, cnpj FROM Usuario AS U INNER JOIN Farmacia AS F ON U.id = F.id_usuario INNER JOIN TipoUsuario AS TP ON U.id_tipo_usuario = TP.id;";

                    SqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        Farmacia farmacia = new Farmacia();
                        TipoUsuario tipoUsuario = new TipoUsuario();

                        tipoUsuario.Id = (int)dr["id_tipo_usuario"];
                        tipoUsuario.Tipo = (string)dr["tipo"];

                        farmacia.Id = (int)dr["id"];
                        farmacia.Nome = (string)dr["nome"];
                        farmacia.Email = (string)dr["email"];
                        farmacia.Senha = (string)dr["senha"];
                        farmacia.NomeFoto = dr["foto"] != DBNull.Value ? (string?)dr["foto"] : null;
                        farmacia.Ativo = (bool)dr["ativo"];
                        farmacia.CNPJ = (string)dr["cnpj"];
                        farmacia.Tipo = tipoUsuario;
                        farmacia.Produtos = GetProdutos(farmacia.Id);

                        farmacias.Add(farmacia);
                    }
                }
            }

            return farmacias;
        }

        public Farmacia GetFarmacia(int idFarmacia)
        {
            Farmacia farmacia = new Farmacia();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT U.id, nome, email, senha, foto, ativo, id_tipo_usuario, tipo, cnpj FROM Usuario AS U INNER JOIN Farmacia AS F ON U.id = F.id_usuario INNER JOIN TipoUsuario AS TP ON U.id_tipo_usuario = TP.id WHERE U.id = @id;";
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = idFarmacia;

                    SqlDataReader dr = command.ExecuteReader();

                    if (dr.Read())
                    {
                        TipoUsuario tipoUsuario = new TipoUsuario();

                        tipoUsuario.Id = (int)dr["id_tipo_usuario"];
                        tipoUsuario.Tipo = (string)dr["tipo"];

                        farmacia.Id = (int)dr["id"];
                        farmacia.Nome = (string)dr["nome"];
                        farmacia.Email = (string)dr["email"];
                        farmacia.Senha = (string)dr["senha"];
                        farmacia.NomeFoto = dr["foto"] != DBNull.Value ? (string?)dr["foto"] : null;
                        farmacia.Ativo = (bool)dr["ativo"];
                        farmacia.CNPJ = (string)dr["cnpj"];
                        farmacia.Tipo = tipoUsuario;
                        farmacia.Produtos = GetProdutos(farmacia.Id);
                    }
                }
            }

            return farmacia;
        }

        public void AlterarFarmacia(Farmacia farmacia)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE Usuario SET nome = @nome, foto = @foto WHERE id = @id;";
                    command.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = farmacia.Nome;

                    if (!string.IsNullOrEmpty(farmacia.NomeFoto))
                        command.Parameters.Add(new SqlParameter("@foto", System.Data.SqlDbType.VarChar)).Value = farmacia.NomeFoto;

                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = farmacia.Id;

                    command.ExecuteNonQuery();

                    command.CommandText = "UPDATE Farmacia SET cnpj = @cnpj WHERE id_usuario = @id_usuario;";
                    command.Parameters.Add(new SqlParameter("@cnpj", System.Data.SqlDbType.VarChar)).Value = farmacia.CNPJ;
                    command.Parameters.Add(new SqlParameter("@id_usuario", System.Data.SqlDbType.Int)).Value = farmacia.Id;

                    command.ExecuteNonQuery();
                }
            }
        }

        #endregion [ Farmácia ]

        #region [ Produto ]

        public List<Produto> GetProdutos(int idFarmacia)
        {
            List<Produto> produtos = new List<Produto>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT id, nome, preco, quantidade, descricao, foto FROM Produto WHERE id_usuario = @id_usuario;";
                    command.Parameters.Add(new SqlParameter("@id_usuario", System.Data.SqlDbType.Int)).Value = idFarmacia;

                    SqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        Produto produto = new Produto();

                        produto.Id = (int)dr["id"];
                        produto.Nome = (string)dr["nome"];
                        produto.Preco = (decimal)dr["preco"];
                        produto.Quantidade = (int)dr["quantidade"];
                        produto.Descricao = dr["descricao"] != DBNull.Value ? (string)dr["descricao"] : null;
                        produto.NomeFoto = dr["foto"] != DBNull.Value ? (string?)dr["foto"] : null;

                        produtos.Add(produto);
                    }
                }
            }

            return produtos;
        }

        public List<Produto> GetProdutosDestaque()
        {
            List<Produto> produtos = new List<Produto>();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT id, nome, preco, quantidade, descricao, foto FROM Produto WHERE destaque = 1;";

                    SqlDataReader dr = command.ExecuteReader();

                    while (dr.Read())
                    {
                        Produto produto = new Produto();

                        produto.Id = (int)dr["id"];
                        produto.Nome = (string)dr["nome"];
                        produto.Preco = (decimal)dr["preco"];
                        produto.Quantidade = (int)dr["quantidade"];
                        produto.Descricao = dr["descricao"] != DBNull.Value ? (string)dr["descricao"] : null;
                        produto.NomeFoto = dr["foto"] != DBNull.Value ? (string?)dr["foto"] : null;

                        produtos.Add(produto);
                    }
                }
            }

            return produtos;
        }

        public Produto GetProduto(int idProduto)
        {
            Produto produto = new Produto();

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT id, nome, preco, quantidade, descricao, foto FROM Produto WHERE id = @id_produto;";
                    command.Parameters.Add(new SqlParameter("@id_produto", System.Data.SqlDbType.Int)).Value = idProduto;

                    SqlDataReader dr = command.ExecuteReader();

                    if (dr.Read())
                    {
                        produto.Id = (int)dr["id"];
                        produto.Nome = (string)dr["nome"];
                        produto.Preco = (decimal)dr["preco"];
                        produto.Quantidade = (int)dr["quantidade"];
                        produto.Descricao = dr["descricao"] != DBNull.Value ? (string)dr["descricao"] : null;
                        produto.NomeFoto = dr["foto"] != DBNull.Value ? (string?)dr["foto"] : null;
                    }
                }
            }

            return produto;
        }

        public void NovoProduto(int idFarmacia, Produto produto)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "INSERT INTO Produto (id_usuario, nome, preco, quantidade, descricao, foto) VALUES (@id_usuario, @nome, @preco, @quantidade, @descricao, @foto); SELECT CONVERT(INT, @@IDENTITY) AS id;";

                    command.Parameters.Add(new SqlParameter("@id_usuario", System.Data.SqlDbType.Int)).Value = idFarmacia;
                    command.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = produto.Nome;
                    command.Parameters.Add(new SqlParameter("@preco", System.Data.SqlDbType.Decimal)).Value = produto.Preco;
                    command.Parameters.Add(new SqlParameter("@quantidade", System.Data.SqlDbType.Int)).Value = produto.Quantidade;
                    command.Parameters.Add(new SqlParameter("@descricao", System.Data.SqlDbType.VarChar)).Value = !string.IsNullOrEmpty(produto.Descricao) ? produto.Descricao : DBNull.Value;
                    command.Parameters.Add(new SqlParameter("@foto", System.Data.SqlDbType.VarChar)).Value = !string.IsNullOrEmpty(produto.NomeFoto) ? produto.NomeFoto : fotoPadraoProduto;

                    produto.Id = (int)command.ExecuteScalar();
                }
            }
        }

        public void AlterarProduto(Produto produto)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "UPDATE Produto SET nome = @nome, preco = @preco, quantidade = @quantidade, descricao = @descricao, foto = @foto WHERE id = @id;";
                    command.Parameters.Add(new SqlParameter("@nome", System.Data.SqlDbType.VarChar)).Value = produto.Nome;
                    command.Parameters.Add(new SqlParameter("@preco", System.Data.SqlDbType.Decimal)).Value = produto.Preco;
                    command.Parameters.Add(new SqlParameter("@quantidade", System.Data.SqlDbType.Int)).Value = produto.Quantidade;
                    command.Parameters.Add(new SqlParameter("@descricao", System.Data.SqlDbType.Text)).Value = !string.IsNullOrEmpty(produto.Descricao) ? produto.Descricao : DBNull.Value;

                    if (!string.IsNullOrEmpty(produto.NomeFoto))
                        command.Parameters.Add(new SqlParameter("@foto", System.Data.SqlDbType.VarChar)).Value = produto.NomeFoto;

                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = produto.Id;

                    command.ExecuteNonQuery();
                }
            }
        }

        public void ExcluirProduto(int idProduto)
        {
            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "DELETE FROM Produto WHERE id = @id;";
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = idProduto;

                    command.ExecuteNonQuery();
                }
            }
        }

        public string GetNomeFotoProduto(int idProduto)
        {
            string nomeFoto = fotoPadraoUsuario;

            using (SqlConnection connection = new SqlConnection(this.connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = connection;
                    command.CommandText = "SELECT foto FROM Produto WHERE id = @id;";
                    command.Parameters.Add(new SqlParameter("@id", System.Data.SqlDbType.Int)).Value = idProduto;

                    SqlDataReader dr = command.ExecuteReader();

                    if (dr.Read())
                        nomeFoto = (string)dr["foto"];
                }
            }

            return nomeFoto;
        }

        #endregion [ Produto ]
    }
}
