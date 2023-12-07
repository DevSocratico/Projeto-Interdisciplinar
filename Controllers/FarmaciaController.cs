using LocFarma.Models;
using LocFarma.Repositories.ADO.SQLServer;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;

namespace LocFarma.Controllers
{
    public class FarmaciaController : Controller
    {
        #region [ Repositório ]

        private readonly FarmaciaDAO repositorio;
        private readonly UsuarioDAO usuarioDAO;
        private readonly IWebHostEnvironment webHostEnvironment;

        public FarmaciaController(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            this.repositorio = new FarmaciaDAO(configuration.GetConnectionString(Configurations.Appsettings.GetKeyConnectionString()));
            this.usuarioDAO = new UsuarioDAO(configuration.GetConnectionString(Configurations.Appsettings.GetKeyConnectionString()));
            this.webHostEnvironment = webHostEnvironment;
        }

        #endregion [ Repositório ]

        #region [ Listagem Farmácia ]

        public IActionResult Farmacias()
        {
            int idTipoUsuario = HttpContext.Session.GetInt32("IdTipoUsuario") ?? 0;

            ViewBag.TipoUsuario = idTipoUsuario;

            List<Farmacia> farmacias = this.repositorio.GetFarmacias();

            return View(farmacias);
        }

        #endregion [ Listagem Farmácia ]

        #region [ Novo Produto ]

        public IActionResult CadastrarProduto()
        {
            int idTipoUsuario = HttpContext.Session.GetInt32("IdTipoUsuario") ?? 0;

            ViewBag.TipoUsuario = idTipoUsuario;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CadastrarProduto(Produto produto)
        {
            try
            {
                if (produto.Foto != null)
                {
                    produto.NomeFoto = $"{Guid.NewGuid()}-{produto.Foto.FileName}";
                    string pasta = $"imgs/produtos/{produto.NomeFoto}";
                    string pastaServidor = Path.Combine(this.webHostEnvironment.WebRootPath, pasta);
                    produto.Foto.CopyTo(new FileStream(pastaServidor, FileMode.Create));
                }

                int idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;
                this.repositorio.NovoProduto(idUsuario, produto);

                return RedirectToAction(nameof(Produtos));
            }
            catch
            {
                return View();
            }
        }

        #endregion [ Novo Produto ]

        #region [ Editar Farmácia ]

        [HttpGet]
        public IActionResult EditarFarmacia(int id)
        {
            int idTipoUsuario = HttpContext.Session.GetInt32("IdTipoUsuario") ?? 0;

            ViewBag.TipoUsuario = idTipoUsuario;

            Usuario usuario = this.repositorio.GetFarmacia(id);

            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditarFarmacia(Farmacia farmacia)
        {
            try
            {
                if (farmacia.Foto != null)
                {
                    farmacia.NomeFoto = $"{Guid.NewGuid()}-{farmacia.Foto.FileName}";
                    string pasta = $"imgs/perfil/{farmacia.NomeFoto}";
                    string pastaServidor = Path.Combine(this.webHostEnvironment.WebRootPath, pasta);
                    farmacia.Foto.CopyTo(new FileStream(pastaServidor, FileMode.Create));
                }
                else
                {
                    farmacia.NomeFoto = this.usuarioDAO.GetNomeFoto(farmacia.Id);
                }

                this.repositorio.AlterarFarmacia(farmacia);

                int idTipoUsuario = HttpContext.Session.GetInt32("IdTipoUsuario") ?? 0;

                if (idTipoUsuario == 1)
                    return RedirectToAction("Perfil", "Usuario");

                return RedirectToAction(nameof(Perfil));
            }
            catch
            {
                return View();
            }
        }

        #endregion [ Editar Farmácia ]

        #region [ Perfil Farmácia ]

        [HttpGet]
        public IActionResult Perfil()
        {
            int idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;

            Usuario usuario = this.repositorio.GetFarmacia(idUsuario);

            HttpContext.Session.SetInt32("IdTipoUsuario", usuario.Tipo.Id);

            ViewBag.TipoUsuario = usuario.Tipo.Id;

            if (usuario.Tipo.Id == 1)
                return RedirectToAction("Perfil", "Usuario");

            return View(usuario);
        }

        #endregion [ Perfil Farmácia ]

        #region [ Listagem Produtos ]

        public IActionResult Produtos()
        {
            int idTipoUsuario = HttpContext.Session.GetInt32("IdTipoUsuario") ?? 0;

            ViewBag.TipoUsuario = idTipoUsuario;

            int idUsuario = HttpContext.Session.GetInt32("IdUsuario") ?? 0;

            if (idTipoUsuario == 1)
                return RedirectToAction("Produtos", "Administrador");

            List<Produto> produtos = this.repositorio.GetProdutos(idUsuario);

            return View(produtos);
        }

        #endregion [ Listagem Produtos ]

        #region [ Detalhes Produto ]
        
        public IActionResult DetalhesProduto(int id)
        {
            int idTipoUsuario = HttpContext.Session.GetInt32("IdTipoUsuario") ?? 0;

            ViewBag.TipoUsuario = idTipoUsuario;

            Produto produto = this.repositorio.GetProduto(id);

            return View(produto);
        }

        #endregion [ Detalhes Produto ]

        #region [ Editar Produto ]

        [HttpGet]
        public IActionResult EditarProduto(int id)
        {
            int idTipoUsuario = HttpContext.Session.GetInt32("IdTipoUsuario") ?? 0;

            ViewBag.TipoUsuario = idTipoUsuario;

            Produto produto = this.repositorio.GetProduto(id);

            return View(produto);
        }

        [HttpPost]
        public IActionResult EditarProduto(Produto produto)
        {
            try
            {
                if (produto.Foto != null)
                {
                    produto.NomeFoto = $"{Guid.NewGuid()}-{produto.Foto.FileName}";
                    string pasta = $"imgs/produtos/{produto.NomeFoto}";
                    string pastaServidor = Path.Combine(this.webHostEnvironment.WebRootPath, pasta);
                    produto.Foto.CopyTo(new FileStream(pastaServidor, FileMode.Create));
                }
                else
                {
                    produto.NomeFoto = this.repositorio.GetNomeFotoProduto(produto.Id);
                }

                this.repositorio.AlterarProduto(produto);

                int idTipoUsuario = HttpContext.Session.GetInt32("IdTipoUsuario") ?? 0;

                if (idTipoUsuario == 1)
                    return RedirectToAction("Produtos", "Administrador");

                return RedirectToAction(nameof(Produtos));
            }
            catch
            {
                return View();
            }
        }

        #endregion [ Editar Produto ]

        #region [ Excluir Produto ]

        public IActionResult ExcluirProduto(int id)
        {
            this.repositorio.ExcluirProduto(id);

            int idTipoUsuario = HttpContext.Session.GetInt32("IdTipoUsuario") ?? 0;

            if (idTipoUsuario == 1)
                return RedirectToAction(nameof(Produtos), "Administrador");

            return RedirectToAction(nameof(Produtos));
        }

        #endregion [ Excluir Produto ]
    }
}
