using LocFarma.Models;
using LocFarma.Repositories.ADO.SQLServer;
using Microsoft.AspNetCore.Mvc;

namespace LocFarma.Controllers
{
    public class ProdutoController : Controller
    {
        #region [ Repositório ]

        private readonly FarmaciaDAO repositorio;
        private readonly UsuarioDAO usuarioDAO;

        public ProdutoController(IConfiguration configuration)
        {
            this.repositorio = new FarmaciaDAO(configuration.GetConnectionString(Configurations.Appsettings.GetKeyConnectionString()));
            this.usuarioDAO = new UsuarioDAO(configuration.GetConnectionString(Configurations.Appsettings.GetKeyConnectionString()));
        }

        #endregion [ Repositório ]

        #region [ Produto ]

        [HttpGet]
        public IActionResult Produtos(string pesquisa)
        {
            List<Farmacia> farmacias = this.repositorio.GetFarmacias();
            //List<List<Endereco>> locaisFarmacias = new List<List<Endereco>>();

            //foreach (var farmacia in farmacias)
            //{
            //    List<Endereco> enderecos = this.usuarioDAO.GetEnderecos(farmacia.Id);
            //    locaisFarmacias.Add(enderecos);
            //}

            ViewData["Farmacias"] = farmacias;
            //ViewData["LocaisFarmacias"] = locaisFarmacias;

            if (!string.IsNullOrEmpty(pesquisa))
            {
                var farmaciasComProdutosFiltrados = farmacias
                    .Select(farmacia =>
                    {
                        var produtosFiltrados = farmacia.Produtos
                            .Where(produto => produto.Nome.Contains(pesquisa, StringComparison.OrdinalIgnoreCase))
                            .ToList();

                        // Cria uma cópia da farmácia atual, mas com a lista de produtos filtrados
                        return new Farmacia
                        {
                            Id = farmacia.Id,
                            Nome = farmacia.Nome,
                            Produtos = produtosFiltrados
                        };
                    })
                    .Where(farmacia => farmacia.Produtos.Any()) // Filtra apenas as farmácias que têm produtos filtrados
                    .ToList();

                return View(farmaciasComProdutosFiltrados);
            }

            return View(farmacias);
        }

        [HttpGet]
        public IActionResult Produto(int id)
        {
            Produto produto = this.repositorio.GetProduto(id);

            return View(produto);
        }

        #endregion [ Produto ]
    }
}
